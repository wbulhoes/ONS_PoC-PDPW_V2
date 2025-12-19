# ? CHECKLIST EXECUTIVO - REUNIÃO DO SQUAD

**Data:** 19/12/2024 - 15:00h  
**Objetivo:** Kick-off do desenvolvimento da PoC PDPW  
**Duração:** 45 minutos

---

## ?? PRÉ-REUNIÃO (Preparação)

### Documentos a Ter Abertos
- [ ] `docs/APRESENTACAO_REUNIAO_SQUAD.md` (este documento)
- [ ] `docs/SQUAD_BRIEFING_19DEC.md` (briefing detalhado)
- [ ] `docs/ANALISE_TECNICA_CODIGO_LEGADO.md` (análise técnica)
- [ ] `docs/SETUP_AMBIENTE_GUIA.md` (guia de instalação)

### Verificações Técnicas
- [ ] Repositório está no GitHub e acessível a todos
- [ ] Branch `develop` existe e está atualizada
- [ ] Código legado está em `pdpw_act/pdpw/`
- [ ] Documentação existente revisada

### Materiais de Apoio
- [ ] Projetor/tela compartilhamento conectado
- [ ] Acesso ao repositório GitHub
- [ ] Acesso ao código legado
- [ ] Swagger da API atual (se houver)

---

## ?? DURANTE A REUNIÃO

### ABERTURA (2 min) - 15:00-15:02

**Script:**
> "Boa tarde, pessoal! Obrigado por estarem aqui. Esta é nossa reunião de kick-off da PoC PDPW. Temos 7 dias úteis para entregar 2 fluxos completos do sistema. Vamos começar?"

**Checklist:**
- [ ] Todos os participantes presentes (3 devs + 1 QA)
- [ ] Reunião gravada (se aplicável)
- [ ] Timer iniciado

---

### 1. CONTEXTO DO PROJETO (5 min) - 15:02-15:07

**Pontos a cobrir:**
- [ ] **Objetivo:** Modernizar PDPW de VB.NET/WebForms para .NET 8/React
- [ ] **Cliente:** ONS (Operador Nacional do Sistema Elétrico)
- [ ] **Prazo:** Entrega 26/12, Apresentação 05/01, Estimativa 12/01
- [ ] **Escopo:** 2 vertical slices (Usinas + DADGER)
- [ ] **Stack:** .NET 8 + React 18 + Docker + SQL Server InMemory

**Perguntas a fazer:**
- [ ] "Todos entenderam o que é uma PoC?"
- [ ] "Alguém tem dúvidas sobre o prazo?"
- [ ] "Todos sabem o que é um vertical slice?"

**? Critério de sucesso:** Todos entendem o objetivo e o prazo

---

### 2. ANÁLISE DO CÓDIGO LEGADO (10 min) - 15:07-15:17

**Estatísticas a apresentar:**
- [ ] 473 arquivos VB.NET
- [ ] 168 páginas ASPX (WebForms)
- [ ] .NET Framework 4.8 + SQL Server
- [ ] Arquitetura em 3 camadas (DAO/Business/DTO)

**Demonstração prática:**
- [ ] Abrir `pdpw_act/pdpw/Dao/UsinaDAO.vb`
- [ ] Mostrar query SQL inline
- [ ] Mostrar mapeamento manual com SqlDataReader
- [ ] Abrir `pdpw_act/pdpw/DTOs/UsinaDTO.vb`
- [ ] Mostrar propriedades da entidade

**Pontos a destacar:**
- [ ] ? Código bem estruturado
- [ ] ? Separação de responsabilidades clara
- [ ] ?? SQL inline (vulnerável a injection)
- [ ] ?? Sem ORM moderno
- [ ] ?? VB.NET precisa ser convertido para C#

**Perguntas a fazer:**
- [ ] "Alguém já trabalhou com VB.NET?"
- [ ] "Alguém tem dúvidas sobre a arquitetura legada?"
- [ ] "Ficou claro o que precisamos migrar?"

**? Critério de sucesso:** Todos entendem o código legado e os desafios

---

### 3. DIVISÃO DE TAREFAS (15 min) - 15:17-15:32

#### DEV 1 - Backend: SLICE 1 (Usinas) - 5 min

**Responsabilidades:**
- [ ] CRUD completo de Usinas
- [ ] 6 endpoints REST (GET/POST/PUT/DELETE)
- [ ] Validações de negócio
- [ ] Testes unitários

**Entregáveis:**
- [ ] Entidade `Usina.cs` no Domain
- [ ] Repositório no Infrastructure
- [ ] Service na Application
- [ ] Controller na API
- [ ] Swagger documentado

**Prazo:** 20/12 (Sexta) - 2 dias

