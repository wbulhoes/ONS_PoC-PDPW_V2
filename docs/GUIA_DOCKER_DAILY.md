# ?? GUIA DOCKER - APRESENTA��O DAILY

**Data:** 19/12/2024  
**Objetivo:** Rodar API PDPW via Docker para demonstra��o  
**Tempo:** 5-10 minutos

---

## ?? RESUMO R�PIDO

```bash
# 1. Subir aplica��o
.\docker-start.ps1

# 2. Abrir Swagger
http://localhost:5000/swagger

# 3. Parar aplica��o
.\docker-stop.ps1
```

---

## ?? PR�-REQUISITOS

### 1. Docker Desktop Instalado
- [ ] Docker Desktop instalado
- [ ] Docker Desktop rodando
- [ ] WSL2 configurado (Windows)

**Download:** https://www.docker.com/products/docker-desktop

### 2. Portas Dispon�veis
- [ ] Porta **5000** livre (API)
- [ ] Porta **1433** livre (SQL Server)

**Verificar portas:**
```powershell
netstat -ano | findstr :5000
netstat -ano | findstr :1433
```

### 3. Recursos M�nimos
- RAM: 4 GB dispon�vel
- Disco: 10 GB livres
- CPU: 2 cores

---

## ?? PASSO A PASSO

### PASSO 1: Verificar Docker

```powershell
# Verificar se Docker est� rodando
docker --version
docker info

# Resultado esperado:
# Docker version 24.x.x
# Server Running
```

### PASSO 2: Navegar para o projeto

```powershell
cd C:\temp\_ONS_PoC-PDPW
```

### PASSO 3: Subir aplica��o

```powershell
.\docker-start.ps1
```

**O que o script faz:**
1. ? Verifica Docker
2. ? Limpa containers antigos
3. ? Build da aplica��o
4. ? Sobe SQL Server
5. ? Aguarda SQL inicializar
6. ? Sobe API
7. ? Aplica migrations
8. ? Insere seed data

**Tempo:** ~3-5 minutos (primeira vez)

### PASSO 4: Aguardar inicializa��o

```
? SQL Server: 15-20 segundos
? API: 10-15 segundos
? Migrations: 5-10 segundos

Total: ~40-45 segundos
```

### PASSO 5: Testar Swagger

**Abrir no navegador:**
```
http://localhost:5000/swagger
```

**Voc� deve ver:**
- ? Swagger UI carregado
- ? Endpoints da API Usina
- ? "Try it out" dispon�vel

---

## ?? TESTES R�PIDOS

### Teste 1: Health Check
```
http://localhost:5000/health
```

**Resultado esperado:**
```json
{
  "status": "Healthy"
}
```

### Teste 2: GET Usinas
```
http://localhost:5000/api/usinas
```

**Resultado esperado:**
```json
[
  {
    "id": 1,
    "codigo": "UHE-ITAIPU",
    "nome": "Usina Hidrel�trica de Itaipu",
    ...
  }
]
```

### Teste 3: Swagger Interativo
1. Abrir http://localhost:5000/swagger
2. Expandir `GET /api/usinas`
3. Clicar "Try it out"
4. Clicar "Execute"
5. Ver 10 usinas retornadas

---

## ?? ROTEIRO PARA DAILY

### Prepara��o (5 min antes)
```powershell
# 1. Subir aplica��o
cd C:\temp\_ONS_PoC-PDPW
.\docker-start.ps1

# 2. Aguardar logs
# "? APLICA��O RODANDO COM SUCESSO!"

# 3. Testar Swagger
start http://localhost:5000/swagger

# 4. Verificar dados
# GET /api/usinas deve retornar 10 usinas
```

### Durante a Daily (2 min)

**"Vou demonstrar a API funcionando via Docker"**

1. **Mostrar Swagger UI**
   ```
   "Aqui est� o Swagger com a documenta��o autom�tica"
   ```

2. **Testar GET lista**
   ```
   "Vou buscar todas as usinas cadastradas"
   ? Try it out ? Execute
   ? Mostrar 10 usinas retornadas
   ```

