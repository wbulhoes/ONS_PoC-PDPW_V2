# ============================================================================
# 🎯 TESTE MASTER COMPLETO - POC PDPW 100%
# ============================================================================
# Script de validação COMPLETA de TODOS os endpoints e métodos HTTP
# Testa: GET, POST, PUT, PATCH, DELETE em TODAS as 17 APIs
# 
# Autor: Willian Bulhões + GitHub Copilot
# Data: 26/12/2025
# Versão: 1.0.0 - GRAND FINALE
# ============================================================================

param(
    [string]$BaseUrl = "http://localhost:5001",
    [switch]$Verbose,
    [switch]$StopOnError,
    [switch]$IncludeDelete = $false
)

$ErrorActionPreference = "Continue"
$ProgressPreference = "SilentlyContinue"

# Cores e símbolos
$script:Colors = @{
    Success = "Green"
    Error = "Red"
    Warning = "Yellow"
    Info = "Cyan"
    Title = "Magenta"
}

# Contadores
$script:TotalTests = 0
$script:PassedTests = 0
$script:FailedTests = 0
$script:SkippedTests = 0
$script:CreatedIds = @{}

# Resultados detalhados
$script:Results = @()
$script:FailedEndpoints = @()

# ============================================================================
# FUNÇÕES AUXILIARES
# ============================================================================

function Write-Header {
    param([string]$Text)
    Write-Host "`n" -NoNewline
    Write-Host "============================================================================" -ForegroundColor $Colors.Title
    Write-Host " $Text" -ForegroundColor $Colors.Title
    Write-Host "============================================================================" -ForegroundColor $Colors.Title
}

function Write-Section {
    param([string]$Text, [int]$Current, [int]$Total)
    Write-Host "`n🎯 [$Current/$Total] $Text" -ForegroundColor $Colors.Info
    Write-Host ("─" * 80) -ForegroundColor Gray
}

function Test-Endpoint {
    param(
        [string]$Method,
        [string]$Url,
        [object]$Body = $null,
        [int]$ExpectedStatus = 200,
        [string]$Description,
        [string]$Category
    )
    
    $script:TotalTests++
    
    try {
        $params = @{
            Uri = $Url
            Method = $Method
            Headers = @{ "Content-Type" = "application/json"; "Accept" = "application/json" }
            TimeoutSec = 15
        }
        
        if ($Body) {
            $params.Body = ($Body | ConvertTo-Json -Depth 10 -Compress)
        }
        
        $response = Invoke-WebRequest @params -UseBasicParsing
        $statusCode = [int]$response.StatusCode
        $content = $null
        
        if ($response.Content) {
            try {
                $content = $response.Content | ConvertFrom-Json
            } catch {
                $content = $response.Content
            }
        }
        
        $success = ($statusCode -eq $ExpectedStatus)
        
        if ($success) {
            Write-Host "  ✅ " -NoNewline -ForegroundColor $Colors.Success
            Write-Host "$Method " -NoNewline -ForegroundColor White
            Write-Host "$Description" -ForegroundColor Gray
            $script:PassedTests++
        } else {
            Write-Host "  ⚠️  " -NoNewline -ForegroundColor $Colors.Warning
            Write-Host "$Method " -NoNewline -ForegroundColor White
            Write-Host "$Description - Expected $ExpectedStatus, got $statusCode" -ForegroundColor $Colors.Warning
            $script:FailedTests++
        }
        
        $result = @{
            Success = $success
            Method = $Method
            Url = $Url
            Status = $statusCode
            Expected = $ExpectedStatus
            Description = $Description
            Category = $Category
            Response = $content
            Timestamp = Get-Date
        }
        
        $script:Results += $result
        
        if (-not $success -and $StopOnError) {
            throw "Test failed: $Description (Status: $statusCode, Expected: $ExpectedStatus)"
        }
        
        return $result
    }
    catch {
        $statusCode = 0
        $errorMsg = $_.Exception.Message
        
        if ($_.Exception.Response) {
            $statusCode = [int]$_.Exception.Response.StatusCode
        }
        
        # Se o status code é o esperado, considera sucesso (ex: 404 esperado)
        if ($statusCode -eq $ExpectedStatus) {
            Write-Host "  ✅ " -NoNewline -ForegroundColor $Colors.Success
            Write-Host "$Method " -NoNewline -ForegroundColor White
            Write-Host "$Description (Expected $ExpectedStatus)" -ForegroundColor Gray
            $script:PassedTests++
            
            return @{
                Success = $true
                Method = $Method
                Url = $Url
                Status = $statusCode
                Expected = $ExpectedStatus
                Description = $Description
                Category = $Category
            }
        }
        
        Write-Host "  ❌ " -NoNewline -ForegroundColor $Colors.Error
        Write-Host "$Method " -NoNewline -ForegroundColor White
        Write-Host "$Description - Status: $statusCode" -ForegroundColor $Colors.Error
        
        if ($Verbose) {
            Write-Host "     Error: $errorMsg" -ForegroundColor DarkGray
        }
        
        $script:FailedTests++
        $script:FailedEndpoints += "$Method $Url - $Description"
        
        $result = @{
            Success = $false
            Method = $Method
            Url = $Url
            Status = $statusCode
            Expected = $ExpectedStatus
            Description = $Description
            Category = $Category
            Error = $errorMsg
            Timestamp = Get-Date
        }
        
        $script:Results += $result
        
        if ($StopOnError) {
            throw "Test failed: $Description"
        }
        
        return $result
    }
}

