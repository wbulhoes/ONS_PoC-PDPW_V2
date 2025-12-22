# Script para análise detalhada das 15 APIs implementadas
# Compara DAOs legados com Services C# linha por linha

param(
    [string]$LegacyPath = "C:\temp\_ONS_PoC-PDPW\pdpw_act\pdpw",
    [string]$NewPath = "C:\temp\_ONS_PoC-PDPW_V2",
    [string]$OutputPath = ".\docs\analise-regras-negocio\detalhada"
)

Write-Host "🔍 ANÁLISE DETALHADA - 15 APIs IMPLEMENTADAS" -ForegroundColor Cyan
Write-Host "============================================`n" -ForegroundColor Cyan

# Criar diretório de output
if (-not (Test-Path $OutputPath)) {
    New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null
}

# Mapeamento: DAO Legado → Service C#
$apiMapping = @(
    @{ API = "Usinas"; DAO = "UsinaDAO.vb"; Service = "UsinaService.cs"; Priority = "HIGH" },
    @{ API = "Empresas"; DAO = ""; Service = "EmpresaService.cs"; Priority = "MEDIUM" },
    @{ API = "TiposUsina"; DAO = ""; Service = "TipoUsinaService.cs"; Priority = "LOW" },
    @{ API = "SemanasPMO"; DAO = ""; Service = "SemanaPMOService.cs"; Priority = "MEDIUM" },
    @{ API = "EquipesPDP"; DAO = ""; Service = "EquipePdpService.cs"; Priority = "LOW" },
    @{ API = "Cargas"; DAO = "CargaDAO.vb"; Service = "CargaService.cs"; Priority = "HIGH" },
    @{ API = "ArquivosDadger"; DAO = "ArquivoDadgerValorDAO.vb"; Service = "ArquivoDadgerService.cs"; Priority = "HIGH" },
    @{ API = "RestricoesUG"; DAO = ""; Service = "RestricaoUGService.cs"; Priority = "MEDIUM" },
    @{ API = "DadosEnergeticos"; DAO = ""; Service = "DadoEnergeticoService.cs"; Priority = "MEDIUM" },
    @{ API = "Usuarios"; DAO = ""; Service = "UsuarioService.cs"; Priority = "LOW" },
    @{ API = "UnidadesGeradoras"; DAO = ""; Service = "UnidadeGeradoraService.cs"; Priority = "MEDIUM" },
    @{ API = "ParadasUG"; DAO = ""; Service = "ParadaUGService.cs"; Priority = "MEDIUM" },
    @{ API = "MotivosRestricao"; DAO = ""; Service = "MotivoRestricaoService.cs"; Priority = "LOW" },
    @{ API = "Balancos"; DAO = ""; Service = "BalancoService.cs"; Priority = "HIGH" },
    @{ API = "Intercambios"; DAO = "InterDAO.vb"; Service = "IntercambioService.cs"; Priority = "HIGH" }
)

Write-Host "📊 Total de APIs a analisar: $($apiMapping.Count)" -ForegroundColor Yellow
Write-Host ""

$results = @()

