# ?? CRONOGRAMA ATUALIZADO - PoC PDPW (Revisão Detalhada)

**Data de Revisão:** 19/12/2024 09:00  
**Status Docker:** ? Funcionando (Linux containers)  
**Data Início:** 19/12/2024  
**Data Entrega:** 26/12/2024  
**Dias Úteis:** 6 dias + meio período

---

## ?? VISÃO GERAL DO CRONOGRAMA

```
???????????????????????????????????????????????????????
? LINHA DO TEMPO - 6 DIAS                             ?
???????????????????????????????????????????????????????
? DIA 1 (19/12 Qui) ? 5 APIs    | Acumulado: 5        ?
? DIA 2 (20/12 Sex) ? 6 APIs    | Acumulado: 11       ?
? DIA 3 (21/12 Sáb) ? 5 APIs    | Acumulado: 16       ?
? DIA 4 (22/12 Dom) ? 6 APIs    | Acumulado: 22       ?
? DIA 5 (23/12 Seg) ? 5 APIs    | Acumulado: 27       ?
? DIA 6 (24/12 Ter) ? 2 APIs    | Acumulado: 29 ?    ?
???????????????????????????????????????????????????????
? DIA 7 (25/12 Qua) ? FERIADO (NATAL) ??             ?
? DIA 8 (26/12 Qui) ? ENTREGA + APRESENTAÇÃO ??      ?
???????????????????????????????????????????????????????
```

---

## ?? SQUAD E DISTRIBUIÇÃO

### Composição do Time

| Dev | Perfil | Foco Principal | Meta |
|-----|--------|---------------|------|
| **DEV 1** | Backend Senior | Gestão Ativos + Core | 12-14 APIs |
| **DEV 2** | Backend Pleno | Arquivos + Restrições | 12-14 APIs |
| **DEV 3** | Frontend | Tela Usinas | 1 tela completa |

### Velocidade Esperada

**Backend (cada dev):**
- APIs Simples: 1-1.5h (Enum, entidade básica)
- APIs Médias: 2-3h (CRUD completo)
- APIs Complexas: 4-5h (relacionamentos múltiplos)

**Frontend:**
- Setup + estrutura: 4h
- Componente listagem: 4h
- Componente formulário: 4h
- CRUD completo + validações: 8h
- Testes + polish: 8h

---

## ?? CRONOGRAMA DETALHADO DIA A DIA

---

### ??? DIA 1: QUINTA-FEIRA 19/12/2024

**Objetivo:** Setup completo + primeiras 5 APIs + estrutura frontend

#### ? 09:00 - 09:15 | DAILY STANDUP (15 min)

**Todos os devs juntos:**
- ? Validar que Docker está funcionando
- ? Definir branches de cada um
- ? Alinhar expectativas do dia
- ? Tirar dúvidas de setup

**Checklist pré-trabalho:**
- [ ] Git configurado
- [ ] Docker rodando
- [ ] .NET 8 instalado (backend)
- [ ] Node.js 20 instalado (frontend)
- [ ] Branches criadas

---

#### ????? DEV 1 (Backend Senior) - 09:15 às 18:00

**Branch:** `feature/gestao-ativos`

##### 09:15 - 10:00 | Setup e Estrutura Base (45 min)
```powershell
git checkout develop
git pull origin develop
git checkout -b feature/gestao-ativos
git push -u origin feature/gestao-ativos

# Decisão: Local ou Docker?
# RECOMENDADO: Local (muito mais rápido)
cd src\PDPW.API
dotnet watch run
# Hot reload! Edita código e vê mudanças na hora
```

**Tarefas:**
- [ ] Criar estrutura base de pastas
- [ ] Verificar que projeto compila
- [ ] Planejar 3 APIs do dia

---

##### 10:00 - 13:00 | ? API 1: USINA (3h) - COMPLEXA

**Prioridade:** ?? CRÍTICA (usada por várias outras APIs)

**Estrutura completa:**

