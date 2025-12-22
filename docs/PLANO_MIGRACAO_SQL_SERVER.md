# ??? PLANO DE MIGRA��O - BASE SQL SERVER COM DADOS REAIS

## ?? **OBJETIVO**

Criar um banco de dados SQL Server com **~150 registros reais** extra�dos do backup do cliente, mantendo relacionamentos e integridade referencial para testes robustos via Swagger.

---

## ?? **SITUA��O ATUAL**

### ? **O que temos:**
- ? Backup do cliente: `C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak` (43.2 GB)
- ? C�digo legado VB.NET: `C:\temp\_ONS_PoC-PDPW_V2\legado`
- ? POC .NET 8 funcionando com **InMemory** (69 registros)
- ? Migrations EF Core criadas
- ? Docker Compose configurado

### ?? **Limita��es:**
- ? Backup completo requer 350 GB (n�o temos espa�o)
- ? InMemory perde dados ao reiniciar
- ? InMemory n�o simula comportamento real de SQL Server

---

## ?? **ESTRAT�GIA RECOMENDADA: EXTRA��O SELETIVA**

### **Abordagem em 3 Fases:**

```
FASE 1: Criar banco SQL Server vazio com schema
  ?
FASE 2: Extrair ~150 registros do backup (seletivo)
  ?
FASE 3: Popular banco SQL Server da POC
```

**Espa�o necess�rio:** ~10-15 GB (vi�vel!)

---

## ?? **FASE 1: CRIAR BANCO SQL SERVER VAZIO**

### **Op��o A: Usar docker-compose.full.yml (Recomendado)**

**Vantagens:**
- ? J� configurado
- ? Isolado em container
- ? F�cil de resetar
- ? Persist�ncia em volumes Docker

**Como fazer:**
```powershell
# 1. Parar InMemory
docker-compose down

# 2. Iniciar com SQL Server
docker-compose -f docker-compose.full.yml up -d

# 3. Aguardar SQL Server inicializar (~1 minuto)
timeout /t 60

# 4. Aplicar migrations
cd src/PDPW.API
dotnet ef database update

# 5. Verificar banco criado
docker exec pdpw-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong" -Q "SELECT name FROM sys.databases"
```

### **Op��o B: SQL Server Express Local**

**Vantagens:**
- ? Mais performance
- ? Ferramentas GUI (SSMS)
- ? Sem overhead de Docker

**Como fazer:**
```powershell
# 1. Atualizar appsettings.json
# UseInMemoryDatabase: false
# ConnectionString: Server=localhost\SQLEXPRESS;Database=PDPW_DB;Trusted_Connection=true;

# 2. Aplicar migrations
cd src/PDPW.API
dotnet ef database update

# 3. Verificar no SSMS
# Conectar em: localhost\SQLEXPRESS
# Banco: PDPW_DB
```

---

## ?? **FASE 2: EXTRAIR DADOS DO BACKUP**

### **Estrat�gia de Extra��o Seletiva**

#### **Tabelas Priorit�rias (150 registros):**

| Tabela | Qtd Registros | Crit�rio de Sele��o |
|--------|---------------|---------------------|
| **Empresa** | 30 | Top 30 por quantidade de usinas |
| **TipoUsina** | 10 | Todos os tipos (UHE, UTE, EOL, UFV, PCH, etc) |
| **Usina** | 50 | Top 50 por pot�ncia instalada |
| **SemanaPMO** | 26 | �ltimos 6 meses (26 semanas) |
| **EquipePDP** | 10 | Todas as equipes ativas |
| **UnidadeGeradora** | 20 | Top 20 UGs das maiores usinas |
| **RestricaoUS** | 10 | �ltimas 10 restri��es ativas |
| **ArquivoDadger** | 5 | �ltimos 5 arquivos |
| **TOTAL** | **~150** | Dados recentes e relevantes |

### **Script de Extra��o:**

Criaremos `scripts/Extract-And-Migrate-Data.ps1`:

