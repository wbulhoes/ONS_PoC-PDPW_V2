# 🧪 TESTE COMPLETO CRUD - TODOS OS ENDPOINTS
# Simulação de operações manuais via Swagger
# Data: 27/12/2024

$BaseUrl = "http://localhost:5001"
$ErrorActionPreference = "Continue"

Write-Host "`n" -NoNewline
Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║   TESTE COMPLETO CRUD - VALIDAÇÃO DOCKER + SWAGGER           ║" -ForegroundColor Cyan
Write-Host "║   Simulando operações manuais em todos os endpoints          ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host "`n"

$totalTests = 0
$passedTests = 0
$failedTests = 0
$results = @()

function Test-Endpoint {
    param(
        [string]$Method,
        [string]$Url,
        [object]$Body = $null,
        [string]$Description,
        [int]$ExpectedStatus = 200
    )
    
    $global:totalTests++
    
    try {
        $headers = @{ "Content-Type" = "application/json" }
        
        $response = if ($Body) {
            Invoke-RestMethod -Uri $Url -Method $Method -Body ($Body | ConvertTo-Json -Depth 10) -Headers $headers
        } else {
            Invoke-RestMethod -Uri $Url -Method $Method -Headers $headers
        }
        
        Write-Host "  ✅ $Description" -ForegroundColor Green
        $global:passedTests++
        
        $global:results += [PSCustomObject]@{
            Method = $Method
            Description = $Description
            Status = "PASS"
            Response = $response
        }
        
        return $response
    }
    catch {
        $statusCode = $_.Exception.Response.StatusCode.value__
        
        if ($statusCode -eq $ExpectedStatus) {
            Write-Host "  ✅ $Description (Erro esperado: $ExpectedStatus)" -ForegroundColor Green
            $global:passedTests++
            
            $global:results += [PSCustomObject]@{
                Method = $Method
                Description = $Description
                Status = "PASS (Expected Error)"
            }
        }
        else {
            Write-Host "  ❌ $Description - Erro: $($_.Exception.Message)" -ForegroundColor Red
            $global:failedTests++
            
            $global:results += [PSCustomObject]@{
                Method = $Method
                Description = $Description
                Status = "FAIL"
                Error = $_.Exception.Message
            }
        }
        
        return $null
    }
}

# ============================================================================
# 1. TIPOS DE USINA
# ============================================================================

Write-Host "`n📋 [1/14] Testando API TiposUsina..." -ForegroundColor Yellow

# CREATE
$novoTipo = @{
    nome = "Hidrogênio Verde"
    descricao = "Usina que gera energia através de hidrogênio verde"
    fonteEnergia = "Hidrogênio"
}
$tipoCreated = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/tiposusina" -Body $novoTipo -Description "CREATE: Criar novo tipo de usina"

# READ
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/tiposusina" -Description "READ: Listar todos os tipos"

if ($tipoCreated) {
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/tiposusina/$($tipoCreated.id)" -Description "READ: Buscar tipo criado por ID"
}

# SEARCH
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/tiposusina/buscar?termo=Hidro" -Description "SEARCH: Buscar tipos por termo"

# UPDATE
if ($tipoCreated) {
    $tipoUpdate = @{
        nome = "Hidrogênio Verde - Atualizado"
        descricao = "Usina moderna de hidrogênio verde com alta eficiência"
        fonteEnergia = "Hidrogênio Renovável"
    }
    Test-Endpoint -Method "PUT" -Url "$BaseUrl/api/tiposusina/$($tipoCreated.id)" -Body $tipoUpdate -Description "UPDATE: Atualizar tipo criado"
}

# DELETE
if ($tipoCreated) {
    Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/tiposusina/$($tipoCreated.id)" -Description "DELETE: Remover tipo criado"
}

# ============================================================================
# 2. EMPRESAS
# ============================================================================

Write-Host "`n📋 [2/14] Testando API Empresas..." -ForegroundColor Yellow

# CREATE
$novaEmpresa = @{
    nome = "Energia Verde Brasil S.A."
    cnpj = "12345678000199"
    telefone = "(11) 9999-8888"
    email = "contato@energiaverde.com.br"
    endereco = "Av. Paulista, 1000 - São Paulo, SP"
}
$empresaCreated = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/empresas" -Body $novaEmpresa -Description "CREATE: Criar nova empresa"

# READ
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/empresas" -Description "READ: Listar todas as empresas"

if ($empresaCreated) {
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/empresas/$($empresaCreated.id)" -Description "READ: Buscar empresa criada por ID"
}

