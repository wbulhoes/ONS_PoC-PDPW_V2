# ?? GLOSS�RIO COMPLETO - Linguagem Ub�qua PDPw

**Vers�o:** 2.0  
**Data:** 20/12/2024  
**Branch:** `feature/backend`

---

## ?? OBJETIVO

Este gloss�rio define a **linguagem ub�qua** do dom�nio PDPw, alinhada com a terminologia oficial do **ONS (Operador Nacional do Sistema El�trico)** e do setor el�trico brasileiro.

---

## ? DOM�NIO: SISTEMA EL�TRICO

### **SIN (Sistema Interligado Nacional)**
Sistema de produ��o e transmiss�o de energia el�trica do Brasil, integrado nacionalmente.

### **ONS (Operador Nacional do Sistema El�trico)**
Entidade respons�vel pela coordena��o e controle da opera��o do SIN.

### **Subsistema El�trico**
Divis�o regional do SIN. Principais subsistemas:
- **SE** - Sudeste/Centro-Oeste
- **SUL** - Sul
- **NE** - Nordeste
- **N** - Norte

---

## ?? DOM�NIO: AGENTES E ATIVOS

### **AgenteSetorEletrico** (antiga "Empresa")
Pessoa jur�dica autorizada a atuar no setor el�trico brasileiro.

**Tipos:**
- Gerador
- Transmissor
- Distribuidor
- Comercializador

**Nomenclatura:**
```csharp
// ? Antiga (gen�rica)
public class Empresa

// ? Nova (ub�qua)
public class AgenteSetorEletrico
```

---

### **UsinaGeradora** (antiga "Usina")
Instala��o destinada � produ��o de energia el�trica.

**Nomenclatura:**
```csharp
// ? Antiga
public class Usina

// ? Nova
public class UsinaGeradora
```

**Sin�nimos aceitos:**
- Usina
- Central Geradora

---

### **TipoUsinaGeradora** (antiga "TipoUsina")
Classifica��o da usina por fonte de energia.

**Tipos principais:**
- **UHE** - Usina Hidrel�trica
- **UTE** - Usina Termel�trica
- **EOL** - Usina E�lica
- **UFV** - Usina Fotovoltaica (Solar)
- **PCH** - Pequena Central Hidrel�trica
- **CGH** - Central Geradora Hidrel�trica
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

**Caracter�sticas:**
- C�digo �nico (ex: UTE001.01)
- Pot�ncia nominal (MW)
- Pot�ncia m�nima (MW)
- Status operacional

**Exemplo:** Usina Angra 1 possui 1 UG nuclear de 640 MW.

---

### **PotenciaInstalada**
Capacidade m�xima de gera��o de energia de uma usina ou UG, medida em **MW (megawatts)**.

---

### **PotenciaDisponivel**
Capacidade real de gera��o no momento, considerando restri��es e paradas.

---

## ?? DOM�NIO: PROGRAMA��O

### **PDPw (Programa��o Di�ria de Produ��o)**
Sistema e processo de planejamento da opera��o el�trica do SIN para o dia seguinte.

**Nome completo:** Programa��o Di�ria da Produ��o de Energia El�trica

---

### **PMO (Programa Mensal de Opera��o)**
Planejamento mensal da opera��o do SIN, base para a programa��o di�ria.

---

### **SemanaProgramaMensalOperacao** (antiga "SemanaPMO")
Semana operativa do PMO, utilizada para referenciar per�odos de programa��o.

**Nomenclatura:**
```csharp
// ? Antiga (sigla n�o explicativa)
public class SemanaPMO

// ? Nova (nome completo)
public class SemanaProgramaMensalOperacao

// ? Aceito (sigla no namespace/DTO se contexto claro)
public class SemanaPMODto
```

---

### **DESSEM (Despacho Hidrot�rmico de Curto Prazo)**
Modelo computacional de otimiza��o da opera��o el�trica do SIN.

---

### **ProgramacaoEnergetica** (antiga "Programa��o")
Planejamento de gera��o, carga e interc�mbios para determinado per�odo.

---

## ?? DOM�NIO: OPERA��O

### **GeracaoEnergetica**
Produ��o de energia el�trica por uma usina ou unidade geradora, medida em **MW m�dio** ou **MWh**.

---

### **CargaEletrica** (antiga "Carga")
Demanda de energia el�trica de um subsistema ou �rea, medida em **MW m�dio**.

**Tipos:**
- **CargaPrevista** - Estimativa de demanda
- **CargaVerificada** - Demanda real medida

**Nomenclatura:**
```csharp
// ? Antiga (gen�rica)
public class Carga

// ? Nova
public class CargaEletrica
```

---

### **IntercambioEnergetico** (antiga "Intercambio")
Transfer�ncia de energia el�trica entre subsistemas.

**Exemplo:** Interc�mbio SE ? SUL: 2.000 MW m�dios

**Nomenclatura:**
```csharp
// ? Antiga
public class Intercambio

// ? Nova
public class IntercambioEnergetico
```

---

### **BalancoEnergetico** (antiga "Balanco")
Equil�brio entre gera��o, carga e interc�mbios de um subsistema.

