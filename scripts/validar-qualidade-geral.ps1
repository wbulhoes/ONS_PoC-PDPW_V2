# Script de Validação de Qualidade Geral da POC
# Executa todas as validações e gera dashboard consolidado

param(
    [switch]$Verbose,
    [switch]$ExportReport
)

$ErrorActionPreference = "Continue"

Write-Host "🏆 VALIDAÇÃO DE QUALIDADE GERAL - POC PDPw" -ForegroundColor Cyan
Write-Host "==========================================`n" -ForegroundColor Cyan

$results = @{
    Backend = @{ Score = 0; Details = @() }
    RegrasNegocio = @{ Score = 0; Details = @() }
    Validacoes = @{ Score = 0; Details = @() }
    Testes = @{ Score = 0; Details = @() }
    BancoDados = @{ Score = 0; Details = @() }
    Documentacao = @{ Score = 0; Details = @() }
    Frontend = @{ Score = 0; Details = @() }
}

# ============================================================================
# 1. VALIDAÇÃO DE BACKEND
# ============================================================================

Write-Host "📊 [1/7] Validando Backend..." -ForegroundColor Yellow

try {
    # Verificar se projeto compila
    Write-Host "  Compilando projeto..." -ForegroundColor Gray
    $buildOutput = dotnet build src/PDPW.API/PDPW.API.csproj -c Release --no-incremental 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        $results.Backend.Score += 40
        $results.Backend.Details += "✅ Projeto compila sem erros"
    } else {
        $results.Backend.Details += "❌ Projeto tem erros de compilação"
    }
    
    # Contar warnings
    $warnings = ($buildOutput | Select-String "warning").Count
    if ($warnings -eq 0) {
        $results.Backend.Score += 30
        $results.Backend.Details += "✅ Sem warnings de compilação"
    } elseif ($warnings -lt 10) {
        $results.Backend.Score += 20
        $results.Backend.Details += "⚠️  $warnings warnings (aceitável)"
    } else {
        $results.Backend.Details += "❌ $warnings warnings (muitos!)"
    }
    
    # Contar Services
    $services = (Get-ChildItem -Path "src/PDPW.Application/Services" -Filter "*.cs" -Recurse).Count
    if ($services -ge 15) {
        $results.Backend.Score += 30
        $results.Backend.Details += "✅ $services Services implementados"
    } else {
        $results.Backend.Score += 15
        $results.Backend.Details += "⚠️  Apenas $services Services (esperado 15+)"
    }
    
} catch {
    $results.Backend.Details += "❌ Erro ao validar backend: $_"
}

Write-Host "  Backend Score: $($results.Backend.Score)/100" -ForegroundColor $(if ($results.Backend.Score -ge 80) { "Green" } elseif ($results.Backend.Score -ge 60) { "Yellow" } else { "Red" })

# ============================================================================
# 2. VALIDAÇÃO DE REGRAS DE NEGÓCIO
# ============================================================================

Write-Host "`n📊 [2/7] Validando Regras de Negócio..." -ForegroundColor Yellow

try {
    # Verificar se análise de regras existe
    $analiseRegras = Test-Path "docs/analise-regras-negocio/RESULTADO_FINAL_ANALISE.md"
    
    if ($analiseRegras) {
        $results.RegrasNegocio.Score += 50
        $results.RegrasNegocio.Details += "✅ Análise de regras de negócio documentada"
        
        # Ler análise para verificar cobertura
        $conteudo = Get-Content "docs/analise-regras-negocio/RESULTADO_FINAL_ANALISE.md" -Raw
        
        if ($conteudo -match "95%|100%") {
            $results.RegrasNegocio.Score += 50
            $results.RegrasNegocio.Details += "✅ Cobertura de regras >= 95%"
        } else {
            $results.RegrasNegocio.Score += 25
            $results.RegrasNegocio.Details += "⚠️  Cobertura de regras < 95%"
        }
    } else {
        $results.RegrasNegocio.Details += "❌ Análise de regras não encontrada"
    }
    
} catch {
    $results.RegrasNegocio.Details += "❌ Erro ao validar regras: $_"
}