# SEARCH
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/empresas/buscar?termo=Verde" -Description "SEARCH: Buscar empresas por termo"
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/empresas/cnpj/12345678000199" -Description "SEARCH: Buscar por CNPJ"

# UPDATE
if ($empresaCreated) {
    $empresaUpdate = @{
        nome = "Energia Verde Brasil Ltda - ATUALIZADA"
        cnpj = "12345678000199"
        telefone = "(11) 8888-7777"
        email = "novoemail@energiaverde.com.br"
        endereco = "Av. Paulista, 2000 - São Paulo, SP"
    }
    Test-Endpoint -Method "PUT" -Url "$BaseUrl/api/empresas/$($empresaCreated.id)" -Body $empresaUpdate -Description "UPDATE: Atualizar empresa criada"
}

# DELETE
if ($empresaCreated) {
    Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/empresas/$($empresaCreated.id)" -Description "DELETE: Remover empresa criada"
}

# ============================================================================
# 3. USINAS
# ============================================================================

Write-Host "`n📋 [3/14] Testando API Usinas..." -ForegroundColor Yellow

# CREATE
$novaUsina = @{
    codigo = "UHE-TESTE-001"
    nome = "Usina Hidrelétrica Teste Automático"
    tipoUsinaId = 1  # Hidrelétrica
    empresaId = 1    # Itaipu
    capacidadeInstalada = 500.00
    localizacao = "Rio Grande do Sul"
    dataOperacao = "2024-12-27T00:00:00"
}
$usinaCreated = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/usinas" -Body $novaUsina -Description "CREATE: Criar nova usina"

# READ
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usinas" -Description "READ: Listar todas as usinas"

if ($usinaCreated) {
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usinas/$($usinaCreated.id)" -Description "READ: Buscar usina criada por ID"
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usinas/codigo/UHE-TESTE-001" -Description "READ: Buscar por código"
}

# FILTER
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usinas/tipo/1" -Description "FILTER: Filtrar por tipo"
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usinas/empresa/1" -Description "FILTER: Filtrar por empresa"

# UPDATE
if ($usinaCreated) {
    $usinaUpdate = @{
        codigo = "UHE-TESTE-001"
        nome = "Usina Hidrelétrica Teste ATUALIZADA"
        tipoUsinaId = 1
        empresaId = 1
        capacidadeInstalada = 750.00
        localizacao = "Rio Grande do Sul - Região Sul"
        dataOperacao = "2024-12-27T00:00:00"
    }
    Test-Endpoint -Method "PUT" -Url "$BaseUrl/api/usinas/$($usinaCreated.id)" -Body $usinaUpdate -Description "UPDATE: Atualizar usina criada"
}

# DELETE
if ($usinaCreated) {
    Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/usinas/$($usinaCreated.id)" -Description "DELETE: Remover usina criada"
}

# ============================================================================
# 4. SEMANAS PMO
# ============================================================================

Write-Host "`n📋 [4/14] Testando API SemanasPMO..." -ForegroundColor Yellow

# CREATE
$novaSemana = @{
    numero = 1
    ano = 2027
    dataInicio = "2027-01-02T00:00:00"
    dataFim = "2027-01-08T00:00:00"
    observacoes = "Semana de teste automático"
}
$semanaCreated = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/semanaspmo" -Body $novaSemana -Description "CREATE: Criar nova semana PMO"

# READ
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/semanaspmo" -Description "READ: Listar todas as semanas"
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/semanaspmo/atual" -Description "READ: Semana atual"
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/semanaspmo/proximas?quantidade=4" -Description "READ: Próximas 4 semanas"

if ($semanaCreated) {
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/semanaspmo/$($semanaCreated.id)" -Description "READ: Buscar semana criada por ID"
}

# FILTER
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/semanaspmo/ano/2025" -Description "FILTER: Filtrar por ano 2025"

# UPDATE
if ($semanaCreated) {
    $semanaUpdate = @{
        numero = 1
        ano = 2027
        dataInicio = "2027-01-02T00:00:00"
        dataFim = "2027-01-08T00:00:00"
        observacoes = "Semana ATUALIZADA - Teste automático"
    }
    Test-Endpoint -Method "PUT" -Url "$BaseUrl/api/semanaspmo/$($semanaCreated.id)" -Body $semanaUpdate -Description "UPDATE: Atualizar semana criada"
}

# DELETE
if ($semanaCreated) {
    Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/semanaspmo/$($semanaCreated.id)" -Description "DELETE: Remover semana criada"
}

