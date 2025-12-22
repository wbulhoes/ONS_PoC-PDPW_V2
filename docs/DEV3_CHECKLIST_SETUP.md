# ?? CHECKLIST DE SETUP - DEV 3 (Frontend)

**Data:** 19/12/2024  
**Respons�vel:** DEV 3  
**Objetivo:** Validar ambiente antes de come�ar

---

## ? PR�-REQUISITOS

### 1. Git Configurado

```powershell
# Verificar vers�o
git --version
# Esperado: git version 2.x.x

# Verificar configura��o
git config --list | Select-String "user"
# Deve mostrar seu nome e email
```

**Checklist:**
- [ ] Git instalado (2.x+)
- [ ] Nome configurado (`git config user.name`)
- [ ] Email configurado (`git config user.email`)

---

### 2. Node.js e npm

```powershell
# Verificar Node.js
node --version
# Esperado: v20.x.x (m�nimo v18.x)

# Verificar npm
npm --version
# Esperado: 10.x.x (m�nimo 9.x)
```

**Checklist:**
- [ ] Node.js 20+ instalado
- [ ] npm 10+ instalado

**Se n�o estiver instalado:**
```powershell
# Baixar e instalar Node.js 20 LTS
# https://nodejs.org/en/download/

# Ou via Chocolatey (se tiver):
choco install nodejs-lts
```

---

### 3. Editor de C�digo (VS Code)

```powershell
# Verificar se VS Code est� instalado
code --version
```

**Checklist:**
- [ ] VS Code instalado
- [ ] Extens�es recomendadas instaladas

**Extens�es Recomendadas:**
```
1. ES7+ React/Redux/React-Native snippets
2. TypeScript Vue Plugin (Volar)
3. ESLint
4. Prettier - Code formatter
5. Auto Rename Tag
6. Path Intellisense
```

**Instalar via VS Code:**
```
Ctrl+Shift+X ? Buscar extens�o ? Install
```

---

### 4. Validar Reposit�rio

```powershell
# Navegar para o projeto
cd C:\temp\_ONS_PoC-PDPW

# Verificar reposit�rio
git remote -v
# Deve mostrar: origin  https://github.com/wbulhoes/ONS_PoC-PDPW

# Verificar branch atual
git branch
# Deve estar em: develop ou main
```

**Checklist:**
- [ ] Reposit�rio clonado em `C:\temp\_ONS_PoC-PDPW`
- [ ] Remote origin configurado
- [ ] Est� na branch develop

---

### 5. Criar Branch de Feature

```powershell
# Garantir que est� na develop atualizada
git checkout develop
git pull origin develop

# Criar nova branch para frontend
git checkout -b feature/frontend-usinas

# Verificar que est� na branch correta
git branch
# Deve mostrar: * feature/frontend-usinas

# Fazer push da branch vazia
git push -u origin feature/frontend-usinas
```

**Checklist:**
- [ ] Branch `feature/frontend-usinas` criada
- [ ] Branch enviada para o remote
- [ ] Git est� trackando a branch

---

### 6. Validar Estrutura do Projeto

```powershell
# Verificar estrutura do frontend
cd frontend
dir

# Deve existir:
# - package.json
# - package-lock.json
# - vite.config.ts
# - tsconfig.json
# - src/
# - public/
```

**Checklist:**
- [ ] Pasta `frontend/` existe
- [ ] Arquivo `package.json` existe
- [ ] Arquivo `package-lock.json` existe
- [ ] Pasta `src/` existe

---

### 7. Instalar Depend�ncias

```powershell
# Dentro da pasta frontend
cd C:\temp\_ONS_PoC-PDPW\frontend

# Limpar node_modules se existir (opcional)
# Remove-Item -Recurse -Force node_modules

# Instalar depend�ncias
npm install

# Aguardar... deve instalar ~226 packages
```

**Checklist:**
- [ ] `npm install` executado sem erros
- [ ] Pasta `node_modules/` criada
- [ ] Nenhum erro cr�tico mostrado

**Se der erro:**
```powershell
# Limpar cache
npm cache clean --force

# Deletar arquivos de lock e node_modules
Remove-Item -Force package-lock.json
Remove-Item -Recurse -Force node_modules

# Reinstalar
npm install
```

---

### 8. Testar Dev Server

```powershell
# Iniciar servidor de desenvolvimento
npm run dev

# Deve mostrar algo como:
# VITE v5.x.x  ready in 500 ms
# ?  Local:   http://localhost:5173/
# ?  Network: use --host to expose
```

