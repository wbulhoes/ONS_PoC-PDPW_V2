# ?? CHECKLIST DE IN�CIO - PoC PDPW

**Squad:** 2 Backend Devs + 1 Frontend Dev  
**Data In�cio:** 19/12/2024  
**Data Entrega:** 26/12/2024

---

## ? PR�-REQUISITOS (Validar Hoje)

### Ambiente de Desenvolvimento

#### Todos os Devs
- [ ] Git instalado e configurado
- [ ] Acesso ao reposit�rio: https://github.com/wbulhoes/ONS_PoC-PDPW
- [ ] Branch `develop` clonada localmente
- [ ] Docker Desktop instalado e rodando
- [ ] `docker-compose up` testado e funcionando

#### Backend Devs (DEV 1 e DEV 2)
- [ ] .NET 8 SDK instalado (`dotnet --version`)
- [ ] Visual Studio 2022 ou Rider instalado
- [ ] `dotnet build` na solu��o funciona sem erros
- [ ] `dotnet run` na API funciona
- [ ] Swagger acess�vel: http://localhost:5000/swagger

#### Frontend Dev (DEV 3)
- [ ] Node.js 20+ instalado (`node --version`)
- [ ] VS Code instalado
- [ ] `npm install` no frontend funciona
- [ ] `npm run dev` funciona
- [ ] App acess�vel: http://localhost:5173

---

## ??? ESTRUTURA DE BRANCHES

### Branch Estrat�gia

```
main (produ��o)
  ?? develop (desenvolvimento)
      ?? feature/gestao-ativos (APIs: Usina, Empresa, etc.)
      ?? feature/arquivos-dados (APIs: DADGER, SemanaPMO, etc.)
      ?? feature/restricoes (APIs: RestricaoUG, ParadaUG, etc.)
      ?? feature/operacao (APIs: Intercambio, Balanco, etc.)
      ?? feature/consolidados (APIs: DCA, DCR)
      ?? feature/equipes (APIs: Usuario, Responsavel, etc.)
      ?? feature/documentos (APIs: Arquivo, Relatorio, etc.)
      ?? feature/frontend-usinas (Tela de Usinas)
```

### Criar Branches Iniciais

```powershell
# DEV 1 - Backend Senior (Gest�o de Ativos)
git checkout develop
git pull origin develop
git checkout -b feature/gestao-ativos
git push -u origin feature/gestao-ativos

# DEV 2 - Backend Pleno (Arquivos e Dados)
git checkout develop
git checkout -b feature/arquivos-dados
git push -u origin feature/arquivos-dados

# DEV 3 - Frontend (Tela de Usinas)
git checkout develop
git checkout -b feature/frontend-usinas
git push -u origin feature/frontend-usinas
```

---

## ?? CRONOGRAMA DETALHADO

### DIA 1: Quinta (19/12) - Setup + Primeiras APIs

#### DEV 1 (Backend Senior) - 8h
**Branch:** `feature/gestao-ativos`

**Manh� (4h):**
- [ ] 09:00-10:00 - Setup ambiente + estrutura base
- [ ] 10:00-13:00 - **API 1: Usina** (entidade + repository + service + controller + seed)

**Tarde (4h):**
- [ ] 14:00-16:00 - **API 2: Empresa**
- [ ] 16:00-18:00 - **API 3: TipoUsina**

**Entreg�veis:**
- [ ] 3 APIs completas (15 endpoints)
- [ ] Swagger documentado
- [ ] Seed data populado
- [ ] Testes b�sicos

**Commit:**
```bash
git add .
git commit -m "[GESTAO-ATIVOS] feat: adiciona APIs Usina, Empresa e TipoUsina

- Entidades domain criadas
- Repositories implementados
- Services com valida��es
- Controllers com 15 endpoints
- Seed data com dados realistas
- Testes unit�rios b�sicos"
git push origin feature/gestao-ativos
```

#### DEV 2 (Backend Pleno) - 8h
**Branch:** `feature/arquivos-dados`

**Manh� (4h):**
- [ ] 09:00-10:00 - Setup ambiente + estrutura base
- [ ] 10:00-13:00 - **API 11: UnidadeGeradora**

**Tarde (4h):**
- [ ] 14:00-16:30 - **API 12: ParadaUG**
- [ ] 16:30-18:00 - In�cio **API 13: RestricaoUG** (parcial)

**Entreg�veis:**
- [ ] 2 APIs completas (10 endpoints)
- [ ] Swagger documentado
- [ ] Seed data populado

**Commit:**
```bash
git add .
git commit -m "[ARQUIVOS-DADOS] feat: adiciona APIs UnidadeGeradora e ParadaUG

- Entidades criadas com relacionamentos
- Repositories com filtros
- Services implementados
- Controllers com 10 endpoints
- Seed data"
git push origin feature/arquivos-dados
```

