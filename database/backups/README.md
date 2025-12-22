# ??? Banco de Dados - PDPW

## ?? Localiza��o do Backup

O backup do banco de dados fornecido pelo cliente ONS est� armazenado em:

**Caminho:** `C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak`

---

## ?? Informa��es do Banco de Dados

### Banco de Dados Legado

| Item | Detalhes |
|------|----------|
| **SGBD** | SQL Server |
| **Nome do Banco** | PDP_TST |
| **Backup File** | Backup_PDP_TST.bak |
| **Tipo de Backup** | Full Database Backup (.bak) |

### Backup Fornecido

| Item | Detalhes |
|------|----------|
| **Tipo de Backup** | .bak (SQL Server Full Backup) |
| **Localiza��o** | C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak |
| **Status** | ? Recebido e dispon�vel |

---

## ?? Estrutura de Armazenamento

```
C:\temp\_ONS_PoC-PDPW\
??? pdpw_act/
?   ??? Backup_PDP_TST.bak        ? ? Backup fornecido pelo cliente
?
??? database/
?   ??? backups/                   ? Organiza��o de backups
?   ?   ??? original/              ? Backup original (n�o modificar!)
?   ?   ??? working/               ? C�pia para trabalho
?   ?   ??? README.md              ? Este arquivo
?   ?
?   ??? scripts/                   ? Scripts SQL �teis
?   ?   ??? schema/                ? Scripts de cria��o de schema
?   ?   ??? data/                  ? Scripts de dados de teste
?   ?   ??? migrations/            ? Scripts de migra��o
?   ?
?   ??? docs/                      ? Documenta��o do banco
?       ??? schema.md              ? Documenta��o do schema
?       ??? tables.md              ? Descri��o das tabelas
?       ??? relationships.md       ? Relacionamentos
```

---

## ?? Restaura��o R�pida

### ? Passo 1: Copiar para Working (Recomendado)

```powershell
# Criar c�pia para trabalho (n�o modificar o original!)
Copy-Item "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak" "C:\temp\_ONS_PoC-PDPW\database\backups\working\Backup_PDP_TST.bak"
```

### ? Passo 2: Executar Restaura��o Automatizada

```powershell
# Navegar para o projeto
cd C:\temp\_ONS_PoC-PDPW

# Restaurar usando script automatizado
.\database\restore-database.ps1 -BackupPath "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak" -DatabaseName "PDPW_DB"
```

**O script vai:**
- ? Verificar conex�o com SQL Server
- ? Analisar o backup
- ? Restaurar o banco como `PDPW_DB`
- ? Verificar integridade
- ? Atualizar estat�sticas
- ? Exibir informa��es do banco

---

## ?? Como Restaurar o Backup

### Op��o 1: Script PowerShell Automatizado (RECOMENDADO)

```powershell
# Restaura��o completa com an�lise e gera��o de scripts
.\database\restore-database.ps1 `
    -BackupPath "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak" `
    -DatabaseName "PDPW_DB" `
    -GenerateScripts
```

### Op��o 2: SQL Server Management Studio (SSMS)

```sql
-- 1. Restaurar backup
USE master;
GO

-- Verificar conte�do do backup
RESTORE FILELISTONLY 
FROM DISK = 'C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak';
GO

-- Restaurar banco
RESTORE DATABASE [PDPW_DB]
FROM DISK = 'C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak'
WITH 
    MOVE 'PDP_TST' TO 'C:\temp\_ONS_PoC-PDPW\database\data\PDPW_DB.mdf',
    MOVE 'PDP_TST_log' TO 'C:\temp\_ONS_PoC-PDPW\database\data\PDPW_DB_log.ldf',
    REPLACE,
    RECOVERY,
    STATS = 10;
GO

-- 2. Verificar restaura��o
SELECT name, state_desc, recovery_model_desc 
FROM sys.databases 
WHERE name = 'PDPW_DB';
GO

-- 3. Verificar integridade
DBCC CHECKDB('PDPW_DB') WITH NO_INFOMSGS;
GO

-- 4. Atualizar estat�sticas
USE PDPW_DB;
GO
EXEC sp_updatestats;
GO
```

### Op��o 3: PowerShell Manual

```powershell
# Restaurar backup via PowerShell
$backupFile = "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak"
$dataFile = "C:\temp\_ONS_PoC-PDPW\database\data\PDPW_DB.mdf"
$logFile = "C:\temp\_ONS_PoC-PDPW\database\data\PDPW_DB_log.ldf"

# Importar m�dulo SQL Server
Import-Module SqlServer

# Restaurar
Restore-SqlDatabase `
    -ServerInstance "localhost" `
    -Database "PDPW_DB" `
    -BackupFile $backupFile `
    -RelocateFile @{
        "PDP_TST" = $dataFile
        "PDP_TST_log" = $logFile
    } `
    -ReplaceDatabase
```

