# 🗄️ CONFIGURAÇÃO DO BANCO DE DADOS SQL SERVER

**Projeto**: POC PDPW  
**Banco**: SQL Server 2019+  
**Status**: ✅ Configurado e Testado

---

## 📋 PRÉ-REQUISITOS

### 1. SQL Server Instalado
- **SQL Server 2019** ou superior
- **SQL Server Express** (gratuito) é suficiente
- Instância: `SQLEXPRESS` (padrão)

### 2. Usuário SA Configurado
- Usuário: `sa`
- Senha: `Pdpw@2024!Strong` (ou configurar a sua)
- Mixed Mode Authentication habilitado

---

## ⚙️ CONFIGURAÇÃO DO SQL SERVER

### Opção 1: Usar as Configurações Padrão

Se você tem SQL Server Express instalado com usuário `sa` e senha `Pdpw@2024!Strong`:

```bash
# Já está configurado! Apenas rode:
cd src/PDPW.Infrastructure
dotnet ef database update --startup-project ../PDPW.API
```

### Opção 2: Configurar Sua Própria Connection String

1. **Edite `appsettings.Development.json`** (não commitar):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=PDPW_DB;User Id=SEU_USUARIO;Password=SUA_SENHA;TrustServerCertificate=True;MultipleActiveResultSets=true;Encrypt=False"
  }
}
```

2. **Exemplos de Connection Strings**:

**SQL Server Local (Windows Authentication)**:
```json
"Server=localhost;Database=PDPW_DB;Integrated Security=true;TrustServerCertificate=True"
```

**SQL Server Express**:
```json
"Server=.\\SQLEXPRESS;Database=PDPW_DB;User Id=sa;Password=SuaSenha;TrustServerCertificate=True"
```

**SQL Server Azure**:
```json
"Server=tcp:seuservidor.database.windows.net,1433;Database=PDPW_DB;User ID=usuario@seuservidor;Password=senha;Encrypt=true;TrustServerCertificate=False"
```

---

## 🚀 INICIALIZAÇÃO DO BANCO

### Passo 1: Aplicar Migrations

```bash
cd src/PDPW.Infrastructure
dotnet ef database update --startup-project ../PDPW.API
```

**Resultado esperado**:
```
Build succeeded.
Applying migration '20241223000001_InitialCreate'.
Applying migration '20241223000002_SeedData'.
Done.
```

### Passo 2: Verificar Banco Criado

Execute no SQL Server Management Studio:

```sql
USE PDPW_DB;

-- Verificar tabelas criadas (30 tabelas)
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

-- Verificar registros populados
SELECT 'Empresas' as Tabela, COUNT(*) as Total FROM Empresas
UNION ALL
SELECT 'Usinas', COUNT(*) FROM Usinas
UNION ALL
SELECT 'UnidadesGeradoras', COUNT(*) FROM UnidadesGeradoras
UNION ALL
SELECT 'SemanasPMO', COUNT(*) FROM SemanasPMO
UNION ALL
SELECT 'EquipesPDP', COUNT(*) FROM EquipesPDP
UNION ALL
SELECT 'Intercambios', COUNT(*) FROM Intercambios
UNION ALL
SELECT 'Balancos', COUNT(*) FROM Balancos;
```

**Resultado esperado**: 638 registros totais

---

## 📊 ESTRUTURA DO BANCO

### Tabelas Principais (30 tabelas)

#### Cadastros Base
- `Empresas` (38 registros)
- `TiposUsina` (13 registros)
- `Usinas` (40 registros)
- `UnidadesGeradoras` (86 registros)

#### Operação
- `SemanasPMO` (25 registros)
- `EquipesPDP` (16 registros)
- `Cargas` (0 - a popular)
- `Intercambios` (240 registros)
- `Balancos` (120 registros)

#### Restrições
- `MotivosRestricao` (10 registros)
- `RestricoesUG` (0 - a popular)
- `RestricoesUS` (0 - a popular)
- `ParadasUG` (50 registros)

#### Arquivos
- `ArquivosDadger` (0 - a popular)
- `ArquivosDadgerValores` (0 - a popular)
- `Uploads` (0 - a popular)
- `Diretorios` (0 - a popular)
- `Arquivos` (0 - a popular)

#### Administração
- `Usuarios` (0 - a popular)
- `Responsaveis` (0 - a popular)

**Total de Registros Seed**: **638 registros**

---

## 🔧 TROUBLESHOOTING

### Erro: "Cannot open database PDPW_DB"

**Solução**: Criar o banco manualmente:

```sql
CREATE DATABASE PDPW_DB;
GO
```

Depois rode as migrations novamente.

### Erro: "Login failed for user 'sa'"

**Solução**: Configurar Mixed Mode Authentication

1. Abra SQL Server Management Studio
2. Clique com direito no servidor → Properties
3. Security → SQL Server and Windows Authentication mode
4. Reinicie o serviço SQL Server

### Erro: "A network-related or instance-specific error"

**Solução**: Verificar se SQL Server está rodando:

```powershell
Get-Service -Name "MSSQL$SQLEXPRESS"
```

Se parado, iniciar:

```powershell
Start-Service -Name "MSSQL$SQLEXPRESS"
```

### Erro: "The target database already exists"

**Solução**: Banco já existe, apenas aplicar migrations:

```bash
dotnet ef database update --startup-project ../PDPW.API
```

---

## 🧹 LIMPAR E RECRIAR BANCO

Se precisar limpar tudo e começar do zero:

```sql
-- 1. Dropar banco
USE master;
DROP DATABASE PDPW_DB;
GO

