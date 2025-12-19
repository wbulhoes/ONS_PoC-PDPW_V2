@echo off
REM Script para abrir o ambiente de desenvolvimento PDPW PoC (Versão Batch)
REM Compatível com cmd.exe

setlocal enabledelayedexpansion

set "projectRoot=%~dp0"
set "solutionPath=%projectRoot%PDPW.sln"
set "frontendPath=%projectRoot%frontend"

echo.
echo 🚀 Iniciando ambiente PDPW PoC...
echo.

REM Verifica se a solução existe
if not exist "%solutionPath%" (
    echo ❌ Solução não encontrada em: %solutionPath%
    pause
    exit /b 1
)

REM Abre Visual Studio Community com a solução
echo 📂 Abrindo Visual Studio Community com PDPW.sln...
start devenv.exe "%solutionPath%"

REM Aguarda um pouco para o VS abrir
timeout /t 2 /nobreak

REM Abre novo terminal para frontend (usando Windows Terminal ou cmd)
echo 📱 Abrindo terminal para frontend...

REM Tenta usar Windows Terminal se disponível
if exist "%LOCALAPPDATA%\Microsoft\WindowsApps\wt.exe" (
    start wt.exe new-tab -d "%frontendPath%"
) else (
    REM Caso contrário, abre cmd na pasta frontend
    start cmd.exe /k "cd /d "%frontendPath%" && echo 🎨 Terminal Frontend aberto. Execute: npm install (se necessário) e depois npm run dev && echo. && cmd.exe /k"
)

echo.
echo ✅ Ambiente pronto!
echo.
echo 📝 Próximos passos:
echo    1️⃣  No Visual Studio: defina 'PDPW.API' como projeto de inicialização e pressione F5
echo    2️⃣  No terminal frontend: execute 'npm run dev'
echo    3️⃣  Acesse:
echo       - Backend API: http://localhost:5000/swagger
echo       - Frontend: http://localhost:5173
echo.
pause
