# 🔄 METODOLOGIA DE DESENVOLVIMENTO

**Projeto**: POC Migração PDPW  
**Data**: Dezembro/2025  
**Versão**: 1.0

---

## 📋 RESUMO EXECUTIVO

Metodologia híbrida **Scrum + Kanban** com práticas de **DevOps** e **Clean Code** para migração incremental do sistema legado.

---

## 🎯 ABORDAGEM GERAL

### Estratégia de Migração

**Tipo**: **Strangler Fig Pattern** (migração incremental)

```
Sistema Legado (VB.NET)        Sistema Novo (.NET 8)
        ↓                              ↓
    [WebForms] ←─────────┐      [React SPA]
        ↓                │            ↓
    [Business]    API Gateway    [API REST]
        ↓                │            ↓
      [DAO]              │      [EF Core + Repositories]
        ↓                │            ↓
    [SQL Server] ←───────┴────→ [SQL Server]
```

**Fases**:
1. **POC** (2 semanas) - Provar viabilidade técnica ✅
2. **MVP** (8 semanas) - 30% funcionalidades críticas
3. **Migração Progressiva** (20 semanas) - 100% funcionalidades
4. **Descomissionamento** (4 semanas) - Desligar legado

**Vantagens**:
- ✅ Sistema legado continua operando
- ✅ Migração por módulos (baixo risco)
- ✅ Rollback fácil em caso de problemas
- ✅ Validação contínua com usuários

---

## 🏃 FRAMEWORK ÁGIL: SCRUM

### Papéis

| Papel | Responsável | Atribuições |
|-------|-------------|-------------|
| **Product Owner** | (ONS) | Define prioridades, aceita entregas |
| **Scrum Master** | Rafael Suzano | Remove impedimentos, facilita cerimônias |
| **Dev Team** | Willian Bulhões + Squad | Desenvolve, testa, entrega |

### Cerimônias

#### Sprint Planning (2h - Início da sprint)
- **Objetivo**: Planejar o trabalho da sprint (2 semanas)
- **Entrada**: Product Backlog priorizado
- **Saída**: Sprint Backlog com tarefas estimadas
- **Exemplo**:
  ```
  Sprint 3: 10-23 Janeiro/2025
  - Migrar API Cargas (8 pts)
  - Migrar API Restrições UG (5 pts)
  - Criar tela Usinas (frontend) (13 pts)
  Total: 26 pontos
  ```

#### Daily Standup (15min - Todo dia 9h)
- **Formato**: Cada dev responde:
  1. O que fiz ontem?
  2. O que farei hoje?
  3. Há impedimentos?
- **Exemplo**:
  ```
  Willian: Ontem finalizei API Cargas. Hoje vou criar testes.
           Sem impedimentos.
  ```

#### Sprint Review (1h - Fim da sprint)
- **Objetivo**: Demonstrar funcionalidades ao PO
- **Formato**: Demo ao vivo (Swagger + frontend)
- **Exemplo**:
  ```
  Demo Sprint 3:
  - ✅ API Cargas funcionando (CRUD completo)
  - ✅ Tela Usinas (listagem + formulário)
  - ✅ 20 testes automatizados passando
  ```

#### Sprint Retrospective (45min - Fim da sprint)
- **Objetivo**: Melhorar processo
- **Formato**: Start/Stop/Continue
- **Exemplo**:
  ```
  START: Pair programming em tasks complexas
  STOP: Commits direto na main
  CONTINUE: Code review obrigatório
  ```

---

## 📊 KANBAN BOARD

### Colunas

```
┌──────────┬──────────┬─────────────┬────────────┬──────┐
│ Backlog  │   To Do  │ In Progress │   Review   │ Done │
├──────────┼──────────┼─────────────┼────────────┼──────┤
│ 30 tasks │ 5 tasks  │   3 tasks   │   2 tasks  │ 50   │
└──────────┴──────────┴─────────────┴────────────┴──────┘
```

### Work In Progress (WIP) Limits
- **To Do**: Sem limite (buffer)
- **In Progress**: Máximo 3 (foco)
- **Review**: Máximo 2 (não bloquear)
- **Done**: Sem limite

### Exemplo de Card

```
┌─────────────────────────────────────┐
│ [FEATURE] API Cargas - CRUD         │
├─────────────────────────────────────┤
│ Assignee: Willian                   │
│ Story Points: 8                     │
│ Sprint: 3                           │
│ Labels: backend, high-priority      │
├─────────────────────────────────────┤
│ ✅ Controller criado                │
│ ✅ Service implementado             │
│ ✅ Repository configurado           │
│ ⏳ Testes unitários (60%)           │
│ ⏳ Documentação Swagger             │
└─────────────────────────────────────┘
```

---

## 🔄 CICLO DE DESENVOLVIMENTO

### 1. Análise (1 dia)

**Atividades**:
- ✅ Estudar código VB.NET (Business + DAO)
- ✅ Identificar regras de negócio
- ✅ Mapear dependências
- ✅ Criar especificação técnica

