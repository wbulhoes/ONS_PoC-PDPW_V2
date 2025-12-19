# ============================================
# SCRIPT DE ANÁLISE DO BANCO LEGADO
# ============================================
# Arquivo: Analyze-LegacyDatabase.ps1
# Descrição: Analisa a estrutura e dados do banco legado restaurado
# ============================================

param(
    [string]$ServerInstance = "localhost\SQLEXPRESS",
    [string]$DatabaseName = "PDPW_Legacy",
    [string]$OutputPath = "C:\temp\_ONS_PoC-PDPW\docs\legacy_analysis"
)

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  ANÁLISE DO BANCO LEGADO" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Criar diretório de saída
if (-not (Test-Path $OutputPath)) {
    New-Item -Path $OutputPath -ItemType Directory | Out-Null
    Write-Host "?? Diretório criado: $OutputPath" -ForegroundColor Green
}

# 1. Lista completa de tabelas
Write-Host "1. Exportando lista de tabelas..." -ForegroundColor Yellow
$tablesQuery = @"
USE [$DatabaseName];
SELECT 
    TABLE_SCHEMA,
    TABLE_NAME,
    TABLE_TYPE
FROM INFORMATION_SCHEMA.TABLES
ORDER BY TABLE_SCHEMA, TABLE_NAME;
"@
$tablesFile = "$OutputPath\01_tables_list.txt"
sqlcmd -S $ServerInstance -E -Q $tablesQuery -o $tablesFile
Write-Host "   ? Exportado: $tablesFile" -ForegroundColor Green

# 2. Contagem de registros por tabela
Write-Host "2. Contando registros por tabela..." -ForegroundColor Yellow
$rowCountsQuery = @"
USE [$DatabaseName];
SELECT 
    t.NAME AS TableName,
    SUM(p.rows) AS RowCounts,
    SUM(a.total_pages) * 8 / 1024 AS SizeMB
FROM sys.tables t
INNER JOIN sys.partitions p ON t.object_id = p.OBJECT_ID
INNER JOIN sys.allocation_units a ON p.partition_id = a.container_id
WHERE p.index_id < 2
GROUP BY t.NAME
ORDER BY RowCounts DESC;
"@
$rowCountsFile = "$OutputPath\02_row_counts.txt"
sqlcmd -S $ServerInstance -E -Q $rowCountsQuery -o $rowCountsFile
Write-Host "   ? Exportado: $rowCountsFile" -ForegroundColor Green

# 3. Estrutura das principais tabelas (baseado em nomes comuns)
Write-Host "3. Analisando estrutura das principais tabelas..." -ForegroundColor Yellow

$commonTables = @(
    "TB_USINA", "USINA", "Usinas",
    "TB_EMPRESA", "EMPRESA", "Empresas",
    "TB_TIPO_USINA", "TIPO_USINA", "TiposUsina",
    "TB_SEMANA_PMO", "SEMANA_PMO", "SemanasPMO",
    "TB_EQUIPE", "EQUIPE", "EquipesPDP",
    "TB_ARQUIVO_DADGER", "ARQUIVO_DADGER",
    "TB_USUARIO", "USUARIO", "Usuarios"
)

foreach ($tableName in $commonTables) {
    $checkTableQuery = @"
USE [$DatabaseName];
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '$tableName')
    SELECT 1 AS TableExists
ELSE
    SELECT 0 AS TableExists;
"@
    
    $exists = sqlcmd -S $ServerInstance -E -Q $checkTableQuery -h -1
    if ($exists -eq 1) {
        Write-Host "   ?? Analisando: $tableName" -ForegroundColor Cyan
        
        $structureQuery = @"
USE [$DatabaseName];
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = '$tableName'
ORDER BY ORDINAL_POSITION;
"@
        $structureFile = "$OutputPath\03_structure_$tableName.txt"
        sqlcmd -S $ServerInstance -E -Q $structureQuery -o $structureFile
        
        # Amostra de dados (top 5)
        $sampleQuery = "USE [$DatabaseName]; SELECT TOP 5 * FROM [$tableName];"
        $sampleFile = "$OutputPath\04_sample_$tableName.txt"
        sqlcmd -S $ServerInstance -E -Q $sampleQuery -o $sampleFile -s"|" -W
        
        Write-Host "      ? Estrutura e amostra exportadas" -ForegroundColor Green
    }
}

# 4. Relacionamentos (Foreign Keys)
Write-Host ""
Write-Host "4. Analisando relacionamentos (FKs)..." -ForegroundColor Yellow
$fkQuery = @"
USE [$DatabaseName];
SELECT 
    FK.name AS ForeignKeyName,
    TP.name AS ParentTable,
    CP.name AS ParentColumn,
    TR.name AS ReferencedTable,
    CR.name AS ReferencedColumn
FROM sys.foreign_keys FK
INNER JOIN sys.tables TP ON FK.parent_object_id = TP.object_id
INNER JOIN sys.tables TR ON FK.referenced_object_id = TR.object_id
INNER JOIN sys.foreign_key_columns FKC ON FKC.constraint_object_id = FK.object_id
INNER JOIN sys.columns CP ON FKC.parent_column_id = CP.column_id AND FKC.parent_object_id = CP.object_id
INNER JOIN sys.columns CR ON FKC.referenced_column_id = CR.column_id AND FKC.referenced_object_id = CR.object_id
ORDER BY ParentTable, FK.name;
"@
$fkFile = "$OutputPath\05_foreign_keys.txt"
sqlcmd -S $ServerInstance -E -Q $fkQuery -o $fkFile
Write-Host "   ? Exportado: $fkFile" -ForegroundColor Green

