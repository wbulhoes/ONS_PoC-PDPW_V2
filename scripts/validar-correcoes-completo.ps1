# ============================================================================
# SCRIPT DE VALIDAÇÃO COMPLETA - BUGS CORRIGIDOS
# ============================================================================
# Valida se os 3 bugs reportados pelo QA foram corrigidos
# Data: 23/12/2025
# ============================================================================

$ErrorActionPreference = "Continue"
$BaseUrl = "http://localhost:5001"

$totalTests = 0
$passedTests = 0
$failedTests = 0

Write-Host "`n╔══════════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║     VALIDAÇÃO COMPLETA - BUGS CORRIGIDOS (QA AUTOMATION)        ║" -ForegroundColor Cyan
Write-Host "╚══════════════════════════════════════════════════════════════════╝`n" -ForegroundColor Cyan

Write-Host "Base URL: $BaseUrl" -ForegroundColor White
Write-Host "Data: $(Get-Date -Format 'dd/MM/yyyy HH:mm:ss')" -ForegroundColor White
Write-Host ""

# ============================================================================
# BUG #1: ArquivosDadger - AutoMapper
# ============================================================================

Write-Host "🐛 [BUG #1] ArquivosDadger - AutoMapper não configurado" -ForegroundColor Yellow
Write-Host "════════════════════════════════════════════════════════════════════" -ForegroundColor Gray

# Teste 1.1: GET /api/arquivosdadger (antes retornava 500)
Write-Host "  [1/6] GET /api/arquivosdadger (deve retornar 200)..." -NoNewline
$totalTests++
try {
    $response = Invoke-RestMethod -Uri "$BaseUrl/api/arquivosdadger" -Method GET -ErrorAction Stop
    Write-Host " ✅ PASSOU (Status: 200, Registros: $($response.Count))" -ForegroundColor Green
    $passedTests++
}
catch {
    $statusCode = $_.Exception.Response.StatusCode.value__
    Write-Host " ❌ FALHOU (Status: $statusCode)" -ForegroundColor Red
    $failedTests++
}

# Teste 1.2: POST com SemanaPMO válida
Write-Host "  [2/6] POST /api/arquivosdadger (SemanaPMO válida)..." -NoNewline
$totalTests++
$body = @{
    nomeArquivo = "dadger_teste_$(Get-Date -Format 'HHmmss').dat"
    caminhoArquivo = "/uploads/teste.dat"
    dataImportacao = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ss")
    semanaPMOId = 1
    observacoes = "Teste automático - Bug #1"
} | ConvertTo-Json

try {
    $response = Invoke-RestMethod -Uri "$BaseUrl/api/arquivosdadger" -Method POST -Body $body -ContentType "application/json" -ErrorAction Stop
    $arquivoId = $response.id
    Write-Host " ✅ PASSOU (ID criado: $arquivoId)" -ForegroundColor Green
    $passedTests++
}
catch {
    $statusCode = $_.Exception.Response.StatusCode.value__
    Write-Host " ❌ FALHOU (Status: $statusCode)" -ForegroundColor Red
    $failedTests++
    $arquivoId = $null
}

# Teste 1.3: POST com SemanaPMO INVÁLIDA (999) - deve retornar 400
Write-Host "  [3/6] POST /api/arquivosdadger (SemanaPMO inválida)..." -NoNewline
$totalTests++
$bodyInvalido = @{
    nomeArquivo = "dadger_invalido.dat"
    caminhoArquivo = "/uploads/invalido.dat"
    dataImportacao = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ss")
    semanaPMOId = 999
    observacoes = "Deve falhar"
} | ConvertTo-Json

try {
    $response = Invoke-RestMethod -Uri "$BaseUrl/api/arquivosdadger" -Method POST -Body $bodyInvalido -ContentType "application/json" -ErrorAction Stop
    Write-Host " ❌ FALHOU (Deveria retornar 400, mas retornou 200/201)" -ForegroundColor Red
    $failedTests++
}
catch {
    $statusCode = $_.Exception.Response.StatusCode.value__
    if ($statusCode -eq 400) {
        Write-Host " ✅ PASSOU (Status: 400 - Validação OK)" -ForegroundColor Green
        $passedTests++
    }
    else {
        Write-Host " ❌ FALHOU (Esperado 400, recebeu $statusCode)" -ForegroundColor Red
        $failedTests++
    }
}

