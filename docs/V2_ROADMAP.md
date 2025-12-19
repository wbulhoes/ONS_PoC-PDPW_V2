# ?? ROADMAP V2 - Backend Completo

**Branch:** `feature/backend`  
**Data Início:** 20/12/2024  
**Objetivo:** Implementar backend completo com qualidade produção

---

## ?? ESCOPO DAS MELHORIAS

### 1?? **Implementar 24 APIs Restantes**
- Total de APIs no legado: 29
- Já implementadas: 5 (Usinas, TiposUsina, Empresas, SemanasPMO, EquipesPDP)
- **Pendentes: 24 APIs**

### 2?? **Testes Unitários (xUnit)**
- Cobertura mínima: 80%
- Testes para Services
- Testes para Repositories
- Testes para Controllers

### 3?? **Linguagem Ubíqua Completa**
- Refatorar nomenclatura existente
- Alinhar com glossário do domínio PDP
- Atualizar toda documentação

### 4?? **Result<T> Pattern**
- Implementar classe `Result<T>`
- Substituir exceptions por Result em operações de negócio
- Melhorar tratamento de erros

### 5?? **Testes de Integração**
- Testes de API end-to-end
- Testes com banco InMemory
- Validação de fluxos completos

---

## ?? CRONOGRAMA PROPOSTO

### **FASE 1: Fundação (Dias 1-2)**
Preparar base para desenvolvimento escalável

- [ ] Implementar Result<T> pattern
- [ ] Refatorar nomenclatura para linguagem ubíqua
- [ ] Criar estrutura de testes (xUnit + fixtures)
- [ ] Documentar padrões de código

**Entregas:**
- ? `Common/Result.cs` implementado
- ? Entidades renomeadas (Usina ? UsinaGeradora, etc)
- ? Projeto `PDPW.UnitTests` configurado
- ? Projeto `PDPW.IntegrationTests` configurado

---

### **FASE 2: APIs Críticas (Dias 3-7)**
Implementar APIs de maior valor de negócio

#### **Grupo A: Operação Energética (Prioridade ALTA)**
- [ ] ArquivoDadger (5 endpoints) - SLICE 2
- [ ] UnidadeGeradora (8 endpoints)
- [ ] RestricaoUG (6 endpoints)
- [ ] RestricaoUS (6 endpoints)
- [ ] GerForaMerito (6 endpoints)

**Entregas:**
- ? 5 APIs + testes unitários
- ? Cobertura > 80%

---

#### **Grupo B: Cargas e Balanço (Prioridade ALTA)**
- [ ] Carga (8 endpoints)
- [ ] Intercambio (8 endpoints)
- [ ] Balanco (8 endpoints)

**Entregas:**
- ? 3 APIs + testes unitários
- ? Cobertura > 80%

---

#### **Grupo C: Consolidados (Prioridade MÉDIA)**
- [ ] DCA (8 endpoints)
- [ ] DCR (8 endpoints)
- [ ] Responsavel (6 endpoints)

**Entregas:**
- ? 3 APIs + testes unitários
- ? Cobertura > 80%

---

### **FASE 3: APIs Secundárias (Dias 8-12)**

#### **Grupo D: Térmicas e Contratos (Prioridade MÉDIA)**
- [ ] ModalidadeOpTermica (8 endpoints)
- [ ] InflexibilidadeContratada (8 endpoints)
- [ ] RampasUsinaTermica (8 endpoints)
- [ ] UsinaConversora (8 endpoints)

**Entregas:**
- ? 4 APIs + testes unitários
- ? Cobertura > 80%

---

#### **Grupo E: Paradas e Motivos (Prioridade MÉDIA)**
- [ ] ParadaUG (8 endpoints)
- [ ] MotivoRestricao (6 endpoints)

**Entregas:**
- ? 2 APIs + testes unitários
- ? Cobertura > 80%

---

#### **Grupo F: Documentos (Prioridade BAIXA)**
- [ ] Upload (6 endpoints)
- [ ] Relatorio (8 endpoints)
- [ ] Arquivo (8 endpoints)
- [ ] Diretorio (8 endpoints)

**Entregas:**
- ? 4 APIs + testes unitários
- ? Cobertura > 80%

---

#### **Grupo G: Administrativo (Prioridade BAIXA)**
- [ ] Usuario (8 endpoints)
- [ ] Observacao (8 endpoints)

**Entregas:**
- ? 2 APIs + testes unitários
- ? Cobertura > 80%

