# ?? GUIA DOCKER - PDPW POC

## ?? Pré-requisitos

- Docker Desktop instalado e rodando
- Docker Compose (incluído no Docker Desktop)
- Mínimo 4GB RAM disponível
- 10GB espaço em disco

---

## ?? INÍCIO RÁPIDO

### **1. Clonar o repositório**
```bash
git clone https://github.com/wbulhoes/ONS_PoC-PDPW_V2.git
cd ONS_PoC-PDPW_V2
```

### **2. Iniciar todos os serviços**
```bash
docker-compose up -d
```

### **3. Acessar a aplicação**
- **Swagger API:** http://localhost:5001/swagger
- **SQL Server:** localhost:1433 (sa / Pdpw@2024!Strong)

### **4. Parar os serviços**
```bash
docker-compose down
```

---

## ?? SERVIÇOS DISPONÍVEIS

### **Docker Compose Principal** (`docker-compose.yml`)
Inclui **Backend + SQL Server** com persistência.

```yaml
Serviços:
  - pdpw-sqlserver (SQL Server 2022)
    Porta: 1433
    Volume: pdpw_sqldata
    
  - pdpw-backend (.NET 8 API)
    Porta: 5001 (HTTP)
    Porta: 5002 (HTTPS)
```

### **Docker Compose Completo** (`docker-compose.full.yml`)
Mesma configuração, mantido para compatibilidade.

---

## ?? COMANDOS ÚTEIS

### **Iniciar serviços**
```bash
# Iniciar em modo detached (background)
docker-compose up -d

# Iniciar e acompanhar logs
docker-compose up

# Iniciar apenas um serviço
docker-compose up -d sqlserver
docker-compose up -d backend
```

### **Ver logs**
```bash
# Logs de todos os serviços
docker-compose logs -f

# Logs apenas do backend
docker-compose logs -f backend

# Logs apenas do SQL Server
docker-compose logs -f sqlserver

# Últimas 100 linhas
docker-compose logs --tail=100 backend
```

### **Parar serviços**
```bash
# Parar sem remover containers
docker-compose stop

# Parar e remover containers
docker-compose down

# Parar, remover containers E volumes (CUIDADO: apaga dados!)
docker-compose down -v
```

### **Reconstruir imagens**
```bash
# Rebuild apenas do backend
docker-compose build backend

# Rebuild de tudo
docker-compose build

# Rebuild sem cache
docker-compose build --no-cache

# Rebuild e reiniciar
docker-compose up -d --build
```

### **Executar comandos dentro do container**
```bash
# Shell no backend
docker-compose exec backend bash

# Shell no SQL Server
docker-compose exec sqlserver bash

# Executar migrations manualmente
docker-compose exec backend dotnet ef database update --project /app/PDPW.Infrastructure.dll
```

---

## ??? GERENCIAMENTO DO BANCO DE DADOS

### **Conectar via SQL Server Management Studio (SSMS)**
```
Server: localhost,1433
Authentication: SQL Server Authentication
Login: sa
Password: Pdpw@2024!Strong
```

### **Conectar via Azure Data Studio**
```
Server: localhost,1433
Authentication Type: SQL Login
User name: sa
Password: Pdpw@2024!Strong
Database: PDPW_DB
```

### **Conectar via sqlcmd (dentro do container)**
```bash
docker-compose exec sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "Pdpw@2024!Strong" -C
```

### **Backup do banco de dados**
```bash
# Criar backup
docker-compose exec sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "Pdpw@2024!Strong" -C \
  -Q "BACKUP DATABASE PDPW_DB TO DISK = N'/var/opt/mssql/PDPW_DB.bak' WITH FORMAT"

# Copiar backup para host
docker cp pdpw-sqlserver:/var/opt/mssql/PDPW_DB.bak ./backups/
```

### **Restaurar backup**
```bash
# Copiar backup para container
docker cp ./backups/PDPW_DB.bak pdpw-sqlserver:/var/opt/mssql/

# Restaurar
docker-compose exec sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "Pdpw@2024!Strong" -C \
  -Q "RESTORE DATABASE PDPW_DB FROM DISK = N'/var/opt/mssql/PDPW_DB.bak' WITH REPLACE"
```

---

## ?? PERSISTÊNCIA DE DADOS

### **Volumes Docker**
Os dados do SQL Server são armazenados em um volume Docker nomeado:

```bash
# Listar volumes
docker volume ls

# Inspecionar volume
docker volume inspect pdpw_sqldata

# Localização física (Windows com WSL)
\\wsl$\docker-desktop-data\data\docker\volumes\pdpw_sqldata\_data
```

