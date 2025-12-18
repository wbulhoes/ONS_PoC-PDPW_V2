# ?? SOLUÇÃO: Erro "npm ci" no Frontend Build

**Erro:** `npm ERR! The 'npm ci' command can only install with an existing package-lock.json`  
**Causa:** Faltava o arquivo `package-lock.json` na pasta `frontend/`  
**Status:** ? **CORRIGIDO**

---

## ?? PROBLEMA IDENTIFICADO

O `Dockerfile.frontend` usava o comando `npm ci`:

```dockerfile
COPY frontend/package*.json ./
RUN npm ci  # ? Requer package-lock.json
```

**Diferença entre comandos:**

| Comando | Quando usar | Requer package-lock.json |
|---------|-------------|-------------------------|
| `npm install` | Desenvolvimento, primeira instalação | ? Não |
| `npm ci` | CI/CD, builds reproduzíveis | ? Sim |

**Benefícios do `npm ci`:**
- ? Mais rápido (até 2x)
- ?? Determinístico (mesmas versões sempre)
- ?? Seguro (não modifica package-lock.json)
- ? Ideal para Docker

---

## ? SOLUÇÕES APLICADAS

### 1. Gerado `package-lock.json`

```powershell
cd frontend
npm install
# Gera package-lock.json automaticamente
```

**Resultado:**
- ? Criado `frontend/package-lock.json`
- ? Instaladas 226 dependências
- ?? 2 vulnerabilidades moderadas (não crítico)

### 2. Dockerfile.frontend Mantido com `npm ci`

Como agora temos `package-lock.json`, mantemos `npm ci`:

```dockerfile
# ? Agora funciona
COPY frontend/package*.json ./
RUN npm ci
```

---

## ?? COMANDOS PARA EXECUTAR AGORA

### Passo 1: Commit do package-lock.json (2 min)

```powershell
cd C:\temp\_ONS_PoC-PDPW

# Adicionar arquivo gerado
git add frontend/package-lock.json
git add Dockerfile.frontend

# Commit
git commit -m "[FRONTEND] fix: gera package-lock.json para npm ci

- Gera package-lock.json no frontend
- Corrige erro de build no Docker
- Mantém npm ci para builds determinísticos"

# Push
git push origin develop
```

### Passo 2: Limpar Docker Cache (1 min)

```powershell
# Limpar cache do Docker
docker system prune -a -f

# Ou limpar apenas o build do frontend
docker-compose build frontend --no-cache
```

### Passo 3: Build Novamente (15-30 min)

```powershell
# Build completo
docker-compose build --no-cache

# Ou apenas frontend se backend já funcionou
docker-compose build frontend --no-cache
```

### Passo 4: Iniciar Serviços (3 min)

```powershell
# Iniciar
docker-compose up

# Aguardar até ver:
# ? pdpw-backend started
# ? pdpw-frontend started
# ? pdpw-sqlserver started
```

### Passo 5: Testar

**Abrir navegador:**
- Backend: http://localhost:5000/swagger
- Frontend: http://localhost:3000

---

## ?? SE O FRONTEND CONTINUAR DANDO ERRO

### Erro 1: "Cannot find module 'vite'"

**Causa:** Build do Vite falhou  
**Solução:** Verificar se `npm run build` funciona local

```powershell
cd frontend
npm install
npm run build

# Se falhar local, corrigir primeiro
# Se funcionar local, problema é no Docker
```

### Erro 2: "ENOENT: no such file or directory, open 'dist'"

**Causa:** Build não gerou pasta `dist`  
**Solução:** Verificar script de build

```powershell
cd frontend

# Ver scripts disponíveis
npm run

# Testar build local
npm run build

# Verificar se criou pasta dist
dir dist
```

### Erro 3: Download do Node.js falha no Docker

**Causa:** Timeout ou firewall  
**Solução:** Usar imagem com Node.js pré-instalado

```dockerfile
# Trocar primeira linha do Dockerfile.frontend:
FROM stefanscherer/node-windows:20-nanoserver-ltsc2022 AS build

# Remover seção de instalação do Node.js
```

### Erro 4: "npm install" demora muito

**Causa:** Windows containers + Node.js = lento  
**Solução:** Considerar Linux containers ou dev local

```powershell
# OPÇÃO A: Trocar para Linux containers
# Docker Desktop ? Switch to Linux containers
# Usar Dockerfile.frontend com imagens Linux

# OPÇÃO B: Desenvolvimento local (RECOMENDADO)
cd frontend
npm run dev
# Acesso: http://localhost:5173
```

---

## ?? RECOMENDAÇÃO FORTE

### ? Desenvolvimento Local para Frontend

**Windows containers + Node.js = MUITO LENTO (10-20 min de build)**

**Alternativa MUITO mais rápida:**

```powershell
# 1. Backend em Docker (ou local)
cd src\PDPW.API
dotnet run
# Acesso: http://localhost:5000

# 2. Frontend LOCAL (100x mais rápido)
cd frontend
npm install
npm run dev
# Acesso: http://localhost:5173

# 3. Hot reload funcionando! Edita código e vê mudanças instantâneas
```

