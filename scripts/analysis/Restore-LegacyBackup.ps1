# ============================================
# SCRIPT DE RESTAURAÇÃO DO BACKUP LEGADO
# ============================================
# Arquivo: Restore-LegacyBackup.ps1
# Descrição: Restaura o backup do cliente em um banco separado
# ============================================

param(
    [string]$ServerInstance = "localhost\SQLEXPRESS",
    [string]$BackupFile = "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak",
    [string]$NewDatabaseName = "PDPW_Legacy",
    [string]$DataPath = "C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA"
)

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  RESTAURAÇÃO DO BACKUP LEGADO" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# 1. Verificar se o backup existe
Write-Host "1. Verificando arquivo de backup..." -ForegroundColor Yellow
if (-not (Test-Path $BackupFile)) {
    Write-Host "   ? ERRO: Arquivo de backup não encontrado!" -ForegroundColor Red
    Write-Host "   Caminho: $BackupFile" -ForegroundColor Red
    exit 1
}
Write-Host "   ? Backup encontrado: $((Get-Item $BackupFile).Length / 1GB) GB" -ForegroundColor Green

# 2. Verificar conexão com SQL Server
Write-Host ""
Write-Host "2. Testando conexão com SQL Server..." -ForegroundColor Yellow
try {
    $testQuery = "SELECT @@VERSION"
    $result = sqlcmd -S $ServerInstance -E -Q $testQuery -h -1 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ? Conexão estabelecida com sucesso!" -ForegroundColor Green
    } else {
        throw "Erro na conexão"
    }
} catch {
    Write-Host "   ? ERRO: Não foi possível conectar ao SQL Server!" -ForegroundColor Red
    Write-Host "   Server: $ServerInstance" -ForegroundColor Red
    exit 1
}

# 3. Verificar se o banco já existe
Write-Host ""
Write-Host "3. Verificando se banco já existe..." -ForegroundColor Yellow
$checkDbQuery = "SELECT COUNT(*) FROM sys.databases WHERE name = '$NewDatabaseName'"
$dbExists = sqlcmd -S $ServerInstance -E -Q $checkDbQuery -h -1
if ($dbExists -gt 0) {
    Write-Host "   ??  AVISO: Banco '$NewDatabaseName' já existe!" -ForegroundColor Yellow
    $confirm = Read-Host "   Deseja sobrescrever? (S/N)"
    if ($confirm -ne "S") {
        Write-Host "   ? Operação cancelada pelo usuário." -ForegroundColor Red
        exit 0
    }
    Write-Host "   ???  Removendo banco existente..." -ForegroundColor Yellow
    $dropQuery = @"
ALTER DATABASE [$NewDatabaseName] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [$NewDatabaseName];
"@
    sqlcmd -S $ServerInstance -E -Q $dropQuery
    Write-Host "   ? Banco anterior removido!" -ForegroundColor Green
}

# 4. Obter informações dos arquivos do backup
Write-Host ""
Write-Host "4. Analisando estrutura do backup..." -ForegroundColor Yellow
$fileListQuery = "RESTORE FILELISTONLY FROM DISK = '$BackupFile'"
$fileList = sqlcmd -S $ServerInstance -E -Q $fileListQuery -h -1 -s "|" | Select-Object -Skip 2

Write-Host "   ?? Arquivos encontrados no backup:" -ForegroundColor Cyan
$dataFile = $null
$logFiles = @()

foreach ($line in $fileList) {
    if ([string]::IsNullOrWhiteSpace($line)) { continue }
    $parts = $line -split '\|'
    if ($parts.Length -lt 2) { continue }
    
    $logicalName = $parts[0].Trim()
    $fileType = $parts[2].Trim()
    
    Write-Host "      - $logicalName ($fileType)" -ForegroundColor Gray
    
    if ($fileType -eq 'D') {
        $dataFile = $logicalName
    } elseif ($fileType -eq 'L') {
        $logFiles += $logicalName
    }
}

if (-not $dataFile) {
    Write-Host "   ? ERRO: Arquivo de dados não encontrado no backup!" -ForegroundColor Red
    exit 1
}

# 5. Construir comando RESTORE
Write-Host ""
Write-Host "5. Preparando comando de restauração..." -ForegroundColor Yellow

$moveStatements = @()
$moveStatements += "MOVE '$dataFile' TO '$DataPath\$NewDatabaseName.mdf'"
$logIndex = 1
foreach ($logFile in $logFiles) {
    $moveStatements += "MOVE '$logFile' TO '$DataPath\${NewDatabaseName}_log$logIndex.ldf'"
    $logIndex++
}

