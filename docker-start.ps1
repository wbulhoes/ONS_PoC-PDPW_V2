# Script para subir a aplicação PDPW via Docker
# Para apresentação na Daily

Write-Host "?? SUBINDO PDPW VIA DOCKER" -ForegroundColor Cyan
Write-Host "=" * 60
Write-Host ""

# Verificar se Docker está rodando
Write-Host "?? Verificando Docker..." -ForegroundColor Yellow

$maxTries = 3
$tries = 0
$dockerOk = $false

while ($tries -lt $maxTries -and -not $dockerOk) {
    $tries++
    try {
        docker info 2>$null | Out-Null
        if ($LASTEXITCODE -eq 0) {
            $dockerOk = $true
        } else {
            if ($tries -lt $maxTries) {
                Write-Host "   ? Docker não respondeu, tentando novamente ($tries/$maxTries)..." -ForegroundColor Yellow
                Start-Sleep -Seconds 5
            }
        }
    } catch {
        if ($tries -lt $maxTries) {
            Write-Host "   ? Erro ao conectar, tentando novamente ($tries/$maxTries)..." -ForegroundColor Yellow
            Start-Sleep -Seconds 5
        }
    }
}

if (-not $dockerOk) {
    Write-Host "? ERRO: Docker não está rodando ou não responde!" -ForegroundColor Red
    Write-Host ""
    Write-Host "?? SOLUÇÕES:" -ForegroundColor Yellow
    Write-Host "   1. Abra o Docker Desktop" -ForegroundColor White
    Write-Host "   2. Aguarde o status: 'Docker Desktop is running'" -ForegroundColor White
    Write-Host "   3. Execute: .\docker-diagnostico.ps1" -ForegroundColor Cyan
    Write-Host "   4. Se persistir: .\docker-fix.ps1" -ForegroundColor Cyan
    Write-Host ""
    exit 1
}

Write-Host "? Docker está rodando!" -ForegroundColor Green
Write-Host ""

# Limpar containers antigos (opcional)
Write-Host "?? Limpando containers antigos (se existirem)..." -ForegroundColor Yellow
docker-compose down -v 2>$null
Write-Host ""

# Build e start
Write-Host "?? Fazendo build da aplicação..." -ForegroundColor Yellow
Write-Host "   (Isso pode levar alguns minutos na primeira vez)" -ForegroundColor Gray
Write-Host ""

docker-compose build --no-cache

if ($LASTEXITCODE -ne 0) {
    Write-Host "? ERRO no build!" -ForegroundColor Red
    Write-Host ""
    Write-Host "?? Verifique:" -ForegroundColor Yellow
    Write-Host "   - Dockerfile existe" -ForegroundColor White
    Write-Host "   - docker-compose.yml está correto" -ForegroundColor White
    Write-Host "   - Internet está funcionando" -ForegroundColor White
    Write-Host ""
    exit 1
}

Write-Host ""
Write-Host "?? Subindo containers..." -ForegroundColor Yellow
docker-compose up -d

if ($LASTEXITCODE -ne 0) {
    Write-Host "? ERRO ao subir containers!" -ForegroundColor Red
    Write-Host ""
    Write-Host "?? Ver logs:" -ForegroundColor Yellow
    Write-Host "   docker-compose logs" -ForegroundColor Cyan
    Write-Host ""
    exit 1
}

Write-Host ""
Write-Host "? Aguardando SQL Server inicializar..." -ForegroundColor Yellow
Start-Sleep -Seconds 20

Write-Host ""
Write-Host "? Aguardando API inicializar..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

Write-Host ""
Write-Host "=" * 60
Write-Host "? APLICAÇÃO RODANDO COM SUCESSO!" -ForegroundColor Green
Write-Host "=" * 60
Write-Host ""

Write-Host "?? URLs Disponíveis:" -ForegroundColor Cyan
Write-Host ""
Write-Host "   Swagger UI:      http://localhost:5000/swagger" -ForegroundColor White
Write-Host "   API Base:        http://localhost:5000/api" -ForegroundColor White
Write-Host "   Health Check:    http://localhost:5000/health" -ForegroundColor White
Write-Host ""

Write-Host "?? Testar APIs:" -ForegroundColor Cyan
Write-Host ""
Write-Host "   GET Usinas:      http://localhost:5000/api/usinas" -ForegroundColor White
Write-Host ""

Write-Host "?? Comandos Úteis:" -ForegroundColor Yellow
Write-Host ""
Write-Host "   Ver logs:        docker-compose logs -f api" -ForegroundColor Gray
Write-Host "   Testar API:      .\docker-test.ps1" -ForegroundColor Gray
Write-Host "   Parar:           docker-compose stop" -ForegroundColor Gray
Write-Host "   Parar e limpar:  docker-compose down -v" -ForegroundColor Gray
Write-Host "   Restart:         docker-compose restart api" -ForegroundColor Gray
Write-Host ""

Write-Host "?? Para a Daily:" -ForegroundColor Cyan
Write-Host "   1. Abra: http://localhost:5000/swagger" -ForegroundColor White
Write-Host "   2. Teste GET /api/usinas" -ForegroundColor White
Write-Host "   3. Mostre os dados reais (10 usinas)" -ForegroundColor White
Write-Host ""

Write-Host "?? PRONTO PARA APRESENTAR! ??" -ForegroundColor Green
Write-Host ""
