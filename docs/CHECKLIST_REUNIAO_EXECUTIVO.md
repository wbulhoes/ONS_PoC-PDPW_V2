# ? CHECKLIST EXECUTIVO - REUNI�O DO SQUAD

**Data:** 19/12/2024 - 15:00h  
**Objetivo:** Kick-off do desenvolvimento da PoC PDPW  
**Dura��o:** 45 minutos

---

## ?? PR�-REUNI�O (Prepara��o)

### Documentos a Ter Abertos
- [ ] `docs/APRESENTACAO_REUNIAO_SQUAD.md` (este documento)
- [ ] `docs/SQUAD_BRIEFING_19DEC.md` (briefing detalhado)
- [ ] `docs/ANALISE_TECNICA_CODIGO_LEGADO.md` (an�lise t�cnica)
- [ ] `docs/SETUP_AMBIENTE_GUIA.md` (guia de instala��o)

### Verifica��es T�cnicas
- [ ] Reposit�rio est� no GitHub e acess�vel a todos
- [ ] Branch `develop` existe e est� atualizada
- [ ] C�digo legado est� em `pdpw_act/pdpw/`
- [ ] Documenta��o existente revisada

### Materiais de Apoio
- [ ] Projetor/tela compartilhamento conectado
- [ ] Acesso ao reposit�rio GitHub
- [ ] Acesso ao c�digo legado
- [ ] Swagger da API atual (se houver)

---

## ?? DURANTE A REUNI�O

### ABERTURA (2 min) - 15:00-15:02

**Script:**
> "Boa tarde, pessoal! Obrigado por estarem aqui. Esta � nossa reuni�o de kick-off da PoC PDPW. Temos 7 dias �teis para entregar 2 fluxos completos do sistema. Vamos come�ar?"

**Checklist:**
- [ ] Todos os participantes presentes (3 devs + 1 QA)
- [ ] Reuni�o gravada (se aplic�vel)
- [ ] Timer iniciado

---

### 1. CONTEXTO DO PROJETO (5 min) - 15:02-15:07

**Pontos a cobrir:**
- [ ] **Objetivo:** Modernizar PDPW de VB.NET/WebForms para .NET 8/React
- [ ] **Cliente:** ONS (Operador Nacional do Sistema El�trico)
- [ ] **Prazo:** Entrega 26/12, Apresenta��o 05/01, Estimativa 12/01
- [ ] **Escopo:** 2 vertical slices (Usinas + DADGER)
- [ ] **Stack:** .NET 8 + React 18 + Docker + SQL Server InMemory

**Perguntas a fazer:**
- [ ] "Todos entenderam o que � uma PoC?"
- [ ] "Algu�m tem d�vidas sobre o prazo?"
- [ ] "Todos sabem o que � um vertical slice?"

**? Crit�rio de sucesso:** Todos entendem o objetivo e o prazo

---

### 2. AN�LISE DO C�DIGO LEGADO (10 min) - 15:07-15:17

**Estat�sticas a apresentar:**
- [ ] 473 arquivos VB.NET
- [ ] 168 p�ginas ASPX (WebForms)
- [ ] .NET Framework 4.8 + SQL Server
- [ ] Arquitetura em 3 camadas (DAO/Business/DTO)

**Demonstra��o pr�tica:**
- [ ] Abrir `pdpw_act/pdpw/Dao/UsinaDAO.vb`
- [ ] Mostrar query SQL inline
- [ ] Mostrar mapeamento manual com SqlDataReader
- [ ] Abrir `pdpw_act/pdpw/DTOs/UsinaDTO.vb`
- [ ] Mostrar propriedades da entidade

**Pontos a destacar:**
- [ ] ? C�digo bem estruturado
- [ ] ? Separa��o de responsabilidades clara
- [ ] ?? SQL inline (vulner�vel a injection)
- [ ] ?? Sem ORM moderno
- [ ] ?? VB.NET precisa ser convertido para C#

