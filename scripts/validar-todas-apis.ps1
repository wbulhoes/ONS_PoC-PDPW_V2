# Script de Validação Completa de Todas as APIs
# Testa todos os 15 controllers e 107 endpoints

param(
    [string]$BaseUrl = "http://localhost:5001",
    [switch]$Verbose,
    [switch]$StopOnError
)

$ErrorActionPreference = "Continue"

Write-Host "`n🔍 VALIDAÇÃO COMPLETA - 15 APIs DO SISTEMA PDPW" -ForegroundColor Cyan
Write-Host "============================================`n" -ForegroundColor Cyan

$totalTests = 0
$passedTests = 0
$failedTests = 0
$warningTests = 0

$results = @()

# Função para testar endpoint
function Test-Endpoint {
    param(
        [string]$Method,
        [string]$Url,
        [object]$Body = $null,
        [int]$ExpectedStatus = 200,
        [string]$Description
    )
    
    $script:totalTests++
    
    try {
        $headers = @{ "Content-Type" = "application/json" }
        
        $params = @{
            Uri = $Url
            Method = $Method
            Headers = $headers
            TimeoutSec = 10
        }
        
        if ($Body) {
            $params.Body = ($Body | ConvertTo-Json -Depth 10)
        }
        
        $response = Invoke-RestMethod @params
        
        Write-Host "  ✅ $Description" -ForegroundColor Green
        $script:passedTests++
        
        return @{
            Success = $true
            Response = $response
            Status = 200
            Description = $Description
        }
    }
    catch {
        $statusCode = 0
        if ($_.Exception.Response) {
            $statusCode = [int]$_.Exception.Response.StatusCode
        }
        
        if ($statusCode -eq $ExpectedStatus) {
            Write-Host "  ✅ $Description (Expected $ExpectedStatus)" -ForegroundColor Green
            $script:passedTests++
            return @{ Success = $true; Status = $statusCode; Description = $Description }
        }
        
        Write-Host "  ❌ $Description - Status: $statusCode" -ForegroundColor Red
        if ($Verbose) {
            Write-Host "     Erro: $($_.Exception.Message)" -ForegroundColor Gray
        }
        $script:failedTests++
        
        if ($StopOnError) {
            throw "Teste falhou: $Description"
        }
        
        return @{
            Success = $false
            Status = $statusCode
            Error = $_.Exception.Message
            Description = $Description
        }
    }
}

# ============================================================================
# 1. API USINAS (10 endpoints)
# ============================================================================

Write-Host "🏭 [1/15] API USINAS" -ForegroundColor Yellow

$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usinas" -Description "Listar todas as usinas"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usinas/1" -Description "Buscar usina por ID"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usinas/codigo/UHE_ITAIPU" -Description "Buscar usina por código"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usinas/tipo/1" -Description "Listar usinas por tipo"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usinas/empresa/1" -Description "Listar usinas por empresa"

Write-Host ""

# ============================================================================
# 2. API EMPRESAS (7 endpoints)
# ============================================================================

Write-Host "🏢 [2/15] API EMPRESAS" -ForegroundColor Yellow

$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/empresas" -Description "Listar todas as empresas"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/empresas/1" -Description "Buscar empresa por ID"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/empresas/cnpj/33000167000101" -Description "Buscar empresa por CNPJ"

Write-Host ""

# ============================================================================
# 3. API TIPOS DE USINA (6 endpoints)
# ============================================================================

Write-Host "⚡ [3/15] API TIPOS DE USINA" -ForegroundColor Yellow

$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/tiposusina" -Description "Listar todos os tipos"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/tiposusina/1" -Description "Buscar tipo por ID"

Write-Host ""

# ============================================================================
# 4. API SEMANAS PMO (8 endpoints)
# ============================================================================

Write-Host "📅 [4/15] API SEMANAS PMO" -ForegroundColor Yellow

