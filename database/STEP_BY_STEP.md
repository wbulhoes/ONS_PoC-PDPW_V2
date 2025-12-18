# ?? Passo a Passo - Restauração do Banco PDPW

## ? Backup Disponível!

**Arquivo:** `C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak`  
**Status:** ? Pronto para restauração

---

## ?? Etapas de Restauração

### Passo 1: Verificar Pré-requisitos

#### 1.1. Executar Script de Verificação (Recomendado - Não Precisa de Admin)

```powershell
# Navegar para o projeto
cd C:\temp\_ONS_PoC-PDPW

# Executar script de verificação completa
.\database\check-prerequisites.ps1
```

**O script vai verificar:**
- ? SQL Server (via processo, não precisa de admin)
- ? Porta SQL Server (1433)
- ? Arquivo de backup
- ? Espaço em disco
- ? dotnet-ef tool
- ? .NET SDK
- ? Estrutura do projeto

#### 1.2. Verificação Manual (Alternativa)

##### SQL Server (Método Alternativo)

```powershell
# Opção 1: Via processo (não precisa de admin)
Get-Process | Where-Object {$_.ProcessName -like "*sqlservr*"}

# Opção 2: Via porta
Test-NetConnection -ComputerName localhost -Port 1433 -InformationLevel Quiet

# Opção 3: Via Task Manager
# 1. Abrir Task Manager (Ctrl + Shift + Esc)
# 2. Aba "Services"
# 3. Procurar "MSSQLSERVER" ou "MSSQL$SQLEXPRESS"
```

**Se SQL Server não estiver rodando:**
- Abrir `services.msc`
- Procurar por "SQL Server (MSSQLSERVER)" ou "SQL Server (SQLEXPRESS)"
- Clicar com botão direito > Iniciar

**Se não estiver instalado:**
- Download: https://www.microsoft.com/sql-server/sql-server-downloads
- Ou instalar SQL Server Express (gratuito)

##### Verificar Espaço em Disco

```powershell
# Verificar espaço disponível no drive C:
Get-PSDrive C | Select-Object Used,Free

# Recomendado: Pelo menos 5 GB livres
```

##### Verificar Arquivo de Backup

```powershell
# Verificar se backup existe
Test-Path "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak"

# Ver informações do arquivo
Get-Item "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak" | Select-Object Name, Length, CreationTime
```

---

### Passo 2: Executar Restauração

#### 2.1. Abrir PowerShell como Administrador

```
1. Pressionar Win + X
2. Selecionar "Windows PowerShell (Admin)" ou "Terminal (Admin)"
3. Navegar para o projeto:
   cd C:\temp\_ONS_PoC-PDPW
```

#### 2.2. Executar Script de Restauração

```powershell
# Restauração básica
.\database\restore-database.ps1 -BackupPath "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak" -DatabaseName "PDPW_DB"

# Restauração com geração de scripts do schema
.\database\restore-database.ps1 `
    -BackupPath "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak" `
    -DatabaseName "PDPW_DB" `
    -GenerateScripts

# Apenas analisar (não restaurar)
.\database\restore-database.ps1 `
    -BackupPath "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak" `
    -AnalyzeOnly
```

#### 2.3. Aguardar Conclusão

**Durante a restauração, o script irá:**
1. ? Verificar conexão com SQL Server
2. ? Analisar conteúdo do backup
3. ? Dropar banco existente (se houver)
4. ? Restaurar banco de dados
5. ? Verificar integridade (DBCC CHECKDB)
6. ? Atualizar estatísticas
7. ? Exibir informações do banco

**Tempo estimado:** 2-10 minutos (depende do tamanho)

---

### Passo 3: Verificar Restauração

#### 3.1. Via PowerShell

```powershell
# Verificar se banco foi criado
Invoke-Sqlcmd -ServerInstance "localhost" -Query "SELECT name, state_desc FROM sys.databases WHERE name = 'PDPW_DB'" -TrustServerCertificate

# Listar tabelas
Invoke-Sqlcmd -ServerInstance "localhost" -Database "PDPW_DB" -Query "SELECT s.name + '.' + t.name AS TableName FROM sys.tables t INNER JOIN sys.schemas s ON t.schema_id = s.schema_id ORDER BY s.name, t.name" -TrustServerCertificate
```

#### 3.2. Via SQL Server Management Studio (SSMS)

```
1. Abrir SSMS
2. Conectar a: localhost (ou localhost\SQLEXPRESS)
3. Expandir Databases
4. Verificar se PDPW_DB aparece
5. Expandir PDPW_DB > Tables
6. Ver tabelas restauradas
```

---

### Passo 4: Explorar o Banco

#### 4.1. Informações Gerais

```sql
-- Conectar ao banco PDPW_DB via SSMS

