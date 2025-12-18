# ??? GUIA DE SETUP DO AMBIENTE - PoC PDPW

**Objetivo:** Configurar ambiente de desenvolvimento completo em < 30 minutos  
**Data:** 19/12/2024  
**Versão:** 1.0

---

## ?? PRÉ-REQUISITOS

### Sistema Operacional
- ? Windows 10/11 (64-bit)
- ? Mínimo 8GB RAM (recomendado 16GB)
- ? 10GB de espaço livre em disco
- ? Conexão com internet

---

## ?? INSTALAÇÃO RÁPIDA (TODOS OS DEVS)

### 1?? Instalar Winget (se não tiver)

```powershell
# Verificar se já está instalado
winget --version

# Se não estiver, baixar de:
# https://aka.ms/getwinget
```

### 2?? Instalar Git

```powershell
# Instalar
winget install Git.Git

# Verificar
git --version
# Saída esperada: git version 2.43.0 (ou superior)

# Configurar (substituir por seus dados)
git config --global user.name "Seu Nome"
git config --global user.email "seu.email@example.com"
```

### 3?? Clonar o Repositório

```powershell
# Navegar para diretório de trabalho
cd C:\temp

# Clonar (se ainda não clonou)
git clone https://github.com/wbulhoes/ONS_PoC-PDPW.git

# Entrar no diretório
cd ONS_PoC-PDPW

# Verificar branch
git branch -a
# Deve mostrar: develop, main

# Criar/mudar para branch develop
git checkout develop
```

---

## ?? SETUP PARA DEVS BACKEND (DEV 1 e DEV 2)

### 1?? Instalar .NET 8 SDK

```powershell
# Instalar
winget install Microsoft.DotNet.SDK.8

# Verificar
dotnet --version
# Saída esperada: 8.0.xxx

# Listar SDKs instalados
dotnet --list-sdks
```

### 2?? Instalar Visual Studio 2022 Community

```powershell
# Instalar
winget install Microsoft.VisualStudio.2022.Community

# Ou instalar Rider (alternativa)
winget install JetBrains.Rider
```

**Workloads necessários:**
- ASP.NET and web development
- .NET desktop development
- Azure development (opcional)

### 3?? Instalar SQL Server Express (Opcional)

```powershell
# Instalar SQL Server
winget install Microsoft.SQLServer.2022.Express

# Instalar SQL Server Management Studio (SSMS)
winget install Microsoft.SQLServerManagementStudio
```

?? **Nota:** Para a PoC, usaremos InMemory Database, então SQL Server é opcional.

### 4?? Instalar Docker Desktop

```powershell
# Instalar
winget install Docker.DockerDesktop

# Após instalação, reiniciar o computador

# Verificar
docker --version
# Saída esperada: Docker version 24.x.x

docker-compose --version
# Saída esperada: Docker Compose version v2.x.x
```

### 5?? Testar o Backend

```powershell
# Navegar para o projeto API
cd C:\temp\ONS_PoC-PDPW\src\PDPW.API

# Restaurar dependências
dotnet restore

# Compilar
dotnet build

# Executar
dotnet run
```

**Saída esperada:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

**Testar no navegador:**
- Swagger: http://localhost:5000/swagger
- Health: http://localhost:5000/health

### 6?? Extensões Recomendadas para Visual Studio

- C# Dev Kit
- GitHub Copilot (se disponível)
- REST Client
- Docker Extension
- GitLens

---

## ?? SETUP PARA DEV FRONTEND (DEV 3)

### 1?? Instalar Node.js 20 LTS

```powershell
# Instalar
winget install OpenJS.NodeJS.LTS

# Verificar
node --version
# Saída esperada: v20.x.x

npm --version
# Saída esperada: 10.x.x
```

### 2?? Instalar Visual Studio Code

```powershell
# Instalar
winget install Microsoft.VisualStudioCode

# Abrir VS Code
code .
```

### 3?? Instalar Extensões do VS Code

```powershell
# ES7+ React/Redux/React-Native snippets
code --install-extension dsznajder.es7-react-js-snippets

# ESLint
code --install-extension dbaeumer.vscode-eslint

# Prettier
code --install-extension esbenp.prettier-vscode

# Auto Rename Tag
code --install-extension formulahendry.auto-rename-tag

# Path Intellisense
code --install-extension christian-kohler.path-intellisense

# GitLens
code --install-extension eamodio.gitlens

# GitHub Copilot (se disponível)
code --install-extension GitHub.copilot
```

### 4?? Testar o Frontend

```powershell
# Navegar para o projeto frontend
cd C:\temp\ONS_PoC-PDPW\frontend

# Instalar dependências
npm install

# Executar em modo desenvolvimento
npm run dev
```

