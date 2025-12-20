# ============================================================================
# Script de Extração de Schema do Backup PDPW
# ============================================================================
# Descrição: Restaura temporariamente para extrair schema, depois remove
# Autor: Willian Charantola Bulhoes
# Data: 18/12/2025
# ============================================================================

param(
    [Parameter(Mandatory=$false)]
    [string]$BackupPath = "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak",
    
    [Parameter(Mandatory=$false)]
    [string]$ServerInstance = "localhost\SQLEXPRESS",
    
    [Parameter(Mandatory=$false)]
    [string]$TempDatabaseName = "PDPW_TEMP_SCHEMA",
    
    [Parameter(Mandatory=$false)]
    [string]$OutputDir = "C:\temp\_ONS_PoC-PDPW\database\schema"
)

$ErrorActionPreference = "Stop"

function Write-Header {
    param([string]$Message)
    Write-Host "`n============================================================================" -ForegroundColor Cyan
    Write-Host " $Message" -ForegroundColor Cyan
    Write-Host "============================================================================`n" -ForegroundColor Cyan
}

function Write-Success {
    param([string]$Message)
    Write-Host "? $Message" -ForegroundColor Green
}

function Write-Info {
    param([string]$Message)
    Write-Host "? $Message" -ForegroundColor Yellow
}

function Write-Error-Custom {
    param([string]$Message)
    Write-Host "? $Message" -ForegroundColor Red
}

# ============================================================================
# Criar diretório de saída
# ============================================================================

Write-Header "Extração de Schema PDPW para PoC"