1. **Domain/Entities/Usina.cs** (30 min)
```csharp
public class Usina
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public int TipoUsinaId { get; set; }
    public TipoUsina? TipoUsina { get; set; }
    public int EmpresaId { get; set; }
    public Empresa? Empresa { get; set; }
    public decimal CapacidadeInstalada { get; set; }
    public string? Localizacao { get; set; }
    public DateTime DataOperacao { get; set; }
    public bool Ativa { get; set; }
    
    // Relacionamentos
    public ICollection<UnidadeGeradora>? UnidadesGeradoras { get; set; }
    public ICollection<RestricaoUG>? Restricoes { get; set; }
}
```

2. **Domain/Interfaces/IUsinaRepository.cs** (15 min)
```csharp
public interface IUsinaRepository
{
    Task<IEnumerable<Usina>> ObterTodasAsync();
    Task<Usina?> ObterPorIdAsync(int id);
    Task<Usina?> ObterPorCodigoAsync(string codigo);
    Task<IEnumerable<Usina>> ObterPorTipoAsync(int tipoId);
    Task<IEnumerable<Usina>> ObterPorEmpresaAsync(int empresaId);
    Task<Usina> AdicionarAsync(Usina usina);
    Task AtualizarAsync(Usina usina);
    Task RemoverAsync(int id);
}
```

3. **Infrastructure/Repositories/UsinaRepository.cs** (30 min)

4. **Application/DTOs/UsinaDto.cs** (20 min)
```csharp
public class UsinaDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string TipoUsina { get; set; } = string.Empty;
    public string Empresa { get; set; } = string.Empty;
    public decimal CapacidadeInstalada { get; set; }
    public bool Ativa { get; set; }
}

public class CriarUsinaDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public int TipoUsinaId { get; set; }
    public int EmpresaId { get; set; }
    public decimal CapacidadeInstalada { get; set; }
    public string? Localizacao { get; set; }
}
```

5. **Application/Services/UsinaService.cs** (30 min)

6. **API/Controllers/UsinasController.cs** (30 min)
```csharp
[ApiController]
[Route("api/[controller]")]
[Tags("Gestão de Ativos")]
public class UsinasController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsinaDto>>> Get() { }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UsinaDto>> GetById(int id) { }
    
    [HttpGet("codigo/{codigo}")]
    public async Task<ActionResult<UsinaDto>> GetByCodigo(string codigo) { }
    
    [HttpPost]
    public async Task<ActionResult<UsinaDto>> Post([FromBody] CriarUsinaDto dto) { }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] CriarUsinaDto dto) { }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) { }
}
```

7. **Infrastructure/Data/EntityConfigurations/UsinaConfiguration.cs** (15 min)

8. **Seed Data** (15 min)
```csharp
// 10 usinas exemplo: 3 hidrelétricas, 3 térmicas, 2 eólicas, 2 solares
```

9. **Swagger Documentation** (10 min)
```csharp
/// <summary>
/// Obtém todas as usinas cadastradas
/// </summary>
/// <returns>Lista de usinas</returns>
```

**Endpoints gerados:** 6 (GET all, GET by id, GET by codigo, POST, PUT, DELETE)

**Validar:**
```powershell
# Swagger: http://localhost:5000/swagger
# Testar cada endpoint (Try it out)
```

---

##### 13:00 - 14:00 | ??? ALMOÇO

---

##### 14:00 - 16:00 | ? API 2: EMPRESA (2h) - MÉDIA

**Prioridade:** ?? ALTA

**Estrutura:**
1. Domain/Entities/Empresa.cs (20 min)
2. Domain/Interfaces/IEmpresaRepository.cs (10 min)
3. Infrastructure/Repositories/EmpresaRepository.cs (20 min)
4. Application/DTOs/EmpresaDto.cs (15 min)
5. Application/Services/EmpresaService.cs (20 min)
6. API/Controllers/EmpresasController.cs (20 min)
7. Seed Data (10 min) - 8 empresas exemplo
8. Swagger Documentation (5 min)

**Endpoints gerados:** 5 (CRUD completo + filtros)

---

