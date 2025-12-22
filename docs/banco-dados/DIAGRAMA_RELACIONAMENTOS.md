# 🗄️ DIAGRAMA DE RELACIONAMENTOS - BANCO DE DADOS PDPw

**Data**: 22/12/2025  
**Banco**: PDPW_DB  
**Total de Tabelas**: 31  
**Total de FKs**: 20

---

## 📊 MAPA DE RELACIONAMENTOS PRINCIPAIS

### **1. CORE - Cadastros Base**

```
┌────────────────┐
│   TiposUsina   │
│  (5 registros) │
└────┬───────────┘
     │
     │ TipoUsinaId
     │
┌────▼───────────┐        ┌──────────────┐
│    Empresas    │        │  EquipesPDP  │
│ (30 registros) │        │(11 registros)│
└────┬───────────┘        └──────┬───────┘
     │                            │
     │ EmpresaId                  │ EquipePDPId
     │                            │
┌────▼────────────┐        ┌─────▼────────┐
│     Usinas      │        │   Usuarios   │
│  (50 registros) │        │ (15 registros│
└────┬────────────┘        └──────────────┘
     │
     ├─────────────────────┐
     │                     │
     │ UsinaId             │ UsinaId
     │                     │
┌────▼─────────────┐  ┌───▼──────────────┐
│UnidadesGeradoras │  │  RestricoesUS    │
│ (100 registros)  │  │  (6 registros)   │
└────┬─────────────┘  └──────────────────┘
     │
     ├────────────────────┐
     │                    │
     │ UnidadeGeradoraId  │ UnidadeGeradoraId
     │                    │
┌────▼────────┐      ┌───▼──────────┐
│  ParadasUG  │      │RestricoesUG  │
│(50 registros│      │(8 registros) │
└─────────────┘      └──────┬───────┘
                            │
                            │ MotivoRestricaoId
                            │
                     ┌──────▼──────────┐
                     │MotivosRestricao │
                     │  (10 registros) │
                     └─────────────────┘
```

---

### **2. OPERAÇÃO - Dados Operativos**

```
┌────────────────┐
│  SemanasPMO    │
│ (25 semanas)   │
└────┬───────────┘
     │
     ├──────────────┬────────────────┐
     │              │                │
     │SemanaPMOId   │SemanaPMOId     │SemanaPMOId
     │              │                │
┌────▼───────┐ ┌───▼───────┐   ┌────▼──────┐
│ArquivosDadger│ │    DCAs   │   │   DCRs    │
│(10 arquivos) │ │(10 regist)│   │(8 registros│
└────┬────────┘ └───────────┘   └───┬───────┘
     │                               │
     │ArquivoDadgerId                │DCAId
     │                               │
┌────▼──────────────┐           ┌───▼───────┐
│ArquivosDadgerVal │           │   DCRs    │
│ (50 valores)     │           │(ref a DCA)│
└──────────────────┘           └───────────┘
```

---

### **3. SISTEMA - Balanços e Intercâmbios**

```
┌─────────────────────────────────────────┐
│     Subsistemas (SE, S, NE, N)          │
└─────┬───────────────────┬───────────────┘
      │                   │
      │DataRef+Sub        │DataRef+Sub
      │                   │
┌─────▼──────┐      ┌─────▼──────────────┐
│   Cargas   │      │   Intercambios     │
│(120 regist)│      │   (240 registros)  │
└────────────┘      └────────────────────┘
                            │
                            │
                     ┌──────▼─────────┐
                     │    Balancos    │
                     │  (120 registros│
                     └────────────────┘
```

---

### **4. CONFIGURAÇÕES - Térmicas**

