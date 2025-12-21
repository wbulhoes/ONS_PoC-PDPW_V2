# ============================================
# SCRIPT DE EXPANSÃO - 100 REGISTROS REAIS
# ============================================
# Descrição: Extrai dados do backup do cliente e expande base POC para ~100 registros
# Autor: GitHub Copilot
# Data: 20/12/2024
# ============================================

param(
    [string]$BackupPath = "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak",
    [string]$SqlServerLocal = "localhost\SQLEXPRESS",
    [string]$SqlServerDocker = "localhost,1433",
    [string]$TempDatabase = "PDPW_BACKUP_TEMP",
    [string]$TargetDatabase = "PDPW_DB",
    [string]$SaPassword = "Pdpw@2024!Strong",
    [string]$OutputPath = ".\scripts\migration\data"
)

$ErrorActionPreference = "Stop"
$startTime = Get-Date

# ============================================
# FUNÇÕES AUXILIARES
# ============================================

function Write-Step {
    param([string]$Message)
    Write-Host ""
    Write-Host "? $Message" -ForegroundColor Yellow
    Write-Host ("?" * 80) -ForegroundColor DarkGray
}

function Write-Success {
    param([string]$Message)
    Write-Host "  ? $Message" -ForegroundColor Green
}

function Write-Info {
    param([string]$Message)
    Write-Host "  ? $Message" -ForegroundColor Cyan
}

function Write-Warning-Custom {
    param([string]$Message)
    Write-Host "  ? $Message" -ForegroundColor Yellow
}

function Write-Error-Custom {
    param([string]$Message)
    Write-Host "  ? $Message" -ForegroundColor Red
}

function Invoke-SqlLocal {
    param(
        [string]$Query,
        [string]$Database = "master"
    )
    
    $connectionString = "Server=$SqlServerLocal;Database=$Database;Integrated Security=True;TrustServerCertificate=True;"
    
    try {
        $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
        $connection.Open()
        
        $command = $connection.CreateCommand()
        $command.CommandText = $Query
        $command.CommandTimeout = 600
        
        $adapter = New-Object System.Data.SqlClient.SqlDataAdapter($command)
        $dataset = New-Object System.Data.DataSet
        $adapter.Fill($dataset) | Out-Null
        
        $connection.Close()
        
        return $dataset.Tables[0]
    }
    catch {
        Write-Error-Custom "Erro ao executar SQL Local: $_"
        throw
    }
}

function Invoke-SqlDocker {
    param(
        [string]$Query,
        [string]$Database = "master"
    )
    
    $connectionString = "Server=$SqlServerDocker;Database=$Database;User Id=sa;Password=$SaPassword;TrustServerCertificate=True;"
    
    try {
        $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
        $connection.Open()
        
        $command = $connection.CreateCommand()
        $command.CommandText = $Query
        $command.CommandTimeout = 600
        
        $result = $command.ExecuteNonQuery()
        
        $connection.Close()
        
        return $result
    }
    catch {
        Write-Error-Custom "Erro ao executar SQL Docker: $_"
        throw
    }
}

# ============================================
# FASE 1: VALIDAÇÕES
# ============================================

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  EXPANSÃO PARA 100 REGISTROS REAIS" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

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
if ($freeSpace -lt 25) {
    Write-Warning-Custom "Pouco espaço livre: $([math]::Round($freeSpace, 2)) GB"
    Write-Warning-Custom "Recomendado: 25 GB ou mais"
    
    Write-Host ""
    $response = Read-Host "Deseja continuar mesmo assim? (S/N)"
    if ($response -ne 'S' -and $response -ne 's') {
        Write-Info "Operação cancelada pelo usuário"
        exit 0
    }
}
else {
    Write-Success "Espaço livre suficiente: $([math]::Round($freeSpace, 2)) GB"
}

# Verificar SQL Server Local
try {
    $version = Invoke-SqlLocal -Query "SELECT @@VERSION as Version"
    Write-Success "SQL Server Local conectado"
    Write-Info $version.Rows[0].Version.Split("`n")[0]
}
catch {
    Write-Error-Custom "Não foi possível conectar ao SQL Server Local ($SqlServerLocal)"
    exit 1
}

