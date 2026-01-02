# 🐳 Guia Docker para Apresentação - PDPw

## 📋 Pré-requisitos

- Docker Desktop instalado e rodando
- Docker Compose instalado
- Portas disponíveis: `1433` (SQL Server), `5001` (Backend), `5173` (Frontend)

---

## 🚀 INÍCIO RÁPIDO (Para Apresentação)

### 1. Limpar ambiente anterior (se existir)
```bash
docker-compose down -v
docker system prune -af --volumes
```

### 2. Iniciar todo o ambiente
```bash
docker-compose up --build -d
```

### 3. Aguardar inicialização (~2-3 minutos)
```bash
# Verificar status dos containers
docker-compose ps

# Ver logs em tempo real
docker-compose logs -f
```

### 4. Acessar aplicação
- **Frontend:** http://localhost:5173
- **Backend API:** http://localhost:5001/swagger
- **Health Check:** http://localhost:5001/health

---

## 📊 VERIFICAÇÃO DE SAÚDE

### Verificar se todos os serviços estão rodando
```bash
# Status resumido
docker-compose ps

# Verificar saúde dos containers
docker ps --format "table {{.Names}}\t{{.Status}}"
```

### Saída esperada:
```
NAME              STATUS
pdpw-frontend     Up About a minute
pdpw-backend      Up About a minute (healthy)
pdpw-sqlserver    Up About 2 minutes (healthy)
```

---

## 🔍 COMANDOS DE DIAGNÓSTICO

### Ver logs de um serviço específico
```bash
# Frontend
docker-compose logs -f frontend

# Backend
docker-compose logs -f backend

# SQL Server
docker-compose logs -f sqlserver
```

### Verificar conectividade do banco
```bash
docker exec -it pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "Pdpw@2024!Strong" -C \
  -Q "SELECT name FROM sys.databases"
```

### Acessar terminal de um container
```bash
# Backend
docker exec -it pdpw-backend bash

# Frontend
docker exec -it pdpw-frontend sh

# SQL Server
docker exec -it pdpw-sqlserver bash
```

---

## 🎯 CENÁRIOS DE APRESENTAÇÃO

### Cenário 1: Demo Completo (Recomendado)
```bash
# 1. Iniciar ambiente limpo
docker-compose down -v && docker-compose up --build -d

# 2. Aguardar 2-3 minutos

# 3. Abrir navegador em http://localhost:5173

# 4. Mostrar funcionalidades:
#    - Login/Splash
#    - Menu Consultas (29 páginas)
#    - Upload/Download
#    - Cadastros
#    - DESSEM
```

### Cenário 2: Reinício Rápido (Se já rodou antes)
```bash
# Apenas reiniciar (mantém dados)
docker-compose restart

# Ou parar e iniciar novamente
docker-compose stop
docker-compose start
```

### Cenário 3: Resetar Dados do Banco
```bash
# Parar e remover volumes
docker-compose down -v

# Iniciar novamente (banco vazio)
docker-compose up -d
```

---

## 🛠️ SOLUÇÃO DE PROBLEMAS

### Problema: Frontend não carrega
```bash
# Verificar logs
docker-compose logs frontend

# Rebuild do frontend
docker-compose up --build frontend -d
```

### Problema: Backend não responde
```bash
# Verificar se banco está saudável
docker-compose ps sqlserver

# Verificar logs do backend
docker-compose logs backend

# Rebuild do backend
docker-compose up --build backend -d
```

### Problema: Banco de dados não conecta
```bash
# Verificar se SQL Server está rodando
docker exec pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "Pdpw@2024!Strong" -C -Q "SELECT @@VERSION"

# Recriar banco
docker-compose down -v
docker-compose up sqlserver -d
```

### Problema: Portas em uso
```bash
# Windows - Verificar porta 5173
netstat -ano | findstr :5173

# Matar processo (Windows)
taskkill /PID <PID> /F

# Linux/Mac - Verificar porta 5173
lsof -i :5173

# Matar processo (Linux/Mac)
kill -9 <PID>
```

---

## 📝 CHECKLIST PRÉ-APRESENTAÇÃO

### 30 minutos antes
- [ ] Fechar todos os containers antigos: `docker-compose down -v`
- [ ] Limpar Docker: `docker system prune -af --volumes`
- [ ] Verificar espaço em disco: `docker system df`
- [ ] Garantir que portas estão livres: `netstat -ano | findstr :5173`

### 15 minutos antes
- [ ] Iniciar ambiente: `docker-compose up --build -d`
- [ ] Aguardar todos os healthchecks passarem: `docker-compose ps`
- [ ] Acessar http://localhost:5173 e verificar carregamento
- [ ] Testar navegação em 2-3 páginas

### 5 minutos antes
- [ ] Abrir navegador em aba anônima (sem cache)
- [ ] Abrir Swagger em outra aba: http://localhost:5001/swagger
- [ ] Preparar terminal com `docker-compose logs -f` rodando
- [ ] Ter Docker Desktop aberto mostrando containers

---