##### 16:00 - 18:00 | ? API 3: TIPO USINA (2h) - SIMPLES

**Prioridade:** ?? MÉDIA (pode ser enum, mas faremos entidade)

**Estrutura:**
1. Domain/Entities/TipoUsina.cs (15 min)
2. Repository + Service + Controller (1h 30min)
3. Seed Data (10 min) - Hidrelétrica, Térmica, Eólica, Solar, Nuclear
4. Swagger (5 min)

**Endpoints gerados:** 4 (GET all, GET by id, POST, PUT)

---

##### 18:00 - 18:15 | Commit e Push (15 min)

```powershell
git add .
git commit -m "[GESTAO-ATIVOS] feat: adiciona APIs Usina, Empresa e TipoUsina

Implementações:
- API 1: Usina (6 endpoints)
  - CRUD completo
  - Busca por código
  - Filtros por tipo e empresa
  - Seed: 10 usinas

- API 2: Empresa (5 endpoints)
  - CRUD completo
  - Filtros
  - Seed: 8 empresas

- API 3: TipoUsina (4 endpoints)
  - CRUD básico
  - Seed: 5 tipos

Total: 15 endpoints
Swagger: documentado com XML comments
Testes: validados via Swagger"

git push origin feature/gestao-ativos
```

**Resultado DIA 1 - DEV 1:**
- ? 3 APIs completas
- ? 15 endpoints funcionando
- ? Swagger documentado
- ? Seed data populado

---

#### ????? DEV 2 (Backend Pleno) - 09:15 às 18:00

**Branch:** `feature/arquivos-dados`

##### 09:15 - 10:00 | Setup (45 min)

Igual ao DEV 1

---

##### 10:00 - 13:00 | ? API 11: UNIDADE GERADORA (3h) - COMPLEXA

**Prioridade:** ?? CRÍTICA

**Estrutura completa:**

```csharp
public class UnidadeGeradora
{
    public int Id { get; set; }
    public string Codigo { get; set; }
    public string Nome { get; set; }
    public int UsinaId { get; set; }
    public Usina? Usina { get; set; }
    public decimal PotenciaNominal { get; set; }
    public decimal PotenciaMinima { get; set; }
    public DateTime DataComissionamento { get; set; }
    public string? Status { get; set; }
    public bool Ativa { get; set; }
    
    // Relacionamentos
    public ICollection<ParadaUG>? Paradas { get; set; }
    public ICollection<RestricaoUG>? Restricoes { get; set; }
}
```

**Endpoints:** 7 (CRUD + filtro por usina + filtro por status)

---

##### 13:00 - 14:00 | ALMOÇO

---

##### 14:00 - 16:30 | ? API 12: PARADA UG (2.5h) - MÉDIA

**Prioridade:** ?? ALTA

```csharp
public class ParadaUG
{
    public int Id { get; set; }
    public int UnidadeGeradoraId { get; set; }
    public UnidadeGeradora? UnidadeGeradora { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public string MotivoParada { get; set; }
    public string? Observacoes { get; set; }
    public bool Programada { get; set; }
}
```

**Endpoints:** 6 (CRUD + filtro por UG + filtro por período)

---

##### 16:30 - 18:00 | ? API 13: RESTRIÇÃO UG (1.5h) - INÍCIO

**Prioridade:** ?? ALTA

**Conseguir fazer:**
- [ ] Entidade
- [ ] Interface
- [ ] Repository (parcial)

**Completar amanhã**

---

##### 18:00 - 18:15 | Commit

```powershell
git commit -m "[ARQUIVOS-DADOS] feat: adiciona APIs UnidadeGeradora e ParadaUG

- API 11: UnidadeGeradora (7 endpoints)
- API 12: ParadaUG (6 endpoints)
- API 13: RestricaoUG (início - 30%)

Total: 13 endpoints completos
Seed data: 15 UGs, 10 paradas"

git push origin feature/arquivos-dados
```

**Resultado DIA 1 - DEV 2:**
- ? 2 APIs completas
- ? 13 endpoints funcionando
- ?? 1 API em progresso (30%)

