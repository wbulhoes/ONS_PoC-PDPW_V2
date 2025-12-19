# ?? APRESENTAÇÃO PARA REUNIÃO DO SQUAD
## PoC PDPW - Modernização ONS

**Data:** 19/12/2024 - 15:00h  
**Duração:** 45 minutos  
**Participantes:** 3 Devs + 1 QA + Tech Lead

---

## ?? AGENDA DA REUNIÃO

1. **Contexto do Projeto** (5 min)
2. **Análise do Código Legado** (10 min)
3. **Divisão de Tarefas** (15 min)
4. **Setup do Ambiente** (10 min)
5. **Cronograma e Entregas** (5 min)

---

## 1?? CONTEXTO DO PROJETO (5 min)

### ?? Objetivo da PoC
> Modernizar o sistema legado PDPW do ONS, migrando de .NET Framework/VB.NET/WebForms para .NET 8 + React + Docker

### ?? Prazos Críticos
- **Entrega do código:** 26/12/2024 (7 dias úteis)
- **Apresentação:** 05/01/2025
- **Estimativa completa:** 12/01/2025

### ?? Escopo da PoC
? **2 Vertical Slices:**
1. Cadastro de Usinas (CRUD completo)
2. Consulta de Arquivos DADGER (Leitura + Filtros)

? **Stack Tecnológico:**
- Backend: .NET 8 (Clean Architecture)
- Frontend: React 18 + TypeScript
- Banco: SQL Server (InMemory para PoC)
- DevOps: Docker + Docker Compose

---

## 2?? ANÁLISE DO CÓDIGO LEGADO (10 min)

### ?? Estatísticas
```
?? Estrutura do Repositório Legado
??? 473 arquivos VB.NET
??? 168 páginas ASPX (WebForms)
??? .NET Framework 4.8
??? SQL Server (migrado de Informix)
```

### ??? Arquitetura Atual
```
???????????????????????????????????????????
?    Apresentação (WebForms + ASPX)       ? ? 168 páginas
???????????????????????????????????????????
                 ?
???????????????????????????????????????????
?         Business Layer (VB.NET)         ? ? Lógica de negócio
???????????????????????????????????????????
                 ?
???????????????????????????????????????????
?      Data Access Layer (DAO + DTO)      ? ? SQL inline
???????????????????????????????????????????
                 ?
???????????????????????????????????????????
?         SQL Server Database             ?
???????????????????????????????????????????
```

### ? Pontos Positivos do Legado
1. ? **Separação de camadas** bem definida (DAO/Business/DTO)
2. ? **Padrões de projeto** implementados (Repository, DTO)
3. ? **Testes unitários** existentes
4. ? **Logging estruturado** (Log4Net + ElasticSearch)
5. ? **Sistema de cache** implementado

### ?? Desafios Identificados
1. ?? **WebForms legado** (dificulta migração de UI)
2. ?? **VB.NET** em vez de C# (requer conversão manual)
3. ?? **SQL inline** (sem ORM moderno)
4. ?? **Autenticação complexa** (POP - fora do escopo da PoC)
5. ?? **Banco de 350GB** (impossível restaurar - usaremos InMemory)

### ?? Código Analisado

#### SLICE 1: UsinaDAO.vb
```vb
Public Class UsinaDAO
    Inherits BaseDAO(Of UsinaDTO)
    
    Public Function ListarUsina(codUsina As String) As List(Of UsinaDTO)
        ' Query SQL inline
        Dim sql = "SELECT codusina, nomusina, potinstalada..."
        ' Mapeamento manual com SqlDataReader
    End Function
End Class
```

**Campos da entidade Usina:**
- CodUsina (PK)
- NomeUsina
- CodEmpre (FK)
- PotInstalada (MW)
- TpusinaId (UTE, UHE, EOL)
- DppId, UsiBdtId, Sigsme