## 🎬 ROTEIRO DE DEMONSTRAÇÃO

### Parte 1: Arquitetura (2 min)
```bash
# Mostrar containers rodando
docker-compose ps

# Mostrar Docker Desktop com os 3 containers
```
**Falar:**
- "Temos 3 containers: SQL Server, Backend .NET 8, Frontend React"
- "Tudo orquestrado com Docker Compose"
- "Healthchecks garantem inicialização ordenada"

### Parte 2: Frontend (5 min)
**Abrir:** http://localhost:5173

**Demonstrar:**
1. Splash/Login
2. Menu Consultas → Escolher 2-3 consultas
3. Mostrar filtros e grid
4. Menu Ferramentas → Upload/Download
5. Menu Cadastros → Empresas/Usinas

**Falar:**
- "81 páginas implementadas (100%)"
- "React + TypeScript + Material-UI"
- "Template reutilizável (BaseQueryPage)"

### Parte 3: Backend (3 min)
**Abrir:** http://localhost:5001/swagger

**Demonstrar:**
1. Endpoints de Consulta
2. Endpoints de Cadastro
3. Health Check
4. Executar 1-2 requests

**Falar:**
- "Backend .NET 8 com Clean Architecture"
- "Swagger para documentação automática"
- "Repository Pattern + AutoMapper"

### Parte 4: Banco de Dados (2 min)
```bash
# Executar query no SQL Server
docker exec -it pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "Pdpw@2024!Strong" -C \
  -Q "SELECT COUNT(*) AS TotalEmpresas FROM Empresas"
```

**Falar:**
- "SQL Server 2022 containerizado"
- "Migrations automáticas via EF Core"
- "Dados persistidos em volume Docker"

---

## 📊 DADOS DE DEMONSTRAÇÃO

### Endpoints de teste disponíveis
- `GET /api/empresas` - Listar empresas
- `GET /api/usinas` - Listar usinas
- `GET /api/consulta/carga` - Consulta de carga
- `GET /health` - Health check

### Credenciais
- **Banco de dados:**
  - Servidor: `localhost,1433`
  - Usuário: `sa`
  - Senha: `Pdpw@2024!Strong`

---

## 🔄 COMANDOS ÚTEIS DURANTE APRESENTAÇÃO

### Mostrar logs em tempo real
```bash
docker-compose logs -f
```

### Verificar uso de recursos
```bash
docker stats
```

### Reiniciar um serviço específico
```bash
docker-compose restart frontend
docker-compose restart backend
```

### Parar tudo ao final
```bash
docker-compose down
```

### Limpar tudo (pós-apresentação)
```bash
docker-compose down -v
docker system prune -af --volumes
```

---

## 📱 ACESSO REMOTO (Opcional)

Se precisar demonstrar em outra máquina na mesma rede:

1. Descobrir IP da máquina host:
```bash
# Windows
ipconfig

# Linux/Mac
ifconfig
```

2. Acessar de outro dispositivo:
- Frontend: `http://<IP-HOST>:5173`
- Backend: `http://<IP-HOST>:5001`

---

## ⚠️ AVISOS IMPORTANTES

### Antes da apresentação
- ✅ Testar TUDO pelo menos 1 hora antes
- ✅ Ter plano B: slides ou vídeo gravado
- ✅ Garantir internet estável (se precisar baixar imagens)
- ✅ Desabilitar antivírus se causar problemas

### Durante apresentação
- ✅ Usar aba anônima (sem cache/extensões)
- ✅ Zoom no navegador para melhor visualização
- ✅ Preparar tabs importantes previamente
- ✅ Ter terminal com logs visível

### Após apresentação
- ✅ Fazer backup dos logs: `docker-compose logs > apresentacao.log`
- ✅ Exportar métricas: `docker stats --no-stream > metricas.txt`
- ✅ Limpar ambiente: `docker-compose down -v`

---

## 🎓 PERGUNTAS FREQUENTES

**P: Quanto tempo leva para iniciar?**
R: ~2-3 minutos (SQL Server é o mais lento)

**P: Precisa de internet?**
R: Apenas primeira vez (download de imagens). Depois funciona offline.

**P: Posso mudar as portas?**
R: Sim, edite `docker-compose.yml` nas seções `ports:`

**P: Como adicionar dados de exemplo?**
R: Execute scripts SQL via `docker exec` ou use endpoints POST do backend

**P: E se der erro de memória?**
R: Aumente memória do Docker Desktop para 4GB+

---

## 📞 SUPORTE RÁPIDO

### Se algo der errado durante apresentação:

1. **Mantenha a calma** 😌
2. Mostre os **slides** enquanto investiga
3. Verifique **logs**: `docker-compose logs -f`
4. **Reinicie** o serviço problemático: `docker-compose restart <serviço>`
5. Em último caso: mostre **vídeo/print** do sistema funcionando

---

**Boa apresentação! 🚀**

*Criado em: 02/01/2026*  
*Versão: 1.0*  
*Projeto: PDPw - 100% Implementado*
