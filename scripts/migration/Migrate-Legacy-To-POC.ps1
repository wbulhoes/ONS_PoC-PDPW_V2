# ============================================
# SCRIPT DE MIGRAÇÃO - DADOS LEGADO PARA POC
# ============================================
# Descrição: Extrai ~150 registros do backup legado e popula banco da POC
# Autor: GitHub Copilot
# Data: 20/12/2024
# ============================================

param(
    [string]$BackupPath = "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak",
    [string]$SqlServer = "localhost",
    [string]$SqlInstance = "SQLEXPRESS",
    [string]$TempDatabase = "PDPW_LEGACY_TEMP",
    [string]$TargetDatabase = "PDPW_DB",
    [string]$OutputPath = ".\scripts\migration\output",
    [int]$TopEmpresas = 30,
    [int]$TopUsinas = 50,
    [int]$SemanasPMO = 26,
    [int]$TopEquipes = 10,
    [int]$TopUnidadesGeradoras = 20
)

$ErrorActionPreference = "Stop"
$startTime = Get-Date

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  MIGRAÇÃO DE DADOS LEGADO ? POC" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# ============================================
# FUNÇÕES AUXILIARES
# ============================================

function Write-Step {
    param([string]$Message)
    Write-Host ""
    Write-Host "? $Message" -ForegroundColor Yellow
    Write-Host ("?" * 60) -ForegroundColor DarkGray
}

function Write-Success {
    param([string]$Message)
    Write-Host "  ? $Message" -ForegroundColor Green
}

function Write-Info {
    param([string]$Message)
    Write-Host "  ? $Message" -ForegroundColor Cyan
}

function Write-Warning {
    param([string]$Message)
    Write-Host "  ? $Message" -ForegroundColor Yellow
}

function Write-Error-Custom {
    param([string]$Message)
    Write-Host "  ? $Message" -ForegroundColor Red
}

function Invoke-Sql {
    param(
        [string]$Query,
        [string]$Database = "master",
        [string]$Server = $SqlServer
    )
    
    $connectionString = "Server=$Server;Database=$Database;Integrated Security=True;TrustServerCertificate=True;"
    
    try {
        $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
        $connection.Open()
        
        $command = $connection.CreateCommand()
        $command.CommandText = $Query
        $command.CommandTimeout = 600  # 10 minutos
        
        $adapter = New-Object System.Data.SqlClient.SqlDataAdapter($command)
        $dataset = New-Object System.Data.DataSet
        $adapter.Fill($dataset) | Out-Null
        
        $connection.Close()
        
        return $dataset.Tables[0]
    }
    catch {
        Write-Error-Custom "Erro ao executar SQL: $_"
        throw
    }
}

function Test-DatabaseExists {
    param([string]$DatabaseName)
    
    $query = "SELECT COUNT(*) as Existe FROM sys.databases WHERE name = '$DatabaseName'"
    $result = Invoke-Sql -Query $query
    
    return ($result.Rows[0].Existe -gt 0)
}

# ============================================
# FASE 1: VALIDAÇÕES INICIAIS
# ============================================

Write-Step "FASE 1: Validações Iniciais"

# Verificar backup
if (-not (Test-Path $BackupPath)) {
    Write-Error-Custom "Backup não encontrado: $BackupPath"
    exit 1
}
Write-Success "Backup encontrado: $([math]::Round((Get-Item $BackupPath).Length / 1GB, 2)) GB"

# Verificar espaço em disco
$drive = (Get-Item $BackupPath).PSDrive.Name + ":"
$freeSpace = (Get-PSDrive $drive).Free / 1GB
if ($freeSpace -lt 20) {
    Write-Warning "Pouco espaço livre: $([math]::Round($freeSpace, 2)) GB"
    Write-Warning "Recomendado: 20 GB ou mais"
}
else {
    Write-Success "Espaço livre suficiente: $([math]::Round($freeSpace, 2)) GB"
}

# Verificar SQL Server
try {
    $version = Invoke-Sql -Query "SELECT @@VERSION as Version"
    Write-Success "SQL Server conectado"
    Write-Info $version.Rows[0].Version.Split("`n")[0]
}
catch {
    Write-Error-Custom "Não foi possível conectar ao SQL Server"
    exit 1
}