#### DEV 3 (Frontend) - 8h
**Branch:** `feature/frontend-usinas`

**Manh� (4h):**
- [ ] 09:00-10:00 - Setup Node.js, React, Vite
- [ ] 10:00-12:00 - Analisar tela legada (`pdpw_act/pdpw/frmCadUsina.aspx`)
- [ ] 12:00-13:00 - Criar estrutura de componentes

**Tarde (4h):**
- [ ] 14:00-16:00 - Componente de listagem de usinas
- [ ] 16:00-18:00 - Componente de formul�rio (estrutura)

**Entreg�veis:**
- [ ] Estrutura de pastas criada
- [ ] Servi�o API (Axios) configurado
- [ ] Componente de listagem (b�sico)
- [ ] Componente de formul�rio (estrutura)

**Commit:**
```bash
git add .
git commit -m "[FRONTEND] feat: estrutura inicial tela de Usinas

- Configura��o React + TypeScript
- Servi�o de API (Axios)
- Componente de listagem
- Componente de formul�rio (estrutura)
- Integra��o com backend"
git push origin feature/frontend-usinas
```

---

### DIA 2: Sexta (20/12) - APIs Core + Frontend 90%

#### DEV 1 (Backend Senior) - 8h
**Branch:** `feature/gestao-ativos`

**Manh� (4h):**
- [ ] 09:00-11:30 - **API 4: SemanaPMO**
- [ ] 11:30-13:00 - **API 8: EquipePDP**

**Tarde (4h):**
- [ ] 14:00-18:00 - **API 5: ArquivoDadger** (complexa - 4h)

**Entreg�veis:**
- [ ] 3 APIs completas (15 endpoints)
- [ ] Relacionamentos funcionando
- [ ] Swagger atualizado

**Commit:**
```bash
git commit -m "[GESTAO-ATIVOS] feat: adiciona APIs SemanaPMO, EquipePDP e ArquivoDadger

- SemanaPMO com filtros por per�odo
- EquipePDP com CRUD completo
- ArquivoDadger com relacionamentos complexos
- Seed data enriquecido"
```

#### DEV 2 (Backend Pleno) - 8h
**Branch:** `feature/restricoes` (nova branch)

**Manh� (4h):**
- [ ] 09:00-10:30 - Concluir **API 13: RestricaoUG**
- [ ] 10:30-13:00 - **API 14: RestricaoUS**

**Tarde (4h):**
- [ ] 14:00-15:30 - **API 15: MotivoRestricao**
- [ ] 15:30-18:00 - In�cio **API 16: Intercambio**

**Entreg�veis:**
- [ ] 3 APIs completas (14 endpoints)

**Commit:**
```bash
git checkout -b feature/restricoes
git commit -m "[RESTRICOES] feat: adiciona APIs Restricao e Motivos

- RestricaoUG com valida��es
- RestricaoUS implementada
- MotivoRestricao (enumera��o)
- Filtros por usina e per�odo"
```

#### DEV 3 (Frontend) - 8h
**Branch:** `feature/frontend-usinas`

**Manh� (4h):**
- [ ] 09:00-11:00 - Integra��o com API de Usinas (GET/POST/PUT/DELETE)
- [ ] 11:00-13:00 - Valida��es de formul�rio

**Tarde (4h):**
- [ ] 14:00-16:00 - Filtros e busca
- [ ] 16:00-18:00 - Mensagens de erro/sucesso + polish

**Entreg�veis:**
- [ ] Tela de Usinas 90% completa
- [ ] CRUD funcional
- [ ] Valida��es implementadas

**Commit:**
```bash
git commit -m "[FRONTEND] feat: CRUD de Usinas completo

- Listagem com filtros
- Formul�rio de cria��o/edi��o
- Valida��es em tempo real
- Integra��o completa com API
- Mensagens de feedback"
```

---

### DIA 3: S�bado (21/12) - APIs Complexas

#### DEV 1 (Backend Senior) - 8h

**Manh� (4h):**
- [ ] 09:00-13:00 - **API 6: ArquivoDadgerValor** (muito complexa - 4h)

**Tarde (4h):**
- [ ] 14:00-16:30 - **API 7: Carga**
- [ ] 16:30-18:00 - In�cio **API 9: Usuario**

**Entreg�veis:**
- [ ] 2 APIs completas + 1 parcial (16 endpoints)

#### DEV 2 (Backend Pleno) - 8h

**Manh� (4h):**
- [ ] 09:00-10:00 - Concluir **API 16: Intercambio**
- [ ] 10:00-13:30 - **API 17: Balanco**

**Tarde (4h):**
- [ ] 14:00-16:30 - **API 18: GerForaMerito**
- [ ] 16:30-18:00 - In�cio **API 19: DCA**