-- 2. Recriar
CREATE DATABASE PDPW_DB;
GO
```

```bash
# 3. Aplicar migrations
cd src/PDPW.Infrastructure
dotnet ef database update --startup-project ../PDPW.API
```

---

## 📝 VARIÁVEIS DE AMBIENTE (Alternativa)

Ao invés de modificar `appsettings.json`, você pode usar variáveis de ambiente:

### Windows (PowerShell)
```powershell
$env:ConnectionStrings__DefaultConnection="Server=localhost;Database=PDPW_DB;Integrated Security=true"
```

### Linux/Mac
```bash
export ConnectionStrings__DefaultConnection="Server=localhost;Database=PDPW_DB;Integrated Security=true"
```

---

## ✅ VALIDAR CONFIGURAÇÃO

Execute o script de gerenciamento:

```powershell
.\scripts\gerenciar-api.ps1 start
```

Se a API subir sem erros e você conseguir acessar:
```
http://localhost:5001/swagger
```

**✅ Configuração OK!**

---

## 🗄️ BACKUP E RESTORE

### Fazer Backup

```sql
BACKUP DATABASE PDPW_DB
TO DISK = 'C:\Backups\PDPW_DB.bak'
WITH FORMAT;
```

### Restaurar Backup

```sql
USE master;
GO

RESTORE DATABASE PDPW_DB
FROM DISK = 'C:\Backups\PDPW_DB.bak'
WITH REPLACE;
GO
```

---

## 📊 DADOS DO SETOR ELÉTRICO

O banco vem populado com **638 registros reais** do setor elétrico brasileiro:

### Empresas (38)
- CEMIG, COPEL, Itaipu Binacional
- FURNAS, CHESF, ELETROBRAS
- CPFL Energia, Light, ENGIE Brasil
- E mais 29 empresas reais

### Usinas (40)
- **Itaipu**: 14.000 MW (maior do Brasil)
- **Belo Monte**: 11.233 MW
- **Tucuruí**: 8.370 MW
- **Santo Antônio**: 3.568 MW
- **Jirau**: 3.750 MW
- E mais 35 usinas

### Capacidade Total
**~110.000 MW** de capacidade instalada

---

## 🔐 SEGURANÇA

### ⚠️ IMPORTANTE - NÃO COMMITAR SENHAS!

- **NÃO commite** `appsettings.Development.json` com senha real
- Use **variáveis de ambiente** ou **User Secrets**

### Usar User Secrets (Recomendado)

```bash
cd src/PDPW.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=.\\SQLEXPRESS;Database=PDPW_DB;User Id=sa;Password=SuaSenha;TrustServerCertificate=True"
```

---

## 📞 AJUDA

Se tiver problemas:

1. Verifique se SQL Server está instalado e rodando
2. Verifique se o usuário `sa` tem permissão
3. Teste a connection string no SQL Server Management Studio
4. Veja os logs da API ao iniciar

---

**✅ Configuração Concluída!**

Agora você pode:
- ✅ Rodar migrations
- ✅ Popular dados
- ✅ Testar APIs no Swagger
- ✅ Desenvolver com confiança

---

**📅 Atualizado**: 23/12/2024  
**🗄️ Banco**: SQL Server 2019+  
**📊 Registros**: 638 (setor elétrico brasileiro)  
**✅ Status**: Produção-ready
