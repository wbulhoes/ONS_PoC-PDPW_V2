# 🌳 ESTRUTURA DE BRANCHES - REPOSITÓRIO ORGANIZADO

**Repositório**: https://github.com/wbulhoes/ONS_PoC-PDPW_V2  
**Data**: 23/12/2024  
**Status**: ✅ Organizado e Configurado

---

## 📊 ESTRUTURA ATUAL

### **Branches Mantidas (3):**

```
origin (wbulhoes/ONS_PoC-PDPW_V2):
├── main              [Produção/Release]
├── develop           [Desenvolvimento/Integração]
└── feature/backend   [Trabalho Ativo] ⭐ BRANCH PRINCIPAL DE TRABALHO
```

### **Branches Removidas (4):**
```
✅ feature/arquivos-dados       (removida)
✅ feature/frontend-usinas      (removida)
✅ feature/gestao-ativos        (removida)
✅ integracao/preparar-pr-squad (removida)
```

---

## ⚙️ CONFIGURAÇÃO APLICADA

### **1. Branch de Trabalho Padrão**
```bash
Branch ativa: feature/backend
Upstream:     origin/feature/backend
Push padrão:  current (sempre para branch atual)
```

### **2. Aliases Git Criados**
```bash
git sync   → git fetch origin && git merge origin/feature/backend
git pushf  → git push origin feature/backend
git pullf  → git pull origin feature/backend
```

---

## 🚀 WORKFLOW SIMPLIFICADO

### **Fluxo de Trabalho Diário:**

```bash
# 1. Fazer alterações no código
# ...editar arquivos...

# 2. Adicionar ao stage
git add .

# 3. Commit
git commit -m "feat: descrição da mudança"

# 4. Push (vai automaticamente para feature/backend)
git push

# 5. Pull (quando necessário)
git pull
```

### **Comandos Úteis:**

```bash
# Ver status
git status

# Sincronizar com remote
git sync

# Ver branches locais
git branch

# Ver branches remotas
git branch -r

# Mudar de branch (se necessário)
git checkout main       # Para ver produção
git checkout develop    # Para ver desenvolvimento
git checkout feature/backend  # Voltar para trabalho
```

---

## 📋 ESTRATÉGIA DE BRANCHES

### **main (Produção)**
- ✅ Código estável e testado
- ✅ Pronto para apresentação ao cliente
- ⚠️ **NÃO** fazer commit direto
- ✅ Merge apenas de `develop` após testes

### **develop (Desenvolvimento)**
- ✅ Integração de features
- ✅ Código em desenvolvimento
- ⚠️ **NÃO** fazer commit direto
- ✅ Merge de branches `feature/*`

### **feature/backend (Trabalho Ativo)** ⭐
- ✅ **SEU TRABALHO DIÁRIO AQUI**
- ✅ Commits frequentes permitidos
- ✅ Push automático configurado
- ✅ Merge para `develop` quando feature estiver pronta

---

## 🔄 FLUXO DE MERGE

```
┌─────────────────────────────────────────────┐
│  1. Trabalho em feature/backend             │
│     git add .                                │
│     git commit -m "..."                      │
│     git push                                 │
└──────────────┬──────────────────────────────┘
               │
┌──────────────▼──────────────────────────────┐
│  2. Quando feature estiver pronta:          │
│     git checkout develop                     │
│     git merge feature/backend                │
│     git push origin develop                  │
└──────────────┬──────────────────────────────┘
               │
┌──────────────▼──────────────────────────────┐
│  3. Quando develop estiver estável:         │
│     git checkout main                        │
│     git merge develop                        │
│     git push origin main                     │
│     git tag v1.0.0                           │
└─────────────────────────────────────────────┘
```

---

## 🛠️ SCRIPTS DISPONÍVEIS

### **1. `scripts/setup-git-workflow.ps1`**
Configura o workflow Git para trabalhar com `feature/backend`.

```powershell
.\scripts\setup-git-workflow.ps1
```

**O que faz:**
- ✅ Muda para branch `feature/backend`
- ✅ Configura upstream
- ✅ Cria aliases úteis
- ✅ Configura push padrão

### **2. `scripts/cleanup-branches.ps1`**
Remove branches desnecessárias do remote.

```powershell
.\scripts\cleanup-branches.ps1
```

**O que faz:**
- ✅ Lista branches remotas
- ✅ Identifica branches para remover
- ✅ Remove com confirmação
- ✅ Faz prune local

