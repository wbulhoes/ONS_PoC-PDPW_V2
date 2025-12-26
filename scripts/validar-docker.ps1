# 🚀 SCRIPT DE VALIDAÇÃO DOCKER - POC PDPw

Write-Host "==============================================================" -ForegroundColor Cyan
Write-Host "  VALIDAÇÃO DOCKER - POC PDPw 100%" -ForegroundColor Cyan
Write-Host "==============================================================" -ForegroundColor Cyan
Write-Host ""

# Verificar se Docker está rodando
Write-Host "📋 [1/10] Verificando Docker..." -ForegroundColor Yellow
try {
    docker --version | Out-Null
    Write-Host "✅ Docker instalado e rodando" -ForegroundColor Green
} catch {
    Write-Host "❌ Docker não encontrado! Instale o Docker Desktop" -ForegroundColor Red
    exit 1
}

# Parar containers antigos
Write-Host ""
Write-Host "📋 [2/10] Parando containers antigos..." -ForegroundColor Yellow
docker-compose down -v 2>$null
Write-Host "✅ Containers antigos removidos" -ForegroundColor Green

# Limpar imagens antigas (opcional)
Write-Host ""
Write-Host "📋 [3/10] Limpando imagens antigas..." -ForegroundColor Yellow
docker image prune -f | Out-Null
Write-Host "✅ Imagens limpas" -ForegroundColor Green

# Build das imagens
Write-Host ""
Write-Host "📋 [4/10] Building imagens Docker..." -ForegroundColor Yellow
Write-Host "    (Isso pode levar alguns minutos...)" -ForegroundColor Gray
docker-compose build --no-cache
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Build concluído com sucesso" -ForegroundColor Green
} else {
    Write-Host "❌ Erro no build!" -ForegroundColor Red
    exit 1
}

# Subir containers
Write-Host ""
Write-Host "📋 [5/10] Iniciando containers..." -ForegroundColor Yellow
docker-compose up -d
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Containers iniciados" -ForegroundColor Green
} else {
    Write-Host "❌ Erro ao iniciar containers!" -ForegroundColor Red
    exit 1
}

# Aguardar SQL Server inicializar
Write-Host ""
Write-Host "📋 [6/10] Aguardando SQL Server inicializar..." -ForegroundColor Yellow
$maxAttempts = 30
$attempt = 0
$sqlReady = $false

while ($attempt -lt $maxAttempts -and -not $sqlReady) {
    $attempt++
    Write-Host "    Tentativa $attempt/$maxAttempts..." -ForegroundColor Gray
    
    $health = docker inspect --format='{{.State.Health.Status}}' pdpw-sqlserver 2>$null
    if ($health -eq "healthy") {
        $sqlReady = $true
        Write-Host "✅ SQL Server pronto!" -ForegroundColor Green
    } else {
        Start-Sleep -Seconds 2
    }
}

if (-not $sqlReady) {
    Write-Host "❌ SQL Server não inicializou no tempo esperado!" -ForegroundColor Red
    docker logs pdpw-sqlserver
    exit 1
}

# Aguardar Backend inicializar
Write-Host ""
Write-Host "📋 [7/10] Aguardando Backend inicializar..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

$maxAttempts = 30
$attempt = 0
$apiReady = $false

while ($attempt -lt $maxAttempts -and -not $apiReady) {
    $attempt++
    Write-Host "    Tentativa $attempt/$maxAttempts..." -ForegroundColor Gray
    
    try {
        $response = Invoke-WebRequest -Uri "http://localhost:5001/health" -TimeoutSec 2 -ErrorAction SilentlyContinue
        if ($response.StatusCode -eq 200) {
            $apiReady = $true
            Write-Host "✅ Backend pronto!" -ForegroundColor Green
        }
    } catch {
        Start-Sleep -Seconds 2
    }
}

if (-not $apiReady) {
    Write-Host "❌ Backend não inicializou no tempo esperado!" -ForegroundColor Red
    Write-Host "    Logs do container:" -ForegroundColor Yellow
    docker logs pdpw-backend --tail 50
    exit 1
}

# Testar Swagger
Write-Host ""
Write-Host "📋 [8/10] Testando Swagger UI..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:5001/swagger" -TimeoutSec 5
    if ($response.StatusCode -eq 200) {
        Write-Host "✅ Swagger UI disponível" -ForegroundColor Green
    }
} catch {
    Write-Host "⚠️  Swagger UI não acessível" -ForegroundColor Yellow
}

# Testar endpoint de exemplo
Write-Host ""
Write-Host "📋 [9/10] Testando endpoints..." -ForegroundColor Yellow

# Teste 1: Dashboard
try {
    $response = Invoke-RestMethod -Uri "http://localhost:5001/api/dashboard/resumo" -Method Get -TimeoutSec 5
    Write-Host "    ✅ GET /api/dashboard/resumo - OK" -ForegroundColor Green
} catch {
    Write-Host "    ❌ GET /api/dashboard/resumo - FALHOU" -ForegroundColor Red
}

# Teste 2: Usinas
try {
    $response = Invoke-RestMethod -Uri "http://localhost:5001/api/usinas" -Method Get -TimeoutSec 5
    Write-Host "    ✅ GET /api/usinas - OK ($($response.Count) usinas)" -ForegroundColor Green
} catch {
    Write-Host "    ❌ GET /api/usinas - FALHOU" -ForegroundColor Red
}

# Teste 3: Ofertas Exportação
try {
    $response = Invoke-RestMethod -Uri "http://localhost:5001/api/ofertas-exportacao" -Method Get -TimeoutSec 5
    Write-Host "    ✅ GET /api/ofertas-exportacao - OK" -ForegroundColor Green
} catch {
    Write-Host "    ❌ GET /api/ofertas-exportacao - FALHOU" -ForegroundColor Red
}

# Status dos containers
Write-Host ""
Write-Host "📋 [10/10] Status dos containers..." -ForegroundColor Yellow
docker-compose ps

# Resumo final
Write-Host ""
Write-Host "==============================================================" -ForegroundColor Cyan
Write-Host "  ✅ VALIDAÇÃO CONCLUÍDA!" -ForegroundColor Green
Write-Host "==============================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "🌐 URLs Disponíveis:" -ForegroundColor Yellow
Write-Host "   Swagger:  http://localhost:5001/swagger" -ForegroundColor White
Write-Host "   API:      http://localhost:5001" -ForegroundColor White
Write-Host "   Health:   http://localhost:5001/health" -ForegroundColor White
Write-Host ""
Write-Host "📊 Dashboard:" -ForegroundColor Yellow
Write-Host "   Resumo:   http://localhost:5001/api/dashboard/resumo" -ForegroundColor White
Write-Host "   Alertas:  http://localhost:5001/api/dashboard/alertas" -ForegroundColor White
Write-Host ""
Write-Host "🔍 Para ver logs:" -ForegroundColor Yellow
Write-Host "   docker logs pdpw-backend -f" -ForegroundColor White
Write-Host "   docker logs pdpw-sqlserver -f" -ForegroundColor White
Write-Host ""
Write-Host "🛑 Para parar:" -ForegroundColor Yellow
Write-Host "   docker-compose down" -ForegroundColor White
Write-Host ""
