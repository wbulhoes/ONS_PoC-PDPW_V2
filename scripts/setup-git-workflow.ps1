# Script para configurar Git workflow com feature/backend como branch principal de trabalho

Write-Host "⚙️  Configurando Git Workflow" -ForegroundColor Cyan
Write-Host "============================`n" -ForegroundColor Cyan

# Configuração da branch atual
$currentBranch = git rev-parse --abbrev-ref HEAD

Write-Host "📍 Branch atual: $currentBranch" -ForegroundColor Yellow
Write-Host ""

# Garantir que estamos na feature/backend
if ($currentBranch -ne "feature/backend") {
    Write-Host "🔄 Mudando para feature/backend..." -ForegroundColor Yellow
    git checkout feature/backend
    Write-Host "   ✅ Agora em feature/backend`n" -ForegroundColor Green
}

# Configurar upstream
Write-Host "🔗 Configurando upstream..." -ForegroundColor Yellow
git branch --set-upstream-to=origin/feature/backend feature/backend
Write-Host "   ✅ Upstream configurado: origin/feature/backend`n" -ForegroundColor Green

# Configurar push padrão
Write-Host "⚙️  Configurando push padrão..." -ForegroundColor Yellow
git config push.default current
Write-Host "   ✅ Push padrão: current (sempre para branch atual)`n" -ForegroundColor Green

# Criar alias úteis
Write-Host "🔧 Criando aliases Git..." -ForegroundColor Yellow

git config alias.sync "!git fetch origin && git merge origin/feature/backend"
Write-Host "   ✅ Alias 'sync' criado (git sync = fetch + merge feature/backend)" -ForegroundColor Green

git config alias.pushf "push origin feature/backend"
Write-Host "   ✅ Alias 'pushf' criado (git pushf = push origin feature/backend)" -ForegroundColor Green

git config alias.pullf "pull origin feature/backend"
Write-Host "   ✅ Alias 'pullf' criado (git pullf = pull origin feature/backend)" -ForegroundColor Green

Write-Host ""

# Resumo da configuração
Write-Host "📋 Resumo da Configuração:" -ForegroundColor Cyan
Write-Host "========================`n" -ForegroundColor Cyan
Write-Host "  Branch de trabalho:  feature/backend" -ForegroundColor White
Write-Host "  Remote:              origin (wbulhoes/ONS_PoC-PDPW_V2)" -ForegroundColor White
Write-Host "  Push padrão:         origin/feature/backend" -ForegroundColor White
Write-Host ""
Write-Host "  Comandos disponíveis:" -ForegroundColor Yellow
Write-Host "    git push            → Faz push para origin/feature/backend" -ForegroundColor Gray
Write-Host "    git pull            → Faz pull de origin/feature/backend" -ForegroundColor Gray
Write-Host "    git sync            → Sincroniza com origin/feature/backend" -ForegroundColor Gray
Write-Host "    git pushf           → Alias para push origin feature/backend" -ForegroundColor Gray
Write-Host "    git pullf           → Alias para pull origin feature/backend" -ForegroundColor Gray
Write-Host ""

Write-Host "✅ Configuração concluída!" -ForegroundColor Green
Write-Host ""
Write-Host "💡 Dica: A partir de agora, use apenas:" -ForegroundColor Cyan
Write-Host "   git add ." -ForegroundColor White
Write-Host "   git commit -m 'mensagem'" -ForegroundColor White
Write-Host "   git push" -ForegroundColor White
Write-Host ""
Write-Host "   O push será feito automaticamente para origin/feature/backend ✅" -ForegroundColor Green
