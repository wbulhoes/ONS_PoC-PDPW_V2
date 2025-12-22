# ?? SOLU��O: Erro "Can't add file" no Docker Build

**Erro:** `Can't add file \\?\C:\temp\_ONS_PoC-PDPW\src\PDPW.Tools.HelloWorld\...`  
**Causa:** Dockerfile estava copiando TODOS os projetos da pasta `src`, incluindo `PDPW.Tools.HelloWorld` que tem problemas  
**Status:** ? **CORRIGIDO**

---

## ?? PROBLEMA IDENTIFICADO

O `Dockerfile.backend` tinha esta linha:

```dockerfile
# ? PROBLEMA: Copia TUDO da pasta src
COPY src/ .
```

Isso copiava:
- ? PDPW.API (necess�rio)
- ? PDPW.Application (necess�rio)
- ? PDPW.Domain (necess�rio)
- ? PDPW.Infrastructure (necess�rio)
- ? **PDPW.Tools.HelloWorld** (n�o necess�rio e problem�tico)

---

## ? SOLU��ES APLICADAS

### 1. Dockerfile.backend Atualizado

**Antes:**
```dockerfile
COPY src/ .  # ? Copia tudo
```

**Depois:**
```dockerfile
# ? Copia apenas os 4 projetos necess�rios
COPY ["src/PDPW.API/", "PDPW.API/"]
COPY ["src/PDPW.Application/", "PDPW.Application/"]
COPY ["src/PDPW.Domain/", "PDPW.Domain/"]
COPY ["src/PDPW.Infrastructure/", "PDPW.Infrastructure/"]
```

### 2. Criado `.dockerignore`

Arquivo que diz ao Docker o que **N�O copiar**:

```
# Projetos n�o necess�rios
src/PDPW.Tools.HelloWorld/
HelloWorld/
pdpw_act/
Backup/

# Build artifacts
**/bin/
**/obj/

# Node modules
**/node_modules/

# Documenta��o
docs/
*.md
```

---

## ?? COMANDOS PARA EXECUTAR AGORA

### Passo 1: Limpar Ambiente (1 min)

```powershell
cd C:\temp\_ONS_PoC-PDPW

# Parar containers
docker-compose down -v

# Limpar cache do Docker
docker system prune -a --volumes -f
```

### Passo 2: Build Novamente (15-30 min)

```powershell
# Build sem cache
docker-compose build --no-cache

# Ou com logs detalhados para debug:
docker-compose build --no-cache --progress=plain
```

### Passo 3: Iniciar Servi�os (3 min)

```powershell
# Iniciar
docker-compose up

# Aguardar at� ver:
# ? pdpw-sqlserver started
# ? pdpw-backend started
# ? pdpw-frontend started
```

### Passo 4: Testar

**Abrir navegador:**
- http://localhost:5000/swagger (Backend)
- http://localhost:3000 (Frontend)

---

## ?? SE O ERRO CONTINUAR

### Op��o 1: Ver Logs Detalhados

```powershell
docker-compose build backend --no-cache --progress=plain 2>&1 | Tee-Object -FilePath build-log.txt

# Ver o arquivo build-log.txt para identificar o erro exato
```

### Op��o 2: Build em Etapas

```powershell
# Build s� do backend
docker-compose build backend --no-cache

# Se funcionar, build do frontend
docker-compose build frontend --no-cache
```

### Op��o 3: Verificar Projeto HelloWorld

Se o projeto HelloWorld � realmente necess�rio:

```powershell
# Ver o que tem nele
cd src\PDPW.Tools.HelloWorld
dir

# Testar se compila local
dotnet build

# Se n�o compilar, corrigir ou remover
```

### Op��o 4: Remover Projeto HelloWorld

Se n�o for necess�rio:

```powershell
# Remover da solution
dotnet sln remove src\PDPW.Tools.HelloWorld\PDPW.Tools.HelloWorld.csproj

# Remover pasta (backup antes!)
Move-Item src\PDPW.Tools.HelloWorld ..\_backup\PDPW.Tools.HelloWorld
```

---

## ?? OUTROS ERROS COMUNS

### Erro: "no matching manifest for windows/amd64"

**Causa:** Imagem n�o dispon�vel para Windows  
**Solu��o:** Trocar para imagem compat�vel ou usar Linux containers

```dockerfile
# Se der este erro, trocar para:
FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS build
FROM mcr.microsoft.com/dotnet/aspnet:8.0-windowsservercore-ltsc2022 AS final
```

### Erro: "denied: requested access to the resource is denied"

