# Script para limpar branches desnecessárias
# Mantém apenas: main, develop, feature/backend

Write-Host "🧹 Limpeza de Branches - Repositório Origin" -ForegroundColor Cyan
Write-Host "============================================`n" -ForegroundColor Cyan

# Branches que devem ser mantidas
$keepBranches = @('main', 'develop', 'feature/backend')

Write-Host "📋 Branches que serão MANTIDAS:" -ForegroundColor Green
$keepBranches | ForEach-Object {
    Write-Host "  ✅ $_" -ForegroundColor Green
}
Write-Host ""

# Listar branches remotas do origin
Write-Host "🔍 Verificando branches remotas no origin..." -ForegroundColor Yellow
$remoteBranches = git branch -r | Where-Object { $_ -match 'origin/' } | ForEach-Object { $_.Trim() -replace 'origin/', '' } | Where-Object { $_ -notmatch 'HEAD' }

Write-Host "📊 Branches encontradas no origin:" -ForegroundColor Cyan
$remoteBranches | ForEach-Object {
    if ($keepBranches -contains $_) {
        Write-Host "  ✅ $_" -ForegroundColor Green
    } else {
        Write-Host "  ❌ $_ (será removida)" -ForegroundColor Red
    }
}
Write-Host ""

# Branches a serem removidas
$branchesToDelete = $remoteBranches | Where-Object { $keepBranches -notcontains $_ }

if ($branchesToDelete.Count -eq 0) {
    Write-Host "✅ Nenhuma branch para remover! Repositório já está limpo." -ForegroundColor Green
    exit 0
}

Write-Host "⚠️  ATENÇÃO: As seguintes branches serão REMOVIDAS do origin:" -ForegroundColor Yellow
$branchesToDelete | ForEach-Object {
    Write-Host "  ❌ $_" -ForegroundColor Red
}
Write-Host ""

$response = Read-Host "Deseja continuar com a remoção? (S/N)"

if ($response -ne "S" -and $response -ne "s") {
    Write-Host "`n⚠️  Operação cancelada pelo usuário." -ForegroundColor Yellow
    exit 0
}

Write-Host "`n🗑️  Removendo branches..." -ForegroundColor Yellow

foreach ($branch in $branchesToDelete) {
    try {
        Write-Host "  Removendo: $branch..." -ForegroundColor Gray
        git push origin --delete $branch 2>&1 | Out-Null
        Write-Host "    ✅ $branch removida com sucesso" -ForegroundColor Green
    }
    catch {
        Write-Host "    ❌ Erro ao remover $branch`: $_" -ForegroundColor Red
    }
}

Write-Host "`n✅ Limpeza concluída!" -ForegroundColor Green
Write-Host ""
Write-Host "📋 Branches mantidas no origin:" -ForegroundColor Cyan
git branch -r | Where-Object { $_ -match 'origin/' -and $_ -notmatch 'HEAD' } | ForEach-Object {
    Write-Host "  $_" -ForegroundColor Green
}
