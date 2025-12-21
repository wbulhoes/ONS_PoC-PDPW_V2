# ============================================
# TESTES AUTOMATIZADOS - TODAS AS APIs
# ============================================
# Testa todos os endpoints com dados fictícios
# Gera relatório completo
# Data: 20/12/2024
# ============================================

param(
    [string]$BaseUrl = "http://localhost:5001/api",
    [string]$ReportPath = ".\docs\relatorio-testes-automatizados.md"
)

$ErrorActionPreference = "Continue"
$startTime = Get-Date

# ============================================
# FUNÇÕES AUXILIARES
# ============================================

function Write-TestStep {
    param([string]$Message)
    Write-Host ""
    Write-Host "? $Message" -ForegroundColor Cyan
    Write-Host ("?" * 80) -ForegroundColor DarkGray
}

function Write-TestSuccess {
    param([string]$Message)
    Write-Host "  ? $Message" -ForegroundColor Green
}

function Write-TestFail {
    param([string]$Message)
    Write-Host "  ? $Message" -ForegroundColor Red
}

function Write-TestInfo {
    param([string]$Message)
    Write-Host "  ? $Message" -ForegroundColor Yellow
}

function Invoke-ApiTest {
    param(
        [string]$Method,
        [string]$Endpoint,
        [object]$Body = $null,
        [string]$Description
    )
    
    $result = @{
        Method = $Method
        Endpoint = $Endpoint
        Description = $Description
        Success = $false
        StatusCode = 0
        Response = $null
        Error = $null
        Duration = 0
    }
    
    try {
        $uri = "$BaseUrl$Endpoint"
        $timer = [System.Diagnostics.Stopwatch]::StartNew()
        
        if ($Method -eq "GET") {
            $response = Invoke-RestMethod -Uri $uri -Method Get -ContentType "application/json" -ErrorAction Stop
        }
        elseif ($Method -eq "POST") {
            $jsonBody = $Body | ConvertTo-Json -Depth 10
            $response = Invoke-RestMethod -Uri $uri -Method Post -Body $jsonBody -ContentType "application/json" -ErrorAction Stop
        }
        elseif ($Method -eq "PUT") {
            $jsonBody = $Body | ConvertTo-Json -Depth 10
            $response = Invoke-RestMethod -Uri $uri -Method Put -Body $jsonBody -ContentType "application/json" -ErrorAction Stop
        }
        elseif ($Method -eq "DELETE") {
            $response = Invoke-RestMethod -Uri $uri -Method Delete -ContentType "application/json" -ErrorAction Stop
        }
        
        $timer.Stop()
        
        $result.Success = $true
        $result.StatusCode = 200
        $result.Response = $response
        $result.Duration = $timer.ElapsedMilliseconds
        
        Write-TestSuccess "$Description - $($timer.ElapsedMilliseconds)ms"
    }
    catch {
        $timer.Stop()
        $result.Success = $false
        $result.StatusCode = $_.Exception.Response.StatusCode.value__
        $result.Error = $_.Exception.Message
        $result.Duration = $timer.ElapsedMilliseconds
        
        Write-TestFail "$Description - Erro: $($result.Error)"
    }
    
    return $result
}

# ============================================
# INICIALIZAÇÃO
# ============================================

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  TESTES AUTOMATIZADOS - PDPw APIs" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Base URL: $BaseUrl" -ForegroundColor Yellow
Write-Host "Início: $(Get-Date -Format 'dd/MM/yyyy HH:mm:ss')" -ForegroundColor Yellow
Write-Host ""

$testResults = @()

# ============================================
# TESTES - API EMPRESAS
# ============================================

Write-TestStep "1. TESTANDO API EMPRESAS"

# GET - Listar todas
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/empresas" -Description "GET /empresas - Listar todas"

# GET - Buscar por ID
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/empresas/200" -Description "GET /empresas/200 - Buscar fictícia"

# POST - Criar nova
$novaEmpresa = @{
    nome = "Empresa Teste Automated"
    cnpj = "99888777000166"
    telefone = "(99) 99999-9999"
    email = "automated@teste.com"
}
$testResults += Invoke-ApiTest -Method "POST" -Endpoint "/empresas" -Body $novaEmpresa -Description "POST /empresas - Criar nova"

# PUT - Atualizar
$empresaAtualizada = @{
    nome = "Empresa Teste Automated - ATUALIZADA"
    cnpj = "99888777000166"
    telefone = "(99) 88888-8888"
    email = "updated@teste.com"
    ativo = $true
}
$testResults += Invoke-ApiTest -Method "PUT" -Endpoint "/empresas/200" -Body $empresaAtualizada -Description "PUT /empresas/200 - Atualizar"

