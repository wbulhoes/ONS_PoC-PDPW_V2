# ============================================================================
# Script de Análise do Backup PDPW
# ============================================================================
# Descrição: Analisa o backup sem restaurar para entender a estrutura
# Autor: Willian Charantola Bulhoes
# Data: 18/12/2025
# ============================================================================

param(
    [Parameter(Mandatory=$false)]
    [string]$BackupPath = "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak",
    
    [Parameter(Mandatory=$false)]
    [string]$ServerInstance = "localhost\SQLEXPRESS"
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

# ============================================================================
# Análise do Backup
# ============================================================================

Write-Header "Análise Detalhada do Backup PDPW"

Write-Info "Arquivo: $BackupPath"
$backupFile = Get-Item $BackupPath
Write-Host "  Tamanho: $([math]::Round($backupFile.Length/1GB,2)) GB"
Write-Host "  Data: $($backupFile.LastWriteTime)"

# Informações do backup
Write-Header "Informações do Backup"

$query1 = @"
RESTORE HEADERONLY 
FROM DISK = '$BackupPath'
"@

try {
    $header = Invoke-Sqlcmd -ServerInstance $ServerInstance -Query $query1 -TrustServerCertificate
    
    Write-Success "Informações Gerais:"
    Write-Host "  Database Name: $($header.DatabaseName)"
    Write-Host "  Backup Date: $($header.BackupStartDate)"
    Write-Host "  SQL Server Version: $($header.SoftwareVersionMajor).$($header.SoftwareVersionMinor).$($header.SoftwareVersionBuild)"
    Write-Host "  Backup Type: $($header.BackupType)"
    Write-Host "  Compressed: $($header.Compressed)"
    Write-Host "  Collation: $($header.Collation)"
}
catch {
    Write-Host "Erro ao ler header: $_" -ForegroundColor Red
}

# Lista de arquivos
Write-Header "Arquivos no Backup"

$query2 = @"
RESTORE FILELISTONLY 
FROM DISK = '$BackupPath'
"@

try {
    $fileList = Invoke-Sqlcmd -ServerInstance $ServerInstance -Query $query2 -TrustServerCertificate
    
    Write-Success "Arquivos encontrados:"
    $totalSize = 0
    foreach ($file in $fileList) {
        $sizeGB = [math]::Round($file.Size/1GB,2)
        $totalSize += $file.Size
        Write-Host "  [$($file.Type)] $($file.LogicalName)" -ForegroundColor Cyan
        Write-Host "      Tamanho: $sizeGB GB"
        Write-Host "      Físico Original: $($file.PhysicalName)"
    }
    
    Write-Host "`n  TOTAL ESTIMADO: $([math]::Round($totalSize/1GB,2)) GB" -ForegroundColor Yellow
}
catch {
    Write-Host "Erro ao ler file list: $_" -ForegroundColor Red
}

# Verificar espaço disponível
Write-Header "Análise de Espaço em Disco"

$drives = Get-PSDrive -PSProvider FileSystem | Where-Object {$_.Name -match "^[A-Z]$"}
foreach ($drive in $drives) {
    $freeGB = [math]::Round($drive.Free/1GB,2)
    $totalGB = [math]::Round(($drive.Used + $drive.Free)/1GB,2)
    $usedGB = [math]::Round($drive.Used/1GB,2)
    $freePercent = [math]::Round(($drive.Free / ($drive.Used + $drive.Free)) * 100, 2)
    
    Write-Host "  Drive $($drive.Name):" -ForegroundColor Cyan
    Write-Host "      Total: $totalGB GB"
    Write-Host "      Usado: $usedGB GB"
    Write-Host "      Livre: $freeGB GB ($freePercent%)"
    
    if ($freeGB -gt ($totalSize/1GB)) {
        Write-Host "      Status: ? ESPAÇO SUFICIENTE" -ForegroundColor Green
    }
    else {
        Write-Host "      Status: ? ESPAÇO INSUFICIENTE" -ForegroundColor Red
    }
}

# Recomendações
Write-Header "Recomendações"

$requiredGB = [math]::Round($totalSize/1GB,2)
$availableGB = [math]::Round((Get-PSDrive C).Free/1GB,2)

if ($availableGB -lt $requiredGB) {
    Write-Host "? PROBLEMA DE ESPAÇO DETECTADO" -ForegroundColor Red
    Write-Host ""
    Write-Host "Espaço necessário: $requiredGB GB"
    Write-Host "Espaço disponível: $availableGB GB"
    Write-Host "Déficit: $([math]::Round($requiredGB - $availableGB,2)) GB"
    Write-Host ""
    Write-Host "OPÇÕES:" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "1. LIBERAR ESPAÇO NO DRIVE C:"
    Write-Host "   - Limpar arquivos temporários"
    Write-Host "   - Desinstalar programas não utilizados"
    Write-Host "   - Mover arquivos grandes para outro local"
    Write-Host ""
    Write-Host "2. USAR OUTRO DRIVE (se disponível):"
    Write-Host "   - Modificar script para apontar para D:\ ou outro drive"
    Write-Host ""
    Write-Host "3. RESTAURAR EM MÁQUINA VIRTUAL/SERVIDOR COM MAIS ESPAÇO"
    Write-Host ""
    Write-Host "4. TRABALHAR COM SUBSET DOS DADOS (PoC):"
    Write-Host "   - Criar backup parcial apenas com tabelas críticas"
    Write-Host "   - Usar script para extrair DDL e popular com dados sample"
    Write-Host ""
    Write-Host "5. USAR InMemory Database (desenvolvimento inicial):"
    Write-Host "   - Mapear schema manualmente"
    Write-Host "   - Popular com dados de exemplo"
    Write-Host "   - Integrar com banco real depois"
}
else {
    Write-Success "Espaço suficiente disponível!"
    Write-Host ""
    Write-Host "Você pode prosseguir com a restauração usando:"
    Write-Host "  .\database\restore-database.ps1 -ServerInstance '$ServerInstance' -BackupPath '$BackupPath' -DatabaseName 'PDPW_DB'"
}

Write-Host ""