# ============================================================================
# 5. EQUIPES PDP
# ============================================================================

Write-Host "`n📋 [5/14] Testando API EquipesPDP..." -ForegroundColor Yellow

# CREATE
$novaEquipe = @{
    nome = "Equipe de Testes Automatizados"
    descricao = "Equipe responsável por validação automática"
    coordenador = "João da Silva Teste"
    email = "teste@ons.org.br"
    telefone = "(21) 9999-9999"
}
$equipeCreated = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/equipespdp" -Body $novaEquipe -Description "CREATE: Criar nova equipe"

# READ
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/equipespdp" -Description "READ: Listar todas as equipes"

if ($equipeCreated) {
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/equipespdp/$($equipeCreated.id)" -Description "READ: Buscar equipe criada por ID"
}

# UPDATE
if ($equipeCreated) {
    $equipeUpdate = @{
        nome = "Equipe de Testes ATUALIZADA"
        descricao = "Equipe de validação automática e testes integrados"
        coordenador = "João da Silva Teste Atualizado"
        email = "teste.novo@ons.org.br"
        telefone = "(21) 8888-8888"
    }
    Test-Endpoint -Method "PUT" -Url "$BaseUrl/api/equipespdp/$($equipeCreated.id)" -Body $equipeUpdate -Description "UPDATE: Atualizar equipe criada"
}

# DELETE
if ($equipeCreated) {
    Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/equipespdp/$($equipeCreated.id)" -Description "DELETE: Remover equipe criada"
}

# ============================================================================
# 6. MOTIVOS RESTRIÇÃO
# ============================================================================

Write-Host "`n📋 [6/14] Testando API MotivosRestricao..." -ForegroundColor Yellow

# CREATE
$novoMotivo = @{
    nome = "Teste de Automação"
    descricao = "Motivo criado durante teste automatizado"
    categoria = "Teste"
}
$motivoCreated = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/motivosrestricao" -Body $novoMotivo -Description "CREATE: Criar novo motivo"

# READ
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/motivosrestricao" -Description "READ: Listar todos os motivos"

if ($motivoCreated) {
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/motivosrestricao/$($motivoCreated.id)" -Description "READ: Buscar motivo criado por ID"
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/motivosrestricao/categoria/Teste" -Description "FILTER: Filtrar por categoria"
}

# UPDATE
if ($motivoCreated) {
    $motivoUpdate = @{
        nome = "Teste de Automação ATUALIZADO"
        descricao = "Motivo atualizado durante teste automatizado"
        categoria = "Teste Atualizado"
    }
    Test-Endpoint -Method "PUT" -Url "$BaseUrl/api/motivosrestricao/$($motivoCreated.id)" -Body $motivoUpdate -Description "UPDATE: Atualizar motivo criado"
}

# DELETE
if ($motivoCreated) {
    Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/motivosrestricao/$($motivoCreated.id)" -Description "DELETE: Remover motivo criado"
}

# ============================================================================
# 7. UNIDADES GERADORAS
# ============================================================================

Write-Host "`n📋 [7/14] Testando API UnidadesGeradoras..." -ForegroundColor Yellow

# CREATE
$novaUG = @{
    codigo = "UG-TESTE-001"
    nome = "Unidade Geradora Teste Automático"
    usinaId = 1  # Itaipu
    potenciaNominal = 100.00
    potenciaMinima = 50.00
    dataComissionamento = "2024-12-27T00:00:00"
    status = "Teste"
}
$ugCreated = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/unidadesgeradoras" -Body $novaUG -Description "CREATE: Criar nova unidade geradora"

# READ
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/unidadesgeradoras" -Description "READ: Listar todas as UGs"

if ($ugCreated) {
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/unidadesgeradoras/$($ugCreated.id)" -Description "READ: Buscar UG criada por ID"
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/unidadesgeradoras/codigo/UG-TESTE-001" -Description "READ: Buscar por código"
}

# FILTER
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/unidadesgeradoras/usina/1" -Description "FILTER: Filtrar por usina"
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/unidadesgeradoras/status/Operando" -Description "FILTER: Filtrar por status"

# UPDATE
if ($ugCreated) {
    $ugUpdate = @{
        codigo = "UG-TESTE-001"
        nome = "Unidade Geradora Teste ATUALIZADA"
        usinaId = 1
        potenciaNominal = 150.00
        potenciaMinima = 75.00
        dataComissionamento = "2024-12-27T00:00:00"
        status = "Operando"
    }
    Test-Endpoint -Method "PUT" -Url "$BaseUrl/api/unidadesgeradoras/$($ugCreated.id)" -Body $ugUpdate -Description "UPDATE: Atualizar UG criada"
}

