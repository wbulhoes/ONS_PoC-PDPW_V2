# 🧪 TESTES AUTOMATIZADOS - POC PDPw 100%

param(
    [string]$BaseUrl = "http://localhost:5001"
)

Write-Host "==============================================================" -ForegroundColor Cyan
Write-Host "  TESTES AUTOMATIZADOS - POC PDPw 100%" -ForegroundColor Cyan
Write-Host "==============================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "🎯 Base URL: $BaseUrl" -ForegroundColor Yellow
Write-Host ""

$totalTests = 0
$passedTests = 0
$failedTests = 0

function Test-Endpoint {
    param(
        [string]$Method,
        [string]$Endpoint,
        [object]$Body = $null,
        [int]$ExpectedStatus = 200,
        [string]$Description
    )
    
    $script:totalTests++
    
    try {
        $uri = "$BaseUrl$Endpoint"
        
        $params = @{
            Uri = $uri
            Method = $Method
            TimeoutSec = 10
            ContentType = "application/json"
        }
        
        if ($Body) {
            $params.Body = ($Body | ConvertTo-Json -Depth 10)
        }
        
        $response = Invoke-WebRequest @params -SkipHttpErrorCheck
        
        if ($response.StatusCode -eq $ExpectedStatus) {
            Write-Host "    ✅ $Description" -ForegroundColor Green
            $script:passedTests++
            return $response
        } else {
            Write-Host "    ❌ $Description (Status: $($response.StatusCode))" -ForegroundColor Red
            $script:failedTests++
            return $null
        }
    } catch {
        Write-Host "    ❌ $Description (Erro: $($_.Exception.Message))" -ForegroundColor Red
        $script:failedTests++
        return $null
    }
}

# ========================================
# GRUPO 1: DASHBOARD
# ========================================
Write-Host "📊 [1/8] Testando Dashboard..." -ForegroundColor Cyan
Test-Endpoint -Method "GET" -Endpoint "/api/dashboard/resumo" -Description "GET Dashboard Resumo"
Test-Endpoint -Method "GET" -Endpoint "/api/dashboard/metricas/ofertas" -Description "GET Métricas Ofertas"
Test-Endpoint -Method "GET" -Endpoint "/api/dashboard/alertas" -Description "GET Alertas"

# ========================================
# GRUPO 2: USINAS
# ========================================
Write-Host ""
Write-Host "🏭 [2/8] Testando Usinas..." -ForegroundColor Cyan
$usinas = Test-Endpoint -Method "GET" -Endpoint "/api/usinas" -Description "GET Todas Usinas"

if ($usinas -and $usinas.Content) {
    $usinasData = $usinas.Content | ConvertFrom-Json
    if ($usinasData.Count -gt 0) {
        $usinaId = $usinasData[0].id
        Test-Endpoint -Method "GET" -Endpoint "/api/usinas/$usinaId" -Description "GET Usina por ID"
    }
}

# ========================================
# GRUPO 3: EMPRESAS
# ========================================
Write-Host ""
Write-Host "🏢 [3/8] Testando Empresas..." -ForegroundColor Cyan
$empresas = Test-Endpoint -Method "GET" -Endpoint "/api/empresas" -Description "GET Todas Empresas"

if ($empresas -and $empresas.Content) {
    $empresasData = $empresas.Content | ConvertFrom-Json
    if ($empresasData.Count -gt 0) {
        $empresaId = $empresasData[0].id
        Test-Endpoint -Method "GET" -Endpoint "/api/empresas/$empresaId" -Description "GET Empresa por ID"
    }
}

# ========================================
# GRUPO 4: OFERTAS EXPORTAÇÃO
# ========================================
Write-Host ""
Write-Host "📤 [4/8] Testando Ofertas de Exportação..." -ForegroundColor Cyan
Test-Endpoint -Method "GET" -Endpoint "/api/ofertas-exportacao" -Description "GET Todas Ofertas Exportação"
Test-Endpoint -Method "GET" -Endpoint "/api/ofertas-exportacao/pendentes" -Description "GET Ofertas Pendentes"
Test-Endpoint -Method "GET" -Endpoint "/api/ofertas-exportacao/aprovadas" -Description "GET Ofertas Aprovadas"

# Criar nova oferta
if ($usinas -and $usinasData.Count -gt 0) {
    $novaOferta = @{
        usinaId = $usinasData[0].id
        dataOferta = (Get-Date).ToString("yyyy-MM-dd")
        dataPDP = (Get-Date).AddDays(2).ToString("yyyy-MM-dd")
        valorMW = 150.5
        precoMWh = 250.75
        horaInicial = "08:00:00"
        horaFinal = "18:00:00"
        observacoes = "Teste automatizado"
    }
    
    $created = Test-Endpoint -Method "POST" -Endpoint "/api/ofertas-exportacao" -Body $novaOferta -ExpectedStatus 201 -Description "POST Criar Oferta Exportação"
    
    if ($created) {
        $createdData = $created.Content | ConvertFrom-Json
        $ofertaId = $createdData.id
        Test-Endpoint -Method "GET" -Endpoint "/api/ofertas-exportacao/$ofertaId" -Description "GET Oferta Criada"
    }
}