-- Ver informações do banco
SELECT 
    name AS DatabaseName,
    state_desc AS State,
    recovery_model_desc AS RecoveryModel,
    compatibility_level AS CompatibilityLevel,
    create_date AS CreateDate
FROM sys.databases 
WHERE name = 'PDPW_DB';

-- Ver tamanho do banco
EXEC sp_spaceused;

-- Ver usuários
SELECT name FROM sys.database_principals WHERE type = 'S';
```

#### 4.2. Tabelas Principais

```sql
-- Listar todas as tabelas com quantidade de registros
SELECT 
    s.name AS SchemaName,
    t.name AS TableName,
    p.rows AS RowCount,
    SUM(a.total_pages) * 8 AS TotalSpaceKB
FROM sys.tables t
INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
INNER JOIN sys.partitions p ON t.object_id = p.object_id
INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id
WHERE p.index_id IN (0, 1)
GROUP BY s.name, t.name, p.rows
ORDER BY p.rows DESC;

-- Ver primeiros registros de cada tabela
-- Substitua [SchemaName] e [TableName]
SELECT TOP 10 * FROM [SchemaName].[TableName];
```

#### 4.3. Stored Procedures

```sql
-- Listar stored procedures
SELECT 
    s.name AS SchemaName,
    p.name AS ProcedureName,
    p.create_date,
    p.modify_date
FROM sys.procedures p
INNER JOIN sys.schemas s ON p.schema_id = s.schema_id
WHERE p.is_ms_shipped = 0
ORDER BY s.name, p.name;

-- Ver definição de uma stored procedure
EXEC sp_helptext 'SchemaName.ProcedureName';
```

#### 4.4. Views

```sql
-- Listar views
SELECT 
    s.name AS SchemaName,
    v.name AS ViewName,
    v.create_date,
    v.modify_date
FROM sys.views v
INNER JOIN sys.schemas s ON v.schema_id = s.schema_id
WHERE v.is_ms_shipped = 0
ORDER BY s.name, v.name;
```

#### 4.5. Foreign Keys (Relacionamentos)

```sql
-- Listar relacionamentos
SELECT 
    fk.name AS ForeignKeyName,
    OBJECT_NAME(fk.parent_object_id) AS TableName,
    COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS ColumnName,
    OBJECT_NAME(fk.referenced_object_id) AS ReferencedTableName,
    COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) AS ReferencedColumnName
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
ORDER BY TableName, ColumnName;
```

---

### Passo 5: Scaffold do EF Core

#### 5.1. Instalar Ferramenta EF

```powershell
# Instalar dotnet-ef globalmente
dotnet tool install --global dotnet-ef

# Verificar instalação
dotnet ef --version

# Resultado esperado: Entity Framework Core .NET Command-line Tools 8.x.x
```

#### 5.2. Navegar para Projeto Infrastructure

```powershell
cd C:\temp\_ONS_PoC-PDPW\src\PDPW.Infrastructure
```

#### 5.3. Executar Scaffold

```powershell
# Scaffold completo (todas as tabelas)
dotnet ef dbcontext scaffold "Server=localhost;Database=PDPW_DB;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir LegacyModels --context-dir Data --context PDPWLegacyContext --force --no-onconfiguring --data-annotations

# Scaffold de tabelas específicas
dotnet ef dbcontext scaffold "Server=localhost;Database=PDPW_DB;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --output-dir LegacyModels --context PDPWLegacyContext --table Tabela1 --table Tabela2 --force
```

#### 5.4. Verificar Arquivos Gerados

```powershell
# Listar arquivos gerados
Get-ChildItem -Path .\LegacyModels -Recurse
Get-ChildItem -Path .\Data -Filter "*LegacyContext.cs"

# Resultado esperado:
# LegacyModels/
#   ??? Entidade1.cs
#   ??? Entidade2.cs
#   ??? ...
# Data/
#   ??? PDPWLegacyContext.cs
```

---

### Passo 6: Atualizar appsettings.json

#### 6.1. Editar appsettings.Development.json

```powershell
# Abrir arquivo
code C:\temp\_ONS_PoC-PDPW\src\PDPW.API\appsettings.Development.json
```

#### 6.2. Adicionar Connection String

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PDPW_DB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "UseInMemoryDatabase": false,
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Information",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

#### 6.3. Testar Conexão

```powershell
# Voltar para raiz do projeto
cd C:\temp\_ONS_PoC-PDPW