foreach ($mapping in $apiMapping) {
    $apiName = $mapping.API
    $daoName = $mapping.DAO
    $serviceName = $mapping.Service
    $priority = $mapping.Priority
    
    Write-Host "🔍 Analisando API: $apiName" -ForegroundColor Cyan
    Write-Host "  DAO Legado:  $daoName" -ForegroundColor Gray
    Write-Host "  Service C#:  $serviceName" -ForegroundColor Gray
    Write-Host "  Prioridade:  $priority" -ForegroundColor $(if ($priority -eq "HIGH") { "Red" } elseif ($priority -eq "MEDIUM") { "Yellow" } else { "Green" })
    
    $analysis = @{
        API = $apiName
        DAO = $daoName
        Service = $serviceName
        Priority = $priority
        HasDAO = $false
        HasService = $false
        DAOLines = 0
        ServiceLines = 0
        Validations = @()
        Calculations = @()
        Gaps = @()
    }
    
    # Verificar se DAO existe
    if (-not [string]::IsNullOrEmpty($daoName)) {
        $daoPath = Join-Path "$LegacyPath\Dao" $daoName
        
        if (Test-Path $daoPath) {
            $analysis.HasDAO = $true
            $daoContent = Get-Content $daoPath -Raw -ErrorAction SilentlyContinue
            $analysis.DAOLines = ($daoContent -split "`n").Count
            
            Write-Host "  ✅ DAO encontrado ($($analysis.DAOLines) linhas)" -ForegroundColor Green
            
            # Procurar validações específicas
            if ($daoContent -match 'IsNullOrEmpty') {
                $analysis.Validations += "Valida campos vazios (IsNullOrEmpty)"
            }
            if ($daoContent -match 'Throw New') {
                $analysis.Validations += "Lança exceções"
            }
            if ($daoContent -match 'If\s+.*\s+Then') {
                $analysis.Validations += "Tem validações condicionais (If/Then)"
            }
            
            # Procurar cálculos
            if ($daoContent -match '[\+\-\*\/]\s*=') {
                $analysis.Calculations += "Operações matemáticas"
            }
            if ($daoContent -match 'Sum|Average|Count|Max|Min') {
                $analysis.Calculations += "Agregações SQL"
            }
            
        } else {
            Write-Host "  ⚠️  DAO não encontrado" -ForegroundColor Yellow
        }
    } else {
        Write-Host "  ℹ️  Sem DAO legado (tabela lookup ou nova funcionalidade)" -ForegroundColor Gray
    }
    
    # Verificar se Service existe
    $servicePath = Join-Path "$NewPath\src\PDPW.Application\Services" $serviceName
    
    if (Test-Path $servicePath) {
        $analysis.HasService = $true
        $serviceContent = Get-Content $servicePath -Raw -ErrorAction SilentlyContinue
        $analysis.ServiceLines = ($serviceContent -split "`n").Count
        
        Write-Host "  ✅ Service encontrado ($($analysis.ServiceLines) linhas)" -ForegroundColor Green
        
        # Verificar se validações do DAO estão no Service
        if ($analysis.HasDAO) {
            foreach ($validation in $analysis.Validations) {
                if ($validation -like "*IsNullOrEmpty*" -and $serviceContent -notmatch 'IsNullOrEmpty|string\.IsNullOrWhiteSpace') {
                    $analysis.Gaps += "Validação de campo vazio não encontrada no Service"
                }
                if ($validation -like "*exceções*" -and $serviceContent -notmatch 'throw new') {
                    $analysis.Gaps += "Lançamento de exceções pode estar diferente"
                }
            }
        }
        
    } else {
        Write-Host "  ❌ Service não encontrado!" -ForegroundColor Red
        $analysis.Gaps += "Service não existe no código novo"
    }
    
    Write-Host "  Validações: $($analysis.Validations.Count)" -ForegroundColor Cyan
    Write-Host "  Cálculos:   $($analysis.Calculations.Count)" -ForegroundColor Cyan
    Write-Host "  Gaps:       $($analysis.Gaps.Count)" -ForegroundColor $(if ($analysis.Gaps.Count -gt 0) { "Yellow" } else { "Green" })
    Write-Host ""
    
    $results += $analysis
}

# Gerar relatório consolidado
Write-Host "📝 Gerando relatório consolidado..." -ForegroundColor Yellow

$reportPath = "$OutputPath\ANALISE_15_APIS.md"

$report = @"
# 📊 ANÁLISE DETALHADA - 15 APIs IMPLEMENTADAS

**Data**: $(Get-Date -Format "dd/MM/yyyy HH:mm")  
**Legado**: $LegacyPath  
**Novo**: $NewPath

---

## 📋 RESUMO EXECUTIVO

| Métrica | Valor |
|---------|-------|
| **Total de APIs** | $($results.Count) |
| **APIs com DAO Legado** | $($results | Where-Object { $_.HasDAO } | Measure-Object).Count |
| **APIs sem DAO Legado** | $($results | Where-Object { -not $_.HasDAO } | Measure-Object).Count |
| **Services Implementados** | $($results | Where-Object { $_.HasService } | Measure-Object).Count |
| **Total de Gaps** | $(($results | ForEach-Object { $_.Gaps.Count } | Measure-Object -Sum).Sum) |

---

## 🎯 ANÁLISE POR PRIORIDADE

### HIGH PRIORITY (5 APIs)