3. **Destacar dados reais**
   ```
   "Temos dados reais do SIN:
   - Itaipu (14.000 MW)
   - Belo Monte (11.233 MW)
   - Tucuru� (8.370 MW)"
   ```

4. **Mostrar CRUD**
   ```
   "Temos 8 endpoints:
   - GET lista e busca
   - POST criar
   - PUT atualizar
   - DELETE remover"
   ```

### Ap�s Daily
```powershell
# Parar aplica��o (se necess�rio)
.\docker-stop.ps1

# Ou manter rodando para mais testes
```

---

## ?? TROUBLESHOOTING

### Problema: "Docker n�o est� rodando"

**Solu��o:**
1. Abrir Docker Desktop
2. Aguardar inicializar completamente
3. Tentar novamente

### Problema: "Port 5000 already in use"

**Solu��o:**
```powershell
# Ver o que est� usando a porta
netstat -ano | findstr :5000

# Matar processo (substitua PID)
taskkill /F /PID <PID>

# Ou mudar porta no docker-compose.yml
# - "5001:80"
```

### Problema: "Port 1433 already in use"

**Solu��o:**
```powershell
# Parar SQL Server local
Stop-Service MSSQLSERVER

# Ou usar porta diferente
# - "1434:1433"
```

### Problema: Build falhou

**Solu��o:**
```powershell
# Limpar tudo
docker-compose down -v
docker system prune -a

# Build novamente
.\docker-start.ps1
```

### Problema: Swagger n�o abre

**Solu��o:**
```powershell
# Ver logs da API
docker-compose logs -f api

# Verificar se API subiu
docker ps

# Resultado esperado:
# pdpw-api    Up
# pdpw-sqlserver    Up
```

### Problema: Dados n�o aparecem

**Solu��o:**
```powershell
# Entrar no container
docker exec -it pdpw-api bash

# Aplicar migrations manualmente
dotnet ef database update --project /app/PDPW.Infrastructure.dll

# Verificar banco
docker exec -it pdpw-sqlserver /opt/mssql-tools/bin/sqlcmd \
  -S localhost -U sa -P "Pdpw@2024!Strong" \
  -Q "SELECT COUNT(*) FROM PDPW_DB_Dev.dbo.Usinas"
```

---

## ?? COMANDOS �TEIS

### Gerenciar Containers

```powershell
# Ver status
docker ps

# Ver logs da API
docker-compose logs -f api

# Ver logs do SQL Server
docker-compose logs -f sqlserver

# Restart API
docker-compose restart api

# Parar tudo
docker-compose stop

# Parar e remover volumes
docker-compose down -v
```

### Acessar Containers

```powershell
# Entrar na API
docker exec -it pdpw-api bash

# Entrar no SQL Server
docker exec -it pdpw-sqlserver bash

# Executar comando no container
docker exec pdpw-api dotnet --version
```

### Verificar Sa�de

```powershell
# Health check da API
curl http://localhost:5000/health

# Health check do SQL Server
docker exec pdpw-sqlserver /opt/mssql-tools/bin/sqlcmd \
  -S localhost -U sa -P "Pdpw@2024!Strong" -Q "SELECT 1"
```

---

## ?? ARQUITETURA DOCKER

```
???????????????????????????????????????
?     Docker Compose                  ?
???????????????????????????????????????
?                                     ?
?  ????????????????  ??????????????? ?
?  ?  pdpw-api    ?  ? sqlserver   ? ?
?  ?              ?  ?             ? ?
?  ?  .NET 8      ?? ? SQL Server  ? ?
?  ?  Port 5000   ?  ? Port 1433   ? ?
?  ????????????????  ??????????????? ?
?         ?                ?          ?
?  ????????????????  ??????????????? ?
?  ?   Volume     ?  ?  Volume     ? ?
?  ?  (app data)  ?  ?  (SQL data) ? ?
?  ????????????????  ??????????????? ?
?                                     ?
???????????????????????????????????????
         ?
    Host Machine
    localhost:5000 ? Swagger
    localhost:1433 ? SQL Server
```

