# ?? CHECKLIST DE SETUP - DEV 3 (Frontend)

**Data:** 19/12/2024  
**Responsável:** DEV 3  
**Objetivo:** Validar ambiente antes de começar

---

## ? PRÉ-REQUISITOS

### 1. Git Configurado

```powershell
# Verificar versão
git --version
# Esperado: git version 2.x.x

# Verificar configuração
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
# Esperado: v20.x.x (mínimo v18.x)

# Verificar npm
npm --version
# Esperado: 10.x.x (mínimo 9.x)
```

**Checklist:**
- [ ] Node.js 20+ instalado
- [ ] npm 10+ instalado

**Se não estiver instalado:**
```powershell
# Baixar e instalar Node.js 20 LTS
# https://nodejs.org/en/download/

# Ou via Chocolatey (se tiver):
choco install nodejs-lts
```

---

### 3. Editor de Código (VS Code)

```powershell
# Verificar se VS Code está instalado
code --version
```

**Checklist:**
- [ ] VS Code instalado
- [ ] Extensões recomendadas instaladas

**Extensões Recomendadas:**
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
Ctrl+Shift+X ? Buscar extensão ? Install
```

---

### 4. Validar Repositório

```powershell
# Navegar para o projeto
cd C:\temp\_ONS_PoC-PDPW

# Verificar repositório
git remote -v
# Deve mostrar: origin  https://github.com/wbulhoes/ONS_PoC-PDPW

# Verificar branch atual
git branch
# Deve estar em: develop ou main
```

**Checklist:**
- [ ] Repositório clonado em `C:\temp\_ONS_PoC-PDPW`
- [ ] Remote origin configurado
- [ ] Está na branch develop

---

### 5. Criar Branch de Feature

```powershell
# Garantir que está na develop atualizada
git checkout develop
git pull origin develop

# Criar nova branch para frontend
git checkout -b feature/frontend-usinas

# Verificar que está na branch correta
git branch
# Deve mostrar: * feature/frontend-usinas

# Fazer push da branch vazia
git push -u origin feature/frontend-usinas
```

**Checklist:**
- [ ] Branch `feature/frontend-usinas` criada
- [ ] Branch enviada para o remote
- [ ] Git está trackando a branch

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

### 7. Instalar Dependências

```powershell
# Dentro da pasta frontend
cd C:\temp\_ONS_PoC-PDPW\frontend

# Limpar node_modules se existir (opcional)
# Remove-Item -Recurse -Force node_modules

# Instalar dependências
npm install

# Aguardar... deve instalar ~226 packages
```

**Checklist:**
- [ ] `npm install` executado sem erros
- [ ] Pasta `node_modules/` criada
- [ ] Nenhum erro crítico mostrado

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
- [ ] Página React carrega sem erros

**Testar Hot Reload:**
```typescript
// Editar: src/App.tsx
// Trocar o título
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
# Verificar se backend está rodando
# Abrir navegador: http://localhost:5000/swagger

# Deve aparecer: Swagger UI com endpoints
```

**Checklist:**
- [ ] Backend está rodando
- [ ] Swagger UI acessível
- [ ] Consegue ver endpoints (mesmo que poucos ainda)

**Se backend não estiver rodando:**
```powershell
# Avisar DEV 1 ou DEV 2
# Ou iniciar você mesmo:
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

## ? VALIDAÇÃO FINAL

### Checklist Completo

- [ ] ? Git configurado e funcionando
- [ ] ? Node.js 20+ instalado
- [ ] ? npm 10+ instalado
- [ ] ? VS Code instalado e configurado
- [ ] ? Extensões recomendadas instaladas
- [ ] ? Repositório clonado
- [ ] ? Branch `feature/frontend-usinas` criada e pushed
- [ ] ? `npm install` executado com sucesso
- [ ] ? `npm run dev` funciona
- [ ] ? Hot reload testado e funcionando
- [ ] ? Backend acessível (Swagger)
- [ ] ? VS Code workspace configurado

---

## ?? PRÓXIMO PASSO

**Se todos os itens acima estão ?:**

? **Partir para PARTE 2: Análise da Tela Legada**

---

## ?? TROUBLESHOOTING

### Erro: "npm install" falha

```powershell
# Solução 1: Limpar cache
npm cache clean --force
npm install

# Solução 2: Usar npm ci (se package-lock.json existe)
npm ci

# Solução 3: Deletar tudo e reinstalar
Remove-Item -Recurse -Force node_modules
Remove-Item -Force package-lock.json
npm install
```

### Erro: "npm run dev" falha

```powershell
# Ver erros detalhados
npm run dev --verbose

# Verificar se porta 5173 está em uso
netstat -ano | findstr :5173

# Se estiver em uso, matar processo
taskkill /PID <numero> /F

# Ou usar outra porta
npm run dev -- --port 3000
```

### Erro: Git não reconhece comandos

```powershell
# Verificar se Git está no PATH
$env:Path -split ';' | Select-String "Git"

# Se não estiver, adicionar:
# Painel de Controle ? Sistema ? Variáveis de Ambiente
# Adicionar: C:\Program Files\Git\cmd
```

### Erro: Backend não está acessível

```powershell
# Verificar se backend está rodando
# Perguntar para DEV 1 ou DEV 2

# Ou iniciar você mesmo:
cd C:\temp\_ONS_PoC-PDPW\src\PDPW.API
dotnet run

# Aguardar até ver:
# "Now listening on: http://localhost:5000"
```

---

**Checklist criado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Versão:** 1.0  
**Tempo estimado:** 45 minutos

**Após completar este checklist, você está pronto para começar o desenvolvimento! ??**
