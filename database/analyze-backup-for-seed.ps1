# Script para extrair 20% dos dados do backup do cliente para Seed Data
# Data: 19/12/2024

Write-Host "?? ANALISANDO BACKUP DO CLIENTE PARA SEED DATA" -ForegroundColor Cyan
Write-Host "=" * 60

$backupPath = "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak"
$serverInstance = "(localdb)\mssqllocaldb"
$tempDatabase = "PDPW_TEMP_ANALYSIS"
$outputPath = "C:\temp\_ONS_PoC-PDPW\database\seed-data-extracted"

# Criar diretório de saída
if (-not (Test-Path $outputPath)) {
    New-Item -ItemType Directory -Path $outputPath -Force | Out-Null
}

Write-Host ""
Write-Host "?? Configurações:" -ForegroundColor Yellow
Write-Host "   Backup: $backupPath"
Write-Host "   Servidor: $serverInstance"
Write-Host "   Banco temporário: $tempDatabase"
Write-Host "   Saída: $outputPath"
Write-Host ""

# Verificar se backup existe
if (-not (Test-Path $backupPath)) {
    Write-Host "? ERRO: Backup não encontrado!" -ForegroundColor Red
    Write-Host "   Caminho: $backupPath" -ForegroundColor Red
    exit 1
}

Write-Host "? Backup encontrado!" -ForegroundColor Green
Write-Host ""

# SQL para consultar dados
$sqlQueries = @"
-- ========================================
-- TIPOS DE USINA (100% dos dados - são poucos)
-- ========================================
SELECT TOP 100 PERCENT *
FROM [dbo].[TiposUsina]
ORDER BY Nome;

-- ========================================
-- EMPRESAS (20% dos dados)
-- ========================================
SELECT TOP 20 PERCENT *
FROM [dbo].[Empresas]
ORDER BY Nome;

-- ========================================
-- USINAS (20% dos dados)
-- ========================================
SELECT TOP 20 PERCENT *
FROM [dbo].[Usinas]
ORDER BY Nome;

-- ========================================
-- UNIDADES GERADORAS (20% dos dados)
-- ========================================
SELECT TOP 20 PERCENT *
FROM [dbo].[UnidadesGeradoras]
ORDER BY Nome;

-- ========================================
-- SEMANAS PMO (últimas 10 semanas)
-- ========================================
SELECT TOP 10 *
FROM [dbo].[SemanasPMO]
ORDER BY Ano DESC, Numero DESC;
"@

Write-Host "?? INSTRUÇÕES MANUAIS:" -ForegroundColor Cyan
Write-Host ""
Write-Host "Como o backup está em .bak, você precisará:" -ForegroundColor Yellow
Write-Host ""
Write-Host "1??  Restaurar o backup em um banco temporário:" -ForegroundColor White
Write-Host "   ```sql" -ForegroundColor Gray
Write-Host "   USE master;"
Write-Host "   RESTORE DATABASE [$tempDatabase]"
Write-Host "   FROM DISK = '$backupPath'"
Write-Host "   WITH"
Write-Host "       MOVE 'PDP_TST' TO 'C:\temp\PDPW_TEMP_ANALYSIS.mdf',"
Write-Host "       MOVE 'PDP_TST_log' TO 'C:\temp\PDPW_TEMP_ANALYSIS_log.ldf',"
Write-Host "       REPLACE;"
Write-Host "   ```" -ForegroundColor Gray
Write-Host ""

Write-Host "2??  Executar consultas para extrair 20% dos dados:" -ForegroundColor White
Write-Host "   ```sql" -ForegroundColor Gray
Write-Host "   USE [$tempDatabase];"
Write-Host ""
Write-Host "   -- TIPOS DE USINA" -ForegroundColor Green
Write-Host "   SELECT * FROM TiposUsina ORDER BY Nome;"
Write-Host ""
Write-Host "   -- EMPRESAS (20%)" -ForegroundColor Green
Write-Host "   SELECT TOP 20 PERCENT * FROM Empresas ORDER BY Nome;"
Write-Host ""
Write-Host "   -- USINAS (20%)" -ForegroundColor Green  
Write-Host "   SELECT TOP 20 PERCENT * FROM Usinas ORDER BY Nome;"
Write-Host ""
Write-Host "   -- UNIDADES GERADORAS (20%)" -ForegroundColor Green
Write-Host "   SELECT TOP 20 PERCENT * FROM UnidadesGeradoras ORDER BY Nome;"
Write-Host ""
Write-Host "   -- SEMANAS PMO (últimas 10)" -ForegroundColor Green
Write-Host "   SELECT TOP 10 * FROM SemanasPMO ORDER BY Ano DESC, Numero DESC;"
Write-Host "   ```" -ForegroundColor Gray
Write-Host ""

Write-Host "3??  Exportar resultados para JSON/CSV" -ForegroundColor White
Write-Host ""

Write-Host "4??  OU usar o script PowerShell que vou gerar:" -ForegroundColor White
Write-Host "   .\database\extract-seed-data.ps1" -ForegroundColor Cyan
Write-Host ""

Write-Host "=" * 60
Write-Host ""
Write-Host "?? Gerando script SQL para facilitar..." -ForegroundColor Yellow

# Salvar script SQL
$sqlScriptPath = "$outputPath\extract-seed-data.sql"
$sqlQueries | Out-File -FilePath $sqlScriptPath -Encoding UTF8

Write-Host "? Script SQL salvo em:" -ForegroundColor Green
Write-Host "   $sqlScriptPath" -ForegroundColor Cyan
Write-Host ""

Write-Host "?? ALTERNATIVA SIMPLES:" -ForegroundColor Cyan
Write-Host "   Vou criar seed data com dados fictícios mas realistas!" -ForegroundColor Yellow
Write-Host "   Baseados na estrutura do sistema PDPW do ONS." -ForegroundColor Yellow
Write-Host ""

Write-Host "?? Próximos passos:" -ForegroundColor Green
Write-Host "   1. Criar seed data com dados fictícios realistas"
Write-Host "   2. Popular banco de desenvolvimento"
Write-Host "   3. Testar APIs com dados reais"
Write-Host ""