---

## ?? CREDENCIAIS

### SQL Server
```
Server: localhost,1433
Database: PDPW_DB_Dev
User: sa
Password: Pdpw@2024!Strong
```

### Connection String
```
Server=localhost,1433;Database=PDPW_DB_Dev;User Id=sa;Password=Pdpw@2024!Strong;TrustServerCertificate=True;MultipleActiveResultSets=true
```

---

## ?? CHECKLIST PR�-DAILY

### 30 min antes
- [ ] Docker Desktop rodando
- [ ] Portas 5000 e 1433 livres
- [ ] Executar `.\docker-start.ps1`
- [ ] Aguardar "? APLICA��O RODANDO"

### 10 min antes
- [ ] Abrir Swagger: http://localhost:5000/swagger
- [ ] Testar GET /api/usinas
- [ ] Verificar 10 usinas retornadas
- [ ] Testar GET /api/usinas/1 (Itaipu)

### Durante Daily
- [ ] Compartilhar tela
- [ ] Mostrar Swagger UI
- [ ] Executar GET /api/usinas
- [ ] Destacar dados reais
- [ ] Mostrar 8 endpoints dispon�veis

### Ap�s Daily
- [ ] Manter rodando (se mais demos)
- [ ] Ou parar: `.\docker-stop.ps1`

---

## ?? VANTAGENS DO DOCKER

### Para a Daily
? **Isolado** - N�o interfere com ambiente local  
? **Reproduz�vel** - Funciona igual em qualquer m�quina  
? **R�pido** - Sobe em ~1 minuto  
? **Limpo** - Remove tudo com 1 comando  

### Para Desenvolvimento
? **Consistente** - Mesmo ambiente para todos  
? **Port�vel** - Leva para qualquer lugar  
? **Escal�vel** - F�cil adicionar servi�os  
? **Profissional** - Padr�o da ind�stria  

---

## ?? M�TRICAS DE PERFORMANCE

### Primeira execu��o
```
Build:        2-3 minutos
SQL Server:   15-20 segundos
API:          10-15 segundos
Migrations:   5-10 segundos
Total:        ~3-4 minutos
```

### Execu��es seguintes
```
SQL Server:   15-20 segundos
API:          10-15 segundos
Total:        ~30-35 segundos
```

### Recursos utilizados
```
RAM:          ~2 GB
Disco:        ~5 GB
CPU:          <10%
```

---

## ?? SUPORTE

### Se algo der errado:

1. **Ver logs:**
   ```powershell
   docker-compose logs -f
   ```

2. **Limpar tudo:**
   ```powershell
   docker-compose down -v
   docker system prune -a
   ```

3. **Recome�ar:**
   ```powershell
   .\docker-start.ps1
   ```

---

## ? VERIFICA��O FINAL

Antes da daily, confirme:

```powershell
# 1. Containers rodando
docker ps
# Deve mostrar: pdpw-api e pdpw-sqlserver

# 2. API respondendo
curl http://localhost:5000/health
# Deve retornar: {"status":"Healthy"}

# 3. Swagger acess�vel
start http://localhost:5000/swagger
# Deve abrir navegador com Swagger UI

# 4. Dados presentes
curl http://localhost:5000/api/usinas
# Deve retornar array com 10 usinas
```

**Se todos os 4 itens ? ? PRONTO PARA DAILY! ??**

---

## ?? COMANDOS DE EMERG�NCIA

```powershell
# Parar tudo IMEDIATAMENTE
docker-compose down

# Remover TUDO (cuidado!)
docker-compose down -v
docker system prune -a -f

# Restart for�ado
docker-compose restart --force-recreate

# Logs em tempo real
docker-compose logs -f --tail=100
```

---

**Criado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Vers�o:** 1.0  
**Status:** ? PRONTO PARA USO

**BOA SORTE NA DAILY COM DOCKER! ????**
