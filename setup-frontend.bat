@echo off
echo ========================================
echo   SETUP FRONTEND PDPw
echo   Configuracao Automatica
echo ========================================
echo.

REM Navegar para o diretorio frontend
cd frontend

echo [1/4] Verificando Node.js...
node --version
if errorlevel 1 (
    echo ERRO: Node.js nao encontrado!
    echo Por favor, instale Node.js 18+ de https://nodejs.org
    pause
    exit /b 1
)
echo OK!
echo.

echo [2/4] Instalando dependencias...
call npm install
if errorlevel 1 (
    echo ERRO ao instalar dependencias!
    pause
    exit /b 1
)
echo OK!
echo.

echo [3/4] Configurando variaveis de ambiente...
if not exist .env (
    copy .env.example .env
    echo Arquivo .env criado!
) else (
    echo Arquivo .env ja existe.
)
echo OK!
echo.

echo [4/4] Verificando TypeScript...
call npm run type-check
if errorlevel 1 (
    echo AVISO: Erros de TypeScript encontrados
    echo Isso e normal se for a primeira vez
) else (
    echo OK!
)
echo.

echo ========================================
echo   SETUP CONCLUIDO COM SUCESSO!
echo ========================================
echo.
echo Para iniciar o frontend:
echo   npm run dev
echo.
echo Frontend estara disponivel em:
echo   http://localhost:5173
echo.
echo Certifique-se de que o backend esta rodando:
echo   cd ../src/PDPW.API
echo   dotnet run
echo.
pause