# ========================================
# GRUPO 5: OFERTAS RESPOSTA VOLUNTÁRIA
# ========================================
Write-Host ""
Write-Host "🔄 [5/8] Testando Ofertas Resposta Voluntária..." -ForegroundColor Cyan
Test-Endpoint -Method "GET" -Endpoint "/api/ofertas-resposta-voluntaria" -Description "GET Todas Ofertas RV"
Test-Endpoint -Method "GET" -Endpoint "/api/ofertas-resposta-voluntaria/pendentes" -Description "GET Ofertas RV Pendentes"

# Criar nova oferta RV
if ($empresas -and $empresasData.Count -gt 0) {
    $novaOfertaRV = @{
        empresaId = $empresasData[0].id
        dataOferta = (Get-Date).ToString("yyyy-MM-dd")
        dataPDP = (Get-Date).AddDays(2).ToString("yyyy-MM-dd")
        reducaoDemandaMW = 50.0
        precoMWh = 100.0
        horaInicial = "08:00:00"
        horaFinal = "18:00:00"
        tipoPrograma = "Interruptível"
        observacoes = "Teste automatizado"
    }
    
    Test-Endpoint -Method "POST" -Endpoint "/api/ofertas-resposta-voluntaria" -Body $novaOfertaRV -ExpectedStatus 201 -Description "POST Criar Oferta RV"
}

# ========================================
# GRUPO 6: PREVISÕES EÓLICAS
# ========================================
Write-Host ""
Write-Host "🌬️  [6/8] Testando Previsões Eólicas..." -ForegroundColor Cyan
Test-Endpoint -Method "GET" -Endpoint "/api/previsoes-eolicas" -Description "GET Todas Previsões"

if ($usinas -and $usinasData.Count -gt 0) {
    Test-Endpoint -Method "GET" -Endpoint "/api/previsoes-eolicas/usina/$($usinasData[0].id)" -Description "GET Previsões por Usina"
    
    # Criar nova previsão
    $novaPrevisao = @{
        usinaId = $usinasData[0].id
        dataHoraReferencia = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ss")
        dataHoraPrevista = (Get-Date).AddHours(24).ToString("yyyy-MM-ddTHH:mm:ss")
        geracaoPrevistaMWmed = 85.5
        velocidadeVentoMS = 12.5
        modeloPrevisao = "WRF"
        horizontePrevisaoHoras = 24
        tipoPrevisao = "Curto Prazo"
    }
    
    Test-Endpoint -Method "POST" -Endpoint "/api/previsoes-eolicas" -Body $novaPrevisao -ExpectedStatus 201 -Description "POST Criar Previsão Eólica"
}

# ========================================
# GRUPO 7: ARQUIVOS DADGER
# ========================================
Write-Host ""
Write-Host "📁 [7/8] Testando Arquivos DADGER..." -ForegroundColor Cyan
Test-Endpoint -Method "GET" -Endpoint "/api/arquivosdadger" -Description "GET Todos Arquivos DADGER"
Test-Endpoint -Method "GET" -Endpoint "/api/arquivosdadger/status/Aberto" -Description "GET Arquivos Abertos"
Test-Endpoint -Method "GET" -Endpoint "/api/arquivosdadger/pendentes-aprovacao" -Description "GET Pendentes Aprovação"

# ========================================
# GRUPO 8: DADOS ENERGÉTICOS
# ========================================
Write-Host ""
Write-Host "⚡ [8/8] Testando Dados Energéticos..." -ForegroundColor Cyan
Test-Endpoint -Method "GET" -Endpoint "/api/dadosenergeticos" -Description "GET Todos Dados Energéticos"

# Criar novo dado energético
$novoDado = @{
    dataReferencia = (Get-Date).ToString("yyyy-MM-dd")
    codigoUsina = "TST001"
    producaoMWh = 450.5
    capacidadeDisponivel = 600.0
    status = "Operando"
    energiaVertida = 50.0
    motivoVertimento = "Teste automatizado - excesso de vazão"
}

Test-Endpoint -Method "POST" -Endpoint "/api/dadosenergeticos" -Body $novoDado -ExpectedStatus 201 -Description "POST Criar Dado Energético"

# ========================================
# RESUMO FINAL
# ========================================
Write-Host ""
Write-Host "==============================================================" -ForegroundColor Cyan
Write-Host "  RESUMO DOS TESTES" -ForegroundColor Cyan
Write-Host "==============================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "📊 Total de Testes:  $totalTests" -ForegroundColor White
Write-Host "✅ Testes Passaram:  $passedTests" -ForegroundColor Green
Write-Host "❌ Testes Falharam:  $failedTests" -ForegroundColor Red
Write-Host ""

$successRate = [math]::Round(($passedTests / $totalTests) * 100, 2)
Write-Host "📈 Taxa de Sucesso:  $successRate%" -ForegroundColor $(if ($successRate -ge 90) { "Green" } elseif ($successRate -ge 70) { "Yellow" } else { "Red" })
Write-Host ""

if ($failedTests -eq 0) {
    Write-Host "🎉 TODOS OS TESTES PASSARAM! POC 100% FUNCIONAL!" -ForegroundColor Green
    exit 0
} else {
    Write-Host "⚠️  Alguns testes falharam. Verifique os logs." -ForegroundColor Yellow
    exit 1
}
