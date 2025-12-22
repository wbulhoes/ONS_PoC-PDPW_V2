# Script para Gerenciar a API PDPW
# Uso: .\gerenciar-api.ps1 [start|stop|restart|status|logs|test]

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet('start', 'stop', 'restart', 'status', 'logs', 'test', 'clean')]
    [string]$Action = 'start',
    
    [Parameter(Mandatory=$false)]
    [int]$Port = 5001
)

$ErrorActionPreference = "SilentlyContinue"
$projectPath = "C:\temp\_ONS_PoC-PDPW_V2\src\PDPW.API"
$baseUrl = "http://localhost:$Port"

function Stop-PDPWApi {
    Write-Host "🛑 Parando API..." -ForegroundColor Yellow
    
    # Matar todos os processos dotnet
    Get-Process -Name "dotnet" | Stop-Process -Force 2>$null
    Get-Process -Name "w3wp" | Stop-Process -Force 2>$null
    
    # Liberar porta
    netstat -ano | findstr ":$Port" | ForEach-Object {
        if ($_ -match '\s+(\d+)$') {
            taskkill /F /PID $matches[1] 2>$null | Out-Null
        }
    }
    
    Start-Sleep -Seconds 2
    Write-Host "✅ API parada!" -ForegroundColor Green
}

function Start-PDPWApi {
    Write-Host "🚀 Iniciando API na porta $Port..." -ForegroundColor Cyan
    
    # Garantir que está parada
    Stop-PDPWApi
    
    # Iniciar API
    Push-Location $projectPath
    $process = Start-Process -FilePath "dotnet" `
                             -ArgumentList "run --urls=$baseUrl" `
                             -WindowStyle Hidden `
                             -PassThru
    Pop-Location
    
    Write-Host "⏳ Aguardando API iniciar..." -ForegroundColor Yellow
    Start-Sleep -Seconds 12
    
    # Testar se subiu
    try {
        $response = Invoke-RestMethod -Uri "$baseUrl/" -Method GET -TimeoutSec 5
        Write-Host "✅ API iniciada com sucesso!" -ForegroundColor Green
        Write-Host "   Process ID: $($process.Id)" -ForegroundColor Gray
        Write-Host "   URL: $baseUrl" -ForegroundColor Gray
        Write-Host "   Swagger: $baseUrl/swagger" -ForegroundColor Gray
        return $true
    }
    catch {
        Write-Host "❌ Erro ao iniciar API: $($_.Exception.Message)" -ForegroundColor Red
        return $false
    }
}

function Get-PDPWApiStatus {
    Write-Host "📊 Status da API:" -ForegroundColor Cyan
    
    # Verificar processos
    $processes = Get-Process -Name "dotnet" 2>$null
    if ($processes) {
        Write-Host "   Processos dotnet rodando: $($processes.Count)" -ForegroundColor Yellow
        $processes | ForEach-Object {
            Write-Host "   - PID: $($_.Id) | CPU: $($_.CPU.ToString('F2'))s | Memory: $([Math]::Round($_.WorkingSet64/1MB, 2)) MB" -ForegroundColor Gray
        }
    }
    else {
        Write-Host "   ❌ Nenhum processo dotnet rodando" -ForegroundColor Red
    }
    
    # Testar endpoint
    Write-Host "`n🔍 Testando conectividade..." -ForegroundColor Cyan
    try {
        $response = Invoke-RestMethod -Uri "$baseUrl/" -Method GET -TimeoutSec 3
        Write-Host "   ✅ API respondendo!" -ForegroundColor Green
        Write-Host "   Status: $($response.status)" -ForegroundColor Gray
        Write-Host "   Versão: $($response.version)" -ForegroundColor Gray
        Write-Host "   Ambiente: $($response.environment)" -ForegroundColor Gray
    }
    catch {
        Write-Host "   ❌ API não está respondendo" -ForegroundColor Red
        Write-Host "   Erro: $($_.Exception.Message)" -ForegroundColor Red
    }
}