# 5. Índices
Write-Host "5. Analisando índices..." -ForegroundColor Yellow
$indexQuery = @"
USE [$DatabaseName];
SELECT 
    OBJECT_NAME(i.object_id) AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType,
    COL_NAME(ic.object_id, ic.column_id) AS ColumnName
FROM sys.indexes i
INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
WHERE i.type > 0
ORDER BY TableName, IndexName, ic.key_ordinal;
"@
$indexFile = "$OutputPath\06_indexes.txt"
sqlcmd -S $ServerInstance -E -Q $indexQuery -o $indexFile
Write-Host "   ? Exportado: $indexFile" -ForegroundColor Green

# 6. Constraints (Unique, Check, Default)
Write-Host "6. Analisando constraints..." -ForegroundColor Yellow
$constraintsQuery = @"
USE [$DatabaseName];
SELECT 
    OBJECT_NAME(object_id) AS TableName,
    name AS ConstraintName,
    type_desc AS ConstraintType,
    definition
FROM sys.check_constraints
UNION ALL
SELECT 
    OBJECT_NAME(parent_object_id) AS TableName,
    name AS ConstraintName,
    'DEFAULT' AS ConstraintType,
    definition
FROM sys.default_constraints
UNION ALL
SELECT 
    OBJECT_NAME(parent_object_id) AS TableName,
    name AS ConstraintName,
    type_desc AS ConstraintType,
    NULL AS definition
FROM sys.key_constraints
WHERE type = 'UQ'
ORDER BY TableName, ConstraintName;
"@
$constraintsFile = "$OutputPath\07_constraints.txt"
sqlcmd -S $ServerInstance -E -Q $constraintsQuery -o $constraintsFile
Write-Host "   ? Exportado: $constraintsFile" -ForegroundColor Green

# 7. Criar resumo em Markdown
Write-Host ""
Write-Host "7. Gerando resumo em Markdown..." -ForegroundColor Yellow

$summaryContent = @"
# ?? ANÁLISE DO BANCO LEGADO - PDPW

**Banco**: $DatabaseName  
**Servidor**: $ServerInstance  
**Data da Análise**: $(Get-Date -Format "dd/MM/yyyy HH:mm:ss")

---

## ?? Arquivos Gerados

"@

Get-ChildItem $OutputPath -File | ForEach-Object {
    $summaryContent += "- ? ``$($_.Name)``  `n"
}

$summaryContent += @"

---

## ?? TABELAS ENCONTRADAS

"@

# Ler lista de tabelas do arquivo gerado
$tablesContent = Get-Content $tablesFile | Select-Object -Skip 2 | Where-Object { $_.Trim() -ne "" }
$tableCount = ($tablesContent | Measure-Object).Count - 1

$summaryContent += "**Total de Tabelas**: $tableCount  `n`n"

# Top 10 tabelas por registros
$summaryContent += "### Top 10 Tabelas (por quantidade de registros)`n`n"
$summaryContent += "| Tabela | Registros | Tamanho (MB) |`n"
$summaryContent += "|--------|-----------|--------------|`n"

$rowCountsContent = Get-Content $rowCountsFile | Select-Object -Skip 2 | Select-Object -First 10 | Where-Object { $_.Trim() -ne "" }
foreach ($line in $rowCountsContent) {
    $parts = $line -split '\s+'
    if ($parts.Length -ge 3) {
        $summaryContent += "| $($parts[0]) | $($parts[1]) | $($parts[2]) |`n"
    }
}

$summaryContent += "`n---`n`n"

# Estruturas encontradas
$summaryContent += "## ?? ESTRUTURAS ANALISADAS`n`n"
$structureFiles = Get-ChildItem "$OutputPath\03_structure_*.txt"
foreach ($file in $structureFiles) {
    $tableName = $file.Name -replace '03_structure_', '' -replace '.txt', ''
    $summaryContent += "### ? $tableName`n`n"
    $summaryContent += "Arquivo: ``$($file.Name)```n`n"
}

$summaryContent += "`n---`n`n"

$summaryContent += @"
## ?? PRÓXIMOS PASSOS

1. ? Banco restaurado e analisado
2. ? Mapear tabelas Legado ? POC
3. ? Criar scripts de migração de dados
4. ? Validar qualidade dos dados
5. ? Popular banco da POC com dados reais

---

**Analista**: GitHub Copilot  
**Projeto**: PDPW PoC - Migração de Dados Legados
"@

$summaryFile = "$OutputPath\00_RESUMO_ANALISE.md"
$summaryContent | Out-File -FilePath $summaryFile -Encoding UTF8
Write-Host "   ? Resumo gerado: $summaryFile" -ForegroundColor Green

# 8. Finalização
Write-Host ""
Write-Host "============================================" -ForegroundColor Green
Write-Host "  ANÁLISE CONCLUÍDA!" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Green
Write-Host ""
Write-Host "?? Arquivos gerados em: $OutputPath" -ForegroundColor Cyan
Write-Host "?? Resumo principal: 00_RESUMO_ANALISE.md" -ForegroundColor Cyan
Write-Host ""
Write-Host "?? Para visualizar o resumo:" -ForegroundColor Yellow
Write-Host "   code `"$summaryFile`"" -ForegroundColor White
Write-Host ""
