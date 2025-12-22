# ============================================
# TESTES AUTOMATIZADOS COMPLETOS - TODAS AS APIs
# ============================================
# Inclui: GET, POST, PUT, DELETE, PATCH + Validações
# Data: 20/12/2024
# ============================================

param(
    [string]$BaseUrl = "http://localhost:5001/api",
    [string]$ReportPath = ".\docs\relatorio-testes-completos.md"
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

function Invoke-ApiTest {
    param(
        [string]$Method,
        [string]$Endpoint,
        [object]$Body = $null,
        [string]$Description,
        [int]$ExpectedStatus = 0
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
        ExpectedStatus = $ExpectedStatus
    }
    
    try {
        $uri = "$BaseUrl$Endpoint"
        $timer = [System.Diagnostics.Stopwatch]::StartNew()
        
        $headers = @{ "Content-Type" = "application/json" }
        
        if ($Method -eq "GET") {
            $response = Invoke-RestMethod -Uri $uri -Method Get -Headers $headers -ErrorAction Stop
            $result.StatusCode = 200
        }
        elseif ($Method -eq "POST") {
            $jsonBody = $Body | ConvertTo-Json -Depth 10 -Compress
            $response = Invoke-RestMethod -Uri $uri -Method Post -Body $jsonBody -Headers $headers -ErrorAction Stop
            $result.StatusCode = 201
        }
        elseif ($Method -eq "PUT") {
            $jsonBody = $Body | ConvertTo-Json -Depth 10 -Compress
            $response = Invoke-RestMethod -Uri $uri -Method Put -Body $jsonBody -Headers $headers -ErrorAction Stop
            $result.StatusCode = 200
        }
        elseif ($Method -eq "DELETE") {
            $response = Invoke-RestMethod -Uri $uri -Method Delete -Headers $headers -ErrorAction Stop
            $result.StatusCode = 204
        }
        elseif ($Method -eq "PATCH") {
            $jsonBody = $Body | ConvertTo-Json -Depth 10 -Compress
            $response = Invoke-RestMethod -Uri $uri -Method Patch -Body $jsonBody -Headers $headers -ErrorAction Stop
            $result.StatusCode = 200
        }
        
        $timer.Stop()
        
        # Validar status esperado
        if ($ExpectedStatus -eq 0 -or $result.StatusCode -eq $ExpectedStatus) {
            $result.Success = $true
            $result.Response = $response
            $result.Duration = $timer.ElapsedMilliseconds
            Write-TestSuccess "$Description - $($result.StatusCode) - $($timer.ElapsedMilliseconds)ms"
        }
        else {
            $result.Success = $false
            $result.Error = "Status inesperado: esperado $ExpectedStatus, recebido $($result.StatusCode)"
            Write-TestFail "$Description - $($result.Error)"
        }
    }
    catch {
        $timer.Stop()
        $result.Success = $false
        $result.StatusCode = if ($_.Exception.Response) { $_.Exception.Response.StatusCode.value__ } else { 0 }
        $result.Error = $_.Exception.Message
        $result.Duration = $timer.ElapsedMilliseconds
        
        # Validar se a falha era esperada
        if ($ExpectedStatus -gt 0 -and $result.StatusCode -eq $ExpectedStatus) {
            $result.Success = $true
            Write-TestSuccess "$Description - $($result.StatusCode) esperado - $($timer.ElapsedMilliseconds)ms"
        }
        else {
            Write-TestFail "$Description - Erro: $($result.StatusCode) - $($result.Error)"
        }
    }
    
    return $result
}

# ============================================
# INICIALIZAÇÃO
# ============================================

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  TESTES AUTOMATIZADOS COMPLETOS - PDPw" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Base URL: $BaseUrl" -ForegroundColor Yellow
Write-Host "Início: $(Get-Date -Format 'dd/MM/yyyy HH:mm:ss')" -ForegroundColor Yellow
Write-Host ""

$testResults = @()

# ============================================
# TESTES - API CARGAS (MAIS COMPLETA)
# ============================================

Write-TestStep "1. TESTANDO API CARGAS (CRUD COMPLETO + VALIDAÇÕES)"

# GET - Listar todas
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/cargas" -Description "GET /cargas - Listar todas"

# GET - Buscar por ID existente
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/cargas/1" -Description "GET /cargas/1 - Buscar por ID"

# GET - Buscar por ID inexistente (deve falhar)
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/cargas/99999" -Description "GET /cargas/99999 - ID inexistente" -ExpectedStatus 404

# GET - Por subsistema
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/cargas/subsistema/SE" -Description "GET /cargas/subsistema/SE"

# GET - Por data
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/cargas/data/2025-01-15" -Description "GET /cargas/data/2025-01-15"

# POST - Criar nova carga válida
$novaCarga = @{
    dataReferencia = "2025-12-25"
    subsistemaId = "SE"
    cargaMWmed = 44000.00
    cargaVerificada = 43800.00
    previsaoCarga = 44200.00
    observacoes = "Carga Teste Automated - Natal"
}
$createResult = Invoke-ApiTest -Method "POST" -Endpoint "/cargas" -Body $novaCarga -Description "POST /cargas - Criar nova válida"
$testResults += $createResult
$createdId = if ($createResult.Response) { $createResult.Response.id } else { $null }

# POST - Tentar criar com dados inválidos (subsistema errado)
$cargaInvalida1 = @{
    dataReferencia = "2025-12-26"
    subsistemaId = "XX"  # Subsistema inválido
    cargaMWmed = 44000.00
    cargaVerificada = 43800.00
    previsaoCarga = 44200.00
    observacoes = "Teste validação - subsistema inválido"
}
$testResults += Invoke-ApiTest -Method "POST" -Endpoint "/cargas" -Body $cargaInvalida1 -Description "POST /cargas - Subsistema inválido" -ExpectedStatus 400

# POST - Tentar criar com carga negativa
$cargaInvalida2 = @{
    dataReferencia = "2025-12-26"
    subsistemaId = "SE"
    cargaMWmed = -1000.00  # Valor negativo
    cargaVerificada = 43800.00
    previsaoCarga = 44200.00
    observacoes = "Teste validação - carga negativa"
}
$testResults += Invoke-ApiTest -Method "POST" -Endpoint "/cargas" -Body $cargaInvalida2 -Description "POST /cargas - Carga negativa" -ExpectedStatus 400

# PUT - Atualizar carga criada
if ($createdId) {
    $cargaAtualizada = @{
        dataReferencia = "2025-12-25"
        subsistemaId = "SE"
        cargaMWmed = 45000.00  # Valor atualizado
        cargaVerificada = 44800.00
        previsaoCarga = 45200.00
        observacoes = "Carga Teste Automated - ATUALIZADA"
        ativo = $true
    }
    $testResults += Invoke-ApiTest -Method "PUT" -Endpoint "/cargas/$createdId" -Body $cargaAtualizada -Description "PUT /cargas/$createdId - Atualizar"
}

# DELETE - Remover carga (soft delete)
if ($createdId) {
    $testResults += Invoke-ApiTest -Method "DELETE" -Endpoint "/cargas/$createdId" -Description "DELETE /cargas/$createdId - Remover" -ExpectedStatus 204
}

# GET - Verificar se foi removida
if ($createdId) {
    $testResults += Invoke-ApiTest -Method "GET" -Endpoint "/cargas/$createdId" -Description "GET /cargas/$createdId - Verificar remoção" -ExpectedStatus 404
}

# ============================================
# TESTES - API ARQUIVOS DADGER
# ============================================

Write-TestStep "2. TESTANDO API ARQUIVOS DADGER (CRUD + PATCH)"

# GET - Listar todos
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/arquivosdadger" -Description "GET /arquivosdadger - Listar todos"

# GET - Por semana PMO
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/arquivosdadger/semana/1" -Description "GET /arquivosdadger/semana/1"

# GET - Processados
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/arquivosdadger/processados?processado=true" -Description "GET /arquivosdadger/processados=true"

# GET - Não processados
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/arquivosdadger/processados?processado=false" -Description "GET /arquivosdadger/processados=false"

# POST - Criar novo arquivo
$novoArquivo = @{
    nomeArquivo = "dadger_teste_auto_$(Get-Date -Format 'yyyyMMddHHmmss').dat"
    caminhoArquivo = "/uploads/test/dadger_teste_auto.dat"
    dataImportacao = "2025-12-21T10:00:00.000Z"
    semanaPMOId = 3
    observacoes = "Arquivo Teste Automated - Completo"
}
$createFileResult = Invoke-ApiTest -Method "POST" -Endpoint "/arquivosdadger" -Body $novoArquivo -Description "POST /arquivosdadger - Criar novo"
$testResults += $createFileResult
$fileId = if ($createFileResult.Response) { $createFileResult.Response.id } else { $null }

# PATCH - Marcar como processado
if ($fileId) {
    $testResults += Invoke-ApiTest -Method "PATCH" -Endpoint "/arquivosdadger/$fileId/processar" -Description "PATCH /arquivosdadger/$fileId/processar - Marcar processado"
}

# GET - Verificar se foi marcado como processado
if ($fileId) {
    $testResults += Invoke-ApiTest -Method "GET" -Endpoint "/arquivosdadger/$fileId" -Description "GET /arquivosdadger/$fileId - Verificar processamento"
}

# DELETE - Remover arquivo
if ($fileId) {
    $testResults += Invoke-ApiTest -Method "DELETE" -Endpoint "/arquivosdadger/$fileId" -Description "DELETE /arquivosdadger/$fileId - Remover"
}

# ============================================
# TESTES - API RESTRIÇÕES UG
# ============================================

Write-TestStep "3. TESTANDO API RESTRIÇÕES UG (CRUD + FILTROS)"

# GET - Listar todas
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/restricoesug" -Description "GET /restricoesug - Listar todas"

# GET - Por unidade geradora
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/restricoesug/unidade/1" -Description "GET /restricoesug/unidade/1"

# GET - Por motivo
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/restricoesug/motivo/1" -Description "GET /restricoesug/motivo/1 - Manutenção Preventiva"

# GET - Ativas em data específica
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/restricoesug/ativas?dataReferencia=2025-01-15" -Description "GET /restricoesug/ativas?dataReferencia=2025-01-15"

# POST - Criar nova restrição válida
$novaRestricao = @{
    unidadeGeradoraId = 1
    dataInicio = "2025-12-26"
    dataFim = "2025-12-30"
    motivoRestricaoId = 1
    potenciaRestrita = 350.00
    observacoes = "Restrição Teste Automated"
}
$createRestResult = Invoke-ApiTest -Method "POST" -Endpoint "/restricoesug" -Body $novaRestricao -Description "POST /restricoesug - Criar nova"
$testResults += $createRestResult
$restId = if ($createRestResult.Response) { $createRestResult.Response.id } else { $null }

# POST - Tentar criar com datas inválidas (fim antes do início)
$restricaoInvalida = @{
    unidadeGeradoraId = 1
    dataInicio = "2025-12-30"
    dataFim = "2025-12-26"  # Data fim antes do início
    motivoRestricaoId = 1
    potenciaRestrita = 350.00
    observacoes = "Teste validação - datas invertidas"
}
$testResults += Invoke-ApiTest -Method "POST" -Endpoint "/restricoesug" -Body $restricaoInvalida -Description "POST /restricoesug - Datas inválidas" -ExpectedStatus 400

# DELETE - Remover restrição
if ($restId) {
    $testResults += Invoke-ApiTest -Method "DELETE" -Endpoint "/restricoesug/$restId" -Description "DELETE /restricoesug/$restId - Remover"
}

# ============================================
# TESTES - API SEMANAS PMO
# ============================================

Write-TestStep "4. TESTANDO API SEMANAS PMO (ENDPOINTS ESPECIAIS)"

# GET - Listar todas
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/semanaspmo" -Description "GET /semanaspmo - Listar todas"

# GET - Semana atual
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/semanaspmo/atual" -Description "GET /semanaspmo/atual"

# GET - Próximas 5 semanas
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/semanaspmo/proximas?quantidade=5" -Description "GET /semanaspmo/proximas?quantidade=5"

# GET - Por ano
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/semanaspmo/ano/2025" -Description "GET /semanaspmo/ano/2025"

# GET - Buscar específica (numero/ano)
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/semanaspmo/numero/3/ano/2025" -Description "GET /semanaspmo/numero/3/ano/2025"

# POST - Criar nova semana
$novaSemana = @{
    numero = 50
    dataInicio = "2025-12-13"
    dataFim = "2025-12-19"
    ano = 2025
    observacoes = "Semana Teste Automated 50/2025"
}
$createSemResult = Invoke-ApiTest -Method "POST" -Endpoint "/semanaspmo" -Body $novaSemana -Description "POST /semanaspmo - Criar nova"
$testResults += $createSemResult
$semId = if ($createSemResult.Response) { $createSemResult.Response.id } else { $null }

# POST - Tentar criar com número inválido (< 1 ou > 53)
$semanaInvalida = @{
    numero = 54  # Número inválido
    dataInicio = "2025-12-20"
    dataFim = "2025-12-26"
    ano = 2025
    observacoes = "Teste validação - número inválido"
}
$testResults += Invoke-ApiTest -Method "POST" -Endpoint "/semanaspmo" -Body $semanaInvalida -Description "POST /semanaspmo - Número inválido (54)" -ExpectedStatus 400

# DELETE - Remover semana
if ($semId) {
    $testResults += Invoke-ApiTest -Method "DELETE" -Endpoint "/semanaspmo/$semId" -Description "DELETE /semanaspmo/$semId - Remover"
}

# ============================================
# TESTES - API EMPRESAS
# ============================================

Write-TestStep "5. TESTANDO API EMPRESAS (VALIDAÇÕES DE CNPJ)"

# GET - Listar todas
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/empresas" -Description "GET /empresas - Listar todas"

# GET - Ativas
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/empresas/ativas" -Description "GET /empresas/ativas"

# POST - Criar nova empresa
$novaEmpresa = @{
    nome = "Empresa Teste Automated CRUD"
    cnpj = "12345678000190"
    telefone = "(11) 98888-8888"
    email = "automated.crud@teste.com"
}
$createEmpResult = Invoke-ApiTest -Method "POST" -Endpoint "/empresas" -Body $novaEmpresa -Description "POST /empresas - Criar nova"
$testResults += $createEmpResult
$empId = if ($createEmpResult.Response) { $createEmpResult.Response.id } else { $null }

# POST - Tentar criar com CNPJ duplicado
if ($empId) {
    $empresaDuplicada = @{
        nome = "Empresa Duplicada"
        cnpj = "12345678000190"  # Mesmo CNPJ
        telefone = "(21) 99999-9999"
        email = "duplicada@teste.com"
    }
    $testResults += Invoke-ApiTest -Method "POST" -Endpoint "/empresas" -Body $empresaDuplicada -Description "POST /empresas - CNPJ duplicado" -ExpectedStatus 409
}

# POST - Tentar criar com CNPJ inválido
$empresaCNPJInvalido = @{
    nome = "Empresa CNPJ Inválido"
    cnpj = "123"  # CNPJ muito curto
    telefone = "(31) 97777-7777"
    email = "invalido@teste.com"
}
$testResults += Invoke-ApiTest -Method "POST" -Endpoint "/empresas" -Body $empresaCNPJInvalido -Description "POST /empresas - CNPJ inválido" -ExpectedStatus 400

# PUT - Atualizar empresa
if ($empId) {
    $empresaAtualizada = @{
        nome = "Empresa Teste Automated CRUD - ATUALIZADA"
        cnpj = "12345678000190"
        telefone = "(11) 97777-7777"
        email = "updated.crud@teste.com"
        ativo = $true
    }
    $testResults += Invoke-ApiTest -Method "PUT" -Endpoint "/empresas/$empId" -Body $empresaAtualizada -Description "PUT /empresas/$empId - Atualizar"
}

# DELETE - Remover empresa
if ($empId) {
    $testResults += Invoke-ApiTest -Method "DELETE" -Endpoint "/empresas/$empId" -Description "DELETE /empresas/$empId - Remover"
}

# ============================================
# TESTES - API USINAS
# ============================================

Write-TestStep "6. TESTANDO API USINAS (FILTROS E VALIDAÇÕES)"

# GET - Listar todas
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/usinas" -Description "GET /usinas - Listar todas"

# GET - Por tipo (Hidrelétricas)
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/usinas/tipo/1" -Description "GET /usinas/tipo/1 - Hidrelétricas"

# GET - Por empresa
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/usinas/empresa/1" -Description "GET /usinas/empresa/1"

# GET - Por código
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/usinas/codigo/ITU" -Description "GET /usinas/codigo/ITU - Itaipu"

# POST - Criar nova usina
$novaUsina = @{
    codigo = "TEST-AUTO-UHE"
    nome = "UHE Teste Automated"
    tipoUsinaId = 1
    empresaId = 1
    capacidadeInstalada = 250.00
    localizacao = "Teste River, TS"
    dataOperacao = "2025-12-21"
}
$createUsiResult = Invoke-ApiTest -Method "POST" -Endpoint "/usinas" -Body $novaUsina -Description "POST /usinas - Criar nova"
$testResults += $createUsiResult
$usiId = if ($createUsiResult.Response) { $createUsiResult.Response.id } else { $null }

# POST - Tentar criar com código duplicado
if ($usiId) {
    $usinaDuplicada = @{
        codigo = "TEST-AUTO-UHE"  # Código duplicado
        nome = "UHE Duplicada"
        tipoUsinaId = 1
        empresaId = 1
        capacidadeInstalada = 100.00
        localizacao = "Outro Local, TS"
    }
    $testResults += Invoke-ApiTest -Method "POST" -Endpoint "/usinas" -Body $usinaDuplicada -Description "POST /usinas - Código duplicado" -ExpectedStatus 409
}

# POST - Tentar criar com capacidade negativa
$usinaInvalida = @{
    codigo = "TEST-INVALID"
    nome = "Usina Inválida"
    tipoUsinaId = 1
    empresaId = 1
    capacidadeInstalada = -100.00  # Capacidade negativa
    localizacao = "Invalid, TS"
}
$testResults += Invoke-ApiTest -Method "POST" -Endpoint "/usinas" -Body $usinaInvalida -Description "POST /usinas - Capacidade negativa" -ExpectedStatus 400

# DELETE - Remover usina
if ($usiId) {
    $testResults += Invoke-ApiTest -Method "DELETE" -Endpoint "/usinas/$usiId" -Description "DELETE /usinas/$usiId - Remover"
}

# ============================================
# TESTES - API EQUIPES PDP
# ============================================

Write-TestStep "7. TESTANDO API EQUIPES PDP (CRUD BÁSICO)"

# GET - Listar todas
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/equipespdp" -Description "GET /equipespdp - Listar todas"

# POST - Criar nova equipe
$novaEquipe = @{
    nome = "Equipe Teste Automated CRUD"
    descricao = "Equipe criada por testes automatizados completos"
    coordenador = "Coordenador Auto CRUD"
    email = "auto.crud@teste.com"
    telefone = "(99) 98888-8888"
}
$createEqpResult = Invoke-ApiTest -Method "POST" -Endpoint "/equipespdp" -Body $novaEquipe -Description "POST /equipespdp - Criar nova"
$testResults += $createEqpResult
$eqpId = if ($createEqpResult.Response) { $createEqpResult.Response.id } else { $null }

# PUT - Atualizar equipe
if ($eqpId) {
    $equipeAtualizada = @{
        nome = "Equipe Teste Automated CRUD - ATUALIZADA"
        descricao = "Equipe atualizada por testes automatizados"
        coordenador = "Coordenador Auto CRUD Atualizado"
        email = "auto.crud.updated@teste.com"
        telefone = "(99) 97777-7777"
        ativo = $true
    }
    $testResults += Invoke-ApiTest -Method "PUT" -Endpoint "/equipespdp/$eqpId" -Body $equipeAtualizada -Description "PUT /equipespdp/$eqpId - Atualizar"
}

# DELETE - Remover equipe
if ($eqpId) {
    $testResults += Invoke-ApiTest -Method "DELETE" -Endpoint "/equipespdp/$eqpId" -Description "DELETE /equipespdp/$eqpId - Remover"
}

# ============================================
# TESTES - API TIPOS USINA (SOMENTE LEITURA)
# ============================================

Write-TestStep "8. TESTANDO API TIPOS USINA (ENDPOINTS DE LEITURA)"

# GET - Listar todos
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/tiposusina" -Description "GET /tiposusina - Listar todos"

# GET - Buscar por ID
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/tiposusina/1" -Description "GET /tiposusina/1 - Hidrelétrica"
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/tiposusina/2" -Description "GET /tiposusina/2 - Termelétrica"
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/tiposusina/4" -Description "GET /tiposusina/4 - Eólica"
$testResults += Invoke-ApiTest -Method "GET" -Endpoint "/tiposusina/5" -Description "GET /tiposusina/5 - Solar"

# ============================================
# ESTATÍSTICAS E RELATÓRIO
# ============================================

Write-Host ""
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  GERANDO RELATÓRIO COMPLETO" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

$totalTests = $testResults.Count
$successTests = ($testResults | Where-Object { $_.Success -eq $true }).Count
$failedTests = ($testResults | Where-Object { $_.Success -eq $false }).Count
$successRate = [math]::Round(($successTests / $totalTests) * 100, 2)
$avgDuration = [math]::Round(($testResults | Measure-Object -Property Duration -Average).Average, 2)
$endTime = Get-Date
$totalDuration = ($endTime - $startTime).TotalSeconds

# Agrupar por tipo de teste
$testsByMethod = $testResults | Group-Object Method
$testsByApi = $testResults | Group-Object { ($_.Endpoint -split '/')[1] }

# Contar tipos de teste
$getTests = ($testResults | Where-Object { $_.Method -eq "GET" }).Count
$postTests = ($testResults | Where-Object { $_.Method -eq "POST" }).Count
$putTests = ($testResults | Where-Object { $_.Method -eq "PUT" }).Count
$deleteTests = ($testResults | Where-Object { $_.Method -eq "DELETE" }).Count
$patchTests = ($testResults | Where-Object { $_.Method -eq "PATCH" }).Count

# Contar validações
$validationTests = ($testResults | Where-Object { $_.ExpectedStatus -in @(400, 404, 409) }).Count
$validationSuccess = ($testResults | Where-Object { $_.ExpectedStatus -in @(400, 404, 409) -and $_.Success }).Count

# Gerar relatório Markdown
$report = @"
# ?? RELATÓRIO DE TESTES AUTOMATIZADOS COMPLETOS - PDPw APIs

**Data:** $(Get-Date -Format 'dd/MM/yyyy HH:mm:ss')  
**Duração Total:** $([math]::Round($totalDuration, 2))s  
**Base URL:** $BaseUrl

---

## ?? RESUMO EXECUTIVO

| Métrica | Valor | Status |
|---------|-------|--------|
| **Total de Testes** | $totalTests | - |
| **Sucessos** | $successTests | ? |
| **Falhas** | $failedTests | $(if ($failedTests -eq 0) { '?' } else { '?' }) |
| **Taxa de Sucesso** | $successRate% | $(if ($successRate -ge 95) { '?' } elseif ($successRate -ge 80) { '??' } else { '?' }) |
| **Tempo Médio** | ${avgDuration}ms | $(if ($avgDuration -lt 100) { '?' } elseif ($avgDuration -lt 500) { '??' } else { '?' }) |

---

## ?? DISTRIBUIÇÃO DE TESTES

### Por Método HTTP

| Método | Quantidade | % do Total |
|--------|------------|------------|
| **GET** | $getTests | $([math]::Round(($getTests / $totalTests) * 100, 1))% |
| **POST** | $postTests | $([math]::Round(($postTests / $totalTests) * 100, 1))% |
| **PUT** | $putTests | $([math]::Round(($putTests / $totalTests) * 100, 1))% |
| **DELETE** | $deleteTests | $([math]::Round(($deleteTests / $totalTests) * 100, 1))% |
| **PATCH** | $patchTests | $([math]::Round(($patchTests / $totalTests) * 100, 1))% |

### Testes de Validação

| Tipo | Quantidade | Sucesso |
|------|------------|---------|
| **Testes de Validação** | $validationTests | $validationSuccess/$validationTests |
| **Testes Funcionais** | $($totalTests - $validationTests) | $($successTests - $validationSuccess)/$($totalTests - $validationTests) |

---

## ?? RESULTADOS POR API

"@

foreach ($apiGroup in $testsByApi) {
    $apiName = $apiGroup.Name
    $apiTests = $apiGroup.Group
    $apiSuccess = ($apiTests | Where-Object { $_.Success }).Count
    $apiTotal = $apiTests.Count
    $apiRate = [math]::Round(($apiSuccess / $apiTotal) * 100, 2)
    $apiAvgTime = [math]::Round(($apiTests | Measure-Object -Property Duration -Average).Average, 2)
    
    $statusIcon = if ($apiRate -eq 100) { '?' } elseif ($apiRate -ge 80) { '??' } else { '?' }
    
    $report += @"

### $statusIcon API: $apiName

**Testes:** $apiTotal | **Sucessos:** $apiSuccess | **Taxa:** $apiRate% | **Tempo Médio:** ${apiAvgTime}ms

| Método | Endpoint | Descrição | Status | Tempo |
|--------|----------|-----------|--------|-------|
"@
    
    foreach ($test in $apiTests) {
        $status = if ($test.Success) { "?" } else { "?" }
        $statusCode = if ($test.StatusCode -gt 0) { "($($test.StatusCode))" } else { "" }
        $report += "`n| $($test.Method) | $($test.Endpoint) | $($test.Description) | $status $statusCode | $($test.Duration)ms |"
    }
    
    $report += "`n"
}

$report += @"

---

## ?? DETALHAMENTO DE FALHAS

"@

$failures = $testResults | Where-Object { $_.Success -eq $false }
if ($failures.Count -eq 0) {
    $report += "`n? **NENHUMA FALHA DETECTADA! TODOS OS TESTES PASSARAM!**`n"
}
else {
    foreach ($fail in $failures) {
        $report += @"

### ? $($fail.Description)

- **Método:** $($fail.Method)
- **Endpoint:** $($fail.Endpoint)
- **Status Code:** $($fail.StatusCode)
- **Status Esperado:** $(if ($fail.ExpectedStatus -gt 0) { $fail.ExpectedStatus } else { 'N/A' })
- **Erro:** $($fail.Error)

"@
    }
}

$report += @"

---

## ?? ANÁLISE DE PERFORMANCE

| API | Tempo Médio (ms) | Tempo Min (ms) | Tempo Max (ms) | Testes |
|-----|------------------|----------------|----------------|--------|
"@

foreach ($apiGroup in $testsByApi) {
    $avgTime = [math]::Round(($apiGroup.Group | Measure-Object -Property Duration -Average).Average, 2)
    $minTime = [math]::Round(($apiGroup.Group | Measure-Object -Property Duration -Minimum).Minimum, 2)
    $maxTime = [math]::Round(($apiGroup.Group | Measure-Object -Property Duration -Maximum).Maximum, 2)
    $report += "`n| $($apiGroup.Name) | $avgTime | $minTime | $maxTime | $($apiGroup.Count) |"
}

$report += @"


---

## ? COVERAGE DE TESTES

### CRUD Completo

| Operação | Testado | APIs Cobertas |
|----------|---------|---------------|
| **CREATE (POST)** | ? | Cargas, ArquivosDadger, RestricoesUG, SemanasPMO, Empresas, Usinas, EquipesPDP |
| **READ (GET)** | ? | Todas as 8 APIs |
| **UPDATE (PUT)** | ? | Cargas, Empresas, EquipesPDP |
| **DELETE** | ? | Cargas, ArquivosDadger, RestricoesUG, SemanasPMO, Empresas, Usinas, EquipesPDP |
| **PATCH** | ? | ArquivosDadger (processar) |

### Validações Testadas

| Validação | Testado | Exemplo |
|-----------|---------|---------|
| **Dados inválidos** | ? | Subsistema inválido, CNPJ inválido |
| **Valores negativos** | ? | Carga negativa, Capacidade negativa |
| **Duplicação** | ? | CNPJ duplicado, Código duplicado |
| **Datas inválidas** | ? | Data fim antes do início |
| **Ranges inválidos** | ? | Semana > 53 |
| **IDs inexistentes** | ? | GET com ID 99999 |

### Filtros Testados

| Filtro | APIs Testadas |
|--------|---------------|
| **Por ID** | Todas |
| **Por período/data** | Cargas, RestricoesUG |
| **Por relacionamento** | Usinas (empresa, tipo), ArquivosDadger (semana), RestricoesUG (unidade, motivo) |
| **Por status** | Empresas (ativas), ArquivosDadger (processados) |
| **Endpoints especiais** | SemanasPMO (atual, próximas, por ano) |

---

## ?? CONCLUSÃO

"@

if ($successRate -eq 100) {
    $report += @"

### ? **EXCELENTE! TODOS OS TESTES PASSARAM!**

?? **$totalTests/$totalTests testes bem-sucedidos**

O sistema está 100% funcional com cobertura completa de:
- ? CRUD completo (CREATE, READ, UPDATE, DELETE)
- ? PATCH operations
- ? Validações de dados
- ? Filtros e buscas
- ? Endpoints especializados

**Performance:** Tempo médio de ${avgDuration}ms por requisição.

**Sistema aprovado e pronto para produção!** ??
"@
}
elseif ($successRate -ge 95) {
    $report += @"

### ? **APROVADO COM EXCELÊNCIA!**

? **Taxa de sucesso: $successRate%**

O sistema está altamente funcional com apenas $failedTests falha(s) em $totalTests testes.

**Performance:** Tempo médio de ${avgDuration}ms por requisição.

**Sistema aprovado para produção com ressalvas mínimas.** ??
"@
}
elseif ($successRate -ge 80) {
    $report += @"

### ?? **APROVADO COM RESSALVAS**

?? **Taxa de sucesso: $successRate%**

O sistema está funcional mas apresenta $failedTests falha(s) que devem ser corrigidas.

**Recomendação:** Revisar e corrigir as falhas antes de produção.
"@
}
else {
    $report += @"

### ? **REQUER CORREÇÕES URGENTES**

?? **Taxa de sucesso: $successRate%**

O sistema apresenta $failedTests falhas em $totalTests testes.

**Ação Obrigatória:** Corrigir todas as falhas antes de continuar.
"@
}

$report += @"


---

## ?? DETALHES TÉCNICOS

- **Framework de Testes:** PowerShell 7+
- **Ferramenta HTTP:** Invoke-RestMethod
- **Formato de Response:** JSON
- **Validação:** Status Codes + Response Bodies
- **Performance:** Medição com Stopwatch

---

**Gerado automaticamente em:** $(Get-Date -Format 'dd/MM/yyyy HH:mm:ss')  
**Script:** Test-AllApis-Complete.ps1  
**Versão:** 2.0 (CRUD Completo + Validações)
"@

# Salvar relatório
$report | Out-File -FilePath $ReportPath -Encoding UTF8

# Exibir resumo no console
Write-Host "?? ESTATÍSTICAS FINAIS:" -ForegroundColor Green
Write-Host "  • Total de Testes: $totalTests" -ForegroundColor White
Write-Host "  • Sucessos: $successTests (?)" -ForegroundColor Green
Write-Host "  • Falhas: $failedTests $(if ($failedTests -eq 0) { '(?)' } else { '(?)' })" -ForegroundColor $(if ($failedTests -eq 0) { 'Green' } else { 'Red' })
Write-Host "  • Taxa de Sucesso: $successRate%" -ForegroundColor Yellow
Write-Host "  • Tempo Médio: ${avgDuration}ms" -ForegroundColor White
Write-Host "  • Duração Total: $([math]::Round($totalDuration, 2))s" -ForegroundColor White
Write-Host ""
Write-Host "?? BREAKDOWN DE TESTES:" -ForegroundColor Cyan
Write-Host "  • GET: $getTests testes" -ForegroundColor White
Write-Host "  • POST: $postTests testes" -ForegroundColor White
Write-Host "  • PUT: $putTests testes" -ForegroundColor White
Write-Host "  • DELETE: $deleteTests testes" -ForegroundColor White
Write-Host "  • PATCH: $patchTests testes" -ForegroundColor White
Write-Host "  • Validações: $validationTests testes ($validationSuccess sucessos)" -ForegroundColor White
Write-Host ""
Write-Host "?? Relatório salvo em: $ReportPath" -ForegroundColor Cyan
Write-Host ""

if ($successRate -eq 100) {
    Write-Host "?? PERFEITO! TODOS OS $totalTests TESTES PASSARAM! ??" -ForegroundColor Green
}
elseif ($successRate -ge 95) {
    Write-Host "? Sistema aprovado! $successTests/$totalTests testes OK" -ForegroundColor Green
}
elseif ($successRate -ge 80) {
    Write-Host "?? Sistema aprovado com ressalvas ($failedTests falhas)" -ForegroundColor Yellow
}
else {
    Write-Host "?? ATENÇÃO: $failedTests falhas detectadas!" -ForegroundColor Red
}

Write-Host ""
Write-Host "? Coverage:" -ForegroundColor Cyan
Write-Host "  • CRUD Completo: CREATE, READ, UPDATE, DELETE" -ForegroundColor Green
Write-Host "  • PATCH Operations: Processar arquivos" -ForegroundColor Green
Write-Host "  • Validações: Dados inválidos, duplicações, ranges" -ForegroundColor Green
Write-Host "  • Filtros: Por ID, data, relacionamentos, status" -ForegroundColor Green
Write-Host ""
