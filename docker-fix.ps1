# Script para corrigir problemas comuns do Docker

Write-Host "?? CORRIGINDO DOCKER" -ForegroundColor Cyan
Write-Host "=" * 60
Write-Host ""

# 1. Parar containers existentes
Write-Host "1. Parando containers existentes..." -ForegroundColor Yellow
docker-compose down -v 2>$null
Write-Host "? Containers parados" -ForegroundColor Green
Write-Host ""

# 2. Limpar sistema Docker
Write-Host "2. Limpando sistema Docker..." -ForegroundColor Yellow
Write-Host "   (Isso pode levar alguns segundos)" -ForegroundColor Gray
docker system prune -f 2>$null
Write-Host "? Sistema limpo" -ForegroundColor Green
Write-Host ""

# 3. Verificar Docker Desktop
Write-Host "3. Verificando Docker Desktop..." -ForegroundColor Yellow
$dockerProcess = Get-Process "Docker Desktop" -ErrorAction SilentlyContinue

if (-not $dockerProcess) {
    Write-Host "??  Docker Desktop não está rodando!" -ForegroundColor Yellow
    Write-Host "   Abrindo Docker Desktop..." -ForegroundColor Yellow
    
    Start-Process "C:\Program Files\Docker\Docker\Docker Desktop.exe"
    
    Write-Host "   Aguardando inicialização (60 segundos)..." -ForegroundColor Yellow
    Start-Sleep -Seconds 60
} else {
    Write-Host "? Docker Desktop está rodando" -ForegroundColor Green
}

Write-Host ""

# 4. Testar conexão
Write-Host "4. Testando conexão Docker..." -ForegroundColor Yellow
$maxTries = 5
$tries = 0
$connected = $false

while ($tries -lt $maxTries -and -not $connected) {
    $tries++
    Write-Host "   Tentativa $tries de $maxTries..." -ForegroundColor Gray
    
    try {
        docker info 2>$null | Out-Null
        if ($LASTEXITCODE -eq 0) {
            $connected = $true
            Write-Host "? Conexão estabelecida!" -ForegroundColor Green
        } else {
            Start-Sleep -Seconds 10
        }
    } catch {
        Start-Sleep -Seconds 10
    }
}

if (-not $connected) {
    Write-Host "? Não foi possível conectar ao Docker!" -ForegroundColor Red
    Write-Host ""
    Write-Host "?? SOLUÇÕES MANUAIS:" -ForegroundColor Yellow
    Write-Host "   1. Feche completamente o Docker Desktop" -ForegroundColor White
    Write-Host "   2. Abra o Gerenciador de Tarefas" -ForegroundColor White
    Write-Host "   3. Finalize TODOS os processos Docker" -ForegroundColor White
    Write-Host "   4. Abra o Docker Desktop novamente" -ForegroundColor White
    Write-Host "   5. Aguarde o status: 'Docker Desktop is running'" -ForegroundColor White
    Write-Host "   6. Execute este script novamente" -ForegroundColor White
    Write-Host ""
    exit 1
}

Write-Host ""
Write-Host "=" * 60
Write-Host "? DOCKER CORRIGIDO E PRONTO!" -ForegroundColor Green
Write-Host ""
Write-Host "?? Próximo passo:" -ForegroundColor Yellow
Write-Host "   .\docker-start.ps1" -ForegroundColor Cyan
Write-Host ""