**Checklist:**
- [ ] `npm run dev` inicia sem erros
- [ ] Servidor roda em `http://localhost:5173`
- [ ] Navegador abre automaticamente (ou abrir manualmente)
- [ ] P�gina React carrega sem erros

**Testar Hot Reload:**
```typescript
// Editar: src/App.tsx
// Trocar o t�tulo
<h1>TESTE - Hot Reload Funcionando!</h1>

// Salvar arquivo (Ctrl+S)
// Navegador deve atualizar automaticamente ?
```

**Parar servidor:**
```
Ctrl+C no terminal
```

---

### 9. Validar Backend (DEV 1 e DEV 2 devem ter iniciado)

```powershell
# Verificar se backend est� rodando
# Abrir navegador: http://localhost:5000/swagger

# Deve aparecer: Swagger UI com endpoints
```

**Checklist:**
- [ ] Backend est� rodando
- [ ] Swagger UI acess�vel
- [ ] Consegue ver endpoints (mesmo que poucos ainda)

**Se backend n�o estiver rodando:**
```powershell
# Avisar DEV 1 ou DEV 2
# Ou iniciar voc� mesmo:
cd C:\temp\_ONS_PoC-PDPW\src\PDPW.API
dotnet run
```

---

### 10. Configurar VS Code (Workspace Settings)

**Criar:** `.vscode/settings.json` na raiz do projeto

```json
{
  "editor.formatOnSave": true,
  "editor.defaultFormatter": "esbenp.prettier-vscode",
  "editor.codeActionsOnSave": {
    "source.fixAll.eslint": true
  },
  "typescript.tsdk": "frontend/node_modules/typescript/lib",
  "typescript.enablePromptUseWorkspaceTsdk": true,
  "files.exclude": {
    "**/node_modules": true,
    "**/dist": true,
    "**/.git": true
  }
}
```

---

## ? VALIDA��O FINAL

### Checklist Completo

- [ ] ? Git configurado e funcionando
- [ ] ? Node.js 20+ instalado
- [ ] ? npm 10+ instalado
- [ ] ? VS Code instalado e configurado
- [ ] ? Extens�es recomendadas instaladas
- [ ] ? Reposit�rio clonado
- [ ] ? Branch `feature/frontend-usinas` criada e pushed
- [ ] ? `npm install` executado com sucesso
- [ ] ? `npm run dev` funciona
- [ ] ? Hot reload testado e funcionando
- [ ] ? Backend acess�vel (Swagger)
- [ ] ? VS Code workspace configurado

---

## ?? PR�XIMO PASSO

**Se todos os itens acima est�o ?:**

? **Partir para PARTE 2: An�lise da Tela Legada**

---

## ?? TROUBLESHOOTING

### Erro: "npm install" falha

```powershell
# Solu��o 1: Limpar cache
npm cache clean --force
npm install

# Solu��o 2: Usar npm ci (se package-lock.json existe)
npm ci

# Solu��o 3: Deletar tudo e reinstalar
Remove-Item -Recurse -Force node_modules
Remove-Item -Force package-lock.json
npm install
```

### Erro: "npm run dev" falha

```powershell
# Ver erros detalhados
npm run dev --verbose

# Verificar se porta 5173 est� em uso
netstat -ano | findstr :5173

# Se estiver em uso, matar processo
taskkill /PID <numero> /F

# Ou usar outra porta
npm run dev -- --port 3000
```

### Erro: Git n�o reconhece comandos

```powershell
# Verificar se Git est� no PATH
$env:Path -split ';' | Select-String "Git"

# Se n�o estiver, adicionar:
# Painel de Controle ? Sistema ? Vari�veis de Ambiente
# Adicionar: C:\Program Files\Git\cmd
```

### Erro: Backend n�o est� acess�vel

```powershell
# Verificar se backend est� rodando
# Perguntar para DEV 1 ou DEV 2

# Ou iniciar voc� mesmo:
cd C:\temp\_ONS_PoC-PDPW\src\PDPW.API
dotnet run

# Aguardar at� ver:
# "Now listening on: http://localhost:5000"
```

---

**Checklist criado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Vers�o:** 1.0  
**Tempo estimado:** 45 minutos

**Ap�s completar este checklist, voc� est� pronto para come�ar o desenvolvimento! ??**