# Verificar SQL Server Docker
try {
    $testQuery = "SELECT 1 as Test"
    $connectionString = "Server=$SqlServerDocker;Database=master;User Id=sa;Password=$SaPassword;TrustServerCertificate=True;"
    $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $connection.Open()
    $connection.Close()
    Write-Success "SQL Server Docker conectado"
}
catch {
    Write-Error-Custom "Não foi possível conectar ao SQL Server Docker"
    Write-Info "Certifique-se de que o container está rodando: docker ps"
    exit 1
}

# Criar diretório de saída
if (-not (Test-Path $OutputPath)) {
    New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null
}
Write-Success "Diretório de saída: $OutputPath"

# ============================================
# FASE 2: RESTAURAR BACKUP TEMPORÁRIO
# ============================================

Write-Step "FASE 2: Restaurar Backup Temporário (SQL Server Local)"

# Verificar se banco temp já existe
try {
    $checkDb = Invoke-SqlLocal -Query "SELECT COUNT(*) as Existe FROM sys.databases WHERE name = '$TempDatabase'"
    
    if ($checkDb.Rows[0].Existe -gt 0) {
        Write-Info "Removendo banco temporário anterior..."
        
        $dropQuery = @"
ALTER DATABASE [$TempDatabase] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [$TempDatabase];
"@
        Invoke-SqlLocal -Query $dropQuery | Out-Null
        Write-Success "Banco temporário removido"
    }
}
catch {
    Write-Info "Nenhum banco temporário encontrado (primeira execução)"
}

Write-Info "Iniciando restauração do backup..."
Write-Warning-Custom "Esta operação pode demorar 10-15 minutos"

# Obter nomes lógicos dos arquivos do backup
$fileListQuery = "RESTORE FILELISTONLY FROM DISK = '$BackupPath'"
$fileList = Invoke-SqlLocal -Query $fileListQuery

$dataPath = "C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA"

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
    Invoke-SqlLocal -Query $restoreQuery | Out-Null
    Write-Success "Backup restaurado com sucesso!"
}
catch {
    Write-Error-Custom "Erro ao restaurar backup: $_"
    Write-Info ""
    Write-Info "POSSÍVEIS SOLUÇÕES:"
    Write-Info "1. Verifique se há espaço suficiente em disco"
    Write-Info "2. Execute o SQL Server como Administrador"
    Write-Info "3. Verifique permissões na pasta: $dataPath"
    
    exit 1
}

# ============================================
# FASE 3: EXTRAIR DADOS
# ============================================

Write-Step "FASE 3: Extrair Dados do Backup"

$allInserts = @()
$totalRecords = 0

# EMPRESAS (Top 25)
Write-Info "Extraindo Empresas (Top 25)..."
$queryEmpresas = @"
SELECT TOP 25
    ROW_NUMBER() OVER (ORDER BY (SELECT COUNT(*) FROM Usina u WHERE u.CodEmpre = e.CodEmpre) DESC) + 100 as Id,
    RTRIM(e.CodEmpre) as Codigo,
    RTRIM(e.SigEmpre) as Sigla,
    RTRIM(e.NomEmpre) as Nome,
    e.NumCNPJ as CNPJ,
    COALESCE(e.NumTelefone, '') as Telefone,
    COALESCE(e.EmailContato, '') as Email,
    GETDATE() as DataCriacao,
    1 as Ativo
FROM Empresa e
WHERE e.Ativo = 1
ORDER BY (SELECT COUNT(*) FROM Usina u WHERE u.CodEmpre = e.CodEmpre) DESC
"@

$empresas = Invoke-SqlLocal -Database $TempDatabase -Query $queryEmpresas
Write-Success "$($empresas.Rows.Count) empresas extraídas"
$totalRecords += $empresas.Rows.Count