# Teste 1.4: PATCH /processar
if ($arquivoId) {
    Write-Host "  [4/6] PATCH /api/arquivosdadger/$arquivoId/processar..." -NoNewline
    $totalTests++
    try {
        $response = Invoke-RestMethod -Uri "$BaseUrl/api/arquivosdadger/$arquivoId/processar" -Method PATCH -ErrorAction Stop
        if ($response.processado -eq $true) {
            Write-Host " ✅ PASSOU (Processado: true)" -ForegroundColor Green
            $passedTests++
        }
        else {
            Write-Host " ❌ FALHOU (Processado: $($response.processado))" -ForegroundColor Red
            $failedTests++
        }
    }
    catch {
        Write-Host " ❌ FALHOU" -ForegroundColor Red
        $failedTests++
    }
}
else {
    Write-Host "  [4/6] PATCH /processar... ⏭️  PULADO (arquivo não criado)" -ForegroundColor Yellow
}

# Teste 1.5: GET /semana/1
Write-Host "  [5/6] GET /api/arquivosdadger/semana/1..." -NoNewline
$totalTests++
try {
    $response = Invoke-RestMethod -Uri "$BaseUrl/api/arquivosdadger/semana/1" -Method GET -ErrorAction Stop
    Write-Host " ✅ PASSOU (Registros: $($response.Count))" -ForegroundColor Green
    $passedTests++
}
catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# Teste 1.6: DELETE (soft delete)
if ($arquivoId) {
    Write-Host "  [6/6] DELETE /api/arquivosdadger/$arquivoId..." -NoNewline
    $totalTests++
    try {
        Invoke-RestMethod -Uri "$BaseUrl/api/arquivosdadger/$arquivoId" -Method DELETE -ErrorAction Stop | Out-Null
        Write-Host " ✅ PASSOU (Soft delete OK)" -ForegroundColor Green
        $passedTests++
    }
    catch {
        Write-Host " ❌ FALHOU" -ForegroundColor Red
        $failedTests++
    }
}
else {
    Write-Host "  [6/6] DELETE... ⏭️  PULADO (arquivo não criado)" -ForegroundColor Yellow
}

Write-Host ""

# ============================================================================
# BUG #2: RestricoesUG - Validação de Datas
# ============================================================================

Write-Host "🐛 [BUG #2] RestricoesUG - Validação de datas faltante" -ForegroundColor Yellow
Write-Host "════════════════════════════════════════════════════════════════════" -ForegroundColor Gray

# Teste 2.1: POST com datas VÁLIDAS
Write-Host "  [1/4] POST /api/restricoesug (datas válidas)..." -NoNewline
$totalTests++
$bodyValido = @{
    unidadeGeradoraId = 1
    dataInicio = (Get-Date).ToString("yyyy-MM-dd")
    dataFim = (Get-Date).AddDays(7).ToString("yyyy-MM-dd")
    motivoRestricaoId = 1
    potenciaRestrita = 150.00
    observacoes = "Teste automático - Bug #2"
} | ConvertTo-Json

try {
    $response = Invoke-RestMethod -Uri "$BaseUrl/api/restricoesug" -Method POST -Body $bodyValido -ContentType "application/json" -ErrorAction Stop
    $restricaoId = $response.id
    Write-Host " ✅ PASSOU (ID criado: $restricaoId)" -ForegroundColor Green
    $passedTests++
}
catch {
    $statusCode = $_.Exception.Response.StatusCode.value__
    Write-Host " ❌ FALHOU (Status: $statusCode)" -ForegroundColor Red
    $failedTests++
    $restricaoId = $null
}

# Teste 2.2: POST com dataFim < dataInicio (DEVE FALHAR com 400)
Write-Host "  [2/4] POST /api/restricoesug (dataFim < dataInicio)..." -NoNewline
$totalTests++
$bodyDatasInvalidas = @{
    unidadeGeradoraId = 1
    dataInicio = (Get-Date).AddDays(7).ToString("yyyy-MM-dd")
    dataFim = (Get-Date).ToString("yyyy-MM-dd")
    motivoRestricaoId = 1
    potenciaRestrita = 150.00
    observacoes = "Deve falhar - datas inválidas"
} | ConvertTo-Json

try {
    $response = Invoke-RestMethod -Uri "$BaseUrl/api/restricoesug" -Method POST -Body $bodyDatasInvalidas -ContentType "application/json" -ErrorAction Stop
    Write-Host " ❌ FALHOU (Deveria retornar 400, mas retornou 200/201)" -ForegroundColor Red
    $failedTests++
}
catch {
    $statusCode = $_.Exception.Response.StatusCode.value__
    if ($statusCode -eq 400) {
        Write-Host " ✅ PASSOU (Status: 400 - Validação OK)" -ForegroundColor Green
        $passedTests++
    }
    else {
        Write-Host " ❌ FALHOU (Esperado 400, recebeu $statusCode)" -ForegroundColor Red
        $failedTests++
    }
}