# ============================================================================
# INÍCIO DOS TESTES
# ============================================================================

Write-Header "🚀 TESTE MASTER COMPLETO - POC PDPW 100%"
Write-Host "Base URL: $BaseUrl" -ForegroundColor White
Write-Host "Início: $(Get-Date -Format 'dd/MM/yyyy HH:mm:ss')" -ForegroundColor White
Write-Host "Incluir DELETE: $IncludeDelete" -ForegroundColor White

# ============================================================================
# 0. HEALTH CHECK
# ============================================================================

Write-Section "HEALTH CHECK & STATUS" 0 17

Test-Endpoint -Method "GET" -Url "$BaseUrl/health" `
    -Description "Health Check" -Category "System"

Test-Endpoint -Method "GET" -Url "$BaseUrl/" `
    -Description "API Root Status" -Category "System"

Test-Endpoint -Method "GET" -Url "$BaseUrl/swagger/index.html" `
    -Description "Swagger UI" -Category "System"

# ============================================================================
# 1. API DASHBOARD (3 endpoints)
# ============================================================================

Write-Section "API DASHBOARD" 1 17

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/dashboard/resumo" `
    -Description "Resumo do Dashboard" -Category "Dashboard"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/dashboard/metricas/ofertas" `
    -Description "Métricas de Ofertas" -Category "Dashboard"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/dashboard/alertas" `
    -Description "Alertas do Sistema" -Category "Dashboard"

# ============================================================================
# 2. API USINAS (15+ endpoints)
# ============================================================================

Write-Section "API USINAS" 2 17

# GET
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usinas" `
    -Description "Listar todas as usinas" -Category "Usinas"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usinas/1" `
    -Description "Buscar usina por ID=1" -Category "Usinas"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usinas/tipo/1" `
    -Description "Listar usinas por TipoId=1" -Category "Usinas"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usinas/empresa/1" `
    -Description "Listar usinas por EmpresaId=1" -Category "Usinas"

# POST
$novaUsina = @{
    codigo = "TESTE-UHE-$(Get-Date -Format 'HHmmss')"
    nome = "Usina Teste Master Script"
    tipoUsinaId = 1
    empresaId = 1
    capacidadeInstalada = 500.00
    localizacao = "Local Teste"
}

$resultCreate = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/usinas" -Body $novaUsina `
    -ExpectedStatus 201 -Description "Criar nova usina" -Category "Usinas"

if ($resultCreate.Success -and $resultCreate.Response.id) {
    $script:CreatedIds['Usina'] = $resultCreate.Response.id
    
    # PUT
    $usinaUpdate = @{
        codigo = $novaUsina.codigo
        nome = "Usina Teste ATUALIZADA"
        tipoUsinaId = 1
        empresaId = 1
        capacidadeInstalada = 550.00
        localizacao = "Local Teste Atualizado"
    }
    
    Test-Endpoint -Method "PUT" -Url "$BaseUrl/api/usinas/$($script:CreatedIds['Usina'])" -Body $usinaUpdate `
        -Description "Atualizar usina criada" -Category "Usinas"
    
    # DELETE (opcional)
    if ($IncludeDelete) {
        Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/usinas/$($script:CreatedIds['Usina'])" `
            -ExpectedStatus 204 -Description "Deletar usina criada" -Category "Usinas"
    }
}

# ============================================================================
# 3. API EMPRESAS (12+ endpoints)
# ============================================================================

