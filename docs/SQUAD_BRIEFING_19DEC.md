# ?? BRIEFING DO SQUAD - PoC PDPW
**Data:** 19/12/2024 - 15:00h  
**Projeto:** Modernização PDPW - ONS  
**Prazo Final:** 26/12/2024

---

## ?? OBJETIVO DA REUNIÃO

1. ? Setup do ambiente de desenvolvimento completo
2. ?? Análise das primeiras impressões do repositório legado
3. ?? Estruturação e divisão das atividades entre devs
4. ?? Alinhamento de cronograma e entregas

---

## ?? ANÁLISE DO REPOSITÓRIO LEGADO

### ?? Localização
```
C:\temp\_ONS_PoC-PDPW\pdpw_act\pdpw\
```

### ?? Estatísticas do Código Legado

| Métrica | Quantidade | Observação |
|---------|------------|------------|
| **Arquivos VB.NET** | 473 | Código backend |
| **Arquivos ASPX** | 168 | Páginas WebForms |
| **Tecnologia** | .NET Framework | VB.NET + WebForms |
| **Banco de Dados** | SQL Server | Anteriormente Informix |
| **Arquitetura** | Monolítica | 3 camadas (DAO/Business/DTOs) |

### ??? Estrutura do Código Legado

```
pdpw_act/pdpw/
??? Business/          # Lógica de negócio
??? Dao/               # Acesso a dados (Data Access Objects)
??? DTOs/              # Data Transfer Objects
??? Common/            # Classes base e utilitários
??? Model/             # Modelos auxiliares
??? Enums/             # Enumerações
??? Inteface/          # Interfaces
??? ons.pdpw.test/     # Testes unitários
??? *.aspx             # 168 páginas WebForms
??? Web.config         # Configuração da aplicação
??? Connected Services # Web Services (SAGIC)
```

### ?? Arquitetura Legada Identificada

#### ? PONTOS POSITIVOS
- ? **Separação em camadas** (DAO/Business/DTO)
- ? **Padrão Repository** implementado (BaseDAO)
- ? **DTOs definidos** para transferência de dados
- ? **Sistema de cache** implementado
- ? **Testes unitários** existentes
- ? **Logging estruturado** (Log4Net + ElasticSearch)
- ? **Uso de herança** para reuso de código

#### ?? PONTOS DE ATENÇÃO
- ?? **WebForms legado** (dificulta migração de UI)
- ?? **VB.NET** em vez de C# (requer conversão)
- ?? **SQL inline** nos DAOs (sem ORM)
- ?? **Dependências antigas** (.NET Framework 4.8)
- ?? **Autenticação complexa** (POP/Forms Authentication)
- ?? **Configurações hardcoded** no Web.config

---

## ?? VERTICAL SLICES DEFINIDOS

### **SLICE 1: Cadastro de Usinas** ???
**Prioridade:** ALTA | **Complexidade:** MÉDIA | **Tempo:** 2 dias

#### Backend (2 devs - 1 dia)
- [x] Análise do código legado concluída
- [ ] Entidade `Usina` no Domain
- [ ] Interface `IUsinaRepository`
- [ ] Implementação do repositório
- [ ] DTOs (Request/Response)
- [ ] Service com validações
- [ ] Controller com 6 endpoints
- [ ] Testes unitários

#### Frontend (1 dev - 1 dia)
- [ ] Componente de listagem
- [ ] Componente de formulário
- [ ] Serviço de integração com API
- [ ] Validações de formulário
- [ ] Filtros e busca

#### Código Fonte Legado de Referência
- `pdpw_act/pdpw/Dao/UsinaDAO.vb` ?
- `pdpw_act/pdpw/DTOs/UsinaDTO.vb` ?
- `pdpw_act/pdpw/Business/UsinaBusiness.vb`

---

### **SLICE 2: Consulta de Arquivos DADGER** ???
**Prioridade:** ALTA | **Complexidade:** ALTA | **Tempo:** 3 dias

#### Backend (2 devs - 2 dias)
- [x] Análise do código legado concluída
- [ ] Entidades: `ArquivoDadger`, `ArquivoDadgerValor`, `SemanaPMO`
- [ ] Repositórios com JOINs complexos
- [ ] Services com filtros
- [ ] Controller com 5 endpoints
- [ ] Seed data com relacionamentos
- [ ] Testes de integração

