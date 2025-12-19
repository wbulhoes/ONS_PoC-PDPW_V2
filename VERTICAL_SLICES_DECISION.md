# ?? DECISÃO: Vertical Slices Selecionados para PoC PDPW

**Data:** 18/12/2025  
**Status:** ? APROVADO  
**Prazo:** 26/12/2025

---

## ?? SITUAÇÃO ATUAL

### Problema: Banco de Dados
- ? Backup disponível: 43GB compactado (~350GB descompactado)
- ? Espaço disponível: 245GB
- ? Déficit: ~105GB
- ? Impossível restaurar banco legado no ambiente atual

### Solução Adotada
? **Engenharia Reversa do Código VB.NET**  
? **Mapeamento manual das entidades através dos DAOs**  
? **Uso de InMemory Database para PoC**  
? **Criação de dados seed realistas**

---

## ?? VERTICAL SLICES SELECIONADOS

### **SLICE 1: Cadastro de Usinas** ???

**Por quê escolhemos:**
- ? Entidade central do sistema
- ? CRUD completo demonstra arquitetura
- ? Complexidade MÉDIA (ideal para PoC)
- ? Código legado claro e bem documentado (`UsinaDAO.vb`)
- ? Representa bem o padrão de migração

**O que será entregue:**

**Backend:**
- Entidade `Usina` completa com todos os campos
- Repositório com CRUD completo
- Serviço com validações de negócio
- API REST com endpoints:
  - `GET /api/usinas` - Listar todas
  - `GET /api/usinas/{id}` - Buscar por ID
  - `GET /api/usinas/codigo/{codigo}` - Buscar por código
  - `POST /api/usinas` - Criar nova usina
  - `PUT /api/usinas/{id}` - Atualizar usina
  - `DELETE /api/usinas/{id}` - Remover usina (soft delete)

**Frontend:**
- Tela de listagem de usinas (grid/tabela)
- Formulário de cadastro/edição
- Busca e filtros (por código, nome, tipo)
- Validação de formulários
- Mensagens de sucesso/erro

**Tempo Estimado:** 2 dias (14h)

---

### **SLICE 2: Consulta de Arquivos DADGER** ???

**Por quê escolhemos:**
- ? Funcionalidade core do PDPW
- ? Demonstra relacionamentos entre entidades
- ? Representa integração com PMO (Programa Mensal de Operação)
- ? Complexidade ALTA (mostra capacidade técnica)
- ? Valor de negócio ALTO para ONS

**O que será entregue:**

**Backend:**
- Entidades: `ArquivoDadger`, `ArquivoDadgerValor`, `SemanaPMO`
- Repositórios com consultas complexas
- Serviços com lógica de filtros
- API REST com endpoints:
  - `GET /api/arquivosdadger` - Listar todos
  - `GET /api/arquivosdadger/{id}` - Buscar por ID
  - `GET /api/arquivosdadger/semana/{idSemana}` - Por semana PMO
  - `GET /api/arquivosdadger/usina/{codUsina}` - Por usina
  - `GET /api/arquivosdadger/{id}/valores` - Valores do arquivo

**Frontend:**
- Tela de consulta de arquivos
- Filtros: por período, por usina, por semana PMO
- Grid com valores de inflexibilidade
- Detalhamento de valores (CVU, inflexibilidades)
- Exportação para CSV (opcional)

**Tempo Estimado:** 3 dias (22h)

---

## ?? ENTIDADES MAPEADAS

### 1. **Usina** (SLICE 1)
```csharp
public class Usina : BaseEntity
{
    public string CodUsina { get; set; }           // Código da usina (PK)
    public string NomUsina { get; set; }           // Nome
    public string? CodEmpre { get; set; }          // Código da empresa
    public int? PotInstalada { get; set; }         // Potência instalada (MW)
    public string? UsiBdtId { get; set; }          // ID no BDT
    public double? DppId { get; set; }             // ID no DPP
    public string? Sigsme { get; set; }            // Sigla SME
    public string? TpUsinaId { get; set; }         // Tipo: UTE, UHE, EOL, etc.
}
```