**Perguntas ao DEV 1:**
- [ ] "Você já trabalhou com Clean Architecture?"
- [ ] "Tem experiência com EF Core?"
- [ ] "Alguma dúvida sobre as tarefas?"

---

#### DEV 2 - Backend: SLICE 2 (DADGER) - 5 min

**Responsabilidades:**
- [ ] Consulta de Arquivos DADGER
- [ ] 3 entidades relacionadas (ArquivoDadger, ArquivoDadgerValor, SemanaPMO)
- [ ] JOINs complexos
- [ ] Filtros (por período, usina, semana)
- [ ] 5 endpoints REST
- [ ] Testes de integração

**Entregáveis:**
- [ ] 3 entidades no Domain
- [ ] Repositórios com JOINs
- [ ] Services com filtros
- [ ] Controller na API
- [ ] Seed data com relacionamentos

**Prazo:** 22/12 (Domingo) - 4 dias (incluindo sábado/domingo)

**Perguntas ao DEV 2:**
- [ ] "Você já trabalhou com relacionamentos EF Core?"
- [ ] "Tem experiência com queries complexas?"
- [ ] "Alguma dúvida sobre as tarefas?"

---

#### DEV 3 - Frontend: React + TypeScript - 3 min

**Responsabilidades:**
- [ ] 2 telas completas (Usinas + DADGER)
- [ ] Integração com API REST
- [ ] Validações de formulário
- [ ] UI responsiva

**Entregáveis:**
- [ ] Tela de listagem de Usinas
- [ ] Formulário de cadastro/edição
- [ ] Tela de consulta DADGER
- [ ] Filtros dinâmicos
- [ ] Serviços de integração (Axios)

**Prazo:** 21/12 (Sábado) - 3 dias

**Perguntas ao DEV 3:**
- [ ] "Você já trabalhou com React 18?"
- [ ] "Tem experiência com TypeScript?"
- [ ] "Alguma biblioteca UI preferida? (Material-UI, Ant Design, etc.)"

---

#### QA - Quality Assurance - 2 min

**Responsabilidades:**
- [ ] Criar casos de teste
- [ ] Testar endpoints via Postman/Swagger
- [ ] Validar integração frontend/backend
- [ ] Documentar bugs
- [ ] Relatório de qualidade final

**Entregáveis:**
- [ ] `TEST_PLAN.md`
- [ ] `TEST_CASES_USINAS.md`
- [ ] `TEST_CASES_DADGER.md`
- [ ] `BUG_REPORT.md` (se houver)
- [ ] `QUALITY_CHECKLIST.md`

**Prazo:** Diário (19-24/12)

**Perguntas ao QA:**
- [ ] "Você já trabalhou com testes de API REST?"
- [ ] "Tem experiência com Postman?"
- [ ] "Alguma dúvida sobre os critérios de aceite?"

---

**? Critério de sucesso:** Cada pessoa sabe exatamente o que deve fazer e quando

---

### 4. SETUP DO AMBIENTE (10 min) - 15:32-15:42

**Instruções gerais:**
- [ ] Mostrar guia completo em `docs/SETUP_AMBIENTE_GUIA.md`
- [ ] Explicar que cada dev deve seguir o guia após a reunião
- [ ] Tempo estimado de setup: 30-45 minutos

**Para Backend Devs:**
- [ ] Instalar .NET 8 SDK: `winget install Microsoft.DotNet.SDK.8`
- [ ] Instalar Visual Studio 2022: `winget install Microsoft.VisualStudio.2022.Community`
- [ ] Instalar Docker: `winget install Docker.DockerDesktop`
- [ ] Testar: `dotnet --version` (deve ser 8.0.xxx)

**Para Frontend Dev:**
- [ ] Instalar Node.js 20: `winget install OpenJS.NodeJS.LTS`
- [ ] Instalar VS Code: `winget install Microsoft.VisualStudioCode`
- [ ] Instalar extensões (lista no guia)
- [ ] Testar: `node --version` (deve ser v20.x.x)

**Para QA:**
- [ ] Instalar Postman: `winget install Postman.Postman`
- [ ] Instalar Git: `winget install Git.Git`
- [ ] Acesso ao repositório verificado

**Demonstração (se houver tempo):**
- [ ] Clonar repositório
- [ ] Criar branch pessoal
- [ ] Executar backend: `dotnet run`
- [ ] Executar frontend: `npm run dev`
- [ ] Mostrar Swagger: http://localhost:5000/swagger

**? Critério de sucesso:** Todos sabem como fazer o setup e têm o guia

---

### 5. CRONOGRAMA E COMUNICAÇÃO (5 min) - 15:42-15:47

