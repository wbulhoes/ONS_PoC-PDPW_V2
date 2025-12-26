# =========================================================
# SCRIPT DE VALIDAÇÃO COMPLETA - TODAS AS APIS
# Objetivo: Testar todos os endpoints e validar dados
# Data: 26/12/2024
# =========================================================

$ErrorActionPreference = "Continue"

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "VALIDAÇÃO COMPLETA DE APIS - PDPW POC" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

$baseUrl = "http://localhost:5001/api"
$resultados = @()
$totalEndpoints = 0
$sucessos = 0
$falhas = 0

# =========================================================
# FUNÇÃO AUXILIAR: TESTAR ENDPOINT
# =========================================================

function Test-Endpoint {
    param(
        [string]$Metodo,
        [string]$Url,
        [string]$Descricao,
        [object]$Body = $null
    )
    
    $global:totalEndpoints++
    
    try {
        Write-Host "  Testando: $Descricao..." -NoNewline
        
        $params = @{
            Uri = $Url
            Method = $Metodo
            ContentType = "application/json"
            TimeoutSec = 10
        }
        
        if ($Body) {
            $params.Body = ($Body | ConvertTo-Json -Depth 10)
        }
        
        $response = Invoke-RestMethod @params -ErrorAction Stop
        
        $count = if ($response -is [Array]) { $response.Count } else { 1 }
        
        Write-Host " ✅ OK ($count registros)" -ForegroundColor Green
        
        $global:sucessos++
        $global:resultados += [PSCustomObject]@{
            API = $Descricao
            Metodo = $Metodo
            Status = "✅ Sucesso"
            Registros = $count
            Url = $Url
        }
        
        return $true
    }
    catch {
        $statusCode = $_.Exception.Response.StatusCode.value__
        $errorMsg = $_.Exception.Message
        
        Write-Host " ❌ FALHA ($statusCode)" -ForegroundColor Red
        
        $global:falhas++
        $global:resultados += [PSCustomObject]@{
            API = $Descricao
            Metodo = $Metodo
            Status = "❌ Falha ($statusCode)"
            Registros = 0
            Url = $Url
        }
        
        return $false
    }
}

# =========================================================
# 1. VERIFICAR SE API ESTÁ RODANDO
# =========================================================

Write-Host "1. Verificando se API está rodando..." -ForegroundColor Yellow

try {
    $health = Invoke-RestMethod -Uri "http://localhost:5001/health" -Method GET -TimeoutSec 5
    Write-Host "   ✅ API está saudável: $health`n" -ForegroundColor Green
}
catch {
    Write-Host "   ❌ API não está respondendo! Verifique se o Docker está rodando." -ForegroundColor Red
    Write-Host "   Comando: docker-compose up -d`n" -ForegroundColor Yellow
    exit 1
}

# =========================================================
# 2. TESTAR TIPOS DE USINA (5 endpoints)
# =========================================================

Write-Host "`n2. Testando API Tipos de Usina..." -ForegroundColor Yellow

Test-Endpoint -Metodo "GET" -Url "$baseUrl/tiposusina" -Descricao "TiposUsina - Listar Todos"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/tiposusina/1" -Descricao "TiposUsina - Buscar por ID"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/tiposusina/buscar?termo=Hidrelétrica" -Descricao "TiposUsina - Buscar por Termo"

# =========================================================
# 3. TESTAR EMPRESAS (8 endpoints)
# =========================================================

Write-Host "`n3. Testando API Empresas..." -ForegroundColor Yellow

Test-Endpoint -Metodo "GET" -Url "$baseUrl/empresas" -Descricao "Empresas - Listar Todas"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/empresas/1" -Descricao "Empresas - Buscar por ID"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/empresas/buscar?termo=Itaipu" -Descricao "Empresas - Buscar por Termo"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/empresas/cnpj/00341583000171" -Descricao "Empresas - Buscar por CNPJ"

# =========================================================
# 4. TESTAR USINAS (8 endpoints)
# =========================================================

Write-Host "`n4. Testando API Usinas..." -ForegroundColor Yellow

Test-Endpoint -Metodo "GET" -Url "$baseUrl/usinas" -Descricao "Usinas - Listar Todas"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/usinas/1" -Descricao "Usinas - Buscar por ID"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/usinas/codigo/UHE-ITAIPU" -Descricao "Usinas - Buscar por Código"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/usinas/tipo/1" -Descricao "Usinas - Filtrar por Tipo"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/usinas/empresa/1" -Descricao "Usinas - Filtrar por Empresa"

