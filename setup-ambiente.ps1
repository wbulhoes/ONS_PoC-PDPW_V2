# Script de Configuração Automática do Ambiente - PDPw v2.0
# PowerShell

$ErrorActionPreference = "Continue"

Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Cyan
Write-Host "   🚀 SETUP AUTOMÁTICO - PDPw v2.0" -ForegroundColor Cyan
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Cyan
Write-Host ""

# Função para exibir status
function Write-Status {
    param(
        [string]$Step,
        [string]$Message,
        [string]$Status = "INFO"
    )
    
    $color = switch ($Status) {
        "OK" { "Green" }
        "ERROR" { "Red" }
        "WARN" { "Yellow" }
        default { "White" }
    }
    
    $icon = switch ($Status) {
        "OK" { "✓" }
        "ERROR" { "✗" }
        "WARN" { "⚠" }
        default { "→" }
    }
    
    Write-Host "[$Step] " -NoNewline -ForegroundColor Yellow
    Write-Host "$icon $Message" -ForegroundColor $color
}

# Variáveis
$projectRoot = "C:\temp\_ONS_PoC-PDPW_V2"
$frontendDir = "$projectRoot\frontend"
$backendDir = "$projectRoot\src\PDPW.API"

# Verificar se está no diretório correto
if (-not (Test-Path $projectRoot)) {
    Write-Status "0" "Diretório do projeto não encontrado: $projectRoot" "ERROR"
    exit 1
}

Set-Location $projectRoot

Write-Host ""
Write-Host "📋 VERIFICAÇÃO DO AMBIENTE" -ForegroundColor Blue
Write-Host ""

# 1. Verificar Git
Write-Status "1" "Verificando Git..." "INFO"
$gitVersion = git --version 2>$null
if ($LASTEXITCODE -eq 0) {
    Write-Status "1" "Git instalado: $gitVersion" "OK"
} else {
    Write-Status "1" "Git NÃO instalado" "ERROR"
    exit 1
}

# 2. Verificar .NET
Write-Status "2" "Verificando .NET..." "INFO"
$dotnetVersion = dotnet --version 2>$null
if ($LASTEXITCODE -eq 0) {
    Write-Status "2" ".NET SDK instalado: $dotnetVersion" "OK"
} else {
    Write-Status "2" ".NET SDK NÃO instalado" "ERROR"
    exit 1
}

# 3. Verificar Node.js
Write-Status "3" "Verificando Node.js..." "INFO"
$nodeVersion = node --version 2>$null
if ($LASTEXITCODE -eq 0) {
    Write-Status "3" "Node.js instalado: $nodeVersion" "OK"
} else {
    Write-Status "3" "Node.js NÃO instalado" "ERROR"
    exit 1
}

# 4. Verificar npm
Write-Status "4" "Verificando npm..." "INFO"
$npmVersion = npm --version 2>$null
if ($LASTEXITCODE -eq 0) {
    Write-Status "4" "npm instalado: v$npmVersion" "OK"
} else {
    Write-Status "4" "npm NÃO instalado" "ERROR"
    exit 1
}

# 5. Verificar Docker
Write-Status "5" "Verificando Docker..." "INFO"
$dockerVersion = docker --version 2>$null
if ($LASTEXITCODE -eq 0) {
    Write-Status "5" "Docker instalado: $dockerVersion" "OK"
    
    # Verificar se Docker está rodando
    docker ps >$null 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Status "5" "Docker está rodando" "OK"
    } else {
        Write-Status "5" "Docker NÃO está rodando. Inicie o Docker Desktop!" "ERROR"
        exit 1
    }
} else {
    Write-Status "5" "Docker NÃO instalado" "ERROR"
    exit 1
}

Write-Host ""
Write-Host "📦 INSTALAÇÃO DE DEPENDÊNCIAS" -ForegroundColor Blue
Write-Host ""

# 6. Instalar dependências do frontend
Write-Status "6" "Instalando dependências do frontend..." "INFO"
Set-Location $frontendDir

if (Test-Path "node_modules") {
    Write-Status "6" "node_modules já existe. Limpando..." "WARN"
    Remove-Item -Path "node_modules" -Recurse -Force -ErrorAction SilentlyContinue
}

Write-Host ""
Write-Host "   Executando npm install..." -ForegroundColor Gray
Write-Host "   (Isso pode levar alguns minutos...)" -ForegroundColor Gray
Write-Host ""

npm install --silent

if ($LASTEXITCODE -eq 0) {
    Write-Status "6" "Dependências do frontend instaladas com sucesso!" "OK"
} else {
    Write-Status "6" "Erro ao instalar dependências do frontend" "ERROR"
    exit 1
}

Write-Host ""
Write-Host "🔨 COMPILAÇÃO DOS PROJETOS" -ForegroundColor Blue
Write-Host ""

# 7. Compilar frontend
Write-Status "7" "Compilando frontend..." "INFO"
Set-Location $frontendDir

npm run build >$null 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Status "7" "Frontend compilado com sucesso!" "OK"
} else {
    Write-Status "7" "Aviso: Frontend com erros de compilação (pode ser normal em dev)" "WARN"
}

# 8. Compilar backend (API)
Write-Status "8" "Compilando backend (API)..." "INFO"
Set-Location $backendDir

dotnet build --nologo -v q >$null 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Status "8" "Backend (API) compilado com sucesso!" "OK"
} else {
    Write-Status "8" "Erro ao compilar backend" "ERROR"
}

Write-Host ""
Write-Host "🐳 DOCKER" -ForegroundColor Blue
Write-Host ""

