# ============================================================================
# Script de Restauração do Banco de Dados PDPW
# ============================================================================
# Descrição: Script automatizado para restaurar backup do banco de dados
# Autor: Willian Charantola Bulhoes
# Data: 17/12/2025
# ============================================================================

param(
    [Parameter(Mandatory=$false)]
    [string]$BackupPath = "",
    
    [Parameter(Mandatory=$false)]
    [string]$ServerInstance = "localhost",
    
    [Parameter(Mandatory=$false)]
    [string]$DatabaseName = "PDPW_DB",
    
    [Parameter(Mandatory=$false)]
    [switch]$AnalyzeOnly,
    
    [Parameter(Mandatory=$false)]
    [switch]$GenerateScripts
)

# ============================================================================
# Configurações
# ============================================================================

$ErrorActionPreference = "Stop"
$projectRoot = $PSScriptRoot | Split-Path | Split-Path
$backupDir = Join-Path $projectRoot "database\backups"
$workingDir = Join-Path $backupDir "working"
$scriptsDir = Join-Path $projectRoot "database\scripts"
$dataDir = Join-Path $projectRoot "database\data"

# ============================================================================
# Funções Auxiliares
# ============================================================================

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

function Test-SqlServerConnection {
    param([string]$Server)
    
    try {
        $connectionString = "Server=$Server;Database=master;Integrated Security=True;TrustServerCertificate=True;"
        $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
        $connection.Open()
        $connection.Close()
        return $true
    }
    catch {
        return $false
    }
}

function Get-BackupFileInfo {
    param([string]$BackupFile)
    
    try {
        $query = @"
RESTORE FILELISTONLY 
FROM DISK = '$BackupFile'
"@
        
        $result = Invoke-Sqlcmd -ServerInstance $ServerInstance -Query $query -TrustServerCertificate
        return $result
    }
    catch {
        Write-Error-Custom "Erro ao ler informações do backup: $_"
        return $null
    }
}

function Restore-Database {
    param(
        [string]$BackupFile,
        [string]$Server,
        [string]$Database,
        [array]$FileList
    )
    
    # Criar diretório de dados se não existir
    if (-not (Test-Path $dataDir)) {
        New-Item -ItemType Directory -Path $dataDir -Force | Out-Null
    }
    
    # Construir comando RESTORE com MOVE
    $moveStatements = @()
    $logFileIndex = 1
    foreach ($file in $FileList) {
        $logicalName = $file.LogicalName
        if ($file.Type -eq 'D') {
            # Arquivo de dados
            $physicalPath = Join-Path $dataDir "$Database.mdf"
        } else {
            # Arquivo de log (múltiplos)
            $physicalPath = Join-Path $dataDir "$Database`_log$logFileIndex.ldf"
            $logFileIndex++
        }
        $moveStatements += "MOVE '$logicalName' TO '$physicalPath'"
    }
    
    $moveClause = $moveStatements -join ", `n    "
    
    $query = @"
-- Verificar se banco existe e colocar em modo SINGLE_USER
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = '$Database')
BEGIN
    ALTER DATABASE [$Database] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$Database];
END
GO

-- Restaurar banco
RESTORE DATABASE [$Database]
FROM DISK = '$BackupFile'
WITH 
    $moveClause,
    REPLACE,
    RECOVERY,
    STATS = 10;
GO

-- Verificar integridade
DBCC CHECKDB('$Database') WITH NO_INFOMSGS;
GO

-- Atualizar estatísticas
USE [$Database];
GO
EXEC sp_updatestats;
GO
"@
    
    try {
        Write-Info "Iniciando restauração do banco '$Database'..."
        Write-Info "Backup: $BackupFile"
        Write-Info "Servidor: $Server"
        
        Invoke-Sqlcmd -ServerInstance $Server -Query $query -QueryTimeout 0 -TrustServerCertificate
        
        Write-Success "Banco restaurado com sucesso!"
        return $true
    }
    catch {
        Write-Error-Custom "Erro ao restaurar banco: $_"
        return $false
    }
}

