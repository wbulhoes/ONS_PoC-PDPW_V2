# Script para converter arquivos para UTF-8
param(
    [string]$Path = ".",
    [string]$Extension = "*.md",
    [switch]$DryRun = $false
)

Write-Host "?? Convertendo arquivos $Extension para UTF-8..." -ForegroundColor Cyan
Write-Host "Diretório: $Path" -ForegroundColor Gray
if ($DryRun) {
    Write-Host "??  MODO DRY-RUN (não fará alterações)" -ForegroundColor Yellow
}
Write-Host ""

$files = Get-ChildItem -Path $Path -Filter $Extension -Recurse -File

$count = 0
$fixed = 0
$errors = 0

foreach ($file in $files) {
    try {
        $count++
        
        # Ler conteúdo com encoding atual
        $content = Get-Content -Path $file.FullName -Raw -Encoding Default
        
        # Verificar se tem caracteres problemáticos
        $hasProblems = $false
        $problems = @()
        
        if ($content -match '[?]') {
            $hasProblems = $true
            $problems += "caractere inválido (?)"
        }
        if ($content -match 'Ã§') {
            $hasProblems = $true
            $problems += "cedilha (ç)"
        }
        if ($content -match 'Ã£o') {
            $hasProblems = $true
            $problems += "til (ão)"
        }
        if ($content -match 'Ã¡') {
            $hasProblems = $true
            $problems += "acento agudo (á)"
        }
        
        if ($hasProblems) {
            Write-Host "  ??  Corrigindo: $($file.Name)" -ForegroundColor Yellow
            Write-Host "     Problemas: $($problems -join ', ')" -ForegroundColor Gray
            
            # Corrigir caracteres
            $content = $content -replace 'Ã§', 'ç'
            $content = $content -replace 'Ã£o', 'ão'
            $content = $content -replace 'Ã£', 'ã'
            $content = $content -replace 'Ã¡', 'á'
            $content = $content -replace 'Ã©', 'é'
            $content = $content -replace 'Ã­', 'í'
            $content = $content -replace 'Ã³', 'ó'
            $content = $content -replace 'Ãº', 'ú'
            $content = $content -replace 'Ã', 'Ã'
            $content = $content -replace 'Ã‡', 'Ç'
            
            if (-not $DryRun) {
                # Salvar como UTF-8 com BOM
                [System.IO.File]::WriteAllText($file.FullName, $content, [System.Text.Encoding]::UTF8)
            }
            
            $fixed++
        } else {
            # Ainda assim, garantir UTF-8
            if (-not $DryRun) {
                [System.IO.File]::WriteAllText($file.FullName, $content, [System.Text.Encoding]::UTF8)
            }
        }
    }
    catch {
        Write-Host "  ? Erro em $($file.Name): $_" -ForegroundColor Red
        $errors++
    }
}

Write-Host ""
Write-Host "?? Resumo:" -ForegroundColor Cyan
Write-Host "  Total de arquivos processados: $count" -ForegroundColor White
Write-Host "  Arquivos com problemas corrigidos: $fixed" -ForegroundColor Yellow
Write-Host "  Erros: $errors" -ForegroundColor $(if ($errors -gt 0) { "Red" } else { "Green" })

if ($DryRun) {
    Write-Host ""
    Write-Host "??  MODO DRY-RUN: Nenhuma alteração foi feita!" -ForegroundColor Yellow
    Write-Host "Execute sem -DryRun para aplicar as correções." -ForegroundColor Gray
} else {
    Write-Host ""
    Write-Host "? Conversão concluída!" -ForegroundColor Green
}