### **Backup de Volume**
```bash
# Criar backup do volume
docker run --rm \
  -v pdpw_sqldata:/data \
  -v $(pwd)/backups:/backup \
  alpine tar czf /backup/pdpw_sqldata_backup.tar.gz /data

# Restaurar backup do volume
docker run --rm \
  -v pdpw_sqldata:/data \
  -v $(pwd)/backups:/backup \
  alpine tar xzf /backup/pdpw_sqldata_backup.tar.gz -C /
```

---

## ?? TROUBLESHOOTING

### **Problema: Backend não inicia**
```bash
# Ver logs detalhados
docker-compose logs backend

# Verificar se SQL Server está saudável
docker-compose ps

# Reiniciar apenas backend
docker-compose restart backend
```

### **Problema: SQL Server não está pronto**
```bash
# Verificar health check
docker-compose ps

# Ver logs do SQL Server
docker-compose logs sqlserver

# Esperar mais tempo (pode levar até 30s)
```

### **Problema: Migrations não são aplicadas**
```bash
# Aplicar manualmente
docker-compose exec backend dotnet ef database update \
  --project /app/PDPW.Infrastructure.dll \
  --startup-project /app/PDPW.API.dll
```

### **Problema: Porta 1433 já em uso**
```bash
# Verificar o que está usando a porta
netstat -ano | findstr :1433

# Parar SQL Server local
net stop MSSQL$SQLEXPRESS

# Ou mudar porta no docker-compose.yml
ports:
  - "1434:1433"  # Usar porta 1434 no host
```

### **Problema: Containers não param**
```bash
# Forçar parada
docker-compose kill

# Remover containers órfãos
docker-compose down --remove-orphans

# Limpar tudo (CUIDADO!)
docker system prune -a
```

---

## ?? TESTES E VALIDAÇÃO

### **1. Verificar saúde dos containers**
```bash
docker-compose ps

# Saída esperada:
# NAME              STATUS             PORTS
# pdpw-backend      Up (healthy)       0.0.0.0:5001->80/tcp
# pdpw-sqlserver    Up (healthy)       0.0.0.0:1433->1433/tcp
```

### **2. Testar API**
```bash
# Health check
curl http://localhost:5001/health

# Swagger
curl http://localhost:5001/swagger/index.html

# Listar empresas
curl http://localhost:5001/api/empresas
```

### **3. Testar banco de dados**
```bash
# Contar registros
docker-compose exec sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "Pdpw@2024!Strong" -C \
  -Q "SELECT COUNT(*) FROM PDPW_DB.dbo.Empresas"
```

---

## ?? MONITORAMENTO

### **Recursos dos containers**
```bash
# Ver uso de CPU/Memória
docker stats

# Ver uso de disco
docker system df

# Ver logs em tempo real
docker-compose logs -f --tail=50
```

### **Logs estruturados**
Os logs da aplicação seguem formato estruturado e incluem:
- Timestamp
- Nível de log (Info, Warning, Error)
- Contexto da operação
- Dados sensíveis (apenas em Development)

---

## ?? CI/CD e Produção

### **Build de imagem para produção**
```bash
# Build otimizado
docker build -f src/PDPW.API/Dockerfile \
  -t pdpw-backend:latest \
  -t pdpw-backend:1.0.0 \
  --build-arg BUILD_CONFIGURATION=Release \
  .

# Push para registry
docker tag pdpw-backend:latest seu-registry/pdpw-backend:latest
docker push seu-registry/pdpw-backend:latest
```

### **Variáveis de ambiente produção**
```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Production
  - UseInMemoryDatabase=false
  - EnableSensitiveDataLogging=false
  - ConnectionStrings__DefaultConnection=${SQL_CONNECTION_STRING}
```

---

## ?? CHECKLIST DE DEPLOY

- [ ] Docker e Docker Compose instalados
- [ ] Portas 1433 e 5001 disponíveis
- [ ] Variáveis de ambiente configuradas
- [ ] Volumes criados
- [ ] Build da imagem bem-sucedido
- [ ] Containers iniciados e saudáveis
- [ ] Migrations aplicadas
- [ ] Dados populados (via Seeder)
- [ ] APIs respondendo
- [ ] Swagger acessível
- [ ] Banco de dados acessível

---

## ?? LINKS ÚTEIS

- **Swagger API:** http://localhost:5001/swagger
- **SQL Server:** localhost:1433
- **Documentação:** [README.md](../README.md)
- **Status POC:** [docs/POC_STATUS_E_ROADMAP.md](../docs/POC_STATUS_E_ROADMAP.md)

---

## ?? SUPORTE

Para problemas com Docker:
1. Verificar logs: `docker-compose logs`
2. Verificar health: `docker-compose ps`
3. Consultar documentação: https://docs.docker.com/
4. Issues no GitHub: https://github.com/wbulhoes/ONS_PoC-PDPW_V2/issues

---

**? DOCKER CONFIGURADO E PRONTO PARA USO!**

**Última atualização:** 22/12/2024