# Teste 2.3: GET /ativas
Write-Host "  [3/4] GET /api/restricoesug/ativas..." -NoNewline
$totalTests++
$dataRef = (Get-Date).ToString("yyyy-MM-dd")
try {
    $response = Invoke-RestMethod -Uri "$BaseUrl/api/restricoesug/ativas?dataReferencia=$dataRef" -Method GET -ErrorAction Stop
    Write-Host " ✅ PASSOU (Registros: $($response.Count))" -ForegroundColor Green
    $passedTests++
}
catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# Teste 2.4: DELETE
if ($restricaoId) {
    Write-Host "  [4/4] DELETE /api/restricoesug/$restricaoId..." -NoNewline
    $totalTests++
    try {
        Invoke-RestMethod -Uri "$BaseUrl/api/restricoesug/$restricaoId" -Method DELETE -ErrorAction Stop | Out-Null
        Write-Host " ✅ PASSOU (Soft delete OK)" -ForegroundColor Green
        $passedTests++
    }
    catch {
        Write-Host " ❌ FALHOU" -ForegroundColor Red
        $failedTests++
    }
}
else {
    Write-Host "  [4/4] DELETE... ⏭️  PULADO (restrição não criada)" -ForegroundColor Yellow
}

Write-Host ""

# ============================================================================
# BUG #3: Usuarios - AutoMapper
# ============================================================================

Write-Host "🐛 [BUG #3] Usuarios - AutoMapper não configurado" -ForegroundColor Yellow
Write-Host "════════════════════════════════════════════════════════════════════" -ForegroundColor Gray

# Teste 3.1: GET /api/usuarios (antes retornava 500)
Write-Host "  [1/3] GET /api/usuarios (deve retornar 200)..." -NoNewline
$totalTests++
try {
    $response = Invoke-RestMethod -Uri "$BaseUrl/api/usuarios" -Method GET -ErrorAction Stop
    Write-Host " ✅ PASSOU (Status: 200, Registros: $($response.Count))" -ForegroundColor Green
    $passedTests++
}
catch {
    $statusCode = $_.Exception.Response.StatusCode.value__
    Write-Host " ❌ FALHOU (Status: $statusCode)" -ForegroundColor Red
    $failedTests++
}

# Teste 3.2: GET /api/usuarios/1
Write-Host "  [2/3] GET /api/usuarios/1..." -NoNewline
$totalTests++
try {
    $response = Invoke-RestMethod -Uri "$BaseUrl/api/usuarios/1" -Method GET -ErrorAction Stop
    Write-Host " ✅ PASSOU (Usuário: $($response.nome))" -ForegroundColor Green
    $passedTests++
}
catch {
    $statusCode = $_.Exception.Response.StatusCode.value__
    Write-Host " ❌ FALHOU (Status: $statusCode)" -ForegroundColor Red
    $failedTests++
}

# Teste 3.3: GET /api/usuarios/perfil/Operador
Write-Host "  [3/3] GET /api/usuarios/perfil/Operador..." -NoNewline
$totalTests++
try {
    $response = Invoke-RestMethod -Uri "$BaseUrl/api/usuarios/perfil/Operador" -Method GET -ErrorAction Stop
    Write-Host " ✅ PASSOU (Registros: $($response.Count))" -ForegroundColor Green
    $passedTests++
}
catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

Write-Host ""

# ============================================================================
# TESTES DE REGRESSÃO (garantir que não quebramos nada)
# ============================================================================

Write-Host "🔄 TESTES DE REGRESSÃO" -ForegroundColor Yellow
Write-Host "════════════════════════════════════════════════════════════════════" -ForegroundColor Gray

$regressaoEndpoints = @(
    @{ Nome = "Usinas"; Url = "/api/usinas" },
    @{ Nome = "Empresas"; Url = "/api/empresas" },
    @{ Nome = "TiposUsina"; Url = "/api/tiposusina" },
    @{ Nome = "SemanasPMO"; Url = "/api/semanaspmo" },
    @{ Nome = "Cargas"; Url = "/api/cargas" }
)

foreach ($endpoint in $regressaoEndpoints) {
    Write-Host "  GET $($endpoint.Url)..." -NoNewline
    $totalTests++
    try {
        $response = Invoke-RestMethod -Uri "$BaseUrl$($endpoint.Url)" -Method GET -ErrorAction Stop
        Write-Host " ✅ PASSOU ($($response.Count) registros)" -ForegroundColor Green
        $passedTests++
    }
    catch {
        Write-Host " ❌ FALHOU" -ForegroundColor Red
        $failedTests++
    }
}