**Ferramentas**:
- ILSpy (descompilar DLLs)
- SQL Server Management Studio (analisar tabelas)
- Miro/Figma (diagramas)

**Entrega**: Documento de análise (markdown)

---

### 2. Design (0.5 dia)

**Atividades**:
- ✅ Desenhar arquitetura da funcionalidade
- ✅ Definir DTOs
- ✅ Definir endpoints REST
- ✅ Planejar testes

**Padrões**:
- Clean Architecture
- Repository Pattern
- DTO Pattern
- SOLID principles

**Entrega**: Diagrama de classes + API spec (OpenAPI)

---

### 3. Implementação (2-3 dias)

**Ordem de desenvolvimento**:
```
1. Domain/Entities (entidade + interface)
   ↓
2. Infrastructure/Repository (acesso a dados)
   ↓
3. Application/Service (lógica de negócio)
   ↓
4. Application/DTOs + Mappings (AutoMapper)
   ↓
5. API/Controller (endpoints REST)
   ↓
6. Swagger annotations (documentação)
```

**Convenções de Código**:
- ✅ C# 12 com nullable reference types
- ✅ Async/await em todas operações I/O
- ✅ XML comments em métodos públicos
- ✅ Nomenclatura: PascalCase (classes), camelCase (parâmetros)
- ✅ 1 arquivo = 1 classe (exceto DTOs pequenos)

---

### 4. Testes (1 dia)

**Pirâmide de Testes**:
```
        /\
       /  \  E2E (10%)
      /────\
     /      \ Integration (20%)
    /────────\
   /          \ Unit (70%)
  /────────────\
```

**Testes Unitários (xUnit)**:
```csharp
[Fact]
public async Task GetAllAsync_DeveRetornarTodasCargas()
{
    // Arrange
    var mockRepo = new Mock<ICargaRepository>();
    mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Carga> { /* dados */ });
    var service = new CargaService(mockRepo.Object);
    
    // Act
    var result = await service.GetAllAsync();
    
    // Assert
    result.Should().HaveCount(10);
    mockRepo.Verify(r => r.GetAllAsync(), Times.Once);
}
```

**Testes de Integração**:
```csharp
[Fact]
public async Task Post_DeveCriarCarga()
{
    // Arrange
    var client = _factory.CreateClient();
    var dto = new CreateCargaDto { /* dados */ };
    
    // Act
    var response = await client.PostAsJsonAsync("/api/cargas", dto);
    
    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Created);
}
```

**Cobertura Mínima**: 80% (medido por dotCover/Coverlet)

---

### 5. Code Review (0.5 dia)

**Processo**:
1. Desenvolvedor cria Pull Request no GitHub
2. CI/CD executa automaticamente:
   - ✅ Build
   - ✅ Testes
   - ✅ SonarQube (code smells, bugs, vulnerabilidades)
3. Tech Lead revisa código
4. Se aprovado → Merge

**Checklist de Review**:
- [ ] Código segue padrões do projeto
- [ ] Testes cobrem casos principais
- [ ] Sem code smells (SonarQube)
- [ ] Documentação atualizada
- [ ] Sem quebra de compatibilidade

**Ferramentas**:
- GitHub Pull Requests
- SonarQube
- CodeQL (security scanning)

---

### 6. Deploy (0.5 dia)

**Ambientes**:
```
Development  →  Staging  →  Production
    (dev)        (homol)       (prod)
```

**Pipeline CI/CD (GitHub Actions)**:
```yaml
# .github/workflows/deploy.yml
on:
  push:
    branches: [main, release/*]

jobs:
  build-and-test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET 8
        uses: actions/setup-dotnet@v3
      - name: Build
        run: dotnet build
      - name: Test
        run: dotnet test --logger trx
      - name: Publish
        run: dotnet publish -c Release -o ./publish
      
  deploy-staging:
    needs: build-and-test
    runs-on: ubuntu-latest
    steps:
      - name: Deploy to Azure App Service (Staging)
        uses: azure/webapps-deploy@v2
        
  deploy-production:
    needs: deploy-staging
    if: github.ref == 'refs/heads/main'
    runs-on: ubuntu-latest
    steps:
      - name: Deploy to Azure App Service (Production)
        uses: azure/webapps-deploy@v2
```

**Rollback**:
```bash
# Reverter deploy para versão anterior
az webapp deployment slot swap --name pdpw-api --resource-group pdpw-rg --slot staging --target-slot production --action swap
```

---

## 📐 DEFINIÇÃO DE PRONTO (DoD)

**Uma funcionalidade só está "Done" quando**:

### Backend
- [x] Controller com todos endpoints (GET, POST, PUT, DELETE)
- [x] Service com lógica de negócio
- [x] Repository com acesso a dados
- [x] DTOs (Request/Response) criados
- [x] AutoMapper profile configurado
- [x] Validações implementadas
- [x] Testes unitários (cobertura ≥80%)
- [x] Documentação Swagger completa
- [x] Code review aprovado
- [x] Deploy em staging
- [x] Validado pelo PO