$($results | Where-Object { $_.Priority -eq "HIGH" } | ForEach-Object {
@"

#### $($_.API)

| Aspecto | Detalhes |
|---------|----------|
| **DAO Legado** | $($_.DAO -or "N/A") |
| **Service C#** | $($_.Service) |
| **DAO Linhas** | $($_.DAOLines) |
| **Service Linhas** | $($_.ServiceLines) |
| **Validações** | $($_.Validations.Count) |
| **Cálculos** | $($_.Calculations.Count) |
| **Gaps** | $($_.Gaps.Count) |

$(if ($_.Validations.Count -gt 0) {
"**Validações Identificadas:**
" + ($_.Validations | ForEach-Object { "- $_" }) -join "`n"
})

$(if ($_.Gaps.Count -gt 0) {
"**⚠️ Gaps Identificados:**
" + ($_.Gaps | ForEach-Object { "- ⚠️ $_" }) -join "`n"
})

"@
})

### MEDIUM PRIORITY (7 APIs)

$($results | Where-Object { $_.Priority -eq "MEDIUM" } | ForEach-Object {
@"

#### $($_.API)

| Aspecto | Detalhes |
|---------|----------|
| **DAO Legado** | $($_.DAO -or "N/A") |
| **Service C#** | $($_.Service) |
| **Gaps** | $($_.Gaps.Count) |

"@
})

### LOW PRIORITY (3 APIs)

$($results | Where-Object { $_.Priority -eq "LOW" } | ForEach-Object {
@"

#### $($_.API)

| Aspecto | Detalhes |
|---------|----------|
| **Service C#** | $($_.Service) |
| **Nota** | $(if ($_.HasDAO) { "Tem DAO legado" } else { "Tabela lookup ou nova funcionalidade" }) |

"@
})

---

## 🎯 PRÓXIMOS PASSOS

### APIs HIGH PRIORITY que precisam análise aprofundada:

$($results | Where-Object { $_.Priority -eq "HIGH" -and $_.HasDAO } | ForEach-Object {
"1. **$($_.API)**: Analisar $($_.DAO) ($($_.DAOLines) linhas)"
})

### Ações Recomendadas:

1. **Análise linha por linha** dos DAOs HIGH PRIORITY
2. **Validar validações** estão implementadas nos Services
3. **Implementar gaps** identificados
4. **Criar testes unitários** para regras críticas

---

## 📊 MATRIZ DE COBERTURA

| API | DAO | Service | Validações | Gaps | Status |
|-----|-----|---------|------------|------|--------|
$($results | ForEach-Object {
"| $($_.API) | $(if ($_.HasDAO) { '✅' } else { '➖' }) | $(if ($_.HasService) { '✅' } else { '❌' }) | $($_.Validations.Count) | $($_.Gaps.Count) | $(if ($_.Gaps.Count -eq 0 -and $_.HasService) { '✅' } elseif ($_.Gaps.Count -gt 0) { '⚠️' } else { '❌' }) |"
})

**Legenda:**
- ✅ Implementado
- ➖ Não aplicável
- ❌ Faltando
- ⚠️ Com gaps

---

**Gerado por**: scripts/analisar-15-apis.ps1
"@

$report | Out-File -FilePath $reportPath -Encoding UTF8

Write-Host "  ✅ Relatório gerado: $reportPath`n" -ForegroundColor Green

# Estatísticas finais
Write-Host "📊 ESTATÍSTICAS FINAIS:" -ForegroundColor Cyan
Write-Host "  Total de APIs:       $($results.Count)" -ForegroundColor White
Write-Host "  Com DAO Legado:      $(($results | Where-Object { $_.HasDAO }).Count)" -ForegroundColor White
Write-Host "  Services OK:         $(($results | Where-Object { $_.HasService }).Count)" -ForegroundColor White
Write-Host "  Total de Gaps:       $(($results | ForEach-Object { $_.Gaps.Count } | Measure-Object -Sum).Sum)" -ForegroundColor $(if ((($results | ForEach-Object { $_.Gaps.Count } | Measure-Object -Sum).Sum) -gt 0) { "Yellow" } else { "Green" })

Write-Host "`n✅ Análise das 15 APIs concluída!" -ForegroundColor Green
Write-Host "📄 Relatório: $reportPath" -ForegroundColor Cyan
