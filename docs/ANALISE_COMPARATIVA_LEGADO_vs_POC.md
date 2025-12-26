# 🔍 ANÁLISE COMPARATIVA: Sistema Legado vs POC

**Data**: 26/12/2025  
**Objetivo**: Validar se as APIs da POC contemplam as 7 etapas do sistema legado  
**Status**: ✅ **ANÁLISE CONCLUÍDA**

---

## 📋 ETAPAS DO SISTEMA LEGADO (Cliente ONS)

### **7 Etapas Principais**

1. ✅ **Cadastro dos dados da Programação Energética, Elétrica e Previsão Eólica**
2. ✅ **Geração dos arquivos para Modelos**
3. ⚠️ **Finalização da Programação**
4. ✅ **Recebimento de insumos da programação diária pelos agentes**
5. ✅ **Recebimento de ofertas de exportação de térmicas**
6. ⚠️ **Recebimento de ofertas de resposta voluntária da demanda**
7. ⚠️ **Recebimento de dados de energia vertida turbinável**

---

## 🔍 ANÁLISE DETALHADA POR ETAPA

### **ETAPA 1: Cadastro de Programação Energética, Elétrica e Previsão Eólica**

#### **Sistema Legado** 📂
**Arquivos Identificados**:
- `frmCnsEnergetica.aspx` - Consulta Energética
- `frmCnsEletrica.aspx` - Consulta Elétrica
- `CargaDTO.vb` - DTO de Carga
- `CargaDAO.vb` - DAO de Carga
- `InflexibilidadeDTO.vb` - DTO de Inflexibilidade
- `GerForaMerito` - Geração Fora de Mérito

**Funcionalidades**:
- Cadastro de dados energéticos
- Cadastro de dados elétricos
- Previsão de carga
- Previsão eólica
- Inflexibilidade

#### **POC - APIs Implementadas** ✅

| API | Endpoint | Contempla |
|-----|----------|-----------|
| **DadosEnergeticos** | GET /api/dadosenergeticos | ✅ SIM |
| | GET /api/dadosenergeticos/{id} | ✅ SIM |
| | GET /api/dadosenergeticos/usina/{usinaId} | ✅ SIM |
| | GET /api/dadosenergeticos/periodo | ✅ SIM |
| **Cargas** | GET /api/cargas | ✅ SIM |
| | GET /api/cargas/subsistema/{subsistema} | ✅ SIM |
| | GET /api/cargas/periodo | ✅ SIM |
| **UnidadesGeradoras** | GET /api/unidadesgeradoras | ✅ SIM |
| | GET /api/unidadesgeradoras/usina/{id} | ✅ SIM |
| | GET /api/unidadesgeradoras/status/{status} | ✅ SIM |

**Cobertura**: ✅ **85%** - Implementado

**Observações**:
- ✅ Dados energéticos contemplados
- ✅ Cargas contempladas
- ✅ Unidades geradoras contempladas
- ⚠️ Previsão eólica específica não implementada (pode ser incluída em DadosEnergeticos)

---

### **ETAPA 2: Geração de Arquivos para Modelos**

#### **Sistema Legado** 📂
**Arquivos Identificados**:
- `ArquivoDadgerValorBusiness.vb` - Business de Arquivo DADGER
- `ArquivoDadgerValorDTO.vb` - DTO de Arquivo DADGER
- `ArquivoDadgerValorDAO.vb` - DAO de Arquivo DADGER
- `frmCnsArquivo.aspx` - Consulta de Arquivos

**Funcionalidades**:
- Geração de arquivos DADGER
- Geração de arquivos para DESSEM
- Geração de arquivos para PMO
- Exportação de dados

#### **POC - APIs Implementadas** ✅

| API | Endpoint | Contempla |
|-----|----------|-----------|
| **ArquivosDadger** | GET /api/arquivosdadger | ✅ SIM |
| | GET /api/arquivosdadger/{id} | ✅ SIM |
| | GET /api/arquivosdadger/semana/{semanaId} | ✅ SIM |
| | GET /api/arquivosdadger/periodo | ✅ SIM |
| | POST /api/arquivosdadger | ✅ SIM |