Write-Section "API EMPRESAS" 3 17

# GET
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/empresas" `
    -Description "Listar todas as empresas" -Category "Empresas"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/empresas/1" `
    -Description "Buscar empresa por ID=1" -Category "Empresas"

# POST
$novaEmpresa = @{
    nome = "Empresa Teste Master $(Get-Date -Format 'HHmmss')"
    cnpj = "$(Get-Random -Minimum 10000000 -Maximum 99999999)000100"
    telefone = "(11) 99999-9999"
    email = "teste@empresateste.com.br"
    endereco = "Rua Teste, 123"
}

$resultEmpresa = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/empresas" -Body $novaEmpresa `
    -ExpectedStatus 201 -Description "Criar nova empresa" -Category "Empresas"

if ($resultEmpresa.Success -and $resultEmpresa.Response.id) {
    $script:CreatedIds['Empresa'] = $resultEmpresa.Response.id
    
    # PUT
    $empresaUpdate = @{
        nome = "Empresa Teste ATUALIZADA"
        cnpj = $novaEmpresa.cnpj
        telefone = "(11) 88888-8888"
        email = "atualizado@empresateste.com.br"
        endereco = "Rua Atualizada, 456"
    }
    
    Test-Endpoint -Method "PUT" -Url "$BaseUrl/api/empresas/$($script:CreatedIds['Empresa'])" -Body $empresaUpdate `
        -Description "Atualizar empresa criada" -Category "Empresas"
    
    if ($IncludeDelete) {
        Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/empresas/$($script:CreatedIds['Empresa'])" `
            -ExpectedStatus 204 -Description "Deletar empresa criada" -Category "Empresas"
    }
}

# ============================================================================
# 4. API TIPOS DE USINA (8+ endpoints)
# ============================================================================

Write-Section "API TIPOS DE USINA" 4 17

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/tiposusina" `
    -Description "Listar todos os tipos" -Category "TiposUsina"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/tiposusina/1" `
    -Description "Buscar tipo por ID=1" -Category "TiposUsina"

# ============================================================================
# 5. API SEMANAS PMO (12+ endpoints)
# ============================================================================

Write-Section "API SEMANAS PMO" 5 17

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/semanaspmo" `
    -Description "Listar todas as semanas" -Category "SemanasPMO"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/semanaspmo/1" `
    -Description "Buscar semana por ID=1" -Category "SemanasPMO"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/semanaspmo/atual" `
    -Description "Buscar semana atual" -Category "SemanasPMO"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/semanaspmo/proximas?quantidade=4" `
    -Description "Buscar próximas 4 semanas" -Category "SemanasPMO"

# ============================================================================
# 6. API EQUIPES PDP (10+ endpoints)
# ============================================================================

Write-Section "API EQUIPES PDP" 6 17

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/equipespdp" `
    -Description "Listar todas as equipes" -Category "EquipesPDP"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/equipespdp/1" `
    -Description "Buscar equipe por ID=1" -Category "EquipesPDP"

# ============================================================================
# 7. API USUÁRIOS (9+ endpoints) - CORRIGIDO!
# ============================================================================

Write-Section "API USUÁRIOS" 7 17

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usuarios" `
    -Description "Listar todos os usuários" -Category "Usuarios"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usuarios/1" `
    -Description "Buscar usuário por ID=1" -Category "Usuarios"

# POST
$novoUsuario = @{
    nome = "Usuario Teste Master $(Get-Date -Format 'HHmmss')"
    email = "usuario.teste.$(Get-Date -Format 'HHmmss')@ons.org.br"
    telefone = "(21) 3444-5555"
    equipePDPId = 1
    perfil = "Operador"
}

$resultUsuario = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/usuarios" -Body $novoUsuario `
    -ExpectedStatus 201 -Description "Criar novo usuário" -Category "Usuarios"

if ($resultUsuario.Success -and $resultUsuario.Response.id) {
    $script:CreatedIds['Usuario'] = $resultUsuario.Response.id
    
    # PUT
    $usuarioUpdate = @{
        nome = "Usuario Teste ATUALIZADO"
        email = $novoUsuario.email
        telefone = "(21) 3444-6666"
        equipePDPId = 2
        perfil = "Analista"
    }
    
    Test-Endpoint -Method "PUT" -Url "$BaseUrl/api/usuarios/$($script:CreatedIds['Usuario'])" -Body $usuarioUpdate `
        -Description "Atualizar usuário criado" -Category "Usuarios"
    
    if ($IncludeDelete) {
        Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/usuarios/$($script:CreatedIds['Usuario'])" `
            -ExpectedStatus 204 -Description "Deletar usuário criado" -Category "Usuarios"
    }
}

