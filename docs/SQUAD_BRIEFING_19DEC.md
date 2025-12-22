# ?? BRIEFING DO SQUAD - PoC PDPW
**Data:** 19/12/2024 - 15:00h  
**Projeto:** Moderniza��o PDPW - ONS  
**Prazo Final:** 26/12/2024

---

## ?? OBJETIVO DA REUNI�O

1. ? Setup do ambiente de desenvolvimento completo
2. ?? An�lise das primeiras impress�es do reposit�rio legado
3. ?? Estrutura��o e divis�o das atividades entre devs
4. ?? Alinhamento de cronograma e entregas

---

## ?? AN�LISE DO REPOSIT�RIO LEGADO

### ?? Localiza��o
```
C:\temp\_ONS_PoC-PDPW\pdpw_act\pdpw\
```

### ?? Estat�sticas do C�digo Legado

| M�trica | Quantidade | Observa��o |
|---------|------------|------------|
| **Arquivos VB.NET** | 473 | C�digo backend |
| **Arquivos ASPX** | 168 | P�ginas WebForms |
| **Tecnologia** | .NET Framework | VB.NET + WebForms |
| **Banco de Dados** | SQL Server | Anteriormente Informix |
| **Arquitetura** | Monol�tica | 3 camadas (DAO/Business/DTOs) |

### ??? Estrutura do C�digo Legado

```
pdpw_act/pdpw/
??? Business/          # L�gica de neg�cio
??? Dao/               # Acesso a dados (Data Access Objects)
??? DTOs/              # Data Transfer Objects
??? Common/            # Classes base e utilit�rios
??? Model/             # Modelos auxiliares
??? Enums/             # Enumera��es
??? Inteface/          # Interfaces
??? ons.pdpw.test/     # Testes unit�rios
??? *.aspx             # 168 p�ginas WebForms
??? Web.config         # Configura��o da aplica��o
??? Connected Services # Web Services (SAGIC)
```

### ?? Arquitetura Legada Identificada

#### ? PONTOS POSITIVOS
- ? **Separa��o em camadas** (DAO/Business/DTO)
- ? **Padr�o Repository** implementado (BaseDAO)
- ? **DTOs definidos** para transfer�ncia de dados
- ? **Sistema de cache** implementado
- ? **Testes unit�rios** existentes
- ? **Logging estruturado** (Log4Net + ElasticSearch)
- ? **Uso de heran�a** para reuso de c�digo

#### ?? PONTOS DE ATEN��O
- ?? **WebForms legado** (dificulta migra��o de UI)
- ?? **VB.NET** em vez de C# (requer convers�o)
- ?? **SQL inline** nos DAOs (sem ORM)
- ?? **Depend�ncias antigas** (.NET Framework 4.8)
- ?? **Autentica��o complexa** (POP/Forms Authentication)
- ?? **Configura��es hardcoded** no Web.config

---

## ?? VERTICAL SLICES DEFINIDOS

### **SLICE 1: Cadastro de Usinas** ???
**Prioridade:** ALTA | **Complexidade:** M�DIA | **Tempo:** 2 dias

#### Backend (2 devs - 1 dia)
- [x] An�lise do c�digo legado conclu�da
- [ ] Entidade `Usina` no Domain
- [ ] Interface `IUsinaRepository`
- [ ] Implementa��o do reposit�rio
- [ ] DTOs (Request/Response)
- [ ] Service com valida��es
- [ ] Controller com 6 endpoints
- [ ] Testes unit�rios

#### Frontend (1 dev - 1 dia)
- [ ] Componente de listagem
- [ ] Componente de formul�rio
- [ ] Servi�o de integra��o com API
- [ ] Valida��es de formul�rio
- [ ] Filtros e busca

#### C�digo Fonte Legado de Refer�ncia
- `pdpw_act/pdpw/Dao/UsinaDAO.vb` ?
- `pdpw_act/pdpw/DTOs/UsinaDTO.vb` ?
- `pdpw_act/pdpw/Business/UsinaBusiness.vb`

---

### **SLICE 2: Consulta de Arquivos DADGER** ???
**Prioridade:** ALTA | **Complexidade:** ALTA | **Tempo:** 3 dias

#### Backend (2 devs - 2 dias)
- [x] An�lise do c�digo legado conclu�da
- [ ] Entidades: `ArquivoDadger`, `ArquivoDadgerValor`, `SemanaPMO`
- [ ] Reposit�rios com JOINs complexos
- [ ] Services com filtros
- [ ] Controller com 5 endpoints
- [ ] Seed data com relacionamentos
- [ ] Testes de integra��o