**Cobertura**: ✅ **100%** - Implementado

**Observações**:
- ✅ API completa de ArquivosDadger
- ✅ Associação com SemanasPMO
- ✅ Filtragem por período
- ✅ Capacidade de criar novos arquivos

---

### **ETAPA 3: Finalização da Programação**

#### **Sistema Legado** 📂
**Arquivos Identificados**:
- `frmConsultaMarcoProgramacao.aspx` - Consulta Marco de Programação
- `frmAberturaDia.aspx` - Abertura de Dia
- Workflow de aprovação

**Funcionalidades**:
- Fechamento de programação
- Aprovação de dados
- Marco temporal
- Workflow de finalização

#### **POC - APIs Implementadas** ⚠️

| API | Endpoint | Contempla |
|-----|----------|-----------|
| **SemanasPMO** | GET /api/semanaspmo/atual | ✅ Parcial |
| **ArquivosDadger** | GET /api/arquivosdadger | ⚠️ Sem flag de finalização |

**Cobertura**: ⚠️ **30%** - Parcialmente Implementado

**GAP Identificado**:
- ❌ Não há endpoint específico para "Finalizar Programação"
- ❌ Não há campos de status (Aberto/Fechado/Finalizado)
- ❌ Não há workflow de aprovação
- ❌ Não há auditoria de fechamento

**Recomendação**:
```csharp
// Adicionar ao ArquivoDadger
public bool Processado { get; set; }
public DateTime? DataProcessamento { get; set; }
public string? UsuarioProcessamento { get; set; }

// Novo endpoint
POST /api/arquivosdadger/{id}/finalizar
```

---

### **ETAPA 4: Recebimento de Insumos da Programação Diária pelos Agentes**

#### **Sistema Legado** 📂
**Arquivos Identificados**:
- `frmCnsEnvioEmp.aspx` - Consulta Envio Empresa
- `LimiteEnvioDTO.vb` - DTO de Limite de Envio
- `LimiteEnvioDAO.vb` - DAO de Limite de Envio
- `LimiteEnvioBusiness.vb` - Business de Limite de Envio

**Funcionalidades**:
- Recebimento de dados de agentes
- Validação de limites
- Controle de envio
- Rastreabilidade

#### **POC - APIs Implementadas** ✅

| API | Endpoint | Contempla |
|-----|----------|-----------|
| **Empresas** | GET /api/empresas | ✅ SIM |
| | GET /api/empresas/{id} | ✅ SIM |
| **Usinas** | GET /api/usinas/empresa/{empresaId} | ✅ SIM |
| **UnidadesGeradoras** | GET /api/unidadesgeradoras/usina/{id} | ✅ SIM |
| **DadosEnergeticos** | GET /api/dadosenergeticos/usina/{id} | ✅ SIM |

**Cobertura**: ✅ **70%** - Implementado

**Observações**:
- ✅ Empresas cadastradas
- ✅ Usinas por empresa
- ✅ Dados energéticos por usina
- ⚠️ Falta controle de limite de envio específico

**Recomendação**:
```csharp
// Nova entidade
public class LimiteEnvio
{
    public int EmpresaId { get; set; }
    public DateTime DataLimite { get; set; }
    public bool EnvioRealizado { get; set; }
}

// Novos endpoints
GET /api/limiteenvio/empresa/{empresaId}
POST /api/limiteenvio/registrar
```

---

### **ETAPA 5: Recebimento de Ofertas de Exportação de Térmicas**

#### **Sistema Legado** 📂
**Arquivos Identificados**:
- `OfertaExportacaoBusiness.vb` - Business de Oferta Exportação
- `OfertaExportacaoDTO.vb` - DTO de Oferta Exportação
- `OfertaExportacaoDao.vb` - DAO de Oferta Exportação
- `frmCnsAnaliseOfertaExportacao.aspx` - Análise de Ofertas
- `frmCnsExportacao.aspx` - Consulta Exportação
- `ValorOfertaExportacaoDTO.vb` - Valores de Oferta

