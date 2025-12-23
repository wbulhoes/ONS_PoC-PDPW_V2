# Script de Validação Rápida para QA
# Valida se os bugs reportados foram corrigidos na versão atual

param(
    [string]$BaseUrl = "http://localhost:5001",
    [switch]$Verbose
)

$ErrorActionPreference = "Continue"

Write-Host "`n🧪 VALIDAÇÃO DE BUGS - QA AUTOMATION" -ForegroundColor Cyan
Write-Host "===================================`n" -ForegroundColor Cyan

$results = @{
    ArquivosDadger = @()
    RestricoesUG = @()
}

$totalTests = 0
$passedTests = 0
$failedTests = 0

# Função auxiliar para testar endpoint
function Test-Endpoint {
    param(
        [string]$Method,
        [string]$Url,
        [object]$Body = $null,
        [int]$ExpectedStatus,
        [string]$ExpectedMessage = $null
    )
    
    try {
        $headers = @{ "Content-Type" = "application/json" }
        
        $response = if ($Body) {
            Invoke-RestMethod -Uri $Url -Method $Method -Body ($Body | ConvertTo-Json) -Headers $headers -StatusCode

 $ExpectedStatus
        } else {
            Invoke-RestMethod -Uri $Url -Method $Method -Headers $headers -StatusCode $ExpectedStatus
        }
        
        if ($Verbose) {
            Write-Host "    Response: $($response | ConvertTo-Json -Depth 2)" -ForegroundColor Gray
        }
        
        return @{
            Success = $true
            Response = $response
            Status = $ExpectedStatus
        }
    }
    catch {
        $actualStatus = $_.Exception.Response.StatusCode.value__
        
        if ($actualStatus -eq $ExpectedStatus) {
            return @{
                Success = $true
                Response = $null
                Status = $actualStatus
            }
        }
        
        return @{
            Success = $false
            Response = $null
            Status = $actualStatus
            Error = $_.Exception.Message
        }
    }
}

# ============================================================================
# TESTES: ArquivosDadger
# ============================================================================

Write-Host "📁 [1/2] Validando API ArquivosDadger..." -ForegroundColor Yellow
Write-Host ""

# Teste 1: Criar arquivo com SemanaPMO válida
Write-Host "  [1/6] Criar arquivo DADGER com SemanaPMO válida..." -NoNewline
$totalTests++

$body = @{
    nomeArquivo = "dadger_teste_qa_$(Get-Date -Format 'yyyyMMddHHmmss').dat"
    caminhoArquivo = "/uploads/teste_qa.dat"
    dataImportacao = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ss")
    semanaPMOId = 1
    observacoes = "Teste QA - Validação correção bugs"
}

$result = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/arquivosdadger" -Body $body -ExpectedStatus 201

if ($result.Success) {
    Write-Host " ✅ PASSOU" -ForegroundColor Green
    $passedTests++
    $arquivoId = $result.Response.id
} else {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
    Write-Host "    Erro: $($result.Error)" -ForegroundColor Red
}

# Teste 2: Validar SemanaPMO inválida (999)
Write-Host "  [2/6] Validar SemanaPMO inválida (999)..." -NoNewline
$totalTests++

$body = @{
    nomeArquivo = "dadger_invalido.dat"
    caminhoArquivo = "/uploads/invalido.dat"
    dataImportacao = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ss")
    semanaPMOId = 999
}

$result = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/arquivosdadger" -Body $body -ExpectedStatus 400

if ($result.Success) {
    Write-Host " ✅ PASSOU" -ForegroundColor Green
    $passedTests++
} else {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
    Write-Host "    Esperado: 400, Recebido: $($result.Status)" -ForegroundColor Red
}

