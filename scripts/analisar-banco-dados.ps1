# Script de Análise Profunda do Banco de Dados PDPw
# Valida estrutura, relacionamentos, índices e integridade

param(
    [string]$ConnectionString = "Server=.\SQLEXPRESS;Database=PDPW_DB;User Id=sa;Password=Pdpw@2024!Strong;TrustServerCertificate=True;",
    [switch]$ExportReport
)

Write-Host "🗄️  ANÁLISE PROFUNDA DO BANCO DE DADOS PDPw" -ForegroundColor Cyan
Write-Host "=========================================`n" -ForegroundColor Cyan

# Carregar assembly do SQL Client
Add-Type -AssemblyName "System.Data"

$results = @{
    Tabelas = @()
    ForeignKeys = @()
    Indices = @()
    Constraints = @()
    Triggers = @()
    Stats = @{}
    Issues = @()
}

try {
    $connection = New-Object System.Data.SqlClient.SqlConnection($ConnectionString)
    $connection.Open()
    
    Write-Host "✅ Conexão estabelecida com sucesso!`n" -ForegroundColor Green

    # ============================================================================
    # 1. ANÁLISE DE TABELAS
    # ============================================================================
    
    Write-Host "📊 [1/6] Analisando Tabelas..." -ForegroundColor Yellow
    
    $sqlTabelas = @"
SELECT 
    t.TABLE_NAME as Tabela,
    (SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS c WHERE c.TABLE_NAME = t.TABLE_NAME) as TotalColunas,
    (SELECT COUNT(*) 
     FROM sys.foreign_keys fk 
     INNER JOIN sys.tables tb ON fk.parent_object_id = tb.object_id 
     WHERE tb.name = t.TABLE_NAME) as TotalFKs
FROM INFORMATION_SCHEMA.TABLES t
WHERE t.TABLE_TYPE = 'BASE TABLE'
  AND t.TABLE_SCHEMA = 'dbo'
ORDER BY t.TABLE_NAME
"@

    $cmd = $connection.CreateCommand()
    $cmd.CommandText = $sqlTabelas
    $reader = $cmd.ExecuteReader()
    
    while ($reader.Read()) {
        $results.Tabelas += @{
            Nome = $reader["Tabela"]
            Colunas = $reader["TotalColunas"]
            FKs = $reader["TotalFKs"]
        }
    }
    $reader.Close()
    
    $results.Stats.TotalTabelas = $results.Tabelas.Count
    Write-Host "  Total de Tabelas: $($results.Stats.TotalTabelas)" -ForegroundColor White

    # ============================================================================
    # 2. ANÁLISE DE FOREIGN KEYS
    # ============================================================================
    
    Write-Host "`n📊 [2/6] Analisando Foreign Keys..." -ForegroundColor Yellow
    
    $sqlFKs = @"
SELECT 
    fk.name AS FK_Name,
    OBJECT_NAME(fk.parent_object_id) AS Parent_Table,
    COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS Parent_Column,
    OBJECT_NAME(fk.referenced_object_id) AS Referenced_Table,
    COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) AS Referenced_Column,
    CASE fk.delete_referential_action
        WHEN 0 THEN 'NO ACTION'
        WHEN 1 THEN 'CASCADE'
        WHEN 2 THEN 'SET NULL'
        WHEN 3 THEN 'SET DEFAULT'
    END AS OnDelete,
    CASE fk.update_referential_action
        WHEN 0 THEN 'NO ACTION'
        WHEN 1 THEN 'CASCADE'
        WHEN 2 THEN 'SET NULL'
        WHEN 3 THEN 'SET DEFAULT'
    END AS OnUpdate
FROM sys.foreign_keys AS fk
INNER JOIN sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
ORDER BY Parent_Table, FK_Name
"@

    $cmd.CommandText = $sqlFKs
    $reader = $cmd.ExecuteReader()
    
    while ($reader.Read()) {
        $results.ForeignKeys += @{
            Nome = $reader["FK_Name"]
            TabelaPai = $reader["Parent_Table"]
            ColunaPai = $reader["Parent_Column"]
            TabelaReferenciada = $reader["Referenced_Table"]
            ColunaReferenciada = $reader["Referenced_Column"]
            OnDelete = $reader["OnDelete"]
            OnUpdate = $reader["OnUpdate"]
        }
    }
    $reader.Close()
    
    $results.Stats.TotalFKs = $results.ForeignKeys.Count
    Write-Host "  Total de Foreign Keys: $($results.Stats.TotalFKs)" -ForegroundColor White

    # ============================================================================
    # 3. ANÁLISE DE ÍNDICES
    # ============================================================================
    
    Write-Host "`n📊 [3/6] Analisando Índices..." -ForegroundColor Yellow
    
    $sqlIndices = @"
