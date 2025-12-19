# Script para parar a aplicação PDPW via Docker

Write-Host "?? PARANDO PDPW DOCKER" -ForegroundColor Cyan
Write-Host "=" * 60
Write-Host ""

Write-Host "??  Parando containers..." -ForegroundColor Yellow
docker-compose stop

Write-Host ""
Write-Host "? Containers parados!" -ForegroundColor Green
Write-Host ""

Write-Host "?? Para remover completamente (incluindo volumes):" -ForegroundColor Yellow
Write-Host "   docker-compose down -v" -ForegroundColor Gray
Write-Host ""

Write-Host "?? Para iniciar novamente:" -ForegroundColor Yellow
Write-Host "   .\docker-start.ps1" -ForegroundColor Gray
Write-Host ""