#### Frontend (1 dev - 1 dia)
- [ ] Componente de consulta
- [ ] Filtros dinâmicos (período, usina, semana)
- [ ] Grid com valores tabulares
- [ ] Detalhamento de valores

#### Código Fonte Legado de Referência
- `pdpw_act/pdpw/Dao/ArquivoDadgerValorDAO.vb` ?
- `pdpw_act/pdpw/DTOs/ArquivoDadgerValorDTO.vb`
- `pdpw_act/pdpw/frmCnsArquivo.aspx` ?

---

## ?? DIVISÃO DE TAREFAS POR PESSOA

### ?? **DEV 1 - Backend Lead (SLICE 1: Usinas)**
**Responsabilidade:** Implementar CRUD completo de Usinas

#### Dia 1 (19/12 - Quinta) - 8h
- [ ] **09:00-10:00** - Setup ambiente (.NET 8 SDK, Docker)
- [ ] **10:00-12:00** - Criar entidade `Usina` no Domain
- [ ] **13:00-14:00** - Criar interface `IUsinaRepository`
- [ ] **14:00-16:00** - Implementar repositório no Infrastructure
- [ ] **16:00-18:00** - Configurar DbContext e seed data

#### Dia 2 (20/12 - Sexta) - 8h
- [ ] **09:00-11:00** - Criar DTOs e Mapper
- [ ] **11:00-13:00** - Implementar Service com validações
- [ ] **14:00-16:00** - Criar Controller com endpoints
- [ ] **16:00-18:00** - Testes unitários básicos

**Entregáveis:**
- ? API REST com 6 endpoints funcionando
- ? Swagger documentado
- ? Testes passando

**Arquivos a criar:**
```
src/PDPW.Domain/Entities/Usina.cs
src/PDPW.Domain/Interfaces/IUsinaRepository.cs
src/PDPW.Application/DTOs/Usina/UsinaRequestDTO.cs
src/PDPW.Application/DTOs/Usina/UsinaResponseDTO.cs
src/PDPW.Application/Services/UsinaService.cs
src/PDPW.Infrastructure/Repositories/UsinaRepository.cs
src/PDPW.API/Controllers/UsinasController.cs
```

---

### ?? **DEV 2 - Backend (SLICE 2: ArquivoDadger)**
**Responsabilidade:** Implementar consulta de arquivos DADGER

#### Dia 1 (19/12 - Quinta) - 8h
- [ ] **09:00-10:00** - Setup ambiente
- [ ] **10:00-12:00** - Analisar código legado DADGER
- [ ] **13:00-15:00** - Criar entidades (ArquivoDadger, ArquivoDadgerValor, SemanaPMO)
- [ ] **15:00-18:00** - Criar interfaces e configurar relacionamentos EF

#### Dia 2-3 (21-22/12 - Sábado/Domingo) - 16h
- [ ] **Sábado Manhã** - Implementar repositórios com JOINs
- [ ] **Sábado Tarde** - Criar services com filtros complexos
- [ ] **Domingo Manhã** - Criar DTOs e controllers
- [ ] **Domingo Tarde** - Testes de integração

**Entregáveis:**
- ? API REST com 5 endpoints
- ? Relacionamentos funcionando
- ? Filtros por período, usina e semana
- ? Seed data populado

**Arquivos a criar:**
```
src/PDPW.Domain/Entities/ArquivoDadger.cs
src/PDPW.Domain/Entities/ArquivoDadgerValor.cs
src/PDPW.Domain/Entities/SemanaPMO.cs
src/PDPW.Application/Services/ArquivoDadgerService.cs
src/PDPW.Infrastructure/Repositories/ArquivoDadgerRepository.cs
src/PDPW.API/Controllers/ArquivosDadgerController.cs
```

---

### ?? **DEV 3 - Frontend Lead (React + TypeScript)**
**Responsabilidade:** Implementar interfaces de usuário

#### Dia 1 (19/12 - Quinta) - 8h
- [ ] **09:00-10:00** - Setup ambiente (Node.js 20, VS Code)
- [ ] **10:00-12:00** - Analisar telas legadas (.aspx)
- [ ] **13:00-15:00** - Criar estrutura de componentes React
- [ ] **15:00-18:00** - Configurar Axios e rotas