# DELETE
if ($ugCreated) {
    Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/unidadesgeradoras/$($ugCreated.id)" -Description "DELETE: Remover UG criada"
}

# ============================================================================
# 8. USUÁRIOS
# ============================================================================

Write-Host "`n📋 [8/14] Testando API Usuarios..." -ForegroundColor Yellow

# CREATE
$novoUsuario = @{
    nome = "Teste Automatizado Silva"
    email = "teste.auto@ons.org.br"
    telefone = "(21) 7777-7777"
    perfil = "Teste"
    equipePDPId = 1
}
$usuarioCreated = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/usuarios" -Body $novoUsuario -Description "CREATE: Criar novo usuário"

# READ
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usuarios" -Description "READ: Listar todos os usuários"

if ($usuarioCreated) {
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usuarios/$($usuarioCreated.id)" -Description "READ: Buscar usuário criado por ID"
}

# FILTER
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usuarios/perfil/Operador" -Description "FILTER: Filtrar por perfil"
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/usuarios/equipe/1" -Description "FILTER: Filtrar por equipe"

# UPDATE
if ($usuarioCreated) {
    $usuarioUpdate = @{
        nome = "Teste Automatizado Silva ATUALIZADO"
        email = "teste.novo@ons.org.br"
        telefone = "(21) 6666-6666"
        perfil = "Analista"
        equipePDPId = 2
    }
    Test-Endpoint -Method "PUT" -Url "$BaseUrl/api/usuarios/$($usuarioCreated.id)" -Body $usuarioUpdate -Description "UPDATE: Atualizar usuário criado"
}

# DELETE
if ($usuarioCreated) {
    Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/usuarios/$($usuarioCreated.id)" -Description "DELETE: Remover usuário criado"
}

# ============================================================================
# 9. CARGAS
# ============================================================================

Write-Host "`n📋 [9/14] Testando API Cargas..." -ForegroundColor Yellow

# CREATE
$novaCarga = @{
    dataReferencia = "2024-12-27T00:00:00"
    subsistemaId = "TESTE"
    cargaMWmed = 1000.00
    cargaVerificada = 980.00
    previsaoCarga = 1020.00
}
$cargaCreated = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/cargas" -Body $novaCarga -Description "CREATE: Criar nova carga"

# READ
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/cargas" -Description "READ: Listar todas as cargas"

if ($cargaCreated) {
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/cargas/$($cargaCreated.id)" -Description "READ: Buscar carga criada por ID"
}

# FILTER
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/cargas/subsistema/SE" -Description "FILTER: Filtrar por subsistema"

# UPDATE
if ($cargaCreated) {
    $cargaUpdate = @{
        dataReferencia = "2024-12-27T00:00:00"
        subsistemaId = "TESTE"
        cargaMWmed = 1100.00
        cargaVerificada = 1080.00
        previsaoCarga = 1120.00
    }
    Test-Endpoint -Method "PUT" -Url "$BaseUrl/api/cargas/$($cargaCreated.id)" -Body $cargaUpdate -Description "UPDATE: Atualizar carga criada"
}

# DELETE
if ($cargaCreated) {
    Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/cargas/$($cargaCreated.id)" -Description "DELETE: Remover carga criada"
}

# ============================================================================
# 10. INTERCÂMBIOS
# ============================================================================

Write-Host "`n📋 [10/14] Testando API Intercambios..." -ForegroundColor Yellow

# CREATE
$novoIntercambio = @{
    dataReferencia = "2024-12-27T00:00:00"
    subsistemaOrigem = "TESTE"
    subsistemaDestino = "SE"
    energiaIntercambiada = 250.00
}
$intercambioCreated = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/intercambios" -Body $novoIntercambio -Description "CREATE: Criar novo intercâmbio"

# READ
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/intercambios" -Description "READ: Listar todos os intercâmbios"

if ($intercambioCreated) {
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/intercambios/$($intercambioCreated.id)" -Description "READ: Buscar intercâmbio criado por ID"
}

# FILTER
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/intercambios/subsistema?origem=SE&destino=S" -Description "FILTER: Filtrar por subsistemas"

