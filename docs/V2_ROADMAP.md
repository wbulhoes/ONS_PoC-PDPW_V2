# ?? ROADMAP V2 - Backend Completo

**Branch:** `feature/backend`  
**Data In�cio:** 20/12/2024  
**Objetivo:** Implementar backend completo com qualidade produ��o

---

## ?? ESCOPO DAS MELHORIAS

### 1?? **Implementar 24 APIs Restantes**
- Total de APIs no legado: 29
- J� implementadas: 5 (Usinas, TiposUsina, Empresas, SemanasPMO, EquipesPDP)
- **Pendentes: 24 APIs**

### 2?? **Testes Unit�rios (xUnit)**
- Cobertura m�nima: 80%
- Testes para Services
- Testes para Repositories
- Testes para Controllers

### 3?? **Linguagem Ub�qua Completa**
- Refatorar nomenclatura existente
- Alinhar com gloss�rio do dom�nio PDP
- Atualizar toda documenta��o

### 4?? **Result<T> Pattern**
- Implementar classe `Result<T>`
- Substituir exceptions por Result em opera��es de neg�cio
- Melhorar tratamento de erros

### 5?? **Testes de Integra��o**
- Testes de API end-to-end
- Testes com banco InMemory
- Valida��o de fluxos completos

---

## ?? CRONOGRAMA PROPOSTO

### **FASE 1: Funda��o (Dias 1-2)**
Preparar base para desenvolvimento escal�vel

- [ ] Implementar Result<T> pattern
- [ ] Refatorar nomenclatura para linguagem ub�qua
- [ ] Criar estrutura de testes (xUnit + fixtures)
- [ ] Documentar padr�es de c�digo

**Entregas:**
- ? `Common/Result.cs` implementado
- ? Entidades renomeadas (Usina ? UsinaGeradora, etc)
- ? Projeto `PDPW.UnitTests` configurado
- ? Projeto `PDPW.IntegrationTests` configurado

---

### **FASE 2: APIs Cr�ticas (Dias 3-7)**
Implementar APIs de maior valor de neg�cio

#### **Grupo A: Opera��o Energ�tica (Prioridade ALTA)**
- [ ] ArquivoDadger (5 endpoints) - SLICE 2
- [ ] UnidadeGeradora (8 endpoints)
- [ ] RestricaoUG (6 endpoints)
- [ ] RestricaoUS (6 endpoints)
- [ ] GerForaMerito (6 endpoints)

**Entregas:**
- ? 5 APIs + testes unit�rios
- ? Cobertura > 80%

---

#### **Grupo B: Cargas e Balan�o (Prioridade ALTA)**
- [ ] Carga (8 endpoints)
- [ ] Intercambio (8 endpoints)
- [ ] Balanco (8 endpoints)

**Entregas:**
- ? 3 APIs + testes unit�rios
- ? Cobertura > 80%

---

#### **Grupo C: Consolidados (Prioridade M�DIA)**
- [ ] DCA (8 endpoints)
- [ ] DCR (8 endpoints)
- [ ] Responsavel (6 endpoints)

**Entregas:**
- ? 3 APIs + testes unit�rios
- ? Cobertura > 80%

---

### **FASE 3: APIs Secund�rias (Dias 8-12)**

#### **Grupo D: T�rmicas e Contratos (Prioridade M�DIA)**
- [ ] ModalidadeOpTermica (8 endpoints)
- [ ] InflexibilidadeContratada (8 endpoints)
- [ ] RampasUsinaTermica (8 endpoints)
- [ ] UsinaConversora (8 endpoints)

**Entregas:**
- ? 4 APIs + testes unit�rios
- ? Cobertura > 80%

---

#### **Grupo E: Paradas e Motivos (Prioridade M�DIA)**
- [ ] ParadaUG (8 endpoints)
- [ ] MotivoRestricao (6 endpoints)

**Entregas:**
- ? 2 APIs + testes unit�rios
- ? Cobertura > 80%

---

#### **Grupo F: Documentos (Prioridade BAIXA)**
- [ ] Upload (6 endpoints)
- [ ] Relatorio (8 endpoints)
- [ ] Arquivo (8 endpoints)
- [ ] Diretorio (8 endpoints)

