#!/bin/bash

echo "========================================"
echo "  SETUP FRONTEND PDPw"
echo "  Configuração Automática"
echo "========================================"
echo ""

# Navegar para o diretório frontend
cd frontend

echo "[1/4] Verificando Node.js..."
if ! command -v node &> /dev/null; then
    echo "ERRO: Node.js não encontrado!"
    echo "Por favor, instale Node.js 18+ de https://nodejs.org"
    exit 1
fi
node --version
echo "OK!"
echo ""

echo "[2/4] Instalando dependências..."
npm install
if [ $? -ne 0 ]; then
    echo "ERRO ao instalar dependências!"
    exit 1
fi
echo "OK!"
echo ""

echo "[3/4] Configurando variáveis de ambiente..."
if [ ! -f .env ]; then
    cp .env.example .env
    echo "Arquivo .env criado!"
else
    echo "Arquivo .env já existe."
fi
echo "OK!"
echo ""

echo "[4/4] Verificando TypeScript..."
npm run type-check
if [ $? -ne 0 ]; then
    echo "AVISO: Erros de TypeScript encontrados"
    echo "Isso é normal se for a primeira vez"
else
    echo "OK!"
fi
echo ""

echo "========================================"
echo "  SETUP CONCLUÍDO COM SUCESSO!"
echo "========================================"
echo ""
echo "Para iniciar o frontend:"
echo "  npm run dev"
echo ""
echo "Frontend estará disponível em:"
echo "  http://localhost:5173"
echo ""
echo "Certifique-se de que o backend está rodando:"
echo "  cd ../src/PDPW.API"
echo "  dotnet run"
echo ""