**Perguntas a fazer:**
- [ ] "Algu�m j� trabalhou com VB.NET?"
- [ ] "Algu�m tem d�vidas sobre a arquitetura legada?"
- [ ] "Ficou claro o que precisamos migrar?"

**? Crit�rio de sucesso:** Todos entendem o c�digo legado e os desafios

---

### 3. DIVIS�O DE TAREFAS (15 min) - 15:17-15:32

#### DEV 1 - Backend: SLICE 1 (Usinas) - 5 min

**Responsabilidades:**
- [ ] CRUD completo de Usinas
- [ ] 6 endpoints REST (GET/POST/PUT/DELETE)
- [ ] Valida��es de neg�cio
- [ ] Testes unit�rios

**Entreg�veis:**
- [ ] Entidade `Usina.cs` no Domain
- [ ] Reposit�rio no Infrastructure
- [ ] Service na Application
- [ ] Controller na API
- [ ] Swagger documentado

**Prazo:** 20/12 (Sexta) - 2 dias

**Perguntas ao DEV 1:**
- [ ] "Voc� j� trabalhou com Clean Architecture?"
- [ ] "Tem experi�ncia com EF Core?"
- [ ] "Alguma d�vida sobre as tarefas?"

---

#### DEV 2 - Backend: SLICE 2 (DADGER) - 5 min

**Responsabilidades:**
- [ ] Consulta de Arquivos DADGER
- [ ] 3 entidades relacionadas (ArquivoDadger, ArquivoDadgerValor, SemanaPMO)
- [ ] JOINs complexos
- [ ] Filtros (por per�odo, usina, semana)
- [ ] 5 endpoints REST
- [ ] Testes de integra��o

**Entreg�veis:**
- [ ] 3 entidades no Domain
- [ ] Reposit�rios com JOINs
- [ ] Services com filtros
- [ ] Controller na API
- [ ] Seed data com relacionamentos

**Prazo:** 22/12 (Domingo) - 4 dias (incluindo s�bado/domingo)

**Perguntas ao DEV 2:**
- [ ] "Voc� j� trabalhou com relacionamentos EF Core?"
- [ ] "Tem experi�ncia com queries complexas?"
- [ ] "Alguma d�vida sobre as tarefas?"

---

#### DEV 3 - Frontend: React + TypeScript - 3 min

**Responsabilidades:**
- [ ] 2 telas completas (Usinas + DADGER)
- [ ] Integra��o com API REST
- [ ] Valida��es de formul�rio
- [ ] UI responsiva

**Entreg�veis:**
- [ ] Tela de listagem de Usinas
- [ ] Formul�rio de cadastro/edi��o
- [ ] Tela de consulta DADGER
- [ ] Filtros din�micos
- [ ] Servi�os de integra��o (Axios)

**Prazo:** 21/12 (S�bado) - 3 dias

**Perguntas ao DEV 3:**
- [ ] "Voc� j� trabalhou com React 18?"
- [ ] "Tem experi�ncia com TypeScript?"
- [ ] "Alguma biblioteca UI preferida? (Material-UI, Ant Design, etc.)"

---

#### QA - Quality Assurance - 2 min

**Responsabilidades:**
- [ ] Criar casos de teste
- [ ] Testar endpoints via Postman/Swagger
- [ ] Validar integra��o frontend/backend
- [ ] Documentar bugs
- [ ] Relat�rio de qualidade final

**Entreg�veis:**
- [ ] `TEST_PLAN.md`
- [ ] `TEST_CASES_USINAS.md`
- [ ] `TEST_CASES_DADGER.md`
- [ ] `BUG_REPORT.md` (se houver)
- [ ] `QUALITY_CHECKLIST.md`

**Prazo:** Di�rio (19-24/12)

**Perguntas ao QA:**
- [ ] "Voc� j� trabalhou com testes de API REST?"
- [ ] "Tem experi�ncia com Postman?"
- [ ] "Alguma d�vida sobre os crit�rios de aceite?"

---

**? Crit�rio de sucesso:** Cada pessoa sabe exatamente o que deve fazer e quando