---

### **FASE 4: Testes de Integração (Dias 13-14)**
- [ ] Testes E2E para fluxos críticos
- [ ] Testes de relacionamentos entre entidades
- [ ] Testes de validações de negócio
- [ ] Testes de performance básicos

**Entregas:**
- ? 50+ testes de integração
- ? Documentação de cenários de teste

---

### **FASE 5: Finalização (Dias 15-16)**
- [ ] Code review completo
- [ ] Ajustes de nomenclatura final
- [ ] Documentação completa (Swagger + README)
- [ ] Atualização de diagramas
- [ ] Preparação para merge

**Entregas:**
- ? Código revisado
- ? Documentação atualizada
- ? Pull Request criado

---

## ?? MÉTRICAS DE QUALIDADE

### **Objetivos:**
- ? Cobertura de testes: > 80%
- ? APIs implementadas: 29/29 (100%)
- ? Endpoints totais: ~200+
- ? 0 warnings do compilador
- ? 0 code smells críticos
- ? Documentação XML completa

### **Definição de Pronto (DoD):**
Para cada API:
- [ ] Entidade com navegação completa
- [ ] Repository com métodos customizados
- [ ] Service com validações de negócio
- [ ] Controller com documentação Swagger
- [ ] DTOs (Create, Update, Response)
- [ ] Testes unitários (>80% cobertura)
- [ ] Testes de integração (cenários principais)
- [ ] Seed data (se aplicável)

---

## ??? ESTRUTURA DE TESTES

```
tests/
??? PDPW.UnitTests/
?   ??? Services/
?   ?   ??? UsinaGeradoraServiceTests.cs
?   ?   ??? UnidadeGeradoraServiceTests.cs
?   ?   ??? ...
?   ??? Repositories/
?   ?   ??? UsinaGeradoraRepositoryTests.cs
?   ?   ??? ...
?   ??? Fixtures/
?   ?   ??? TestFixture.cs
?   ??? Helpers/
?       ??? TestDataBuilder.cs
?
??? PDPW.IntegrationTests/
    ??? Controllers/
    ?   ??? UsinasGeradorasControllerTests.cs
    ?   ??? ...
    ??? Fixtures/
    ?   ??? WebApplicationFactoryFixture.cs
    ??? Scenarios/
        ??? FluxoOperacionalTests.cs
```

---

## ?? NOMENCLATURA - LINGUAGEM UBÍQUA

### **Mudanças Principais:**

| Atual | Novo (Ubíqua) | Motivo |
|-------|---------------|--------|
| `Usina` | `UsinaGeradora` | Termo técnico preciso do setor |
| `Empresa` | `AgenteSetorEletrico` | Nomenclatura ONS oficial |
| `TipoUsina` | `TipoUsinaGeradora` | Consistência com UsinaGeradora |
| `EquipePDP` | `EquipeProgramacaoDiaria` | Nome completo do domínio |
| `SemanaPMO` | `SemanaProgramaMensalOperacao` | Clareza para novos devs |

### **Padrões de Nomenclatura:**

**Controllers:**
```csharp
// Antes
UsinasController

// Depois
UsinasGeradorasController
```

**Services:**
```csharp
// Antes
IUsinaService

// Depois
IUsinaGeradoraService
```

**DTOs:**
```csharp
// Antes
UsinaDto

// Depois
UsinaGeradoraDto
```

---

## ?? RESULT<T> PATTERN

### **Implementação:**

```csharp
public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public string Error { get; }
    
    // Métodos Success, Failure, etc
}
```

### **Uso nos Services:**

```csharp
// Antes
public async Task<UsinaDto> CreateAsync(CreateUsinaDto dto)
{
    if (await CodigoExisteAsync(dto.Codigo))
        throw new InvalidOperationException("Código já existe");
    
    // ...
}

// Depois
public async Task<Result<UsinaGeradoraDto>> CreateAsync(CreateUsinaGeradoraDto dto)
{
    if (await CodigoExisteAsync(dto.Codigo))
        return Result<UsinaGeradoraDto>.Failure("Código já existe");
    
    // ...
    return Result<UsinaGeradoraDto>.Success(usinaDto);
}
```

---

## ?? PRIORIZAÇÃO DAS 24 APIs

### **Critérios:**
1. **Valor de Negócio** (impacto para ONS)
2. **Dependências Técnicas** (outras APIs precisam)
3. **Complexidade** (mais simples primeiro para ganhar ritmo)