**F�rmula:**
```
Gera��o - Carga + Interc�mbio - Perdas = D�ficit/Sobra
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
Situa��o em que a gera��o dispon�vel n�o atende � carga de um subsistema.

---

## ?? DOM�NIO: RESTRI��ES

### **RestricaoUnidadeGeradora** (antiga "RestricaoUG")
Limita��o operacional de uma unidade geradora.

**Nomenclatura:**
```csharp
// ? Antiga (sigla)
public class RestricaoUG

// ? Nova (nome completo)
public class RestricaoUnidadeGeradora
```

---

### **RestricaoUsina** (antiga "RestricaoUS")
Limita��o operacional de uma usina como um todo.

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
Motivo/categoria de uma restri��o operacional.

**Exemplos:**
- Manuten��o Programada
- Falha de Equipamento
- Restri��o Hidr�ulica
- Restri��o de Combust�vel

---

### **ParadaUnidadeGeradora** (antiga "ParadaUG")
Per�odo em que uma unidade geradora est� fora de opera��o.

**Tipos:**
- **Programada** - Planejada para manuten��o
- **For�ada** - Por falha/emerg�ncia

**Nomenclatura:**
```csharp
// ? Antiga
public class ParadaUG

// ? Nova
public class ParadaUnidadeGeradora
```

---

## ?? DOM�NIO: USINAS T�RMICAS

### **CVU (Custo Vari�vel Unit�rio)**
Custo para gerar 1 MWh de energia em uma usina t�rmica, incluindo combust�vel.

**Unidade:** R$/MWh

---

### **InflexibilidadeOperacional**
Gera��o m�nima obrigat�ria de uma usina t�rmica, independente do despacho econ�mico.

**Tipos:**
- **Inflexibilidade Leve** - Limite m�nimo de gera��o
- **Inflexibilidade M�dia** - N�vel intermedi�rio
- **Inflexibilidade Pesada** - Gera��o m�xima obrigat�ria

---

### **InflexibilidadeContratada**
Inflexibilidade definida em contrato de suprimento.

---

### **GeracaoForaMerito** (antiga "GerForaMerito")
Gera��o de uma usina t�rmica por motivos n�o econ�micos (ex: seguran�a energ�tica, restri��es de rede).

**Nomenclatura:**
```csharp
// ? Antiga (abreviada)
public class GerForaMerito

// ? Nova
public class GeracaoForaMerito
```

---

### **RampasOperacionais** (antiga "RampasUsinaTermica")
Taxas de varia��o de gera��o de uma usina t�rmica.

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
Modo de opera��o contratado para uma usina t�rmica.

**Exemplos:**
- Disponibilidade Total
- Disponibilidade Parcial
- Inflexibilidade

---

### **UsinaConversora**
Usina que converte energia entre formas (ex: solar + bateria).

---

## ?? DOM�NIO: ARQUIVOS E DOCUMENTOS

### **ArquivoDadger** (ou **DADGER**)
Arquivo de dados gerais do DESSEM, cont�m informa��es de usinas, restri��es e previs�es.

**Formato:** Arquivo texto estruturado (.dat)

---

### **ArquivoDadgerValor**
Valor espec�fico extra�do de um arquivo DADGER (ex: CVU, inflexibilidade).

---

### **DCA (Declara��o de Carga Agregada)**
Documento consolidado de previs�o de carga de um agente.

---

### **DCR (Declara��o de Carga Revisada)**
Revis�o de uma DCA, ajustando previs�es.

---

### **RelatorioOperacional** (antiga "Relatorio")
Documento t�cnico gerado pelo sistema.

---

### **UploadDocumento** (antiga "Upload")
Arquivo enviado por usu�rio ao sistema.

---

## ?? DOM�NIO: ADMINISTRATIVO

### **EquipeProgramacaoDiaria** (antiga "EquipePDP")
Equipe respons�vel pela programa��o di�ria da opera��o.

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
Pessoa respons�vel tecnicamente por declara��es, documentos ou processos.

---

### **ObservacaoOperacional** (antiga "Observacao")
Nota/coment�rio sobre evento operacional.

---

## ?? REGRAS DE NOMENCLATURA

### **Entidades do Domain:**
```csharp
// Usar nomes completos, evitar siglas obscuras
UsinaGeradora (n�o "Usina")
AgenteSetorEletrico (n�o "Empresa")
SemanaProgramaMensalOperacao (n�o "SemanaPMO")
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

| Antiga (Gen�rica) | Nova (Ub�qua) | Contexto |
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

## ? CHECKLIST DE VALIDA��O

Ao criar nova entidade/classe, verificar:

- [ ] Nome usa terminologia do dom�nio (n�o gen�rica)
- [ ] Nome seria entendido por especialista ONS
- [ ] Documenta��o XML explica termos t�cnicos
- [ ] Propriedades usam nomes do setor el�trico
- [ ] Evita siglas obscuras (ou documenta se usar)
- [ ] Consistente com este gloss�rio

---

## ?? REFER�NCIAS

- [Gloss�rio ONS](http://www.ons.org.br/paginas/sobre-o-ons/glossario)
- [Procedimentos de Rede ONS](http://www.ons.org.br/paginas/sobre-o-sin/procedimentos-de-rede)
- [C�digo Legado VB.NET](../legado/pdpw_vb/pdpw/)

---

**�ltima atualiza��o:** 20/12/2024  
**Respons�vel:** Willian + GitHub Copilot  
**Status:** ?? Em evolu��o cont�nua