---

### 4. SETUP DO AMBIENTE (10 min) - 15:32-15:42

**Instru��es gerais:**
- [ ] Mostrar guia completo em `docs/SETUP_AMBIENTE_GUIA.md`
- [ ] Explicar que cada dev deve seguir o guia ap�s a reuni�o
- [ ] Tempo estimado de setup: 30-45 minutos

**Para Backend Devs:**
- [ ] Instalar .NET 8 SDK: `winget install Microsoft.DotNet.SDK.8`
- [ ] Instalar Visual Studio 2022: `winget install Microsoft.VisualStudio.2022.Community`
- [ ] Instalar Docker: `winget install Docker.DockerDesktop`
- [ ] Testar: `dotnet --version` (deve ser 8.0.xxx)

**Para Frontend Dev:**
- [ ] Instalar Node.js 20: `winget install OpenJS.NodeJS.LTS`
- [ ] Instalar VS Code: `winget install Microsoft.VisualStudioCode`
- [ ] Instalar extens�es (lista no guia)
- [ ] Testar: `node --version` (deve ser v20.x.x)

**Para QA:**
- [ ] Instalar Postman: `winget install Postman.Postman`
- [ ] Instalar Git: `winget install Git.Git`
- [ ] Acesso ao reposit�rio verificado

**Demonstra��o (se houver tempo):**
- [ ] Clonar reposit�rio
- [ ] Criar branch pessoal
- [ ] Executar backend: `dotnet run`
- [ ] Executar frontend: `npm run dev`
- [ ] Mostrar Swagger: http://localhost:5000/swagger

**? Crit�rio de sucesso:** Todos sabem como fazer o setup e t�m o guia

---

### 5. CRONOGRAMA E COMUNICA��O (5 min) - 15:42-15:47

**Cronograma:**
- [ ] **19/12 (Qui):** Setup + In�cio desenvolvimento
- [ ] **20/12 (Sex):** SLICE 1 completo
- [ ] **21/12 (S�b):** Integra��o SLICE 1 + In�cio SLICE 2
- [ ] **22/12 (Dom):** SLICE 2 completo
- [ ] **23/12 (Seg):** Integra��o SLICE 2 + Ajustes
- [ ] **24/12 (Ter):** Docker + Testes + Docs
- [ ] **25/12 (Qua):** FERIADO ??
- [ ] **26/12 (Qui):** Apresenta��o + Entrega

**Daily Standup:**
- [ ] Hor�rio: 09:00 (todos os dias)
- [ ] Dura��o: 15 minutos
- [ ] Formato: O que fiz? O que vou fazer? Bloqueios?

**Comunica��o:**
- [ ] GitHub Issues para tarefas
- [ ] GitHub Projects para board
- [ ] Teams/Slack para comunica��o ass�ncrona

**Padr�o de commits:**
```bash
[SLICE-1] feat: adiciona entidade Usina
[SLICE-2] fix: corrige filtro de data
[DOCS] docs: atualiza README
[TEST] test: adiciona testes unit�rios
```

**? Crit�rio de sucesso:** Todos entendem o cronograma e como se comunicar

---

### ENCERRAMENTO (3 min) - 15:47-15:50

**Revis�o r�pida:**
- [ ] "Algu�m tem alguma d�vida sobre suas tarefas?"
- [ ] "Todos entenderam o cronograma?"
- [ ] "Todos sabem onde encontrar a documenta��o?"
- [ ] "Algu�m precisa de algum acesso que ainda n�o tem?"

**Pr�ximos passos imediatos:**
- [ ] **Todos:** Fazer setup do ambiente (30-45 min)
- [ ] **Todos:** Criar branch pessoal
- [ ] **DEV 1:** Come�ar a criar entidade Usina
- [ ] **DEV 2:** Analisar c�digo legado DADGER
- [ ] **DEV 3:** Analisar telas legadas ASPX
- [ ] **QA:** Criar estrutura de documenta��o de testes

