@echo off
REM Script de verificação rápida do ambiente PDPw

echo.
echo ========================================
echo PDPw - Verificacao Rapida
echo ========================================
echo.

REM Verificar containers
echo [1] Status dos Containers:
docker-compose ps
echo.

REM Verificar saúde
echo [2] Health Checks:
docker ps --format "table {{.Names}}\t{{.Status}}"
echo.

REM Testar URLs
echo [3] Testando Endpoints:

curl -s http://localhost:5173 >nul 2>&1
if errorlevel 1 (
    echo [X] Frontend: OFFLINE
) else (
    echo [OK] Frontend: http://localhost:5173
)

curl -s http://localhost:5001/health >nul 2>&1
if errorlevel 1 (
    echo [X] Backend: OFFLINE
) else (
    echo [OK] Backend: http://localhost:5001
)

curl -s http://localhost:5001/swagger >nul 2>&1
if errorlevel 1 (
    echo [X] Swagger: OFFLINE
) else (
    echo [OK] Swagger: http://localhost:5001/swagger
)

echo.

REM Verificar banco
echo [4] Banco de Dados:
docker exec pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong" -C -Q "SELECT @@VERSION" 2>nul
if errorlevel 1 (
    echo [X] SQL Server: OFFLINE ou nao acessivel
) else (
    echo [OK] SQL Server: ONLINE
)

echo.
echo ========================================
echo Verificacao Completa!
echo ========================================
echo.

pause