$restoreQuery = @"
RESTORE DATABASE [$NewDatabaseName]
FROM DISK = '$BackupFile'
WITH 
    $($moveStatements -join ",`n    "),
    REPLACE,
    RECOVERY,
    STATS = 5;
"@

Write-Host "   ?? Comando gerado:" -ForegroundColor Cyan
Write-Host $restoreQuery -ForegroundColor Gray

# 6. Executar restauração
Write-Host ""
Write-Host "6. Iniciando restauração (isso pode demorar vários minutos)..." -ForegroundColor Yellow
Write-Host "   ? Aguarde... (Backup de 43 GB)" -ForegroundColor Yellow
Write-Host ""

try {
    $restoreOutput = sqlcmd -S $ServerInstance -E -Q $restoreQuery 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "   ? Restauração concluída com sucesso!" -ForegroundColor Green
    } else {
        throw "Erro na restauração: $restoreOutput"
    }
} catch {
    Write-Host ""
    Write-Host "   ? ERRO durante a restauração!" -ForegroundColor Red
    Write-Host "   Detalhes: $_" -ForegroundColor Red
    exit 1
}

# 7. Verificar banco restaurado
Write-Host ""
Write-Host "7. Verificando banco restaurado..." -ForegroundColor Yellow
$verifyQuery = @"
SELECT 
    name,
    state_desc,
    recovery_model_desc,
    (size * 8 / 1024) AS SizeMB
FROM sys.databases 
WHERE name = '$NewDatabaseName'
"@
$dbInfo = sqlcmd -S $ServerInstance -E -Q $verifyQuery
Write-Host $dbInfo -ForegroundColor Gray

# 8. Contar tabelas e registros
Write-Host ""
Write-Host "8. Analisando conteúdo..." -ForegroundColor Yellow
$analysisQuery = @"
USE [$NewDatabaseName];

-- Contar tabelas
SELECT 'Total de Tabelas' AS Info, COUNT(*) AS Valor
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'

UNION ALL

-- Contar registros totais
SELECT 'Total de Registros' AS Info, SUM(p.rows) AS Valor
FROM sys.tables t
INNER JOIN sys.partitions p ON t.object_id = p.OBJECT_ID
WHERE p.index_id < 2;
"@

$analysis = sqlcmd -S $ServerInstance -E -Q $analysisQuery
Write-Host $analysis -ForegroundColor Cyan

# 9. Listar top 10 tabelas com mais registros
Write-Host ""
Write-Host "9. Top 10 Tabelas (por quantidade de registros):" -ForegroundColor Yellow
$top10Query = @"
USE [$NewDatabaseName];

SELECT TOP 10
    t.NAME AS TableName,
    SUM(p.rows) AS RowCounts
FROM sys.tables t
INNER JOIN sys.partitions p ON t.object_id = p.OBJECT_ID
WHERE p.index_id < 2
GROUP BY t.NAME
ORDER BY RowCounts DESC;
"@

$top10 = sqlcmd -S $ServerInstance -E -Q $top10Query
Write-Host $top10 -ForegroundColor Cyan

# 10. Finalização
Write-Host ""
Write-Host "============================================" -ForegroundColor Green
Write-Host "  RESTAURAÇÃO CONCLUÍDA COM SUCESSO!" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Green
Write-Host ""
Write-Host "?? Informações do Banco:" -ForegroundColor Cyan
Write-Host "   Servidor: $ServerInstance" -ForegroundColor White
Write-Host "   Banco: $NewDatabaseName" -ForegroundColor White
Write-Host "   Caminho: $DataPath" -ForegroundColor White
Write-Host ""
Write-Host "?? Próximos Passos:" -ForegroundColor Cyan
Write-Host "   1. Analisar estrutura das tabelas" -ForegroundColor White
Write-Host "   2. Mapear tabelas Legado -> POC" -ForegroundColor White
Write-Host "   3. Criar scripts de migração de dados" -ForegroundColor White
Write-Host "   4. Testar integridade dos dados migrados" -ForegroundColor White
Write-Host ""
Write-Host "?? Connection String:" -ForegroundColor Cyan
Write-Host "   Server=$ServerInstance;Database=$NewDatabaseName;Trusted_Connection=True;TrustServerCertificate=True;" -ForegroundColor Yellow
Write-Host ""