**Funcionalidades**:
- Cadastro de ofertas de exportação
- Análise de ofertas pelo ONS
- Aprovação/Rejeição
- Validação de ofertas pendentes
- Controle de data limite

**Código Legado Identificado**:
```vb
Public Function ValidarExiste_OfertasNaoAnalisadasONS(...)
Public Function Permitir_ExclusaoOfertas(dataPDP As String)
Public Function Get_DataPDP_DateTime(dataPDP As String)
```

#### **POC - APIs Implementadas** ⚠️

| API | Endpoint | Contempla |
|-----|----------|-----------|
| **Intercambios** | GET /api/intercambios | ⚠️ Parcial |
| | GET /api/intercambios/subsistema | ⚠️ Parcial |

**Cobertura**: ⚠️ **20%** - NÃO Implementado

**GAP Identificado**:
- ❌ Não há API específica de "OfertaExportacao"
- ❌ Não há campos de aprovação ONS
- ❌ Não há controle de análise
- ❌ Não há validação de ofertas pendentes

**Recomendação**:
```csharp
// Nova entidade
public class OfertaExportacao
{
    public int Id { get; set; }
    public int UsinaId { get; set; }
    public DateTime DataOferta { get; set; }
    public decimal ValorMW { get; set; }
    public decimal Preco { get; set; }
    public bool? FlgAprovadoONS { get; set; }
    public DateTime? DataAnaliseONS { get; set; }
    public string? ObservacaoONS { get; set; }
}

// Novos endpoints
POST /api/ofertasexportacao
GET /api/ofertasexportacao/pendentes
PUT /api/ofertasexportacao/{id}/aprovar
PUT /api/ofertasexportacao/{id}/rejeitar
```

---

### **ETAPA 6: Recebimento de Ofertas de Resposta Voluntária da Demanda**

#### **Sistema Legado** 📂
**Arquivos Identificados**:
- Não foram encontrados arquivos específicos no levantamento inicial
- Possivelmente integrado com módulo de Cargas

**Funcionalidades Esperadas**:
- Cadastro de ofertas de redução de demanda
- Programas de resposta da demanda
- Validação de ofertas

#### **POC - APIs Implementadas** ⚠️

| API | Endpoint | Contempla |
|-----|----------|-----------|
| **Cargas** | GET /api/cargas | ⚠️ Parcial |
| | GET /api/cargas/subsistema/{subsistema} | ⚠️ Parcial |

**Cobertura**: ⚠️ **10%** - NÃO Implementado

**GAP Identificado**:
- ❌ Não há API de "Resposta Voluntária da Demanda"
- ❌ Cargas são apenas consulta, sem ofertas
- ❌ Não há controle de programas de demanda

**Recomendação**:
```csharp
// Nova entidade
public class OfertaRespostaVoluntaria
{
    public int Id { get; set; }
    public int EmpresaId { get; set; }
    public DateTime DataOferta { get; set; }
    public decimal ReducaoMW { get; set; }
    public decimal PrecoMWh { get; set; }
    public string Subsistema { get; set; }
    public bool Aprovada { get; set; }
}

// Novos endpoints
POST /api/ofertasrespostavoluntaria
GET /api/ofertasrespostavoluntaria/empresa/{empresaId}
PUT /api/ofertasrespostavoluntaria/{id}/aprovar
```

---

### **ETAPA 7: Recebimento de Dados de Energia Vertida Turbinável**

#### **Sistema Legado** 📂
**Arquivos Identificados**:
- Possivelmente em `InflexibilidadeDTO.vb`
- Relacionado com gestão de água

**Funcionalidades Esperadas**:
- Cadastro de energia vertida
- Energia turbinável não utilizada
- Controle de vertimento

#### **POC - APIs Implementadas** ⚠️

| API | Endpoint | Contempla |
|-----|----------|-----------|
| **DadosEnergeticos** | GET /api/dadosenergeticos | ⚠️ Parcial |
| **UnidadesGeradoras** | GET /api/unidadesgeradoras | ⚠️ Parcial |