Write-Host "  Regras de Negócio Score: $($results.RegrasNegocio.Score)/100" -ForegroundColor $(if ($results.RegrasNegocio.Score -ge 80) { "Green" } elseif ($results.RegrasNegocio.Score -ge 60) { "Yellow" } else { "Red" })

# ============================================================================
# 3. VALIDAÇÃO DE VALIDAÇÕES
# ============================================================================

Write-Host "`n📊 [3/7] Validando Validações Implementadas..." -ForegroundColor Yellow

try {
    # Verificar se validações foram implementadas
    $validacoesDoc = Test-Path "docs/analise-regras-negocio/IMPLEMENTACAO_VALIDACOES.md"
    
    if ($validacoesDoc) {
        $results.Validacoes.Score += 50
        $results.Validacoes.Details += "✅ Documentação de validações existe"
        
        # Verificar se Services têm validações
        $servicesComValidacao = 0
        $servicesToCheck = @("UsinaService.cs", "CargaService.cs", "ArquivoDadgerService.cs", "IntercambioService.cs")
        
        foreach ($service in $servicesToCheck) {
            $servicePath = "src/PDPW.Application/Services/$service"
            if (Test-Path $servicePath) {
                $content = Get-Content $servicePath -Raw
                if ($content -match "ArgumentException|IsNullOrWhiteSpace|Failure") {
                    $servicesComValidacao++
                }
            }
        }
        
        if ($servicesComValidacao -eq 4) {
            $results.Validacoes.Score += 50
            $results.Validacoes.Details += "✅ $servicesComValidacao/4 Services com validações"
        } else {
            $results.Validacoes.Score += 25
            $results.Validacoes.Details += "⚠️  Apenas $servicesComValidacao/4 Services com validações"
        }
    } else {
        $results.Validacoes.Details += "❌ Documentação de validações não encontrada"
    }
    
} catch {
    $results.Validacoes.Details += "❌ Erro ao validar validações: $_"
}

Write-Host "  Validações Score: $($results.Validacoes.Score)/100" -ForegroundColor $(if ($results.Validacoes.Score -ge 80) { "Green" } elseif ($results.Validacoes.Score -ge 60) { "Yellow" } else { "Red" })

# ============================================================================
# 4. VALIDAÇÃO DE TESTES
# ============================================================================

Write-Host "`n📊 [4/7] Validando Testes Automatizados..." -ForegroundColor Yellow

try {
    # Executar testes
    Write-Host "  Executando testes..." -ForegroundColor Gray
    $testOutput = dotnet test --no-build --verbosity quiet 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        $results.Testes.Score += 40
        $results.Testes.Details += "✅ Todos os testes passando"
    } else {
        $results.Testes.Details += "❌ Alguns testes falhando"
    }
    
    # Contar testes
    $testFiles = (Get-ChildItem -Path "tests" -Filter "*Tests.cs" -Recurse).Count
    if ($testFiles -ge 10) {
        $results.Testes.Score += 30
        $results.Testes.Details += "✅ $testFiles arquivos de teste"
    } elseif ($testFiles -ge 5) {
        $results.Testes.Score += 15
        $results.Testes.Details += "⚠️  $testFiles arquivos de teste (esperado 10+)"
    } else {
        $results.Testes.Details += "❌ Apenas $testFiles arquivos de teste"
    }
    
    # Cobertura de código (se coverlet estiver configurado)
    # TODO: Implementar quando tiver cobertura
    $results.Testes.Score += 10
    $results.Testes.Details += "⚠️  Cobertura de código não medida"
    
} catch {
    $results.Testes.Details += "❌ Erro ao validar testes: $_"
}

Write-Host "  Testes Score: $($results.Testes.Score)/100" -ForegroundColor $(if ($results.Testes.Score -ge 80) { "Green" } elseif ($results.Testes.Score -ge 60) { "Yellow" } else { "Red" })

# ============================================================================
# 5. VALIDAÇÃO DE BANCO DE DADOS
# ============================================================================

Write-Host "`n📊 [5/7] Validando Banco de Dados..." -ForegroundColor Yellow