#### Dia 2 (20/12 - Sexta) - 8h
- [ ] **09:00-13:00** - Criar componentes Usinas (lista + form)
- [ ] **14:00-18:00** - Integração com API + validações

#### Dia 3 (21/12 - Sábado) - 8h
- [ ] **09:00-13:00** - Criar componentes DADGER (consulta + grid)
- [ ] **14:00-18:00** - Implementar filtros dinâmicos

**Entregáveis:**
- ? 2 telas funcionais (Usinas + DADGER)
- ? Validações de formulário
- ? Integração completa com backend
- ? UI responsiva

**Arquivos a criar:**
```
frontend/src/pages/Usinas/UsinasListPage.tsx
frontend/src/pages/Usinas/UsinaFormPage.tsx
frontend/src/pages/DADGER/DadgerConsultaPage.tsx
frontend/src/services/usinaService.ts
frontend/src/services/dadgerService.ts
```

---

### ?? **QA Specialist - Quality Assurance**
**Responsabilidade:** Garantir qualidade e documentação

#### Diário (19-23/12) - 4h/dia
- [ ] **Manhã** - Testar features entregues do dia anterior
- [ ] **Tarde** - Documentar bugs e validar correções

#### Atividades Específicas
- [ ] Criar casos de teste para SLICE 1 (Usinas)
- [ ] Criar casos de teste para SLICE 2 (DADGER)
- [ ] Validar endpoints da API via Swagger
- [ ] Testar integração frontend/backend
- [ ] Validar responsividade
- [ ] Documentar fluxos de teste
- [ ] Criar checklist de validação

#### Dia Final (24/12 - Terça)
- [ ] **09:00-13:00** - Testes de regressão completos
- [ ] **14:00-18:00** - Validação Docker Compose
- [ ] **Final** - Sign-off da qualidade

**Entregáveis:**
- ? Plano de testes documentado
- ? Casos de teste executados
- ? Relatório de bugs (se houver)
- ? Checklist de validação final

**Arquivos a criar:**
```
docs/TEST_PLAN.md
docs/TEST_CASES_USINAS.md
docs/TEST_CASES_DADGER.md
docs/BUG_REPORT.md
docs/QUALITY_CHECKLIST.md
```

---

## ??? SETUP DO AMBIENTE DE DESENVOLVIMENTO

### Pré-requisitos

#### Todos os Devs
```powershell
# Verificar instalações
node --version          # Deve ser >= 20.x
dotnet --version        # Deve ser >= 8.0
docker --version        # Deve estar instalado
git --version           # Deve estar instalado
```

#### Backend Devs (DEV 1 e DEV 2)
```powershell
# Instalar .NET 8 SDK
winget install Microsoft.DotNet.SDK.8

# Instalar SQL Server Express (opcional)
winget install Microsoft.SQLServer.2022.Express

# Visual Studio 2022 ou Rider
winget install Microsoft.VisualStudio.2022.Community
```

#### Frontend Dev (DEV 3)
```powershell
# Instalar Node.js 20 LTS
winget install OpenJS.NodeJS.LTS

# Instalar VS Code
winget install Microsoft.VisualStudioCode

# Extensões recomendadas
code --install-extension dsznajder.es7-react-js-snippets
code --install-extension dbaeumer.vscode-eslint
code --install-extension esbenp.prettier-vscode
```

### Clonar o Repositório

```powershell
# Navegar para diretório de trabalho
cd C:\temp

# Clonar (se não clonou ainda)
git clone https://github.com/wbulhoes/ONS_PoC-PDPW.git
cd ONS_PoC-PDPW

# Criar branch de desenvolvimento
git checkout -b develop
git push -u origin develop
```

### Executar o Projeto

#### Backend
```powershell
cd src\PDPW.API
dotnet restore
dotnet run
```
Acesso: http://localhost:5000/swagger

#### Frontend
```powershell
cd frontend
npm install
npm run dev
```
Acesso: http://localhost:3000

#### Docker Compose (Quando pronto)
```powershell
# Na raiz do projeto
docker-compose up --build
```

---

