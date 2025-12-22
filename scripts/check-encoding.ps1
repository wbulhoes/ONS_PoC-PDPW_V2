# Script de diagnóstico de encoding
param(
    [string]$Path = ".",
    [switch]$Detailed = $false
)

Write-Host "?? Diagnóstico de Encoding" -ForegroundColor Cyan
Write-Host "==========================`n" -ForegroundColor Cyan

# Função para detectar encoding
function Get-FileEncoding {
    param([string]$FilePath)
    
    $bytes = [System.IO.File]::ReadAllBytes($FilePath)
    
    # UTF-8 BOM
    if ($bytes.Length -ge 3 -and $bytes[0] -eq 0xEF -and $bytes[1] -eq 0xBB -and $bytes[2] -eq 0xBF) {
        return "UTF-8 BOM"
    }
    
    # UTF-16 LE BOM
    if ($bytes.Length -ge 2 -and $bytes[0] -eq 0xFF -and $bytes[1] -eq 0xFE) {
        return "UTF-16 LE"
    }
    
    # UTF-16 BE BOM
    if ($bytes.Length -ge 2 -and $bytes[0] -eq 0xFE -and $bytes[1] -eq 0xFF) {
        return "UTF-16 BE"
    }
    
    # ASCII/UTF-8 sem BOM
    $allAscii = $true
    foreach ($byte in $bytes) {
        if ($byte -gt 127) {
            $allAscii = $false
            break
        }
    }
    
    if ($allAscii) {
        return "ASCII"
    } else {
        return "UTF-8 sem BOM (ou outro)"
    }
}

# Analisar arquivos
$extensions = @("*.cs", "*.md", "*.json", "*.xml", "*.txt", "*.ps1")
$results = @{}

Write-Host "?? Analisando arquivos em: $Path`n" -ForegroundColor Gray

foreach ($ext in $extensions) {
    Write-Host "  Procurando $ext..." -ForegroundColor Yellow
    $files = Get-ChildItem -Path $Path -Filter $ext -Recurse -File -ErrorAction SilentlyContinue
    
    foreach ($file in $files) {
        # Ignorar pastas node_modules, bin, obj
        if ($file.FullName -match '(node_modules|\\bin\\|\\obj\\|\.git\\)') {
            continue
        }
        
        $encoding = Get-FileEncoding -FilePath $file.FullName
        
        if (-not $results.ContainsKey($encoding)) {
            $results[$encoding] = @()
        }
        
        $results[$encoding] += @{
            Path = $file.FullName
            Name = $file.Name
            Extension = $file.Extension
        }
    }
}

# Mostrar resultados
Write-Host "`n?? Resumo por Encoding:" -ForegroundColor Cyan
Write-Host "========================`n" -ForegroundColor Cyan

foreach ($encoding in ($results.Keys | Sort-Object)) {
    $count = $results[$encoding].Count
    $icon = switch ($encoding) {
        "UTF-8 BOM" { "?" }
        "UTF-8 sem BOM (ou outro)" { "?? " }
        "ASCII" { "?" }
        default { "?" }
    }
    
    Write-Host "$icon $encoding ($count arquivos):" -ForegroundColor $(if ($encoding -eq "UTF-8 BOM" -or $encoding -eq "ASCII") { "Green" } else { "Yellow" })
    
    if ($Detailed) {
        $results[$encoding] | ForEach-Object {
            Write-Host "    - $($_.Name)" -ForegroundColor Gray
        }
    } else {
        $results[$encoding] | Select-Object -First 3 | ForEach-Object {
            Write-Host "    - $($_.Name)" -ForegroundColor Gray
        }
        if ($count -gt 3) {
            Write-Host "    ... e mais $($count - 3) arquivos" -ForegroundColor DarkGray
        }
    }
    Write-Host ""
}

# Procurar caracteres problemáticos
Write-Host "?? Procurando caracteres inválidos...`n" -ForegroundColor Cyan

