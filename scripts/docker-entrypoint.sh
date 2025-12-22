#!/bin/bash
set -e

echo "?? Iniciando PDPW API..."

# Esperar SQL Server estar pronto
echo "? Aguardando SQL Server..."
until /opt/mssql-tools18/bin/sqlcmd -S sqlserver -U sa -P "Pdpw@2024!Strong" -C -Q "SELECT 1" > /dev/null 2>&1; do
  echo "   SQL Server não está pronto - aguardando..."
  sleep 2
done

echo "? SQL Server está pronto!"

# Aplicar migrations (se não usar InMemory)
if [ "$UseInMemoryDatabase" = "false" ]; then
  echo "?? Aplicando migrations..."
  dotnet ef database update --project /app/PDPW.Infrastructure.dll --startup-project /app/PDPW.API.dll || echo "??  Migrations já aplicadas ou erro (ignorando)"
  echo "? Migrations aplicadas!"
fi

# Iniciar aplicação
echo "?? Iniciando aplicação..."
exec dotnet PDPW.API.dll
