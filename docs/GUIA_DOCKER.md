# ?? GUIA DOCKER - PDPW API

**Data:** 2025-01-20  
**Versão:** 1.0

---

## ?? OPÇÕES DISPONÍVEIS

### **OPÇÃO 1: Docker Compose Simples (Banco em Memória)** ? RECOMENDADO

Usa banco de dados em memória - mais rápido e sem dependências.

```sh
docker-compose up --build
```

Acessar:
- API: http://localhost:5000
- Swagger: http://localhost:5000/swagger
- Health: http://localhost:5000/health

---

### **OPÇÃO 2: Docker Compose Completo (SQL Server)**

Usa SQL Server containerizado - para testes com persistência.

```sh
docker-compose -f docker-compose.full.yml up --build
```

Acessar:
- API: http://localhost:5000
- Swagger: http://localhost:5000/swagger
- Health: http://localhost:5000/health
- SQL Server: localhost:1433 (sa / Pdpw@2024!Strong)

---

## ?? COMANDOS ÚTEIS

### Build e Start
```sh
# Opção 1 (Banco em Memória)
docker-compose up --build

# Opção 2 (SQL Server)
docker-compose -f docker-compose.full.yml up --build

# Modo detached (background)
docker-compose up -d --build
```

### Parar e Remover
```sh
# Parar containers
docker-compose down

# Parar e remover volumes
docker-compose down -v
```

### Logs
```sh
# Ver logs
docker-compose logs -f

# Logs apenas do backend
docker-compose logs -f backend

# Logs apenas do SQL Server
docker-compose -f docker-compose.full.yml logs -f sqlserver
```

### Rebuild
```sh
# Rebuild forçado
docker-compose up --build --force-recreate

# Rebuild apenas do backend
docker-compose up --build backend
```

---

## ?? TROUBLESHOOTING

### Erro: "port is already allocated"
```sh
# Verificar o que está usando a porta 5000
netstat -ano | findstr :5000

# Matar o processo (substitua PID)
taskkill /PID <numero_pid> /F

# Ou mudar a porta no docker-compose.yml
ports:
  - "5001:80"  # Mudar 5000 para 5001
```

### Erro: "no configuration file provided"
```sh
# Certifique-se de estar na raiz do projeto
cd C:\temp\_ONS_PoC-PDPW_V2

# E que o arquivo docker-compose.yml existe
dir docker-compose.yml
```

### Erro: SQL Server não inicia
```sh
# Aumentar memória do Docker Desktop
# Settings > Resources > Memory: 4GB mínimo

# Verificar logs
docker-compose -f docker-compose.full.yml logs sqlserver

# Recriar volume
docker-compose -f docker-compose.full.yml down -v
docker-compose -f docker-compose.full.yml up --build
```

### Erro: "Dockerfile not found"
```sh
# Verificar se existe
dir src\PDPW.API\Dockerfile

# Se não existir, o arquivo foi criado nesta correção
```

### Erro: Build falha
```sh
# Limpar cache do Docker
docker system prune -a

# Rebuild do zero
docker-compose build --no-cache
docker-compose up
```

---

## ?? VERIFICAR SE ESTÁ FUNCIONANDO

### 1. Verificar containers rodando
```sh
docker ps
```

Deve mostrar:
```
CONTAINER ID   IMAGE                    STATUS         PORTS
xxxxx          pdpw-backend            Up             0.0.0.0:5000->80/tcp
```

### 2. Testar Health Check
```sh
curl http://localhost:5000/health
```

Resposta esperada:
```json
{
  "status": "Healthy"
}
```

### 3. Acessar Swagger
Abrir navegador: http://localhost:5000/swagger

Deve mostrar **9 controllers** com todos os endpoints.

---

## ?? ARQUIVOS DOCKER

### Estrutura:
```
C:\temp\_ONS_PoC-PDPW_V2\
??? docker-compose.yml              ? Simples (Banco em Memória)
??? docker-compose.full.yml         ? Completo (SQL Server)
??? .dockerignore                   ? Otimização de build
??? src/
    ??? PDPW.API/
        ??? Dockerfile              ? Build da API
```

---

## ?? CONFIGURAÇÕES

### docker-compose.yml (Simples)
```yaml
services:
  backend:
    build:
      context: .
      dockerfile: src/PDPW.API/Dockerfile
    ports:
      - "5000:80"
    environment:
      - UseInMemoryDatabase=true  # Banco em memória
```

### docker-compose.full.yml (Completo)
```yaml
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    # ...
  
  backend:
    build:
      context: .
      dockerfile: src/PDPW.API/Dockerfile
    depends_on:
      - sqlserver
    environment:
      - UseInMemoryDatabase=false  # SQL Server
      - ConnectionStrings__DefaultConnection=Server=sqlserver;...
```

---

## ?? QUANDO USAR CADA OPÇÃO

### Banco em Memória (docker-compose.yml)
? **Use quando:**
- Desenvolvimento rápido
- Testes locais
- Demonstrações
- Não precisa de persistência

? **Não use quando:**
- Precisa manter dados entre restarts
- Quer testar migrations
- Desenvolvimento de longo prazo

### SQL Server (docker-compose.full.yml)
? **Use quando:**
- Precisa de persistência
- Quer testar com dados reais
- Desenvolvimento de funcionalidades complexas
- Testes de integração

? **Não use quando:**
- Quer algo rápido
- Recursos de hardware limitados
- Apenas testando endpoints

---

## ?? PRÓXIMOS PASSOS

Após subir o Docker:

1. ? Acessar Swagger: http://localhost:5000/swagger
2. ? Testar endpoints
3. ? Validar que todas as 9 APIs aparecem
4. ? Executar requests de teste

---

## ?? LINKS ÚTEIS

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Docker Compose Docs](https://docs.docker.com/compose/)
- [SQL Server Docker](https://hub.docker.com/_/microsoft-mssql-server)

---

**Status:** ? PRONTO PARA USO  
**Última Atualização:** 2025-01-20