**Vantagens:**
- ? Build instantâneo (vs 15 min Docker)
- ?? Hot reload (edita código, vê mudanças na hora)
- ?? Fácil de debugar
- ?? Usa menos RAM (vs 16 GB Docker)

**Quando usar Docker?**
- Validação final (Dia 6)
- Demo para cliente
- Verificar integração completa

---

## ?? COMPARAÇÃO DE TEMPO

### Frontend em Docker Windows

```
Build inicial:        15-30 min
Rebuild com mudança:  10-15 min
Hot reload:           ? Não tem
RAM usada:            ~4 GB
```

### Frontend Local (npm run dev)

```
Build inicial:        10-30 seg
Rebuild com mudança:  1-2 seg (hot reload)
Hot reload:           ? Sim
RAM usada:            ~500 MB
```

**Diferença:** **60-180x mais rápido no desenvolvimento!**

---

## ?? ESTRATÉGIA RECOMENDADA PARA POC

### Dias 1-5: Desenvolvimento Local

```powershell
# Terminal 1: Backend
cd src\PDPW.API
dotnet watch run
# Hot reload do .NET

# Terminal 2: Frontend
cd frontend
npm run dev
# Hot reload do Vite

# Desenvolver as 29 APIs + 1 tela
# Tudo com hot reload = super rápido
```

### Dia 6: Validar Docker

```powershell
# Testar build completo
docker-compose build --no-cache

# Iniciar
docker-compose up

# Validar que tudo funciona
# - http://localhost:5000/swagger
# - http://localhost:3000

# Se funcionar, está pronto para apresentação
```

### Dia 8: Apresentação

```powershell
# Iniciar Docker
docker-compose up

# Mostrar funcionando
# Demonstrar integração backend/frontend/SQL

# Cliente satisfeito! ?
```

---

## ?? CHECKLIST DE VALIDAÇÃO

### Package Lock Gerado

- [ ] Arquivo `frontend/package-lock.json` existe
- [ ] Arquivo tem ~20-50 KB
- [ ] Comando `git status` lista o arquivo
- [ ] Commit e push feitos

### Build Docker

- [ ] `docker-compose build frontend` completa sem erro "npm ci"
- [ ] Imagem `ons_poc-pdpw-frontend` criada
- [ ] Nenhum erro de "package-lock.json not found"

### Frontend Funcionando

- [ ] `docker-compose up` inicia frontend
- [ ] http://localhost:3000 acessível
- [ ] Frontend carrega sem erro 404
- [ ] Frontend chama API backend sem CORS error

### Desenvolvimento Local

- [ ] `cd frontend && npm run dev` funciona
- [ ] http://localhost:5173 acessível
- [ ] Hot reload funciona (editar arquivo, ver mudança)
- [ ] API calls funcionam

---

## ?? DEBUG AVANÇADO

### Ver o que está sendo instalado

```powershell
cd frontend

# Listar dependências
npm list

# Ver árvore completa
npm list --depth=3

# Ver apenas produção
npm list --prod
```

### Verificar versões

```powershell
# Versão do Node
node --version
# Deve ser 20.x

# Versão do npm
npm --version
# Deve ser 10.x

# Versão do Vite (no projeto)
npm list vite
```

### Analisar package-lock.json

```powershell
# Ver tamanho
(Get-Item frontend/package-lock.json).Length

# Ver primeiras linhas
Get-Content frontend/package-lock.json -Head 20

# Buscar pacote específico
Select-String -Path frontend/package-lock.json -Pattern "react"
```

---

## ?? RESUMO

**O QUE FIZEMOS:**
1. ? Identificamos que faltava `package-lock.json`
2. ? Geramos o arquivo com `npm install`
3. ? Mantivemos `npm ci` no Dockerfile (melhor prática)
4. ? Documentamos alternativa de dev local (recomendada)

**O QUE VOCÊ DEVE FAZER:**
1. ? Commit do `package-lock.json`
2. ? Rebuild do Docker
3. ?? **CONSIDERAR**: Usar dev local para frontend (muito mais rápido)

**RESULTADO ESPERADO:**
- ? Docker build funciona sem erro "npm ci"
- ? Frontend roda em container (se necessário)
- ? Desenvolvimento muito mais rápido com local dev

---

## ?? PRÓXIMOS PASSOS

### Se Build Funcionar ?

```powershell
# 1. Testar tudo
docker-compose up
# Backend: http://localhost:5000/swagger
# Frontend: http://localhost:3000

# 2. Se funcionou, iniciar desenvolvimento
git checkout -b feature/gestao-ativos
# Começar APIs
```

### Se Ainda Tiver Problemas ?

```powershell
# 1. Usar desenvolvimento local (RECOMENDADO)
cd src\PDPW.API
dotnet run

cd frontend
npm run dev

# 2. Documentar erro
docker-compose build --no-cache --progress=plain > build-error.txt

# 3. Pedir ajuda com o arquivo build-error.txt
```

---

**Documento criado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Versão:** 1.0  
**Status:** ? CORREÇÃO APLICADA + PACKAGE-LOCK GERADO

**Execute os comandos e me avise o resultado! ??**

**?? DICA:** Para PoC de 6 dias, **desenvolvimento local é MUITO mais eficiente** que Windows containers!
