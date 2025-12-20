# ============================================================================
# Guia: Habilitar TCP/IP no SQL Server
# ============================================================================

Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host " Guia para Habilitar TCP/IP no SQL Server" -ForegroundColor Cyan
Write-Host "============================================================================`n" -ForegroundColor Cyan

Write-Host "IMPORTANTE: Este processo requer reinicialização do SQL Server`n" -ForegroundColor Yellow

Write-Host "Passo 1: Abrir SQL Server Configuration Manager" -ForegroundColor Yellow
Write-Host "  1. Pressionar Win + R" -ForegroundColor Gray
Write-Host "  2. Digitar: SQLServerManager15.msc (SQL 2019)" -ForegroundColor Gray
Write-Host "     Ou: SQLServerManager14.msc (SQL 2017)" -ForegroundColor Gray
Write-Host "     Ou: SQLServerManager13.msc (SQL 2016)" -ForegroundColor Gray
Write-Host "  3. Pressionar Enter`n" -ForegroundColor Gray

Write-Host "Passo 2: Habilitar TCP/IP" -ForegroundColor Yellow
Write-Host "  1. Expandir 'SQL Server Network Configuration'" -ForegroundColor Gray
Write-Host "  2. Clicar em 'Protocols for MSSQLSERVER' (ou 'Protocols for SQLEXPRESS')" -ForegroundColor Gray
Write-Host "  3. Clicar com botão direito em 'TCP/IP'" -ForegroundColor Gray
Write-Host "  4. Selecionar 'Enable'" -ForegroundColor Gray
Write-Host "  5. Clicar em 'OK'`n" -ForegroundColor Gray

Write-Host "Passo 3: Configurar Porta 1433 (se necessário)" -ForegroundColor Yellow
Write-Host "  1. Clicar com botão direito em 'TCP/IP' > Properties" -ForegroundColor Gray
Write-Host "  2. Ir para aba 'IP Addresses'" -ForegroundColor Gray
Write-Host "  3. Rolar até 'IPAll'" -ForegroundColor Gray
Write-Host "  4. Configurar:" -ForegroundColor Gray
Write-Host "     - TCP Dynamic Ports: (deixar vazio)" -ForegroundColor Gray
Write-Host "     - TCP Port: 1433" -ForegroundColor Gray
Write-Host "  5. Clicar em 'OK'`n" -ForegroundColor Gray

Write-Host "Passo 4: Reiniciar SQL Server" -ForegroundColor Yellow
Write-Host "  1. No Configuration Manager, ir em 'SQL Server Services'" -ForegroundColor Gray
Write-Host "  2. Clicar com botão direito em 'SQL Server (MSSQLSERVER)'" -ForegroundColor Gray
Write-Host "     Ou: 'SQL Server (SQLEXPRESS)'" -ForegroundColor Gray
Write-Host "  3. Selecionar 'Restart'" -ForegroundColor Gray
Write-Host "  4. Aguardar reinicialização`n" -ForegroundColor Gray

Write-Host "Passo 5: Verificar" -ForegroundColor Yellow
Write-Host "  Executar: Test-NetConnection -ComputerName localhost -Port 1433`n" -ForegroundColor Cyan

Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host " Ou use SQL Server Express (mais simples)" -ForegroundColor Cyan
Write-Host "============================================================================`n" -ForegroundColor Cyan

Write-Host "Se você tem SQL Server Express, é mais simples usar a instância nomeada:" -ForegroundColor White
Write-Host ".\database\restore-database.ps1 -ServerInstance 'localhost\SQLEXPRESS' -BackupPath '...' -DatabaseName 'PDPW_DB'`n" -ForegroundColor Cyan

Write-Host "Deseja tentar restaurar agora usando SQLEXPRESS? (S/N): " -NoNewline -ForegroundColor Yellow
$response = Read-Host

if ($response -eq "S" -or $response -eq "s") {
    Write-Host "`nIniciando restauração com SQL Server Express...`n" -ForegroundColor Green
    
    $backupPath = "C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak"
    $databaseName = "PDPW_DB"
    
    # Tentar restaurar
    & "$PSScriptRoot\restore-database.ps1" -ServerInstance "localhost\SQLEXPRESS" -BackupPath $backupPath -DatabaseName $databaseName -GenerateScripts
} else {
    Write-Host "`nPor favor, siga os passos acima para habilitar TCP/IP" -ForegroundColor Yellow
    Write-Host "Após habilitar, execute:" -ForegroundColor Yellow
    Write-Host ".\database\restore-database.ps1 -BackupPath 'C:\temp\_ONS_PoC-PDPW\pdpw_act\Backup_PDP_TST.bak' -DatabaseName 'PDPW_DB'`n" -ForegroundColor Cyan
}