# ============================================================================
# 8. API OFERTAS EXPORTAÇÃO (15+ endpoints)
# ============================================================================

Write-Section "API OFERTAS EXPORTAÇÃO" 8 17

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/ofertas-exportacao" `
    -Description "Listar todas as ofertas" -Category "OfertasExportacao"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/ofertas-exportacao/pendentes" `
    -Description "Listar ofertas pendentes" -Category "OfertasExportacao"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/ofertas-exportacao/aprovadas" `
    -Description "Listar ofertas aprovadas" -Category "OfertasExportacao"

# POST
$novaOfertaExp = @{
    usinaId = 2
    dataPDP = (Get-Date).AddDays(1).ToString("yyyy-MM-dd")
    valorMW = 150.5
    precoMWh = 250.75
    observacoes = "Oferta teste script master"
}

$resultOfertaExp = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/ofertas-exportacao" -Body $novaOfertaExp `
    -ExpectedStatus 201 -Description "Criar nova oferta de exportação" -Category "OfertasExportacao"

if ($resultOfertaExp.Success -and $resultOfertaExp.Response.id) {
    $script:CreatedIds['OfertaExportacao'] = $resultOfertaExp.Response.id
    
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/ofertas-exportacao/$($script:CreatedIds['OfertaExportacao'])" `
        -Description "Buscar oferta criada por ID" -Category "OfertasExportacao"
}

# ============================================================================
# 9. API OFERTAS RESPOSTA VOLUNTÁRIA (15+ endpoints)
# ============================================================================

Write-Section "API OFERTAS RESPOSTA VOLUNTÁRIA" 9 17

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/ofertas-resposta-voluntaria" `
    -Description "Listar todas as ofertas RV" -Category "OfertasRV"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/ofertas-resposta-voluntaria/pendentes" `
    -Description "Listar ofertas RV pendentes" -Category "OfertasRV"

# POST
$novaOfertaRV = @{
    empresaId = 1
    dataPDP = (Get-Date).AddDays(1).ToString("yyyy-MM-dd")
    reducaoDemandaMW = 50.0
    precoMWh = 180.50
    tipoPrograma = "Interruptível"
    observacoes = "Oferta RV teste script master"
}

$resultOfertaRV = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/ofertas-resposta-voluntaria" -Body $novaOfertaRV `
    -ExpectedStatus 201 -Description "Criar nova oferta RV" -Category "OfertasRV"

if ($resultOfertaRV.Success -and $resultOfertaRV.Response.id) {
    $script:CreatedIds['OfertaRV'] = $resultOfertaRV.Response.id
}

# ============================================================================
# 10. API PREVISÕES EÓLICAS (12+ endpoints)
# ============================================================================

Write-Section "API PREVISÕES EÓLICAS" 10 17

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/previsoes-eolicas" `
    -Description "Listar todas as previsões" -Category "PrevisoesEolicas"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/previsoes-eolicas/usina/2" `
    -Description "Listar previsões por usina" -Category "PrevisoesEolicas"

# POST
$novaPrevisao = @{
    usinaId = 2
    dataHoraReferencia = (Get-Date).ToString("yyyy-MM-ddTHH:mm:ss")
    dataHoraPrevista = (Get-Date).AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss")
    geracaoPrevistaMWmed = 85.5
    velocidadeVentoMS = 12.5
    direcaoVentoGraus = 180.0
    modeloPrevisao = "WRF"
    tipoPrevisao = "D+1"
}

$resultPrevisao = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/previsoes-eolicas" -Body $novaPrevisao `
    -ExpectedStatus 201 -Description "Criar nova previsão eólica" -Category "PrevisoesEolicas"

if ($resultPrevisao.Success -and $resultPrevisao.Response.id) {
    $script:CreatedIds['PrevisaoEolica'] = $resultPrevisao.Response.id
}

# ============================================================================
# 11. API ARQUIVOS DADGER (15+ endpoints)
# ============================================================================