$problematicFiles = @()
$patterns = @{
    '[?]' = 'Caractere inválido (?)'
    'Ã§' = 'Cedilha quebrada (ç)'
    'Ã£o' = 'Til quebrado (ão)'
    'Ã¡' = 'Acento agudo quebrado (á)'
    'Ã©' = 'Acento agudo quebrado (é)'
}

Get-ChildItem -Path $Path -Include @("*.md", "*.cs", "*.txt") -Recurse -File -ErrorAction SilentlyContinue | ForEach-Object {
    # Ignorar pastas especiais
    if ($_.FullName -match '(node_modules|\\bin\\|\\obj\\|\.git\\)') {
        return
    }
    
    $content = Get-Content $_.FullName -Raw -Encoding Default -ErrorAction SilentlyContinue
    $foundProblems = @()
    
    foreach ($pattern in $patterns.Keys) {
        if ($content -match $pattern) {
            $foundProblems += $patterns[$pattern]
        }
    }
    
    if ($foundProblems.Count -gt 0) {
        $problematicFiles += @{
            Path = $_.FullName
            Name = $_.Name
            Problems = $foundProblems
        }
    }
}

if ($problematicFiles.Count -gt 0) {
    Write-Host "? Arquivos com caracteres problemáticos:`n" -ForegroundColor Red
    
    $problematicFiles | ForEach-Object {
        Write-Host "  ?? $($_.Name)" -ForegroundColor Yellow
        Write-Host "     Caminho: $($_.Path)" -ForegroundColor Gray
        Write-Host "     Problemas:" -ForegroundColor Gray
        $_.Problems | ForEach-Object {
            Write-Host "       - $_" -ForegroundColor DarkYellow
        }
        Write-Host ""
    }
    
    Write-Host "?? Solução: Execute o script de correção:" -ForegroundColor Cyan
    Write-Host "   .\scripts\fix-encoding.ps1 -Path `"$Path`"`n" -ForegroundColor White
} else {
    Write-Host "? Nenhum arquivo com caracteres problemáticos encontrado!`n" -ForegroundColor Green
}

# Verificar configuração do Git
Write-Host "??  Configuração do Git:" -ForegroundColor Cyan
Write-Host "========================`n" -ForegroundColor Cyan

$gitConfigs = @{
    "core.quotepath" = "false"
    "gui.encoding" = "utf-8"
    "i18n.commit.encoding" = "utf-8"
    "i18n.logoutputencoding" = "utf-8"
}

foreach ($config in $gitConfigs.Keys) {
    $value = git config --global --get $config 2>$null
    $expected = $gitConfigs[$config]
    
    if ($value -eq $expected) {
        Write-Host "  ? $config = $value" -ForegroundColor Green
    } else {
        Write-Host "  ? $config = $value (esperado: $expected)" -ForegroundColor Red
        Write-Host "     Execute: git config --global $config $expected" -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "?? Resumo Final:" -ForegroundColor Cyan
Write-Host "===============`n" -ForegroundColor Cyan
Write-Host "  Total de arquivos analisados: $(($results.Values | ForEach-Object { $_.Count } | Measure-Object -Sum).Sum)" -ForegroundColor White
Write-Host "  Arquivos com problemas: $($problematicFiles.Count)" -ForegroundColor $(if ($problematicFiles.Count -gt 0) { "Red" } else { "Green" })
Write-Host "  Encodings diferentes de UTF-8: $(($results.Keys | Where-Object { $_ -ne 'UTF-8 BOM' -and $_ -ne 'ASCII' }).Count)" -ForegroundColor Yellow

if ($problematicFiles.Count -gt 0) {
    Write-Host "`n??  AÇÃO NECESSÁRIA: Execute o script de correção!" -ForegroundColor Yellow
} else {
    Write-Host "`n? Tudo OK! Encoding configurado corretamente." -ForegroundColor Green
}