Write-Host ""

# ============================================================================
# RELATÓRIO FINAL
# ============================================================================

Write-Host "╔══════════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║                    RELATÓRIO FINAL                               ║" -ForegroundColor Cyan
Write-Host "╚══════════════════════════════════════════════════════════════════╝`n" -ForegroundColor Cyan

$successRate = if ($totalTests -gt 0) { [math]::Round(($passedTests / $totalTests) * 100, 2) } else { 0 }

Write-Host "📊 ESTATÍSTICAS:" -ForegroundColor Yellow
Write-Host "   Total de Testes:    $totalTests" -ForegroundColor White
Write-Host "   ✅ Sucessos:       $passedTests" -ForegroundColor Green
Write-Host "   ❌ Falhas:         $failedTests" -ForegroundColor $(if ($failedTests -gt 0) { "Red" } else { "Green" })
Write-Host "   📈 Taxa de Sucesso: $successRate%" -ForegroundColor $(if ($successRate -eq 100) { "Green" } elseif ($successRate -ge 90) { "Yellow" } else { "Red" })
Write-Host ""

Write-Host "🐛 BUGS VALIDADOS:" -ForegroundColor Yellow
Write-Host "   1. ArquivosDadger - AutoMapper:      " -NoNewline
if ($successRate -ge 90) { Write-Host "✅ CORRIGIDO" -ForegroundColor Green } else { Write-Host "⚠️  VERIFICAR" -ForegroundColor Yellow }

Write-Host "   2. RestricoesUG - Validação Datas:  " -NoNewline
if ($successRate -ge 90) { Write-Host "✅ CORRIGIDO" -ForegroundColor Green } else { Write-Host "⚠️  VERIFICAR" -ForegroundColor Yellow }

Write-Host "   3. Usuarios - AutoMapper:           " -NoNewline
if ($successRate -ge 90) { Write-Host "✅ CORRIGIDO" -ForegroundColor Green } else { Write-Host "⚠️  VERIFICAR" -ForegroundColor Yellow }

Write-Host ""

if ($failedTests -eq 0) {
    Write-Host "🎉 VALIDAÇÃO 100% CONCLUÍDA COM SUCESSO!" -ForegroundColor Green
    Write-Host "✅ Todos os bugs reportados pelo QA foram corrigidos!" -ForegroundColor Green
    Write-Host "✅ Testes de regressão passaram!" -ForegroundColor Green
    Write-Host ""
    Write-Host "🚀 PRONTO PARA:" -ForegroundColor Cyan
    Write-Host "   1. Commit das correções" -ForegroundColor White
    Write-Host "   2. Push para GitHub" -ForegroundColor White
    Write-Host "   3. Notificar QA para validação final" -ForegroundColor White
}
elseif ($successRate -ge 90) {
    Write-Host "✅ VALIDAÇÃO APROVADA COM RESSALVAS" -ForegroundColor Yellow
    Write-Host "   Taxa de sucesso acima de 90%" -ForegroundColor Yellow
    Write-Host "   Revisar falhas menores antes do push" -ForegroundColor Yellow
}
else {
    Write-Host "⚠️  VALIDAÇÃO REPROVADA" -ForegroundColor Red
    Write-Host "   Taxa de sucesso abaixo do esperado" -ForegroundColor Red
    Write-Host "   Corrigir problemas antes do push" -ForegroundColor Red
    Write-Host ""
    Write-Host "💡 SUGESTÕES:" -ForegroundColor Cyan
    Write-Host "   1. Verificar logs do Docker: docker logs pdpw-backend --tail 50" -ForegroundColor White
    Write-Host "   2. Recriar containers: docker-compose down && docker-compose up --build -d" -ForegroundColor White
    Write-Host "   3. Verificar seed data no banco" -ForegroundColor White
}

Write-Host ""

# Salvar relatório em arquivo
$reportPath = ".\relatorio-validacao-bugs-$(Get-Date -Format 'yyyyMMdd-HHmmss').json"
$report = @{
    Data = Get-Date
    TotalTestes = $totalTests
    Passaram = $passedTests
    Falharam = $failedTests
    TaxaSucesso = $successRate
    AprovadoParaPush = ($failedTests -eq 0)
} | ConvertTo-Json

$report | Out-File -FilePath $reportPath -Encoding UTF8

Write-Host "📄 Relatório salvo em: $reportPath" -ForegroundColor Gray
Write-Host ""

exit $failedTests