**Saída esperada:**
```
  VITE v5.x.x  ready in xxx ms

  ?  Local:   http://localhost:3000/
  ?  Network: use --host to expose
  ?  press h + enter to show help
```

**Testar no navegador:**
- Frontend: http://localhost:3000

### 5?? Ferramentas Adicionais

```powershell
# Instalar Postman (para testar APIs)
winget install Postman.Postman

# Ou Insomnia (alternativa)
winget install Insomnia.Insomnia
```

---

## ?? SETUP PARA QA SPECIALIST

### 1?? Instalar Ferramentas de Teste

```powershell
# Postman (para testes de API)
winget install Postman.Postman

# Navegadores para testes
winget install Google.Chrome
winget install Mozilla.Firefox
winget install Microsoft.Edge
```

### 2?? Instalar Git e VS Code

```powershell
# Git
winget install Git.Git

# VS Code (para editar documentação)
winget install Microsoft.VisualStudioCode
```

### 3?? Ferramentas Opcionais

```powershell
# ScreenToGif (para capturar bugs em GIF)
winget install NickeManarin.ScreenToGif

# Greenshot (para screenshots)
winget install Greenshot.Greenshot
```

---

## ? CHECKLIST DE VERIFICAÇÃO

### Backend Devs

```powershell
# Executar todos os comandos abaixo e verificar saída

# 1. .NET 8 SDK
dotnet --version
# ? Deve ser 8.0.xxx

# 2. Git
git --version
# ? Deve ser 2.43.x ou superior

# 3. Docker
docker --version
# ? Deve ser 24.x.x ou superior

# 4. Compilar solução
cd C:\temp\ONS_PoC-PDPW
dotnet build
# ? Build succeeded. 0 Warning(s). 0 Error(s).

# 5. Executar API
cd src\PDPW.API
dotnet run
# ? Now listening on: http://localhost:5000

# 6. Testar Swagger
# Abrir http://localhost:5000/swagger
# ? Página do Swagger carrega
```

### Frontend Dev

```powershell
# 1. Node.js
node --version
# ? Deve ser v20.x.x

# 2. npm
npm --version
# ? Deve ser 10.x.x

# 3. Git
git --version
# ? Deve ser 2.43.x ou superior

# 4. Instalar dependências
cd C:\temp\ONS_PoC-PDPW\frontend
npm install
# ? Sem erros

# 5. Executar dev server
npm run dev
# ? Server running on http://localhost:3000

# 6. Testar no navegador
# Abrir http://localhost:3000
# ? Aplicação React carrega
```

### QA Specialist

```powershell
# 1. Postman
postman --version
# ? Instalado

# 2. Git
git --version
# ? Deve ser 2.43.x ou superior

# 3. VS Code
code --version
# ? Instalado

# 4. Clonar repositório
cd C:\temp\ONS_PoC-PDPW
git status
# ? On branch develop
```

---

## ?? TROUBLESHOOTING

### Problema: Porta 5000 já está em uso

```powershell
# Verificar o que está usando a porta
netstat -ano | findstr :5000

# Matar o processo (substituir XXXX pelo PID)
taskkill /PID XXXX /F

# Ou mudar a porta em launchSettings.json
# src/PDPW.API/Properties/launchSettings.json
# "applicationUrl": "http://localhost:5001"
```

### Problema: Porta 3000 já está em uso

```powershell
# Verificar o que está usando a porta
netstat -ano | findstr :3000

# Matar o processo
taskkill /PID XXXX /F

# Ou o Vite perguntará automaticamente para usar outra porta
```

### Problema: dotnet build falha

```powershell
# Limpar build anterior
dotnet clean

# Restaurar pacotes
dotnet restore

# Compilar novamente
dotnet build
```

### Problema: npm install falha

```powershell
# Limpar cache
npm cache clean --force

# Deletar node_modules e package-lock.json
rm -rf node_modules
rm package-lock.json

# Instalar novamente
npm install
```

### Problema: Docker Desktop não inicia

```powershell
# 1. Verificar se WSL 2 está instalado
wsl --version

# 2. Se não estiver, instalar WSL 2
wsl --install

# 3. Reiniciar o computador

# 4. Iniciar Docker Desktop manualmente
# Procurar "Docker Desktop" no menu Iniciar
```

### Problema: Git clone falha (autenticação)

```powershell
# Usar GitHub Personal Access Token

# 1. Gerar token em: https://github.com/settings/tokens
# 2. Ao clonar, usar token como senha

# Ou configurar SSH
ssh-keygen -t ed25519 -C "seu.email@example.com"
# Adicionar chave pública em: https://github.com/settings/keys
```