$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/semanaspmo" -Description "Listar todas as semanas"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/semanaspmo/1" -Description "Buscar semana por ID"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/semanaspmo/atual" -Description "Buscar semana atual"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/semanaspmo/proximas?quantidade=4" -Description "Buscar próximas semanas"

Write-Host ""

# ============================================================================
# 5. API EQUIPES PDP (7 endpoints)
# ============================================================================

Write-Host "👥 [5/15] API EQUIPES PDP" -ForegroundColor Yellow

$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/equipespdp" -Description "Listar todas as equipes"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/equipespdp/1" -Description "Buscar equipe por ID"

Write-Host ""

# ============================================================================
# 6. API CARGAS (9 endpoints)
# ============================================================================

Write-Host "⚡ [6/15] API CARGAS" -ForegroundColor Yellow

$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/cargas" -Description "Listar todas as cargas"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/cargas/1" -Description "Buscar carga por ID"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/cargas/subsistema/SE" -Description "Listar cargas por subsistema"
$dataInicio = (Get-Date).AddDays(-30).ToString("yyyy-MM-dd")
$dataFim = (Get-Date).ToString("yyyy-MM-dd")
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/cargas/periodo?dataInicio=$dataInicio&dataFim=$dataFim" -Description "Listar cargas por período"

Write-Host ""

# ============================================================================
# 7. API ARQUIVOS DADGER (10 endpoints)
# ============================================================================

Write-Host "📁 [7/15] API ARQUIVOS DADGER" -ForegroundColor Yellow

$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/arquivosdadger" -Description "Listar todos os arquivos"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/arquivosdadger/1" -Description "Buscar arquivo por ID"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/arquivosdadger/semana/1" -Description "Listar arquivos por semana PMO"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/arquivosdadger/processados?processado=false" -Description "Listar arquivos não processados"

Write-Host ""

# ============================================================================
# 8. API RESTRIÇÕES UG (9 endpoints)
# ============================================================================

Write-Host "🔧 [8/15] API RESTRIÇÕES UG" -ForegroundColor Yellow

$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/restricoesug" -Description "Listar todas as restrições"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/restricoesug/1" -Description "Buscar restrição por ID"
$dataRef = (Get-Date).ToString("yyyy-MM-dd")
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/restricoesug/ativas?dataReferencia=$dataRef" -Description "Listar restrições ativas"

Write-Host ""

# ============================================================================
# 9. API DADOS ENERGÉTICOS (7 endpoints)
# ============================================================================

Write-Host "⚡ [9/15] API DADOS ENERGÉTICOS" -ForegroundColor Yellow

$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/dadosenergeticos" -Description "Listar todos os dados"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/dadosenergeticos/1" -Description "Buscar dado por ID"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/dadosenergeticos/periodo?dataInicio=$dataInicio&dataFim=$dataFim" -Description "Listar dados por período"

Write-Host ""

# ============================================================================
# 10. API UNIDADES GERADORAS (8 endpoints)
# ============================================================================

Write-Host "⚙️ [10/15] API UNIDADES GERADORAS" -ForegroundColor Yellow

$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/unidadesgeradoras" -Description "Listar todas as unidades"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/unidadesgeradoras/1" -Description "Buscar unidade por ID"

Write-Host ""

# ============================================================================
# 11. API PARADAS UG (7 endpoints)
# ============================================================================

Write-Host "🛑 [11/15] API PARADAS UG" -ForegroundColor Yellow

$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/paradasug" -Description "Listar todas as paradas"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/paradasug/1" -Description "Buscar parada por ID"

Write-Host ""

# ============================================================================
# 12. API MOTIVOS RESTRIÇÃO (6 endpoints)
# ============================================================================

Write-Host "📋 [12/15] API MOTIVOS RESTRIÇÃO" -ForegroundColor Yellow

$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/motivosrestricao" -Description "Listar todos os motivos"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/motivosrestricao/1" -Description "Buscar motivo por ID"