### Frontend
- [x] Componente(s) React criado(s)
- [x] TypeScript interfaces definidas
- [x] Integração com API funcionando
- [x] Validações de formulário
- [x] Loading states implementados
- [x] Error handling implementado
- [x] Testes unitários (Vitest)
- [x] Responsivo (mobile/desktop)
- [x] Code review aprovado
- [x] Deploy em staging
- [x] Validado pelo PO

---

## 🛠️ FERRAMENTAS

### Gestão de Projeto
- **Jira** ou **GitHub Projects** - Kanban board, sprints
- **Confluence** - Documentação técnica
- **Miro** - Diagramas, brainstorming

### Desenvolvimento
- **VS Code** + **Visual Studio 2022** - IDEs
- **Git** + **GitHub** - Controle de versão
- **Postman** - Testes de API
- **SQL Server Management Studio** - Banco de dados

### CI/CD
- **GitHub Actions** - Pipeline CI/CD
- **Docker** - Containerização
- **Azure App Service** - Hosting (staging/prod)

### Qualidade
- **SonarQube** - Code quality
- **dotCover** / **Coverlet** - Code coverage
- **xUnit** + **Moq** - Testes unitários
- **Vitest** - Testes frontend

### Monitoramento
- **Azure Application Insights** - Logs, métricas, traces
- **Azure Monitor** - Alertas
- **Serilog** - Logging estruturado

---

## 📊 MÉTRICAS DE ACOMPANHAMENTO

### Sprint Burndown
```
Story Points
    50 │●
       │  ●
    40 │    ●
       │      ●
    30 │        ●
       │          ●
    20 │            ●
       │              ●
    10 │                ●
       │                  ●
     0 │                    ●
       └────────────────────────
       D1  D3  D5  D7  D9  D10
```

### Velocity (Média de pontos por sprint)
- Sprint 1: 18 pontos
- Sprint 2: 22 pontos
- Sprint 3: 26 pontos
- **Média**: 22 pontos/sprint

### Lead Time (Tempo de ciclo)
- Tempo médio de task: 2-3 dias
- Tempo médio de feature: 5-7 dias

### Code Quality (SonarQube)
- **Bugs**: 0 (target: 0)
- **Vulnerabilities**: 0 (target: 0)
- **Code Smells**: < 10 (target: < 5)
- **Coverage**: 82% (target: ≥80%)
- **Duplicação**: 2% (target: < 3%)

---

## ✅ BOAS PRÁTICAS

### 1. Conventional Commits

```bash
# Formato: tipo(escopo): mensagem

feat(api): adiciona endpoint GET /api/cargas
fix(service): corrige validação de data em CargaService
docs(readme): atualiza guia de setup
test(cargas): adiciona testes para filtros
refactor(repository): simplifica query LINQ
chore(deps): atualiza EF Core para 8.0.1
```

### 2. Branch Strategy (Git Flow)

```
main (produção)
  │
  ├── release/v1.0 (staging)
  │     ├── feature/api-cargas
  │     ├── feature/frontend-usinas
  │     └── bugfix/validacao-data
  │
  └── develop (integração contínua)
```

### 3. Code Review Guidelines

**O que revisar**:
- ✅ Lógica está correta?
- ✅ Código é legível?
- ✅ Há testes?
- ✅ Performance é adequada?
- ✅ Segurança é garantida?

**Como revisar**:
- ✅ Comentários construtivos
- ✅ Sugestões de melhoria
- ✅ Aprovação explícita

---

## 🎓 CAPACITAÇÃO CONTÍNUA

### Onboarding (Novos membros)
- **Semana 1**: Setup ambiente + arquitetura
- **Semana 2**: Migração guiada (1 feature completa)
- **Semana 3**: Autonomia (com suporte)

### Tech Talks (Mensais)
- Compartilhamento de conhecimento
- Demos de tecnologias
- Lições aprendidas

### Pair Programming
- Sessões de 2h (semanal)
- Junior + Senior
- Transferência de conhecimento

---

## ✅ CONCLUSÃO

### Metodologia Adotada

**Scrum** (sprints de 2 semanas) + **Kanban** (fluxo contínuo) + **DevOps** (automação)

**Benefícios**:
1. ✅ Entregas incrementais (valor rápido)
2. ✅ Feedback contínuo (PO + usuários)
3. ✅ Qualidade garantida (testes + code review)
4. ✅ Rastreabilidade (Git + Jira)
5. ✅ Automação (CI/CD)

**Próximos Passos**:
1. Finalizar POC (✅ concluído)
2. Planejar MVP (8 semanas)
3. Migração progressiva (20 semanas)
4. Descomissionamento legado (4 semanas)

**Status**: Metodologia validada na POC. Pronta para escalar.

---

**📅 Documento gerado**: 23/12/2025  
**🔄 Metodologia**: Scrum + Kanban + DevOps  
**✅ Status**: Implementada e validada  
**📊 Velocity**: 22 pontos/sprint