#### Frontend (1 dev - 1 dia)
- [ ] Componente de consulta
- [ ] Filtros din�micos (per�odo, usina, semana)
- [ ] Grid com valores tabulares
- [ ] Detalhamento de valores

#### C�digo Fonte Legado de Refer�ncia
- `pdpw_act/pdpw/Dao/ArquivoDadgerValorDAO.vb` ?
- `pdpw_act/pdpw/DTOs/ArquivoDadgerValorDTO.vb`
- `pdpw_act/pdpw/frmCnsArquivo.aspx` ?

---

## ?? DIVIS�O DE TAREFAS POR PESSOA

### ?? **DEV 1 - Backend Lead (SLICE 1: Usinas)**
**Responsabilidade:** Implementar CRUD completo de Usinas

#### Dia 1 (19/12 - Quinta) - 8h
- [ ] **09:00-10:00** - Setup ambiente (.NET 8 SDK, Docker)
- [ ] **10:00-12:00** - Criar entidade `Usina` no Domain
- [ ] **13:00-14:00** - Criar interface `IUsinaRepository`
- [ ] **14:00-16:00** - Implementar reposit�rio no Infrastructure
- [ ] **16:00-18:00** - Configurar DbContext e seed data

#### Dia 2 (20/12 - Sexta) - 8h
- [ ] **09:00-11:00** - Criar DTOs e Mapper
- [ ] **11:00-13:00** - Implementar Service com valida��es
- [ ] **14:00-16:00** - Criar Controller com endpoints
- [ ] **16:00-18:00** - Testes unit�rios b�sicos

**Entreg�veis:**
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
- [ ] **10:00-12:00** - Analisar c�digo legado DADGER
- [ ] **13:00-15:00** - Criar entidades (ArquivoDadger, ArquivoDadgerValor, SemanaPMO)
- [ ] **15:00-18:00** - Criar interfaces e configurar relacionamentos EF

#### Dia 2-3 (21-22/12 - S�bado/Domingo) - 16h
- [ ] **S�bado Manh�** - Implementar reposit�rios com JOINs
- [ ] **S�bado Tarde** - Criar services com filtros complexos
- [ ] **Domingo Manh�** - Criar DTOs e controllers
- [ ] **Domingo Tarde** - Testes de integra��o

**Entreg�veis:**
- ? API REST com 5 endpoints
- ? Relacionamentos funcionando
- ? Filtros por per�odo, usina e semana
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
**Responsabilidade:** Implementar interfaces de usu�rio

#### Dia 1 (19/12 - Quinta) - 8h
- [ ] **09:00-10:00** - Setup ambiente (Node.js 20, VS Code)
- [ ] **10:00-12:00** - Analisar telas legadas (.aspx)
- [ ] **13:00-15:00** - Criar estrutura de componentes React
- [ ] **15:00-18:00** - Configurar Axios e rotas

#### Dia 2 (20/12 - Sexta) - 8h
- [ ] **09:00-13:00** - Criar componentes Usinas (lista + form)
- [ ] **14:00-18:00** - Integra��o com API + valida��es

#### Dia 3 (21/12 - S�bado) - 8h
- [ ] **09:00-13:00** - Criar componentes DADGER (consulta + grid)
- [ ] **14:00-18:00** - Implementar filtros din�micos

**Entreg�veis:**
- ? 2 telas funcionais (Usinas + DADGER)
- ? Valida��es de formul�rio
- ? Integra��o completa com backend
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
**Responsabilidade:** Garantir qualidade e documenta��o

#### Di�rio (19-23/12) - 4h/dia
- [ ] **Manh�** - Testar features entregues do dia anterior
- [ ] **Tarde** - Documentar bugs e validar corre��es

#### Atividades Espec�ficas
- [ ] Criar casos de teste para SLICE 1 (Usinas)
- [ ] Criar casos de teste para SLICE 2 (DADGER)
- [ ] Validar endpoints da API via Swagger
- [ ] Testar integra��o frontend/backend
- [ ] Validar responsividade
- [ ] Documentar fluxos de teste
- [ ] Criar checklist de valida��o

#### Dia Final (24/12 - Ter�a)
- [ ] **09:00-13:00** - Testes de regress�o completos
- [ ] **14:00-18:00** - Valida��o Docker Compose
- [ ] **Final** - Sign-off da qualidade

**Entreg�veis:**
- ? Plano de testes documentado
- ? Casos de teste executados
- ? Relat�rio de bugs (se houver)
- ? Checklist de valida��o final

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

