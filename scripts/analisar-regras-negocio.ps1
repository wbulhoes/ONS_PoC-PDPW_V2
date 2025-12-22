# Script para analisar regras de negócio do código legado VB.NET
# e comparar com o código migrado C#

param(
    [string]$LegacyPath = "C:\temp\_ONS_PoC-PDPW\pdpw_act",
    [string]$NewPath = "C:\temp\_ONS_PoC-PDPW_V2",
    [string]$OutputPath = ".\docs\analise-regras-negocio"
)

Write-Host "🔍 ANÁLISE DE REGRAS DE NEGÓCIO - LEGADO vs NOVO" -ForegroundColor Cyan
Write-Host "================================================`n" -ForegroundColor Cyan

# Verificar se os diretórios existem
if (-not (Test-Path $LegacyPath)) {
    Write-Host "❌ Erro: Diretório legado não encontrado: $LegacyPath" -ForegroundColor Red
    exit 1
}

if (-not (Test-Path $NewPath)) {
    Write-Host "❌ Erro: Diretório novo não encontrado: $NewPath" -ForegroundColor Red
    exit 1
}

# Criar diretório de output
if (-not (Test-Path $OutputPath)) {
    New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null
}

Write-Host "📁 Diretórios:" -ForegroundColor Yellow
Write-Host "  Legado: $LegacyPath" -ForegroundColor Gray
Write-Host "  Novo:   $NewPath" -ForegroundColor Gray
Write-Host "  Output: $OutputPath`n" -ForegroundColor Gray

# Estrutura para armazenar análise
$analysis = @{
    DAOs = @()
    Business = @()
    Regras = @()
    Gaps = @()
}

# 1. Analisar DAOs do legado
Write-Host "🔍 [1/4] Analisando DAOs do legado..." -ForegroundColor Yellow

$daos = Get-ChildItem -Path "$LegacyPath\Dao" -Filter "*.vb" -ErrorAction SilentlyContinue

foreach ($dao in $daos) {
    Write-Host "  Analisando: $($dao.Name)" -ForegroundColor Gray
    
    $content = Get-Content $dao.FullName -Raw -ErrorAction SilentlyContinue
    
    # Procurar por validações
    $validations = @()
    if ($content -match 'If\s+.*Then') {
        $validations += "Contém validações (If/Then)"
    }
    if ($content -match 'Throw\s+New') {
        $validations += "Lança exceções"
    }
    if ($content -match 'IsNullOrEmpty') {
        $validations += "Valida campos vazios"
    }
    
    # Procurar por cálculos
    $calculations = @()
    if ($content -match '[\+\-\*\/]\s*=') {
        $calculations += "Contém operações matemáticas"
    }
    if ($content -match 'Sum|Average|Count') {
        $calculations += "Contém agregações"
    }
    
    # Procurar por stored procedures
    $storedProcs = @()
    if ($content -match 'CommandType\.StoredProcedure') {
        $storedProcs += "Usa Stored Procedures"
    }
    
    $analysis.DAOs += @{
        Nome = $dao.Name
        Caminho = $dao.FullName
        Validacoes = $validations
        Calculos = $calculations
        StoredProcs = $storedProcs
        Linhas = ($content -split "`n").Count
    }
}

Write-Host "  ✅ $($daos.Count) DAOs analisados`n" -ForegroundColor Green

# 2. Analisar Business do legado
Write-Host "🔍 [2/4] Analisando Business do legado..." -ForegroundColor Yellow

$business = Get-ChildItem -Path "$LegacyPath\Business" -Filter "*.vb" -ErrorAction SilentlyContinue

foreach ($biz in $business) {
    Write-Host "  Analisando: $($biz.Name)" -ForegroundColor Gray
    
    $content = Get-Content $biz.FullName -Raw -ErrorAction SilentlyContinue
    
    # Procurar por regras de negócio
    $rules = @()
    if ($content -match 'Function\s+Validar') {
        $rules += "Método Validar()"
    }
    if ($content -match 'Function\s+Calcular') {
        $rules += "Método Calcular()"
    }
    if ($content -match 'BusinessException') {
        $rules += "Usa BusinessException"
    }
    
    $analysis.Business += @{
        Nome = $biz.Name
        Caminho = $biz.FullName
        Regras = $rules
        Linhas = ($content -split "`n").Count
    }
}

Write-Host "  ✅ $($business.Count) Business analisados`n" -ForegroundColor Green

# 3. Procurar por regras específicas (palavras-chave)
Write-Host "🔍 [3/4] Procurando regras específicas..." -ForegroundColor Yellow

$keywords = @(
    'Validar',
    'Calcular',
    'Verificar',
    'Autorizar',
    'Permissao',
    'Restricao',
    'Minimo',
    'Maximo',
    'Obrigatorio',
    'BusinessException'
)