**Mensagem final:**
> "Pessoal, temos 7 dias intensos pela frente, mas temos tudo que precisamos: an�lise feita, arquitetura definida, escopo claro. Somos um time e vamos entregar essa PoC com qualidade. Contem comigo para qualquer d�vida. Vamos nessa! ??"

**Lembrete:**
- [ ] Daily Standup amanh� (20/12) �s 09:00
- [ ] Documenta��o dispon�vel em `docs/`
- [ ] C�digo legado em `pdpw_act/pdpw/`

**? Crit�rio de sucesso:** Todos est�o motivados e prontos para come�ar

---

## ?? P�S-REUNI�O (Imediatamente ap�s)

### A��es Administrativas
- [ ] Enviar ata da reuni�o (resumo) por email/Teams
- [ ] Compartilhar links da documenta��o
- [ ] Criar Issues no GitHub para cada tarefa
- [ ] Configurar GitHub Projects (board Kanban)
- [ ] Agendar pr�ximo Daily Standup (20/12 09:00)

### Verifica��es T�cnicas
- [ ] Verificar se todos t�m acesso ao reposit�rio
- [ ] Verificar se todos conseguem clonar o reposit�rio
- [ ] Verificar se h� problemas de acesso/permiss�es

### Follow-up Individual (15-30 min ap�s reuni�o)
- [ ] **DEV 1:** Confirmar que come�ou o setup
- [ ] **DEV 2:** Confirmar que come�ou o setup
- [ ] **DEV 3:** Confirmar que come�ou o setup
- [ ] **QA:** Confirmar acesso ao Postman e reposit�rio

---

## ?? TROUBLESHOOTING DURANTE A REUNI�O

### Se algu�m disser "N�o entendi minhas tarefas"
**Resposta:**
1. Abrir `docs/SQUAD_BRIEFING_19DEC.md`
2. Mostrar se��o espec�fica da pessoa
3. Explicar passo a passo os entreg�veis
4. Oferecer follow-up ap�s a reuni�o

### Se algu�m disser "O prazo � muito apertado"
**Resposta:**
1. Reconhecer a preocupa��o
2. Explicar que � uma PoC (n�o precisa ser perfeito)
3. Mostrar que o escopo � reduzido (apenas 2 slices)
4. Refor�ar que temos buffer de 1 dia (24/12)
5. Oferecer suporte constante

### Se algu�m disser "N�o tenho experi�ncia com X"
**Resposta:**
1. Perguntar: "Com o que voc� tem experi�ncia?"
2. Avaliar se d� para trocar tarefas
3. Se n�o der, oferecer:
   - Documenta��o/tutoriais
   - Pair programming
   - Code review mais frequente
   - Suporte direto do Tech Lead

### Se houver conflito de tarefas/branches
**Resposta:**
1. Definir claramente as boundaries de cada slice
2. SLICE 1 e SLICE 2 s�o independentes (n�o h� conflito)
3. Frontend depende do backend estar pronto primeiro
4. QA trabalha em paralelo, testando � medida que fica pronto

### Se algu�m perguntar sobre horas extras/fim de semana
**Resposta:**
1. Ser transparente: "Sim, teremos que trabalhar s�bado e domingo"
2. Explicar: "O prazo � 26/12, s�o apenas 7 dias �teis"
3. Oferecer: "Folga compensat�ria ap�s a entrega"
4. Perguntar: "Algu�m tem impedimento de trabalhar no fim de semana?"

---

## ?? CHECKLIST DE ENTENDIMENTO (Fazer ao final)

**Perguntar individualmente:**

### DEV 1
- [ ] "Voc� sabe qual entidade vai criar primeiro?" (Usina)
- [ ] "Voc� sabe quantos endpoints precisa implementar?" (6)
- [ ] "Voc� sabe o prazo da sua entrega?" (20/12)