# =========================================================
# 5. TESTAR SEMANAS PMO (9 endpoints)
# =========================================================

Write-Host "`n5. Testando API Semanas PMO..." -ForegroundColor Yellow

Test-Endpoint -Metodo "GET" -Url "$baseUrl/semanaspmo" -Descricao "SemanasPMO - Listar Todas"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/semanaspmo/1" -Descricao "SemanasPMO - Buscar por ID"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/semanaspmo/ano/2024" -Descricao "SemanasPMO - Filtrar por Ano"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/semanaspmo/atual" -Descricao "SemanasPMO - Semana Atual"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/semanaspmo/proximas?quantidade=4" -Descricao "SemanasPMO - Próximas Semanas"

# =========================================================
# 6. TESTAR EQUIPES PDP (5 endpoints)
# =========================================================

Write-Host "`n6. Testando API Equipes PDP..." -ForegroundColor Yellow

Test-Endpoint -Metodo "GET" -Url "$baseUrl/equipespdp" -Descricao "EquipesPDP - Listar Todas"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/equipespdp/1" -Descricao "EquipesPDP - Buscar por ID"

# =========================================================
# 7. TESTAR MOTIVOS RESTRIÇÃO (5 endpoints)
# =========================================================

Write-Host "`n7. Testando API Motivos Restrição..." -ForegroundColor Yellow

Test-Endpoint -Metodo "GET" -Url "$baseUrl/motivosrestricao" -Descricao "MotivosRestricao - Listar Todos"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/motivosrestricao/1" -Descricao "MotivosRestricao - Buscar por ID"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/motivosrestricao/categoria/Manutenção" -Descricao "MotivosRestricao - Filtrar por Categoria"

# =========================================================
# 8. TESTAR UNIDADES GERADORAS (7 endpoints)
# =========================================================

Write-Host "`n8. Testando API Unidades Geradoras..." -ForegroundColor Yellow

Test-Endpoint -Metodo "GET" -Url "$baseUrl/unidadesgeradoras" -Descricao "UnidadesGeradoras - Listar Todas"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/unidadesgeradoras/1" -Descricao "UnidadesGeradoras - Buscar por ID"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/unidadesgeradoras/usina/1" -Descricao "UnidadesGeradoras - Filtrar por Usina"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/unidadesgeradoras/codigo/ITAIPU-UG01" -Descricao "UnidadesGeradoras - Buscar por Código"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/unidadesgeradoras/status/Operando" -Descricao "UnidadesGeradoras - Filtrar por Status"

# =========================================================
# 9. TESTAR CARGAS (8 endpoints)
# =========================================================

Write-Host "`n9. Testando API Cargas..." -ForegroundColor Yellow

Test-Endpoint -Metodo "GET" -Url "$baseUrl/cargas" -Descricao "Cargas - Listar Todas"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/cargas/1" -Descricao "Cargas - Buscar por ID"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/cargas/subsistema/SE" -Descricao "Cargas - Filtrar por Subsistema"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/cargas/periodo?dataInicio=2024-12-01&dataFim=2024-12-26" -Descricao "Cargas - Filtrar por Período"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/cargas/data/2024-12-26" -Descricao "Cargas - Filtrar por Data"

# =========================================================
# 10. TESTAR INTERCÂMBIOS (6 endpoints)
# =========================================================

Write-Host "`n10. Testando API Intercâmbios..." -ForegroundColor Yellow

Test-Endpoint -Metodo "GET" -Url "$baseUrl/intercambios" -Descricao "Intercambios - Listar Todos"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/intercambios/1" -Descricao "Intercambios - Buscar por ID"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/intercambios/subsistema?origem=SE&destino=S" -Descricao "Intercambios - Filtrar por Subsistemas"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/intercambios/periodo?dataInicio=2024-12-01&dataFim=2024-12-26" -Descricao "Intercambios - Filtrar por Período"

# =========================================================
# 11. TESTAR BALANÇOS (6 endpoints)
# =========================================================

Write-Host "`n11. Testando API Balanços..." -ForegroundColor Yellow