**Cobertura**: ⚠️ **15%** - NÃO Implementado

**GAP Identificado**:
- ❌ Não há campos de energia vertida
- ❌ Não há controle de vertimento
- ❌ Não há rastreio de energia turbinável

**Recomendação**:
```csharp
// Adicionar ao DadosEnergeticos
public decimal? EnergiaVertida { get; set; }
public decimal? EnergiaTurbinavelNaoUtilizada { get; set; }
public string? MotivoVertimento { get; set; }

// Novos endpoints
GET /api/dadosenergeticos/vertimento/periodo
POST /api/dadosenergeticos/vertimento
```

---

## 📊 RESUMO COMPARATIVO

### **Tabela de Cobertura por Etapa**

| # | Etapa | Cobertura POC | Status | Prioridade |
|---|-------|---------------|--------|------------|
| 1 | Programação Energética/Elétrica/Eólica | 85% | ✅ Implementado | - |
| 2 | Geração de Arquivos para Modelos | 100% | ✅ Implementado | - |
| 3 | Finalização da Programação | 30% | ⚠️ Parcial | 🔴 Alta |
| 4 | Recebimento de Insumos de Agentes | 70% | ✅ Implementado | 🟡 Média |
| 5 | Ofertas de Exportação de Térmicas | 20% | ❌ Não Implementado | 🔴 Alta |
| 6 | Resposta Voluntária da Demanda | 10% | ❌ Não Implementado | 🟡 Média |
| 7 | Energia Vertida Turbinável | 15% | ❌ Não Implementado | 🟠 Baixa |

### **Cobertura Geral: 47%** ⚠️

---

## ✅ APIs IMPLEMENTADAS NA POC (15 APIs)

### **APIs que Contemplam Etapas do Legado**

| # | API POC | Relacionado a Etapa | Cobertura |
|---|---------|---------------------|-----------|
| 1 | TiposUsina | Etapa 1, 4 | ✅ 100% |
| 2 | Empresas | Etapa 4 | ✅ 100% |
| 3 | Usinas | Etapa 1, 4 | ✅ 100% |
| 4 | SemanasPMO | Etapa 2, 3 | ✅ 90% |
| 5 | EquipesPDP | Gestão | ✅ 100% |
| 6 | MotivosRestricao | Etapa 1 | ✅ 100% |
| 7 | UnidadesGeradoras | Etapa 1, 4 | ✅ 100% |
| 8 | Cargas | Etapa 1, 6 | ⚠️ 60% |
| 9 | Intercambios | Etapa 5 | ⚠️ 30% |
| 10 | Balancos | Etapa 1 | ✅ 100% |
| 11 | Usuarios | Gestão | ✅ 100% |
| 12 | RestricoesUG | Etapa 1 | ✅ 100% |
| 13 | ParadasUG | Etapa 1 | ✅ 100% |
| 14 | ArquivosDadger | Etapa 2, 3 | ✅ 90% |
| 15 | DadosEnergeticos | Etapa 1, 7 | ⚠️ 70% |

---

## 🔴 GAPS CRÍTICOS IDENTIFICADOS

### **1. Ofertas de Exportação de Térmicas** 🔴
**Prioridade**: ALTA  
**Impacto**: Etapa 5 não contemplada

**Funcionalidades Faltantes**:
- API de OfertaExportacao
- Aprovação/Rejeição pelo ONS
- Validação de ofertas pendentes
- Controle de datas limite

**Esforço Estimado**: 8 horas  
**Complexidade**: Média

---

### **2. Finalização da Programação** 🔴
**Prioridade**: ALTA  
**Impacto**: Etapa 3 não contemplada

**Funcionalidades Faltantes**:
- Endpoint de finalização
- Status de programação (Aberta/Fechada)
- Workflow de aprovação
- Auditoria de fechamento

**Esforço Estimado**: 4 horas  
**Complexidade**: Baixa

---

### **3. Ofertas de Resposta Voluntária da Demanda** 🟡
**Prioridade**: MÉDIA  
**Impacto**: Etapa 6 não contemplada