**Entregas:**
- ? 4 APIs + testes unit�rios
- ? Cobertura > 80%

---

#### **Grupo G: Administrativo (Prioridade BAIXA)**
- [ ] Usuario (8 endpoints)
- [ ] Observacao (8 endpoints)

**Entregas:**
- ? 2 APIs + testes unit�rios
- ? Cobertura > 80%

---

### **FASE 4: Testes de Integra��o (Dias 13-14)**
- [ ] Testes E2E para fluxos cr�ticos
- [ ] Testes de relacionamentos entre entidades
- [ ] Testes de valida��es de neg�cio
- [ ] Testes de performance b�sicos

**Entregas:**
- ? 50+ testes de integra��o
- ? Documenta��o de cen�rios de teste

---

### **FASE 5: Finaliza��o (Dias 15-16)**
- [ ] Code review completo
- [ ] Ajustes de nomenclatura final
- [ ] Documenta��o completa (Swagger + README)
- [ ] Atualiza��o de diagramas
- [ ] Prepara��o para merge

**Entregas:**
- ? C�digo revisado
- ? Documenta��o atualizada
- ? Pull Request criado

---

## ?? M�TRICAS DE QUALIDADE

### **Objetivos:**
- ? Cobertura de testes: > 80%
- ? APIs implementadas: 29/29 (100%)
- ? Endpoints totais: ~200+
- ? 0 warnings do compilador
- ? 0 code smells cr�ticos
- ? Documenta��o XML completa

### **Defini��o de Pronto (DoD):**
Para cada API:
- [ ] Entidade com navega��o completa
- [ ] Repository com m�todos customizados
- [ ] Service com valida��es de neg�cio
- [ ] Controller com documenta��o Swagger
- [ ] DTOs (Create, Update, Response)
- [ ] Testes unit�rios (>80% cobertura)
- [ ] Testes de integra��o (cen�rios principais)
- [ ] Seed data (se aplic�vel)

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

## ?? NOMENCLATURA - LINGUAGEM UB�QUA

### **Mudan�as Principais:**

| Atual | Novo (Ub�qua) | Motivo |
|-------|---------------|--------|
| `Usina` | `UsinaGeradora` | Termo t�cnico preciso do setor |
| `Empresa` | `AgenteSetorEletrico` | Nomenclatura ONS oficial |
| `TipoUsina` | `TipoUsinaGeradora` | Consist�ncia com UsinaGeradora |
| `EquipePDP` | `EquipeProgramacaoDiaria` | Nome completo do dom�nio |
| `SemanaPMO` | `SemanaProgramaMensalOperacao` | Clareza para novos devs |

### **Padr�es de Nomenclatura:**

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

### **Implementa��o:**

```csharp
public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public string Error { get; }
    
    // M�todos Success, Failure, etc
}
```

### **Uso nos Services:**

```csharp
// Antes
public async Task<UsinaDto> CreateAsync(CreateUsinaDto dto)
{
    if (await CodigoExisteAsync(dto.Codigo))
        throw new InvalidOperationException("C�digo j� existe");
    
    // ...
}

// Depois
public async Task<Result<UsinaGeradoraDto>> CreateAsync(CreateUsinaGeradoraDto dto)
{
    if (await CodigoExisteAsync(dto.Codigo))
        return Result<UsinaGeradoraDto>.Failure("C�digo j� existe");
    
    // ...
    return Result<UsinaGeradoraDto>.Success(usinaDto);
}
```

---

## ?? PRIORIZA��O DAS 24 APIs

### **Crit�rios:**
1. **Valor de Neg�cio** (impacto para ONS)
2. **Depend�ncias T�cnicas** (outras APIs precisam)
3. **Complexidade** (mais simples primeiro para ganhar ritmo)

### **Lista Priorizada:**