**Entreg�veis:**
- [ ] 3 APIs completas + 1 parcial (16 endpoints)

#### DEV 3 (Frontend) - 8h

**Manh� (4h):**
- [ ] 09:00-13:00 - Finalizar tela de Usinas (�ltimos ajustes)

**Tarde (4h):**
- [ ] 14:00-18:00 - Testes E2E + Responsividade + Documenta��o

**Entreg�veis:**
- [ ] Tela de Usinas 100% completa e testada

---

### DIA 4: Domingo (22/12) - Finaliza��o Backend

#### DEV 1 (Backend Senior) - 8h

**Manh� (4h):**
- [ ] 09:00-10:00 - Concluir **API 9: Usuario**
- [ ] 10:00-12:00 - **API 10: Responsavel**
- [ ] 12:00-13:00 - Buffer/ajustes

**Tarde (4h):**
- [ ] 14:00-17:30 - **API 20: DCR**
- [ ] 17:30-18:00 - Testes de integra��o

**Entreg�veis:**
- [ ] 3 APIs (16 endpoints)

#### DEV 2 (Backend Pleno) - 8h

**Manh� (4h):**
- [ ] 09:00-11:00 - Concluir **API 19: DCA**
- [ ] 11:00-13:00 - **API 21: Observacao**

**Tarde (4h):**
- [ ] 14:00-16:00 - **API 22: Diretorio**
- [ ] 16:00-18:00 - **API 23: Arquivo** (parcial)

**Entreg�veis:**
- [ ] 3 APIs completas + 1 parcial (19 endpoints)

#### DEV 3 (Frontend) - FOLGA
- [ ] Tela de Usinas j� completa
- [ ] Pode auxiliar em documenta��o ou QA

---

### DIA 5: Segunda (23/12) - APIs Extras + Testes

#### DEV 1 (Backend Senior) - 8h

**Manh� (4h):**
- [ ] 09:00-12:00 - **API 24: Upload**

**Tarde (4h):**
- [ ] 13:00-16:00 - **API 25: Relatorio**
- [ ] 16:00-18:00 - Testes de integra��o + Code review

**Entreg�veis:**
- [ ] 2 APIs (9 endpoints)

#### DEV 2 (Backend Pleno) - 8h

**Manh� (4h):**
- [ ] 09:00-09:30 - Concluir **API 23: Arquivo**
- [ ] 09:30-12:00 - **API 26: ModalidadeOpTermica**

**Tarde (4h):**
- [ ] 13:00-16:30 - **API 27: InflexibilidadeContratada**
- [ ] 16:30-18:00 - Testes + ajustes

**Entreg�veis:**
- [ ] 3 APIs (16 endpoints)

#### DEV 3 (Frontend) - 8h
- [ ] 09:00-18:00 - QA intensivo da tela de Usinas + Documenta��o

---

### DIA 6: Ter�a (24/12) - Finaliza��o e Docker

#### DEV 1 (Backend Senior) - 4h (meio per�odo)

**Manh� (4h):**
- [ ] 09:00-11:00 - **API 28: RampasUsinaTermica** (parcial)
- [ ] 11:00-13:00 - Docker Compose + Swagger final + Testes

**Entreg�veis:**
- [ ] Docker funcionando
- [ ] Swagger 100% documentado

#### DEV 2 (Backend Pleno) - 4h (meio per�odo)

**Manh� (4h):**
- [ ] 09:00-11:30 - **API 29: UsinaConversora**
- [ ] 11:30-13:00 - Testes finais + Seed data completo

**Entreg�veis:**
- [ ] �ltima API
- [ ] Seed data validado

#### DEV 3 (Frontend) - 4h (meio per�odo)
- [ ] 09:00-13:00 - Documenta��o final + Preparar demonstra��o

---

## ?? MERGE STRATEGY

### Merge para Develop (Fim de Cada Dia)

```powershell
# Exemplo: DEV 1 fim do Dia 1

# Garantir que est� atualizado
git checkout develop
git pull origin develop

# Merge da feature
git merge feature/gestao-ativos --no-ff

# Resolver conflitos (se houver)

# Testar que n�o quebrou nada
dotnet build
dotnet test

# Push
git push origin develop

# Voltar para feature branch
git checkout feature/gestao-ativos
```

### Pull Request (Opcional - se quiser review)

```
1. Push feature branch
2. Criar PR no GitHub
3. Aguardar review
4. Merge ap�s aprova��o
```

---

## ?? DAILY STANDUP (09:00 - 15 min)

### Template de Daily