---

## 📊 VERIFICAÇÃO

### **Verificar Estrutura Atual:**

```powershell
# Ver branches locais
git branch

# Ver branches remotas
git branch -r

# Ver configuração atual
git remote -v
git config --get-regexp alias
```

**Resultado Esperado:**

```
Branches remotas:
  origin/develop
  origin/feature/backend
  origin/main

Aliases:
alias.sync !git fetch origin && git merge origin/feature/backend
alias.pushf push origin feature/backend
alias.pullf pull origin feature/backend
```

---

## 🎯 REMOTES CONFIGURADOS

```
origin     → https://github.com/wbulhoes/ONS_PoC-PDPW_V2.git
meu-fork   → https://github.com/wbulhoes/POCMigracaoPDPw.git
squad      → https://github.com/RafaelSuzanoACT/POCMigracaoPDPw.git
```

### **Push Padrão:**
- ✅ `git push` → `origin/feature/backend`
- ⚠️ Para outros remotes, usar: `git push meu-fork` ou `git push squad`

---

## 💡 BOAS PRÁTICAS

### **✅ DO (Faça):**
1. Trabalhe sempre em `feature/backend`
2. Commits frequentes e descritivos
3. Push regularmente (backup na nuvem)
4. Merge para `develop` quando feature estiver completa
5. Use mensagens de commit semânticas:
   - `feat:` - Nova funcionalidade
   - `fix:` - Correção de bug
   - `chore:` - Tarefas gerais (config, docs)
   - `docs:` - Documentação
   - `refactor:` - Refatoração

### **❌ DON'T (Não faça):**
1. ❌ Commit direto em `main`
2. ❌ Commit direto em `develop`
3. ❌ Deletar branches sem verificar
4. ❌ Force push sem necessidade
5. ❌ Trabalhar em múltiplas branches simultaneamente

---

## 🚨 TROUBLESHOOTING

### **Problema: "Push foi para branch errada"**

**Solução:**
```powershell
# Re-executar configuração
.\scripts\setup-git-workflow.ps1
```

### **Problema: "Branch remota ainda aparece após deletar"**

**Solução:**
```powershell
git fetch origin --prune
```

### **Problema: "Quero mudar para outra branch"**

**Solução:**
```powershell
git checkout nome-da-branch

# Para voltar para feature/backend:
git checkout feature/backend
```

### **Problema: "Quero desfazer último commit"**

**Solução:**
```powershell
# Desfaz commit mas mantém alterações
git reset --soft HEAD~1

# Desfaz commit E descarta alterações (CUIDADO!)
git reset --hard HEAD~1
```

---

## 📚 REFERÊNCIAS

### **Documentação:**
- [Git Branching Strategy](https://git-scm.com/book/en/v2/Git-Branching-Branching-Workflows)
- [Conventional Commits](https://www.conventionalcommits.org/)

### **Arquivos Relacionados:**
- `scripts/setup-git-workflow.ps1` - Configuração de workflow
- `scripts/cleanup-branches.ps1` - Limpeza de branches
- `.gitattributes` - Configuração de encoding
- `.editorconfig` - Padrões de formatação

---

## ✅ CHECKLIST DE VERIFICAÇÃO

```
┌─────────────────────────────────────────────┐
│  CHECKLIST: ESTRUTURA DE BRANCHES           │
├─────────────────────────────────────────────┤
│  ✅ Branches remotas: main, develop, feature/backend │
│  ✅ Branch ativa: feature/backend           │
│  ✅ Upstream configurado                    │
│  ✅ Push padrão configurado                 │
│  ✅ Aliases criados                         │
│  ✅ Branches antigas removidas              │
│  ✅ Prune executado                         │
│  ✅ Scripts criados                         │
│  ✅ Documentação atualizada                 │
└─────────────────────────────────────────────┘
```

---

## 🎉 CONCLUSÃO

**Repositório organizado com sucesso!** ✅

A partir de agora:
- ✅ Trabalhe em `feature/backend`
- ✅ Use `git push` normalmente (vai para lugar certo)
- ✅ Estrutura limpa e profissional
- ✅ Fácil de entender e manter

**🚀 Bom trabalho!**

---

**📅 Última Atualização**: 23/12/2024  
**👤 Responsável**: Willian Bulhões  
**🔗 Repositório**: https://github.com/wbulhoes/ONS_PoC-PDPW_V2