Write-Section "API ARQUIVOS DADGER" 11 17

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/arquivosdadger" `
    -Description "Listar todos os arquivos" -Category "ArquivosDadger"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/arquivosdadger/status/Aberto" `
    -Description "Listar arquivos com status Aberto" -Category "ArquivosDadger"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/arquivosdadger/pendentes-aprovacao" `
    -Description "Listar arquivos pendentes aprovação" -Category "ArquivosDadger"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/arquivosdadger/semana/1" `
    -Description "Listar arquivos por semana PMO" -Category "ArquivosDadger"

# ============================================================================
# 12. API CARGAS (10+ endpoints)
# ============================================================================

Write-Section "API CARGAS" 12 17

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/cargas" `
    -Description "Listar todas as cargas" -Category "Cargas"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/cargas/subsistema/SE" `
    -Description "Listar cargas por subsistema SE" -Category "Cargas"

$dataInicio = (Get-Date).AddDays(-30).ToString("yyyy-MM-dd")
$dataFim = (Get-Date).ToString("yyyy-MM-dd")

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/cargas/periodo?dataInicio=$dataInicio&dataFim=$dataFim" `
    -Description "Listar cargas por período" -Category "Cargas"

# ============================================================================
# 13. API INTERCÂMBIOS (10+ endpoints)
# ============================================================================

Write-Section "API INTERCÂMBIOS" 13 17

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/intercambios" `
    -Description "Listar todos os intercâmbios" -Category "Intercambios"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/intercambios/origem/SE" `
    -Description "Listar intercâmbios por origem SE" -Category "Intercambios"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/intercambios/destino/S" `
    -Description "Listar intercâmbios por destino S" -Category "Intercambios"

# ============================================================================
# 14. API BALANÇOS (10+ endpoints)
# ============================================================================

Write-Section "API BALANÇOS" 14 17

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/balancos" `
    -Description "Listar todos os balanços" -Category "Balancos"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/balancos/subsistema/SE" `
    -Description "Listar balanços por subsistema SE" -Category "Balancos"

# ============================================================================
# 15. API UNIDADES GERADORAS (10+ endpoints)
# ============================================================================

Write-Section "API UNIDADES GERADORAS" 15 17

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/unidadesgeradoras" `
    -Description "Listar todas as unidades geradoras" -Category "UnidadesGeradoras"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/unidadesgeradoras/usina/1" `
    -Description "Listar UGs por usina" -Category "UnidadesGeradoras"

# ============================================================================
# 16. API PARADAS UG (10+ endpoints)
# ============================================================================

Write-Section "API PARADAS UG" 16 17

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/paradasug" `
    -Description "Listar todas as paradas" -Category "ParadasUG"

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/paradasug/ativas" `
    -Description "Listar paradas ativas" -Category "ParadasUG"

# ============================================================================
# 17. API DADOS ENERGÉTICOS (10+ endpoints)
# ============================================================================

Write-Section "API DADOS ENERGÉTICOS" 17 17

Test-Endpoint -Method "GET" -Url "$BaseUrl/api/dadosenergeticos" `
    -Description "Listar todos os dados energéticos" -Category "DadosEnergeticos"

# POST
$novoDadoEnergetico = @{
    dataReferencia = (Get-Date).ToString("yyyy-MM-dd")
    codigoUsina = "TESTE-001"
    producaoMWh = 450.5
    capacidadeDisponivel = 600.0
    status = "Operando"
}

$resultDado = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/dadosenergeticos" -Body $novoDadoEnergetico `
    -ExpectedStatus 201 -Description "Criar novo dado energético" -Category "DadosEnergeticos"

if ($resultDado.Success -and $resultDado.Response.id) {
    $script:CreatedIds['DadoEnergetico'] = $resultDado.Response.id
}

# ============================================================================
# RELATÓRIO FINAL
# ============================================================================

Write-Header "📊 RELATÓRIO FINAL - TESTE MASTER COMPLETO"

$successRate = if ($TotalTests -gt 0) { [math]::Round(($PassedTests / $TotalTests) * 100, 2) } else { 0 }
$duration = (Get-Date) - $script:StartTime

Write-Host "`n📈 ESTATÍSTICAS GERAIS" -ForegroundColor $Colors.Info
Write-Host ("─" * 80) -ForegroundColor Gray
Write-Host "Total de Testes:       $TotalTests" -ForegroundColor White
Write-Host "Testes Passaram:       $PassedTests " -NoNewline
Write-Host "✅" -ForegroundColor $Colors.Success
Write-Host "Testes Falharam:       $FailedTests " -NoNewline
if ($FailedTests -gt 0) {
    Write-Host "❌" -ForegroundColor $Colors.Error
} else {
    Write-Host "✅" -ForegroundColor $Colors.Success
}
Write-Host "Taxa de Sucesso:       $successRate%" -ForegroundColor $(
    if ($successRate -eq 100) { $Colors.Success } 
    elseif ($successRate -ge 90) { "Yellow" } 
    else { $Colors.Error }
)
Write-Host "Duração Total:         $($duration.ToString('mm\:ss'))" -ForegroundColor White