# ============================================
# TESTES - API USINAS
# ============================================

Write-TestStep "2. TESTANDO API USINAS"

# GET - Listar todas
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/usinas" -Description "GET /usinas - Listar todas"

# GET - Buscar por ID
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/usinas/300" -Description "GET /usinas/300 - Buscar fictícia"

# GET - Buscar por código
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/usinas/codigo/TEST-H01" -Description "GET /usinas/codigo/TEST-H01"

# GET - Filtrar por empresa
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/usinas/empresa/200" -Description "GET /usinas/empresa/200"

# GET - Filtrar por tipo
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/usinas/tipo/1" -Description "GET /usinas/tipo/1 - Hidrelétricas"

# POST - Criar nova
$novaUsina = @{
    codigo = "TEST-AUTO"
    nome = "Usina Teste Automated"
    tipoUsinaId = 1
    empresaId = 200
    capacidadeInstalada = 100.00
    localizacao = "Teste City, TS"
    dataOperacao = "2025-12-21"
}
$testResults += Invoke-ApiTest -Method "POST" -Endpoint "/usinas" -Body $novaUsina -Description "POST /usinas - Criar nova"

# ============================================
# TESTES - API TIPOS USINA
# ============================================

Write-TestStep "3. TESTANDO API TIPOS USINA"

# GET - Listar todos
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/tiposusina" -Description "GET /tiposusina - Listar todos"

# GET - Buscar por ID
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/tiposusina/1" -Description "GET /tiposusina/1 - Hidrelétrica"

# ============================================
# TESTES - API SEMANAS PMO
# ============================================

Write-TestStep "4. TESTANDO API SEMANAS PMO"

# GET - Listar todas
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/semanaspmo" -Description "GET /semanaspmo - Listar todas"

# GET - Semana atual
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/semanaspmo/atual" -Description "GET /semanaspmo/atual"

# GET - Próximas semanas
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/semanaspmo/proximas?quantidade=4" -Description "GET /semanaspmo/proximas?quantidade=4"

# GET - Por ano
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/semanaspmo/ano/2026" -Description "GET /semanaspmo/ano/2026"

# GET - Buscar por ID
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/semanaspmo/100" -Description "GET /semanaspmo/100 - Fictícia"

# POST - Criar nova
$novaSemana = @{
    numero = 5
    dataInicio = "2026-01-31"
    dataFim = "2026-02-06"
    ano = 2026
    observacoes = "Semana Teste Automated 5/2026"
}
$testResults += Invoke-ApiTest -Method "POST" -Endpoint "/semanaspmo" -Body $novaSemana -Description "POST /semanaspmo - Criar nova"

# ============================================
# TESTES - API EQUIPES PDP
# ============================================

Write-TestStep "5. TESTANDO API EQUIPES PDP"

# GET - Listar todas
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/equipespdp" -Description "GET /equipespdp - Listar todas"

# GET - Buscar por ID
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/equipespdp/100" -Description "GET /equipespdp/100 - Fictícia"

# POST - Criar nova
$novaEquipe = @{
    nome = "Equipe Teste Automated"
    descricao = "Equipe criada por testes automatizados"
    coordenador = "Coordenador Auto"
    email = "auto@teste.com"
    telefone = "(99) 99999-9999"
}
$testResults += Invoke-ApiTest -Method "POST" -Endpoint "/equipespdp" -Body $novaEquipe -Description "POST /equipespdp - Criar nova"

# ============================================
# TESTES - API CARGAS
# ============================================

Write-TestStep "6. TESTANDO API CARGAS"

# GET - Listar todas
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/cargas" -Description "GET /cargas - Listar todas"

# GET - Buscar por ID
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/cargas/100" -Description "GET /cargas/100 - Fictícia"

# GET - Por subsistema
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/cargas/subsistema/SE" -Description "GET /cargas/subsistema/SE"

# GET - Por data
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/cargas/data/2025-12-21" -Description "GET /cargas/data/2025-12-21"

# POST - Criar nova
$novaCarga = @{
    dataReferencia = "2025-12-24"
    subsistemaId = "SE"
    cargaMWmed = 44000.00
    cargaVerificada = 43800.00
    previsaoCarga = 44200.00
    observacoes = "Carga Teste Automated"
}
$testResults += Invoke-ApiTest -Method "POST" -Endpoint "/cargas" -Body $novaCarga -Description "POST /cargas - Criar nova"