### DEV 2
- [ ] "Voc� sabe quantas entidades vai criar?" (3: ArquivoDadger, ArquivoDadgerValor, SemanaPMO)
- [ ] "Voc� sabe quantos endpoints precisa implementar?" (5)
- [ ] "Voc� sabe o prazo da sua entrega?" (22/12)

### DEV 3
- [ ] "Voc� sabe quantas telas vai criar?" (2: Usinas + DADGER)
- [ ] "Voc� sabe quando pode come�ar a integrar?" (Ap�s backend pronto)
- [ ] "Voc� sabe o prazo da sua entrega?" (21/12)

### QA
- [ ] "Voc� sabe quais testes precisa criar?" (API + Integra��o)
- [ ] "Voc� sabe quando vai testar cada slice?" (Conforme ficarem prontos)
- [ ] "Voc� sabe o prazo do relat�rio final?" (24/12)

---

## ?? M�TRICAS DE SUCESSO DA REUNI�O

### Objetivos da Reuni�o
- [ ] ? Todos entendem o objetivo da PoC
- [ ] ? Todos entendem suas responsabilidades
- [ ] ? Todos sabem o que entregar e quando
- [ ] ? Todos sabem como fazer o setup do ambiente
- [ ] ? Todos est�o alinhados com o cronograma

### Clima do Time
- [ ] ?? Time est� motivado
- [ ] ?? Time est� colaborativo
- [ ] ?? Comunica��o est� fluindo
- [ ] ?? Todos t�m clareza das tarefas
- [ ] ?? Reuni�o terminou no hor�rio (45 min)

---

## ?? TEMPLATE DE ATA (Enviar ap�s reuni�o)

```markdown
# ATA DA REUNI�O - KICK-OFF PoC PDPW

**Data:** 19/12/2024 - 15:00h  
**Participantes:** DEV 1, DEV 2, DEV 3, QA, Tech Lead  
**Dura��o:** 45 minutos

## Decis�es Tomadas
1. Escopo: 2 vertical slices (Usinas + DADGER)
2. Stack: .NET 8 + React 18 + Docker + InMemory DB
3. Prazo: Entrega 26/12, Apresenta��o 05/01
4. Daily Standup: 09:00 (todos os dias)

## Divis�o de Tarefas
- **DEV 1:** SLICE 1 - Cadastro de Usinas (Backend)
- **DEV 2:** SLICE 2 - Consulta DADGER (Backend)
- **DEV 3:** Interfaces React para ambos slices (Frontend)
- **QA:** Testes + Documenta��o

## Pr�ximos Passos
1. Todos: Setup do ambiente (hoje)
2. Todos: Criar branch pessoal (hoje)
3. Devs: Iniciar desenvolvimento (hoje/amanh�)
4. Daily Standup: 20/12 �s 09:00

## Documenta��o
- Briefing: `docs/SQUAD_BRIEFING_19DEC.md`
- An�lise T�cnica: `docs/ANALISE_TECNICA_CODIGO_LEGADO.md`
- Setup: `docs/SETUP_AMBIENTE_GUIA.md`
- Apresenta��o: `docs/APRESENTACAO_REUNIAO_SQUAD.md`

## D�vidas ou Problemas
Contatar Tech Lead via Teams/Slack
```

---

## ?? CRIT�RIOS DE SUCESSO FINAL

Esta reuni�o ser� um sucesso se:

- [ ] ? Todos os 4 membros do squad est�o presentes
- [ ] ? Todos entenderam o objetivo da PoC
- [ ] ? Cada pessoa sabe exatamente o que deve fazer
- [ ] ? Todos sabem o prazo de suas entregas
- [ ] ? Todos sabem como fazer o setup do ambiente
- [ ] ? Nenhuma d�vida cr�tica ficou sem resposta
- [ ] ? Time est� motivado e confiante
- [ ] ? Reuni�o terminou em 45 minutos (m�ximo 60 min)

---

**Preparado por:** Tech Lead  
**Data:** 19/12/2024  
**Vers�o:** 1.0  
**Status:** ? Pronto para reuni�o

**BOA SORTE NA REUNI�O! ??**
