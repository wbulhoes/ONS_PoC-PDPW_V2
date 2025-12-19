# ============================================================================
# Script para Verificar e Iniciar SQL Server
# ============================================================================

Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host " Verificando e Iniciando SQL Server" -ForegroundColor Cyan
Write-Host "============================================================================`n" -ForegroundColor Cyan

# ============================================================================
# 1. Verificar Serviços SQL Server Instalados
# ============================================================================

Write-Host "1. Procurando serviços SQL Server instalados..." -ForegroundColor Yellow

try {
    # Tentar listar serviços SQL (pode falhar sem admin)
    $sqlServices = Get-Service -DisplayName "*SQL Server*" -ErrorAction SilentlyContinue
    
    if ($sqlServices) {
        Write-Host "`n   Serviços SQL Server encontrados:`n" -ForegroundColor Green
        
        foreach ($service in $sqlServices) {
            $statusColor = if ($service.Status -eq "Running") { "Green" } else { "Red" }
            Write-Host "   Nome: " -NoNewline
            Write-Host "$($service.Name)" -ForegroundColor Cyan
            Write-Host "   Display Name: $($service.DisplayName)" -ForegroundColor Gray
            Write-Host "   Status: " -NoNewline
            Write-Host "$($service.Status)" -ForegroundColor $statusColor
            Write-Host "   Start Type: $($service.StartType)`n" -ForegroundColor Gray
        }
        
        # Verificar se algum está rodando
        $runningServices = $sqlServices | Where-Object { $_.Status -eq "Running" }
        
        if ($runningServices) {
            Write-Host "? SQL Server está rodando!" -ForegroundColor Green
            exit 0
        } else {
            Write-Host "? SQL Server está instalado mas NÃO está rodando" -ForegroundColor Yellow
            
            # Perguntar se quer iniciar
            Write-Host "`nDeseja tentar iniciar o SQL Server? (S/N): " -NoNewline -ForegroundColor Yellow
            $response = Read-Host
            
            if ($response -eq "S" -or $response -eq "s") {
                Write-Host "`nTentando iniciar SQL Server..." -ForegroundColor Yellow
                Write-Host "IMPORTANTE: Você precisa executar este script como ADMINISTRADOR`n" -ForegroundColor Red
                
                # Tentar iniciar cada serviço parado
                foreach ($service in $sqlServices) {
                    if ($service.Status -ne "Running") {
                        try {
                            Write-Host "Iniciando $($service.DisplayName)..." -ForegroundColor Yellow
                            Start-Service $service.Name -ErrorAction Stop
                            Write-Host "? $($service.DisplayName) iniciado com sucesso!" -ForegroundColor Green
                        } catch {
                            Write-Host "? Erro ao iniciar $($service.DisplayName): $_" -ForegroundColor Red
                            Write-Host "`nPor favor, execute este comando como ADMINISTRADOR:" -ForegroundColor Yellow
                            Write-Host "Start-Service $($service.Name)`n" -ForegroundColor Cyan
                        }
                    }
                }
            }
        }
    } else {
        Write-Host "   ? Nenhum serviço SQL Server encontrado" -ForegroundColor Red
    }
} catch {
    Write-Host "   ? Erro ao verificar serviços (pode precisar de admin): $_" -ForegroundColor Yellow
}

# ============================================================================
# 2. Instruções de Inicialização Manual
# ============================================================================

Write-Host "`n============================================================================" -ForegroundColor Cyan
Write-Host " Como Iniciar SQL Server Manualmente" -ForegroundColor Cyan
Write-Host "============================================================================`n" -ForegroundColor Cyan

Write-Host "Opção 1: Via Services.msc" -ForegroundColor Yellow
Write-Host "  1. Pressionar Win + R" -ForegroundColor Gray
Write-Host "  2. Digitar: services.msc" -ForegroundColor Gray
Write-Host "  3. Procurar por 'SQL Server (MSSQLSERVER)' ou 'SQL Server (SQLEXPRESS)'" -ForegroundColor Gray
Write-Host "  4. Clicar com botão direito > Iniciar`n" -ForegroundColor Gray

Write-Host "Opção 2: Via PowerShell (como Administrador)" -ForegroundColor Yellow
Write-Host "  Start-Service MSSQLSERVER" -ForegroundColor Cyan
Write-Host "  # ou" -ForegroundColor Gray
Write-Host "  Start-Service 'MSSQL`$SQLEXPRESS'`n" -ForegroundColor Cyan

Write-Host "Opção 3: Via SQL Server Configuration Manager" -ForegroundColor Yellow
Write-Host "  1. Abrir 'SQL Server Configuration Manager'" -ForegroundColor Gray
Write-Host "  2. Expandir 'SQL Server Services'" -ForegroundColor Gray
Write-Host "  3. Clicar com botão direito no serviço > Start`n" -ForegroundColor Gray