SELECT 
    OBJECT_NAME(i.object_id) AS Table_Name,
    i.name AS Index_Name,
    i.type_desc AS Index_Type,
    i.is_unique AS Is_Unique,
    STRING_AGG(c.name, ', ') AS Columns
FROM sys.indexes i
INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
WHERE i.type > 0  -- Exclui heaps
  AND OBJECT_SCHEMA_NAME(i.object_id) = 'dbo'
GROUP BY OBJECT_NAME(i.object_id), i.name, i.type_desc, i.is_unique
ORDER BY Table_Name, Index_Name
"@

    $cmd.CommandText = $sqlIndices
    $reader = $cmd.ExecuteReader()
    
    while ($reader.Read()) {
        $results.Indices += @{
            Tabela = $reader["Table_Name"]
            Nome = $reader["Index_Name"]
            Tipo = $reader["Index_Type"]
            Unico = $reader["Is_Unique"]
            Colunas = $reader["Columns"]
        }
    }
    $reader.Close()
    
    $results.Stats.TotalIndices = $results.Indices.Count
    Write-Host "  Total de Índices: $($results.Stats.TotalIndices)" -ForegroundColor White

    # ============================================================================
    # 4. ANÁLISE DE CONSTRAINTS
    # ============================================================================
    
    Write-Host "`n📊 [4/6] Analisando Constraints..." -ForegroundColor Yellow
    
    $sqlConstraints = @"
SELECT 
    OBJECT_NAME(ccu.constraint_object_id) AS Constraint_Name,
    t.name AS Table_Name,
    c.name AS Column_Name,
    CASE 
        WHEN con.type = 'D' THEN 'DEFAULT'
        WHEN con.type = 'C' THEN 'CHECK'
        WHEN con.type = 'UQ' THEN 'UNIQUE'
        WHEN con.type = 'PK' THEN 'PRIMARY KEY'
        ELSE con.type
    END AS Constraint_Type
FROM sys.key_constraints con
INNER JOIN sys.index_columns ccu ON con.parent_object_id = ccu.object_id AND con.unique_index_id = ccu.index_id
INNER JOIN sys.tables t ON con.parent_object_id = t.object_id
INNER JOIN sys.columns c ON ccu.object_id = c.object_id AND ccu.column_id = c.column_id
WHERE SCHEMA_NAME(t.schema_id) = 'dbo'
ORDER BY Table_Name, Constraint_Name
"@

    $cmd.CommandText = $sqlConstraints
    $reader = $cmd.ExecuteReader()
    
    while ($reader.Read()) {
        $results.Constraints += @{
            Nome = $reader["Constraint_Name"]
            Tabela = $reader["Table_Name"]
            Coluna = $reader["Column_Name"]
            Tipo = $reader["Constraint_Type"]
        }
    }
    $reader.Close()
    
    $results.Stats.TotalConstraints = $results.Constraints.Count
    Write-Host "  Total de Constraints: $($results.Stats.TotalConstraints)" -ForegroundColor White

    # ============================================================================
    # 5. CONTAGEM DE REGISTROS
    # ============================================================================
    
    Write-Host "`n📊 [5/6] Contando Registros..." -ForegroundColor Yellow
    
    foreach ($tabela in $results.Tabelas) {
        $sqlCount = "SELECT COUNT(*) FROM [$($tabela.Nome)]"
        $cmd.CommandText = $sqlCount
        
        try {
            $count = [int]$cmd.ExecuteScalar()
            $tabela.Registros = $count
        } catch {
            $tabela.Registros = 0
        }
    }
    
    $results.Stats.TotalRegistros = ($results.Tabelas | Measure-Object -Property Registros -Sum).Sum
    Write-Host "  Total de Registros: $($results.Stats.TotalRegistros)" -ForegroundColor White

    # ============================================================================
    # 6. VALIDAÇÕES E ISSUES
    # ============================================================================
    
    Write-Host "`n📊 [6/6] Validando Integridade..." -ForegroundColor Yellow
    
    # Verificar tabelas sem PK
    $tabelasSemPK = $results.Tabelas | Where-Object { 
        -not ($results.Constraints | Where-Object { $_.Tabela -eq $_.Nome -and $_.Tipo -eq 'PRIMARY KEY' })
    }
    
    if ($tabelasSemPK) {
        foreach ($tab in $tabelasSemPK) {
            $results.Issues += "⚠️  Tabela '$($tab.Nome)' sem PRIMARY KEY"
        }
    }
    
    # Verificar FKs sem índice
    foreach ($fk in $results.ForeignKeys) {
        $temIndice = $results.Indices | Where-Object {
            $_.Tabela -eq $fk.TabelaPai -and $_.Colunas -match $fk.ColunaPai
        }
        
        if (-not $temIndice) {
            $results.Issues += "⚠️  FK '$($fk.Nome)' sem índice em '$($fk.TabelaPai).$($fk.ColunaPai)'"
        }
    }
    
    Write-Host "  Issues Encontrados: $($results.Issues.Count)" -ForegroundColor $(if ($results.Issues.Count -gt 0) { "Yellow" } else { "Green" })

} catch {
    Write-Host "❌ Erro ao analisar banco: $_" -ForegroundColor Red
} finally {
    if ($connection.State -eq 'Open') {
        $connection.Close()
    }
}

