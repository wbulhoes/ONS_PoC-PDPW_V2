# Script PowerShell para Restaurar Backup do Cliente PDPW
# Uso: .\Restore-ClientBackup.ps1

Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "  RESTAURAÇÃO DE BACKUP - PDPW CLIENTE" -ForegroundColor Cyan
Write-Host "=============================================" -ForegroundColor Cyan
Write-Host ""

# Configurações
$backupPath = "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak"
$sqlScriptPath = ".\scripts\restore-backup-cliente.sql"
$server = "localhost,1433"
$database = "PDPW_DB"
$username = "sa"
$password = "Pdpw@2024!Strong"

# Verificar se o backup existe
if (-Not (Test-Path $backupPath)) {
    Write-Host "? ERRO: Arquivo de backup não encontrado!" -ForegroundColor Red
    Write-Host "   Caminho esperado: $backupPath" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "   Verifique se o caminho está correto." -ForegroundColor Yellow
    exit 1
}

Write-Host "? Backup encontrado: $backupPath" -ForegroundColor Green
Write-Host "  Tamanho: $((Get-Item $backupPath).Length / 1MB) MB" -ForegroundColor Gray
Write-Host ""

# Verificar se SQL Server está rodando
Write-Host "?? Verificando SQL Server..." -ForegroundColor Yellow
try {
    $connection = New-Object System.Data.SqlClient.SqlConnection
    $connection.ConnectionString = "Server=$server;Database=master;User Id=$username;Password=$password;TrustServerCertificate=True;"
    $connection.Open()
    $connection.Close()
    Write-Host "? SQL Server está rodando!" -ForegroundColor Green
    Write-Host ""
} catch {
    Write-Host "? ERRO: Não foi possível conectar ao SQL Server!" -ForegroundColor Red
    Write-Host "   Servidor: $server" -ForegroundColor Yellow
    Write-Host "   Erro: $($_.Exception.Message)" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "   Certifique-se de que:" -ForegroundColor Yellow
    Write-Host "   1. O SQL Server está instalado e rodando" -ForegroundColor Yellow
    Write-Host "   2. A porta 1433 está aberta" -ForegroundColor Yellow
    Write-Host "   3. As credenciais estão corretas" -ForegroundColor Yellow
    exit 1
}

# Executar restauração
Write-Host "?? Iniciando restauração do backup..." -ForegroundColor Yellow
Write-Host "   Isso pode levar alguns minutos..." -ForegroundColor Gray
Write-Host ""

try {
    # Opção 1: Usando sqlcmd (mais confiável)
    if (Get-Command sqlcmd -ErrorAction SilentlyContinue) {
        Write-Host "?? Executando via sqlcmd..." -ForegroundColor Cyan
        
        $sqlcmdArgs = @(
            "-S", $server,
            "-U", $username,
            "-P", $password,
            "-d", "master",
            "-i", $sqlScriptPath
        )
        
        & sqlcmd @sqlcmdArgs
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host ""
            Write-Host "=============================================" -ForegroundColor Green
            Write-Host "  ? RESTAURAÇÃO CONCLUÍDA COM SUCESSO!" -ForegroundColor Green
            Write-Host "=============================================" -ForegroundColor Green
        } else {
            throw "sqlcmd retornou código de erro: $LASTEXITCODE"
        }
    }
    # Opção 2: Usando Invoke-Sqlcmd (requer SqlServer module)
    elseif (Get-Command Invoke-Sqlcmd -ErrorAction SilentlyContinue) {
        Write-Host "?? Executando via Invoke-Sqlcmd..." -ForegroundColor Cyan
        
        Invoke-Sqlcmd -ServerInstance $server `
                      -Username $username `
                      -Password $password `
                      -Database "master" `
                      -InputFile $sqlScriptPath `
                      -Verbose
        
        Write-Host ""
        Write-Host "=============================================" -ForegroundColor Green
        Write-Host "  ? RESTAURAÇÃO CONCLUÍDA COM SUCESSO!" -ForegroundColor Green
        Write-Host "=============================================" -ForegroundColor Green
    }
    else {
        Write-Host "? sqlcmd não encontrado. Instalando módulo SqlServer..." -ForegroundColor Yellow
        Install-Module -Name SqlServer -Scope CurrentUser -Force -AllowClobber
        
        Write-Host "?? Executando via Invoke-Sqlcmd..." -ForegroundColor Cyan
        Invoke-Sqlcmd -ServerInstance $server `
                      -Username $username `
                      -Password $password `
                      -Database "master" `
                      -InputFile $sqlScriptPath `
                      -Verbose
        
        Write-Host ""
        Write-Host "=============================================" -ForegroundColor Green
        Write-Host "  ? RESTAURAÇÃO CONCLUÍDA COM SUCESSO!" -ForegroundColor Green
        Write-Host "=============================================" -ForegroundColor Green
    }
} catch {
    Write-Host ""
    Write-Host "? ERRO durante a restauração!" -ForegroundColor Red
    Write-Host "   $($_.Exception.Message)" -ForegroundColor Yellow
    exit 1
}

Write-Host ""
Write-Host "?? PRÓXIMOS PASSOS:" -ForegroundColor Cyan
Write-Host "   1. Atualizar appsettings.Development.json" -ForegroundColor White
Write-Host "   2. Mudar UseInMemoryDatabase para false" -ForegroundColor White
Write-Host "   3. Reiniciar a aplicação" -ForegroundColor White
Write-Host "   4. Testar no Swagger com dados reais!" -ForegroundColor White
Write-Host ""