| # | API | Prioridade | Complexidade | Estimativa |
|---|-----|------------|--------------|------------|
| 1 | ArquivoDadger | ??? ALTA | ALTA | 1.5 dias |
| 2 | UnidadeGeradora | ??? ALTA | M�DIA | 1 dia |
| 3 | RestricaoUG | ??? ALTA | M�DIA | 1 dia |
| 4 | RestricaoUS | ??? ALTA | M�DIA | 1 dia |
| 5 | GerForaMerito | ??? ALTA | M�DIA | 1 dia |
| 6 | Carga | ??? ALTA | M�DIA | 1 dia |
| 7 | Intercambio | ??? ALTA | M�DIA | 1 dia |
| 8 | Balanco | ??? ALTA | ALTA | 1 dia |
| 9 | DCA | ?? M�DIA | M�DIA | 0.5 dia |
| 10 | DCR | ?? M�DIA | M�DIA | 0.5 dia |
| 11 | Responsavel | ?? M�DIA | BAIXA | 0.5 dia |
| 12 | ModalidadeOpTermica | ?? M�DIA | M�DIA | 0.5 dia |
| 13 | InflexibilidadeContratada | ?? M�DIA | M�DIA | 0.5 dia |
| 14 | RampasUsinaTermica | ?? M�DIA | M�DIA | 0.5 dia |
| 15 | UsinaConversora | ?? M�DIA | BAIXA | 0.5 dia |
| 16 | ParadaUG | ?? M�DIA | BAIXA | 0.5 dia |
| 17 | MotivoRestricao | ?? M�DIA | BAIXA | 0.5 dia |
| 18 | Upload | ? BAIXA | BAIXA | 0.5 dia |
| 19 | Relatorio | ? BAIXA | M�DIA | 0.5 dia |
| 20 | Arquivo | ? BAIXA | BAIXA | 0.5 dia |
| 21 | Diretorio | ? BAIXA | BAIXA | 0.5 dia |
| 22 | Usuario | ? BAIXA | M�DIA | 0.5 dia |
| 23 | Observacao | ? BAIXA | BAIXA | 0.5 dia |
| 24 | DadoEnergetico (refactor) | ? BAIXA | BAIXA | 0.5 dia |

**Total Estimado:** ~20 dias �teis

---

## ?? STATUS TRACKING

### **Legenda:**
- ? N�o iniciado
- ?? Em progresso
- ? Conclu�do
- ?? Bloqueado

### **Progresso Atual:**

| Fase | Status | Progresso |
|------|--------|-----------|
| Fase 1: Funda��o | ? | 0% |
| Fase 2: APIs Cr�ticas | ? | 0% |
| Fase 3: APIs Secund�rias | ? | 0% |
| Fase 4: Testes Integra��o | ? | 0% |
| Fase 5: Finaliza��o | ? | 0% |

**Progresso Geral:** 0% (0/24 APIs pendentes)

---

## ?? CONVEN��ES DE COMMIT

```bash
# Conven��o: tipo(escopo): mensagem

# Exemplos:
feat(api): adiciona API de UnidadeGeradora
test(service): adiciona testes unit�rios para UsinaGeradoraService
refactor(domain): renomeia Usina para UsinaGeradora
fix(repository): corrige filtro de busca por c�digo
docs(readme): atualiza documenta��o de APIs
```

---

## ?? LINKS �TEIS

- [C�digo Legado VB.NET](../legado/pdpw_vb/pdpw/)
- [Gloss�rio do Dom�nio](../GLOSSARIO.md)
- [Documenta��o xUnit](https://xunit.net/)
- [Result Pattern Reference](https://enterprisecraftsmanship.com/posts/functional-c-handling-failures-input-errors/)

---

## ? CHECKLIST FINAL

Antes de fazer merge para `develop`:

- [ ] 29 APIs implementadas (100%)
- [ ] Cobertura de testes > 80%
- [ ] Nomenclatura ub�qua aplicada
- [ ] Result<T> pattern implementado
- [ ] Testes de integra��o funcionando
- [ ] Documenta��o Swagger completa
- [ ] README atualizado
- [ ] Code review aprovado
- [ ] CI/CD pipeline passando
- [ ] Performance validada

---

**Criado em:** 20/12/2024  
**�ltima atualiza��o:** 20/12/2024  
**Branch:** `feature/backend`  
**Respons�vel:** Willian + GitHub Copilot