```powershell
# Pseudoc�digo do script:

1. Criar banco tempor�rio (PDPW_TEMP)
2. Restaurar backup COM NORECOVERY (sem logs)
3. Extrair dados em formato SQL INSERT
4. Aplicar INSERTs no banco da POC
5. Dropar banco tempor�rio
6. Limpar arquivos tempor�rios
```

**Vantagens:**
- ? N�o precisa restaurar backup completo
- ? Economiza ~300 GB de espa�o
- ? Mais r�pido (~10-15 min vs 30-40 min)
- ? Dados j� validados e limpos

---

## ?? **FASE 3: POPULAR BANCO DA POC**

### **Op��o A: Via Script SQL (Mais Seguro)**

```sql
-- 1. Desabilitar constraints temporariamente
ALTER TABLE Usinas NOCHECK CONSTRAINT ALL;

-- 2. Inserir dados respeitando FKs
INSERT INTO Empresas (...) SELECT TOP 30 ... FROM PDPW_TEMP.dbo.Empresas;
INSERT INTO TiposUsina (...) SELECT ... FROM PDPW_TEMP.dbo.TiposUsina;
INSERT INTO Usinas (...) SELECT TOP 50 ... FROM PDPW_TEMP.dbo.Usinas;

-- 3. Re-habilitar constraints
ALTER TABLE Usinas CHECK CONSTRAINT ALL;
```

### **Op��o B: Via BCP (Bulk Copy) - Mais R�pido**

```powershell
# Exportar de PDPW_TEMP
bcp "SELECT TOP 30 * FROM Empresas" queryout empresas.dat -S localhost -T -c

# Importar para PDPW_DB
bcp PDPW_DB.dbo.Empresas in empresas.dat -S localhost -T -c
```

### **Op��o C: Via EF Core Seeder (Mais Manuten�vel)**

```csharp
// Criar arquivo: SqlServerDataSeeder.cs
public static class SqlServerDataSeeder
{
    public static void SeedFromBackup(PdpwDbContext context)
    {
        // Conectar ao PDPW_TEMP
        // Ler dados
        // Inserir no context
        // SaveChanges()
    }
}
```

---

## ??? **IMPLEMENTA��O PR�TICA**

### **Passo 1: Preparar Ambiente**

```powershell
# Verificar espa�o dispon�vel
Get-PSDrive C | Select-Object Used, Free

# Verificar SQL Server rodando
docker ps
# ou
Get-Service MSSQL*

# Criar diret�rio para scripts
mkdir C:\temp\_ONS_PoC-PDPW_V2\scripts\migration
```

### **Passo 2: Criar Script de Extra��o**

Arquivo: `scripts/migration/Extract-Legacy-Data.ps1`

```powershell
param(
    [string]$BackupPath = "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak",
    [string]$OutputPath = "C:\temp\_ONS_PoC-PDPW_V2\scripts\migration\data",
    [int]$TopEmpresas = 30,
    [int]$TopUsinas = 50,
    [int]$SemanasPMO = 26
)

# 1. Criar banco tempor�rio
# 2. Restaurar apenas estrutura
# 3. Extrair dados
# 4. Gerar scripts SQL
# 5. Limpar
```

### **Passo 3: Executar Migra��o**

```powershell
# 1. Executar extra��o
.\scripts\migration\Extract-Legacy-Data.ps1

# 2. Aplicar dados no banco da POC
sqlcmd -S localhost -d PDPW_DB -i .\scripts\migration\data\insert-all.sql

# 3. Verificar dados
sqlcmd -S localhost -d PDPW_DB -Q "SELECT COUNT(*) FROM Empresas"
```

---

## ?? **ESTRUTURA DE DADOS ESPERADA**

### **Relacionamentos Principais:**

```
Empresa (30)
  ?
Usina (50)
  ?
UnidadeGeradora (20)
  ?
RestricaoUG (10)

SemanaPMO (26)
  ?
ArquivoDadger (5)

EquipePDP (10)
  ?
Usuarios (vinculados)
```

