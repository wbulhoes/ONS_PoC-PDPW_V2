#!/bin/bash
set -e

echo "?? Iniciando PDPW API..."

# Esperar SQL Server estar pronto (se não for InMemory)
if [ "$UseInMemoryDatabase" = "false" ]; then
  echo "? Aguardando SQL Server..."
  
  max_attempts=30
  attempt=0
  
  until curl -f http://sqlserver:1433 > /dev/null 2>&1 || [ $attempt -eq $max_attempts ]; do
    echo "   SQL Server não está pronto - tentativa $attempt/$max_attempts"
    sleep 2
    ((attempt++))
  done
  
  if [ $attempt -eq $max_attempts ]; then
    echo "??  SQL Server pode não estar pronto, mas continuando..."
  else
    echo "? SQL Server está pronto!"
  fi
fi

# Iniciar aplicação
# Migrations serão aplicadas automaticamente via Program.cs
echo "?? Iniciando aplicação..."
echo "?? Migrations e Seeder serão executados automaticamente"
exec dotnet PDPW.API.dll
