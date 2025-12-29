# 🚀 COMANDOS RÁPIDOS - PDPw v2.0

## ⚡ Inicialização Rápida

### 🔵 Backend (.NET 8)
```bash
# Terminal 1 - Iniciar API
cd src/PDPW.API
dotnet run
```
✅ **Swagger:** http://localhost:5001/swagger  
✅ **API:** http://localhost:5001/api

---

### 🟢 Frontend (React)
```bash
# Terminal 2 - Iniciar Frontend
cd frontend
npm run dev
```
✅ **App:** http://localhost:5173

---

### 🐳 Docker (Sistema Completo)
```bash
# Iniciar tudo de uma vez
docker-compose up -d

# Ver logs
docker-compose logs -f api

# Parar tudo
docker-compose down
```

---

## 🛠️ Desenvolvimento

### Backend

```bash
# Compilar
dotnet build

# Executar testes
dotnet test

# Verificar erros
dotnet build --no-restore

# Criar migração
dotnet ef migrations add NomeMigracao --project src/PDPW.Infrastructure --startup-project src/PDPW.API

# Aplicar migrações
dotnet ef database update --project src/PDPW.Infrastructure --startup-project src/PDPW.API

# Restaurar dependências
dotnet restore
```

### Frontend

```bash
# Instalar dependências
npm install

# Executar dev
npm run dev

# Build de produção
npm run build

# Preview de produção
npm run preview

# Lint
npm run lint

# Type check
npx tsc --noEmit
```

---

## 🗄️ Banco de Dados

### SQL Server (Docker)

```bash
# Conectar via CLI
docker exec -it pdpw-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourStrong!Passw0rd'

# Backup
docker exec pdpw-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourStrong!Passw0rd' -Q "BACKUP DATABASE [PDPw] TO DISK = '/var/opt/mssql/backup/PDPw.bak'"

# Restore
docker exec pdpw-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourStrong!Passw0rd' -Q "RESTORE DATABASE [PDPw] FROM DISK = '/var/opt/mssql/backup/PDPw.bak' WITH REPLACE"

# Ver databases
docker exec pdpw-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourStrong!Passw0rd' -Q "SELECT name FROM sys.databases"
```

### Dados de Teste

```bash
# Inserir dados de teste (se ainda não inseridos)
cd scripts
dotnet fsi InsertTestData.fsx
```

---

## 🧪 Testes

### Backend

```bash
# Todos os testes
dotnet test

# Testes específicos
dotnet test --filter "FullyQualifiedName~UsinasControllerTests"

# Com coverage
dotnet test /p:CollectCoverage=true

# Verbose
dotnet test --logger "console;verbosity=detailed"
```

### Frontend

```bash
# (Se configurar Jest no futuro)
npm test
npm test -- --coverage
```

---

## 📦 Build & Deploy

### Backend

```bash
# Build Release
dotnet build --configuration Release

# Publish
dotnet publish -c Release -o ./publish

# Docker build
docker build -t pdpw-api:v2.0 .

# Docker run
docker run -d -p 5001:8080 --name pdpw-api pdpw-api:v2.0
```

### Frontend

```bash
# Build produção
npm run build

# Testar build
npm run preview

# Deploy (exemplo com serve)
npm install -g serve
serve -s dist -p 5173
```

---

## 🔍 Debug & Logs

### Backend

```bash
# Ver logs da aplicação
dotnet run --verbosity detailed

# Watch mode (auto-reload)
dotnet watch run

# Debug específico
dotnet run --environment Development
```

### Frontend

```bash
# Dev mode com debug
npm run dev -- --debug

# Ver dependências
npm list

# Limpar cache
npm cache clean --force
rm -rf node_modules package-lock.json
npm install
```

### Docker

```bash
# Ver logs específicos
docker-compose logs -f api
docker-compose logs -f sqlserver

# Ver status
docker-compose ps

# Ver uso de recursos
docker stats

# Entrar no container
docker exec -it pdpw-api bash
docker exec -it pdpw-sqlserver bash

# Reiniciar serviço específico
docker-compose restart api
docker-compose restart sqlserver
```

---

## 🔧 Manutenção

### Limpar Ambiente

```bash
# Backend
dotnet clean
rm -rf bin obj

# Frontend
rm -rf node_modules dist .vite
npm install

# Docker
docker-compose down -v
docker system prune -a
```

### Atualizar Dependências

```bash
# Backend
dotnet list package --outdated
dotnet add package NomePacote

# Frontend
npm outdated
npm update
npm install package-name@latest
```