# UPDATE
if ($intercambioCreated) {
    $intercambioUpdate = @{
        dataReferencia = "2024-12-27T00:00:00"
        subsistemaOrigem = "TESTE"
        subsistemaDestino = "NE"
        energiaIntercambiada = 350.00
    }
    Test-Endpoint -Method "PUT" -Url "$BaseUrl/api/intercambios/$($intercambioCreated.id)" -Body $intercambioUpdate -Description "UPDATE: Atualizar intercâmbio criado"
}

# DELETE
if ($intercambioCreated) {
    Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/intercambios/$($intercambioCreated.id)" -Description "DELETE: Remover intercâmbio criado"
}

# ============================================================================
# 11. BALANÇOS
# ============================================================================

Write-Host "`n📋 [11/14] Testando API Balancos..." -ForegroundColor Yellow

# CREATE
$novoBalanco = @{
    dataReferencia = "2024-12-27T00:00:00"
    subsistemaId = "TESTE"
    geracao = 5000.00
    carga = 4800.00
    intercambio = 200.00
    perdas = 100.00
    deficit = 0.00
}
$balancoCreated = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/balancos" -Body $novoBalanco -Description "CREATE: Criar novo balanço"

# READ
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/balancos" -Description "READ: Listar todos os balanços"

if ($balancoCreated) {
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/balancos/$($balancoCreated.id)" -Description "READ: Buscar balanço criado por ID"
}

# FILTER
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/balancos/subsistema/SE" -Description "FILTER: Filtrar por subsistema"

# UPDATE
if ($balancoCreated) {
    $balancoUpdate = @{
        dataReferencia = "2024-12-27T00:00:00"
        subsistemaId = "TESTE"
        geracao = 5500.00
        carga = 5200.00
        intercambio = 300.00
        perdas = 150.00
        deficit = 0.00
    }
    Test-Endpoint -Method "PUT" -Url "$BaseUrl/api/balancos/$($balancoCreated.id)" -Body $balancoUpdate -Description "UPDATE: Atualizar balanço criado"
}

# DELETE
if ($balancoCreated) {
    Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/balancos/$($balancoCreated.id)" -Description "DELETE: Remover balanço criado"
}

# ============================================================================
# 12. RESTRIÇÕES UG
# ============================================================================

Write-Host "`n📋 [12/14] Testando API RestricoesUG..." -ForegroundColor Yellow

# CREATE
$novaRestricao = @{
    unidadeGeradoraId = 1
    dataInicio = "2024-12-27T00:00:00"
    dataFim = "2024-12-31T00:00:00"
    motivoRestricaoId = 1
    potenciaRestrita = 200.00
    observacoes = "Restrição de teste automático"
}
$restricaoCreated = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/restricoesug" -Body $novaRestricao -Description "CREATE: Criar nova restrição"

# READ
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/restricoesug" -Description "READ: Listar todas as restrições"

if ($restricaoCreated) {
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/restricoesug/$($restricaoCreated.id)" -Description "READ: Buscar restrição criada por ID"
}

# UPDATE
if ($restricaoCreated) {
    $restricaoUpdate = @{
        unidadeGeradoraId = 1
        dataInicio = "2024-12-27T00:00:00"
        dataFim = "2025-01-05T00:00:00"
        motivoRestricaoId = 1
        potenciaRestrita = 250.00
        observacoes = "Restrição ATUALIZADA - teste automático"
    }
    Test-Endpoint -Method "PUT" -Url "$BaseUrl/api/restricoesug/$($restricaoCreated.id)" -Body $restricaoUpdate -Description "UPDATE: Atualizar restrição criada"
}

# DELETE
if ($restricaoCreated) {
    Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/restricoesug/$($restricaoCreated.id)" -Description "DELETE: Remover restrição criada"
}

# ============================================================================
# 13. PARADAS UG
# ============================================================================

Write-Host "`n📋 [13/14] Testando API ParadasUG..." -ForegroundColor Yellow

# CREATE
$novaParada = @{
    unidadeGeradoraId = 1
    dataInicio = "2024-12-27T00:00:00"
    dataFim = "2024-12-30T00:00:00"
    motivoParada = "Teste Automático"
    programada = $true
    observacoes = "Parada de teste automático"
}
$paradaCreated = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/paradasug" -Body $novaParada -Description "CREATE: Criar nova parada"

# READ
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/paradasug" -Description "READ: Listar todas as paradas"

if ($paradaCreated) {
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/paradasug/$($paradaCreated.id)" -Description "READ: Buscar parada criada por ID"
}

