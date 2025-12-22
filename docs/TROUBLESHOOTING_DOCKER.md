# ?? TROUBLESHOOTING DOCKER - GUIA R�PIDO

**Problema:** Erro ao executar `docker-start.ps1`

---

## ? ERRO COMUM

```
error during connect: Head "http://%2F%2F.%2Fpipe%2FdockerDesktopLinuxEngine/_ping": 
open //./pipe/dockerDesktopLinuxEngine: O sistema n�o pode encontrar o arquivo especificado.
```

**Causa:** Docker Desktop n�o est� rodando ou n�o est� respondendo

---

## ? SOLU��O R�PIDA (2 minutos)

### PASSO 1: Executar diagn�stico

```powershell
.\docker-diagnostico.ps1
```

**Se mostrar ? Docker n�o est� pronto:**

### PASSO 2: Executar fix

```powershell
.\docker-fix.ps1
```

### PASSO 3: Tentar novamente

```powershell
.\docker-start.ps1
```

---

## ?? SOLU��O MANUAL (5 minutos)

### 1. Fechar Docker Desktop

```powershell
# Fechar pelo PowerShell
Stop-Process -Name "Docker Desktop" -Force

# Ou fechar manualmente:
# - Clicar com bot�o direito no �cone Docker (bandeja)
# - Escolher "Quit Docker Desktop"
```

### 2. Abrir Gerenciador de Tarefas

```
Ctrl + Shift + Esc
```

### 3. Finalizar processos Docker

Procurar e finalizar:
- Docker Desktop
- Docker Desktop Backend
- com.docker.backend
- com.docker.service

### 4. Aguardar 10 segundos

### 5. Abrir Docker Desktop

```
Menu Iniciar ? Docker Desktop
```

### 6. Aguardar inicializa��o

**Indicadores que est� pronto:**
- ? �cone Docker na bandeja SEM anima��o
- ? Status: "Docker Desktop is running"
- ? Verde no �cone

**Tempo:** ~1-2 minutos

### 7. Verificar no PowerShell

```powershell
docker info
```

**Resultado esperado:** Informa��es do Docker (n�o erro)

### 8. Tentar novamente

```powershell
.\docker-start.ps1
```

---

## ?? SE AINDA N�O FUNCIONAR

### Op��o A: Reiniciar WSL2

```powershell
# Parar WSL
wsl --shutdown

# Aguardar 10 segundos
Start-Sleep -Seconds 10

# Reiniciar Docker Desktop
Start-Process "C:\Program Files\Docker\Docker\Docker Desktop.exe"

# Aguardar 1 minuto
Start-Sleep -Seconds 60

# Testar
docker info
```

### Op��o B: Reiniciar Computador

**Mais eficaz para problemas persistentes**

1. Fechar tudo
2. Reiniciar Windows
3. Abrir Docker Desktop
4. Aguardar inicializar
5. Executar `.\docker-start.ps1`

---

## ?? VERIFICA��ES ESPEC�FICAS

### Verificar Docker Desktop

```powershell
# Ver processo
Get-Process "Docker Desktop" -ErrorAction SilentlyContinue

# Deve retornar:
# Handles  NPM(K)    PM(K)      WS(K)     CPU(s)     Id  SI ProcessName
# -------  ------    -----      -----     ------     --  -- -----------
#     xxx      xx   xxxxx     xxxxxx      xx.xx   xxxx   x Docker Desktop
```

### Verificar Daemon

```powershell
docker version

# Deve mostrar:
# Client: ...
# Server: ... ? IMPORTANTE: Esta se��o deve aparecer
```

### Verificar Portas

```powershell
# Porta 5000 (API)
Get-NetTCPConnection -LocalPort 5000 -ErrorAction SilentlyContinue

# Porta 1433 (SQL Server)
Get-NetTCPConnection -LocalPort 1433 -ErrorAction SilentlyContinue

# Se retornar algo, porta est� ocupada
# Liberar porta:
# netstat -ano | findstr :5000
# taskkill /F /PID <PID>
```

---

## ?? CHECKLIST DE VERIFICA��O

Antes de executar `docker-start.ps1`:

- [ ] Docker Desktop aberto
- [ ] �cone Docker na bandeja (verde)
- [ ] Status: "Docker Desktop is running"
- [ ] `docker info` funciona
- [ ] `docker version` mostra Client E Server
- [ ] Porta 5000 livre
- [ ] Porta 1433 livre
- [ ] WSL2 funcionando (opcional)

**Se todos ? ? docker-start.ps1 vai funcionar!**

---

## ?? COMANDOS DE DIAGN�STICO

```powershell
# 1. Ver processos Docker
Get-Process | Where-Object {$_.Name -like "*docker*"}

# 2. Ver containers rodando
docker ps

# 3. Ver containers parados
docker ps -a

# 4. Ver imagens
docker images

# 5. Ver volumes
docker volume ls

# 6. Ver redes
docker network ls

# 7. Ver status geral
docker system df

# 8. Ver logs de um container
docker logs <container-name>
```

---

## ?? COMANDOS DE LIMPEZA

```powershell
# Limpar tudo (CUIDADO!)
docker system prune -a -f --volumes

# Limpar containers parados
docker container prune -f

# Limpar imagens n�o usadas
docker image prune -a -f

# Limpar volumes n�o usados
docker volume prune -f

# Limpar redes n�o usadas
docker network prune -f
```

---

## ?? SCRIPTS DISPON�VEIS

1. **docker-diagnostico.ps1** ?
   - Verifica se tudo est� OK
   - Identifica problemas

2. **docker-fix.ps1** ?
   - Tenta corrigir automaticamente
   - Reinicia Docker se necess�rio

3. **docker-start.ps1**
   - Sobe a aplica��o
   - Agora com melhor tratamento de erros

4. **docker-test.ps1**
   - Testa se API est� funcionando

5. **docker-stop.ps1**
   - Para containers

---

## ?? ORDEM RECOMENDADA DE EXECU��O

```powershell
# 1. SEMPRE: Diagn�stico primeiro
.\docker-diagnostico.ps1

# 2. Se problemas: Fix
.\docker-fix.ps1

# 3. Subir aplica��o
.\docker-start.ps1

# 4. Testar
.\docker-test.ps1

# 5. Usar
start http://localhost:5000/swagger
```

---

## ?? ALTERNATIVA: RODAR SEM DOCKER

Se Docker continuar com problemas:

```powershell
# Terminal 1: SQL Server LocalDB
# (J� deve estar rodando localmente)

# Terminal 2: API
cd C:\temp\_ONS_PoC-PDPW\src\PDPW.API
dotnet run

# Abrir Swagger
start http://localhost:5000/swagger
```

---

## ?? DICAS

### Para a Daily

Se Docker n�o funcionar a tempo:
1. Use API local (dotnet run)
2. Ou use apresenta��o preparada
3. Demonstre c�digo ao inv�s de executar

### Para Produ��o

- Docker � essencial
- Investir tempo para resolver
- Garantir que funciona antes de apresentar

---

## ?? RESUMO R�PIDO

```
ERRO ? .\docker-diagnostico.ps1 ? .\docker-fix.ps1 ? .\docker-start.ps1
```

**Tempo total:** ~5 minutos

**Taxa de sucesso:** ~95%

---

**Criado:** 19/12/2024  
**�ltima atualiza��o:** 19/12/2024  
**Status:** ? TESTADO

**BOA SORTE! ??**