#### SLICE 2: ArquivoDadgerValorDAO.vb
```vb
Public Class ArquivoDadgerValorDAO
    Inherits BaseDAO(Of ArquivoDadgerValorDTO)
    
    Public Function ListarPor_DataPDP_Usina(...) As List(...)
        ' Query complexa com JOINs
        Dim sql = "SELECT ... FROM tb_arquivodadgervalor v
                   JOIN tb_arquivodadger a ON ...
                   LEFT JOIN Usina u ON u.Dpp_Id = v.Dpp_Id"
    End Function
End Class
```

**Entidades relacionadas:**
- ArquivoDadger (arquivo importado)
- ArquivoDadgerValor (valores de inflexibilidade/CVU)
- SemanaPMO (período do PMO)
- Usina (relação com usinas térmicas)

---

## 3?? DIVISÃO DE TAREFAS (15 min)

### ?? Equipe

| Membro | Papel | Foco |
|--------|-------|------|
| **DEV 1** | Backend Lead | SLICE 1: Cadastro de Usinas |
| **DEV 2** | Backend | SLICE 2: Consulta DADGER |
| **DEV 3** | Frontend Lead | UI React para ambos slices |
| **QA** | Quality Assurance | Testes + Documentação |

---

### ?? DEV 1 - Backend: SLICE 1 (Usinas)

**Objetivo:** Implementar CRUD completo de Usinas

#### Dia 1: 19/12 (Quinta) - 8h
- ? Setup: .NET 8, Docker, Git
- ?? Criar entidade `Usina.cs` no Domain
- ?? Criar interface `IUsinaRepository.cs`
- ?? Implementar `UsinaRepository.cs` no Infrastructure
- ?? Configurar `DbContext` com InMemory
- ?? Criar seed data (5-10 usinas)

#### Dia 2: 20/12 (Sexta) - 8h
- ?? Criar DTOs (UsinaRequestDTO, UsinaResponseDTO)
- ?? Implementar `UsinaService.cs` com validações
- ?? Criar `UsinasController.cs` com 6 endpoints:
  - `GET /api/usinas` (Listar todas)
  - `GET /api/usinas/{id}` (Buscar por ID)
  - `GET /api/usinas/codigo/{codigo}` (Buscar por código)
  - `POST /api/usinas` (Criar)
  - `PUT /api/usinas/{id}` (Atualizar)
  - `DELETE /api/usinas/{id}` (Soft delete)
- ?? Testes unitários básicos

**Entregáveis:**
- ? API REST funcionando
- ? Swagger documentado
- ? Testes passando

---

### ?? DEV 2 - Backend: SLICE 2 (DADGER)

**Objetivo:** Implementar consulta de Arquivos DADGER

#### Dia 1: 19/12 (Quinta) - 8h
- ? Setup: .NET 8, Docker, Git
- ?? Analisar código legado de ArquivoDadgerValorDAO
- ?? Criar entidades:
  - `ArquivoDadger.cs`
  - `ArquivoDadgerValor.cs`
  - `SemanaPMO.cs`
- ?? Criar interfaces de repositórios
- ?? Configurar relacionamentos EF Core

#### Dias 2-3: 21-22/12 (Sábado/Domingo) - 16h
- ?? Implementar repositórios com JOINs complexos
- ?? Criar services com filtros (por período, usina, semana)
- ?? Criar DTOs
- ?? Criar `ArquivosDadgerController.cs` com 5 endpoints:
  - `GET /api/arquivosdadger` (Listar todos)
  - `GET /api/arquivosdadger/{id}` (Buscar por ID)
  - `GET /api/arquivosdadger/semana/{idSemana}` (Por semana PMO)
  - `GET /api/arquivosdadger/usina/{codUsina}` (Por usina)
  - `GET /api/arquivosdadger/{id}/valores` (Valores do arquivo)
- ?? Testes de integração

**Entregáveis:**
- ? API REST funcionando
- ? Relacionamentos funcionando
- ? Filtros complexos implementados

---

### ?? DEV 3 - Frontend: React + TypeScript

**Objetivo:** Implementar interfaces para ambos slices