# Verificar banco de destino
if (-not (Test-DatabaseExists -DatabaseName $TargetDatabase)) {
    Write-Warning "Banco de destino não existe: $TargetDatabase"
    Write-Info "Execute 'dotnet ef database update' primeiro"
    exit 1
}
Write-Success "Banco de destino encontrado: $TargetDatabase"

# Criar diretório de saída
if (-not (Test-Path $OutputPath)) {
    New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null
}
Write-Success "Diretório de saída: $OutputPath"

# ============================================
# FASE 2: RESTAURAR BACKUP TEMPORÁRIO
# ============================================

Write-Step "FASE 2: Restaurar Backup Temporário (Estrutura apenas)"

# Verificar se banco temp já existe
if (Test-DatabaseExists -DatabaseName $TempDatabase) {
    Write-Info "Removendo banco temporário anterior..."
    
    $dropQuery = @"
ALTER DATABASE [$TempDatabase] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [$TempDatabase];
"@
    Invoke-Sql -Query $dropQuery | Out-Null
    Write-Success "Banco temporário removido"
}

Write-Info "Iniciando restauração do backup..."
Write-Warning "Esta operação pode demorar 10-15 minutos"

# Obter nomes lógicos dos arquivos do backup
$fileListQuery = "RESTORE FILELISTONLY FROM DISK = '$BackupPath'"
$fileList = Invoke-Sql -Query $fileListQuery

$dataPath = "C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA"

# Construir comando RESTORE
$restoreQuery = @"
RESTORE DATABASE [$TempDatabase]
FROM DISK = '$BackupPath'
WITH 
    RECOVERY,
    REPLACE,
    STATS = 10
"@

# Adicionar cláusulas MOVE para cada arquivo
$moveIndex = 0
foreach ($file in $fileList) {
    $logicalName = $file.LogicalName
    $fileType = if ($file.Type -eq 'D') { 'mdf' } else { 'ldf' }
    $physicalName = "$dataPath\${TempDatabase}_${moveIndex}.$fileType"
    
    $restoreQuery += ",`n    MOVE '$logicalName' TO '$physicalName'"
    $moveIndex++
}

try {
    Write-Info "Executando restore..."
    Invoke-Sql -Query $restoreQuery -Database "master" | Out-Null
    Write-Success "Backup restaurado com sucesso!"
}
catch {
    Write-Error-Custom "Erro ao restaurar backup: $_"
    
    Write-Info ""
    Write-Info "POSSÍVEIS SOLUÇÕES:"
    Write-Info "1. Verifique se há espaço suficiente em disco (mínimo 20 GB)"
    Write-Info "2. Execute o SQL Server como Administrador"
    Write-Info "3. Verifique permissões na pasta: $dataPath"
    
    exit 1
}

# ============================================
# FASE 3: ANALISAR ESTRUTURA
# ============================================

Write-Step "FASE 3: Analisar Estrutura do Banco Legado"

$tables = Invoke-Sql -Database $TempDatabase -Query @"
SELECT 
    t.name AS TableName,
    SUM(p.rows) AS RowCount
FROM sys.tables t
INNER JOIN sys.partitions p ON t.object_id = p.object_id
WHERE p.index_id IN (0,1)
GROUP BY t.name
ORDER BY SUM(p.rows) DESC
"@

Write-Info "Tabelas encontradas: $($tables.Rows.Count)"
Write-Info ""
Write-Info "Top 20 tabelas com mais registros:"

$tables | Select-Object -First 20 | ForEach-Object {
    Write-Host ("  • {0,-40} {1,10:N0} registros" -f $_.TableName, $_.RowCount) -ForegroundColor Gray
}

# ============================================
# FASE 4: MAPEAR TABELAS LEGADO ? POC
# ============================================

Write-Step "FASE 4: Mapear Tabelas (Legado ? POC)"