# Executar API
dotnet run --project src\PDPW.API\PDPW.API.csproj

# Abrir navegador em: https://localhost:65417/swagger
```

---

### Passo 7: Documentar Descobertas

#### 7.1. Criar Documento de Tabelas

```markdown
# C:\temp\_ONS_PoC-PDPW\database\docs\tables.md

# Tabelas do Banco PDPW_DB

## Análise Inicial

**Total de Tabelas:** [número]  
**Schema Principal:** [dbo / outro]  
**Data da Análise:** 17/12/2025

## Tabelas por Ordem de Importância

### 1. [Nome da Tabela Principal]
- **Registros:** [número]
- **Descrição:** [o que armazena]
- **Uso:** [onde é usada]

### 2. [Outra Tabela]
- ...

## Tabelas Detalhadas

### Tabela: [Nome]

**Colunas:**
| Coluna | Tipo | Nullable | PK | FK | Descrição |
|--------|------|----------|----|----|-----------|
| ... | ... | ... | ... | ... | ... |

**Relacionamentos:**
- FK para [Tabela]

**Observações:**
- Esta tabela é usada para...
```

#### 7.2. Mapear Funcionalidades

```markdown
# C:\temp\_ONS_PoC-PDPW\database\docs\functionalities.md

# Funcionalidades Identificadas

## 1. [Nome da Funcionalidade]

**Tabelas Envolvidas:**
- Tabela1
- Tabela2

**Fluxo:**
1. Usuário faz X
2. Sistema processa Y
3. Resultado Z

**Prioridade:** Alta / Média / Baixa
**Complexidade:** Alta / Média / Baixa
```

---

## ? Checklist Completo

### Pré-Restauração

- [x] Backup disponível em: `C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak`
- [ ] SQL Server instalado e rodando
- [ ] PowerShell aberto como administrador
- [ ] Espaço em disco suficiente (mínimo 5 GB)

### Durante Restauração

- [ ] Script executado sem erros
- [ ] Integridade verificada
- [ ] Estatísticas atualizadas
- [ ] Banco aparece no SSMS

### Pós-Restauração

- [ ] Tabelas exploradas
- [ ] Quantidade de registros documentada
- [ ] Stored procedures identificadas
- [ ] Relacionamentos mapeados
- [ ] Scaffold do EF Core executado
- [ ] Entidades geradas revisadas
- [ ] Connection string atualizada
- [ ] API testada com banco real

### Documentação

- [ ] Tabelas documentadas
- [ ] Funcionalidades mapeadas
- [ ] Fluxos críticos identificados
- [ ] Prioridades definidas

---

## ?? Troubleshooting

### Erro: "Cannot open backup device"

```powershell
# Verificar se arquivo existe
Test-Path "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak"

# Dar permissão ao SQL Server
icacls "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak" /grant "NT SERVICE\MSSQLSERVER:F"
```

### Erro: "SQL Server not found"

```powershell
# Verificar instâncias do SQL Server
Get-Service | Where-Object {$_.Name -like "*SQL*"}

# Se MSSQL$SQLEXPRESS, usar:
.\database\restore-database.ps1 -ServerInstance "localhost\SQLEXPRESS" -BackupPath "..." -DatabaseName "PDPW_DB"
```

### Erro: "Database already exists"

```sql
-- Dropar banco manualmente
USE master;
ALTER DATABASE [PDPW_DB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [PDPW_DB];
```

---

## ?? Próximos Passos

Após completar todos os passos:

1. **Analisar código legado** (repositório pdpw_act)
2. **Mapear fluxos** entre UI e banco de dados
3. **Selecionar vertical slice** (funcionalidade para migrar primeiro)
4. **Iniciar frontend React**
5. **Implementar primeiro fluxo** (backend + frontend)

---

## ?? Suporte

**Dúvidas?** Consulte:
- `database/backups/README.md` - Documentação completa
- `database/QUICK_START.md` - Guia rápido
- `docs/MIGRATION_PLAN.md` - Plano de migração

**Desenvolvedor:** Willian Charantola Bulhoes  
**GitHub:** https://github.com/wbulhoes/ONS_PoC-PDPW

---

**Última atualização:** 17/12/2025  
**Status:** ? Pronto para executar!