# UPDATE
if ($paradaCreated) {
    $paradaUpdate = @{
        unidadeGeradoraId = 1
        dataInicio = "2024-12-27T00:00:00"
        dataFim = "2025-01-02T00:00:00"
        motivoParada = "Teste Automático ATUALIZADO"
        programada = $true
        observacoes = "Parada ATUALIZADA - teste automático"
    }
    Test-Endpoint -Method "PUT" -Url "$BaseUrl/api/paradasug/$($paradaCreated.id)" -Body $paradaUpdate -Description "UPDATE: Atualizar parada criada"
}

# DELETE
if ($paradaCreated) {
    Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/paradasug/$($paradaCreated.id)" -Description "DELETE: Remover parada criada"
}

# ============================================================================
# 14. ARQUIVOS DADGER
# ============================================================================

Write-Host "`n📋 [14/14] Testando API ArquivosDadger..." -ForegroundColor Yellow

# CREATE
$novoArquivo = @{
    nomeArquivo = "DADGER_TESTE_AUTO.DAT"
    caminhoArquivo = "/teste/automático/DADGER_TESTE_AUTO.DAT"
    dataImportacao = "2024-12-27T14:00:00"
    semanaPMOId = 1
    observacoes = "Arquivo de teste automático"
}
$arquivoCreated = Test-Endpoint -Method "POST" -Url "$BaseUrl/api/arquivosdadger" -Body $novoArquivo -Description "CREATE: Criar novo arquivo DADGER"

# READ
Test-Endpoint -Method "GET" -Url "$BaseUrl/api/arquivosdadger" -Description "READ: Listar todos os arquivos"

if ($arquivoCreated) {
    Test-Endpoint -Method "GET" -Url "$BaseUrl/api/arquivosdadger/$($arquivoCreated.id)" -Description "READ: Buscar arquivo criado por ID"
}

# UPDATE
if ($arquivoCreated) {
    $arquivoUpdate = @{
        nomeArquivo = "DADGER_TESTE_AUTO_V2.DAT"
        caminhoArquivo = "/teste/automático/v2/DADGER_TESTE_AUTO_V2.DAT"
        dataImportacao = "2024-12-27T15:00:00"
        semanaPMOId = 1
        observacoes = "Arquivo ATUALIZADO - teste automático"
        processado = $true
    }
    Test-Endpoint -Method "PUT" -Url "$BaseUrl/api/arquivosdadger/$($arquivoCreated.id)" -Body $arquivoUpdate -Description "UPDATE: Atualizar arquivo criado"
}

# DELETE
if ($arquivoCreated) {
    Test-Endpoint -Method "DELETE" -Url "$BaseUrl/api/arquivosdadger/$($arquivoCreated.id)" -Description "DELETE: Remover arquivo criado"
}

# ============================================================================
# RELATÓRIO FINAL
# ============================================================================

Write-Host "`n" -NoNewline
Write-Host "╔════════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║                  RELATÓRIO FINAL DO TESTE                      ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host "`n"

$successRate = if ($totalTests -gt 0) { [math]::Round(($passedTests / $totalTests) * 100, 2) } else { 0 }

Write-Host "📊 ESTATÍSTICAS GERAIS:" -ForegroundColor Yellow
Write-Host "   Total de Testes:     $totalTests" -ForegroundColor White
Write-Host "   ✅ Sucessos:        $passedTests" -ForegroundColor Green
Write-Host "   ❌ Falhas:          $failedTests" -ForegroundColor $(if ($failedTests -gt 0) { "Red" } else { "Green" })
Write-Host "   📈 Taxa de Sucesso:  $successRate%" -ForegroundColor $(if ($successRate -eq 100) { "Green" } elseif ($successRate -ge 90) { "Yellow" } else { "Red" })
Write-Host "`n"

if ($failedTests -eq 0) {
    Write-Host "🎉 TODOS OS TESTES PASSARAM COM SUCESSO!" -ForegroundColor Green
    Write-Host "✅ Sistema Docker + Swagger 100% FUNCIONAL!" -ForegroundColor Green
    Write-Host "✅ CRUD completo validado em todas as 14 APIs!" -ForegroundColor Green
}
else {
    Write-Host "⚠️  ALGUNS TESTES FALHARAM" -ForegroundColor Yellow
    Write-Host "   Verifique os logs acima para detalhes" -ForegroundColor Yellow
}

Write-Host "`n"
Write-Host "📄 Relatório detalhado disponível na variável: `$results" -ForegroundColor Cyan
Write-Host "`n"

# Retornar código de saída
exit $failedTests