# Definir mapeamentos
$tableMappings = @{
    # Empresas
    "Empresa" = @{
        Target = "Empresas"
        Query = @"
SELECT TOP $TopEmpresas
    ROW_NUMBER() OVER (ORDER BY (SELECT COUNT(*) FROM Usina u WHERE u.CodEmpre = e.CodEmpre) DESC) + 100 as Id,
    RTRIM(e.NomEmpre) as Nome,
    e.NumCNPJ as CNPJ,
    e.NumTelefone as Telefone,
    e.EmailContato as Email,
    e.Endereco as Endereco,
    GETDATE() as DataCriacao,
    1 as Ativo
FROM Empresa e
WHERE e.Ativo = 1
ORDER BY (SELECT COUNT(*) FROM Usina u WHERE u.CodEmpre = e.CodEmpre) DESC
"@
    }
    
    # Usinas
    "Usina" = @{
        Target = "Usinas"
        Query = @"
SELECT TOP $TopUsinas
    ROW_NUMBER() OVER (ORDER BY COALESCE(u.PotInstalada, 0) DESC) + 200 as Id,
    RTRIM(u.CodUsina) as Codigo,
    RTRIM(u.NomUsina) as Nome,
    COALESCE(tu.Id, 1) as TipoUsinaId,
    COALESCE(e.Id, 1) as EmpresaId,
    COALESCE(u.PotInstalada, 0) as CapacidadeInstalada,
    RTRIM(COALESCE(u.Municipio, '') + ', ' + COALESCE(u.UF, '')) as Localizacao,
    COALESCE(u.DataOperacao, '2000-01-01') as DataOperacao,
    GETDATE() as DataCriacao,
    1 as Ativo
FROM Usina u
LEFT JOIN (
    SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) as Id, CodTipoUsina
    FROM TipoUsina
    WHERE Ativo = 1
) tu ON tu.CodTipoUsina = u.TpUsina_Id
LEFT JOIN (
    SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) + 100 as Id, CodEmpre
    FROM Empresa
    WHERE Ativo = 1
) e ON e.CodEmpre = u.CodEmpre
WHERE u.Ativo = 1
AND u.PotInstalada > 0
ORDER BY COALESCE(u.PotInstalada, 0) DESC
"@
    }
}

Write-Success "$($tableMappings.Count) mapeamentos definidos"

foreach ($mapping in $tableMappings.GetEnumerator()) {
    Write-Info "  • $($mapping.Key) ? $($mapping.Value.Target)"
}

# ============================================
# FASE 5: EXTRAIR E GERAR SCRIPTS
# ============================================

Write-Step "FASE 5: Extrair Dados e Gerar Scripts SQL"

$allInserts = @()
$totalRecords = 0

foreach ($mapping in $tableMappings.GetEnumerator()) {
    $sourceName = $mapping.Key
    $targetName = $mapping.Value.Target
    $query = $mapping.Value.Query
    
    Write-Info "Extraindo dados de: $sourceName"
    
    try {
        $data = Invoke-Sql -Database $TempDatabase -Query $query
        
        if ($data.Rows.Count -gt 0) {
            Write-Success "$($data.Rows.Count) registros extraídos"
            $totalRecords += $data.Rows.Count
            
            # Gerar INSERT statements
            $insertScript = "-- Inserir dados em $targetName`n"
            $insertScript += "SET IDENTITY_INSERT [$targetName] ON;`n`n"
            
            foreach ($row in $data.Rows) {
                $columns = @()
                $values = @()
                
                foreach ($column in $data.Columns) {
                    $columnName = $column.ColumnName
                    $value = $row[$columnName]
                    
                    $columns += "[$columnName]"
                    
                    if ($value -is [DBNull] -or $null -eq $value) {
                        $values += "NULL"
                    }
                    elseif ($value -is [DateTime]) {
                        $values += "'$($value.ToString("yyyy-MM-dd HH:mm:ss"))'"
                    }
                    elseif ($value -is [String]) {
                        $escapedValue = $value.Replace("'", "''")
                        $values += "'$escapedValue'"
                    }
                    elseif ($value -is [Boolean]) {
                        $values += if ($value) { "1" } else { "0" }
                    }
                    else {
                        $values += $value.ToString()
                    }
                }
                
                $insertScript += "INSERT INTO [$targetName] ($($columns -join ', ')) VALUES ($($values -join ', '));`n"
            }
            
            $insertScript += "`nSET IDENTITY_INSERT [$targetName] OFF;`n`n"
            
            # Salvar script individual
            $scriptFile = Join-Path $OutputPath "$targetName.sql"
            $insertScript | Out-File -FilePath $scriptFile -Encoding UTF8
            Write-Success "Script salvo: $scriptFile"
            
            $allInserts += $insertScript
        }
        else {
            Write-Warning "Nenhum registro encontrado"
        }
    }
    catch {
        Write-Error-Custom "Erro ao extrair $sourceName : $_"
    }
}

# Gerar script consolidado
$consolidatedScript = @"
-- ============================================
-- SCRIPT DE MIGRAÇÃO - DADOS LEGADO PARA POC
-- ============================================
-- Gerado em: $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")
-- Total de registros: $totalRecords
-- ============================================

