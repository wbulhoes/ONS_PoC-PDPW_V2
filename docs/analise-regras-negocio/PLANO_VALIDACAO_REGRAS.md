# 📋 PLANO DE VALIDAÇÃO DE REGRAS DE NEGÓCIO

**Data**: 23/12/2024  
**Objetivo**: Garantir que todas as regras de negócio do legado foram migradas

---

## 🎯 ESTRATÉGIA DE ANÁLISE

### **Fase 1: Análise Inicial** ✅ CONCLUÍDA

```
✅ 17 DAOs identificados no legado
✅ 13 Business identificados no legado
✅ 15 Services criados no novo sistema
✅ 11 Gaps iniciais identificados
```

### **Fase 2: Análise Detalhada** 🔄 EM ANDAMENTO

Vamos analisar **regra por regra** dos principais DAOs/Business:

---

## 📊 PRIORIZAÇÃO DA ANÁLISE

### **Tier 1: APIs Críticas** (Alta Prioridade)

| # | DAO Legado | Service Novo | Complexidade | Status |
|---|------------|--------------|--------------|--------|
| 1 | `UsinaDAO.vb` | `UsinaService.cs` | 🟢 Baixa (128 linhas) | 🔍 Analisar |
| 2 | `CargaDAO.vb` | `CargaService.cs` | 🟢 Baixa (69 linhas) | 🔍 Analisar |
| 3 | `InterDAO.vb` | `IntercambioService.cs` | 🟢 Baixa (79 linhas) | 🔍 Analisar |
| 4 | `ArquivoDadgerValorDAO.vb` | `ArquivoDadgerService.cs` | 🟡 Média (111 linhas) | 🔍 Analisar |

### **Tier 2: APIs Complexas** (Média Prioridade)

| # | DAO Legado | Service Novo | Complexidade | Status |
|---|------------|--------------|--------------|--------|
| 5 | `OfertaExportacaoDao.vb` | N/A | 🔴 Alta (812 linhas) | ⚠️ SEM SERVICE |
| 6 | `UsinaConversoraDao.vb` | N/A | 🔴 Alta (418 linhas) | ⚠️ SEM SERVICE |
| 7 | `InflexibilidadeDao.vb` | N/A | 🟡 Média (85 linhas) | ⚠️ SEM SERVICE |

### **Tier 3: Business com Regras** (Média Prioridade)

| # | Business Legado | Service Novo | Complexidade | Status |
|---|-----------------|--------------|--------------|--------|
| 8 | `OfertaExportacaoBusiness.vb` | N/A | 🔴 CRÍTICA (1728 linhas!) | ⚠️ SEM SERVICE |
| 9 | `IntercambioBusiness.vb` | `IntercambioService.cs` | 🔴 Alta (229 linhas) | 🔍 Analisar |
| 10 | `SaldoInflexibilidadePmoBusiness.vb` | N/A | 🟡 Média (162 linhas) | ⚠️ SEM SERVICE |

---

## 🔍 ANÁLISE DETALHADA POR DAO

### **1. UsinaDAO.vb → UsinaService.cs**

#### **Regras Identificadas no Legado:**

Vamos analisar o código VB.NET:

```vb
' C:\temp\_ONS_PoC-PDPW\pdpw_act\pdpw\Dao\UsinaDAO.vb
```

**Ações:**
1. ✅ Ler arquivo completo
2. ✅ Extrair validações
3. ✅ Extrair cálculos
4. ✅ Extrair stored procedures
5. ✅ Comparar com UsinaService.cs

#### **Validações Esperadas:**

| Validação | Descrição | No Legado? | No Novo? |
|-----------|-----------|------------|----------|
| Código único | Código de usina deve ser único | ? | ? |
| Potência mínima | Potência >= 0 | ? | ? |
| Empresa existe | FK para Empresa válida | ? | ? |
| Tipo existe | FK para TipoUsina válida | ? | ? |

---

### **2. CargaDAO.vb → CargaService.cs**

#### **Regras Identificadas no Legado:**

```vb
' C:\temp\_ONS_PoC-PDPW\pdpw_act\pdpw\Dao\CargaDAO.vb
```

**Ações:**
1. ✅ Ler arquivo completo
2. ✅ Extrair validações
3. ✅ Extrair cálculos
4. ✅ Comparar com CargaService.cs

#### **Validações Esperadas:**

| Validação | Descrição | No Legado? | No Novo? |
|-----------|-----------|------------|----------|
| Data válida | Data não pode ser futura | ? | ? |
| Subsistema válido | Deve ser SE, S, NE ou N | ? | ? |
| Carga > 0 | Carga deve ser positiva | ? | ? |

---

### **3. IntercambioBusiness.vb → IntercambioService.cs**

#### **Regras de Negócio Complexas:**

```vb
' C:\temp\_ONS_PoC-PDPW\pdpw_act\pdpw\Business\IntercambioBusiness.vb
' ATENÇÃO: 229 linhas de código!
```

**Regras esperadas:**
- Validação de origem ≠ destino
- Cálculo de limites de intercâmbio
- Validação de capacidade de transmissão
- Regras de balanço energético

---

### **4. OfertaExportacaoBusiness.vb → ??? (SEM SERVICE!)**

#### **⚠️ CRÍTICO: 1728 LINHAS DE CÓDIGO!**

```vb
' C:\temp\_ONS_PoC-PDPW\pdpw_act\pdpw\Business\OfertaExportacaoBusiness.vb
```

**Status**: ❌ **SEM SERVICE CORRESPONDENTE!**