if (-not (Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
    Write-Success "Diretório criado: $OutputDir"
}

# ============================================================================
# Estratégia Alternativa: Extrair informações sem restauração completa
# ============================================================================

Write-Info "Analisando estrutura do backup..."

# Extrair informações de metadados do backup
$query = @"
-- Criar banco temporário vazio para restaurar apenas filegroup PRIMARY com PARTIAL
RESTORE DATABASE [$TempDatabaseName]
FILEGROUP = 'PRIMARY'
FROM DISK = '$BackupPath'
WITH 
    MOVE 'PDP' TO 'C:\temp\PDPW_TEMP.mdf',
    MOVE 'PDP_log' TO 'C:\temp\PDPW_TEMP_log.ldf',
    MOVE 'PDP_log2' TO 'C:\temp\PDPW_TEMP_log2.ldf',
    MOVE 'PDP_log3' TO 'C:\temp\PDPW_TEMP_log3.ldf',
    PARTIAL,
    REPLACE,
    RECOVERY;
"@

try {
    Write-Info "Restaurando parcialmente apenas metadata (PRIMARY filegroup)..."
    Write-Warning "NOTA: Isso pode levar alguns minutos..."
    
    Invoke-Sqlcmd -ServerInstance $ServerInstance -Query $query -QueryTimeout 0 -TrustServerCertificate -ErrorAction Stop
    
    Write-Success "Banco temporário criado com sucesso!"
    
    # ============================================================================
    # Extrair Schema
    # ============================================================================
    
    Write-Header "Extraindo Schema Completo"
    
    # 1. Tabelas
    Write-Info "Extraindo definição de tabelas..."
    $tablesQuery = @"
SELECT 
    SCHEMA_NAME(t.schema_id) AS SchemaName,
    t.name AS TableName,
    c.name AS ColumnName,
    TYPE_NAME(c.user_type_id) AS DataType,
    c.max_length,
    c.precision,
    c.scale,
    c.is_nullable,
    c.is_identity,
    OBJECT_DEFINITION(c.default_object_id) AS DefaultValue
FROM sys.tables t
INNER JOIN sys.columns c ON t.object_id = c.object_id
WHERE t.is_ms_shipped = 0
ORDER BY t.name, c.column_id;
"@
    
    $tables = Invoke-Sqlcmd -ServerInstance $ServerInstance -Database $TempDatabaseName -Query $tablesQuery -TrustServerCertificate
    
    if ($tables) {
        $tables | Export-Csv -Path (Join-Path $OutputDir "tables_structure.csv") -NoTypeInformation -Encoding UTF8
        Write-Success "Estrutura de tabelas exportada: tables_structure.csv ($($tables.Count) colunas)"
    }
    
    # 2. Lista de Tabelas com Contagem
    Write-Info "Extraindo lista de tabelas..."
    $tableListQuery = @"
SELECT 
    SCHEMA_NAME(t.schema_id) AS SchemaName,
    t.name AS TableName,
    t.create_date AS CreateDate,
    t.modify_date AS ModifyDate,
    (SELECT COUNT(*) FROM sys.columns WHERE object_id = t.object_id) AS ColumnCount
FROM sys.tables t
WHERE t.is_ms_shipped = 0
ORDER BY t.name;
"@
    
    $tableList = Invoke-Sqlcmd -ServerInstance $ServerInstance -Database $TempDatabaseName -Query $tableListQuery -TrustServerCertificate
    
    if ($tableList) {
        $tableList | Export-Csv -Path (Join-Path $OutputDir "tables_list.csv") -NoTypeInformation -Encoding UTF8
        Write-Success "Lista de tabelas exportada: tables_list.csv ($($tableList.Count) tabelas)"
    }
    
    # 3. Primary Keys
    Write-Info "Extraindo primary keys..."
    $pkQuery = @"
SELECT 
    SCHEMA_NAME(t.schema_id) AS SchemaName,
    t.name AS TableName,
    i.name AS PrimaryKeyName,
    c.name AS ColumnName,
    ic.key_ordinal AS KeyOrdinal
FROM sys.tables t
INNER JOIN sys.indexes i ON t.object_id = i.object_id AND i.is_primary_key = 1
INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
WHERE t.is_ms_shipped = 0
ORDER BY t.name, ic.key_ordinal;
"@
    
    $pks = Invoke-Sqlcmd -ServerInstance $ServerInstance -Database $TempDatabaseName -Query $pkQuery -TrustServerCertificate
    
    if ($pks) {
        $pks | Export-Csv -Path (Join-Path $OutputDir "primary_keys.csv") -NoTypeInformation -Encoding UTF8
        Write-Success "Primary keys exportadas: primary_keys.csv"
    }
    
    # 4. Foreign Keys
    Write-Info "Extraindo foreign keys..."
    $fkQuery = @"
SELECT 
    OBJECT_SCHEMA_NAME(fk.parent_object_id) AS SchemaName,
    OBJECT_NAME(fk.parent_object_id) AS TableName,
    fk.name AS ForeignKeyName,
    COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS ColumnName,
    OBJECT_SCHEMA_NAME(fk.referenced_object_id) AS ReferencedSchema,
    OBJECT_NAME(fk.referenced_object_id) AS ReferencedTable,
    COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) AS ReferencedColumn,
    fk.delete_referential_action_desc AS DeleteAction,
    fk.update_referential_action_desc AS UpdateAction
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
ORDER BY TableName, ColumnName;
"@
    
    $fks = Invoke-Sqlcmd -ServerInstance $ServerInstance -Database $TempDatabaseName -Query $fkQuery -TrustServerCertificate
    
    if ($fks) {
        $fks | Export-Csv -Path (Join-Path $OutputDir "foreign_keys.csv") -NoTypeInformation -Encoding UTF8
        Write-Success "Foreign keys exportadas: foreign_keys.csv ($($fks.Count) relacionamentos)"
    }
    
    # 5. Indexes
    Write-Info "Extraindo indexes..."
    $indexQuery = @"
SELECT 
    SCHEMA_NAME(t.schema_id) AS SchemaName,
    t.name AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType,
    i.is_unique AS IsUnique,
    c.name AS ColumnName,
    ic.key_ordinal AS KeyOrdinal,
    ic.is_descending_key AS IsDescending
FROM sys.tables t
INNER JOIN sys.indexes i ON t.object_id = i.object_id
INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
WHERE t.is_ms_shipped = 0 AND i.is_primary_key = 0
ORDER BY t.name, i.name, ic.key_ordinal;
"@
    
    $indexes = Invoke-Sqlcmd -ServerInstance $ServerInstance -Database $TempDatabaseName -Query $indexQuery -TrustServerCertificate
    
    if ($indexes) {
        $indexes | Export-Csv -Path (Join-Path $OutputDir "indexes.csv") -NoTypeInformation -Encoding UTF8
        Write-Success "Indexes exportados: indexes.csv"
    }
    
    # 6. Views
    Write-Info "Extraindo views..."
    $viewQuery = @"
SELECT 
    SCHEMA_NAME(v.schema_id) AS SchemaName,
    v.name AS ViewName,
    v.create_date AS CreateDate,
    OBJECT_DEFINITION(v.object_id) AS Definition
FROM sys.views v
WHERE v.is_ms_shipped = 0
ORDER BY v.name;
"@
    
    $views = Invoke-Sqlcmd -ServerInstance $ServerInstance -Database $TempDatabaseName -Query $viewQuery -TrustServerCertificate
    
    if ($views) {
        $views | Export-Csv -Path (Join-Path $OutputDir "views.csv") -NoTypeInformation -Encoding UTF8
        Write-Success "Views exportadas: views.csv ($($views.Count) views)"
    }
    
    # 7. Stored Procedures
    Write-Info "Extraindo stored procedures..."
    $spQuery = @"
SELECT 
    SCHEMA_NAME(p.schema_id) AS SchemaName,
    p.name AS ProcedureName,
    p.create_date AS CreateDate,
    p.modify_date AS ModifyDate,
    OBJECT_DEFINITION(p.object_id) AS Definition
FROM sys.procedures p
WHERE p.is_ms_shipped = 0
ORDER BY p.name;
"@
    
    $sps = Invoke-Sqlcmd -ServerInstance $ServerInstance -Database $TempDatabaseName -Query $spQuery -TrustServerCertificate
    
    if ($sps) {
        $sps | Export-Csv -Path (Join-Path $OutputDir "stored_procedures.csv") -NoTypeInformation -Encoding UTF8
        Write-Success "Stored procedures exportadas: stored_procedures.csv ($($sps.Count) procedures)"
    }
    
    # 8. Functions
    Write-Info "Extraindo functions..."
    $funcQuery = @"
SELECT 
    SCHEMA_NAME(o.schema_id) AS SchemaName,
    o.name AS FunctionName,
    o.type_desc AS FunctionType,
    o.create_date AS CreateDate,
    OBJECT_DEFINITION(o.object_id) AS Definition
FROM sys.objects o
WHERE o.type IN ('FN', 'IF', 'TF') AND o.is_ms_shipped = 0
ORDER BY o.name;
"@
    
    $funcs = Invoke-Sqlcmd -ServerInstance $ServerInstance -Database $TempDatabaseName -Query $funcQuery -TrustServerCertificate
    
    if ($funcs) {
        $funcs | Export-Csv -Path (Join-Path $OutputDir "functions.csv") -NoTypeInformation -Encoding UTF8
        Write-Success "Functions exportadas: functions.csv ($($funcs.Count) functions)"
    }
    
    # ============================================================================
    # Gerar Documentação em Markdown
    # ============================================================================
    
    Write-Header "Gerando Documentação"
    
    $markdown = @"
# Schema do Banco PDPW - Análise para PoC

**Data de Extração:** $(Get-Date -Format "dd/MM/yyyy HH:mm:ss")  
**Banco Original:** PDP  
**SQL Server Version:** 15.0.4430  

---

## ?? Resumo

- **Tabelas:** $($tableList.Count)
- **Views:** $($views.Count)
- **Stored Procedures:** $($sps.Count)
- **Functions:** $($funcs.Count)
- **Foreign Keys:** $($fks.Count)

---

## ?? Tabelas Principais

| Schema | Tabela | Colunas |
|--------|--------|---------|
"@
    
    foreach ($table in $tableList | Select-Object -First 50) {
        $markdown += "`n| $($table.SchemaName) | $($table.TableName) | $($table.ColumnCount) |"
    }
    
    if ($tableList.Count -gt 50) {
        $markdown += "`n| ... | ... | ... |"
        $markdown += "`n`n*Mostrando apenas as primeiras 50 tabelas. Total: $($tableList.Count)*"
    }
    
    $markdown += @"


---

## ?? Relacionamentos (Foreign Keys)

Total de relacionamentos identificados: **$($fks.Count)**

### Principais Relacionamentos

| Tabela | Coluna | Referencia | Coluna Ref |
|--------|--------|------------|------------|
"@
    
    foreach ($fk in $fks | Select-Object -First 30) {
        $markdown += "`n| $($fk.TableName) | $($fk.ColumnName) | $($fk.ReferencedTable) | $($fk.ReferencedColumn) |"
    }
    
    if ($fks.Count -gt 30) {
        $markdown += "`n| ... | ... | ... | ... |"
        $markdown += "`n`n*Mostrando apenas os primeiros 30 relacionamentos. Total: $($fks.Count)*"
    }
    
    $markdown += @"


---

## ?? Arquivos Exportados

Todos os detalhes foram exportados em formato CSV na pasta: ``````$OutputDir``````

- **tables_structure.csv** - Estrutura completa de todas as colunas
- **tables_list.csv** - Lista de todas as tabelas
- **primary_keys.csv** - Primary keys
- **foreign_keys.csv** - Foreign keys (relacionamentos)
- **indexes.csv** - Indexes
- **views.csv** - Views com definições
- **stored_procedures.csv** - Stored procedures com código
- **functions.csv** - Functions com código

---

## ?? Próximos Passos para PoC

### 1. Identificar Tabelas Críticas

Analise a lista de tabelas e identifique as 10-15 tabelas mais importantes para os fluxos da PoC.

**Critérios sugeridos:**
- Tabelas com relacionamentos importantes
- Tabelas que aparecem frequentemente no código VB.NET
- Tabelas de domínio central (Usinas, Arquivos, etc.)

### 2. Selecionar Vertical Slices

Com base nas tabelas identificadas, escolha 2 fluxos completos:

**Sugestões:**
- **Fluxo 1:** Cadastro de Usinas (CRUD completo)
- **Fluxo 2:** Consulta/Importação de Arquivos

### 3. Criar Entidades no EF Core

Use os arquivos CSV para criar as entidades C# manualmente ou com gerador.

### 4. Popular com Dados Sample

Crie dados de exemplo (seed data) para desenvolvimento e testes.

---

## ?? Análise Recomendada

1. Abrir ``````tables_list.csv`````` e ordenar por nome
2. Comparar com código VB.NET em ``````pdpw_act/pdpw/Business/*.vb``````
3. Identificar tabelas referenciadas nos arquivos ``````.aspx``````
4. Mapear relacionamentos usando ``````foreign_keys.csv``````
5. Priorizar tabelas com mais relacionamentos (core do sistema)

---

**Status:** ? Schema extraído com sucesso!  
**Próximo:** Análise e seleção de vertical slices
"@
    
    $markdown | Out-File -FilePath (Join-Path $OutputDir "SCHEMA_ANALYSIS.md") -Encoding UTF8
    Write-Success "Documentação gerada: SCHEMA_ANALYSIS.md"
    
}
catch {
    Write-Error-Custom "Erro durante extração: $_"
}
finally {
    # ============================================================================
    # Limpar banco temporário
    # ============================================================================
    
    Write-Header "Limpeza"
    
    try {
        Write-Info "Removendo banco temporário..."
        $dropQuery = @"
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = '$TempDatabaseName')
BEGIN
    ALTER DATABASE [$TempDatabaseName] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$TempDatabaseName];
END
"@
        Invoke-Sqlcmd -ServerInstance $ServerInstance -Query $dropQuery -TrustServerCertificate
        Write-Success "Banco temporário removido"
        
        # Remover arquivos físicos se existirem
        $files = @(
            "C:\temp\PDPW_TEMP.mdf",
            "C:\temp\PDPW_TEMP_log.ldf",
            "C:\temp\PDPW_TEMP_log2.ldf",
            "C:\temp\PDPW_TEMP_log3.ldf"
        )
        
        foreach ($file in $files) {
            if (Test-Path $file) {
                Remove-Item $file -Force
            }
        }
    }
    catch {
        Write-Warning "Não foi possível remover banco temporário. Remova manualmente se necessário."
    }
}

Write-Header "Extração Concluída!"
Write-Success "Schema extraído com sucesso para: $OutputDir"
Write-Info "Próximo passo: Analisar SCHEMA_ANALYSIS.md e selecionar vertical slices"