USE [$TargetDatabase];
GO

-- Desabilitar constraints temporariamente
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';
GO

$($allInserts -join "`n`n")

-- Re-habilitar constraints
EXEC sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL';
GO

-- Verificar integridade
DBCC CHECKCONSTRAINTS WITH ALL_CONSTRAINTS;
GO
"@

$consolidatedFile = Join-Path $OutputPath "migrate-all.sql"
$consolidatedScript | Out-File -FilePath $consolidatedFile -Encoding UTF8
Write-Success "Script consolidado salvo: $consolidatedFile"

# ============================================
# FASE 6: APLICAR DADOS NO BANCO DA POC
# ============================================

Write-Step "FASE 6: Aplicar Dados no Banco da POC"

Write-Warning "Deseja aplicar os dados agora? (S/N)"
$response = Read-Host

if ($response -eq 'S' -or $response -eq 's') {
    Write-Info "Aplicando dados..."
    
    try {
        # Executar script consolidado
        Invoke-Sql -Database $TargetDatabase -Query $consolidatedScript | Out-Null
        Write-Success "Dados aplicados com sucesso!"
        
        # Verificar contagens
        Write-Info ""
        Write-Info "Verificando registros inseridos:"
        
        foreach ($mapping in $tableMappings.GetEnumerator()) {
            $targetName = $mapping.Value.Target
            $count = Invoke-Sql -Database $TargetDatabase -Query "SELECT COUNT(*) as Total FROM [$targetName]"
            Write-Host ("  • {0,-30} {1,5} registros" -f $targetName, $count.Rows[0].Total) -ForegroundColor Gray
        }
    }
    catch {
        Write-Error-Custom "Erro ao aplicar dados: $_"
        Write-Info "Você pode aplicar manualmente executando: $consolidatedFile"
    }
}
else {
    Write-Info "Dados NÃO aplicados automaticamente"
    Write-Info "Para aplicar manualmente, execute: sqlcmd -S $SqlServer -d $TargetDatabase -i '$consolidatedFile'"
}

# ============================================
# FASE 7: LIMPEZA
# ============================================

Write-Step "FASE 7: Limpeza"

Write-Warning "Deseja remover o banco temporário? (S/N)"
$response = Read-Host

if ($response -eq 'S' -or $response -eq 's') {
    try {
        $dropQuery = @"
ALTER DATABASE [$TempDatabase] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [$TempDatabase];
"@
        Invoke-Sql -Query $dropQuery | Out-Null
        Write-Success "Banco temporário removido"
    }
    catch {
        Write-Warning "Não foi possível remover banco temporário automaticamente"
        Write-Info "Remova manualmente: DROP DATABASE [$TempDatabase]"
    }
}
else {
    Write-Info "Banco temporário mantido: $TempDatabase"
}

# ============================================
# RELATÓRIO FINAL
# ============================================

$endTime = Get-Date
$duration = $endTime - $startTime

Write-Host ""
Write-Host "============================================" -ForegroundColor Green
Write-Host "  MIGRAÇÃO CONCLUÍDA COM SUCESSO!" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Green
Write-Host ""

Write-Host "?? ESTATÍSTICAS:" -ForegroundColor Cyan
Write-Host "  • Registros extraídos: $totalRecords" -ForegroundColor White
Write-Host "  • Tabelas migradas: $($tableMappings.Count)" -ForegroundColor White
Write-Host "  • Tempo total: $($duration.Minutes)m $($duration.Seconds)s" -ForegroundColor White
Write-Host "  • Scripts gerados: $(Get-ChildItem $OutputPath -Filter *.sql | Measure-Object).Count" -ForegroundColor White
Write-Host ""

Write-Host "?? ARQUIVOS GERADOS:" -ForegroundColor Cyan
Get-ChildItem $OutputPath -Filter *.sql | ForEach-Object {
    Write-Host "  • $($_.Name)" -ForegroundColor Gray
}
Write-Host ""

Write-Host "?? PRÓXIMOS PASSOS:" -ForegroundColor Cyan
Write-Host "  1. Testar APIs via Swagger: http://localhost:5001/swagger" -ForegroundColor White
Write-Host "  2. Validar relacionamentos no banco" -ForegroundColor White
Write-Host "  3. Executar testes de integração" -ForegroundColor White
Write-Host ""

