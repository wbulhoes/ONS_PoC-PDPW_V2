# Script de diagnóstico Docker

Write-Host "?? DIAGNÓSTICO DOCKER" -ForegroundColor Cyan
Write-Host "=" * 60
Write-Host ""

# 1. Verificar se Docker Desktop está rodando
Write-Host "1. Verificando processo Docker Desktop..." -ForegroundColor Yellow
$dockerProcess = Get-Process "Docker Desktop" -ErrorAction SilentlyContinue

if ($dockerProcess) {
    Write-Host "? Docker Desktop está rodando (PID: $($dockerProcess.Id))" -ForegroundColor Green
} else {
    Write-Host "? Docker Desktop NÃO está rodando!" -ForegroundColor Red
    Write-Host "   SOLUÇÃO: Abra o Docker Desktop e aguarde inicializar" -ForegroundColor Yellow
    exit 1
}

Write-Host ""

# 2. Verificar se Docker daemon está acessível
Write-Host "2. Verificando Docker daemon..." -ForegroundColor Yellow
try {
    $version = docker version --format "{{.Server.Version}}" 2>$null
    if ($version) {
        Write-Host "? Docker daemon respondendo (versão: $version)" -ForegroundColor Green
    } else {
        throw "Sem resposta"
    }
} catch {
    Write-Host "? Docker daemon NÃO está acessível!" -ForegroundColor Red
    Write-Host "   SOLUÇÃO: Aguarde Docker Desktop inicializar completamente" -ForegroundColor Yellow
    Write-Host "   Status deve estar: 'Docker Desktop is running'" -ForegroundColor Yellow
    exit 1
}

Write-Host ""

# 3. Verificar info do Docker
Write-Host "3. Verificando informações do Docker..." -ForegroundColor Yellow
try {
    $info = docker info 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "? Docker info acessível" -ForegroundColor Green
    } else {
        throw "Erro ao obter info"
    }
} catch {
    Write-Host "? Docker info falhou!" -ForegroundColor Red
    Write-Host "   SOLUÇÃO: Reinicie o Docker Desktop" -ForegroundColor Yellow
    exit 1
}

Write-Host ""

# 4. Verificar WSL2 (Windows)
Write-Host "4. Verificando WSL2..." -ForegroundColor Yellow
try {
    $wsl = wsl --list --verbose 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "? WSL2 está funcionando" -ForegroundColor Green
    } else {
        Write-Host "??  WSL2 pode não estar configurado" -ForegroundColor Yellow
    }
} catch {
    Write-Host "??  WSL2 não detectado" -ForegroundColor Yellow
}

Write-Host ""

# 5. Verificar portas necessárias
Write-Host "5. Verificando portas necessárias..." -ForegroundColor Yellow

$ports = @(5000, 1433)
foreach ($port in $ports) {
    $connection = Get-NetTCPConnection -LocalPort $port -ErrorAction SilentlyContinue
    if ($connection) {
        Write-Host "??  Porta $port está EM USO!" -ForegroundColor Yellow
        Write-Host "   Processo: $($connection.OwningProcess)" -ForegroundColor Gray
    } else {
        Write-Host "? Porta $port está LIVRE" -ForegroundColor Green
    }
}

Write-Host ""

# 6. Verificar containers existentes
Write-Host "6. Verificando containers existentes..." -ForegroundColor Yellow
$containers = docker ps -a --format "{{.Names}}" 2>$null

if ($containers -like "*pdpw*") {
    Write-Host "??  Containers PDPW existem" -ForegroundColor Yellow
    Write-Host "   Containers: $containers" -ForegroundColor Gray
    Write-Host "   RECOMENDAÇÃO: Limpar com 'docker-compose down -v'" -ForegroundColor Yellow
} else {
    Write-Host "? Nenhum container PDPW existente" -ForegroundColor Green
}

Write-Host ""
Write-Host "=" * 60
Write-Host "?? DIAGNÓSTICO COMPLETO" -ForegroundColor Cyan
Write-Host ""

# Verificação final
$dockerOk = $dockerProcess -and ($LASTEXITCODE -eq 0)

if ($dockerOk) {
    Write-Host "? DOCKER ESTÁ PRONTO!" -ForegroundColor Green
    Write-Host ""
    Write-Host "?? Próximo passo:" -ForegroundColor Yellow
    Write-Host "   .\docker-start.ps1" -ForegroundColor Cyan
} else {
    Write-Host "? DOCKER NÃO ESTÁ PRONTO" -ForegroundColor Red
    Write-Host ""
    Write-Host "?? PASSOS PARA CORRIGIR:" -ForegroundColor Yellow
    Write-Host "   1. Abrir Docker Desktop" -ForegroundColor White
    Write-Host "   2. Aguardar status: 'Docker Desktop is running'" -ForegroundColor White
    Write-Host "   3. Executar este script novamente" -ForegroundColor White
    Write-Host "   4. Se persistir, reiniciar o computador" -ForegroundColor White
}

Write-Host ""