# Gerar INSERT para Empresas
$insertEmpresas = "-- Inserir Empresas`nSET IDENTITY_INSERT Empresas ON;`n`n"
foreach ($row in $empresas.Rows) {
    $codigo = $row["Codigo"].ToString().Replace("'", "''")
    $sigla = $row["Sigla"].ToString().Replace("'", "''")
    $nome = $row["Nome"].ToString().Replace("'", "''")
    $cnpj = if ($row["CNPJ"] -is [DBNull]) { "NULL" } else { "'$($row["CNPJ"])'" }
    $telefone = if ([string]::IsNullOrWhiteSpace($row["Telefone"])) { "NULL" } else { "'$($row["Telefone"].ToString().Replace("'", "''"))'" }
    $email = if ([string]::IsNullOrWhiteSpace($row["Email"])) { "NULL" } else { "'$($row["Email"].ToString().Replace("'", "''"))'" }
    
    $insertEmpresas += "INSERT INTO Empresas (Id, Codigo, Sigla, Nome, CNPJ, Telefone, Email, DataCriacao, Ativo) VALUES ($($row["Id"]), '$codigo', '$sigla', '$nome', $cnpj, $telefone, $email, GETDATE(), 1);`n"
}
$insertEmpresas += "`nSET IDENTITY_INSERT Empresas OFF;`n`n"
$allInserts += $insertEmpresas

# USINAS (Top 40)
Write-Info "Extraindo Usinas (Top 40)..."
$queryUsinas = @"
SELECT TOP 40
    ROW_NUMBER() OVER (ORDER BY COALESCE(u.PotInstalada, 0) DESC) + 200 as Id,
    RTRIM(u.CodUsina) as Codigo,
    RTRIM(u.NomUsina) as Nome,
    1 as TipoUsinaId,
    101 as EmpresaId,
    COALESCE(u.PotInstalada, 0) as CapacidadeInstalada,
    COALESCE(RTRIM(u.Municipio), '') + ', ' + COALESCE(RTRIM(u.UF), '') as Localizacao,
    COALESCE(u.DataOperacao, '2000-01-01') as DataOperacao,
    GETDATE() as DataCriacao,
    1 as Ativo
FROM Usina u
WHERE u.Ativo = 1
AND u.PotInstalada > 0
ORDER BY COALESCE(u.PotInstalada, 0) DESC
"@

$usinas = Invoke-SqlLocal -Database $TempDatabase -Query $queryUsinas
Write-Success "$($usinas.Rows.Count) usinas extraídas"
$totalRecords += $usinas.Rows.Count

# Gerar INSERT para Usinas
$insertUsinas = "-- Inserir Usinas`nSET IDENTITY_INSERT Usinas ON;`n`n"
foreach ($row in $usinas.Rows) {
    $codigo = $row["Codigo"].ToString().Replace("'", "''")
    $nome = $row["Nome"].ToString().Replace("'", "''")
    $localizacao = $row["Localizacao"].ToString().Replace("'", "''")
    $dataOperacao = $row["DataOperacao"].ToString("yyyy-MM-dd")
    
    $insertUsinas += "INSERT INTO Usinas (Id, Codigo, Nome, TipoUsinaId, EmpresaId, CapacidadeInstalada, Localizacao, DataOperacao, DataCriacao, Ativo) VALUES ($($row["Id"]), '$codigo', '$nome', $($row["TipoUsinaId"]), $($row["EmpresaId"]), $($row["CapacidadeInstalada"]), '$localizacao', '$dataOperacao', GETDATE(), 1);`n"
}
$insertUsinas += "`nSET IDENTITY_INSERT Usinas OFF;`n`n"
$allInserts += $insertUsinas

# Salvar scripts
$consolidatedFile = Join-Path $OutputPath "expand-to-100-records.sql"
$consolidatedScript = @"
-- ============================================
-- EXPANSÃO PARA 100 REGISTROS - POC PDPW
-- ============================================
-- Gerado em: $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")
-- Total de registros: $totalRecords
-- ============================================

USE [$TargetDatabase];
GO

$($allInserts -join "`n`n")

-- Verificar contagens
SELECT 'Empresas' as Tabela, COUNT(*) as Total FROM Empresas
UNION SELECT 'Usinas', COUNT(*) FROM Usinas
UNION SELECT 'SemanasPMO', COUNT(*) FROM SemanasPMO
UNION SELECT 'EquipesPDP', COUNT(*) FROM EquipesPDP
UNION SELECT 'TiposUsina', COUNT(*) FROM TiposUsina
ORDER BY Tabela;

GO
"@

$consolidatedScript | Out-File -FilePath $consolidatedFile -Encoding UTF8
Write-Success "Script consolidado salvo: $consolidatedFile"

# ============================================
# FASE 4: APLICAR NO DOCKER
# ============================================

Write-Step "FASE 4: Aplicar Dados no SQL Server Docker"

Write-Warning-Custom "Deseja aplicar os dados agora? (S/N)"
$response = Read-Host

if ($response -eq 'S' -or $response -eq 's') {
    Write-Info "Aplicando dados..."
    
    try {
        Invoke-SqlDocker -Database $TargetDatabase -Query $consolidatedScript
        Write-Success "Dados aplicados com sucesso!"
        
        # Verificar contagens
        Write-Info ""
        Write-Info "Verificando registros inseridos:"
        
        $checkQuery = @"
SELECT 'Empresas' as Tabela, COUNT(*) as Total FROM Empresas
UNION SELECT 'Usinas', COUNT(*) FROM Usinas
UNION SELECT 'SemanasPMO', COUNT(*) FROM SemanasPMO
UNION SELECT 'EquipesPDP', COUNT(*) FROM EquipesPDP
UNION SELECT 'TiposUsina', COUNT(*) FROM TiposUsina
ORDER BY Tabela
"@
        
        $connectionString = "Server=$SqlServerDocker;Database=$TargetDatabase;User Id=sa;Password=$SaPassword;TrustServerCertificate=True;"
        $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
        $connection.Open()
        
        $command = $connection.CreateCommand()
        $command.CommandText = $checkQuery
        
        $adapter = New-Object System.Data.SqlClient.SqlDataAdapter($command)
        $dataset = New-Object System.Data.DataSet
        $adapter.Fill($dataset) | Out-Null
        
        $connection.Close()
        
        foreach ($row in $dataset.Tables[0].Rows) {
            Write-Host ("  • {0,-30} {1,5} registros" -f $row["Tabela"], $row["Total"]) -ForegroundColor Gray
        }
    }
    catch {
        Write-Error-Custom "Erro ao aplicar dados: $_"
        Write-Info "Você pode aplicar manualmente executando: $consolidatedFile"
    }
}
else {
    Write-Info "Dados NÃO aplicados automaticamente"
    Write-Info "Para aplicar manualmente, execute via Docker:"
    Write-Info "docker exec -i pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P `"$SaPassword`" -C -d $TargetDatabase -i /path/to/script.sql"
}

# ============================================
# FASE 5: LIMPEZA
# ============================================

Write-Step "FASE 5: Limpeza"

Write-Warning-Custom "Deseja remover o banco temporário? (S/N)"
$response = Read-Host

if ($response -eq 'S' -or $response -eq 's') {
    try {
        $dropQuery = @"
ALTER DATABASE [$TempDatabase] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [$TempDatabase];
"@
        Invoke-SqlLocal -Query $dropQuery | Out-Null
        Write-Success "Banco temporário removido"
    }
    catch {
        Write-Warning-Custom "Não foi possível remover banco temporário automaticamente"
        Write-Info "Remova manualmente via SSMS"
    }
}
else {
    Write-Info "Banco temporário mantido: $TempDatabase"
    Write-Info "Para consultas futuras, conecte em: $SqlServerLocal"
}

# ============================================
# RELATÓRIO FINAL
# ============================================

$endTime = Get-Date
$duration = $endTime - $startTime

Write-Host ""
Write-Host "============================================" -ForegroundColor Green
Write-Host "  EXPANSÃO CONCLUÍDA COM SUCESSO!" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Green
Write-Host ""

Write-Host "?? ESTATÍSTICAS:" -ForegroundColor Cyan
Write-Host "  • Registros extraídos: $totalRecords" -ForegroundColor White
Write-Host "  • Tempo total: $($duration.Minutes)m $($duration.Seconds)s" -ForegroundColor White
Write-Host "  • Script gerado: $consolidatedFile" -ForegroundColor White
Write-Host ""

Write-Host "?? PRÓXIMOS PASSOS:" -ForegroundColor Cyan
Write-Host "  1. Testar APIs via Swagger: http://localhost:5001/swagger" -ForegroundColor White
Write-Host "  2. Validar relacionamentos no banco" -ForegroundColor White
Write-Host "  3. Executar testes de integração" -ForegroundColor White
Write-Host ""
