# ??? PLANO DE MIGRAÇÃO - BASE SQL SERVER COM DADOS REAIS

## ?? **OBJETIVO**

Criar um banco de dados SQL Server com **~150 registros reais** extraídos do backup do cliente, mantendo relacionamentos e integridade referencial para testes robustos via Swagger.

---

## ?? **SITUAÇÃO ATUAL**

### ? **O que temos:**
- ? Backup do cliente: `C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak` (43.2 GB)
- ? Código legado VB.NET: `C:\temp\_ONS_PoC-PDPW_V2\legado`
- ? POC .NET 8 funcionando com **InMemory** (69 registros)
- ? Migrations EF Core criadas
- ? Docker Compose configurado

### ?? **Limitações:**
- ? Backup completo requer 350 GB (não temos espaço)
- ? InMemory perde dados ao reiniciar
- ? InMemory não simula comportamento real de SQL Server

---

## ?? **ESTRATÉGIA RECOMENDADA: EXTRAÇÃO SELETIVA**

### **Abordagem em 3 Fases:**

```
FASE 1: Criar banco SQL Server vazio com schema
  ?
FASE 2: Extrair ~150 registros do backup (seletivo)
  ?
FASE 3: Popular banco SQL Server da POC
```

**Espaço necessário:** ~10-15 GB (viável!)

---

## ?? **FASE 1: CRIAR BANCO SQL SERVER VAZIO**

### **Opção A: Usar docker-compose.full.yml (Recomendado)**

**Vantagens:**
- ? Já configurado
- ? Isolado em container
- ? Fácil de resetar
- ? Persistência em volumes Docker

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

### **Opção B: SQL Server Express Local**

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

### **Estratégia de Extração Seletiva**

#### **Tabelas Prioritárias (150 registros):**

| Tabela | Qtd Registros | Critério de Seleção |
|--------|---------------|---------------------|
| **Empresa** | 30 | Top 30 por quantidade de usinas |
| **TipoUsina** | 10 | Todos os tipos (UHE, UTE, EOL, UFV, PCH, etc) |
| **Usina** | 50 | Top 50 por potência instalada |
| **SemanaPMO** | 26 | Últimos 6 meses (26 semanas) |
| **EquipePDP** | 10 | Todas as equipes ativas |
| **UnidadeGeradora** | 20 | Top 20 UGs das maiores usinas |
| **RestricaoUS** | 10 | Últimas 10 restrições ativas |
| **ArquivoDadger** | 5 | Últimos 5 arquivos |
| **TOTAL** | **~150** | Dados recentes e relevantes |

### **Script de Extração:**

Criaremos `scripts/Extract-And-Migrate-Data.ps1`:

```powershell
# Pseudocódigo do script:

1. Criar banco temporário (PDPW_TEMP)
2. Restaurar backup COM NORECOVERY (sem logs)
3. Extrair dados em formato SQL INSERT
4. Aplicar INSERTs no banco da POC
5. Dropar banco temporário
6. Limpar arquivos temporários
```

**Vantagens:**
- ? Não precisa restaurar backup completo
- ? Economiza ~300 GB de espaço
- ? Mais rápido (~10-15 min vs 30-40 min)
- ? Dados já validados e limpos

---

## ?? **FASE 3: POPULAR BANCO DA POC**

### **Opção A: Via Script SQL (Mais Seguro)**

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

### **Opção B: Via BCP (Bulk Copy) - Mais Rápido**

```powershell
# Exportar de PDPW_TEMP
bcp "SELECT TOP 30 * FROM Empresas" queryout empresas.dat -S localhost -T -c

# Importar para PDPW_DB
bcp PDPW_DB.dbo.Empresas in empresas.dat -S localhost -T -c
```

### **Opção C: Via EF Core Seeder (Mais Manutenível)**

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

## ??? **IMPLEMENTAÇÃO PRÁTICA**

### **Passo 1: Preparar Ambiente**

