# ?? GLOSSÁRIO COMPLETO - Linguagem Ubíqua PDPw

**Versão:** 2.0  
**Data:** 20/12/2024  
**Branch:** `feature/backend`

---

## ?? OBJETIVO

Este glossário define a **linguagem ubíqua** do domínio PDPw, alinhada com a terminologia oficial do **ONS (Operador Nacional do Sistema Elétrico)** e do setor elétrico brasileiro.

---

## ? DOMÍNIO: SISTEMA ELÉTRICO

### **SIN (Sistema Interligado Nacional)**
Sistema de produção e transmissão de energia elétrica do Brasil, integrado nacionalmente.

### **ONS (Operador Nacional do Sistema Elétrico)**
Entidade responsável pela coordenação e controle da operação do SIN.

### **Subsistema Elétrico**
Divisão regional do SIN. Principais subsistemas:
- **SE** - Sudeste/Centro-Oeste
- **SUL** - Sul
- **NE** - Nordeste
- **N** - Norte

---

## ?? DOMÍNIO: AGENTES E ATIVOS

### **AgenteSetorEletrico** (antiga "Empresa")
Pessoa jurídica autorizada a atuar no setor elétrico brasileiro.

**Tipos:**
- Gerador
- Transmissor
- Distribuidor
- Comercializador

**Nomenclatura:**
```csharp
// ? Antiga (genérica)
public class Empresa

// ? Nova (ubíqua)
public class AgenteSetorEletrico
```

---

### **UsinaGeradora** (antiga "Usina")
Instalação destinada à produção de energia elétrica.

**Nomenclatura:**
```csharp
// ? Antiga
public class Usina

// ? Nova
public class UsinaGeradora
```

**Sinônimos aceitos:**
- Usina
- Central Geradora

---

### **TipoUsinaGeradora** (antiga "TipoUsina")
Classificação da usina por fonte de energia.

**Tipos principais:**
- **UHE** - Usina Hidrelétrica
- **UTE** - Usina Termelétrica
- **EOL** - Usina Eólica
- **UFV** - Usina Fotovoltaica (Solar)
- **PCH** - Pequena Central Hidrelétrica
- **CGH** - Central Geradora Hidrelétrica
- **UNE** - Usina Nuclear

**Nomenclatura:**
```csharp
// ? Antiga
public class TipoUsina

// ? Nova
public class TipoUsinaGeradora
```

---

### **UnidadeGeradora (UG)**
Conjunto de equipamentos (turbina + gerador) que produz energia em uma usina.

**Características:**
- Código único (ex: UTE001.01)
- Potência nominal (MW)
- Potência mínima (MW)
- Status operacional

**Exemplo:** Usina Angra 1 possui 1 UG nuclear de 640 MW.

---

### **PotenciaInstalada**
Capacidade máxima de geração de energia de uma usina ou UG, medida em **MW (megawatts)**.

---

### **PotenciaDisponivel**
Capacidade real de geração no momento, considerando restrições e paradas.

---

## ?? DOMÍNIO: PROGRAMAÇÃO

### **PDPw (Programação Diária de Produção)**
Sistema e processo de planejamento da operação elétrica do SIN para o dia seguinte.

**Nome completo:** Programação Diária da Produção de Energia Elétrica

---

### **PMO (Programa Mensal de Operação)**
Planejamento mensal da operação do SIN, base para a programação diária.

---

### **SemanaProgramaMensalOperacao** (antiga "SemanaPMO")
Semana operativa do PMO, utilizada para referenciar períodos de programação.

**Nomenclatura:**
```csharp
// ? Antiga (sigla não explicativa)
public class SemanaPMO

// ? Nova (nome completo)
public class SemanaProgramaMensalOperacao

// ? Aceito (sigla no namespace/DTO se contexto claro)
public class SemanaPMODto
```

---

### **DESSEM (Despacho Hidrotérmico de Curto Prazo)**
Modelo computacional de otimização da operação elétrica do SIN.

---

### **ProgramacaoEnergetica** (antiga "Programação")
Planejamento de geração, carga e intercâmbios para determinado período.

---

## ?? DOMÍNIO: OPERAÇÃO

### **GeracaoEnergetica**
Produção de energia elétrica por uma usina ou unidade geradora, medida em **MW médio** ou **MWh**.

---

### **CargaEletrica** (antiga "Carga")
Demanda de energia elétrica de um subsistema ou área, medida em **MW médio**.

**Tipos:**
- **CargaPrevista** - Estimativa de demanda
- **CargaVerificada** - Demanda real medida