try {
    # Verificar migrations
    $migrations = (Get-ChildItem -Path "src/PDPW.Infrastructure/Data/Migrations" -Filter "*.cs").Count
    
    if ($migrations -gt 0) {
        $results.BancoDados.Score += 40
        $results.BancoDados.Details += "✅ $migrations migrations encontradas"
    } else {
        $results.BancoDados.Details += "❌ Nenhuma migration encontrada"
    }
    
    # Verificar entities
    $entities = (Get-ChildItem -Path "src/PDPW.Domain/Entities" -Filter "*.cs").Count
    
    if ($entities -ge 30) {
        $results.BancoDados.Score += 40
        $results.BancoDados.Details += "✅ $entities entidades criadas"
    } else {
        $results.BancoDados.Score += 20
        $results.BancoDados.Details += "⚠️  $entities entidades (esperado 30+)"
    }
    
    # Verificar seeder
    $seeder = Test-Path "src/PDPW.Infrastructure/Data/Seeders"
    
    if ($seeder) {
        $results.BancoDados.Score += 20
        $results.BancoDados.Details += "✅ Seed data configurado"
    } else {
        $results.BancoDados.Details += "❌ Seed data não encontrado"
    }
    
} catch {
    $results.BancoDados.Details += "❌ Erro ao validar BD: $_"
}

Write-Host "  Banco de Dados Score: $($results.BancoDados.Score)/100" -ForegroundColor $(if ($results.BancoDados.Score -ge 80) { "Green" } elseif ($results.BancoDados.Score -ge 60) { "Yellow" } else { "Red" })

# ============================================================================
# 6. VALIDAÇÃO DE DOCUMENTAÇÃO
# ============================================================================

Write-Host "`n📊 [6/7] Validando Documentação..." -ForegroundColor Yellow

try {
    $docsEssenciais = @(
        "README.md",
        "docs/README_BACKEND.md",
        "docs/analise-regras-negocio/RESULTADO_FINAL_ANALISE.md",
        "docs/FRAMEWORK_EXCELENCIA.md"
    )
    
    $docsEncontrados = 0
    foreach ($doc in $docsEssenciais) {
        if (Test-Path $doc) {
            $docsEncontrados++
        }
    }
    
    $percentualDocs = ($docsEncontrados / $docsEssenciais.Count) * 100
    $results.Documentacao.Score = $percentualDocs
    $results.Documentacao.Details += "✅ $docsEncontrados/$($docsEssenciais.Count) documentos essenciais"
    
    # Verificar Swagger
    $swaggerConfig = Test-Path "src/PDPW.API/Program.cs"
    if ($swaggerConfig) {
        $content = Get-Content "src/PDPW.API/Program.cs" -Raw
        if ($content -match "Swagger|OpenApi") {
            $results.Documentacao.Details += "✅ Swagger configurado"
        }
    }
    
} catch {
    $results.Documentacao.Details += "❌ Erro ao validar documentação: $_"
}

Write-Host "  Documentação Score: $($results.Documentacao.Score)/100" -ForegroundColor $(if ($results.Documentacao.Score -ge 80) { "Green" } elseif ($results.Documentacao.Score -ge 60) { "Yellow" } else { "Red" })

# ============================================================================
# 7. VALIDAÇÃO DE FRONTEND
# ============================================================================

Write-Host "`n📊 [7/7] Validando Frontend..." -ForegroundColor Yellow

try {
    # Verificar se React app existe
    $reactApp = Test-Path "pdpw-react/package.json"
    
    if ($reactApp) {
        $results.Frontend.Score += 30
        $results.Frontend.Details += "✅ Projeto React existe"
        
        # Verificar componentes
        $componentes = (Get-ChildItem -Path "pdpw-react/src/components" -Filter "*.tsx" -Recurse -ErrorAction SilentlyContinue).Count
        
        if ($componentes -gt 0) {
            $results.Frontend.Score += 40
            $results.Frontend.Details += "✅ $componentes componentes criados"
        } else {
            $results.Frontend.Details += "⚠️  Nenhum componente criado ainda"
        }
        
        # Verificar TypeScript
        $tsConfig = Test-Path "pdpw-react/tsconfig.json"
        if ($tsConfig) {
            $results.Frontend.Score += 30
            $results.Frontend.Details += "✅ TypeScript configurado"
        }
        
    } else {
        $results.Frontend.Details += "❌ Projeto React não encontrado"
    }
    
} catch {
    $results.Frontend.Details += "❌ Erro ao validar frontend: $_"
}