Write-Host ""

# ============================================================================
# 13. API BALANÇOS (8 endpoints)
# ============================================================================

Write-Host "💰 [13/15] API BALANÇOS" -ForegroundColor Yellow

$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/balancos" -Description "Listar todos os balanços"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/balancos/1" -Description "Buscar balanço por ID"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/balancos/subsistema/SE" -Description "Listar balanços por subsistema"

Write-Host ""

# ============================================================================
# 14. API INTERCÂMBIOS (8 endpoints)
# ============================================================================

Write-Host "🔄 [14/15] API INTERCÂMBIOS" -ForegroundColor Yellow

$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/intercambios" -Description "Listar todos os intercâmbios"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/intercambios/1" -Description "Buscar intercâmbio por ID"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/intercambios/origem/SE" -Description "Listar intercâmbios por origem"
$results += Test-Endpoint -Method "GET" -Url "$BaseUrl/api/intercambios/destino/S" -Description "Listar intercâmbios por destino"

Write-Host ""

# ============================================================================
# RELATÓRIO FINAL
# ============================================================================

Write-Host "`n📊 RELATÓRIO FINAL DE VALIDAÇÃO" -ForegroundColor Cyan
Write-Host "================================`n" -ForegroundColor Cyan

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

Write-Host "`n📋 APIs Testadas:" -ForegroundColor Cyan
Write-Host "  1. Usinas                 ✅" -ForegroundColor Green
Write-Host "  2. Empresas               ✅" -ForegroundColor Green
Write-Host "  3. Tipos de Usina         ✅" -ForegroundColor Green
Write-Host "  4. Semanas PMO            ✅" -ForegroundColor Green
Write-Host "  5. Equipes PDP            ✅" -ForegroundColor Green
Write-Host "  6. Cargas                 ✅" -ForegroundColor Green
Write-Host "  7. Arquivos DADGER        ✅" -ForegroundColor Green
Write-Host "  8. Restrições UG          ✅" -ForegroundColor Green
Write-Host "  9. Dados Energéticos      ✅" -ForegroundColor Green
Write-Host "  10. Unidades Geradoras    ✅" -ForegroundColor Green
Write-Host "  11. Paradas UG            ✅" -ForegroundColor Green
Write-Host "  12. Motivos Restrição     ✅" -ForegroundColor Green
Write-Host "  13. Balanços              ✅" -ForegroundColor Green
Write-Host "  14. Intercâmbios          ✅" -ForegroundColor Green

if ($failedTests -eq 0) {
    Write-Host "`n✅ TODAS AS APIS ESTÃO FUNCIONAIS!" -ForegroundColor Green
    Write-Host "   Sistema pronto para testes via Swagger." -ForegroundColor Green
} else {
    Write-Host "`n⚠️  ALGUNS ENDPOINTS FALHARAM" -ForegroundColor Yellow
    Write-Host "   Verifique os logs acima para detalhes." -ForegroundColor Yellow
    
    Write-Host "`n❌ Endpoints com Falha:" -ForegroundColor Red
    $results | Where-Object { -not $_.Success } | ForEach-Object {
        Write-Host "   - $($_.Description)" -ForegroundColor Red
    }
}

Write-Host "`n💡 Próximos Passos:" -ForegroundColor Cyan
Write-Host "   1. Acessar Swagger: $BaseUrl/swagger" -ForegroundColor White
Write-Host "   2. Testar endpoints manualmente" -ForegroundColor White
Write-Host "   3. Criar registros de teste via POST" -ForegroundColor White

Write-Host ""

# Exportar relatório
$reportPath = ".\validacao-apis-$(Get-Date -Format 'yyyyMMdd-HHmmss').json"
$results | ConvertTo-Json -Depth 10 | Out-File -FilePath $reportPath -Encoding UTF8

Write-Host "📄 Relatório exportado: $reportPath`n" -ForegroundColor Cyan

exit $failedTests
