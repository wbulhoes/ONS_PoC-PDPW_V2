# ?? SCRIPT DE TESTE - VALIDAR SWAGGER

# Este script vai:
# 1. Limpar build anterior
# 2. Rebuild do projeto
# 3. Rodar a API em modo Development
# 4. Abrir o Swagger no navegador

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "  VALIDAÇÃO DO SWAGGER - PDPW API  " -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# Passo 1: Navegar para a pasta do projeto
Write-Host "?? Navegando para pasta do projeto..." -ForegroundColor Yellow
Set-Location "C:\temp\_ONS_PoC-PDPW_V2"
Write-Host "? Pasta: $(Get-Location)" -ForegroundColor Green
Write-Host ""

# Passo 2: Garantir ambiente Development
Write-Host "?? Configurando ambiente Development..." -ForegroundColor Yellow
$env:ASPNETCORE_ENVIRONMENT = "Development"
Write-Host "? ASPNETCORE_ENVIRONMENT = $env:ASPNETCORE_ENVIRONMENT" -ForegroundColor Green
Write-Host ""

# Passo 3: Limpar build anterior
Write-Host "?? Limpando build anterior..." -ForegroundColor Yellow
dotnet clean --verbosity quiet
Write-Host "? Build limpo" -ForegroundColor Green
Write-Host ""

# Passo 4: Rebuild
Write-Host "?? Fazendo rebuild do projeto..." -ForegroundColor Yellow
$buildResult = dotnet build --verbosity quiet
if ($LASTEXITCODE -ne 0) {
    Write-Host "? ERRO no build!" -ForegroundColor Red
    Write-Host $buildResult
    exit 1
}
Write-Host "? Build concluído com sucesso" -ForegroundColor Green
Write-Host ""

# Passo 5: Verificar se XML foi gerado
Write-Host "?? Verificando arquivo XML de documentação..." -ForegroundColor Yellow
$xmlPath = "src\PDPW.API\bin\Debug\net8.0\PDPW.API.xml"
if (Test-Path $xmlPath) {
    Write-Host "? Arquivo XML encontrado: $xmlPath" -ForegroundColor Green
} else {
    Write-Host "? Arquivo XML não encontrado!" -ForegroundColor Yellow
}
Write-Host ""

# Passo 6: Informações
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "  PRONTO PARA RODAR A API!        " -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "?? URL do Swagger:" -ForegroundColor Yellow
Write-Host "   http://localhost:5000/swagger" -ForegroundColor White
Write-Host ""
Write-Host "?? Endpoints Esperados:" -ForegroundColor Yellow
Write-Host "   ? Empresas           (9 endpoints)" -ForegroundColor Green
Write-Host "   ? TiposUsina         (6 endpoints)" -ForegroundColor Green
Write-Host "   ? Usinas             (8 endpoints)" -ForegroundColor Green
Write-Host "   ? SemanasPmo         (9 endpoints)" -ForegroundColor Green
Write-Host "   ? EquipesPdp         (6 endpoints)" -ForegroundColor Green
Write-Host "   ? DadosEnergeticos   (6 endpoints)" -ForegroundColor Green
Write-Host "   ? Cargas             (8 endpoints)" -ForegroundColor Green
Write-Host "   ? ArquivosDadger     (9 endpoints)" -ForegroundColor Green
Write-Host "   ? RestricoesUG       (9 endpoints)" -ForegroundColor Green
Write-Host "   ====================================="-ForegroundColor Cyan
Write-Host "   TOTAL: 65 endpoints" -ForegroundColor Cyan
Write-Host ""

# Passo 7: Perguntar se deve rodar
Write-Host "?? Deseja rodar a API agora?" -ForegroundColor Yellow
Write-Host "   (O Swagger será aberto automaticamente)" -ForegroundColor Gray
Write-Host ""
$resposta = Read-Host "Pressione ENTER para continuar ou Ctrl+C para cancelar"

# Passo 8: Rodar API
Write-Host ""
Write-Host "?? Iniciando PDPW API..." -ForegroundColor Yellow
Write-Host "   (Aguarde alguns segundos para inicialização)" -ForegroundColor Gray
Write-Host ""

# Abrir navegador após 3 segundos
Start-Job -ScriptBlock {
    Start-Sleep -Seconds 3
    Start-Process "http://localhost:5000/swagger"
} | Out-Null

# Rodar API
Set-Location "src\PDPW.API"
dotnet run