# ============================================================================
# 3. Verificar se SQL Server Está Instalado
# ============================================================================

Write-Host "============================================================================" -ForegroundColor Cyan
Write-Host " SQL Server Está Instalado?" -ForegroundColor Cyan
Write-Host "============================================================================`n" -ForegroundColor Cyan

# Verificar pastas comuns de instalação
$sqlPaths = @(
    "C:\Program Files\Microsoft SQL Server",
    "C:\Program Files (x86)\Microsoft SQL Server"
)

$sqlInstalled = $false

foreach ($path in $sqlPaths) {
    if (Test-Path $path) {
        $sqlInstalled = $true
        Write-Host "? SQL Server parece estar instalado em: $path" -ForegroundColor Green
        
        # Tentar listar versões instaladas
        $versionDirs = Get-ChildItem -Path $path -Directory -ErrorAction SilentlyContinue | 
                       Where-Object { $_.Name -match "^\d+$" }
        
        if ($versionDirs) {
            Write-Host "`n  Versões detectadas:" -ForegroundColor Gray
            foreach ($dir in $versionDirs) {
                $versionNumber = switch ($dir.Name) {
                    "150" { "SQL Server 2019" }
                    "140" { "SQL Server 2017" }
                    "130" { "SQL Server 2016" }
                    "120" { "SQL Server 2014" }
                    "110" { "SQL Server 2012" }
                    default { "SQL Server (versão $($dir.Name))" }
                }
                Write-Host "    - $versionNumber" -ForegroundColor Gray
            }
        }
    }
}

if (-not $sqlInstalled) {
    Write-Host "? SQL Server NÃO parece estar instalado" -ForegroundColor Red
    
    Write-Host "`n============================================================================" -ForegroundColor Yellow
    Write-Host " Como Instalar SQL Server" -ForegroundColor Yellow
    Write-Host "============================================================================`n" -ForegroundColor Yellow
    
    Write-Host "Recomendação: SQL Server 2019 Express (Gratuito)`n" -ForegroundColor Green
    
    Write-Host "Passo 1: Download" -ForegroundColor Yellow
    Write-Host "  https://www.microsoft.com/sql-server/sql-server-downloads`n" -ForegroundColor Cyan
    
    Write-Host "Passo 2: Escolher Edição" -ForegroundColor Yellow
    Write-Host "  - Express: Gratuito, ideal para desenvolvimento" -ForegroundColor Gray
    Write-Host "  - Developer: Gratuito, todas as funcionalidades`n" -ForegroundColor Gray
    
    Write-Host "Passo 3: Instalação" -ForegroundColor Yellow
    Write-Host "  - Escolher: 'Basic' (instalação simplificada)" -ForegroundColor Gray
    Write-Host "  - Aceitar termos" -ForegroundColor Gray
    Write-Host "  - Aguardar instalação (5-15 minutos)`n" -ForegroundColor Gray
    
    Write-Host "Passo 4: Após Instalação" -ForegroundColor Yellow
    Write-Host "  - Reiniciar o computador (recomendado)" -ForegroundColor Gray
    Write-Host "  - Executar este script novamente`n" -ForegroundColor Gray
    
    Write-Host "Quer baixar agora? Pressione Enter para abrir o navegador..." -ForegroundColor Yellow
    Read-Host
    Start-Process "https://www.microsoft.com/sql-server/sql-server-downloads"
}

# ============================================================================
# Resumo
# ============================================================================

Write-Host "`n============================================================================" -ForegroundColor Cyan
Write-Host " Resumo e Próximos Passos" -ForegroundColor Cyan
Write-Host "============================================================================`n" -ForegroundColor Cyan

if ($sqlInstalled) {
    Write-Host "? SQL Server está instalado" -ForegroundColor Green
    Write-Host "? Mas NÃO está rodando" -ForegroundColor Yellow
    Write-Host "`nAções:" -ForegroundColor Yellow
    Write-Host "  1. Iniciar SQL Server via services.msc (instruções acima)" -ForegroundColor Gray
    Write-Host "  2. Após iniciar, executar novamente:" -ForegroundColor Gray
    Write-Host "     .\database\check-prerequisites.ps1`n" -ForegroundColor Cyan
} else {
    Write-Host "? SQL Server NÃO está instalado" -ForegroundColor Red
    Write-Host "`nAções:" -ForegroundColor Yellow
    Write-Host "  1. Baixar SQL Server Express (gratuito)" -ForegroundColor Gray
    Write-Host "  2. Instalar" -ForegroundColor Gray
    Write-Host "  3. Reiniciar computador" -ForegroundColor Gray
    Write-Host "  4. Executar novamente:" -ForegroundColor Gray
    Write-Host "     .\database\check-prerequisites.ps1`n" -ForegroundColor Cyan
}