**Cronograma:**
- [ ] **19/12 (Qui):** Setup + Início desenvolvimento
- [ ] **20/12 (Sex):** SLICE 1 completo
- [ ] **21/12 (Sáb):** Integração SLICE 1 + Início SLICE 2
- [ ] **22/12 (Dom):** SLICE 2 completo
- [ ] **23/12 (Seg):** Integração SLICE 2 + Ajustes
- [ ] **24/12 (Ter):** Docker + Testes + Docs
- [ ] **25/12 (Qua):** FERIADO ??
- [ ] **26/12 (Qui):** Apresentação + Entrega

**Daily Standup:**
- [ ] Horário: 09:00 (todos os dias)
- [ ] Duração: 15 minutos
- [ ] Formato: O que fiz? O que vou fazer? Bloqueios?

**Comunicação:**
- [ ] GitHub Issues para tarefas
- [ ] GitHub Projects para board
- [ ] Teams/Slack para comunicação assíncrona

**Padrão de commits:**
```bash
[SLICE-1] feat: adiciona entidade Usina
[SLICE-2] fix: corrige filtro de data
[DOCS] docs: atualiza README
[TEST] test: adiciona testes unitários
```

**? Critério de sucesso:** Todos entendem o cronograma e como se comunicar

---

### ENCERRAMENTO (3 min) - 15:47-15:50

**Revisão rápida:**
- [ ] "Alguém tem alguma dúvida sobre suas tarefas?"
- [ ] "Todos entenderam o cronograma?"
- [ ] "Todos sabem onde encontrar a documentação?"
- [ ] "Alguém precisa de algum acesso que ainda não tem?"

**Próximos passos imediatos:**
- [ ] **Todos:** Fazer setup do ambiente (30-45 min)
- [ ] **Todos:** Criar branch pessoal
- [ ] **DEV 1:** Começar a criar entidade Usina
- [ ] **DEV 2:** Analisar código legado DADGER
- [ ] **DEV 3:** Analisar telas legadas ASPX
- [ ] **QA:** Criar estrutura de documentação de testes

**Mensagem final:**
> "Pessoal, temos 7 dias intensos pela frente, mas temos tudo que precisamos: análise feita, arquitetura definida, escopo claro. Somos um time e vamos entregar essa PoC com qualidade. Contem comigo para qualquer dúvida. Vamos nessa! ??"

**Lembrete:**
- [ ] Daily Standup amanhã (20/12) às 09:00
- [ ] Documentação disponível em `docs/`
- [ ] Código legado em `pdpw_act/pdpw/`

**? Critério de sucesso:** Todos estão motivados e prontos para começar

---

## ?? PÓS-REUNIÃO (Imediatamente após)

### Ações Administrativas
- [ ] Enviar ata da reunião (resumo) por email/Teams
- [ ] Compartilhar links da documentação
- [ ] Criar Issues no GitHub para cada tarefa
- [ ] Configurar GitHub Projects (board Kanban)
- [ ] Agendar próximo Daily Standup (20/12 09:00)

### Verificações Técnicas
- [ ] Verificar se todos têm acesso ao repositório
- [ ] Verificar se todos conseguem clonar o repositório
- [ ] Verificar se há problemas de acesso/permissões

### Follow-up Individual (15-30 min após reunião)
- [ ] **DEV 1:** Confirmar que começou o setup
- [ ] **DEV 2:** Confirmar que começou o setup
- [ ] **DEV 3:** Confirmar que começou o setup
- [ ] **QA:** Confirmar acesso ao Postman e repositório

---

## ?? TROUBLESHOOTING DURANTE A REUNIÃO

### Se alguém disser "Não entendi minhas tarefas"
**Resposta:**
1. Abrir `docs/SQUAD_BRIEFING_19DEC.md`
2. Mostrar seção específica da pessoa
3. Explicar passo a passo os entregáveis
4. Oferecer follow-up após a reunião

### Se alguém disser "O prazo é muito apertado"
**Resposta:**
1. Reconhecer a preocupação
2. Explicar que é uma PoC (não precisa ser perfeito)
3. Mostrar que o escopo é reduzido (apenas 2 slices)
4. Reforçar que temos buffer de 1 dia (24/12)
5. Oferecer suporte constante

### Se alguém disser "Não tenho experiência com X"
**Resposta:**
1. Perguntar: "Com o que você tem experiência?"
2. Avaliar se dá para trocar tarefas
3. Se não der, oferecer:
   - Documentação/tutoriais
   - Pair programming
   - Code review mais frequente
   - Suporte direto do Tech Lead

### Se houver conflito de tarefas/branches
**Resposta:**
1. Definir claramente as boundaries de cada slice
2. SLICE 1 e SLICE 2 são independentes (não há conflito)
3. Frontend depende do backend estar pronto primeiro
4. QA trabalha em paralelo, testando à medida que fica pronto