# ============================================================================
# GERAR RELATÓRIO
# ============================================================================

Write-Host "`n`n📊 RELATÓRIO DE ANÁLISE DO BANCO" -ForegroundColor Cyan
Write-Host "================================`n" -ForegroundColor Cyan

Write-Host "📈 ESTATÍSTICAS GERAIS:" -ForegroundColor White
Write-Host "  Tabelas:       $($results.Stats.TotalTabelas)" -ForegroundColor Gray
Write-Host "  Registros:     $($results.Stats.TotalRegistros)" -ForegroundColor Gray
Write-Host "  Foreign Keys:  $($results.Stats.TotalFKs)" -ForegroundColor Gray
Write-Host "  Índices:       $($results.Stats.TotalIndices)" -ForegroundColor Gray
Write-Host "  Constraints:   $($results.Stats.TotalConstraints)" -ForegroundColor Gray

Write-Host "`n🗃️  TOP 10 TABELAS (por registros):" -ForegroundColor White
$results.Tabelas | Sort-Object -Property Registros -Descending | Select-Object -First 10 | ForEach-Object {
    Write-Host "  $($_.Nome.PadRight(30)) - $($_.Registros) registros" -ForegroundColor Gray
}

Write-Host "`n🔗 RELACIONAMENTOS:" -ForegroundColor White
$gruposFK = $results.ForeignKeys | Group-Object -Property TabelaPai | Sort-Object Count -Descending | Select-Object -First 5
foreach ($grupo in $gruposFK) {
    Write-Host "  $($grupo.Name): $($grupo.Count) relacionamentos" -ForegroundColor Gray
}

if ($results.Issues.Count -gt 0) {
    Write-Host "`n⚠️  ISSUES IDENTIFICADOS:" -ForegroundColor Yellow
    foreach ($issue in $results.Issues) {
        Write-Host "  $issue" -ForegroundColor Yellow
    }
} else {
    Write-Host "`n✅ Nenhum issue identificado!" -ForegroundColor Green
}

# Exportar relatório se solicitado
if ($ExportReport) {
    $reportPath = "docs/banco-dados/ANALISE_BD_$(Get-Date -Format 'yyyy-MM-dd-HHmm').md"
    
    $reportContent = @"
# 🗄️ ANÁLISE DO BANCO DE DADOS PDPw

**Data**: $(Get-Date -Format 'dd/MM/yyyy HH:mm')  
**Banco**: PDPW_DB

---

## 📊 ESTATÍSTICAS GERAIS

| Métrica | Valor |
|---------|-------|
| **Tabelas** | $($results.Stats.TotalTabelas) |
| **Registros** | $($results.Stats.TotalRegistros) |
| **Foreign Keys** | $($results.Stats.TotalFKs) |
| **Índices** | $($results.Stats.TotalIndices) |
| **Constraints** | $($results.Stats.TotalConstraints) |

---

## 🗃️ TABELAS

| Tabela | Colunas | Registros | FKs |
|--------|---------|-----------|-----|
$($results.Tabelas | Sort-Object Nome | ForEach-Object { "| $($_.Nome) | $($_.Colunas) | $($_.Registros) | $($_.FKs) |" })

---

## 🔗 FOREIGN KEYS

| FK Name | Tabela Pai | → | Tabela Referenciada | OnDelete |
|---------|------------|---|---------------------|----------|
$($results.ForeignKeys | ForEach-Object { "| $($_.Nome) | $($_.TabelaPai).$($_.ColunaPai) | → | $($_.TabelaReferenciada).$($_.ColunaReferenciada) | $($_.OnDelete) |" })

---

## 🔍 ÍNDICES

| Tabela | Índice | Tipo | Colunas |
|--------|--------|------|---------|
$($results.Indices | ForEach-Object { "| $($_.Tabela) | $($_.Nome) | $($_.Tipo) | $($_.Colunas) |" })

---

## ⚠️ ISSUES

$($results.Issues | ForEach-Object { "- $_" })

---

**Gerado por**: scripts/analisar-banco-dados.ps1
"@

    New-Item -ItemType Directory -Path "docs/banco-dados" -Force -ErrorAction SilentlyContinue | Out-Null
    $reportContent | Out-File -FilePath $reportPath -Encoding UTF8
    
    Write-Host "`n📄 Relatório exportado: $reportPath" -ForegroundColor Cyan
}

Write-Host ""