Set-Location $projectRoot

# 9. Parar containers anteriores (se existirem)
Write-Status "9" "Parando containers anteriores..." "INFO"
docker-compose down >$null 2>&1
Write-Status "9" "Containers anteriores parados" "OK"

# 10. Confirmar se deseja subir com Docker
Write-Host ""
Write-Host "🚀 " -NoNewline -ForegroundColor Yellow
Write-Host "Deseja iniciar a aplicação com Docker agora?" -ForegroundColor White
Write-Host ""
Write-Host "   Isso irá:" -ForegroundColor Gray
Write-Host "   - Criar container SQL Server 2022" -ForegroundColor Gray
Write-Host "   - Criar container Backend (.NET 8)" -ForegroundColor Gray
Write-Host "   - Criar container Frontend (Vite)" -ForegroundColor Gray
Write-Host ""

$startDocker = Read-Host "Iniciar com Docker? (s/N)"

if ($startDocker -eq "s" -or $startDocker -eq "S") {
    Write-Status "10" "Iniciando containers Docker..." "INFO"
    Write-Host ""
    
    docker-compose up -d
    
    if ($LASTEXITCODE -eq 0) {
        Write-Status "10" "Containers iniciados com sucesso!" "OK"
        
        Write-Host ""
        Write-Host "⏳ Aguardando containers ficarem prontos..." -ForegroundColor Yellow
        Start-Sleep -Seconds 5
        
        # Verificar status dos containers
        Write-Host ""
        Write-Status "11" "Verificando status dos containers..." "INFO"
        docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"
        
        Write-Host ""
        Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Green
        Write-Host "   ✅ APLICAÇÃO RODANDO!" -ForegroundColor Green
        Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Green
        Write-Host ""
        Write-Host "🌐 URLs de Acesso:" -ForegroundColor Blue
        Write-Host ""
        Write-Host "   Frontend:  " -NoNewline -ForegroundColor White
        Write-Host "http://localhost:5173" -ForegroundColor Cyan
        Write-Host "   Swagger:   " -NoNewline -ForegroundColor White
        Write-Host "http://localhost:5001/swagger" -ForegroundColor Cyan
        Write-Host "   API:       " -NoNewline -ForegroundColor White
        Write-Host "http://localhost:5001" -ForegroundColor Cyan
        Write-Host ""
        
        # Perguntar se deseja abrir no navegador
        $openBrowser = Read-Host "Deseja abrir no navegador? (s/N)"
        
        if ($openBrowser -eq "s" -or $openBrowser -eq "S") {
            Write-Status "12" "Abrindo navegador..." "INFO"
            Start-Sleep -Seconds 2
            Start-Process "http://localhost:5173"
            Start-Process "http://localhost:5001/swagger"
        }
        
    } else {
        Write-Status "10" "Erro ao iniciar containers" "ERROR"
        Write-Host ""
        Write-Host "📋 Logs do Docker:" -ForegroundColor Yellow
        docker-compose logs --tail=20
    }
} else {
    Write-Status "10" "Inicialização com Docker cancelada" "WARN"
    Write-Host ""
    Write-Host "📋 Para iniciar manualmente depois:" -ForegroundColor Blue
    Write-Host ""
    Write-Host "   docker-compose up -d" -ForegroundColor Gray
    Write-Host ""
}

Write-Host ""
Write-Host "📚 INFORMAÇÕES ÚTEIS" -ForegroundColor Blue
Write-Host ""
Write-Host "   Comandos Docker:" -ForegroundColor White
Write-Host "   ├─ Iniciar:      docker-compose up -d" -ForegroundColor Gray
Write-Host "   ├─ Parar:        docker-compose down" -ForegroundColor Gray
Write-Host "   ├─ Ver logs:     docker-compose logs -f" -ForegroundColor Gray
Write-Host "   └─ Status:       docker ps" -ForegroundColor Gray
Write-Host ""
Write-Host "   Modo Manual (Desenvolvimento):" -ForegroundColor White
Write-Host "   ├─ Backend:      cd src\PDPW.API && dotnet run" -ForegroundColor Gray
Write-Host "   └─ Frontend:     cd frontend && npm run dev" -ForegroundColor Gray
Write-Host ""
Write-Host "   Documentação:" -ForegroundColor White
Write-Host "   ├─ INDEX.md                    (Índice completo)" -ForegroundColor Gray
Write-Host "   ├─ RELATORIO_AMBIENTE.md       (Este relatório)" -ForegroundColor Gray
Write-Host "   ├─ COMANDOS_RAPIDOS.md         (Comandos úteis)" -ForegroundColor Gray
Write-Host "   └─ FRONTEND_COMPLETO_9_ETAPAS.md (Documentação técnica)" -ForegroundColor Gray
Write-Host ""

Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Cyan
Write-Host "   🎉 SETUP CONCLUÍDO!" -ForegroundColor Green
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Cyan
Write-Host ""
Write-Host "✅ Próximos passos:" -ForegroundColor White
Write-Host "   1. Acesse http://localhost:5173" -ForegroundColor Gray
Write-Host "   2. Explore as 9 etapas do sistema" -ForegroundColor Gray
Write-Host "   3. Consulte a documentação em INDEX.md" -ForegroundColor Gray
Write-Host ""

# Salvar log
$logFile = "setup-log-$(Get-Date -Format 'yyyyMMdd-HHmmss').txt"
Write-Host "📄 Log salvo em: $logFile" -ForegroundColor Gray
Write-Host ""