```
┌────────────────┐
│     Usinas     │
│   (UTE only)   │
└────┬───────────┘
     │
     ├──────────────┬────────────────┬─────────────────┐
     │              │                │                 │
     │UsinaId       │UsinaId         │UsinaId          │UsinaId
     │              │                │                 │
┌────▼──────┐ ┌────▼────┐  ┌────────▼─────┐  ┌───────▼────────┐
│GerForaMerito│ │Inflex │  │RampasUsinaTerm│  │UsinaConversora │
│(10 registros│ │Contrat│  │ (5 registros) │  │ (3 registros) │
└─────────────┘ │(8 reg)│  └───────────────┘  └────────────────┘
                └───────┘
```

---

### **5. SISTEMA - Arquivos e Documentos**

```
┌──────────────┐
│  Diretorios  │
│ (8 registros)│
└────┬─────────┘
     │
     ├──────────────────┐
     │                  │
     │DiretorioId       │DiretorioPaiId (auto-ref)
     │                  │
┌────▼────────┐    ┌────▼─────────┐
│  Arquivos   │    │ Subdiretorios│
│(15 registros│    │ (hierárquico)│
└─────────────┘    └──────────────┘

┌──────────────┐    ┌───────────────┐
│   Uploads    │    │  Relatorios   │
│(5 arquivos)  │    │ (5 relatórios)│
└──────────────┘    └───────────────┘

┌──────────────┐    ┌───────────────┐
│ Responsaveis │    │  Observacoes  │
│(10 pessoas)  │    │ (10 registros)│
└──────────────┘    └───────────────┘
```

---

## 🔗 ANÁLISE DE FOREIGN KEYS

### **Por Tipo de Relacionamento**

| Tipo | Quantidade | Exemplos |
|------|------------|----------|
| **CASCADE** | 10 | UnidadesGeradoras → Usinas, ParadasUG → UnidadesGeradoras |
| **NO ACTION** | 8 | Usinas → Empresas, Usinas → TiposUsina |
| **SET NULL** | 2 | DCRs → DCAs, Usuarios → EquipesPDP |

### **Tabelas com Mais Relacionamentos**

| Tabela | Total FKs | Relacionamentos |
|--------|-----------|-----------------|
| **Usinas** | 8 FKs | → TiposUsina, → Empresas, ← UnidadesGeradoras, ← RestricoesUS, ← GeracoesForaMerito, ← InflexibilidadesContratadas, ← RampasUsinasTermicas, ← UsinasConversoras |
| **UnidadesGeradoras** | 3 FKs | → Usinas, ← ParadasUG, ← RestricoesUG |
| **SemanasPMO** | 3 FKs | ← ArquivosDadger, ← DCAs, ← DCRs |
| **MotivosRestricao** | 2 FKs | ← RestricoesUG, ← RestricoesUS |

---

## 📈 ÍNDICES OTIMIZADOS

### **Índices Compostos (Performance)**

| Tabela | Índice | Colunas | Uso |
|--------|--------|---------|-----|
| **Cargas** | IX_Cargas_DataReferencia_SubsistemaId | DataReferencia, SubsistemaId | Consultas por período e subsistema |
| **Balancos** | IX_Balancos_DataReferencia_SubsistemaId | DataReferencia, SubsistemaId | Consultas por período e subsistema |
| **Intercambios** | IX_Intercambios_DataReferencia_SubsistemaOrigem_SubsistemaDestino | DataReferencia, SubsistemaOrigem, SubsistemaDestino | Consultas complexas de intercâmbio |
| **SemanasPMO** | IX_SemanasPMO_Numero_Ano | Numero, Ano | Busca de semana específica |

### **Índices de Busca**

| Tabela | Coluna Indexada | Tipo | Motivo |
|--------|-----------------|------|--------|
| **Empresas** | Nome | NONCLUSTERED | Busca por nome |
| **Empresas** | CNPJ | NONCLUSTERED | Busca por CNPJ |
| **Usinas** | Codigo | NONCLUSTERED | Busca por código ONS |
| **Usinas** | Nome | NONCLUSTERED | Busca por nome |
| **UnidadesGeradoras** | Codigo | NONCLUSTERED | Busca por código |
| **Usuarios** | Email | NONCLUSTERED | Autenticação |

---