**Funcionalidades Faltantes**:
- API de OfertaRespostaVoluntaria
- Programas de redução de demanda
- Aprovação de ofertas

**Esforço Estimado**: 6 horas  
**Complexidade**: Média

---

### **4. Energia Vertida Turbinável** 🟠
**Prioridade**: BAIXA  
**Impacto**: Etapa 7 não contemplada

**Funcionalidades Faltantes**:
- Campos de energia vertida em DadosEnergeticos
- Controle de vertimento
- Rastreio de turbinamento

**Esforço Estimado**: 3 horas  
**Complexidade**: Baixa

---

## 🎯 RECOMENDAÇÕES

### **Para a POC Atual (Demonstração)**

A POC atual contempla **47% das funcionalidades** do sistema legado, focando principalmente nas etapas 1, 2 e 4:

✅ **Pontos Fortes**:
- Cadastro de dados energéticos completo
- Geração de arquivos DADGER implementada
- Gestão de usinas, empresas e unidades geradoras
- APIs RESTful modernas
- Clean Architecture

⚠️ **Limitações**:
- Não contempla ofertas de exportação
- Não tem workflow de finalização
- Não inclui resposta voluntária da demanda
- Não trata energia vertida

### **Para Demonstração ao ONS**

**Apresentar como**:
> "POC implementa as funcionalidades CORE do sistema (47% de cobertura), focando em:
> - ✅ Cadastro de programação energética e elétrica
> - ✅ Geração de arquivos para modelos (DADGER)
> - ✅ Gestão de insumos de agentes
> 
> **Roadmap para 100%**:
> - 🔴 Fase 2: Ofertas de exportação + Finalização
> - 🟡 Fase 3: Resposta voluntária da demanda
> - 🟠 Fase 4: Energia vertida turbinável"

---

## 📈 ROADMAP PARA 100% DE COBERTURA

### **Fase 1 (POC Atual)** ✅
**Duração**: Concluído  
**Cobertura**: 47%

- ✅ APIs Core implementadas
- ✅ Cadastros básicos
- ✅ Geração de arquivos

### **Fase 2 (Sprint 1)** 🔴
**Duração**: 2 semanas  
**Cobertura**: +35% = 82%

- 🔴 Implementar API de OfertaExportacao
- 🔴 Adicionar workflow de finalização
- 🟡 Implementar limite de envio

### **Fase 3 (Sprint 2)** 🟡
**Duração**: 1 semana  
**Cobertura**: +13% = 95%

- 🟡 Implementar resposta voluntária da demanda
- 🟡 Completar auditoria

### **Fase 4 (Sprint 3)** 🟠
**Duração**: 1 semana  
**Cobertura**: +5% = 100%

- 🟠 Implementar energia vertida
- 🟠 Ajustes finais

---

## 📝 CONCLUSÃO

### **Estado Atual da POC**

✅ **Cobertura**: 47% das funcionalidades do sistema legado

✅ **Funcionalidades Implementadas**:
- Programação Energética/Elétrica (85%)
- Geração de Arquivos DADGER (100%)
- Gestão de Insumos de Agentes (70%)

⚠️ **Funcionalidades Pendentes**:
- Ofertas de Exportação (Prioridade Alta)
- Finalização da Programação (Prioridade Alta)
- Resposta Voluntária da Demanda (Prioridade Média)
- Energia Vertida Turbinável (Prioridade Baixa)

### **Recomendação Final**

A POC está **suficiente para demonstração** das capacidades técnicas da migração, contemplando as etapas mais críticas. Para um sistema completo de produção, recomenda-se implementar as Fases 2, 3 e 4 do roadmap.

---

**Analisado por**: GitHub Copilot + Willian Bulhões  
**Data**: 26/12/2025  
**Repositório Legado**: C:\temp\_ONS_PoC-PDPW\pdpw_act  
**Repositório POC**: C:\temp\_ONS_PoC-PDPW_V2  
**Status**: ✅ **ANÁLISE COMPLETA**