```
DATA: __/__/2024

DEV 1 (Backend Senior):
? Ontem:
  - API X completa (Y endpoints)
  - API Z em progresso

?? Hoje:
  - Completar API Z
  - Iniciar API W

?? Bloqueios:
  - Nenhum / [Descrever se houver]

---

DEV 2 (Backend Pleno):
? Ontem:
  - [...]

?? Hoje:
  - [...]

?? Bloqueios:
  - [...]

---

DEV 3 (Frontend):
? Ontem:
  - [...]

?? Hoje:
  - [...]

?? Bloqueios:
  - [...]

---

DECIS�ES:
- [Se houver]

A��ES:
- [ ] A��o 1 (respons�vel)
```

---

## ? VALIDA��O DE ENTREGAS

### Checklist por API

Antes de considerar uma API "completa":

- [ ] **Entidade** criada no Domain
- [ ] **Interface** de reposit�rio no Domain
- [ ] **Repository** implementado no Infrastructure
- [ ] **DTOs** criados (Request, Response, Update)
- [ ] **Service** implementado com valida��es
- [ ] **Controller** com todos os endpoints
- [ ] **Swagger** documentado (XML comments)
- [ ] **Seed data** populado (5-10 registros)
- [ ] **Testes** b�sicos funcionando
- [ ] **Build** sem erros
- [ ] **Testado** via Swagger (Try it out)

---

## ?? CRIT�RIOS DE SUCESSO

### M�nimo Aceit�vel (Dia 6 - 24/12)

- [ ] M�nimo 25 APIs backend completas
- [ ] 130+ endpoints funcionando
- [ ] Swagger 100% documentado
- [ ] 1 tela frontend completa (Usinas)
- [ ] Docker Compose executando
- [ ] Build sem erros
- [ ] Seed data populado

### Meta Ideal (Dia 6 - 24/12)

- [ ] 27-29 APIs backend completas
- [ ] 145-160 endpoints funcionando
- [ ] Swagger 100% documentado
- [ ] 1 tela frontend completa (Usinas)
- [ ] Docker Compose executando
- [ ] Testes (cobertura > 60%)
- [ ] Seed data realista
- [ ] README atualizado

---

## ?? COMUNICA��O

### Canais

- **GitHub Issues:** Rastreamento de tarefas
- **GitHub Projects:** Board Kanban
- **Teams/Slack:** Comunica��o r�pida
- **Daily Standup:** 09:00 (15 min)

### Padr�o de Commits

```bash
[CATEGORIA] tipo: descri��o

Categorias:
- [GESTAO-ATIVOS]
- [ARQUIVOS-DADOS]
- [RESTRICOES]
- [OPERACAO]
- [CONSOLIDADOS]
- [EQUIPES]
- [DOCUMENTOS]
- [FRONTEND]
- [DOCS]
- [TEST]
- [DOCKER]

Tipos:
- feat: nova funcionalidade
- fix: corre��o de bug
- docs: documenta��o
- test: testes
- refactor: refatora��o
- chore: tarefas gerais
```

---

## ?? TROUBLESHOOTING

### Build Falha

```powershell
# Limpar e rebuild
dotnet clean
dotnet restore
dotnet build
```

### Docker N�o Sobe

```powershell
# Ver logs
docker-compose logs

# Rebuild for�ado
docker-compose down -v
docker-compose up --build --force-recreate
```

### Merge Conflicts

```powershell
# Ver arquivos em conflito
git status

# Editar manualmente
# Aceitar changes de ambos os lados se poss�vel

# Adicionar resolvidos
git add .

# Continuar merge
git merge --continue
```

---

## ?? CHECKPOINT 1 (21/12 - S�bado)

### Validar Progresso

- [ ] 16 APIs completas (~55%)
- [ ] 86+ endpoints funcionando
- [ ] Tela de Usinas 100% completa
- [ ] Docker funcionando
- [ ] Swagger organizado

**Se abaixo de 15 APIs:** Revisar prioridades e velocidade

---

## ?? CHECKPOINT 2 (24/12 - Ter�a Manh�)

### Valida��o Final

- [ ] 25+ APIs completas
- [ ] 130+ endpoints funcionando
- [ ] Tela de Usinas completa
- [ ] Docker funcionando
- [ ] Swagger 100% documentado
- [ ] README atualizado

**Se OK:** Preparar apresenta��o  
**Se n�o:** Focar nas APIs mais importantes

---

## ?? ENTREGA (26/12 - Quinta)

### Prepara��o da Apresenta��o (25/12 - Opcional)

- [ ] Ensaiar demonstra��o
- [ ] Testar todos os endpoints cr�ticos
- [ ] Verificar Docker
- [ ] Revisar Swagger
- [ ] Preparar slides (opcional)

### Apresenta��o (26/12)

- [ ] Demo ao vivo (15 min)
- [ ] Q&A (15 min)
- [ ] Pr�ximos passos (10 min)

---

**Checklist criado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Vers�o:** 1.0  
**Status:** ? PRONTO PARA IN�CIO

**BORA COME�AR! ??**