**Fonte:** `pdpw_act/pdpw/Dao/UsinaDAO.vb`

---

### 2. **ArquivoDadger** (SLICE 2)
```csharp
public class ArquivoDadger : BaseEntity
{
    public int IdSemanaPmo { get; set; }           // FK para SemanaPMO
    public int? IdAnoMes { get; set; }             // Ano/Mês
    public DateTime DataImportacao { get; set; }   // Data de importação
    
    // Navigation
    public SemanaPMO SemanaPmo { get; set; }
    public ICollection<ArquivoDadgerValor> Valores { get; set; }
}
```

**Fonte:** `pdpw_act/pdpw/Dao/ArquivoDadgerValorDAO.vb`

---

### 3. **ArquivoDadgerValor** (SLICE 2)
```csharp
public class ArquivoDadgerValor : BaseEntity
{
    public int IdArquivoDadger { get; set; }       // FK para ArquivoDadger
    public double DppId { get; set; }              // ID da usina no DPP
    public string? CodUsina { get; set; }          // Código da usina
    public decimal ValorCvu { get; set; }          // Custo Variável Unitário
    public decimal ValorInflexiLeve { get; set; }  // Inflexibilidade Leve
    public decimal ValorInflexiMedia { get; set; } // Inflexibilidade Média
    public decimal ValorInflexiPesada { get; set; }// Inflexibilidade Pesada
    public int ValorInflexiPmo { get; set; }       // Limite PMO
    
    // Navigation
    public ArquivoDadger ArquivoDadger { get; set; }
    public Usina Usina { get; set; }
}
```

**Fonte:** `pdpw_act/pdpw/Dao/ArquivoDadgerValorDAO.vb`

---

### 4. **SemanaPMO** (SLICE 2 - Simplificada)
```csharp
public class SemanaPMO : BaseEntity
{
    public int IdSemanapmo { get; set; }           // PK
    public int IdAnoMes { get; set; }              // Ano/Mês
    public DateTime DataInicio { get; set; }       // Início da semana
    public DateTime DataFim { get; set; }          // Fim da semana
    public int NumeroSemana { get; set; }          // Número da semana
    
    // Navigation
    public ICollection<ArquivoDadger> Arquivos { get; set; }
}
```

**Fonte:** Referenciada em múltiplos DAOs

---

## ?? RELACIONAMENTOS

```
SemanaPMO (1) ??? (N) ArquivoDadger
ArquivoDadger (1) ??? (N) ArquivoDadgerValor
Usina (1) ??? (N) ArquivoDadgerValor
```

---

## ?? CRONOGRAMA DE IMPLEMENTAÇÃO

| Data | Atividade | Responsável | Status |
|------|-----------|-------------|--------|
| **19/12 (Quinta)** | | | |
| Manhã | Criar entidades Domain (Usina) | Backend | ? |
| Tarde | Criar repositórios e services (Usina) | Backend | ? |
| **20/12 (Sexta)** | | | |
| Manhã | Criar API e testes (Usina) | Backend | ? |
| Tarde | Criar componentes React (Usina) | Frontend | ? |
| **21/12 (Sábado)** | | | |
| Manhã | Integração frontend/backend (Usina) | Todos | ? |
| Tarde | Iniciar Slice 2: Entidades DADGER | Backend | ? |
| **22/12 (Domingo)** | | | |
| Manhã | Criar repositórios e services (DADGER) | Backend | ? |
| Tarde | Criar API (DADGER) | Backend | ? |
| **23/12 (Segunda)** | | | |
| Manhã | Criar componentes React (DADGER) | Frontend | ? |
| Tarde | Integração frontend/backend (DADGER) | Todos | ? |
| **24/12 (Terça)** | | | |
| Manhã | Testes e correções | QA + Todos | ? |
| Tarde | Docker Compose e containerização | DevOps | ? |
| **25/12 (Quarta)** | | | |
| - | **FERIADO** - Folga | - | ?? |
| **26/12 (Quinta)** | | | |
| Manhã | Documentação final | Todos | ? |
| Tarde | Preparar apresentação e commit | Todos | ? |