Write-Host "  Frontend Score: $($results.Frontend.Score)/100" -ForegroundColor $(if ($results.Frontend.Score -ge 80) { "Green" } elseif ($results.Frontend.Score -ge 60) { "Yellow" } else { "Red" })

# ============================================================================
# DASHBOARD FINAL
# ============================================================================

Write-Host "`n`n🏆 DASHBOARD DE QUALIDADE GERAL" -ForegroundColor Cyan
Write-Host "================================`n" -ForegroundColor Cyan

$totalScore = [math]::Round((
    $results.Backend.Score +
    $results.RegrasNegocio.Score +
    $results.Validacoes.Score +
    $results.Testes.Score +
    $results.BancoDados.Score +
    $results.Documentacao.Score +
    $results.Frontend.Score
) / 7)

$scoreColor = if ($totalScore -ge 85) { "Green" } elseif ($totalScore -ge 70) { "Yellow" } else { "Red" }

Write-Host "📊 SCORE GERAL: $totalScore/100" -ForegroundColor $scoreColor -NoNewline
Write-Host " $(if ($totalScore -ge 85) { '⭐⭐⭐⭐⭐' } elseif ($totalScore -ge 70) { '⭐⭐⭐⭐' } elseif ($totalScore -ge 50) { '⭐⭐⭐' } else { '⭐⭐' })"

Write-Host "`nDetalhamento por Camada:`n" -ForegroundColor White

foreach ($key in $results.Keys) {
    $score = $results[$key].Score
    $color = if ($score -ge 80) { "Green" } elseif ($score -ge 60) { "Yellow" } else { "Red" }
    
    Write-Host "  $key`: " -NoNewline
    Write-Host "$score/100" -ForegroundColor $color
    
    if ($Verbose) {
        foreach ($detail in $results[$key].Details) {
            Write-Host "    $detail" -ForegroundColor Gray
        }
    }
}

Write-Host "`n"

# Recomendações
Write-Host "💡 RECOMENDAÇÕES:" -ForegroundColor Cyan

if ($results.Testes.Score -lt 80) {
    Write-Host "  🔴 CRÍTICO: Aumentar cobertura de testes (atual: $($results.Testes.Score)%)" -ForegroundColor Red
}

if ($results.Frontend.Score -lt 60) {
    Write-Host "  🔴 CRÍTICO: Desenvolver frontend (atual: $($results.Frontend.Score)%)" -ForegroundColor Red
}

if ($results.BancoDados.Score -lt 80) {
    Write-Host "  ⚠️  Revisar banco de dados (atual: $($results.BancoDados.Score)%)" -ForegroundColor Yellow
}

if ($totalScore -ge 85) {
    Write-Host "`n✅ POC em excelente estado! Continue assim! 🎉" -ForegroundColor Green
} elseif ($totalScore -ge 70) {
    Write-Host "`n⚠️  POC em bom estado, mas precisa de melhorias." -ForegroundColor Yellow
} else {
    Write-Host "`n🔴 POC precisa de atenção urgente!" -ForegroundColor Red
}

# Exportar relatório se solicitado
if ($ExportReport) {
    $reportPath = "docs/relatorios/qualidade-$(Get-Date -Format 'yyyy-MM-dd-HHmm').md"
    
    $reportContent = @"
# Relatório de Qualidade - $(Get-Date -Format 'dd/MM/yyyy HH:mm')

## Score Geral: $totalScore/100

## Detalhamento

$(foreach ($key in $results.Keys) {
"### $key`: $($results[$key].Score)/100

$($results[$key].Details | ForEach-Object { "- $_" })
"
})

## Gerado por
Script: validar-qualidade-geral.ps1
"@

    New-Item -ItemType Directory -Path "docs/relatorios" -Force -ErrorAction SilentlyContinue | Out-Null
    $reportContent | Out-File -FilePath $reportPath -Encoding UTF8
    
    Write-Host "`n📄 Relatório exportado: $reportPath" -ForegroundColor Cyan
}

Write-Host ""
