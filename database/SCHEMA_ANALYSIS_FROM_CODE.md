# ?? Análise de Schema PDPW - Mapeamento das Entidades

**Data de Análise:** 18/12/2025  
**Método:** Engenharia Reversa do Código VB.NET  
**Status:** ? Pronto para implementação da PoC

---

## ?? RESUMO EXECUTIVO

Como não foi possível restaurar o banco de dados legado (requer 350GB, disponível apenas 245GB), foi realizada **engenharia reversa** a partir do código VB.NET para identificar as entidades principais do sistema.

### Estratégia Adotada para PoC:
1. ? Mapear entidades através dos arquivos DAO (Data Access Object)
2. ? Identificar tabelas críticas para 2 vertical slices
3. ? Criar entidades no EF Core manualmente
4. ? Usar InMemory Database com dados seed para desenvolvimento
5. ?? Integrar com banco real após PoC (se necessário)

---

## ?? ENTIDADES IDENTIFICADAS

### 1. **Usina** (Tabela: `usina`)
**Arquivo:** `UsinaDAO.vb`  
**Prioridade:** ??? CRÍTICA

```sql
-- Estrutura identificada
SELECT 
    Trim(codusina) as CodUsina,         -- PK, VARCHAR
    nomusina,                            -- VARCHAR
    codempre,                            -- VARCHAR (FK para Empresa)
    potinstalada,                        -- INT
    usi_bdt_id,                          -- VARCHAR
    dpp_id,                              -- DOUBLE
    sigsme,                              -- VARCHAR
    tpusina_id                           -- VARCHAR (Tipo: UTE, UHE, etc.)
FROM usina
```

**Relacionamentos:**
- `codempre` ? Empresa (tabela não detalhada no código disponível)
- Usada em: ArquivoDadgerValor, Inflexibilidade, Intercambio

---

### 2. **ArquivoDadger** (Tabela: `tb_arquivodadger`)
**Arquivo:** `ArquivoDadgerValorDAO.vb`  
**Prioridade:** ??? CRÍTICA

```sql
-- Estrutura identificada
SELECT
    id_arquivodadger,                    -- PK, INT/BIGINT
    id_semanapmo,                        -- FK, INT (SemanaPMO)
    id_anomes,                           -- INT (AnoMes)
    [outros campos não detalhados no trecho analisado]
FROM tb_arquivodadger
```

**Relacionamentos:**
- `id_semanapmo` ? SemanaPMO
- Pai de ArquivoDadgerValor (1:N)

---

### 3. **ArquivoDadgerValor** (Tabela: `tb_arquivodadgervalor`)
**Arquivo:** `ArquivoDadgerValorDAO.vb`  
**Prioridade:** ??? CRÍTICA

```sql
-- Estrutura completa identificada
SELECT 
    id_arquivodadgervalor,               -- PK, INT/BIGINT
    id_arquivodadger,                    -- FK, INT (ArquivoDadger)
    dpp_id,                              -- DOUBLE (Identificador DPP)
    val_cvu,                             -- DECIMAL
    val_inflexileve,                     -- DECIMAL
    val_infleximedia,                    -- DECIMAL
    val_inflexipesada,                   -- DECIMAL
    val_inflexipmo                       -- INT
FROM tb_arquivodadgervalor
```

**Relacionamentos:**
- `id_arquivodadger` ? ArquivoDadger (N:1)
- `dpp_id` ? Usina.dpp_id (através de JOIN)

**Observações:**
- Armazena valores de inflexibilidade (leve, média, pesada)
- CVU = Custo Variável Unitário
- Relaciona-se com Usinas através do campo `dpp_id`

---

### 4. **SemanaPMO** (Tabela: não identificada, mas referenciada)
**Arquivo:** Referenciada em múltiplos DAOs  
**Prioridade:** ?? IMPORTANTE

```sql
-- Estrutura inferida
SELECT
    id_semanapmo,                        -- PK
    id_anomes,                           -- INT
    [campos de data não identificados]
FROM [tabela_semanapmo]
```

