# ?? GUIA DOCKER - PDPW API

**Data:** 2025-01-20  
**Vers�o:** 1.0

---

## ?? OP��ES DISPON�VEIS

### **OP��O 1: Docker Compose Simples (Banco em Mem�ria)** ? RECOMENDADO

Usa banco de dados em mem�ria - mais r�pido e sem depend�ncias.

```sh
docker-compose up --build
```

Acessar:
- API: http://localhost:5000
- Swagger: http://localhost:5000/swagger
- Health: http://localhost:5000/health

---

### **OP��O 2: Docker Compose Completo (SQL Server)**

Usa SQL Server containerizado - para testes com persist�ncia.

```sh
docker-compose -f docker-compose.full.yml up --build
```

Acessar:
- API: http://localhost:5000
- Swagger: http://localhost:5000/swagger
- Health: http://localhost:5000/health
- SQL Server: localhost:1433 (sa / Pdpw@2024!Strong)

---

## ?? COMANDOS �TEIS

### Build e Start
```sh
# Op��o 1 (Banco em Mem�ria)
docker-compose up --build

# Op��o 2 (SQL Server)
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
# Rebuild for�ado
docker-compose up --build --force-recreate

# Rebuild apenas do backend
docker-compose up --build backend
```

---

## ?? TROUBLESHOOTING

### Erro: "port is already allocated"
```sh
# Verificar o que est� usando a porta 5000
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

### Erro: SQL Server n�o inicia
```sh
# Aumentar mem�ria do Docker Desktop
# Settings > Resources > Memory: 4GB m�nimo

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

# Se n�o existir, o arquivo foi criado nesta corre��o
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

## ?? VERIFICAR SE EST� FUNCIONANDO

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
??? docker-compose.yml              ? Simples (Banco em Mem�ria)
??? docker-compose.full.yml         ? Completo (SQL Server)
??? .dockerignore                   ? Otimiza��o de build
??? src/
    ??? PDPW.API/
        ??? Dockerfile              ? Build da API
```

---

## ?? CONFIGURA��ES

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
      - UseInMemoryDatabase=true  # Banco em mem�ria
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

## ?? QUANDO USAR CADA OP��O

### Banco em Mem�ria (docker-compose.yml)
? **Use quando:**
- Desenvolvimento r�pido
- Testes locais
- Demonstra��es
- N�o precisa de persist�ncia

? **N�o use quando:**
- Precisa manter dados entre restarts
- Quer testar migrations
- Desenvolvimento de longo prazo

### SQL Server (docker-compose.full.yml)
? **Use quando:**
- Precisa de persist�ncia
- Quer testar com dados reais
- Desenvolvimento de funcionalidades complexas
- Testes de integra��o

? **N�o use quando:**
- Quer algo r�pido
- Recursos de hardware limitados
- Apenas testando endpoints

---

## ?? PR�XIMOS PASSOS

Ap�s subir o Docker:

1. ? Acessar Swagger: http://localhost:5000/swagger
2. ? Testar endpoints
3. ? Validar que todas as 9 APIs aparecem
4. ? Executar requests de teste

---

## ?? LINKS �TEIS

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Docker Compose Docs](https://docs.docker.com/compose/)
- [SQL Server Docker](https://hub.docker.com/_/microsoft-mssql-server)

---

**Status:** ? PRONTO PARA USO  
**�ltima Atualiza��o:** 2025-01-20
