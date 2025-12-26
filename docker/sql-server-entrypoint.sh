#!/bin/bash

# ==================================================
# Script de Inicialização do SQL Server no Docker
# ==================================================
# Este script:
# 1. Inicia o SQL Server
# 2. Aguarda ele ficar pronto
# 3. Cria o banco PDPW_DB
# 4. Mantém o SQL Server rodando
# ==================================================

echo "=========================================="
echo "🚀 Iniciando SQL Server..."
echo "=========================================="

# Inicia SQL Server em background
/opt/mssql/bin/sqlservr &

# Aguarda SQL Server aceitar conexões (máximo 60 segundos)
echo "⏳ Aguardando SQL Server inicializar..."
for i in {1..60}; do
    if /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -Q "SELECT 1" &> /dev/null; then
        echo "✅ SQL Server está pronto!"
        break
    fi
    
    if [ $i -eq 60 ]; then
        echo "❌ Timeout: SQL Server não respondeu em 60 segundos"
        exit 1
    fi
    
    echo "   Tentativa $i/60..."
    sleep 1
done

# Cria o banco de dados PDPW_DB
echo ""
echo "=========================================="
echo "📦 Criando banco de dados PDPW_DB..."
echo "=========================================="

/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -Q "
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'PDPW_DB')
BEGIN
    PRINT '📦 Criando banco de dados PDPW_DB...';
    CREATE DATABASE PDPW_DB;
    PRINT '✅ Banco de dados PDPW_DB criado com sucesso!';
END
ELSE
BEGIN
    PRINT '✅ Banco de dados PDPW_DB já existe.';
END
GO

USE PDPW_DB;
GO

PRINT '🎯 Banco de dados PDPW_DB pronto para uso!';
PRINT '📊 As migrations da API vão popular os dados automaticamente.';
GO
"

if [ $? -eq 0 ]; then
    echo "✅ Banco PDPW_DB criado/verificado com sucesso!"
else
    echo "❌ Erro ao criar banco PDPW_DB"
    exit 1
fi

echo ""
echo "=========================================="
echo "✅ SQL Server inicializado com sucesso!"
echo "=========================================="
echo "📦 Banco: PDPW_DB"
echo "👤 User: sa"
echo "🔒 Password: (configurada via env)"
echo "🌐 Port: 1433"
echo "=========================================="

# Mantém o container rodando (traz o SQL Server para foreground)
wait