**Causa:** Sem permiss�o para baixar imagem  
**Solu��o:** Login no Docker Hub

```powershell
docker login
```

### Erro: "failed to solve with frontend dockerfile.v0"

**Causa:** Sintaxe incorreta no Dockerfile  
**Solu��o:** Verificar linhas COPY est�o corretas

```dockerfile
# ? Correto
COPY ["src/PDPW.API/", "PDPW.API/"]

# ? Errado (falta barra no final)
COPY ["src/PDPW.API", "PDPW.API"]
```

---

## ?? RECOMENDA��O ALTERNATIVA

### Se Windows Containers Continuarem Problem�ticos

**OP��O: Desenvolvimento Local (MAIS R�PIDO)**

```powershell
# 1. Backend local
cd src\PDPW.API
dotnet run
# Acesso: http://localhost:5000/swagger

# 2. Frontend local
cd frontend
npm install
npm run dev
# Acesso: http://localhost:5173

# 3. SQL Server local
# Usar SQL Server Express instalado
# OU usar InMemory database (appsettings.json):
# "UseInMemoryDatabase": true
```

**Vantagens:**
- ? Muito mais r�pido (sem build de 30 min)
- ?? Menos problemas de ambiente
- ?? Foco em desenvolver as 29 APIs
- ?? Economiza tempo da PoC

**Desvantagem:**
- N�o valida Docker

**Solu��o:**
- Desenvolver local (Dias 1-5)
- Validar Docker no final (Dia 6) para apresenta��o

---

## ?? CHECKLIST DE VALIDA��O

### Build Funcionando

- [ ] `docker-compose build --no-cache` completa sem erros
- [ ] Nenhuma mensagem "Can't add file"
- [ ] Nenhuma mensagem "unexpected EOF"
- [ ] `docker images` lista imagem `ons_poc-pdpw-backend`

### Containers Rodando

- [ ] `docker-compose up` inicia sem erros
- [ ] `docker ps` mostra 3 containers (Up)
- [ ] Logs n�o mostram crashes

### Aplica��o Funcional

- [ ] http://localhost:5000/swagger abre
- [ ] Swagger lista endpoints
- [ ] http://localhost:3000 abre (se frontend build der certo)

---

## ?? COMMIT DAS MUDAN�AS

Se funcionou:

```powershell
git add Dockerfile.backend .dockerignore
git commit -m "[DOCKER] fix: corrige build removendo HelloWorld

- Copia apenas 4 projetos necess�rios (API, Application, Domain, Infrastructure)
- Adiciona .dockerignore para excluir arquivos desnecess�rios
- Remove c�pia do projeto PDPW.Tools.HelloWorld problem�tico
- Otimiza build removendo docs, node_modules, etc."

git push origin develop
```

---

## ?? PR�XIMOS PASSOS

### Se Build Funcionar ?

1. **Testar aplica��o**
   ```powershell
   docker-compose up
   # Acessar http://localhost:5000/swagger
   ```

2. **Criar branches de desenvolvimento**
   ```powershell
   git checkout -b feature/gestao-ativos
   git push -u origin feature/gestao-ativos
   ```

3. **Come�ar desenvolvimento das APIs**

### Se Build Continuar Falhando ?

1. **Tentar Linux Containers** (se permitido)
   ```powershell
   # Docker Desktop ? Switch to Linux containers
   # Rebuild
   ```

2. **Ou: Desenvolvimento Local** (recomendado para PoC)
   ```powershell
   cd src\PDPW.API
   dotnet run
   ```

3. **Documentar erro e pedir ajuda**
   ```powershell
   docker-compose build --no-cache --progress=plain 2>&1 | Out-File error-log.txt
   # Enviar error-log.txt
   ```

---

## ?? PRECISA DE AJUDA?

### Comandos de Debug

```powershell
# Ver vers�o do Docker
docker version

# Ver espa�o em disco
docker system df

# Ver logs de build
docker-compose build --no-cache --progress=plain

# Limpar TUDO (cuidado!)
docker system prune -a --volumes
```

### Informa��es �teis

Para ajudar no debug, colete:

```powershell
# Vers�o Windows
winver

# Vers�o Docker
docker version

# Modo Docker (Windows/Linux)
docker info | Select-String "OSType"

# Logs de erro
docker-compose build --no-cache 2>&1 | Out-File docker-error.txt
```

---

**Documento criado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Vers�o:** 1.0  
**Status:** ? CORRE��O APLICADA

**Execute os comandos e me avise se funcionou! ??**