---

## 📊 Verificações

### Health Checks

```bash
# Backend
curl http://localhost:5001/health

# Database
curl http://localhost:5001/health/db

# Swagger
curl http://localhost:5001/swagger/index.html
```

### Endpoints Rápidos

```bash
# Dashboard
curl http://localhost:5001/api/dashboard/resumo

# Usinas
curl http://localhost:5001/api/usinas

# Semanas PMO
curl http://localhost:5001/api/semanaspmo/atual

# Dados Energéticos
curl http://localhost:5001/api/dadosenergeticos
```

---

## 🎯 Atalhos para Testes Rápidos

### Testar Etapa 1 - Dados Energéticos
```bash
# POST
curl -X POST http://localhost:5001/api/dadosenergeticos \
  -H "Content-Type: application/json" \
  -d '{"dataReferencia":"2025-01-15","codigoUsina":"ITAI","producaoMWh":1200,"capacidadeDisponivel":1400,"status":"CONFIRMADO"}'

# GET
curl http://localhost:5001/api/dadosenergeticos
```

### Testar Etapa 7 - Ofertas Exportação
```bash
# GET pendentes
curl http://localhost:5001/api/ofertas-exportacao/pendentes

# POST nova oferta
curl -X POST http://localhost:5001/api/ofertas-exportacao \
  -H "Content-Type: application/json" \
  -d '{"usinaId":1,"semanaPmoId":1,"dataOferta":"2025-01-15","potenciaOfertadaMW":500,"precoOfertaRS":250,"periodoInicio":"2025-01-16T00:00:00","periodoFim":"2025-01-16T23:59:59"}'
```

---

## 🌐 URLs Importantes

| Serviço | URL | Descrição |
|---------|-----|-----------|
| **Frontend** | http://localhost:5173 | Aplicação React |
| **API** | http://localhost:5001/api | Backend REST |
| **Swagger** | http://localhost:5001/swagger | Documentação interativa |
| **Health** | http://localhost:5001/health | Status da API |

---

## 📂 Navegação Rápida

```bash
# Ir para backend
cd src/PDPW.API

# Ir para frontend
cd frontend

# Ir para testes
cd tests/PDPW.IntegrationTests

# Ir para scripts
cd scripts

# Voltar para raiz
cd C:\temp\_ONS_PoC-PDPW_V2
```

---

## 🔥 Comandos de Emergência

### Resetar Tudo
```bash
# Parar tudo
docker-compose down -v
killall dotnet
npx kill-port 5001
npx kill-port 5173

# Limpar caches
dotnet clean
rm -rf frontend/node_modules frontend/dist

# Reinstalar
dotnet restore
cd frontend && npm install
```

### Recriar Banco
```bash
# Dropar e recriar
docker-compose down -v
docker volume prune
docker-compose up -d sqlserver
dotnet ef database update --project src/PDPW.Infrastructure --startup-project src/PDPW.API
```

---

## ✅ Verificação Final

```bash
# 1. Backend rodando?
curl http://localhost:5001/health

# 2. Frontend rodando?
curl http://localhost:5173

# 3. Swagger acessível?
curl http://localhost:5001/swagger/index.html

# 4. Dados no banco?
curl http://localhost:5001/api/dashboard/resumo

# Se todos retornarem 200, está tudo OK! ✅
```

---

## 📚 Documentação Relacionada

- **Guia Completo:** `FRONTEND_COMPLETO_9_ETAPAS.md`
- **Checklist:** `CHECKLIST_VALIDACAO.md`
- **README Frontend:** `frontend/README.md`
- **Guia Rápido:** `frontend/GUIA_RAPIDO.md`

---

## 🎯 Workflow Típico de Desenvolvimento

```bash
# 1. Abrir projeto
cd C:\temp\_ONS_PoC-PDPW_V2

# 2. Iniciar backend
cd src/PDPW.API && dotnet watch run

# 3. Abrir nova aba do terminal

# 4. Iniciar frontend
cd frontend && npm run dev

# 5. Abrir navegador
# Frontend: http://localhost:5173
# Swagger: http://localhost:5001/swagger

# 6. Desenvolver e testar

# 7. Commitar
git add .
git commit -m "feat: nova funcionalidade"
git push
```

---

**PDPw v2.0 - Sistema de Programação Diária**  
**Operador Nacional do Sistema Elétrico - ONS**  
© 2025 - Todos os direitos reservados