#### Dia 1: 19/12 (Quinta) - 8h
- ? Setup: Node.js 20, VS Code, extensões
- ?? Analisar telas legadas ASPX
- ?? Criar estrutura de componentes React
- ?? Configurar Axios para API
- ?? Configurar React Router

#### Dia 2: 20/12 (Sexta) - 8h
- ?? **SLICE 1: Usinas**
  - Componente `UsinasListPage.tsx` (listagem com filtros)
  - Componente `UsinaFormPage.tsx` (formulário)
  - Serviço `usinaService.ts` (integração API)
  - Validações de formulário

#### Dia 3: 21/12 (Sábado) - 8h
- ?? **SLICE 2: DADGER**
  - Componente `DadgerConsultaPage.tsx`
  - Filtros dinâmicos (período, usina, semana)
  - Grid de resultados
  - Serviço `dadgerService.ts`

**Entregáveis:**
- ? 2 telas funcionais
- ? Integração completa com backend
- ? UI responsiva e moderna

---

### ?? QA - Quality Assurance

**Objetivo:** Garantir qualidade e criar documentação de testes

#### Diário (19-23/12) - 4h/dia
- ?? **Dia 1 (19/12)**
  - Setup: Postman, Git, VS Code
  - Criar casos de teste para SLICE 1
  - Documentar: `TEST_PLAN.md`

- ?? **Dia 2 (20/12)**
  - Testar endpoints de Usinas via Swagger/Postman
  - Validar CRUD completo
  - Documentar bugs (se houver)

- ?? **Dia 3 (21/12)**
  - Criar casos de teste para SLICE 2
  - Testar integração frontend/backend SLICE 1
  - Validar responsividade

- ?? **Dia 4 (22/12)**
  - Testar endpoints de DADGER
  - Testar integração frontend/backend SLICE 2
  - Validar filtros complexos

- ?? **Dia 5 (23/12)**
  - Testes de regressão completos
  - Validar Docker Compose
  - Criar checklist de validação

- ?? **Dia 6 (24/12)**
  - Testes finais
  - Sign-off da qualidade
  - Relatório final

**Entregáveis:**
- ? Plano de testes
- ? Casos de teste executados
- ? Relatório de qualidade
- ? Checklist de validação

---

## 4?? SETUP DO AMBIENTE (10 min)

### Pré-requisitos (Todos)
```powershell
# 1. Instalar ferramentas
winget install Git.Git
winget install Microsoft.VisualStudioCode

# 2. Clonar repositório
cd C:\temp
git clone https://github.com/wbulhoes/ONS_PoC-PDPW.git
cd ONS_PoC-PDPW

# 3. Criar branch pessoal
git checkout develop
git checkout -b feature/seu-nome-slice-x
```

### Backend Devs
```powershell
# Instalar .NET 8
winget install Microsoft.DotNet.SDK.8

# Instalar Visual Studio 2022
winget install Microsoft.VisualStudio.2022.Community

# Instalar Docker
winget install Docker.DockerDesktop

# Testar
cd src\PDPW.API
dotnet restore
dotnet build
dotnet run
# Abrir: http://localhost:5000/swagger
```

### Frontend Dev
```powershell
# Instalar Node.js 20
winget install OpenJS.NodeJS.LTS

# Instalar extensões VS Code
code --install-extension dsznajder.es7-react-js-snippets
code --install-extension dbaeumer.vscode-eslint
code --install-extension esbenp.prettier-vscode

# Testar
cd frontend
npm install
npm run dev
# Abrir: http://localhost:3000
```

### QA
```powershell
# Instalar Postman
winget install Postman.Postman

# Testar acesso ao repositório
git status
```

**?? Guia completo:** `docs/SETUP_AMBIENTE_GUIA.md`

---

## 5?? CRONOGRAMA E ENTREGAS (5 min)

### ?? Timeline