### **Lista Priorizada:**

| # | API | Prioridade | Complexidade | Estimativa |
|---|-----|------------|--------------|------------|
| 1 | ArquivoDadger | ??? ALTA | ALTA | 1.5 dias |
| 2 | UnidadeGeradora | ??? ALTA | MÉDIA | 1 dia |
| 3 | RestricaoUG | ??? ALTA | MÉDIA | 1 dia |
| 4 | RestricaoUS | ??? ALTA | MÉDIA | 1 dia |
| 5 | GerForaMerito | ??? ALTA | MÉDIA | 1 dia |
| 6 | Carga | ??? ALTA | MÉDIA | 1 dia |
| 7 | Intercambio | ??? ALTA | MÉDIA | 1 dia |
| 8 | Balanco | ??? ALTA | ALTA | 1 dia |
| 9 | DCA | ?? MÉDIA | MÉDIA | 0.5 dia |
| 10 | DCR | ?? MÉDIA | MÉDIA | 0.5 dia |
| 11 | Responsavel | ?? MÉDIA | BAIXA | 0.5 dia |
| 12 | ModalidadeOpTermica | ?? MÉDIA | MÉDIA | 0.5 dia |
| 13 | InflexibilidadeContratada | ?? MÉDIA | MÉDIA | 0.5 dia |
| 14 | RampasUsinaTermica | ?? MÉDIA | MÉDIA | 0.5 dia |
| 15 | UsinaConversora | ?? MÉDIA | BAIXA | 0.5 dia |
| 16 | ParadaUG | ?? MÉDIA | BAIXA | 0.5 dia |
| 17 | MotivoRestricao | ?? MÉDIA | BAIXA | 0.5 dia |
| 18 | Upload | ? BAIXA | BAIXA | 0.5 dia |
| 19 | Relatorio | ? BAIXA | MÉDIA | 0.5 dia |
| 20 | Arquivo | ? BAIXA | BAIXA | 0.5 dia |
| 21 | Diretorio | ? BAIXA | BAIXA | 0.5 dia |
| 22 | Usuario | ? BAIXA | MÉDIA | 0.5 dia |
| 23 | Observacao | ? BAIXA | BAIXA | 0.5 dia |
| 24 | DadoEnergetico (refactor) | ? BAIXA | BAIXA | 0.5 dia |

**Total Estimado:** ~20 dias úteis

---

## ?? STATUS TRACKING

### **Legenda:**
- ? Não iniciado
- ?? Em progresso
- ? Concluído
- ?? Bloqueado

### **Progresso Atual:**

| Fase | Status | Progresso |
|------|--------|-----------|
| Fase 1: Fundação | ? | 0% |
| Fase 2: APIs Críticas | ? | 0% |
| Fase 3: APIs Secundárias | ? | 0% |
| Fase 4: Testes Integração | ? | 0% |
| Fase 5: Finalização | ? | 0% |

**Progresso Geral:** 0% (0/24 APIs pendentes)

---

## ?? CONVENÇÕES DE COMMIT

```bash
# Convenção: tipo(escopo): mensagem

# Exemplos:
feat(api): adiciona API de UnidadeGeradora
test(service): adiciona testes unitários para UsinaGeradoraService
refactor(domain): renomeia Usina para UsinaGeradora
fix(repository): corrige filtro de busca por código
docs(readme): atualiza documentação de APIs
```

---

## ?? LINKS ÚTEIS

- [Código Legado VB.NET](../legado/pdpw_vb/pdpw/)
- [Glossário do Domínio](../GLOSSARIO.md)
- [Documentação xUnit](https://xunit.net/)
- [Result Pattern Reference](https://enterprisecraftsmanship.com/posts/functional-c-handling-failures-input-errors/)

---

## ? CHECKLIST FINAL

Antes de fazer merge para `develop`:

- [ ] 29 APIs implementadas (100%)
- [ ] Cobertura de testes > 80%
- [ ] Nomenclatura ubíqua aplicada
- [ ] Result<T> pattern implementado
- [ ] Testes de integração funcionando
- [ ] Documentação Swagger completa
- [ ] README atualizado
- [ ] Code review aprovado
- [ ] CI/CD pipeline passando
- [ ] Performance validada

---

**Criado em:** 20/12/2024  
**Última atualização:** 20/12/2024  
**Branch:** `feature/backend`  
**Responsável:** Willian + GitHub Copilot