### Se alguém perguntar sobre horas extras/fim de semana
**Resposta:**
1. Ser transparente: "Sim, teremos que trabalhar sábado e domingo"
2. Explicar: "O prazo é 26/12, são apenas 7 dias úteis"
3. Oferecer: "Folga compensatória após a entrega"
4. Perguntar: "Alguém tem impedimento de trabalhar no fim de semana?"

---

## ?? CHECKLIST DE ENTENDIMENTO (Fazer ao final)

**Perguntar individualmente:**

### DEV 1
- [ ] "Você sabe qual entidade vai criar primeiro?" (Usina)
- [ ] "Você sabe quantos endpoints precisa implementar?" (6)
- [ ] "Você sabe o prazo da sua entrega?" (20/12)

### DEV 2
- [ ] "Você sabe quantas entidades vai criar?" (3: ArquivoDadger, ArquivoDadgerValor, SemanaPMO)
- [ ] "Você sabe quantos endpoints precisa implementar?" (5)
- [ ] "Você sabe o prazo da sua entrega?" (22/12)

### DEV 3
- [ ] "Você sabe quantas telas vai criar?" (2: Usinas + DADGER)
- [ ] "Você sabe quando pode começar a integrar?" (Após backend pronto)
- [ ] "Você sabe o prazo da sua entrega?" (21/12)

### QA
- [ ] "Você sabe quais testes precisa criar?" (API + Integração)
- [ ] "Você sabe quando vai testar cada slice?" (Conforme ficarem prontos)
- [ ] "Você sabe o prazo do relatório final?" (24/12)

---

## ?? MÉTRICAS DE SUCESSO DA REUNIÃO

### Objetivos da Reunião
- [ ] ? Todos entendem o objetivo da PoC
- [ ] ? Todos entendem suas responsabilidades
- [ ] ? Todos sabem o que entregar e quando
- [ ] ? Todos sabem como fazer o setup do ambiente
- [ ] ? Todos estão alinhados com o cronograma

### Clima do Time
- [ ] ?? Time está motivado
- [ ] ?? Time está colaborativo
- [ ] ?? Comunicação está fluindo
- [ ] ?? Todos têm clareza das tarefas
- [ ] ?? Reunião terminou no horário (45 min)

---

## ?? TEMPLATE DE ATA (Enviar após reunião)

```markdown
# ATA DA REUNIÃO - KICK-OFF PoC PDPW

**Data:** 19/12/2024 - 15:00h  
**Participantes:** DEV 1, DEV 2, DEV 3, QA, Tech Lead  
**Duração:** 45 minutos

## Decisões Tomadas
1. Escopo: 2 vertical slices (Usinas + DADGER)
2. Stack: .NET 8 + React 18 + Docker + InMemory DB
3. Prazo: Entrega 26/12, Apresentação 05/01
4. Daily Standup: 09:00 (todos os dias)

## Divisão de Tarefas
- **DEV 1:** SLICE 1 - Cadastro de Usinas (Backend)
- **DEV 2:** SLICE 2 - Consulta DADGER (Backend)
- **DEV 3:** Interfaces React para ambos slices (Frontend)
- **QA:** Testes + Documentação

## Próximos Passos
1. Todos: Setup do ambiente (hoje)
2. Todos: Criar branch pessoal (hoje)
3. Devs: Iniciar desenvolvimento (hoje/amanhã)
4. Daily Standup: 20/12 às 09:00

## Documentação
- Briefing: `docs/SQUAD_BRIEFING_19DEC.md`
- Análise Técnica: `docs/ANALISE_TECNICA_CODIGO_LEGADO.md`
- Setup: `docs/SETUP_AMBIENTE_GUIA.md`
- Apresentação: `docs/APRESENTACAO_REUNIAO_SQUAD.md`

## Dúvidas ou Problemas
Contatar Tech Lead via Teams/Slack
```

---

## ?? CRITÉRIOS DE SUCESSO FINAL

Esta reunião será um sucesso se:

- [ ] ? Todos os 4 membros do squad estão presentes
- [ ] ? Todos entenderam o objetivo da PoC
- [ ] ? Cada pessoa sabe exatamente o que deve fazer
- [ ] ? Todos sabem o prazo de suas entregas
- [ ] ? Todos sabem como fazer o setup do ambiente
- [ ] ? Nenhuma dúvida crítica ficou sem resposta
- [ ] ? Time está motivado e confiante
- [ ] ? Reunião terminou em 45 minutos (máximo 60 min)

---

**Preparado por:** Tech Lead  
**Data:** 19/12/2024  
**Versão:** 1.0  
**Status:** ? Pronto para reunião

**BOA SORTE NA REUNIÃO! ??**