# Teste 3: Marcar como processado
if ($arquivoId) {
    Write-Host "  [3/6] Marcar arquivo como processado..." -NoNewline
    $totalTests++
    
    $result = Test-Endpoint -Method "PATCH" -Url "$BaseUrl/api/arquivosdadger/$arquivoId/processar" -ExpectedStatus 200
    
    if ($result.Success -and $result.Response.processado -eq $true) {
        Write-Host " ✅ PASSOU" -ForegroundColor Green
        $passedTests++
    } else {
        Write-Host " ❌ FALHOU" -ForegroundColor Red
        $failedTests++
        Write-Host "    Campo 'processado' deveria ser true" -ForegroundColor Red
    }
} else {
    Write-Host "  [3/6] Marcar como processado... ⏭️  PULADO (arquivo não foi criado)" -ForegroundColor Yellow
}

# Teste 4: Filtrar por semana PMO
Write-Host "  [4/6] Filtrar arquivos por semana PMO..." -NoNewline
$totalTests++

$result = Test-Endpoint -Method "GET" -Url "$BaseUrl/api/arquivosdadger/semana/1" -ExpectedStatus 200

if ($result.Success) {
    Write-Host " ✅ PASSOU" -ForegroundColor Green
    $passedTests++
    if ($Verbose) {
        Write-Host "    Registros retornados: $($result.Response.Count)" -ForegroundColor Gray
    }
} else {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# Teste 5: Listar todos os arquivos
Write-Host "  [5/6] Listar todos os arquivos..." -NoNewline
$totalTests++

$result = Test-Endpoint -Method "GET" -Url "$BaseUrl/api/arquivosdadger" -ExpectedStatus 200

if ($result.Success) {
    Write-Host " ✅ PASSOU" -ForegroundColor Green
    $passedTests++
    if ($Verbose) {
        Write-Host "    Total de arquivos: $($result.Response.Count)" -ForegroundColor Gray
    }
} else {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# Teste 6: Soft delete
if ($arquivoId) {
    Write-Host "  [6/6] Soft delete do arquivo..." -NoNewline
    $totalTests++
    
    $result = Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/arquivosdadger/$arquivoId" -ExpectedStatus 204
    
    if ($result.Success) {
        Write-Host " ✅ PASSOU" -ForegroundColor Green
        $passedTests++
    } else {
        Write-Host " ❌ FALHOU" -ForegroundColor Red
        $failedTests++
    }
} else {
    Write-Host "  [6/6] Soft delete... ⏭️  PULADO (arquivo não foi criado)" -ForegroundColor Yellow
}

Write-Host ""

# ============================================================================
# TESTES: RestricoesUG
# ============================================================================

Write-Host "🔧 [2/2] Validando API RestricoesUG..." -ForegroundColor Yellow
Write-Host ""

# Teste 1: Criar restrição com datas válidas
Write-Host "  [1/4] Criar restrição com datas válidas..." -NoNewline
$totalTests++

$body = @{
    unidadeGeradoraId = 1
    dataInicio = (Get-Date).ToString("yyyy-MM-dd")
    dataFim = (Get-Date).AddDays(7).ToString("yyyy-MM-dd")
    motivoRestricaoId = 1
    potenciaRestrita = 150.00
    observacoes = "Teste QA - Validação correção bugs"
}

$result = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/restricoesug" -Body $body -ExpectedStatus 201

if ($result.Success) {
    Write-Host " ✅ PASSOU" -ForegroundColor Green
    $passedTests++
    $restricaoId = $result.Response.id
} else {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
    Write-Host "    Erro: $($result.Error)" -ForegroundColor Red
}

# Teste 2: Validar dataFim < dataInicio
Write-Host "  [2/4] Validar dataFim < dataInicio (deve falhar)..." -NoNewline
$totalTests++

$body = @{
    unidadeGeradoraId = 1
    dataInicio = (Get-Date).AddDays(7).ToString("yyyy-MM-dd")
    dataFim = (Get-Date).ToString("yyyy-MM-dd")
    motivoRestricaoId = 1
    potenciaRestrita = 150.00
}

$result = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/restricoesug" -Body $body -ExpectedStatus 400

if ($result.Success) {
    Write-Host " ✅ PASSOU" -ForegroundColor Green
    $passedTests++
} else {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
    Write-Host "    Esperado: 400, Recebido: $($result.Status)" -ForegroundColor Red
}

# Teste 3: Buscar restrições ativas
Write-Host "  [3/4] Buscar restrições ativas..." -NoNewline
$totalTests++

$dataReferencia = (Get-Date).ToString("yyyy-MM-dd")
$result = Test-Endpoint -Method "GET" -Url "$BaseUrl/api/restricoesug/ativas?dataReferencia=$dataReferencia" -ExpectedStatus 200

if ($result.Success) {
    Write-Host " ✅ PASSOU" -ForegroundColor Green
    $passedTests++
    if ($Verbose) {
        Write-Host "    Restrições ativas: $($result.Response.Count)" -ForegroundColor Gray
    }
} else {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# Teste 4: Soft delete
if ($restricaoId) {
    Write-Host "  [4/4] Soft delete da restrição..." -NoNewline
    $totalTests++
    
    $result = Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/restricoesug/$restricaoId" -ExpectedStatus 204
    
    if ($result.Success) {
        Write-Host " ✅ PASSOU" -ForegroundColor Green
        $passedTests++
    } else {
        Write-Host " ❌ FALHOU" -ForegroundColor Red
        $failedTests++
    }
} else {
    Write-Host "  [4/4] Soft delete... ⏭️  PULADO (restrição não foi criada)" -ForegroundColor Yellow
}

Write-Host ""

# ============================================================================
# RELATÓRIO FINAL
# ============================================================================

Write-Host "📊 RELATÓRIO FINAL" -ForegroundColor Cyan
Write-Host "==================`n" -ForegroundColor Cyan

$successRate = if ($totalTests -gt 0) { [math]::Round(($passedTests / $totalTests) * 100, 2) } else { 0 }

Write-Host "Total de Testes:  $totalTests" -ForegroundColor White
Write-Host "Testes Passaram:  $passedTests " -NoNewline
Write-Host "✅" -ForegroundColor Green
Write-Host "Testes Falharam:  $failedTests " -NoNewline
if ($failedTests -gt 0) {
    Write-Host "❌" -ForegroundColor Red
} else {
    Write-Host "✅" -ForegroundColor Green
}
Write-Host "Taxa de Sucesso:  $successRate%" -ForegroundColor $(if ($successRate -eq 100) { "Green" } elseif ($successRate -ge 80) { "Yellow" } else { "Red" })

Write-Host ""

if ($failedTests -eq 0) {
    Write-Host "✅ VALIDAÇÃO CONCLUÍDA COM SUCESSO!" -ForegroundColor Green
    Write-Host "   Todos os bugs reportados foram corrigidos." -ForegroundColor Green
    Write-Host ""
    Write-Host "🎯 Próximos passos:" -ForegroundColor Cyan
    Write-Host "   1. Atualizar issue no Jira: RESOLVED" -ForegroundColor White
    Write-Host "   2. Documentar validação no Confluence" -ForegroundColor White
    Write-Host "   3. Fechar ticket de bugs" -ForegroundColor White
} else {
    Write-Host "⚠️  ALGUNS TESTES FALHARAM" -ForegroundColor Yellow
    Write-Host "   Verifique os logs acima para detalhes." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "🔍 Possíveis causas:" -ForegroundColor Cyan
    Write-Host "   1. API não está rodando em $BaseUrl" -ForegroundColor White
    Write-Host "   2. Banco de dados sem seed data" -ForegroundColor White
    Write-Host "   3. Docker containers não foram recriados" -ForegroundColor White
    Write-Host ""
    Write-Host "💡 Tente:" -ForegroundColor Cyan
    Write-Host "   docker-compose down && docker-compose up --build -d" -ForegroundColor White
}

Write-Host ""

# Retornar código de saída apropriado
exit $failedTests