---

## ?? Checklist de Restaura��o

### Antes de Restaurar

- [x] Backup recebido do cliente
- [x] Backup localizado em: `C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak`
- [ ] Verificar espa�o em disco suficiente
- [ ] SQL Server instalado e rodando
- [ ] Permiss�es adequadas configuradas

### Durante a Restaura��o

- [ ] Executar script de restaura��o
- [ ] Monitorar logs de erro
- [ ] Verificar uso de recursos (CPU, mem�ria)
- [ ] Anotar tempo de restaura��o

### Ap�s a Restaura��o

- [ ] Verificar integridade do banco
  ```sql
  DBCC CHECKDB('PDPW_DB') WITH NO_INFOMSGS;
  ```
- [ ] Atualizar estat�sticas
  ```sql
  EXEC sp_updatestats;
  ```
- [ ] Verificar orphan users
  ```sql
  EXEC sp_change_users_login 'Report';
  ```
- [ ] Documentar schema extra�do

---

## ?? An�lise do Banco de Dados

### Extrair Schema

```sql
-- Listar todas as tabelas
SELECT 
    s.name AS SchemaName,
    t.name AS TableName,
    t.create_date,
    t.modify_date
FROM sys.tables t
INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
ORDER BY s.name, t.name;

-- Listar todas as colunas de uma tabela
SELECT 
    c.name AS ColumnName,
    t.name AS DataType,
    c.max_length,
    c.precision,
    c.scale,
    c.is_nullable,
    c.is_identity
FROM sys.columns c
INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE object_id = OBJECT_ID('[SchemaName].[TableName]')
ORDER BY c.column_id;

-- Listar relacionamentos (Foreign Keys)
SELECT 
    fk.name AS ForeignKeyName,
    OBJECT_NAME(fk.parent_object_id) AS TableName,
    COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS ColumnName,
    OBJECT_NAME(fk.referenced_object_id) AS ReferencedTableName,
    COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) AS ReferencedColumnName
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
ORDER BY TableName, ColumnName;

-- Listar stored procedures
SELECT 
    s.name AS SchemaName,
    p.name AS ProcedureName,
    p.create_date,
    p.modify_date
FROM sys.procedures p
INNER JOIN sys.schemas s ON p.schema_id = s.schema_id
ORDER BY s.name, p.name;
```

### Gerar Script do Schema

```powershell
# Gerar script do schema completo
Import-Module SqlServer

$server = "localhost"
$database = "PDPW_DB"
$outputPath = "C:\temp\_ONS_PoC-PDPW\database\scripts\schema\"

# Criar diret�rio se n�o existir
if (-not (Test-Path $outputPath)) {
    New-Item -ItemType Directory -Path $outputPath -Force | Out-Null
}

$srv = New-Object Microsoft.SqlServer.Management.Smo.Server($server)
$db = $srv.Databases[$database]

$scripter = New-Object Microsoft.SqlServer.Management.Smo.Scripter($srv)
$scripter.Options.ScriptDrops = $false
$scripter.Options.IncludeHeaders = $true
$scripter.Options.ToFileOnly = $true
$scripter.Options.Indexes = $true
$scripter.Options.DriAll = $true

# Script de tabelas
$scripter.Options.FileName = "$outputPath\Tables.sql"
$db.Tables | ForEach-Object { $scripter.Script($_) }

# Script de views
$scripter.Options.FileName = "$outputPath\Views.sql"
$db.Views | Where-Object { -not $_.IsSystemObject } | ForEach-Object { $scripter.Script($_) }

# Script de stored procedures
$scripter.Options.FileName = "$outputPath\StoredProcedures.sql"
$db.StoredProcedures | Where-Object { -not $_.IsSystemObject } | ForEach-Object { $scripter.Script($_) }
```

---

## ??? Mapeamento para EF Core

### Engenharia Reversa (Scaffold)

```powershell
# Navegar para o projeto Infrastructure
cd C:\temp\_ONS_PoC-PDPW\src\PDPW.Infrastructure

# Instalar ferramenta EF (se necess�rio)
dotnet tool install --global dotnet-ef

# Verificar instala��o
dotnet ef --version

# Scaffold do banco de dados
dotnet ef dbcontext scaffold "Server=localhost;Database=PDPW_DB;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir LegacyModels --context-dir Data --context PDPWLegacyContext --force --no-onconfiguring --data-annotations
```

**Resultado:**
```
PDPW.Infrastructure/
??? Data/
?   ??? PDPWLegacyContext.cs    ? DbContext gerado
??? LegacyModels/
    ??? Tabela1.cs              ? Entidades geradas
    ??? Tabela2.cs
    ??? ...
```

### Compara��o: Legado vs Novo

Ap�s o scaffold, comparar entidades geradas com as do Domain:

```
Legado (Scaffold)         Novo (Clean Architecture)
?????????????????         ?????????????????????????
PDPWLegacyContext    ???> PDPWDbContext
LegacyModels/       ???> Domain/Entities/
```

---

## ?? Dados de Teste

### Extrair Dados de Amostra

```sql
-- Conectar ao banco PDPW_DB

-- Extrair dados de uma tabela (max 100 registros)
SELECT TOP 100 * 
FROM [SchemaName].[TableName]
ORDER BY [DataColumn] DESC;

-- Gerar script INSERT (usar SSMS)
-- Bot�o direito na tabela > Script Table as > INSERT To > New Query Window
```

### Criar Dados de Teste

```sql
-- Script para popular banco de testes
-- database/scripts/data/seed_data.sql

USE PDPW_DB;
GO

-- Limpar dados de teste (cuidado!)
-- DELETE FROM [TabelaDependente];
-- DELETE FROM [TabelaPrincipal];

-- Inserir dados de teste
-- INSERT INTO [TabelaPrincipal] (Coluna1, Coluna2, Coluna3)
-- VALUES 
--     ('Valor1', 'Valor2', 'Valor3'),
--     ('Valor4', 'Valor5', 'Valor6');

-- Verificar
-- SELECT * FROM [TabelaPrincipal];
```

---

## ?? Seguran�a e Boas Pr�ticas

### ?? IMPORTANTE

1. **Backup Original**
   - ? **NUNCA** modificar o backup original fornecido pelo cliente
   - ? Sempre trabalhar com uma c�pia em `working/`
   - ? Versionar apenas scripts SQL (n�o os .bak)
   - ? Manter backup original em: `C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak`

2. **Dados Sens�veis**
   - ? **N�O** commitar backups para o Git
   - ? **N�O** commitar dados de produ��o
   - ? Anonimizar dados se necess�rio
   - ? Usar dados de teste/mock

3. **Connection Strings**
   - ? **N�O** commitar connection strings com senhas
   - ? Usar User Secrets / Variables de ambiente
   - ? Documentar apenas o formato

### .gitignore

Adicionar ao `.gitignore`:

```gitignore
# Backups de banco de dados
*.bak
*.bacpac
*.mdf
*.ldf
database/backups/
database/data/
pdpw_act/Backup_PDP_TST.bak

# Connection strings locais
appsettings.local.json
```

---

## ?? Documenta��o do Schema

### Template: database/docs/tables.md

```markdown
# Tabelas do Banco de Dados PDPW

## Tabela: [NomeDaTabela]

**Descri��o:** [O que esta tabela armazena]

### Colunas

| Coluna | Tipo | Nullable | PK | FK | Descri��o |
|--------|------|----------|----|----|-----------|
| Id | int | N�o | Sim | - | Identificador �nico |
| Nome | nvarchar(100) | N�o | - | - | Nome do registro |
| DataCriacao | datetime | N�o | - | - | Data de cria��o |
| ... | ... | ... | ... | ... | ... |

### Relacionamentos

- **FK_Tabela_OutraTabela**: Relacionamento com OutraTabela (muitos-para-um)

### �ndices

- **PK_Tabela**: Chave prim�ria em `Id`
- **IX_Tabela_Nome**: �ndice em `Nome`

### Observa��es

- Esta tabela � usada para...
- Importante: campo X n�o pode ser nulo porque...
```

---

## ?? Pr�ximos Passos

### ? 1. Restaurar o Banco

```powershell
cd C:\temp\_ONS_PoC-PDPW
.\database\restore-database.ps1 -BackupPath "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak" -DatabaseName "PDPW_DB" -GenerateScripts
```

### ? 2. Explorar o Schema

```sql
-- Conectar via SSMS: localhost\PDPW_DB

-- Listar tabelas
SELECT s.name + '.' + t.name AS TableName, p.rows AS RowCount
FROM sys.tables t
INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
INNER JOIN sys.partitions p ON t.object_id = p.object_id
WHERE p.index_id IN (0, 1)
ORDER BY p.rows DESC;
```

### ? 3. Scaffold do EF Core

```powershell
cd C:\temp\_ONS_PoC-PDPW\src\PDPW.Infrastructure
dotnet ef dbcontext scaffold "Server=localhost;Database=PDPW_DB;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir LegacyModels --context PDPWLegacyContext --force
```

### ? 4. Documentar Descobertas

- Criar `database/docs/tables.md`
- Mapear entidades principais
- Identificar fluxos cr�ticos
- Documentar stored procedures

### ? 5. Atualizar appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PDPW_DB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "UseInMemoryDatabase": false
}
```

---

## ?? Contato

**Desenvolvedor:** Willian Charantola Bulhoes  
**GitHub:** https://github.com/wbulhoes

---

**�ltima atualiza��o:** 17/12/2025  
**Status:** ? Backup dispon�vel - Pronto para restaura��o!
