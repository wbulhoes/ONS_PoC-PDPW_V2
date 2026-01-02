#!/bin/bash

# Script de preparação para apresentação do PDPw
# Executa todas as verificações e inicia o ambiente

set -e

echo "🚀 PDPw - Script de Preparação para Apresentação"
echo "================================================"
echo ""

# Cores para output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Função para print colorido
print_success() {
    echo -e "${GREEN}✅ $1${NC}"
}

print_warning() {
    echo -e "${YELLOW}⚠️  $1${NC}"
}

print_error() {
    echo -e "${RED}❌ $1${NC}"
}

# 1. Verificar Docker
echo "1️⃣  Verificando Docker..."
if ! docker --version > /dev/null 2>&1; then
    print_error "Docker não encontrado! Instale o Docker Desktop."
    exit 1
fi
print_success "Docker instalado"

# 2. Verificar Docker Compose
echo "2️⃣  Verificando Docker Compose..."
if ! docker-compose --version > /dev/null 2>&1; then
    print_error "Docker Compose não encontrado!"
    exit 1
fi
print_success "Docker Compose instalado"

# 3. Verificar se Docker está rodando
echo "3️⃣  Verificando se Docker está rodando..."
if ! docker info > /dev/null 2>&1; then
    print_error "Docker não está rodando! Inicie o Docker Desktop."
    exit 1
fi
print_success "Docker está rodando"

# 4. Limpar ambiente anterior
echo "4️⃣  Limpando ambiente anterior..."
print_warning "Parando containers existentes..."
docker-compose down -v > /dev/null 2>&1 || true
print_success "Ambiente limpo"

# 5. Verificar portas
echo "5️⃣  Verificando portas necessárias..."
check_port() {
    if lsof -Pi :$1 -sTCP:LISTEN -t > /dev/null 2>&1; then
        print_error "Porta $1 já está em uso!"
        echo "   Execute: lsof -i :$1 para ver qual processo"
        return 1
    else
        print_success "Porta $1 disponível"
        return 0
    fi
}

if [[ "$OSTYPE" == "darwin"* ]] || [[ "$OSTYPE" == "linux-gnu"* ]]; then
    check_port 1433
    check_port 5001
    check_port 5173
else
    print_warning "Verificação de portas não suportada no Windows via Git Bash"
    print_warning "Execute manualmente: netstat -ano | findstr :5173"
fi

# 6. Verificar espaço em disco
echo "6️⃣  Verificando espaço em disco..."
DISK_USAGE=$(docker system df --format "{{.Type}}\t{{.Size}}" | grep "Images" | awk '{print $2}')
print_warning "Uso atual do Docker: $DISK_USAGE"

# 7. Iniciar containers
echo "7️⃣  Iniciando containers (isso pode levar 2-3 minutos)..."
echo ""
docker-compose up --build -d

# 8. Aguardar healthchecks
echo ""
echo "8️⃣  Aguardando serviços ficarem saudáveis..."
echo "   (SQL Server pode levar até 2 minutos)"
echo ""

sleep 10

# Verificar status
for i in {1..30}; do
    HEALTHY=$(docker-compose ps | grep -c "(healthy)" || true)
    RUNNING=$(docker-compose ps | grep -c "Up" || true)
    
    echo -ne "\r   Containers rodando: $RUNNING/3 | Saudáveis: $HEALTHY/2   "
    
    if [ "$HEALTHY" -eq 2 ]; then
        echo ""
        print_success "Todos os serviços estão saudáveis!"
        break
    fi
    
    sleep 5
done

echo ""

# 9. Verificar status final
echo "9️⃣  Status final dos serviços:"
echo ""
docker-compose ps
echo ""

# 10. Testar endpoints
echo "🔟 Testando endpoints..."

# Backend Health
if curl -f http://localhost:5001/health > /dev/null 2>&1; then
    print_success "Backend respondendo: http://localhost:5001"
else
    print_warning "Backend ainda não está respondendo (aguarde mais alguns segundos)"
fi

# Frontend
if curl -f http://localhost:5173 > /dev/null 2>&1; then
    print_success "Frontend respondendo: http://localhost:5173"
else
    print_warning "Frontend ainda não está respondendo (aguarde mais alguns segundos)"
fi

# 11. Resumo final
echo ""
echo "================================================"
echo "✨ AMBIENTE PRONTO PARA APRESENTAÇÃO! ✨"
echo "================================================"
echo ""
echo "📱 URLs de Acesso:"
echo "   Frontend:  http://localhost:5173"
echo "   Backend:   http://localhost:5001/swagger"
echo "   Health:    http://localhost:5001/health"
echo ""
echo "📊 Comandos Úteis:"
echo "   Ver logs:       docker-compose logs -f"
echo "   Ver status:     docker-compose ps"
echo "   Reiniciar:      docker-compose restart"
echo "   Parar tudo:     docker-compose down"
echo ""
echo "🎬 Boa apresentação! 🚀"
echo ""