---

## ? CRITÉRIOS DE ACEITE

### SLICE 1: Cadastro de Usinas
- [ ] API REST com 6 endpoints funcionando
- [ ] Swagger documentado
- [ ] Frontend com listagem e formulário
- [ ] Validações de campo
- [ ] Mensagens de erro tratadas
- [ ] Dados seed populados
- [ ] Testes básicos passando

### SLICE 2: Consulta de Arquivos DADGER
- [ ] API REST com 5 endpoints funcionando
- [ ] Relacionamentos funcionando (JOIN entre tabelas)
- [ ] Frontend com filtros dinâmicos
- [ ] Grid com dados tabulares
- [ ] Consulta por período funcionando
- [ ] Dados seed com relacionamentos
- [ ] Testes de integração passando

### GERAL
- [ ] Docker Compose executando
- [ ] README atualizado
- [ ] Documentação técnica completa
- [ ] Apresentação preparada
- [ ] Código no GitHub (branch main)

---

## ?? WIREFRAMES (Referência)

### Tela 1: Listagem de Usinas
```
??????????????????????????????????????????????????????
?  PDPW - Cadastro de Usinas                  [+]    ?
??????????????????????????????????????????????????????
?  Buscar: [________________] [??]   Tipo: [Todos?] ?
??????????????????????????????????????????????????????
?  Código ? Nome                ? Tipo ? Potência   ?
????????????????????????????????????????????????????
? UTE001  ? Térmica Exemplo 1  ? UTE  ? 1000 MW   ?
? UHE001  ? Hidro Exemplo 1    ? UHE  ? 2500 MW   ?
? EOL001  ? Eólica Exemplo 1   ? EOL  ? 500 MW    ?
??????????????????????????????????????????????????????
```

### Tela 2: Consulta Arquivos DADGER
```
??????????????????????????????????????????????????????
?  PDPW - Consulta Arquivos DADGER                   ?
??????????????????????????????????????????????????????
?  Semana PMO: [Selecione?]   Usina: [Todas?]      ?
?  [Filtrar]                                         ?
??????????????????????????????????????????????????????
?  Arquivo ? Usina ? CVU  ? Ifx Leve ? Ifx Média   ?
????????????????????????????????????????????????????
? ARQ001   ?UTE001 ?125.50?   100.00 ?    150.00   ?
? ARQ001   ?UHE001 ? 85.00?    80.00 ?    120.00   ?
??????????????????????????????????????????????????????
```

---

## ?? OBSERVAÇÕES FINAIS

1. **InMemory Database:** Usaremos EF Core InMemory para desenvolvimento
   - Vantagem: Setup instantâneo, sem SQL Server
   - Desvantagem: Dados voláteis (resetam ao reiniciar)
   - Solução: Seed data automático na inicialização

2. **Dados Seed:** Criar dados realistas mas simples
   - 5-10 usinas de exemplo
   - 2-3 semanas PMO
   - 10-20 registros de ArquivoDadgerValor

3. **Fidelidade ao Legado:**
   - Manter nomenclatura próxima ao VB.NET
   - Replicar lógica de negócio essencial
   - UI modernizada mas funcionalidades iguais

4. **Documentação:**
   - Comentar código com referências ao legado
   - Criar mapeamento VB.NET ? C#
   - Documentar decisões arquiteturais

---

## ?? PRÓXIMO PASSO IMEDIATO

**INICIAR IMPLEMENTAÇÃO DO SLICE 1: CADASTRO DE USINAS**

1. Criar arquivo: `src/PDPW.Domain/Entities/Usina.cs`
2. Criar arquivo: `src/PDPW.Domain/Interfaces/IUsinaRepository.cs`
3. Atualizar: `src/PDPW.Infrastructure/Data/PdpwDbContext.cs`

---

**Decisão aprovada em:** 18/12/2025  
**Início da implementação:** 19/12/2025 09:00  
**Status:** ? **PRONTO PARA DESENVOLVER!**