```
19/12 (Qui) ????????????????? Kick-off + Setup + Início Dev
20/12 (Sex) ????????????????? Desenvolver SLICE 1
21/12 (Sáb) ????????????????? Integração SLICE 1 + Iniciar SLICE 2
22/12 (Dom) ????????????????? Desenvolver SLICE 2
23/12 (Seg) ????????????????? Integração SLICE 2 + Ajustes
24/12 (Ter) ????????????????? Docker + Testes + Docs
25/12 (Qua) ????????????????? FERIADO ??
26/12 (Qui) ????????????????? Apresentação + Entrega
```

### ?? Entregas por Slice

#### SLICE 1: Cadastro de Usinas (Prazo: 20/12)
- [ ] Backend: API REST com 6 endpoints
- [ ] Frontend: Listagem + Formulário
- [ ] Testes: Casos de teste executados
- [ ] Docs: Swagger atualizado

#### SLICE 2: Consulta DADGER (Prazo: 22/12)
- [ ] Backend: API REST com 5 endpoints + JOINs
- [ ] Frontend: Consulta + Filtros dinâmicos
- [ ] Testes: Testes de integração
- [ ] Docs: Documentação de filtros

#### Infraestrutura (Prazo: 24/12)
- [ ] Docker Compose funcionando
- [ ] Backend containerizado
- [ ] Frontend containerizado
- [ ] README atualizado

#### Apresentação (Prazo: 26/12)
- [ ] Slides preparados
- [ ] Demo funcional
- [ ] Código no GitHub
- [ ] Documentação completa

---

## ?? RISCOS E MITIGAÇÕES

| Risco | Prob. | Impacto | Mitigação |
|-------|-------|---------|-----------|
| Prazo apertado | ?? ALTA | ?? ALTO | Escopo reduzido (2 slices) + trabalho aos fins de semana |
| Complexidade do legado | ?? MÉDIA | ?? MÉDIO | ? Análise prévia concluída |
| Falta de banco legado | ?? ALTA | ?? MÉDIO | ? RESOLVIDO: InMemory Database |
| Problemas de integração | ?? MÉDIA | ?? ALTO | Testes contínuos + QA dedicado |
| Bugs de última hora | ?? MÉDIA | ?? MÉDIO | Buffer de 1 dia (24/12) |

---

## ?? COMUNICAÇÃO DO SQUAD

### Daily Standup
- **Horário:** 09:00 (todos os dias)
- **Duração:** 15 minutos
- **Formato:** O que fiz? O que vou fazer? Tenho bloqueios?

### Canais
- **GitHub Issues:** Rastreamento de tarefas
- **GitHub Projects:** Board Kanban
- **Teams/Slack:** Comunicação assíncrona

### Padrão de Commits
```bash
[SLICE-1] feat: adiciona entidade Usina
[SLICE-2] fix: corrige filtro de data DADGER
[DOCS] docs: atualiza README
[TEST] test: adiciona testes de integração
```

---

## ?? AÇÕES IMEDIATAS (APÓS REUNIÃO)

### Todos
1. ? Confirmar entendimento das tarefas
2. ? Tirar dúvidas (agora!)
3. ?? Iniciar setup do ambiente
4. ?? Criar branch pessoal

### DEV 1 (Backend: Usinas)
1. ?? Verificar setup .NET 8
2. ?? Criar branch `feature/slice-1-usinas`
3. ?? Criar entidade `Usina.cs`
4. ?? Configurar DbContext

### DEV 2 (Backend: DADGER)
1. ?? Verificar setup .NET 8
2. ?? Criar branch `feature/slice-2-dadger`
3. ?? Analisar `ArquivoDadgerValorDAO.vb`
4. ?? Criar entidades no Domain

### DEV 3 (Frontend)
1. ?? Verificar setup Node.js 20
2. ?? Criar branch `feature/frontend-slices`
3. ?? Analisar telas legadas `.aspx`
4. ?? Criar estrutura de componentes

### QA
1. ?? Instalar Postman
2. ?? Criar branch `docs/test-documentation`
3. ?? Criar `TEST_PLAN.md`
4. ?? Preparar casos de teste SLICE 1