function Get-DatabaseInfo {
    param(
        [string]$Server,
        [string]$Database
    )
    
    $query = @"
-- Informações gerais
SELECT 
    name AS DatabaseName,
    state_desc AS State,
    recovery_model_desc AS RecoveryModel,
    compatibility_level AS CompatibilityLevel,
    create_date AS CreateDate
FROM sys.databases 
WHERE name = '$Database';

-- Tamanho do banco
EXEC sp_spaceused;

-- Tabelas
SELECT 
    s.name AS SchemaName,
    t.name AS TableName,
    p.rows AS RowCount
FROM sys.tables t
INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
INNER JOIN sys.partitions p ON t.object_id = p.object_id
WHERE p.index_id IN (0, 1)
ORDER BY p.rows DESC;
"@
    
    try {
        $result = Invoke-Sqlcmd -ServerInstance $Server -Database $Database -Query $query -TrustServerCertificate
        return $result
    }
    catch {
        Write-Error-Custom "Erro ao obter informações do banco: $_"
        return $null
    }
}

function Export-DatabaseSchema {
    param(
        [string]$Server,
        [string]$Database,
        [string]$OutputDir
    )
    
    # Criar diretório de scripts se não existir
    $schemaDir = Join-Path $OutputDir "schema"
    if (-not (Test-Path $schemaDir)) {
        New-Item -ItemType Directory -Path $schemaDir -Force | Out-Null
    }
    
    # Queries para extrair schema
    $queries = @{
        "Tables" = @"
SELECT 
    s.name AS SchemaName,
    t.name AS TableName,
    OBJECT_DEFINITION(t.object_id) AS Definition
FROM sys.tables t
INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
ORDER BY s.name, t.name;
"@
        "Views" = @"
SELECT 
    s.name AS SchemaName,
    v.name AS ViewName,
    OBJECT_DEFINITION(v.object_id) AS Definition
FROM sys.views v
INNER JOIN sys.schemas s ON v.schema_id = s.schema_id
WHERE v.is_ms_shipped = 0
ORDER BY s.name, v.name;
"@
        "StoredProcedures" = @"
SELECT 
    s.name AS SchemaName,
    p.name AS ProcedureName,
    OBJECT_DEFINITION(p.object_id) AS Definition
FROM sys.procedures p
INNER JOIN sys.schemas s ON p.schema_id = s.schema_id
WHERE p.is_ms_shipped = 0
ORDER BY s.name, p.name;
"@
        "ForeignKeys" = @"
SELECT 
    fk.name AS ForeignKeyName,
    OBJECT_NAME(fk.parent_object_id) AS TableName,
    COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS ColumnName,
    OBJECT_NAME(fk.referenced_object_id) AS ReferencedTableName,
    COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) AS ReferencedColumnName
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
ORDER BY TableName, ColumnName;
"@
    }
    
    Write-Info "Exportando schema para: $schemaDir"
    
    foreach ($key in $queries.Keys) {
        try {
            $outputFile = Join-Path $schemaDir "$key.sql"
            $result = Invoke-Sqlcmd -ServerInstance $Server -Database $Database -Query $queries[$key] -TrustServerCertificate
            
            $result | ForEach-Object {
                if ($_.Definition) {
                    $_.Definition | Out-File -FilePath $outputFile -Append -Encoding UTF8
                    "GO`n" | Out-File -FilePath $outputFile -Append -Encoding UTF8
                }
            }
            
            Write-Success "Exportado: $key.sql"
        }
        catch {
            Write-Error-Custom "Erro ao exportar $key : $_"
        }
    }
}

# ============================================================================
# Script Principal
# ============================================================================

Write-Header "Restauração do Banco de Dados PDPW"

# Verificar se SQL Server está acessível
Write-Info "Verificando conexão com SQL Server ($ServerInstance)..."
if (-not (Test-SqlServerConnection -Server $ServerInstance)) {
    Write-Error-Custom "Não foi possível conectar ao SQL Server: $ServerInstance"
    Write-Info "Verifique se:"
    Write-Info "  - SQL Server está instalado e rodando"
    Write-Info "  - Você tem permissões adequadas"
    Write-Info "  - O nome da instância está correto"
    exit 1
}
Write-Success "Conexão OK"