**Ações Necessárias:**
1. 🔍 Analisar detalhadamente (arquivo muito grande)
2. 📝 Documentar todas as regras
3. 🎯 Decidir: 
   - Criar `OfertaExportacaoService.cs`?
   - Distribuir regras em outros Services?
   - Descartar (funcionalidade não usada)?

---

## 🎯 PRÓXIMOS PASSOS

### **IMEDIATO (Hoje - 23/12):**

1. **Analisar UsinaDAO.vb linha por linha**
   ```powershell
   # Criar script de análise detalhada
   .\scripts\analisar-dao-detalhado.ps1 -DAO "UsinaDAO"
   ```

2. **Comparar com UsinaService.cs**
   ```powershell
   # Gerar diff de regras
   .\scripts\comparar-regras.ps1 -DAO "UsinaDAO" -Service "UsinaService"
   ```

3. **Implementar regras faltantes**
   - Se identificar gaps, adicionar ao UsinaService.cs
   - Criar testes unitários

### **CURTO PRAZO (24/12):**

4. **Analisar DAOs Tier 1** (prioridade)
   - CargaDAO
   - InterDAO
   - ArquivoDadgerValorDAO

5. **Decisão sobre OfertaExportacao**
   - Reunir com stakeholder
   - Definir escopo (dentro/fora da POC?)

### **MÉDIO PRAZO (26/12):**

6. **Implementar regras complexas**
   - IntercambioBusiness (229 linhas)
   - SaldoInflexibilidadePMO (162 linhas)

7. **Criar testes de integração**
   - Validar comportamento end-to-end

---

## 📋 TEMPLATE DE ANÁLISE DETALHADA

### **Para cada DAO/Business:**

```markdown
## [NOME_DAO] - Análise Detalhada

### 1. Informações Gerais
- **Arquivo**: UsinaDAO.vb
- **Linhas de Código**: 128
- **Complexidade**: Baixa/Média/Alta
- **Service Correspondente**: UsinaService.cs

### 2. Métodos Públicos
| Método | Descrição | Regras de Negócio |
|--------|-----------|-------------------|
| Inserir | Insere nova usina | - Valida código único<br>- Valida potência > 0 |
| Atualizar | Atualiza usina | - Não altera código |
| Excluir | Exclui usina | - Soft delete apenas |

### 3. Validações Identificadas
- ✅ Código de usina não pode ser nulo
- ✅ Código de usina deve ser único
- ✅ Potência instalada >= 0
- ✅ Empresa deve existir (FK)

### 4. Cálculos Identificados
- Nenhum

### 5. Stored Procedures Usadas
- sp_InsertUsina
- sp_UpdateUsina
- sp_GetUsinaById

### 6. Comparação com Service C#

| Regra | No Legado | No Service C# | Status |
|-------|-----------|---------------|--------|
| Código único | ✅ | ❌ | ⚠️ FALTA IMPLEMENTAR |
| Potência >= 0 | ✅ | ✅ | ✅ OK |
| FK Empresa | ✅ | ✅ | ✅ OK |

### 7. Gaps Identificados
- ⚠️ Validação de código único não implementada no C#
- ⚠️ Stored procedures não migradas (usando EF Core)

### 8. Ações Necessárias
1. Implementar validação de código único em UsinaService.cs
2. Criar teste unitário para validação
3. Validar no Swagger
```

---

## 🛠️ SCRIPTS NECESSÁRIOS

### **1. Analisar DAO Detalhado**
```powershell
scripts/analisar-dao-detalhado.ps1 -DAO "UsinaDAO"
```

### **2. Comparar Regras**
```powershell
scripts/comparar-regras.ps1 -DAO "UsinaDAO" -Service "UsinaService"
```

### **3. Gerar Relatório de Gaps**
```powershell
scripts/gerar-relatorio-gaps.ps1
```

---

## 📊 MÉTRICAS DE PROGRESSO

```
┌─────────────────────────────────────────────┐
│  VALIDAÇÃO DE REGRAS DE NEGÓCIO             │
├─────────────────────────────────────────────┤
│  DAOs Analisados:        0 / 17 (0%)        │
│  Business Analisados:    0 / 13 (0%)        │
│  Regras Validadas:       0 / ??? (0%)       │
│  Gaps Identificados:     11                 │
│  Gaps Resolvidos:        0                  │
└─────────────────────────────────────────────┘
```

**Meta**: 80% das regras críticas validadas até 26/12

---

## 🎯 DECISÕES PENDENTES

### **1. OfertaExportacaoBusiness (1728 linhas)**

**Opções:**
- A) Migrar integralmente (muito trabalho, ~3-5 dias)
- B) Migrar apenas funcionalidades essenciais (1-2 dias)
- C) **Deixar fora da POC** (apresentar apenas APIs core) ⭐ RECOMENDADO

**Recomendação**: Opção C - Focar em APIs core, deixar OfertaExportacao para fase 2

### **2. Stored Procedures**

**No Legado**: Todos os DAOs usam Stored Procedures

**No Novo**: Usamos EF Core + LINQ

**Decisão**: ✅ Substituir SPs por queries EF Core (já feito)

**Validação Necessária**: Garantir que queries EF Core retornam mesmos dados que SPs

---

## 📚 DOCUMENTAÇÃO DE REFERÊNCIA

- `docs/ANALISE_TECNICA_CODIGO_LEGADO.md` - Análise geral do legado
- `docs/analise-regras-negocio/RELATORIO_REGRAS_NEGOCIO.md` - Relatório inicial
- `C:\temp\_ONS_PoC-PDPW\pdpw_act\pdpw\` - Código-fonte legado

---

**📅 Data**: 23/12/2024  
**👤 Responsável**: Willian Bulhões  
**🎯 Status**: Análise Inicial Completa - Próximo: Análise Detalhada