**Observações:**
- PMO = Programa Mensal de Operação
- Relaciona semanas operacionais com arquivos
- Problema conhecido: semanas que cruzam meses (comentado no código)

---

### 5. **Inflexibilidade** (Tabela: inferida de `InflexibilidadeDao.vb`)
**Arquivo:** `InflexibilidadeDao.vb`  
**Prioridade:** ?? IMPORTANTE

**Relacionamentos:**
- Relacionada com Usina
- Relacionada com ArquivoDadgerValor

---

### 6. **Intercambio** (Tabela: inferida de `InterDAO.vb`)
**Arquivo:** `InterDAO.vb`  
**Prioridade:** ?? IMPORTANTE

**Observações:**
- Intercâmbio de energia entre regiões/usinas

---

### 7. **OfertaExportacao** (Tabela: inferida de `OfertaExportacaoDao.vb`)
**Arquivo:** `OfertaExportacaoDao.vb`  
**Prioridade:** ? MÉDIA

---

### 8. **Carga** (Tabela: inferida de `CargaDAO.vb`)
**Arquivo:** `CargaDAO.vb`  
**Prioridade:** ? MÉDIA

---

### 9. **LimiteEnvio** (Tabela: inferida de `LimiteEnvioDAO.vb`)
**Arquivo:** `LimiteEnvioDAO.vb`  
**Prioridade:** ? MÉDIA

---

### 10. **UsinaConversora** (Tabela: inferida de `UsinaConversoraDao.vb`)
**Arquivo:** `UsinaConversoraDao.vb`  
**Prioridade:** ? BAIXA (PoC)

---

## ?? VERTICAL SLICES RECOMENDADOS PARA POC

Com base na análise do código e prioridade de negócio, recomendo os seguintes 2 vertical slices:

---

### **SLICE 1: Cadastro de Usinas** ???
**Complexidade:** MÉDIA  
**Valor de Negócio:** ALTO  
**Tempo Estimado:** 2 dias

#### Escopo:
- **Backend:**
  - Entidade `Usina` completa
  - CRUD completo (Create, Read, Update, Delete)
  - Filtros: por código, por empresa, listar todas
  - Validações de negócio

- **Frontend:**
  - Tela de listagem de usinas (grid/tabela)
  - Formulário de cadastro/edição
  - Busca por código ou nome
  - Filtro por tipo de usina (UTE, UHE, etc.)

#### Tabelas Necessárias:
```
? usina (PRINCIPAL)
```

#### Arquivos Legado para Referência:
```
?? pdpw_act/pdpw/Business/UsinaBusiness.vb
?? pdpw_act/pdpw/Dao/UsinaDAO.vb
?? pdpw_act/pdpw/[buscar].aspx (telas WebForms relacionadas)
```

---

### **SLICE 2: Consulta de Arquivos DADGER** ???
**Complexidade:** ALTA  
**Valor de Negócio:** ALTO  
**Tempo Estimado:** 3 dias

#### Escopo:
- **Backend:**
  - Entidade `ArquivoDadger`
  - Entidade `ArquivoDadgerValor`
  - Entidade `SemanaPMO` (simplificada)
  - Consulta por semana PMO
  - Consulta por usina
  - Listagem de valores (CVU, inflexibilidades)

- **Frontend:**
  - Tela de consulta de arquivos
  - Filtros: por data/período, por usina
  - Exibição de valores de inflexibilidade
  - Grid com dados tabulares

#### Tabelas Necessárias:
```
? tb_arquivodadger
? tb_arquivodadgervalor
? usina (relacionamento)
?? semanapmo (simplificada)
```

#### Arquivos Legado para Referência:
```
?? pdpw_act/pdpw/Business/ArquivoDadgerValorBusiness.vb
?? pdpw_act/pdpw/Dao/ArquivoDadgerValorDAO.vb
?? pdpw_act/pdpw/frmCnsArquivo.aspx
```

---

## ?? DIAGRAMA DE RELACIONAMENTOS (Simplificado)