## ?? CRONOGRAMA DETALHADO

| Data | Atividade | Responsável | Status |
|------|-----------|-------------|--------|
| **19/12 (Quinta)** | | | |
| 09:00-10:00 | Setup de ambiente | Todos | ?? |
| 10:00-12:00 | Kick-off + análise legado | Todos | ?? |
| 13:00-18:00 | Iniciar SLICE 1 (Backend) | DEV 1 | ?? |
| 13:00-18:00 | Iniciar SLICE 2 (Backend) | DEV 2 | ?? |
| 13:00-18:00 | Setup Frontend | DEV 3 | ?? |
| **20/12 (Sexta)** | | | |
| 09:00-18:00 | Concluir SLICE 1 Backend | DEV 1 | ? |
| 09:00-18:00 | Desenvolver SLICE 2 | DEV 2 | ? |
| 09:00-18:00 | Desenvolver UI Usinas | DEV 3 | ? |
| 16:00-18:00 | Testar SLICE 1 | QA | ? |
| **21/12 (Sábado)** | | | |
| 09:00-13:00 | Integração Frontend/Backend | DEV 1 + DEV 3 | ? |
| 09:00-18:00 | Desenvolver SLICE 2 Backend | DEV 2 | ? |
| 14:00-18:00 | Desenvolver UI DADGER | DEV 3 | ? |
| **22/12 (Domingo)** | | | |
| 09:00-18:00 | Concluir SLICE 2 Backend | DEV 2 | ? |
| 09:00-13:00 | Concluir UI DADGER | DEV 3 | ? |
| 14:00-18:00 | Integração SLICE 2 | DEV 2 + DEV 3 | ? |
| **23/12 (Segunda)** | | | |
| 09:00-13:00 | Ajustes e correções | Todos | ? |
| 14:00-18:00 | Testes integrados | QA + Todos | ? |
| **24/12 (Terça)** | | | |
| 09:00-13:00 | Docker Compose + Deploy | DEV 1 | ? |
| 09:00-13:00 | Testes finais | QA | ? |
| 14:00-18:00 | Documentação final | Todos | ? |
| **25/12 (Quarta)** | **FERIADO** | - | ?? |
| **26/12 (Quinta)** | | | |
| 09:00-13:00 | Preparar apresentação | Todos | ? |
| 14:00-18:00 | Commit final + entrega | Todos | ? |

---

## ?? MÉTRICAS DE SUCESSO

### SLICE 1: Cadastro de Usinas
- [ ] 6 endpoints REST funcionando
- [ ] Swagger documentado
- [ ] Tela de listagem funcional
- [ ] Formulário de cadastro funcional
- [ ] Validações implementadas
- [ ] 5-10 usinas seed populadas
- [ ] Cobertura de testes > 70%

### SLICE 2: Consulta DADGER
- [ ] 5 endpoints REST funcionando
- [ ] JOINs funcionando corretamente
- [ ] Filtros por período, usina e semana
- [ ] Grid de consulta funcional
- [ ] Dados seed com relacionamentos
- [ ] Cobertura de testes > 70%

### Infraestrutura
- [ ] Docker Compose executando
- [ ] Backend containerizado
- [ ] Frontend containerizado
- [ ] InMemory Database funcionando
- [ ] Logs estruturados

### Documentação
- [ ] README atualizado
- [ ] Arquitetura documentada
- [ ] Decisões técnicas registradas
- [ ] Apresentação preparada

---

## ?? RISCOS E MITIGAÇÕES

| Risco | Probabilidade | Impacto | Mitigação |
|-------|--------------|---------|-----------|
| Complexidade do código legado | ALTA | ALTO | Análise prévia concluída; foco em 2 slices |
| Prazo apertado (7 dias úteis) | ALTA | ALTO | Trabalho aos finais de semana; escopo reduzido |
| Falta de banco legado | ALTA | MÉDIO | ? RESOLVIDO: InMemory Database |
| Problemas de integração | MÉDIA | ALTO | Testes contínuos; QA dedicado |
| Bugs de última hora | MÉDIA | MÉDIO | Buffer de 1 dia para correções (24/12) |
| Dependências externas | BAIXA | ALTO | Evitar integrações complexas na PoC |

---