**Nomenclatura:**
```csharp
// ? Antiga (genérica)
public class Carga

// ? Nova
public class CargaEletrica
```

---

### **IntercambioEnergetico** (antiga "Intercambio")
Transferência de energia elétrica entre subsistemas.

**Exemplo:** Intercâmbio SE ? SUL: 2.000 MW médios

**Nomenclatura:**
```csharp
// ? Antiga
public class Intercambio

// ? Nova
public class IntercambioEnergetico
```

---

### **BalancoEnergetico** (antiga "Balanco")
Equilíbrio entre geração, carga e intercâmbios de um subsistema.

**Fórmula:**
```
Geração - Carga + Intercâmbio - Perdas = Déficit/Sobra
```

**Nomenclatura:**
```csharp
// ? Antiga
public class Balanco

// ? Nova
public class BalancoEnergetico
```

---

### **DeficitEnergetico**
Situação em que a geração disponível não atende à carga de um subsistema.

---

## ?? DOMÍNIO: RESTRIÇÕES

### **RestricaoUnidadeGeradora** (antiga "RestricaoUG")
Limitação operacional de uma unidade geradora.

**Nomenclatura:**
```csharp
// ? Antiga (sigla)
public class RestricaoUG

// ? Nova (nome completo)
public class RestricaoUnidadeGeradora
```

---

### **RestricaoUsina** (antiga "RestricaoUS")
Limitação operacional de uma usina como um todo.

**Nomenclatura:**
```csharp
// ? Antiga (sigla)
public class RestricaoUS

// ? Nova
public class RestricaoUsinaGeradora
// ou
public class RestricaoUsina
```

---

### **MotivoRestricao**
Motivo/categoria de uma restrição operacional.

**Exemplos:**
- Manutenção Programada
- Falha de Equipamento
- Restrição Hidráulica
- Restrição de Combustível

---

### **ParadaUnidadeGeradora** (antiga "ParadaUG")
Período em que uma unidade geradora está fora de operação.

**Tipos:**
- **Programada** - Planejada para manutenção
- **Forçada** - Por falha/emergência

**Nomenclatura:**
```csharp
// ? Antiga
public class ParadaUG

// ? Nova
public class ParadaUnidadeGeradora
```

---

## ?? DOMÍNIO: USINAS TÉRMICAS

### **CVU (Custo Variável Unitário)**
Custo para gerar 1 MWh de energia em uma usina térmica, incluindo combustível.

**Unidade:** R$/MWh

---

### **InflexibilidadeOperacional**
Geração mínima obrigatória de uma usina térmica, independente do despacho econômico.

**Tipos:**
- **Inflexibilidade Leve** - Limite mínimo de geração
- **Inflexibilidade Média** - Nível intermediário
- **Inflexibilidade Pesada** - Geração máxima obrigatória

---

### **InflexibilidadeContratada**
Inflexibilidade definida em contrato de suprimento.

---

### **GeracaoForaMerito** (antiga "GerForaMerito")
Geração de uma usina térmica por motivos não econômicos (ex: segurança energética, restrições de rede).

**Nomenclatura:**
```csharp
// ? Antiga (abreviada)
public class GerForaMerito

// ? Nova
public class GeracaoForaMerito
```

---

### **RampasOperacionais** (antiga "RampasUsinaTermica")
Taxas de variação de geração de uma usina térmica.

**Tipos:**
- **RampaSubida** - MW/minuto
- **RampaDescida** - MW/minuto
- **TempoPartida** - Horas
- **TempoParada** - Horas

**Nomenclatura:**
```csharp
// ? Antiga
public class RampasUsinaTermica

// ? Nova
public class RampasOperacionaisTermica
// ou
public class RampasUsinaTermica (aceito)
```

---

### **ModalidadeOperacaoTermica**
Modo de operação contratado para uma usina térmica.

**Exemplos:**
- Disponibilidade Total
- Disponibilidade Parcial
- Inflexibilidade

---

### **UsinaConversora**
Usina que converte energia entre formas (ex: solar + bateria).

---

## ?? DOMÍNIO: ARQUIVOS E DOCUMENTOS

### **ArquivoDadger** (ou **DADGER**)
Arquivo de dados gerais do DESSEM, contém informações de usinas, restrições e previsões.

**Formato:** Arquivo texto estruturado (.dat)

---

### **ArquivoDadgerValor**
Valor específico extraído de um arquivo DADGER (ex: CVU, inflexibilidade).

---

### **DCA (Declaração de Carga Agregada)**
Documento consolidado de previsão de carga de um agente.

---