foreach ($keyword in $keywords) {
    Write-Host "  Procurando: $keyword" -ForegroundColor Gray
    
    $matches = Get-ChildItem -Path $LegacyPath -Recurse -Include "*.vb" -ErrorAction SilentlyContinue |
        Select-String -Pattern $keyword -CaseSensitive:$false |
        Select-Object -First 10
    
    if ($matches) {
        $analysis.Regras += @{
            Keyword = $keyword
            Ocorrencias = $matches.Count
            Arquivos = ($matches | Select-Object -ExpandProperty Path -Unique).Count
        }
    }
}

Write-Host "  ✅ Procura por palavras-chave concluída`n" -ForegroundColor Green

# 4. Comparar com Services do novo código
Write-Host "🔍 [4/4] Comparando com Services do novo código..." -ForegroundColor Yellow

$services = Get-ChildItem -Path "$NewPath\src\PDPW.Application\Services" -Filter "*.cs" -ErrorAction SilentlyContinue

foreach ($service in $services) {
    $serviceName = $service.BaseName -replace 'Service', ''
    
    # Procurar DAO correspondente no legado
    $daoCorrespondente = $analysis.DAOs | Where-Object { $_.Nome -like "*$serviceName*" }
    
    if (-not $daoCorrespondente) {
        $analysis.Gaps += @{
            Tipo = "Service sem DAO correspondente"
            Nome = $service.Name
            Descricao = "Service $($service.Name) não tem DAO correspondente no legado"
        }
    }
}

Write-Host "  ✅ Comparação concluída`n" -ForegroundColor Green

# Gerar relatório
Write-Host "📝 Gerando relatório..." -ForegroundColor Yellow

$reportPath = "$OutputPath\RELATORIO_REGRAS_NEGOCIO.md"

$report = @"
# 📊 RELATÓRIO: ANÁLISE DE REGRAS DE NEGÓCIO

**Data**: $(Get-Date -Format "dd/MM/yyyy HH:mm")  
**Legado**: $LegacyPath  
**Novo**: $NewPath

---

## 📋 RESUMO EXECUTIVO

| Categoria | Quantidade |
|-----------|------------|
| **DAOs Analisados** | $($analysis.DAOs.Count) |
| **Business Analisados** | $($analysis.Business.Count) |
| **Regras Identificadas** | $($analysis.Regras.Count) |
| **Gaps Encontrados** | $($analysis.Gaps.Count) |

---

## 🔍 DAOs ANALISADOS

$($analysis.DAOs | ForEach-Object {
@"

### $($_.Nome)

| Aspecto | Detalhes |
|---------|----------|
| **Linhas de Código** | $($_.Linhas) |
| **Validações** | $($_.Validacoes -join ', ' -or 'Nenhuma') |
| **Cálculos** | $($_.Calculos -join ', ' -or 'Nenhum') |
| **Stored Procedures** | $($_.StoredProcs -join ', ' -or 'Nenhum') |

"@
})

---

## 💼 BUSINESS ANALISADOS

$($analysis.Business | ForEach-Object {
@"

### $($_.Nome)

| Aspecto | Detalhes |
|---------|----------|
| **Linhas de Código** | $($_.Linhas) |
| **Regras de Negócio** | $($_.Regras -join ', ' -or 'Nenhuma identificada') |

"@
})

---

## 🔎 REGRAS ESPECÍFICAS (Palavras-chave)

$($analysis.Regras | ForEach-Object {
@"

### Keyword: **$($_.Keyword)**

- **Ocorrências**: $($_.Ocorrencias)
- **Arquivos afetados**: $($_.Arquivos)

"@
})

---

## ⚠️ GAPS IDENTIFICADOS

$($analysis.Gaps | ForEach-Object {
@"

### $($_.Tipo)

- **Nome**: $($_.Nome)
- **Descrição**: $($_.Descricao)

"@
})

---

## 🎯 PRÓXIMOS PASSOS

1. **Analisar DAOs individualmente** - Ler código de cada DAO para extrair regras
2. **Validar Services** - Confirmar que regras estão nos Services C#
3. **Implementar gaps** - Adicionar regras faltantes
4. **Criar testes** - Garantir comportamento correto

---

**Gerado por**: scripts/analisar-regras-negocio.ps1
"@

$report | Out-File -FilePath $reportPath -Encoding UTF8

Write-Host "  ✅ Relatório gerado: $reportPath`n" -ForegroundColor Green

# Mostrar resumo
Write-Host "📊 RESUMO:" -ForegroundColor Cyan
Write-Host "  DAOs:     $($analysis.DAOs.Count)" -ForegroundColor White
Write-Host "  Business: $($analysis.Business.Count)" -ForegroundColor White
Write-Host "  Regras:   $($analysis.Regras.Count)" -ForegroundColor White
Write-Host "  Gaps:     $($analysis.Gaps.Count)" -ForegroundColor $(if ($analysis.Gaps.Count -gt 0) { "Yellow" } else { "Green" })

Write-Host "`n✅ Análise concluída!" -ForegroundColor Green
Write-Host "📄 Relatório: $reportPath" -ForegroundColor Cyan
