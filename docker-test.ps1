# Script para testar API Docker rapidamente

Write-Host "?? TESTANDO API DOCKER" -ForegroundColor Cyan
Write-Host "=" * 60
Write-Host ""

# Verificar se está rodando
Write-Host "?? Verificando containers..." -ForegroundColor Yellow
$containers = docker ps --format "{{.Names}}" 2>$null

if ($containers -notcontains "pdpw-api") {
    Write-Host "? Container pdpw-api não está rodando!" -ForegroundColor Red
    Write-Host "   Execute: .\docker-start.ps1" -ForegroundColor Yellow
    exit 1
}

Write-Host "? Container rodando!" -ForegroundColor Green
Write-Host ""

# Teste 1: Health Check
Write-Host "?? TESTE 1: Health Check" -ForegroundColor Cyan
try {
    $health = Invoke-RestMethod -Uri "http://localhost:5000/health" -TimeoutSec 5
    Write-Host "? Status: $($health.status)" -ForegroundColor Green
} catch {
    Write-Host "? Health check falhou!" -ForegroundColor Red
    Write-Host "   Erro: $_" -ForegroundColor Red
}
Write-Host ""

# Teste 2: GET Usinas
Write-Host "?? TESTE 2: GET /api/usinas" -ForegroundColor Cyan
try {
    $usinas = Invoke-RestMethod -Uri "http://localhost:5000/api/usinas" -TimeoutSec 10
    $count = $usinas.Count
    Write-Host "? Retornou $count usinas" -ForegroundColor Green
    
    if ($count -gt 0) {
        Write-Host ""
        Write-Host "?? Primeiras 3 usinas:" -ForegroundColor Yellow
        $usinas | Select-Object -First 3 | ForEach-Object {
            Write-Host "   • $($_.nome) ($($_.capacidadeInstalada) MW)" -ForegroundColor White
        }
    }
} catch {
    Write-Host "? GET /api/usinas falhou!" -ForegroundColor Red
    Write-Host "   Erro: $_" -ForegroundColor Red
}
Write-Host ""

# Teste 3: GET Usina por ID
Write-Host "?? TESTE 3: GET /api/usinas/1 (Itaipu)" -ForegroundColor Cyan
try {
    $itaipu = Invoke-RestMethod -Uri "http://localhost:5000/api/usinas/1" -TimeoutSec 5
    Write-Host "? $($itaipu.nome)" -ForegroundColor Green
    Write-Host "   Código: $($itaipu.codigo)" -ForegroundColor White
    Write-Host "   Capacidade: $($itaipu.capacidadeInstalada) MW" -ForegroundColor White
    Write-Host "   Empresa: $($itaipu.empresa)" -ForegroundColor White
} catch {
    Write-Host "? GET /api/usinas/1 falhou!" -ForegroundColor Red
    Write-Host "   Erro: $_" -ForegroundColor Red
}
Write-Host ""

Write-Host "=" * 60
Write-Host "?? URLs:" -ForegroundColor Cyan
Write-Host ""
Write-Host "   Swagger:  http://localhost:5000/swagger" -ForegroundColor White
Write-Host "   API:      http://localhost:5000/api" -ForegroundColor White
Write-Host "   Health:   http://localhost:5000/health" -ForegroundColor White
Write-Host ""

Write-Host "? TESTES CONCLUÍDOS!" -ForegroundColor Green
Write-Host ""
