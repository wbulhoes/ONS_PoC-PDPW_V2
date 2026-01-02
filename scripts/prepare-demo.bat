@echo off
REM Script de preparação para apresentação do PDPw (Windows)
REM Executa todas as verificações e inicia o ambiente

setlocal EnableDelayedExpansion

echo.
echo ========================================
echo PDPw - Preparacao para Apresentacao
echo ========================================
echo.

REM 1. Verificar Docker
echo 1. Verificando Docker...
docker --version >nul 2>&1
if errorlevel 1 (
    echo [ERRO] Docker nao encontrado! Instale o Docker Desktop.
    pause
    exit /b 1
)
echo [OK] Docker instalado

REM 2. Verificar Docker Compose
echo 2. Verificando Docker Compose...
docker-compose --version >nul 2>&1
if errorlevel 1 (
    echo [ERRO] Docker Compose nao encontrado!
    pause
    exit /b 1
)
echo [OK] Docker Compose instalado

REM 3. Verificar se Docker está rodando
echo 3. Verificando se Docker esta rodando...
docker info >nul 2>&1
if errorlevel 1 (
    echo [ERRO] Docker nao esta rodando! Inicie o Docker Desktop.
    pause
    exit /b 1
)
echo [OK] Docker esta rodando

REM 4. Limpar ambiente anterior
echo 4. Limpando ambiente anterior...
docker-compose down -v >nul 2>&1
echo [OK] Ambiente limpo

REM 5. Verificar portas
echo 5. Verificando portas necessarias...
netstat -ano | findstr :5173 >nul 2>&1
if not errorlevel 1 (
    echo [AVISO] Porta 5173 ja esta em uso!
    echo Execute: netstat -ano | findstr :5173
    echo Deseja continuar mesmo assim? (S/N)
    set /p continue=
    if /i not "!continue!"=="S" exit /b 1
) else (
    echo [OK] Porta 5173 disponivel
)

netstat -ano | findstr :5001 >nul 2>&1
if not errorlevel 1 (
    echo [AVISO] Porta 5001 ja esta em uso!
) else (
    echo [OK] Porta 5001 disponivel
)

netstat -ano | findstr :1433 >nul 2>&1
if not errorlevel 1 (
    echo [AVISO] Porta 1433 ja esta em uso!
) else (
    echo [OK] Porta 1433 disponivel
)

REM 6. Verificar espaço em disco
echo 6. Verificando espaco em disco do Docker...
docker system df

REM 7. Iniciar containers
echo.
echo 7. Iniciando containers (pode levar 2-3 minutos)...
echo.
docker-compose up --build -d

if errorlevel 1 (
    echo [ERRO] Falha ao iniciar containers!
    echo Verifique os logs com: docker-compose logs
    pause
    exit /b 1
)

REM 8. Aguardar healthchecks
echo.
echo 8. Aguardando servicos ficarem saudaveis...
echo    (SQL Server pode levar ate 2 minutos)
echo.
timeout /t 15 /nobreak >nul

REM Loop de verificação
for /L %%i in (1,1,20) do (
    docker-compose ps | findstr /C:"(healthy)" >nul 2>&1
    if not errorlevel 1 (
        echo [OK] Servicos estao saudaveis!
        goto :healthcheck_done
    )
    echo Aguardando... (tentativa %%i de 20)
    timeout /t 5 /nobreak >nul
)

:healthcheck_done

REM 9. Verificar status final
echo.
echo 9. Status final dos servicos:
echo.
docker-compose ps

REM 10. Testar endpoints
echo.
echo 10. Testando endpoints...
curl -f http://localhost:5001/health >nul 2>&1
if errorlevel 1 (
    echo [AVISO] Backend ainda nao esta respondendo
) else (
    echo [OK] Backend respondendo: http://localhost:5001
)

curl -f http://localhost:5173 >nul 2>&1
if errorlevel 1 (
    echo [AVISO] Frontend ainda nao esta respondendo
) else (
    echo [OK] Frontend respondendo: http://localhost:5173
)

REM 11. Resumo final
echo.
echo ========================================
echo   AMBIENTE PRONTO PARA APRESENTACAO!
echo ========================================
echo.
echo URLs de Acesso:
echo   Frontend:  http://localhost:5173
echo   Backend:   http://localhost:5001/swagger
echo   Health:    http://localhost:5001/health
echo.
echo Comandos Uteis:
echo   Ver logs:       docker-compose logs -f
echo   Ver status:     docker-compose ps
echo   Reiniciar:      docker-compose restart
echo   Parar tudo:     docker-compose down
echo.
echo Boa apresentacao!
echo.

pause