## 🎯 PADRÕES DE DESIGN NO BANCO

### **1. Soft Delete**
Todas as tabelas possuem coluna `Ativo` (bit) para exclusão lógica.

### **2. Auditoria**
Todas as tabelas possuem:
- `DataCriacao` (datetime2)
- `DataAtualizacao` (datetime2, nullable)

### **3. Chaves Primárias**
Todas as tabelas possuem:
- `Id` (int, IDENTITY, PRIMARY KEY CLUSTERED)

### **4. Convenções de Nomenclatura**
- Tabelas: PascalCase plural (Usinas, Empresas)
- Colunas: PascalCase (NomeEmpresa, DataCriacao)
- FKs: `FK_{TabelaPai}_{TabelaRef}_{Coluna}`
- Índices: `IX_{Tabela}_{Colunas}`

---

## 🔍 QUERIES ÚTEIS

### **1. Contar Registros por Tabela**

```sql
SELECT 
    t.name AS Tabela,
    SUM(p.rows) AS Registros
FROM sys.tables t
INNER JOIN sys.partitions p ON t.object_id = p.object_id
WHERE p.index_id IN (0,1)
  AND t.is_ms_shipped = 0
GROUP BY t.name
ORDER BY Registros DESC;
```

### **2. Listar Todas as FKs**

```sql
SELECT 
    fk.name AS FK_Name,
    OBJECT_NAME(fk.parent_object_id) AS Parent_Table,
    COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS Parent_Column,
    OBJECT_NAME(fk.referenced_object_id) AS Referenced_Table,
    COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) AS Referenced_Column
FROM sys.foreign_keys AS fk
INNER JOIN sys.foreign_key_columns AS fkc ON fk.object_id = fkc.constraint_object_id
ORDER BY Parent_Table, FK_Name;
```

### **3. Verificar Índices Não Utilizados**

```sql
SELECT 
    OBJECT_NAME(s.object_id) AS Tabela,
    i.name AS Indice,
    s.user_seeks,
    s.user_scans,
    s.user_lookups,
    s.user_updates
FROM sys.dm_db_index_usage_stats s
INNER JOIN sys.indexes i ON s.object_id = i.object_id AND s.index_id = i.index_id
WHERE s.database_id = DB_ID()
  AND s.user_seeks = 0
  AND s.user_scans = 0
  AND s.user_lookups = 0
ORDER BY s.user_updates DESC;
```

---

## ✅ VALIDAÇÕES DE INTEGRIDADE

### **Checklist de Qualidade**

| Item | Status | Observação |
|------|--------|------------|
| ✅ Todas as tabelas têm PK | ✅ | 31/31 tabelas |
| ✅ Todas as FKs têm índices | ✅ | 20/20 FKs |
| ✅ Campos NOT NULL apropriados | ✅ | Configurados |
| ✅ Índices em colunas de busca | ✅ | Nome, Codigo, Email |
| ✅ Índices compostos em queries frequentes | ✅ | DataRef+Subsistema |
| ✅ Soft Delete implementado | ✅ | Coluna Ativo |
| ✅ Auditoria implementada | ✅ | DataCriacao, DataAtualizacao |

---

## 📊 ESTATÍSTICAS FINAIS

```
┌──────────────────────────────────────┐
│  BANCO DE DADOS PDPw - RESUMO       │
├──────────────────────────────────────┤
│  Tabelas:           31               │
│  Registros:         ~550             │
│  Foreign Keys:      20               │
│  Índices:           64               │
│  Constraints:       31 PKs           │
│                                      │
│  Integridade:       ✅ 100%         │
│  Performance:       ✅ Otimizado    │
│  Documentação:      ✅ Completa     │
└──────────────────────────────────────┘
```

---

**📅 Gerado**: 23/12/2024  
**👤 Responsável**: Willian Bulhões  
**🔧 Script**: `scripts/analisar-banco-dados.ps1`  
**✅ Status**: Aprovado para Produção (POC)