```
???????????????????
?  SemanaPMO      ?
?  (Período)      ?
???????????????????
         ? 1
         ?
         ? N
??????????????????????
?  ArquivoDadger     ?
?  (Arquivo do PMO)  ?
??????????????????????
         ? 1
         ?
         ? N
?????????????????????????????         ???????????????
?  ArquivoDadgerValor       ???????????   Usina     ?
?  (Valores por Usina)      ?  dpp_id ?  (Central)  ?
?                           ?         ???????????????
?  - val_cvu                ?
?  - val_inflexileve        ?
?  - val_infleximedia       ?
?  - val_inflexipesada      ?
?  - val_inflexipmo         ?
?????????????????????????????
```

---

## ?? PRÓXIMOS PASSOS TÉCNICOS

### 1. Criar Entidades no Domain (C#)

```csharp
// src/PDPW.Domain/Entities/Usina.cs
public class Usina : BaseEntity
{
    public string CodUsina { get; set; }           // PK
    public string NomUsina { get; set; }
    public string? CodEmpre { get; set; }
    public int? PotInstalada { get; set; }
    public string? UsiBdtId { get; set; }
    public double? DppId { get; set; }
    public string? Sigsme { get; set; }
    public string? TpUsinaId { get; set; }
    
    // Navigation properties
    public ICollection<ArquivoDadgerValor> ArquivoDadgerValores { get; set; }
}
```

```csharp
// src/PDPW.Domain/Entities/ArquivoDadger.cs
public class ArquivoDadger : BaseEntity
{
    public int IdSemanaPmo { get; set; }
    public int? IdAnoMes { get; set; }
    
    // Navigation properties
    public ICollection<ArquivoDadgerValor> Valores { get; set; }
}
```

```csharp
// src/PDPW.Domain/Entities/ArquivoDadgerValor.cs
public class ArquivoDadgerValor : BaseEntity
{
    public int IdArquivoDadger { get; set; }
    public double DppId { get; set; }
    public string? CodUsina { get; set; }
    public decimal ValorCvu { get; set; }
    public decimal ValorInflexiLeve { get; set; }
    public decimal ValorInflexiMedia { get; set; }
    public decimal ValorInflexiPesada { get; set; }
    public int ValorInflexiPmo { get; set; }
    
    // Navigation properties
    public ArquivoDadger ArquivoDadger { get; set; }
    public Usina? Usina { get; set; }
}
```

### 2. Configurar DbContext

```csharp
// src/PDPW.Infrastructure/Data/PdpwDbContext.cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Usina
    modelBuilder.Entity<Usina>(entity =>
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.CodUsina).HasMaxLength(50).IsRequired();
        entity.Property(e => e.NomUsina).HasMaxLength(200).IsRequired();
        entity.HasIndex(e => e.CodUsina).IsUnique();
    });
    
    // ArquivoDadger
    modelBuilder.Entity<ArquivoDadger>(entity =>
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.IdSemanaPmo).IsRequired();
        entity.HasMany(e => e.Valores)
              .WithOne(v => v.ArquivoDadger)
              .HasForeignKey(v => v.IdArquivoDadger);
    });
    
    // ArquivoDadgerValor
    modelBuilder.Entity<ArquivoDadgerValor>(entity =>
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.ValorCvu).HasPrecision(18, 2);
        entity.Property(e => e.ValorInflexiLeve).HasPrecision(18, 2);
        entity.Property(e => e.ValorInflexiMedia).HasPrecision(18, 2);
        entity.Property(e => e.ValorInflexiPesada).HasPrecision(18, 2);
    });
}
```

### 3. Criar Dados Seed (InMemory)