---

#### ????? DEV 3 (Frontend) - 09:15 às 18:00

**Branch:** `feature/frontend-usinas`

##### 09:15 - 10:00 | Setup Node.js (45 min)

```powershell
cd frontend

# Verificar que npm install já foi feito
npm install

# Testar dev server
npm run dev
# http://localhost:5173 deve abrir
```

**Validar:**
- [ ] Node 20 instalado
- [ ] npm install funcionou
- [ ] npm run dev funciona
- [ ] Hot reload funciona (editar App.tsx e ver mudança)

---

##### 10:00 - 12:00 | Análise da Tela Legada (2h)

**Analisar:** `pdpw_act/pdpw/frmCadUsina.aspx`

**Mapear:**
1. Campos do formulário
2. Validações existentes
3. Fluxo de navegação
4. Listagem e filtros
5. Mensagens de erro/sucesso

**Criar documento:** `docs/ANALISE_TELA_USINAS.md`

---

##### 12:00 - 13:00 | Estrutura de Componentes (1h)

```
frontend/src/
??? pages/
?   ??? Usinas/
?       ??? UsinasList.tsx       (listagem)
?       ??? UsinaForm.tsx        (formulário)
?       ??? UsinasPage.tsx       (página principal)
??? components/
?   ??? Usinas/
?       ??? UsinaCard.tsx        (card na listagem)
?       ??? UsinaFilters.tsx     (filtros)
?       ??? UsinaFormFields.tsx  (campos do form)
??? services/
?   ??? usinaService.ts          (API calls)
??? types/
    ??? usina.ts                 (TypeScript types)
```

---

##### 13:00 - 14:00 | ALMOÇO

---

##### 14:00 - 16:00 | Componente de Listagem (2h)

**Criar:** `pages/Usinas/UsinasList.tsx`

```typescript
export const UsinasList = () => {
  const [usinas, setUsinas] = useState<Usina[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    loadUsinas();
  }, []);

  const loadUsinas = async () => {
    setLoading(true);
    const data = await usinaService.getAll();
    setUsinas(data);
    setLoading(false);
  };

  return (
    <div>
      <h1>Usinas</h1>
      {loading && <Spinner />}
      <table>
        {/* Listagem */}
      </table>
    </div>
  );
};
```

**Funcionalidades:**
- [ ] Listagem de usinas
- [ ] Loading state
- [ ] Empty state (sem dados)
- [ ] Card básico para cada usina

---

##### 16:00 - 18:00 | Componente de Formulário - Estrutura (2h)

**Criar:** `pages/Usinas/UsinaForm.tsx`

```typescript
export const UsinaForm = () => {
  const [formData, setFormData] = useState<CreateUsinaDto>({
    codigo: '',
    nome: '',
    tipoUsinaId: 0,
    empresaId: 0,
    capacidadeInstalada: 0
  });

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    await usinaService.create(formData);
  };

  return (
    <form onSubmit={handleSubmit}>
      <input name="codigo" />
      <input name="nome" />
      {/* Outros campos */}
      <button type="submit">Salvar</button>
    </form>
  );
};
```

**Conseguir fazer:**
- [ ] Estrutura básica do formulário
- [ ] Todos os campos renderizados
- [ ] Submit básico (sem validação ainda)

---

##### 18:00 - 18:15 | Commit

```powershell
git commit -m "[FRONTEND] feat: estrutura inicial tela de Usinas

Implementações:
- Análise da tela legada (doc criado)
- Estrutura de componentes definida
- UsinasList: listagem básica funcionando
- UsinaForm: estrutura criada (40%)
- Services: API integration configurada

Próximo: Completar formulário e validações"

git push origin feature/frontend-usinas
```

**Resultado DIA 1 - DEV 3:**
- ? Setup completo
- ? Estrutura de componentes
- ? Listagem 80% completa
- ?? Formulário 40% completo

---

### ?? RESULTADO FINAL DIA 1

