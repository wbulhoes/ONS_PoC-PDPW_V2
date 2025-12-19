# Script para subir a aplicação PDPW via Docker
# Para apresentação na Daily

Write-Host "?? SUBINDO PDPW VIA DOCKER" -ForegroundColor Cyan
Write-Host "=" * 60
Write-Host ""

# Verificar se Docker está rodando
Write-Host "?? Verificando Docker..." -ForegroundColor Yellow
$dockerRunning = docker info 2>$null
if (-not $dockerRunning) {
    Write-Host "? ERRO: Docker não está rodando!" -ForegroundColor Red
    Write-Host "   Inicie o Docker Desktop e tente novamente." -ForegroundColor Red
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
    exit 1
}

Write-Host ""
Write-Host "?? Subindo containers..." -ForegroundColor Yellow
docker-compose up -d

if ($LASTEXITCODE -ne 0) {
    Write-Host "? ERRO ao subir containers!" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "? Aguardando SQL Server inicializar..." -ForegroundColor Yellow
Start-Sleep -Seconds 15

Write-Host ""
Write-Host "?? Aplicando migrations..." -ForegroundColor Yellow
docker exec pdpw-api dotnet ef database update --project /src/PDPW.Infrastructure --startup-project /src/PDPW.API 2>$null

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