function Test-PDPWApis {
    Write-Host "🧪 Testando APIs..." -ForegroundColor Cyan
    
    $endpoints = @(
        @{Name="Root"; Url="/"},
        @{Name="TiposUsina"; Url="/api/tiposusina"},
        @{Name="Empresas"; Url="/api/empresas"},
        @{Name="Usinas"; Url="/api/usinas"},
        @{Name="SemanasPMO"; Url="/api/semanaspmo"},
        @{Name="Cargas"; Url="/api/cargas"},
        @{Name="Intercambios"; Url="/api/intercambios"},
        @{Name="Balancos"; Url="/api/balancos"},
        @{Name="EquipesPDP"; Url="/api/equipespdp"}
    )
    
    $results = @()
    foreach ($endpoint in $endpoints) {
        try {
            $response = Invoke-RestMethod -Uri "$baseUrl$($endpoint.Url)" -Method GET -TimeoutSec 5
            $count = if ($response.Count) { $response.Count } else { "N/A" }
            Write-Host "   ✅ $($endpoint.Name) - $count registros" -ForegroundColor Green
            $results += @{Name=$endpoint.Name; Status="OK"; Count=$count}
        }
        catch {
            Write-Host "   ❌ $($endpoint.Name) - ERRO" -ForegroundColor Red
            $results += @{Name=$endpoint.Name; Status="ERRO"; Count=0}
        }
    }
    
    Write-Host "`n📊 Resumo:" -ForegroundColor Cyan
    $ok = ($results | Where-Object { $_.Status -eq "OK" }).Count
    $total = $results.Count
    Write-Host "   $ok/$total endpoints funcionando" -ForegroundColor $(if ($ok -eq $total) { "Green" } else { "Yellow" })
}

function Show-PDPWLogs {
    Write-Host "📜 Logs recentes (últimas 50 linhas):" -ForegroundColor Cyan
    Write-Host "   (Pressione Ctrl+C para sair)`n" -ForegroundColor Gray
    
    Push-Location $projectPath
    dotnet run --urls=$baseUrl 2>&1 | Select-Object -Last 50
    Pop-Location
}

function Clean-PDPWPorts {
    Write-Host "🧹 Limpando portas e processos..." -ForegroundColor Yellow
    
    # Matar processos dotnet
    Get-Process -Name "dotnet" | ForEach-Object {
        Write-Host "   Matando processo dotnet (PID: $($_.Id))" -ForegroundColor Gray
        $_ | Stop-Process -Force 2>$null
    }
    
    # Liberar portas
    @(5000, 5001, 5173, 3000) | ForEach-Object {
        $port = $_
        netstat -ano | findstr ":$port" | ForEach-Object {
            if ($_ -match '\s+(\d+)$') {
                $pid = $matches[1]
                Write-Host "   Liberando porta $port (PID: $pid)" -ForegroundColor Gray
                taskkill /F /PID $pid 2>$null | Out-Null
            }
        }
    }
    
    Start-Sleep -Seconds 2
    Write-Host "✅ Limpeza concluída!" -ForegroundColor Green
}

# Execução principal
Write-Host "`n🎯 PDPW API Manager" -ForegroundColor Cyan
Write-Host "==================`n" -ForegroundColor Cyan

switch ($Action.ToLower()) {
    "start" {
        Start-PDPWApi
    }
    "stop" {
        Stop-PDPWApi
    }
    "restart" {
        Stop-PDPWApi
        Start-Sleep -Seconds 2
        Start-PDPWApi
    }
    "status" {
        Get-PDPWApiStatus
    }
    "logs" {
        Show-PDPWLogs
    }
    "test" {
        Test-PDPWApis
    }
    "clean" {
        Clean-PDPWPorts
    }
    default {
        Write-Host "❌ Ação inválida: $Action" -ForegroundColor Red
        Write-Host "`nUso: .\gerenciar-api.ps1 [start|stop|restart|status|logs|test|clean]" -ForegroundColor Yellow
    }
}

Write-Host ""