### **DCR (Declaração de Carga Revisada)**
Revisão de uma DCA, ajustando previsões.

---

### **RelatorioOperacional** (antiga "Relatorio")
Documento técnico gerado pelo sistema.

---

### **UploadDocumento** (antiga "Upload")
Arquivo enviado por usuário ao sistema.

---

## ?? DOMÍNIO: ADMINISTRATIVO

### **EquipeProgramacaoDiaria** (antiga "EquipePDP")
Equipe responsável pela programação diária da operação.

**Nomenclatura:**
```csharp
// ? Antiga (sigla)
public class EquipePDP

// ? Nova (nome completo)
public class EquipeProgramacaoDiaria

// ? Aceito em DTOs/namespaces
EquipePDPDto
```

---

### **UsuarioSistema** (antiga "Usuario")
Pessoa autorizada a acessar o sistema PDPw.

**Perfis:**
- Operador
- Analista
- Administrador
- Consulta

---

### **ResponsavelTecnico** (antiga "Responsavel")
Pessoa responsável tecnicamente por declarações, documentos ou processos.

---

### **ObservacaoOperacional** (antiga "Observacao")
Nota/comentário sobre evento operacional.

---

## ?? REGRAS DE NOMENCLATURA

### **Entidades do Domain:**
```csharp
// Usar nomes completos, evitar siglas obscuras
UsinaGeradora (não "Usina")
AgenteSetorEletrico (não "Empresa")
SemanaProgramaMensalOperacao (não "SemanaPMO")
```

### **DTOs:**
```csharp
// Siglas aceitas se contexto claro + sufixo Dto
SemanaPMODto (aceito - contexto claro)
CreateUsinaGeradoraDto
UsinaGeradoraResponseDto
```

### **Repositories:**
```csharp
// Usar interface + nome completo da entidade
IUsinaGeradoraRepository
IAgenteSetorEletricoRepository
```

### **Services:**
```csharp
// Usar interface + nome completo da entidade
IUsinaGeradoraService
ISemanaPMOService (sigla aceita)
```

### **Controllers:**
```csharp
// Plural do nome da entidade
UsinasGeradorasController
AgentesSetorEletricoController
SemanasPMOController (sigla aceita)
```

---

## ?? MAPEAMENTO: ANTIGA ? NOVA NOMENCLATURA

| Antiga (Genérica) | Nova (Ubíqua) | Contexto |
|-------------------|---------------|----------|
| `Usina` | `UsinaGeradora` | Entidade, Service, Repository |
| `TipoUsina` | `TipoUsinaGeradora` | Entidade |
| `Empresa` | `AgenteSetorEletrico` | Entidade |
| `EquipePDP` | `EquipeProgramacaoDiaria` | Entidade |
| `SemanaPMO` | `SemanaProgramaMensalOperacao` | Entidade |
| `Carga` | `CargaEletrica` | Entidade |
| `Intercambio` | `IntercambioEnergetico` | Entidade |
| `Balanco` | `BalancoEnergetico` | Entidade |
| `RestricaoUG` | `RestricaoUnidadeGeradora` | Entidade |
| `RestricaoUS` | `RestricaoUsinaGeradora` | Entidade |
| `ParadaUG` | `ParadaUnidadeGeradora` | Entidade |
| `GerForaMerito` | `GeracaoForaMerito` | Entidade |
| `Relatorio` | `RelatorioOperacional` | Entidade |
| `Upload` | `UploadDocumento` | Entidade |
| `Responsavel` | `ResponsavelTecnico` | Entidade |
| `Usuario` | `UsuarioSistema` | Entidade |
| `Observacao` | `ObservacaoOperacional` | Entidade |

---

## ? CHECKLIST DE VALIDAÇÃO

Ao criar nova entidade/classe, verificar:

- [ ] Nome usa terminologia do domínio (não genérica)
- [ ] Nome seria entendido por especialista ONS
- [ ] Documentação XML explica termos técnicos
- [ ] Propriedades usam nomes do setor elétrico
- [ ] Evita siglas obscuras (ou documenta se usar)
- [ ] Consistente com este glossário

---

## ?? REFERÊNCIAS

- [Glossário ONS](http://www.ons.org.br/paginas/sobre-o-ons/glossario)
- [Procedimentos de Rede ONS](http://www.ons.org.br/paginas/sobre-o-sin/procedimentos-de-rede)
- [Código Legado VB.NET](../legado/pdpw_vb/pdpw/)

---

**Última atualização:** 20/12/2024  
**Responsável:** Willian + GitHub Copilot  
**Status:** ?? Em evolução contínua