---

## ?? DOCUMENTAÇÃO DISPONÍVEL

### Documentos Criados Hoje
1. ? `docs/SQUAD_BRIEFING_19DEC.md` - Este documento
2. ? `docs/ANALISE_TECNICA_CODIGO_LEGADO.md` - Análise detalhada
3. ? `docs/SETUP_AMBIENTE_GUIA.md` - Guia de instalação

### Documentos Existentes
4. ? `README.md` - Visão geral do projeto
5. ? `VERTICAL_SLICES_DECISION.md` - Decisões dos slices
6. ? `RESUMO_EXECUTIVO.md` - Resumo executivo
7. ? `GLOSSARIO.md` - Termos técnicos

### Código Legado de Referência
8. `pdpw_act/pdpw/Dao/UsinaDAO.vb`
9. `pdpw_act/pdpw/DTOs/UsinaDTO.vb`
10. `pdpw_act/pdpw/Dao/ArquivoDadgerValorDAO.vb`
11. `pdpw_act/pdpw/frmCnsArquivo.aspx`

---

## ? PERGUNTAS E RESPOSTAS

### Q1: Vamos usar SQL Server ou InMemory?
**R:** InMemory Database para a PoC. Facilita setup e deploy. Para o projeto real, SQL Server.

### Q2: Precisamos implementar autenticação?
**R:** Não. Autenticação (POP) está fora do escopo da PoC.

### Q3: Como será o deploy?
**R:** Docker Compose com 3 containers: Backend, Frontend e (futuramente) SQL Server.

### Q4: Quantas telas precisamos migrar?
**R:** 2 fluxos completos: Cadastro de Usinas + Consulta DADGER.

### Q5: E se não der tempo de fazer tudo?
**R:** Prioridade: SLICE 1 completo. SLICE 2 pode ser simplificado se necessário.

### Q6: Vamos trabalhar no fim de semana?
**R:** Sim. O prazo é apertado e precisamos das 40-48 horas úteis para concluir.

---

## ? CHECKLIST DE ENTENDIMENTO

Antes de sair da reunião, cada pessoa deve confirmar:

### Todos
- [ ] Entendi o objetivo da PoC
- [ ] Entendi minhas responsabilidades
- [ ] Sei qual é minha entrega
- [ ] Sei quando é o prazo
- [ ] Tirei todas as minhas dúvidas
- [ ] Sei onde encontrar a documentação

### Backend Devs
- [ ] Entendi qual slice vou desenvolver
- [ ] Analisei o código legado correspondente
- [ ] Sei quais entidades devo criar
- [ ] Sei quais endpoints devo implementar

### Frontend Dev
- [ ] Entendi quais telas devo criar
- [ ] Analisei as telas legadas
- [ ] Sei como integrar com a API
- [ ] Sei qual biblioteca UI usar (ou não)

### QA
- [ ] Entendi quais testes devo criar
- [ ] Sei como documentar os testes
- [ ] Sei quando testar cada slice
- [ ] Sei os critérios de aceite

---

## ?? MENSAGEM FINAL

### Somos um Time! ??

Este é um projeto desafiador, mas temos:
- ? Análise do legado completa
- ? Arquitetura bem definida
- ? Escopo realista
- ? Cronograma detalhado
- ? Squad experiente e comprometido

### Expectativas
- ?? Entregas pontuais
- ?? Comunicação constante
- ?? Colaboração entre devs
- ?? Feedbacks rápidos
- ?? Documentação clara

### Vamos modernizar o PDPW! ??

---

## ?? PRÓXIMA REUNIÃO

**Daily Standup - 20/12/2024 às 09:00**

Pauta:
1. O que fiz ontem?
2. O que vou fazer hoje?
3. Tenho algum bloqueio?

---

**Apresentação preparada por:** Tech Lead  
**Data:** 19/12/2024  
**Versão:** 1.0  
**Status:** ? Pronto para reunião das 15h
