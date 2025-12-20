# Script para abrir o ambiente de desenvolvimento PDPW PoC
# Abre Visual Studio com a solução backend e um terminal para o frontend

param(
    [switch]$SkipNpmInstall = $false,
    [switch]$NoFrontend = $false
)

$projectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$solutionPath = Join-Path $projectRoot "PDPW.sln"
$frontendPath = Join-Path $projectRoot "frontend"

Write-Host "🚀 Iniciando ambiente PDPW PoC..." -ForegroundColor Cyan
Write-Host ""

# Verifica se a solução existe
if (-not (Test-Path $solutionPath)) {
    Write-Host "❌ Solução não encontrada em: $solutionPath" -ForegroundColor Red
    exit 1
}

# Abre Visual Studio Community com a solução
Write-Host "📂 Abrindo Visual Studio Community com PDPW.sln..." -ForegroundColor Green
Start-Process -FilePath "devenv.exe" -ArgumentList "`"$solutionPath`""

# Aguarda um pouco para o VS abrir
Start-Sleep -Seconds 2

# Se não foi especificado -NoFrontend, abre o terminal para o frontend
if (-not $NoFrontend) {
    Write-Host "📱 Abrindo terminal para frontend..." -ForegroundColor Green
    
    # Tenta usar Windows Terminal se disponível, senão usa PowerShell
    $wtPath = "$env:LOCALAPPDATA\Microsoft\WindowsApps\wt.exe"
    
    if (Test-Path $wtPath) {
        # Windows Terminal (mais moderno)
        Start-Process -FilePath $wtPath -ArgumentList "new-tab", "-d", "`"$frontendPath`""
    } else {
        # PowerShell tradicional
        Start-Process -FilePath "powershell.exe" -WorkingDirectory $frontendPath -ArgumentList "-NoExit", "-Command", "Write-Host '🎨 Terminal Frontend aberto. Execute: npm install (se necessário) e depois npm run dev' -ForegroundColor Cyan"
    }
    
    # Instala dependências npm se solicitado
    if (-not $SkipNpmInstall) {
        Write-Host ""
        Write-Host "📦 Verificando dependências npm..." -ForegroundColor Yellow
        
        if (-not (Test-Path (Join-Path $frontendPath "node_modules"))) {
            Write-Host "   ℹ️  node_modules não encontrado. Execute no terminal do frontend:" -ForegroundColor Yellow
            Write-Host "   npm install" -ForegroundColor Cyan
        } else {
            Write-Host "   ✅ node_modules encontrado" -ForegroundColor Green
        }
    }
}

Write-Host ""
Write-Host "✅ Ambiente pronto!" -ForegroundColor Green
Write-Host ""
Write-Host "📝 Próximos passos:" -ForegroundColor Cyan
Write-Host "   1️⃣  No Visual Studio: defina 'PDPW.API' como projeto de inicialização e pressione F5" -ForegroundColor White
Write-Host "   2️⃣  No terminal frontend: execute 'npm run dev'" -ForegroundColor White
Write-Host "   3️⃣  Acesse:" -ForegroundColor White
Write-Host "      - Backend API: http://localhost:5000/swagger" -ForegroundColor Cyan
Write-Host "      - Frontend: http://localhost:5173" -ForegroundColor Cyan
Write-Host ""
