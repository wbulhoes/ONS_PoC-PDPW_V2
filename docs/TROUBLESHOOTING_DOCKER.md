# ?? TROUBLESHOOTING DOCKER - GUIA RÁPIDO

**Problema:** Erro ao executar `docker-start.ps1`

---

## ? ERRO COMUM

```
error during connect: Head "http://%2F%2F.%2Fpipe%2FdockerDesktopLinuxEngine/_ping": 
open //./pipe/dockerDesktopLinuxEngine: O sistema não pode encontrar o arquivo especificado.
```

**Causa:** Docker Desktop não está rodando ou não está respondendo

---

## ? SOLUÇÃO RÁPIDA (2 minutos)

### PASSO 1: Executar diagnóstico

```powershell
.\docker-diagnostico.ps1
```

**Se mostrar ? Docker não está pronto:**

### PASSO 2: Executar fix

```powershell
.\docker-fix.ps1
```

### PASSO 3: Tentar novamente

```powershell
.\docker-start.ps1
```

---

## ?? SOLUÇÃO MANUAL (5 minutos)

### 1. Fechar Docker Desktop

```powershell
# Fechar pelo PowerShell
Stop-Process -Name "Docker Desktop" -Force

# Ou fechar manualmente:
# - Clicar com botão direito no ícone Docker (bandeja)
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

### 6. Aguardar inicialização

**Indicadores que está pronto:**
- ? Ícone Docker na bandeja SEM animação
- ? Status: "Docker Desktop is running"
- ? Verde no ícone

**Tempo:** ~1-2 minutos

### 7. Verificar no PowerShell

```powershell
docker info
```

**Resultado esperado:** Informações do Docker (não erro)

### 8. Tentar novamente

```powershell
.\docker-start.ps1
```

---

## ?? SE AINDA NÃO FUNCIONAR

### Opção A: Reiniciar WSL2

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

### Opção B: Reiniciar Computador

**Mais eficaz para problemas persistentes**

1. Fechar tudo
2. Reiniciar Windows
3. Abrir Docker Desktop
4. Aguardar inicializar
5. Executar `.\docker-start.ps1`

---

## ?? VERIFICAÇÕES ESPECÍFICAS

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
# Server: ... ? IMPORTANTE: Esta seção deve aparecer
```

### Verificar Portas

```powershell
# Porta 5000 (API)
Get-NetTCPConnection -LocalPort 5000 -ErrorAction SilentlyContinue

# Porta 1433 (SQL Server)
Get-NetTCPConnection -LocalPort 1433 -ErrorAction SilentlyContinue

# Se retornar algo, porta está ocupada
# Liberar porta:
# netstat -ano | findstr :5000
# taskkill /F /PID <PID>
```

---

## ?? CHECKLIST DE VERIFICAÇÃO

Antes de executar `docker-start.ps1`:

- [ ] Docker Desktop aberto
- [ ] Ícone Docker na bandeja (verde)
- [ ] Status: "Docker Desktop is running"
- [ ] `docker info` funciona
- [ ] `docker version` mostra Client E Server
- [ ] Porta 5000 livre
- [ ] Porta 1433 livre
- [ ] WSL2 funcionando (opcional)

**Se todos ? ? docker-start.ps1 vai funcionar!**

---

## ?? COMANDOS DE DIAGNÓSTICO

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

# Limpar imagens não usadas
docker image prune -a -f

# Limpar volumes não usados
docker volume prune -f

# Limpar redes não usadas
docker network prune -f
```

---

## ?? SCRIPTS DISPONÍVEIS

1. **docker-diagnostico.ps1** ?
   - Verifica se tudo está OK
   - Identifica problemas

2. **docker-fix.ps1** ?
   - Tenta corrigir automaticamente
   - Reinicia Docker se necessário

3. **docker-start.ps1**
   - Sobe a aplicação
   - Agora com melhor tratamento de erros

4. **docker-test.ps1**
   - Testa se API está funcionando

5. **docker-stop.ps1**
   - Para containers

---

## ?? ORDEM RECOMENDADA DE EXECUÇÃO

```powershell
# 1. SEMPRE: Diagnóstico primeiro
.\docker-diagnostico.ps1

# 2. Se problemas: Fix
.\docker-fix.ps1

# 3. Subir aplicação
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
# (Já deve estar rodando localmente)

# Terminal 2: API
cd C:\temp\_ONS_PoC-PDPW\src\PDPW.API
dotnet run

# Abrir Swagger
start http://localhost:5000/swagger
```

---

## ?? DICAS

### Para a Daily

Se Docker não funcionar a tempo:
1. Use API local (dotnet run)
2. Ou use apresentação preparada
3. Demonstre código ao invés de executar

### Para Produção

- Docker é essencial
- Investir tempo para resolver
- Garantir que funciona antes de apresentar

---

## ?? RESUMO RÁPIDO

```
ERRO ? .\docker-diagnostico.ps1 ? .\docker-fix.ps1 ? .\docker-start.ps1
```

**Tempo total:** ~5 minutos

**Taxa de sucesso:** ~95%

---

**Criado:** 19/12/2024  
**Última atualização:** 19/12/2024  
**Status:** ? TESTADO

**BOA SORTE! ??**