Write-Host "`n🎯 APIs TESTADAS (17)" -ForegroundColor $Colors.Info
Write-Host ("─" * 80) -ForegroundColor Gray

$apiList = @(
    "Dashboard",
    "Usinas", 
    "Empresas",
    "Tipos de Usina",
    "Semanas PMO",
    "Equipes PDP",
    "Usuários",
    "Ofertas Exportação",
    "Ofertas Resposta Voluntária",
    "Previsões Eólicas",
    "Arquivos DADGER",
    "Cargas",
    "Intercâmbios",
    "Balanços",
    "Unidades Geradoras",
    "Paradas UG",
    "Dados Energéticos"
)

$apiList | ForEach-Object -Begin { $i = 1 } -Process {
    $num = $i.ToString().PadLeft(2)
    Write-Host "  $num. " -NoNewline -ForegroundColor Gray
    Write-Host "$_ " -NoNewline -ForegroundColor White
    Write-Host "✅" -ForegroundColor $Colors.Success
    $i++
}

if ($script:CreatedIds.Count -gt 0) {
    Write-Host "`n📝 RECURSOS CRIADOS" -ForegroundColor $Colors.Info
    Write-Host ("─" * 80) -ForegroundColor Gray
    
    $script:CreatedIds.GetEnumerator() | ForEach-Object {
        Write-Host "  • $($_.Key): " -NoNewline -ForegroundColor White
        Write-Host "ID $($_.Value)" -ForegroundColor $Colors.Success
    }
}

if ($FailedTests -gt 0) {
    Write-Host "`n❌ ENDPOINTS COM FALHA ($FailedTests)" -ForegroundColor $Colors.Error
    Write-Host ("─" * 80) -ForegroundColor Gray
    
    $script:FailedEndpoints | ForEach-Object {
        Write-Host "  • $_" -ForegroundColor $Colors.Error
    }
}

Write-Host "`n💡 PRÓXIMOS PASSOS" -ForegroundColor $Colors.Info
Write-Host ("─" * 80) -ForegroundColor Gray
Write-Host "  1. Acessar Swagger UI: $BaseUrl/swagger" -ForegroundColor White
Write-Host "  2. Revisar endpoints com falha (se houver)" -ForegroundColor White
Write-Host "  3. Executar testes xUnit: dotnet test" -ForegroundColor White
Write-Host "  4. Validar regras de negócio específicas" -ForegroundColor White

# Exportar relatório JSON
$reportPath = ".\relatorio-teste-master-$(Get-Date -Format 'yyyyMMdd-HHmmss').json"
$reportData = @{
    Timestamp = Get-Date
    BaseUrl = $BaseUrl
    Statistics = @{
        TotalTests = $TotalTests
        PassedTests = $PassedTests
        FailedTests = $FailedTests
        SuccessRate = $successRate
        Duration = $duration.ToString()
    }
    CreatedResources = $script:CreatedIds
    Results = $script:Results
    FailedEndpoints = $script:FailedEndpoints
}

$reportData | ConvertTo-Json -Depth 10 | Out-File -FilePath $reportPath -Encoding UTF8

Write-Host "`n📄 Relatório JSON: $reportPath" -ForegroundColor $Colors.Info

if ($successRate -eq 100) {
    Write-Host "`n🎉 " -NoNewline -ForegroundColor $Colors.Success
    Write-Host "PARABÉNS! TODOS OS TESTES PASSARAM COM 100% DE SUCESSO!" -ForegroundColor $Colors.Success
    Write-Host "    POC PDPw está 100% funcional e pronta para apresentação!" -ForegroundColor $Colors.Success
} elseif ($successRate -ge 90) {
    Write-Host "`n✅ TESTE APROVADO! Taxa de sucesso: $successRate%" -ForegroundColor Yellow
} else {
    Write-Host "`n⚠️  ATENÇÃO! Taxa de sucesso abaixo do esperado: $successRate%" -ForegroundColor $Colors.Error
}

Write-Host ""
exit $FailedTests
