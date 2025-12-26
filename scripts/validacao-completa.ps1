# 🚀 VALIDAÇÃO COMPLETA - Docker + Testes - POC PDPw 100%

Write-Host "==============================================================" -ForegroundColor Magenta
Write-Host "  🚀 VALIDAÇÃO COMPLETA POC PDPw - 100% DE COBERTURA" -ForegroundColor Magenta
Write-Host "==============================================================" -ForegroundColor Magenta
Write-Host ""

$startTime = Get-Date

# ========================================
# ETAPA 1: VALIDAÇÃO DOCKER
# ========================================
Write-Host "🐳 ETAPA 1: Validação Docker" -ForegroundColor Cyan
Write-Host "==============================================================" -ForegroundColor Gray
& "$PSScriptRoot\validar-docker.ps1"

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "❌ Falha na validação Docker!" -ForegroundColor Red
    exit 1
}

# Aguardar um pouco para garantir estabilidade
Write-Host ""
Write-Host "⏳ Aguardando estabilização dos serviços (30s)..." -ForegroundColor Yellow
Start-Sleep -Seconds 30

# ========================================
# ETAPA 2: TESTES AUTOMATIZADOS
# ========================================
Write-Host ""
Write-Host "🧪 ETAPA 2: Testes Automatizados" -ForegroundColor Cyan
Write-Host "==============================================================" -ForegroundColor Gray
& "$PSScriptRoot\testar-endpoints.ps1"

$testResult = $LASTEXITCODE

# ========================================
# ETAPA 3: RELATÓRIO FINAL
# ========================================
Write-Host ""
Write-Host "==============================================================" -ForegroundColor Magenta
Write-Host "  📊 RELATÓRIO FINAL" -ForegroundColor Magenta
Write-Host "==============================================================" -ForegroundColor Magenta

$endTime = Get-Date
$duration = $endTime - $startTime

Write-Host ""
Write-Host "⏱️  Tempo Total: $($duration.ToString('mm\:ss'))" -ForegroundColor White
Write-Host ""

if ($testResult -eq 0) {
    Write-Host "✅ VALIDAÇÃO COMPLETA: SUCESSO!" -ForegroundColor Green
    Write-Host ""
    Write-Host "🎉 A POC está 100% funcional via Docker!" -ForegroundColor Green
    Write-Host ""
    Write-Host "📝 Próximos Passos:" -ForegroundColor Yellow
    Write-Host "   1. Acessar Swagger: http://localhost:5001/swagger" -ForegroundColor White
    Write-Host "   2. Testar manualmente endpoints específicos" -ForegroundColor White
    Write-Host "   3. Gerar relatório para apresentação" -ForegroundColor White
    Write-Host ""
} else {
    Write-Host "⚠️  VALIDAÇÃO COMPLETA: COM AVISOS" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Alguns testes falharam, mas o sistema está rodando." -ForegroundColor Yellow
    Write-Host "Verifique os logs para mais detalhes." -ForegroundColor Yellow
    Write-Host ""
}

Write-Host "🌐 URLs Importantes:" -ForegroundColor Cyan
Write-Host "   Swagger UI:         http://localhost:5001/swagger" -ForegroundColor White
Write-Host "   Health Check:       http://localhost:5001/health" -ForegroundColor White
Write-Host "   Dashboard Resumo:   http://localhost:5001/api/dashboard/resumo" -ForegroundColor White
Write-Host "   Dashboard Alertas:  http://localhost:5001/api/dashboard/alertas" -ForegroundColor White
Write-Host ""

Write-Host "📚 Documentação:" -ForegroundColor Cyan
Write-Host "   README:            docs/README.md" -ForegroundColor White
Write-Host "   Relatório Final:   docs/RELATORIO_FINAL_POC_83.md" -ForegroundColor White
Write-Host "   Comandos Rápidos:  docs/COMANDOS_RAPIDOS.md" -ForegroundColor White
Write-Host ""

exit $testResult
