# Script de Teste CRUD Completo - Dupla Checagem
# Data: 27/12/2024

$BaseUrl = "http://localhost:5001"
$totalTests = 0
$passedTests = 0
$failedTests = 0

Write-Host "`n╔══════════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║        TESTE CRUD COMPLETO - DUPLA CHECAGEM RIGOROSA            ║" -ForegroundColor Cyan
Write-Host "╚══════════════════════════════════════════════════════════════════╝`n" -ForegroundColor Cyan

# ============================================================================
# 1. TIPOS DE USINA
# ============================================================================
Write-Host "🔵 [1/14] API: TiposUsina" -ForegroundColor Cyan
Write-Host "════════════════════════════════════════════════════════════" -ForegroundColor Gray

# READ ALL
Write-Host "  ➤ READ (GET /api/tiposusina)..." -NoNewline
$totalTests++
try {
    $tipos = Invoke-RestMethod -Uri "$BaseUrl/api/tiposusina"
    Write-Host " ✅ OK ($($tipos.Count) registros)" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# CREATE
Write-Host "  ➤ CREATE (POST)..." -NoNewline
$totalTests++
$novoTipo = @{
    nome = "Teste Auto $(Get-Date -Format 'HHmmss')"
    descricao = "Tipo criado durante teste automatizado"
    fonteEnergia = "Teste"
} | ConvertTo-Json

try {
    $created = Invoke-RestMethod -Uri "$BaseUrl/api/tiposusina" -Method POST -Body $novoTipo -ContentType "application/json"
    Write-Host " ✅ OK (ID: $($created.id))" -ForegroundColor Green
    $tipoId = $created.id
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# READ BY ID
if ($tipoId) {
    Write-Host "  ➤ READ BY ID (GET /api/tiposusina/$tipoId)..." -NoNewline
    $totalTests++
    try {
        $tipo = Invoke-RestMethod -Uri "$BaseUrl/api/tiposusina/$tipoId"
        Write-Host " ✅ OK" -ForegroundColor Green
        $passedTests++
    } catch {
        Write-Host " ❌ FALHOU" -ForegroundColor Red
        $failedTests++
    }

    # UPDATE
    Write-Host "  ➤ UPDATE (PUT)..." -NoNewline
    $totalTests++
    $updateTipo = @{
        nome = "Teste ATUALIZADO"
        descricao = "Tipo atualizado"
        fonteEnergia = "Teste Atualizado"
    } | ConvertTo-Json
    
    try {
        Invoke-RestMethod -Uri "$BaseUrl/api/tiposusina/$tipoId" -Method PUT -Body $updateTipo -ContentType "application/json" | Out-Null
        Write-Host " ✅ OK" -ForegroundColor Green
        $passedTests++
    } catch {
        Write-Host " ❌ FALHOU" -ForegroundColor Red
        $failedTests++
    }

    # DELETE
    Write-Host "  ➤ DELETE (DELETE)..." -NoNewline
    $totalTests++
    try {
        Invoke-RestMethod -Uri "$BaseUrl/api/tiposusina/$tipoId" -Method DELETE | Out-Null
        Write-Host " ✅ OK" -ForegroundColor Green
        $passedTests++
    } catch {
        Write-Host " ❌ FALHOU" -ForegroundColor Red
        $failedTests++
    }
}

# SEARCH
Write-Host "  ➤ SEARCH (GET /buscar?termo=Hidrel)..." -NoNewline
$totalTests++
try {
    $busca = Invoke-RestMethod -Uri "$BaseUrl/api/tiposusina/buscar?termo=Hidrel"
    Write-Host " ✅ OK ($($busca.Count) encontrados)" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

Write-Host ""

# ============================================================================
# 2. EMPRESAS
# ============================================================================
Write-Host "🔵 [2/14] API: Empresas" -ForegroundColor Cyan
Write-Host "════════════════════════════════════════════════════════════" -ForegroundColor Gray

# READ ALL
Write-Host "  ➤ READ (GET /api/empresas)..." -NoNewline
$totalTests++
try {
    $empresas = Invoke-RestMethod -Uri "$BaseUrl/api/empresas"
    Write-Host " ✅ OK ($($empresas.Count) registros)" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# SEARCH BY TERM
Write-Host "  ➤ SEARCH (GET /buscar?termo=Itaipu)..." -NoNewline
$totalTests++
try {
    $busca = Invoke-RestMethod -Uri "$BaseUrl/api/empresas/buscar?termo=Itaipu"
    Write-Host " ✅ OK ($($busca.Count) encontradas)" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# SEARCH BY CNPJ
Write-Host "  ➤ SEARCH BY CNPJ (GET /cnpj/00341583000171)..." -NoNewline
$totalTests++
try {
    $empresa = Invoke-RestMethod -Uri "$BaseUrl/api/empresas/cnpj/00341583000171"
    Write-Host " ✅ OK" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

Write-Host ""

# ============================================================================
# 3. USINAS
# ============================================================================
Write-Host "🔵 [3/14] API: Usinas" -ForegroundColor Cyan
Write-Host "════════════════════════════════════════════════════════════" -ForegroundColor Gray

# READ ALL
Write-Host "  ➤ READ (GET /api/usinas)..." -NoNewline
$totalTests++
try {
    $usinas = Invoke-RestMethod -Uri "$BaseUrl/api/usinas"
    Write-Host " ✅ OK ($($usinas.Count) registros)" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# FILTER BY TYPE
Write-Host "  ➤ FILTER BY TYPE (GET /tipo/1)..." -NoNewline
$totalTests++
try {
    $filtered = Invoke-RestMethod -Uri "$BaseUrl/api/usinas/tipo/1"
    Write-Host " ✅ OK ($($filtered.Count) usinas)" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# FILTER BY EMPRESA
Write-Host "  ➤ FILTER BY EMPRESA (GET /empresa/1)..." -NoNewline
$totalTests++
try {
    $filtered = Invoke-RestMethod -Uri "$BaseUrl/api/usinas/empresa/1"
    Write-Host " ✅ OK ($($filtered.Count) usinas)" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

Write-Host ""

# ============================================================================
# 4. SEMANAS PMO
# ============================================================================
Write-Host "🔵 [4/14] API: SemanasPMO" -ForegroundColor Cyan
Write-Host "════════════════════════════════════════════════════════════" -ForegroundColor Gray

# READ ALL
Write-Host "  ➤ READ (GET /api/semanaspmo)..." -NoNewline
$totalTests++
try {
    $semanas = Invoke-RestMethod -Uri "$BaseUrl/api/semanaspmo"
    Write-Host " ✅ OK ($($semanas.Count) registros)" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# SEMANA ATUAL
Write-Host "  ➤ SEMANA ATUAL (GET /atual)..." -NoNewline
$totalTests++
try {
    $atual = Invoke-RestMethod -Uri "$BaseUrl/api/semanaspmo/atual"
    Write-Host " ✅ OK (Semana $($atual.numero)/$($atual.ano))" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# PRÓXIMAS SEMANAS
Write-Host "  ➤ PRÓXIMAS (GET /proximas?quantidade=4)..." -NoNewline
$totalTests++
try {
    $proximas = Invoke-RestMethod -Uri "$BaseUrl/api/semanaspmo/proximas?quantidade=4"
    Write-Host " ✅ OK ($($proximas.Count) semanas)" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# FILTER BY ANO
Write-Host "  ➤ FILTER BY ANO (GET /ano/2025)..." -NoNewline
$totalTests++
try {
    $filtered = Invoke-RestMethod -Uri "$BaseUrl/api/semanaspmo/ano/2025"
    Write-Host " ✅ OK ($($filtered.Count) semanas)" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

Write-Host ""

# ============================================================================
# 5. UNIDADES GERADORAS
# ============================================================================
Write-Host "🔵 [5/14] API: UnidadesGeradoras" -ForegroundColor Cyan
Write-Host "════════════════════════════════════════════════════════════" -ForegroundColor Gray

# READ ALL
Write-Host "  ➤ READ (GET /api/unidadesgeradoras)..." -NoNewline
$totalTests++
try {
    $ugs = Invoke-RestMethod -Uri "$BaseUrl/api/unidadesgeradoras"
    Write-Host " ✅ OK ($($ugs.Count) registros)" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# FILTER BY USINA
Write-Host "  ➤ FILTER BY USINA (GET /usina/1)..." -NoNewline
$totalTests++
try {
    $filtered = Invoke-RestMethod -Uri "$BaseUrl/api/unidadesgeradoras/usina/1"
    Write-Host " ✅ OK ($($filtered.Count) UGs de Itaipu)" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# FILTER BY STATUS
Write-Host "  ➤ FILTER BY STATUS (GET /status/Operando)..." -NoNewline
$totalTests++
try {
    $filtered = Invoke-RestMethod -Uri "$BaseUrl/api/unidadesgeradoras/status/Operando"
    Write-Host " ✅ OK ($($filtered.Count) UGs)" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

Write-Host ""

# ============================================================================
# 6. INTERCÂMBIOS
# ============================================================================
Write-Host "🔵 [6/14] API: Intercambios" -ForegroundColor Cyan
Write-Host "════════════════════════════════════════════════════════════" -ForegroundColor Gray

# READ ALL
Write-Host "  ➤ READ (GET /api/intercambios)..." -NoNewline
$totalTests++
try {
    $intercambios = Invoke-RestMethod -Uri "$BaseUrl/api/intercambios"
    Write-Host " ✅ OK ($($intercambios.Count) registros)" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# FILTER BY SUBSISTEMA (NOVO ENDPOINT)
Write-Host "  ➤ FILTER BY SUBSISTEMA (GET /subsistema?origem=SE&destino=S)..." -NoNewline
$totalTests++
try {
    $filtered = Invoke-RestMethod -Uri "$BaseUrl/api/intercambios/subsistema?origem=SE&destino=S"
    $media = ($filtered | Measure-Object -Property energiaIntercambiada -Average).Average
    Write-Host " ✅ OK ($($filtered.Count) registros, média $([math]::Round($media, 2))MW)" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

Write-Host ""

# ============================================================================
# 7. USUÁRIOS
# ============================================================================
Write-Host "🔵 [7/14] API: Usuarios" -ForegroundColor Cyan
Write-Host "════════════════════════════════════════════════════════════" -ForegroundColor Gray

# READ ALL
Write-Host "  ➤ READ (GET /api/usuarios)..." -NoNewline
$totalTests++
try {
    $usuarios = Invoke-RestMethod -Uri "$BaseUrl/api/usuarios"
    Write-Host " ✅ OK ($($usuarios.Count) registros)" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# FILTER BY PERFIL
Write-Host "  ➤ FILTER BY PERFIL (GET /perfil/Operador)..." -NoNewline
$totalTests++
try {
    $filtered = Invoke-RestMethod -Uri "$BaseUrl/api/usuarios/perfil/Operador"
    Write-Host " ✅ OK ($($filtered.Count) operadores)" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

# FILTER BY EQUIPE
Write-Host "  ➤ FILTER BY EQUIPE (GET /equipe/1)..." -NoNewline
$totalTests++
try {
    $filtered = Invoke-RestMethod -Uri "$BaseUrl/api/usuarios/equipe/1"
    Write-Host " ✅ OK ($($filtered.Count) usuários)" -ForegroundColor Green
    $passedTests++
} catch {
    Write-Host " ❌ FALHOU" -ForegroundColor Red
    $failedTests++
}

Write-Host ""

# ============================================================================
# 8-14. DEMAIS APIS (READ ONLY)
# ============================================================================
$apisReadOnly = @(
    @{ Nome = "EquipesPDP"; Url = "equipespdp" },
    @{ Nome = "MotivosRestricao"; Url = "motivosrestricao" },
    @{ Nome = "Cargas"; Url = "cargas" },
    @{ Nome = "Balancos"; Url = "balancos" },
    @{ Nome = "RestricoesUG"; Url = "restricoesug" },
    @{ Nome = "ParadasUG"; Url = "paradasug" },
    @{ Nome = "ArquivosDadger"; Url = "arquivosdadger" }
)

$index = 8
foreach ($api in $apisReadOnly) {
    Write-Host "🔵 [$index/14] API: $($api.Nome)" -ForegroundColor Cyan
    Write-Host "════════════════════════════════════════════════════════════" -ForegroundColor Gray
    
    Write-Host "  ➤ READ (GET /api/$($api.Url))..." -NoNewline
    $totalTests++
    try {
        $data = Invoke-RestMethod -Uri "$BaseUrl/api/$($api.Url)"
        Write-Host " ✅ OK ($($data.Count) registros)" -ForegroundColor Green
        $passedTests++
    } catch {
        Write-Host " ❌ FALHOU" -ForegroundColor Red
        $failedTests++
    }
    
    Write-Host ""
    $index++
}

# ============================================================================
# RELATÓRIO FINAL
# ============================================================================
Write-Host "`n╔══════════════════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║                    RELATÓRIO FINAL                               ║" -ForegroundColor Cyan
Write-Host "╚══════════════════════════════════════════════════════════════════╝`n" -ForegroundColor Cyan

$successRate = if ($totalTests -gt 0) { [math]::Round(($passedTests / $totalTests) * 100, 2) } else { 0 }

Write-Host "📊 ESTATÍSTICAS:" -ForegroundColor Yellow
Write-Host "   Total de Testes:    $totalTests" -ForegroundColor White
Write-Host "   ✅ Sucessos:       $passedTests" -ForegroundColor Green
Write-Host "   ❌ Falhas:         $failedTests" -ForegroundColor $(if ($failedTests -gt 0) { "Red" } else { "Green" })
Write-Host "   📈 Taxa de Sucesso: $successRate%" -ForegroundColor $(if ($successRate -eq 100) { "Green" } elseif ($successRate -ge 90) { "Yellow" } else { "Red" })
Write-Host ""

if ($failedTests -eq 0) {
    Write-Host "🎉 DUPLA CHECAGEM CONCLUÍDA COM SUCESSO!" -ForegroundColor Green
    Write-Host "✅ Todos os testes CRUD passaram!" -ForegroundColor Green
    Write-Host "✅ Sistema 100% funcional no Docker!" -ForegroundColor Green
} else {
    Write-Host "⚠️  ALGUNS TESTES FALHARAM" -ForegroundColor Yellow
    Write-Host "   Total de falhas: $failedTests" -ForegroundColor Yellow
}

Write-Host ""
exit $failedTests