```
?????????????????????????????????????????????????
? APIs COMPLETAS: 5                             ?
? ?? DEV 1: 3 APIs (Usina, Empresa, TipoUsina) ?
? ?? DEV 2: 2 APIs (UnidadeGeradora, ParadaUG) ?
?                                               ?
? ENDPOINTS: 28 funcionando                     ?
?                                               ?
? FRONTEND: 60% estrutura completa              ?
?                                               ?
? STATUS: ? NO CRONOGRAMA                      ?
?????????????????????????????????????????????????
```

**Progresso:** 17% da PoC completa (5/29 APIs)

---

## ??? DIA 2: SEXTA-FEIRA 20/12/2024

**Objetivo:** 6 APIs + Frontend 90% completo

### Resumo Rápido

| Dev | Manhã (4h) | Tarde (4h) |
|-----|-----------|-----------|
| **DEV 1** | SemanaPMO, EquipePDP | ArquivoDadger |
| **DEV 2** | Concluir RestricaoUG, RestricaoUS | MotivoRestricao, início Intercambio |
| **DEV 3** | Integração API + validações | Filtros + mensagens + polish |

**Meta:** 11 APIs acumuladas, Frontend 90%

---

## ??? DIA 3: SÁBADO 21/12/2024

**Objetivo:** 5 APIs complexas + Frontend 100%

**Checkpoint:** Validar que temos 16 APIs (~55% completo)

---

## ??? DIA 4: DOMINGO 22/12/2024

**Objetivo:** 6 APIs + testes

**Meta:** 22 APIs acumuladas (75%)

---

## ??? DIA 5: SEGUNDA-FEIRA 23/12/2024

**Objetivo:** 5 APIs + QA intensivo

**Meta:** 27 APIs acumuladas (93%)

---

## ??? DIA 6: TERÇA-FEIRA 24/12/2024 (MEIO PERÍODO)

**Objetivo:** 2 APIs finais + Docker + preparação

**Meta:** 29 APIs completas (100%) ?

---

## ?? DIA 7: QUARTA-FEIRA 25/12/2024

**FERIADO DE NATAL** ??

**Opcional:** Revisar apresentação

---

## ?? DIA 8: QUINTA-FEIRA 26/12/2024

**ENTREGA E APRESENTAÇÃO**

### Agenda do Dia

**09:00 - 09:30** | Preparação Final
- [ ] Iniciar Docker (`docker-compose up`)
- [ ] Testar endpoints principais
- [ ] Verificar frontend

**09:30 - 10:00** | Apresentação (30 min)
- Demo ao vivo: 15 min
- Q&A: 15 min

**10:00 - 10:30** | Discussão Próximos Passos

---

## ?? MÉTRICAS E KPIs

### Velocidade Esperada por Dia

```
DIA 1: 5 APIs   ?  17% ??????????
DIA 2: 6 APIs   ?  38% ??????????
DIA 3: 5 APIs   ?  55% ??????????
DIA 4: 6 APIs   ?  76% ??????????
DIA 5: 5 APIs   ?  93% ????????????
DIA 6: 2 APIs   ? 100% ??????????????
```

### Endpoints por Categoria

```
Gestão de Ativos:        ~35 endpoints (DEV 1)
Arquivos e Dados:        ~40 endpoints (DEV 2)
Restrições e Operação:   ~45 endpoints (DEV 2)
Consolidados:            ~20 endpoints (DEV 1)
Equipes:                 ~10 endpoints (DEV 1)
Documentos:              ~15 endpoints (ambos)
????????????????????????????????????????
TOTAL:                   ~165 endpoints
```

---

## ?? RISCOS E MITIGAÇÕES

### Risco 1: Dev ficar bloqueado

**Mitigação:**
- Daily standup para identificar cedo
- Pair programming se necessário
- Devs podem trocar tarefas

### Risco 2: APIs muito complexas demorando

**Mitigação:**
- Priorizar CRUD básico primeiro
- Relacionamentos complexos podem ser simplificados
- Foco em funcional > perfeito

### Risco 3: Conflitos de merge