# Se BackupPath não foi informado, procurar na pasta working
if ([string]::IsNullOrEmpty($BackupPath)) {
    Write-Info "Procurando arquivos .bak na pasta working..."
    $backupFiles = Get-ChildItem -Path $workingDir -Filter "*.bak" -ErrorAction SilentlyContinue
    
    if ($backupFiles.Count -eq 0) {
        Write-Error-Custom "Nenhum arquivo .bak encontrado em: $workingDir"
        Write-Info "Por favor:"
        Write-Info "  1. Copie o backup fornecido pelo cliente para: $workingDir"
        Write-Info "  2. Execute este script novamente"
        Write-Info "  OU"
        Write-Info "  Execute com o parâmetro -BackupPath: .\restore-database.ps1 -BackupPath 'C:\caminho\do\backup.bak'"
        exit 1
    }
    
    if ($backupFiles.Count -gt 1) {
        Write-Info "Múltiplos backups encontrados. Selecione um:"
        for ($i = 0; $i -lt $backupFiles.Count; $i++) {
            Write-Host "  [$i] $($backupFiles[$i].Name) ($($backupFiles[$i].Length / 1MB) MB)"
        }
        $selection = Read-Host "Digite o número do backup"
        $BackupPath = $backupFiles[$selection].FullName
    }
    else {
        $BackupPath = $backupFiles[0].FullName
    }
}

Write-Success "Backup selecionado: $BackupPath"

# Analisar backup
Write-Info "Analisando arquivo de backup..."
$fileList = Get-BackupFileInfo -BackupFile $BackupPath

if ($null -eq $fileList) {
    Write-Error-Custom "Não foi possível ler o arquivo de backup"
    exit 1
}

Write-Success "Backup contém $($fileList.Count) arquivo(s):"
$fileList | ForEach-Object {
    Write-Host "  - $($_.LogicalName) ($($_.Type))"
}

# Se AnalyzeOnly, parar aqui
if ($AnalyzeOnly) {
    Write-Info "Modo análise (AnalyzeOnly). Restauração não será executada."
    exit 0
}

# Confirmar restauração
Write-Host "`n" -NoNewline
Write-Warning "ATENÇÃO: Esta operação irá:"
Write-Host "  1. Dropar o banco '$DatabaseName' se ele existir"
Write-Host "  2. Restaurar o backup em: $BackupPath"
Write-Host "  3. Criar arquivos de dados em: $dataDir"
Write-Host "`n" -NoNewline

$confirm = Read-Host "Deseja continuar? (S/N)"
if ($confirm -ne "S" -and $confirm -ne "s") {
    Write-Info "Operação cancelada pelo usuário"
    exit 0
}

# Restaurar banco
$success = Restore-Database -BackupFile $BackupPath -Server $ServerInstance -Database $DatabaseName -FileList $fileList

if (-not $success) {
    Write-Error-Custom "Falha na restauração do banco"
    exit 1
}

# Obter informações do banco restaurado
Write-Header "Informações do Banco Restaurado"
Get-DatabaseInfo -Server $ServerInstance -Database $DatabaseName | Format-Table -AutoSize

# Gerar scripts se solicitado
if ($GenerateScripts) {
    Write-Header "Exportando Schema"
    Export-DatabaseSchema -Server $ServerInstance -Database $DatabaseName -OutputDir $scriptsDir
}

# Sucesso
Write-Header "Restauração Concluída com Sucesso!"
Write-Success "Banco '$DatabaseName' restaurado e pronto para uso"
Write-Info "Connection String:"
Write-Host "  Server=$ServerInstance;Database=$DatabaseName;Trusted_Connection=True;TrustServerCertificate=True;" -ForegroundColor Cyan

if ($GenerateScripts) {
    Write-Info "`nScripts do schema exportados para: $scriptsDir\schema"
}

Write-Info "`nPróximos passos:"
Write-Host "  1. Conectar ao banco usando SSMS ou Azure Data Studio"
Write-Host "  2. Explorar as tabelas e dados"
Write-Host "  3. Atualizar appsettings.json com a connection string"
Write-Host "  4. Executar scaffold do EF Core para gerar entidades"