# ============================================
# TESTES - API ARQUIVOS DADGER
# ============================================

Write-TestStep "7. TESTANDO API ARQUIVOS DADGER"

# GET - Listar todos
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/arquivosdadger" -Description "GET /arquivosdadger - Listar todos"

# GET - Buscar por ID
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/arquivosdadger/100" -Description "GET /arquivosdadger/100 - Fictício"

# GET - Por semana PMO
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/arquivosdadger/semana/100" -Description "GET /arquivosdadger/semana/100"

# GET - Processados
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/arquivosdadger/processados?processado=true" -Description "GET /arquivosdadger/processados?processado=true"

# POST - Criar novo
$novoArquivo = @{
    nomeArquivo = "dadger_teste_auto.dat"
    caminhoArquivo = "/uploads/test/dadger_teste_auto.dat"
    dataImportacao = "2025-12-21T10:00:00.000Z"
    semanaPMOId = 100
    observacoes = "Arquivo Teste Automated"
}
$testResults += Invoke-ApiTest -Method "POST" -Endpoint "/arquivosdadger" -Body $novoArquivo -Description "POST /arquivosdadger - Criar novo"

# ============================================
# TESTES - API RESTRIÇÕES UG
# ============================================

Write-TestStep "8. TESTANDO API RESTRIÇÕES UG"

# GET - Listar todas
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/restricoesug" -Description "GET /restricoesug - Listar todas"

# GET - Buscar por ID
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/restricoesug/100" -Description "GET /restricoesug/100 - Fictícia"

# GET - Por unidade geradora
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/restricoesug/unidade/100" -Description "GET /restricoesug/unidade/100"

# GET - Por motivo
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/restricoesug/motivo/1" -Description "GET /restricoesug/motivo/1"

# GET - Ativas em data
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/restricoesug/ativas?dataReferencia=2025-12-22" -Description "GET /restricoesug/ativas?dataReferencia=2025-12-22"

# POST - Criar nova
$novaRestricao = @{
    unidadeGeradoraId = 100
    dataInicio = "2025-12-25"
    dataFim = "2025-12-28"
    motivoRestricaoId = 1
    potenciaRestrita = 150.00
    observacoes = "Restrição Teste Automated"
}
$testResults += Invoke-ApiTest -Method "POST" -Endpoint "/restricoesug" -Body $novaRestricao -Description "POST /restricoesug - Criar nova"

# ============================================
# ESTATÍSTICAS E RELATÓRIO
# ============================================

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  GERANDO RELATÓRIO" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

$totalTests = $testResults.Count
$successTests = ($testResults | Where-Object { $_.Success -eq $true }).Count
$failedTests = ($testResults | Where-Object { $_.Success -eq $false }).Count
$successRate = [math]::Round(($successTests / $totalTests) * 100, 2)
$avgDuration = [math]::Round(($testResults | Measure-Object -Property Duration -Average).Average, 2)
$endTime = Get-Date
$totalDuration = ($endTime - $startTime).TotalSeconds

# Agrupar por API
$testsByApi = $testResults | Group-Object { ($_.Endpoint -split '/')[1] }

# Gerar relatório Markdown
$report = @"
# ?? RELATÓRIO DE TESTES AUTOMATIZADOS - APIs PDPw

**Data:** $(Get-Date -Format 'dd/MM/yyyy HH:mm:ss')  
**Duração Total:** $([math]::Round($totalDuration, 2))s  
**Base URL:** $BaseUrl

---

## ?? RESUMO EXECUTIVO

| Métrica | Valor |
|---------|-------|
| **Total de Testes** | $totalTests |
| **Sucessos** | ? $successTests |
| **Falhas** | ? $failedTests |
| **Taxa de Sucesso** | $successRate% |
| **Tempo Médio** | ${avgDuration}ms |

---

## ?? RESULTADOS POR API

"@

foreach ($apiGroup in $testsByApi) {
    $apiName = $apiGroup.Name
    $apiTests = $apiGroup.Group
    $apiSuccess = ($apiTests | Where-Object { $_.Success }).Count
    $apiTotal = $apiTests.Count
    $apiRate = [math]::Round(($apiSuccess / $apiTotal) * 100, 2)
    
    $report += @"

### API: $apiName

**Testes:** $apiTotal | **Sucessos:** $apiSuccess | **Taxa:** $apiRate%

| Método | Endpoint | Descrição | Status | Tempo |
|--------|----------|-----------|--------|-------|
"@
    
    foreach ($test in $apiTests) {
        $status = if ($test.Success) { "?" } else { "?" }
        $report += "`n| $($test.Method) | $($test.Endpoint) | $($test.Description) | $status | $($test.Duration)ms |"
    }
    
    $report += "`n"
}