**Mitigação:**
- Merge diário para develop
- Comunicação constante sobre arquivos editados
- Usar branches feature separadas

### Risco 4: Frontend depender de API não pronta

**Mitigação:**
- API de Usina (prioridade) no DIA 1
- Frontend pode mockar dados se necessário
- Integração acontece gradualmente

---

## ? CHECKLIST DE VALIDAÇÃO DIÁRIA

### Fim de Cada Dia

**Backend:**
- [ ] Todas as APIs do dia commitadas
- [ ] Build sem erros (`dotnet build`)
- [ ] Swagger atualizado e testado
- [ ] Seed data funcionando
- [ ] Merge para develop feito

**Frontend:**
- [ ] Componentes do dia funcionando
- [ ] `npm run build` sem erros
- [ ] Integração com API testada
- [ ] Commit feito

**Todos:**
- [ ] Daily do próximo dia agendado
- [ ] Bloqueios comunicados
- [ ] Status atualizado no board

---

## ?? CRITÉRIOS DE SUCESSO FINAL

### Mínimo Aceitável (Dia 6)

```
? 25+ APIs backend
? 130+ endpoints
? Swagger 100% documentado
? 1 tela frontend (Usinas) completa
? Docker funcionando
? Seed data populado
? Build sem erros
```

### Meta Ideal (Dia 6)

```
? 29 APIs backend
? 160+ endpoints
? Swagger documentado com exemplos
? 1 tela frontend polished
? Docker + docker-compose.yml
? Seed data realista (100+ registros)
? Testes básicos (>60% cobertura)
? README completo
? Documentação técnica
```

---

## ?? COMUNICAÇÃO E COLABORAÇÃO

### Daily Standup (09:00 - 15 min)

**Formato:**
1. O que fiz ontem? (1 min cada)
2. O que vou fazer hoje? (1 min cada)
3. Bloqueios? (1 min cada)
4. Decisões técnicas necessárias (5 min)

### Padrão de Commits

```bash
[CATEGORIA] tipo: descrição curta

Corpo: detalhes da mudança

Exemplo:
[GESTAO-ATIVOS] feat: adiciona CRUD de Usinas

- Implementa 6 endpoints
- Adiciona validações
- Seed com 10 usinas exemplo
- Documentação Swagger completa
```

### Categorias de Commit

```
[GESTAO-ATIVOS]  - APIs: Usina, Empresa, TipoUsina
[ARQUIVOS-DADOS] - APIs: DADGER, SemanaPMO, etc.
[RESTRICOES]     - APIs: RestricaoUG, ParadaUG, etc.
[OPERACAO]       - APIs: Intercambio, Balanco, etc.
[CONSOLIDADOS]   - APIs: DCA, DCR
[EQUIPES]        - APIs: Usuario, Responsavel, EquipePDP
[DOCUMENTOS]     - APIs: Arquivo, Relatorio, Upload
[FRONTEND]       - Tela de Usinas
[DOCKER]         - Configurações Docker
[DOCS]           - Documentação
[TEST]           - Testes
```

---

## ?? COMEÇAR AGORA

### Ação Imediata (Próximos 15 minutos)

```powershell
# 1. Criar branches
cd C:\temp\_ONS_PoC-PDPW

git checkout develop
git pull origin develop

# DEV 1
git checkout -b feature/gestao-ativos
git push -u origin feature/gestao-ativos

# DEV 2
git checkout develop
git checkout -b feature/arquivos-dados
git push -u origin feature/arquivos-dados

# DEV 3
git checkout develop
git checkout -b feature/frontend-usinas
git push -u origin feature/frontend-usinas

# 2. Escolher: Docker ou Local?
# RECOMENDADO: Local (hot reload)

# Backend local:
cd src\PDPW.API
dotnet watch run

# Frontend local:
cd frontend
npm run dev

# 3. Começar primeira API!
```

---

**Cronograma criado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Versão:** 2.0 (Revisão Detalhada)  
**Status:** ? PRONTO PARA EXECUÇÃO

**BORA COMEÇAR! ??**