### Pr�-requisitos

#### Todos os Devs
```powershell
# Verificar instala��es
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

# Extens�es recomendadas
code --install-extension dsznajder.es7-react-js-snippets
code --install-extension dbaeumer.vscode-eslint
code --install-extension esbenp.prettier-vscode
```

### Clonar o Reposit�rio

```powershell
# Navegar para diret�rio de trabalho
cd C:\temp

# Clonar (se n�o clonou ainda)
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

| Data | Atividade | Respons�vel | Status |
|------|-----------|-------------|--------|
| **19/12 (Quinta)** | | | |
| 09:00-10:00 | Setup de ambiente | Todos | ?? |
| 10:00-12:00 | Kick-off + an�lise legado | Todos | ?? |
| 13:00-18:00 | Iniciar SLICE 1 (Backend) | DEV 1 | ?? |
| 13:00-18:00 | Iniciar SLICE 2 (Backend) | DEV 2 | ?? |
| 13:00-18:00 | Setup Frontend | DEV 3 | ?? |
| **20/12 (Sexta)** | | | |
| 09:00-18:00 | Concluir SLICE 1 Backend | DEV 1 | ? |
| 09:00-18:00 | Desenvolver SLICE 2 | DEV 2 | ? |
| 09:00-18:00 | Desenvolver UI Usinas | DEV 3 | ? |
| 16:00-18:00 | Testar SLICE 1 | QA | ? |
| **21/12 (S�bado)** | | | |
| 09:00-13:00 | Integra��o Frontend/Backend | DEV 1 + DEV 3 | ? |
| 09:00-18:00 | Desenvolver SLICE 2 Backend | DEV 2 | ? |
| 14:00-18:00 | Desenvolver UI DADGER | DEV 3 | ? |
| **22/12 (Domingo)** | | | |
| 09:00-18:00 | Concluir SLICE 2 Backend | DEV 2 | ? |
| 09:00-13:00 | Concluir UI DADGER | DEV 3 | ? |
| 14:00-18:00 | Integra��o SLICE 2 | DEV 2 + DEV 3 | ? |
| **23/12 (Segunda)** | | | |
| 09:00-13:00 | Ajustes e corre��es | Todos | ? |
| 14:00-18:00 | Testes integrados | QA + Todos | ? |
| **24/12 (Ter�a)** | | | |
| 09:00-13:00 | Docker Compose + Deploy | DEV 1 | ? |
| 09:00-13:00 | Testes finais | QA | ? |
| 14:00-18:00 | Documenta��o final | Todos | ? |
| **25/12 (Quarta)** | **FERIADO** | - | ?? |
| **26/12 (Quinta)** | | | |
| 09:00-13:00 | Preparar apresenta��o | Todos | ? |
| 14:00-18:00 | Commit final + entrega | Todos | ? |

---

## ?? M�TRICAS DE SUCESSO

### SLICE 1: Cadastro de Usinas
- [ ] 6 endpoints REST funcionando
- [ ] Swagger documentado
- [ ] Tela de listagem funcional
- [ ] Formul�rio de cadastro funcional
- [ ] Valida��es implementadas
- [ ] 5-10 usinas seed populadas
- [ ] Cobertura de testes > 70%

### SLICE 2: Consulta DADGER
- [ ] 5 endpoints REST funcionando
- [ ] JOINs funcionando corretamente
- [ ] Filtros por per�odo, usina e semana
- [ ] Grid de consulta funcional
- [ ] Dados seed com relacionamentos
- [ ] Cobertura de testes > 70%

### Infraestrutura
- [ ] Docker Compose executando
- [ ] Backend containerizado
- [ ] Frontend containerizado
- [ ] InMemory Database funcionando
- [ ] Logs estruturados

### Documenta��o
- [ ] README atualizado
- [ ] Arquitetura documentada
- [ ] Decis�es t�cnicas registradas
- [ ] Apresenta��o preparada

---

## ?? RISCOS E MITIGA��ES

| Risco | Probabilidade | Impacto | Mitiga��o |
|-------|--------------|---------|-----------|
| Complexidade do c�digo legado | ALTA | ALTO | An�lise pr�via conclu�da; foco em 2 slices |
| Prazo apertado (7 dias �teis) | ALTA | ALTO | Trabalho aos finais de semana; escopo reduzido |
| Falta de banco legado | ALTA | M�DIO | ? RESOLVIDO: InMemory Database |
| Problemas de integra��o | M�DIA | ALTO | Testes cont�nuos; QA dedicado |
| Bugs de �ltima hora | M�DIA | M�DIO | Buffer de 1 dia para corre��es (24/12) |
| Depend�ncias externas | BAIXA | ALTO | Evitar integra��es complexas na PoC |

---

## ?? COMUNICA��O DO SQUAD

### Daily Standup (15 minutos)
- **Hor�rio:** 09:00 (todos os dias)
- **Formato:** O que fiz? O que vou fazer? Tenho bloqueios?

### Canais
- **Teams/Slack:** Comunica��o ass�ncrona
- **GitHub Issues:** Rastreamento de tarefas
- **GitHub Projects:** Board Kanban

### Commits
```bash
# Padr�o de commit
git commit -m "[SLICE-1] feat: adiciona entidade Usina"
git commit -m "[SLICE-2] fix: corrige filtro de data"
git commit -m "[DOCS] docs: atualiza README"
git commit -m "[TEST] test: adiciona testes de Usina"
```

### Pull Requests
- **M�nimo 1 revisor**
- **CI deve passar**
- **Merge apenas se aprovado**

---

## ?? A��ES IMEDIATAS P�S-REUNI�O

### Para DEV 1
1. ? Verificar setup do .NET 8
2. ? Clonar reposit�rio
3. ? Criar branch `feature/slice-1-usinas`
4. ?? Iniciar cria��o da entidade `Usina.cs`

### Para DEV 2
1. ? Verificar setup do .NET 8
2. ? Clonar reposit�rio
3. ? Criar branch `feature/slice-2-dadger`
4. ?? Analisar c�digo legado de ArquivoDadgerValorDAO.vb

### Para DEV 3
1. ? Verificar setup do Node.js 20
2. ? Clonar reposit�rio
3. ? Criar branch `feature/frontend-slices`
4. ?? Analisar telas legadas (frmCnsArquivo.aspx)

### Para QA
1. ? Verificar acesso ao reposit�rio
2. ? Instalar Postman/Insomnia
3. ?? Criar estrutura de documenta��o de testes
4. ?? Preparar casos de teste para SLICE 1

---

## ?? REFER�NCIAS �TEIS

### Documenta��o do Projeto
- [README.md](../README.md)
- [VERTICAL_SLICES_DECISION.md](../VERTICAL_SLICES_DECISION.md)
- [RESUMO_EXECUTIVO.md](../RESUMO_EXECUTIVO.md)
- [GLOSSARIO.md](../GLOSSARIO.md)

### C�digo Legado
- `pdpw_act/pdpw/Dao/UsinaDAO.vb` - Refer�ncia para Usinas
- `pdpw_act/pdpw/Dao/ArquivoDadgerValorDAO.vb` - Refer�ncia para DADGER
- `pdpw_act/pdpw/frmCnsArquivo.aspx` - Tela de consulta

### Tecnologias
- [.NET 8 Docs](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-8)
- [React 18 Docs](https://react.dev)
- [Docker Compose](https://docs.docker.com/compose/)
- [EF Core InMemory](https://learn.microsoft.com/ef/core/providers/in-memory/)

---

## ? CHECKLIST PR�-IN�CIO

Antes de come�ar a desenvolver, cada dev deve confirmar:

### Backend Devs
- [ ] .NET 8 SDK instalado e funcionando
- [ ] Visual Studio 2022 ou Rider configurado
- [ ] Reposit�rio clonado
- [ ] Branch criada
- [ ] Solu��o compila sem erros (`dotnet build`)
- [ ] API sobe sem erros (`dotnet run`)

### Frontend Dev
- [ ] Node.js 20 instalado
- [ ] VS Code com extens�es instaladas
- [ ] Reposit�rio clonado
- [ ] Branch criada
- [ ] Depend�ncias instaladas (`npm install`)
- [ ] Dev server sobe (`npm run dev`)

### QA
- [ ] Postman/Insomnia instalado
- [ ] Acesso ao reposit�rio
- [ ] Estrutura de testes criada
- [ ] Casos de teste documentados

---

## ?? MENSAGEM FINAL

**Estamos prontos para come�ar!** ??

Este � um projeto desafiador mas muito bem planejado. Temos:
- ? An�lise do legado conclu�da
- ? Arquitetura definida
- ? Escopo claro (2 slices)
- ? Cronograma realista
- ? Squad completo e alinhado

**Sucesso � garantido com:** trabalho em equipe, comunica��o constante e foco no objetivo.

**Vamos modernizar o PDPW!** ??

---

**Preparado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Vers�o:** 1.0  
**Pr�xima reuni�o:** Daily Standup - 20/12 �s 09:00