$report += @"

---

## ?? DETALHAMENTO DE FALHAS

"@

$failures = $testResults | Where-Object { $_.Success -eq $false }
if ($failures.Count -eq 0) {
    $report += "`n? **Nenhuma falha detectada!**`n"
}
else {
    foreach ($fail in $failures) {
        $report += @"

### ? $($fail.Description)

- **Método:** $($fail.Method)
- **Endpoint:** $($fail.Endpoint)
- **Status Code:** $($fail.StatusCode)
- **Erro:** $($fail.Error)

"@
    }
}

$report += @"

---

## ?? ANÁLISE DE PERFORMANCE

| API | Tempo Médio (ms) | Testes |
|-----|------------------|--------|
"@

foreach ($apiGroup in $testsByApi) {
    $avgTime = [math]::Round(($apiGroup.Group | Measure-Object -Property Duration -Average).Average, 2)
    $report += "`n| $($apiGroup.Name) | $avgTime | $($apiGroup.Count) |"
}

$report += @"


---

## ? CHECKLIST DE VALIDAÇÃO

"@

$checkItems = @(
    @{ Name = "GET Endpoints"; Passed = ($testResults | Where-Object { $_.Method -eq "GET" -and $_.Success }).Count -gt 0 },
    @{ Name = "POST Endpoints"; Passed = ($testResults | Where-Object { $_.Method -eq "POST" -and $_.Success }).Count -gt 0 },
    @{ Name = "PUT Endpoints"; Passed = ($testResults | Where-Object { $_.Method -eq "PUT" -and $_.Success }).Count -gt 0 },
    @{ Name = "Todas as APIs testadas"; Passed = $testsByApi.Count -ge 8 },
    @{ Name = "Taxa de sucesso >= 90%"; Passed = $successRate -ge 90 },
    @{ Name = "Tempo médio < 1000ms"; Passed = $avgDuration -lt 1000 }
)

foreach ($item in $checkItems) {
    $icon = if ($item.Passed) { "?" } else { "?" }
    $report += "`n- [$icon] $($item.Name)"
}

$report += @"


---

## ?? CONCLUSÃO

"@

if ($successRate -eq 100) {
    $report += @"

? **TODOS OS TESTES PASSARAM COM SUCESSO!**

O sistema está 100% funcional e pronto para uso em produção.
"@
}
elseif ($successRate -ge 90) {
    $report += @"

? **SISTEMA APROVADO COM RESSALVAS**

Taxa de sucesso acima de 90%. Verificar falhas antes de produção.
"@
}
else {
    $report += @"

?? **SISTEMA REQUER CORREÇÕES**

Taxa de sucesso abaixo de 90%. Revisar e corrigir falhas antes de continuar.
"@
}

$report += @"


---

**Gerado automaticamente em:** $(Get-Date -Format 'dd/MM/yyyy HH:mm:ss')  
**Script:** Test-AllApis.ps1
"@

# Salvar relatório
$report | Out-File -FilePath $ReportPath -Encoding UTF8

# Exibir resumo no console
Write-Host "?? ESTATÍSTICAS FINAIS:" -ForegroundColor Green
Write-Host "  • Total de Testes: $totalTests" -ForegroundColor White
Write-Host "  • Sucessos: $successTests (?)" -ForegroundColor Green
Write-Host "  • Falhas: $failedTests (?)" -ForegroundColor Red
Write-Host "  • Taxa de Sucesso: $successRate%" -ForegroundColor Yellow
Write-Host "  • Tempo Médio: ${avgDuration}ms" -ForegroundColor White
Write-Host "  • Duração Total: $([math]::Round($totalDuration, 2))s" -ForegroundColor White
Write-Host ""
Write-Host "?? Relatório salvo em: $ReportPath" -ForegroundColor Cyan
Write-Host ""

if ($successRate -eq 100) {
    Write-Host "?? TODOS OS TESTES PASSARAM! ??" -ForegroundColor Green
}
elseif ($successRate -ge 90) {
    Write-Host "? Sistema aprovado com $failedTests falha(s)" -ForegroundColor Yellow
}
else {
    Write-Host "?? ATENÇÃO: $failedTests falhas detectadas!" -ForegroundColor Red
}

Write-Host ""