---

## ?? CONFIGURAÇÕES ADICIONAIS

### Configurar Git (todos)

```powershell
# Nome e email
git config --global user.name "Seu Nome"
git config --global user.email "seu.email@example.com"

# Editor padrão (VS Code)
git config --global core.editor "code --wait"

# Alias úteis
git config --global alias.st status
git config --global alias.co checkout
git config --global alias.br branch
git config --global alias.cm commit

# Line endings (Windows)
git config --global core.autocrlf true
```

### Configurar VS Code (Frontend)

```json
// settings.json
{
  "editor.formatOnSave": true,
  "editor.defaultFormatter": "esbenp.prettier-vscode",
  "editor.codeActionsOnSave": {
    "source.fixAll.eslint": true
  },
  "typescript.updateImportsOnFileMove.enabled": "always",
  "javascript.updateImportsOnFileMove.enabled": "always"
}
```

### Configurar Prettier (Frontend)

```json
// frontend/.prettierrc
{
  "semi": true,
  "singleQuote": true,
  "tabWidth": 2,
  "trailingComma": "es5",
  "printWidth": 80
}
```

---

## ?? ESTRUTURA DE BRANCHES

```
main (produção)
??? develop (desenvolvimento)
    ??? feature/slice-1-usinas (DEV 1)
    ??? feature/slice-2-dadger (DEV 2)
    ??? feature/frontend-slices (DEV 3)
    ??? docs/test-documentation (QA)
```

### Comandos para criar branches

```powershell
# Backend DEV 1
git checkout develop
git pull origin develop
git checkout -b feature/slice-1-usinas
git push -u origin feature/slice-1-usinas

# Backend DEV 2
git checkout develop
git pull origin develop
git checkout -b feature/slice-2-dadger
git push -u origin feature/slice-2-dadger

# Frontend DEV 3
git checkout develop
git pull origin develop
git checkout -b feature/frontend-slices
git push -u origin feature/frontend-slices

# QA
git checkout develop
git pull origin develop
git checkout -b docs/test-documentation
git push -u origin docs/test-documentation
```

---

## ?? PRÓXIMOS PASSOS

### Após Setup Completo

1. ? **Sincronizar com o time**
   - Confirmar que todos concluíram o setup
   - Resolver problemas em conjunto

2. ? **Estudar o código legado**
   - Ler: `docs/ANALISE_TECNICA_CODIGO_LEGADO.md`
   - Analisar: `pdpw_act/pdpw/Dao/UsinaDAO.vb`
   - Analisar: `pdpw_act/pdpw/Dao/ArquivoDadgerValorDAO.vb`

3. ? **Criar tasks no board**
   - Usar GitHub Issues ou Projects
   - Atribuir tarefas conforme briefing

4. ? **Iniciar desenvolvimento**
   - DEV 1: Criar entidade `Usina`
   - DEV 2: Criar entidades DADGER
   - DEV 3: Analisar telas legadas
   - QA: Criar casos de teste

---

## ?? SUPORTE

### Problemas Técnicos
- Consultar: `docs/ANALISE_TECNICA_CODIGO_LEGADO.md`
- Issues no GitHub: https://github.com/wbulhoes/ONS_PoC-PDPW/issues

### Dúvidas sobre Arquitetura
- Consultar: `README.md`
- Consultar: `VERTICAL_SLICES_DECISION.md`

### Dúvidas sobre o Domínio
- Consultar: `GLOSSARIO.md`
- Analisar código legado em: `pdpw_act/pdpw/`

---

## ? CHECKLIST FINAL PRÉ-DESENVOLVIMENTO

### Todos os Devs
- [ ] Git instalado e configurado
- [ ] Repositório clonado
- [ ] Branch criada e pushed
- [ ] Documentação lida (README, BRIEFING, ANÁLISE TÉCNICA)
- [ ] Ambiente testado (build/run funciona)

### Backend Devs
- [ ] .NET 8 SDK instalado
- [ ] Visual Studio/Rider instalado
- [ ] Solução compila sem erros
- [ ] Swagger acessível

### Frontend Dev
- [ ] Node.js 20 instalado
- [ ] VS Code com extensões instalado
- [ ] npm install concluído
- [ ] Dev server executando

### QA
- [ ] Postman instalado
- [ ] Acesso ao repositório
- [ ] Casos de teste documentados
- [ ] Navegadores instalados

---

**?? SETUP CONCLUÍDO! PRONTO PARA DESENVOLVER!** ??

---

**Documento preparado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Versão:** 1.0  
**Próximo passo:** Daily Standup - 09:00 (20/12)