## ?? COMUNICAÇÃO DO SQUAD

### Daily Standup (15 minutos)
- **Horário:** 09:00 (todos os dias)
- **Formato:** O que fiz? O que vou fazer? Tenho bloqueios?

### Canais
- **Teams/Slack:** Comunicação assíncrona
- **GitHub Issues:** Rastreamento de tarefas
- **GitHub Projects:** Board Kanban

### Commits
```bash
# Padrão de commit
git commit -m "[SLICE-1] feat: adiciona entidade Usina"
git commit -m "[SLICE-2] fix: corrige filtro de data"
git commit -m "[DOCS] docs: atualiza README"
git commit -m "[TEST] test: adiciona testes de Usina"
```

### Pull Requests
- **Mínimo 1 revisor**
- **CI deve passar**
- **Merge apenas se aprovado**

---

## ?? AÇÕES IMEDIATAS PÓS-REUNIÃO

### Para DEV 1
1. ? Verificar setup do .NET 8
2. ? Clonar repositório
3. ? Criar branch `feature/slice-1-usinas`
4. ?? Iniciar criação da entidade `Usina.cs`

### Para DEV 2
1. ? Verificar setup do .NET 8
2. ? Clonar repositório
3. ? Criar branch `feature/slice-2-dadger`
4. ?? Analisar código legado de ArquivoDadgerValorDAO.vb

### Para DEV 3
1. ? Verificar setup do Node.js 20
2. ? Clonar repositório
3. ? Criar branch `feature/frontend-slices`
4. ?? Analisar telas legadas (frmCnsArquivo.aspx)

### Para QA
1. ? Verificar acesso ao repositório
2. ? Instalar Postman/Insomnia
3. ?? Criar estrutura de documentação de testes
4. ?? Preparar casos de teste para SLICE 1

---

## ?? REFERÊNCIAS ÚTEIS

### Documentação do Projeto
- [README.md](../README.md)
- [VERTICAL_SLICES_DECISION.md](../VERTICAL_SLICES_DECISION.md)
- [RESUMO_EXECUTIVO.md](../RESUMO_EXECUTIVO.md)
- [GLOSSARIO.md](../GLOSSARIO.md)

### Código Legado
- `pdpw_act/pdpw/Dao/UsinaDAO.vb` - Referência para Usinas
- `pdpw_act/pdpw/Dao/ArquivoDadgerValorDAO.vb` - Referência para DADGER
- `pdpw_act/pdpw/frmCnsArquivo.aspx` - Tela de consulta

### Tecnologias
- [.NET 8 Docs](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8)
- [React 18 Docs](https://react.dev)
- [Docker Compose](https://docs.docker.com/compose/)
- [EF Core InMemory](https://learn.microsoft.com/ef/core/providers/in-memory/)

---

## ? CHECKLIST PRÉ-INÍCIO

Antes de começar a desenvolver, cada dev deve confirmar:

### Backend Devs
- [ ] .NET 8 SDK instalado e funcionando
- [ ] Visual Studio 2022 ou Rider configurado
- [ ] Repositório clonado
- [ ] Branch criada
- [ ] Solução compila sem erros (`dotnet build`)
- [ ] API sobe sem erros (`dotnet run`)

### Frontend Dev
- [ ] Node.js 20 instalado
- [ ] VS Code com extensões instaladas
- [ ] Repositório clonado
- [ ] Branch criada
- [ ] Dependências instaladas (`npm install`)
- [ ] Dev server sobe (`npm run dev`)

### QA
- [ ] Postman/Insomnia instalado
- [ ] Acesso ao repositório
- [ ] Estrutura de testes criada
- [ ] Casos de teste documentados

---

## ?? MENSAGEM FINAL

**Estamos prontos para começar!** ??

Este é um projeto desafiador mas muito bem planejado. Temos:
- ? Análise do legado concluída
- ? Arquitetura definida
- ? Escopo claro (2 slices)
- ? Cronograma realista
- ? Squad completo e alinhado

**Sucesso é garantido com:** trabalho em equipe, comunicação constante e foco no objetivo.

**Vamos modernizar o PDPW!** ??

---

**Preparado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Versão:** 1.0  
**Próxima reunião:** Daily Standup - 20/12 às 09:00
