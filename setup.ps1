# Setup Automático do Projeto PDPW
# Execute: .\setup.ps1

param(
    [switch]$InMemory = $false,
    [switch]$SkipBuild = $false
)

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "   ?? Setup do Projeto PDPW" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

# 1. Verificar .NET
Write-Host "1??  Verificando .NET SDK..." -ForegroundColor Yellow
try {
    $dotnetVersion = dotnet --version
    Write-Host "   ? .NET $dotnetVersion instalado" -ForegroundColor Green
} catch {
    Write-Host "   ? .NET SDK não encontrado!" -ForegroundColor Red
    Write-Host "   ?? Baixe em: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Yellow
    exit 1
}

# 2. Verificar versão do .NET
$requiredVersion = [version]"8.0.0"
$currentVersion = [version]$dotnetVersion
if ($currentVersion -lt $requiredVersion) {
    Write-Host "   ??  Versão do .NET é $currentVersion, mas é necessário 8.0 ou superior" -ForegroundColor Yellow
    Write-Host "   ?? Atualize em: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Yellow
}

# 3. Restaurar dependências
Write-Host "`n2??  Restaurando pacotes NuGet..." -ForegroundColor Yellow
dotnet restore
if ($LASTEXITCODE -eq 0) {
    Write-Host "   ? Pacotes restaurados com sucesso" -ForegroundColor Green
} else {
    Write-Host "   ? Erro ao restaurar pacotes" -ForegroundColor Red
    exit 1
}

# 4. Build (se não skipado)
if (-not $SkipBuild) {
    Write-Host "`n3??  Compilando projeto..." -ForegroundColor Yellow
    dotnet build --no-restore
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ? Projeto compilado com sucesso" -ForegroundColor Green
    } else {
        Write-Host "   ? Erro na compilação" -ForegroundColor Red
        exit 1
    }
}

# 5. Configurar banco de dados
Write-Host "`n4??  Configurando banco de dados..." -ForegroundColor Yellow

$appsettingsPath = "src\PDPW.API\appsettings.Development.json"

if (Test-Path $appsettingsPath) {
    $appsettings = Get-Content $appsettingsPath -Raw | ConvertFrom-Json
    
    if ($InMemory) {
        Write-Host "   ?? Configurando InMemory Database..." -ForegroundColor Cyan
        $appsettings.UseInMemoryDatabase = $true
        $appsettings | ConvertTo-Json -Depth 10 | Set-Content $appsettingsPath
        Write-Host "   ? InMemory Database configurado" -ForegroundColor Green
        Write-Host "   ??  Dados serão temporários (perdidos ao fechar)" -ForegroundColor Yellow
    } else {
        Write-Host "   ???  Configurando SQL Server/LocalDB..." -ForegroundColor Cyan
        $appsettings.UseInMemoryDatabase = $false
        $appsettings | ConvertTo-Json -Depth 10 | Set-Content $appsettingsPath
        
        # Verificar se EF Tools está instalado
        Write-Host "   ?? Verificando Entity Framework Tools..." -ForegroundColor Cyan
        $efTools = dotnet tool list -g | Select-String "dotnet-ef"
        
        if (-not $efTools) {
            Write-Host "   ?? Instalando dotnet-ef..." -ForegroundColor Yellow
            dotnet tool install --global dotnet-ef
        } else {
            Write-Host "   ? dotnet-ef já instalado" -ForegroundColor Green
        }
        
        # Perguntar se quer aplicar migrations
        Write-Host "`n   ? Deseja criar o banco de dados agora? (S/N)" -ForegroundColor Yellow
        $response = Read-Host "   "
        
        if ($response -eq "S" -or $response -eq "s") {
            Write-Host "   ???  Aplicando migrations..." -ForegroundColor Cyan
            dotnet ef database update --project src\PDPW.Infrastructure --startup-project src\PDPW.API
            
            if ($LASTEXITCODE -eq 0) {
                Write-Host "   ? Banco de dados criado com sucesso" -ForegroundColor Green
            } else {
                Write-Host "   ??  Erro ao criar banco de dados" -ForegroundColor Yellow
                Write-Host "   ?? Você pode criar depois com:" -ForegroundColor Cyan
                Write-Host "      dotnet ef database update --project src\PDPW.Infrastructure --startup-project src\PDPW.API" -ForegroundColor Gray
            }
        } else {
            Write-Host "   ??  Pulando criação do banco de dados" -ForegroundColor Yellow
            Write-Host "   ?? Para criar depois, execute:" -ForegroundColor Cyan
            Write-Host "      dotnet ef database update --project src\PDPW.Infrastructure --startup-project src\PDPW.API" -ForegroundColor Gray
        }
    }
} else {
    Write-Host "   ??  Arquivo appsettings.Development.json não encontrado" -ForegroundColor Yellow
}

# 6. Resumo
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "   ? Setup Concluído!" -ForegroundColor Green
Write-Host "========================================`n" -ForegroundColor Cyan

Write-Host "?? Próximos passos:" -ForegroundColor Yellow
Write-Host ""
Write-Host "   1??  Executar a API:" -ForegroundColor Cyan
Write-Host "      dotnet run --project src\PDPW.API" -ForegroundColor White
Write-Host ""
Write-Host "   2??  Acessar Swagger:" -ForegroundColor Cyan
Write-Host "      https://localhost:65417/swagger" -ForegroundColor White
Write-Host ""
Write-Host "   3??  Executar Hello World (Validação):" -ForegroundColor Cyan
Write-Host "      dotnet run --project HelloWorld" -ForegroundColor White
Write-Host ""
Write-Host "   4??  Acessar Dashboard:" -ForegroundColor Cyan
Write-Host "      http://localhost:5555" -ForegroundColor White
Write-Host ""

if ($InMemory) {
    Write-Host "??  Você está usando InMemory Database" -ForegroundColor Cyan
    Write-Host "   Os dados serão perdidos ao fechar a aplicação" -ForegroundColor Yellow
    Write-Host ""
}

Write-Host "?? Documentação disponível em:" -ForegroundColor Yellow
Write-Host "   - DATABASE_SETUP.md" -ForegroundColor Gray
Write-Host "   - TROUBLESHOOTING.md" -ForegroundColor Gray
Write-Host "   - QUICK_START_INMEMORY.md" -ForegroundColor Gray
Write-Host ""

Write-Host "?? Bom desenvolvimento!" -ForegroundColor Green
Write-Host ""