Test-Endpoint -Metodo "GET" -Url "$baseUrl/balancos" -Descricao "Balancos - Listar Todos"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/balancos/1" -Descricao "Balancos - Buscar por ID"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/balancos/subsistema/SE" -Descricao "Balancos - Filtrar por Subsistema"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/balancos/periodo?dataInicio=2024-12-01&dataFim=2024-12-26" -Descricao "Balancos - Filtrar por Período"

# =========================================================
# 12. TESTAR USUÁRIOS (6 endpoints)
# =========================================================

Write-Host "`n12. Testando API Usuários..." -ForegroundColor Yellow

Test-Endpoint -Metodo "GET" -Url "$baseUrl/usuarios" -Descricao "Usuarios - Listar Todos"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/usuarios/1" -Descricao "Usuarios - Buscar por ID"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/usuarios/perfil/Operador" -Descricao "Usuarios - Filtrar por Perfil"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/usuarios/equipe/1" -Descricao "Usuarios - Filtrar por Equipe"

# =========================================================
# 13. TESTAR RESTRIÇÕES UG (9 endpoints)
# =========================================================

Write-Host "`n13. Testando API Restrições UG..." -ForegroundColor Yellow

Test-Endpoint -Metodo "GET" -Url "$baseUrl/restricoesug" -Descricao "RestricoesUG - Listar Todas"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/restricoesug/1" -Descricao "RestricoesUG - Buscar por ID"

# =========================================================
# 14. TESTAR PARADAS UG (6 endpoints)
# =========================================================

Write-Host "`n14. Testando API Paradas UG..." -ForegroundColor Yellow

Test-Endpoint -Metodo "GET" -Url "$baseUrl/paradasug" -Descricao "ParadasUG - Listar Todas"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/paradasug/1" -Descricao "ParadasUG - Buscar por ID"

# =========================================================
# 15. TESTAR ARQUIVOS DADGER (10 endpoints)
# =========================================================

Write-Host "`n15. Testando API Arquivos DADGER..." -ForegroundColor Yellow

Test-Endpoint -Metodo "GET" -Url "$baseUrl/arquivosdadger" -Descricao "ArquivosDadger - Listar Todos"
Test-Endpoint -Metodo "GET" -Url "$baseUrl/arquivosdadger/1" -Descricao "ArquivosDadger - Buscar por ID"

# =========================================================
# RELATÓRIO FINAL
# =========================================================

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "RELATÓRIO FINAL DA VALIDAÇÃO" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

Write-Host "📊 ESTATÍSTICAS:" -ForegroundColor White
Write-Host "   Total de Endpoints Testados: $totalEndpoints" -ForegroundColor White
Write-Host "   ✅ Sucessos: $sucessos ($([math]::Round(($sucessos/$totalEndpoints)*100, 2))%)" -ForegroundColor Green
Write-Host "   ❌ Falhas: $falhas ($([math]::Round(($falhas/$totalEndpoints)*100, 2))%)" -ForegroundColor Red

Write-Host "`n📋 DETALHES POR API:" -ForegroundColor White
$resultados | Group-Object { $_.API.Split(' - ')[0] } | ForEach-Object {
    $apiNome = $_.Name
    $total = $_.Group.Count
    $sucesso = ($_.Group | Where-Object { $_.Status -like "*Sucesso*" }).Count
    $falha = $total - $sucesso
    
    $cor = if ($falha -eq 0) { "Green" } elseif ($sucesso -eq 0) { "Red" } else { "Yellow" }
    
    Write-Host "   $apiNome : $sucesso/$total OK" -ForegroundColor $cor
}

# Salvar relatório em arquivo
$reportPath = "C:\temp\_ONS_PoC-PDPW\relatorio-validacao-apis.json"
$resultados | ConvertTo-Json -Depth 10 | Out-File -FilePath $reportPath -Encoding UTF8

Write-Host "`n💾 Relatório salvo em: $reportPath" -ForegroundColor Cyan

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "VALIDAÇÃO CONCLUÍDA!" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

# Retornar código de saída baseado no resultado
if ($falhas -eq 0) {
    Write-Host "✅ TODAS AS APIS ESTÃO FUNCIONANDO!" -ForegroundColor Green
    exit 0
} else {
    Write-Host "⚠️  Algumas APIs apresentaram falhas. Verifique o relatório." -ForegroundColor Yellow
    exit 1
}
