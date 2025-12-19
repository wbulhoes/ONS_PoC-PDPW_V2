# ?? SOLUÇÃO: Erro "Can't add file" no Docker Build

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
- ? PDPW.API (necessário)
- ? PDPW.Application (necessário)
- ? PDPW.Domain (necessário)
- ? PDPW.Infrastructure (necessário)
- ? **PDPW.Tools.HelloWorld** (não necessário e problemático)

---

## ? SOLUÇÕES APLICADAS

### 1. Dockerfile.backend Atualizado

**Antes:**
```dockerfile
COPY src/ .  # ? Copia tudo
```

**Depois:**
```dockerfile
# ? Copia apenas os 4 projetos necessários
COPY ["src/PDPW.API/", "PDPW.API/"]
COPY ["src/PDPW.Application/", "PDPW.Application/"]
COPY ["src/PDPW.Domain/", "PDPW.Domain/"]
COPY ["src/PDPW.Infrastructure/", "PDPW.Infrastructure/"]
```

### 2. Criado `.dockerignore`

Arquivo que diz ao Docker o que **NÃO copiar**:

```
# Projetos não necessários
src/PDPW.Tools.HelloWorld/
HelloWorld/
pdpw_act/
Backup/

# Build artifacts
**/bin/
**/obj/

# Node modules
**/node_modules/

# Documentação
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

### Passo 3: Iniciar Serviços (3 min)

```powershell
# Iniciar
docker-compose up

# Aguardar até ver:
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

### Opção 1: Ver Logs Detalhados

```powershell
docker-compose build backend --no-cache --progress=plain 2>&1 | Tee-Object -FilePath build-log.txt

# Ver o arquivo build-log.txt para identificar o erro exato
```

### Opção 2: Build em Etapas

```powershell
# Build só do backend
docker-compose build backend --no-cache

# Se funcionar, build do frontend
docker-compose build frontend --no-cache
```

### Opção 3: Verificar Projeto HelloWorld

Se o projeto HelloWorld é realmente necessário:

```powershell
# Ver o que tem nele
cd src\PDPW.Tools.HelloWorld
dir

# Testar se compila local
dotnet build

# Se não compilar, corrigir ou remover
```

### Opção 4: Remover Projeto HelloWorld

Se não for necessário:

```powershell
# Remover da solution
dotnet sln remove src\PDPW.Tools.HelloWorld\PDPW.Tools.HelloWorld.csproj

# Remover pasta (backup antes!)
Move-Item src\PDPW.Tools.HelloWorld ..\_backup\PDPW.Tools.HelloWorld
```

---

## ?? OUTROS ERROS COMUNS

### Erro: "no matching manifest for windows/amd64"

**Causa:** Imagem não disponível para Windows  
**Solução:** Trocar para imagem compatível ou usar Linux containers

```dockerfile
# Se der este erro, trocar para:
FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS build
FROM mcr.microsoft.com/dotnet/aspnet:8.0-windowsservercore-ltsc2022 AS final
```

### Erro: "denied: requested access to the resource is denied"

**Causa:** Sem permissão para baixar imagem  
**Solução:** Login no Docker Hub

```powershell
docker login
```

### Erro: "failed to solve with frontend dockerfile.v0"

**Causa:** Sintaxe incorreta no Dockerfile  
**Solução:** Verificar linhas COPY estão corretas

```dockerfile
# ? Correto
COPY ["src/PDPW.API/", "PDPW.API/"]

# ? Errado (falta barra no final)
COPY ["src/PDPW.API", "PDPW.API"]
```

---

## ?? RECOMENDAÇÃO ALTERNATIVA

### Se Windows Containers Continuarem Problemáticos

**OPÇÃO: Desenvolvimento Local (MAIS RÁPIDO)**

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
- ? Muito mais rápido (sem build de 30 min)
- ?? Menos problemas de ambiente
- ?? Foco em desenvolver as 29 APIs
- ?? Economiza tempo da PoC

**Desvantagem:**
- Não valida Docker

**Solução:**
- Desenvolver local (Dias 1-5)
- Validar Docker no final (Dia 6) para apresentação

---

## ?? CHECKLIST DE VALIDAÇÃO

### Build Funcionando

- [ ] `docker-compose build --no-cache` completa sem erros
- [ ] Nenhuma mensagem "Can't add file"
- [ ] Nenhuma mensagem "unexpected EOF"
- [ ] `docker images` lista imagem `ons_poc-pdpw-backend`

### Containers Rodando

- [ ] `docker-compose up` inicia sem erros
- [ ] `docker ps` mostra 3 containers (Up)
- [ ] Logs não mostram crashes

### Aplicação Funcional

- [ ] http://localhost:5000/swagger abre
- [ ] Swagger lista endpoints
- [ ] http://localhost:3000 abre (se frontend build der certo)

---

## ?? COMMIT DAS MUDANÇAS

Se funcionou:

```powershell
git add Dockerfile.backend .dockerignore
git commit -m "[DOCKER] fix: corrige build removendo HelloWorld

- Copia apenas 4 projetos necessários (API, Application, Domain, Infrastructure)
- Adiciona .dockerignore para excluir arquivos desnecessários
- Remove cópia do projeto PDPW.Tools.HelloWorld problemático
- Otimiza build removendo docs, node_modules, etc."

git push origin develop
```

---

## ?? PRÓXIMOS PASSOS

### Se Build Funcionar ?

1. **Testar aplicação**
   ```powershell
   docker-compose up
   # Acessar http://localhost:5000/swagger
   ```

2. **Criar branches de desenvolvimento**
   ```powershell
   git checkout -b feature/gestao-ativos
   git push -u origin feature/gestao-ativos
   ```

3. **Começar desenvolvimento das APIs**

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
# Ver versão do Docker
docker version

# Ver espaço em disco
docker system df

# Ver logs de build
docker-compose build --no-cache --progress=plain

# Limpar TUDO (cuidado!)
docker system prune -a --volumes
```

### Informações Úteis

Para ajudar no debug, colete:

```powershell
# Versão Windows
winver

# Versão Docker
docker version

# Modo Docker (Windows/Linux)
docker info | Select-String "OSType"

# Logs de erro
docker-compose build --no-cache 2>&1 | Out-File docker-error.txt
```

---

**Documento criado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Versão:** 1.0  
**Status:** ? CORREÇÃO APLICADA

**Execute os comandos e me avise se funcionou! ??**