### **Valida��es Necess�rias:**

- ? Foreign Keys v�lidas
- ? Dados n�o duplicados
- ? Datas consistentes
- ? CNPJs v�lidos
- ? Pot�ncias > 0

---

## ?? **CONFIGURA��O DO DOCKER COMPOSE**

### **Atualizar docker-compose.full.yml:**

```yaml
version: '3.8'

services:
  backend:
    build:
      context: .
      dockerfile: src/PDPW.API/Dockerfile
    ports:
      - "5001:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - UseInMemoryDatabase=false  # ? MUDAN�A AQUI
      - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=PDPW_DB;User Id=sa;Password=Pdpw@2024!Strong;TrustServerCertificate=true;
    depends_on:
      - sqlserver
    networks:
      - pdpw-network

  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=Pdpw@2024!Strong
      - MSSQL_PID=Express
    ports:
      - "1433:1433"
    volumes:
      - sqlserver-data:/var/opt/mssql
    networks:
      - pdpw-network

volumes:
  sqlserver-data:

networks:
  pdpw-network:
```

---

## ? **CHECKLIST DE IMPLEMENTA��O**

### **Prepara��o:**
- [ ] Verificar espa�o em disco (m�nimo 15 GB livre)
- [ ] SQL Server instalado ou Docker configurado
- [ ] Backup do cliente acess�vel
- [ ] Migrations EF Core aplicadas

### **Extra��o:**
- [ ] Script de extra��o criado
- [ ] Backup tempor�rio restaurado
- [ ] Dados extra�dos com sucesso
- [ ] Scripts SQL gerados

### **Migra��o:**
- [ ] Banco PDPW_DB criado
- [ ] Dados inseridos respeitando FKs
- [ ] Constraints validadas
- [ ] �ndices criados

### **Valida��o:**
- [ ] Contagem de registros correta (~150)
- [ ] Relacionamentos funcionando
- [ ] APIs retornando dados
- [ ] Swagger testado

### **Documenta��o:**
- [ ] Guia de restaura��o criado
- [ ] Troubleshooting documentado
- [ ] Dados catalogados

---

## ?? **PR�XIMOS PASSOS**

### **Decis�o Necess�ria:**

**Qual abordagem preferir:**

**A) ?? Extra��o Autom�tica (Recomendado)**
- Criar script PowerShell completo
- Execu��o �nica (~15 min)
- 150 registros extra�dos automaticamente

**B) ??? Extra��o Manual Guiada**
- Criar queries SQL espec�ficas
- Executar passo a passo
- Mais controle sobre dados

**C) ?? Usar SQL Server em Docker**
- Mais isolado
- F�cil de resetar
- Configura��o via docker-compose

**D) ?? Usar SQL Server Express Local**
- Mais performance
- Acesso via SSMS
- Sem overhead de Docker

---

## ?? **RECOMENDA��O FINAL**

### **Estrat�gia H�brida: C + A**

1. ? **Usar SQL Server em Docker** (docker-compose.full.yml)
2. ? **Criar script de extra��o autom�tica**
3. ? **Popular dados incrementalmente**
4. ? **Manter seed data como fallback**

**Vantagens:**
- Isolamento completo
- F�cil de resetar
- Automatizado
- Reproduz�vel

**Tempo estimado:** 30-45 minutos de implementa��o

---

## ?? **A��O IMEDIATA**

**Gostaria que eu:**

1?? **Crie o script de extra��o completo?**
2?? **Configure docker-compose.full.yml com SQL Server?**
3?? **Atualize migrations para usar SQL Server?**
4?? **Todas as op��es acima?** ? **Recomendado**

---

**Aguardando sua decis�o para prosseguir...** ??

---

**Data:** 20/12/2024  
**Status:** Aguardando Aprova��o  
**Espa�o Necess�rio:** ~15 GB  
**Tempo Estimado:** 30-45 minutos

