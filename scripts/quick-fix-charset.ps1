# Script de correção rápida de encoding
# Execute: .\scripts\quick-fix-charset.ps1

Write-Host "🚀 Correção Rápida de Charset/Encoding" -ForegroundColor Cyan
Write-Host "======================================`n" -ForegroundColor Cyan

# 1. Configurar Git
Write-Host "⚙️  [1/4] Configurando Git..." -ForegroundColor Yellow
git config --global core.quotepath false
git config --global gui.encoding utf-8
git config --global i18n.commit.encoding utf-8
git config --global i18n.logoutputencoding utf-8
Write-Host "      ✅ Git configurado para UTF-8`n" -ForegroundColor Green

# 2. Diagnóstico
Write-Host "🔍 [2/4] Executando diagnóstico..." -ForegroundColor Yellow
& "$PSScriptRoot\check-encoding.ps1" -Path "."
Write-Host ""

# 3. Perguntar se quer corrigir
$response = Read-Host "Deseja corrigir os arquivos problemáticos? (S/N)"

if ($response -eq "S" -or $response -eq "s") {
    Write-Host "`n🔧 [3/4] Corrigindo arquivos..." -ForegroundColor Yellow
    
    # Corrigir Markdown
    Write-Host "   Corrigindo arquivos .md..." -ForegroundColor Gray
    & "$PSScriptRoot\fix-encoding.ps1" -Path "docs" -Extension "*.md"
    
    Write-Host "`n   Corrigindo arquivos .txt..." -ForegroundColor Gray
    & "$PSScriptRoot\fix-encoding.ps1" -Path "." -Extension "*.txt"
    
    Write-Host "`n   Corrigindo README..." -ForegroundColor Gray
    & "$PSScriptRoot\fix-encoding.ps1" -Path "." -Extension "README*.md"
    
    Write-Host "`n      ✅ Arquivos corrigidos!`n" -ForegroundColor Green
    
    # 4. Commit
    Write-Host "📝 [4/4] Preparando commit..." -ForegroundColor Yellow
    
    $commitResponse = Read-Host "Deseja fazer commit das correções? (S/N)"
    
    if ($commitResponse -eq "S" -or $commitResponse -eq "s") {
        git add .gitattributes .editorconfig .vscode/settings.json
        git add docs/*.md
        git add *.md
        
        git commit -m "chore: corrige encoding para UTF-8

- Adiciona .gitattributes para normalizar encoding
- Adiciona .editorconfig com regras de formatação
- Configura VS Code para UTF-8
- Corrige caracteres inválidos em arquivos markdown
- Configura Git para UTF-8 global

Resolve problemas de charset/encoding reportados pela equipe."
        
        Write-Host "`n✅ Commit criado com sucesso!" -ForegroundColor Green
        Write-Host "Execute 'git push' para enviar as alterações.`n" -ForegroundColor Gray
    } else {
        Write-Host "`n⚠️  Arquivos corrigidos mas não commitados." -ForegroundColor Yellow
        Write-Host "Execute 'git status' para ver as mudanças.`n" -ForegroundColor Gray
    }
} else {
    Write-Host "`n⚠️  Correção cancelada. Nenhuma alteração foi feita." -ForegroundColor Yellow
}

Write-Host "`n✅ Script concluído!`n" -ForegroundColor Green
Write-Host "📚 Documentação completa: docs/GUIA_CORRIGIR_CHARSET.md" -ForegroundColor Cyan