```powershell
# Verificar espaço disponível
Get-PSDrive C | Select-Object Used, Free

# Verificar SQL Server rodando
docker ps
# ou
Get-Service MSSQL*

# Criar diretório para scripts
mkdir C:\temp\_ONS_PoC-PDPW_V2\scripts\migration
```

### **Passo 2: Criar Script de Extração**

Arquivo: `scripts/migration/Extract-Legacy-Data.ps1`

```powershell
param(
    [string]$BackupPath = "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak",
    [string]$OutputPath = "C:\temp\_ONS_PoC-PDPW_V2\scripts\migration\data",
    [int]$TopEmpresas = 30,
    [int]$TopUsinas = 50,
    [int]$SemanasPMO = 26
)

# 1. Criar banco temporário
# 2. Restaurar apenas estrutura
# 3. Extrair dados
# 4. Gerar scripts SQL
# 5. Limpar
```

### **Passo 3: Executar Migração**

```powershell
# 1. Executar extração
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

### **Validações Necessárias:**

- ? Foreign Keys válidas
- ? Dados não duplicados
- ? Datas consistentes
- ? CNPJs válidos
- ? Potências > 0

---

## ?? **CONFIGURAÇÃO DO DOCKER COMPOSE**

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
      - UseInMemoryDatabase=false  # ? MUDANÇA AQUI
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

## ? **CHECKLIST DE IMPLEMENTAÇÃO**

### **Preparação:**
- [ ] Verificar espaço em disco (mínimo 15 GB livre)
- [ ] SQL Server instalado ou Docker configurado
- [ ] Backup do cliente acessível
- [ ] Migrations EF Core aplicadas

### **Extração:**
- [ ] Script de extração criado
- [ ] Backup temporário restaurado
- [ ] Dados extraídos com sucesso
- [ ] Scripts SQL gerados

### **Migração:**
- [ ] Banco PDPW_DB criado
- [ ] Dados inseridos respeitando FKs
- [ ] Constraints validadas
- [ ] Índices criados

### **Validação:**
- [ ] Contagem de registros correta (~150)
- [ ] Relacionamentos funcionando
- [ ] APIs retornando dados
- [ ] Swagger testado

### **Documentação:**
- [ ] Guia de restauração criado
- [ ] Troubleshooting documentado
- [ ] Dados catalogados

---

## ?? **PRÓXIMOS PASSOS**

### **Decisão Necessária:**

**Qual abordagem preferir:**

**A) ?? Extração Automática (Recomendado)**
- Criar script PowerShell completo
- Execução única (~15 min)
- 150 registros extraídos automaticamente

**B) ??? Extração Manual Guiada**
- Criar queries SQL específicas
- Executar passo a passo
- Mais controle sobre dados

**C) ?? Usar SQL Server em Docker**
- Mais isolado
- Fácil de resetar
- Configuração via docker-compose

**D) ?? Usar SQL Server Express Local**
- Mais performance
- Acesso via SSMS
- Sem overhead de Docker

---

## ?? **RECOMENDAÇÃO FINAL**

### **Estratégia Híbrida: C + A**

1. ? **Usar SQL Server em Docker** (docker-compose.full.yml)
2. ? **Criar script de extração automática**
3. ? **Popular dados incrementalmente**
4. ? **Manter seed data como fallback**

**Vantagens:**
- Isolamento completo
- Fácil de resetar
- Automatizado
- Reproduzível

**Tempo estimado:** 30-45 minutos de implementação

---

## ?? **AÇÃO IMEDIATA**

**Gostaria que eu:**

1?? **Crie o script de extração completo?**
2?? **Configure docker-compose.full.yml com SQL Server?**
3?? **Atualize migrations para usar SQL Server?**
4?? **Todas as opções acima?** ? **Recomendado**

---

**Aguardando sua decisão para prosseguir...** ??

---

**Data:** 20/12/2024  
**Status:** Aguardando Aprovação  
**Espaço Necessário:** ~15 GB  
**Tempo Estimado:** 30-45 minutos