```csharp
// src/PDPW.Infrastructure/Data/DataSeeder.cs
public static class DataSeeder
{
    public static void SeedData(PdpwDbContext context)
    {
        if (context.Usinas.Any()) return; // Já populado
        
        // Usinas de exemplo
        var usinas = new List<Usina>
        {
            new Usina 
            { 
                CodUsina = "UTE001", 
                NomUsina = "Usina Termelétrica Exemplo 1",
                TpUsinaId = "UTE",
                PotInstalada = 1000,
                DppId = 1.0
            },
            new Usina 
            { 
                CodUsina = "UHE001", 
                NomUsina = "Usina Hidrelétrica Exemplo 1",
                TpUsinaId = "UHE",
                PotInstalada = 2500,
                DppId = 2.0
            }
        };
        
        context.Usinas.AddRange(usinas);
        context.SaveChanges();
    }
}
```

---

## ? CHECKLIST DE IMPLEMENTAÇÃO

### Slice 1: Cadastro de Usinas
- [ ] Criar entidade `Usina.cs` no Domain
- [ ] Criar interface `IUsinaRepository` no Domain
- [ ] Implementar `UsinaRepository` na Infrastructure
- [ ] Criar DTOs (UsinaDto, CriarUsinaDto, AtualizarUsinaDto)
- [ ] Criar interface `IUsinaService` na Application
- [ ] Implementar `UsinaService` na Application
- [ ] Criar `UsinasController` na API
- [ ] Testar endpoints no Swagger
- [ ] Criar componente React de listagem
- [ ] Criar formulário React de cadastro/edição
- [ ] Integrar frontend com backend

### Slice 2: Consulta de Arquivos DADGER
- [ ] Criar entidade `ArquivoDadger.cs`
- [ ] Criar entidade `ArquivoDadgerValor.cs`
- [ ] Criar entidade `SemanaPMO.cs` (simplificada)
- [ ] Configurar relacionamentos no DbContext
- [ ] Criar repositórios
- [ ] Criar serviços
- [ ] Criar controllers
- [ ] Popular dados seed
- [ ] Criar tela React de consulta
- [ ] Criar filtros e busca
- [ ] Exibir dados tabulares

---

## ?? ESTIMATIVA DE ESFORÇO

| Atividade | Tempo | Responsável |
|-----------|-------|-------------|
| **Slice 1: Usinas** | | |
| Backend (Entidades, Repos, Services, API) | 6h | Dev Backend |
| Frontend (Componentes React) | 6h | Dev Frontend |
| Testes e Integração | 2h | QA |
| **Subtotal Slice 1** | **14h (~2 dias)** | |
| | | |
| **Slice 2: Arquivos DADGER** | | |
| Backend (Entidades, Repos, Services, API) | 10h | Dev Backend |
| Frontend (Componentes React) | 8h | Dev Frontend |
| Testes e Integração | 4h | QA |
| **Subtotal Slice 2** | **22h (~3 dias)** | |
| | | |
| **Refinamentos e Documentação** | 8h | Todos |
| **TOTAL** | **44h (~5-6 dias)** | |

---

## ?? STATUS ATUAL

? **Análise de Schema CONCLUÍDA**  
? **Vertical Slices DEFINIDOS**  
? **Entidades MAPEADAS**  
?? **Próximo:** Iniciar implementação do Slice 1 (Usinas)

---

## ?? OBSERVAÇÕES IMPORTANTES

1. **Problema de Espaço em Disco:**
   - Banco legado = 350GB, disponível = 245GB
   - Solução: Usar InMemory Database para PoC
   - Alternativa: Restaurar em servidor remoto com mais espaço

2. **Semana PMO:**
   - Há bug conhecido no legado: semanas que cruzam meses
   - Para PoC, simplificar lógica de SemanaPMO

3. **Nomenclatura:**
   - Manter nomes próximos ao legado para facilitar comparação
   - CVU = Custo Variável Unitário
   - PMO = Programa Mensal de Operação
   - DADGER = Arquivo de dados gerais

4. **Dados Seed:**
   - Criar dados realistas mas simplificados
   - Focar em cenários de teste principais

---

**Documento gerado em:** 18/12/2025  
**Próxima revisão:** Após implementação dos Slices  
**Status:** ? APROVADO PARA IMPLEMENTAÇÃO
