# 📋 Plano de Ação: 100% das Páginas Funcionais

## 🎯 Objetivo
Garantir que todas as páginas do menu PDPw estejam 100% funcionais e integradas ao backend .NET 8.

## 📊 Status Atual (Análise do Menu)

### Menus Principais
1. **Coleta** ⚠️ (83% - 24/29)
2. **Consulta** ⚠️ (66% - 19/29) ⬆️ **ATUALIZADO**
3. **Ferramentas** ❌ (25% - 1/4)
4. **Cadastro** ✅ (100% - 7/7)
5. **Exportação Energia** ⚠️ (75% - 3/4)
6. **Dados DESSEM** ❌ (20% - 1/5)
7. **Manutenção** ❌ (0% - 0/3)

---

## 🔍 ANÁLISE DETALHADA POR MENU

### 1. COLETA (Status: 40% OK)

#### ✅ Páginas OK (Já implementadas)
- `/coleta/hidraulico/vazao` - Flow ✅
- `/coleta/hidraulico/disponibilidade` - Availability ✅
- `/coleta/hidraulico/balanco` - Balance ✅
- `/coleta/termico/disponibilidade` - Availability (Type T) ✅
- `/coleta/termico/inflexibilidade` - Inflexibility ✅
- `/coleta/termico/geracao` - Generation ✅
- `/coleta/termico/modalidade-operativa` - OperatingMode ✅
- `/coleta/termico/despacho-inflexibilidade` - InflexibilityDispatch ✅
- `/coleta/termico/oferta-exportacao` - ExportOffer ✅
- `/coleta/termico/analise-oferta-exportacao` - ExportOfferAnalysis ✅
- `/coleta/termico/rro` - RRO ✅
- `/coleta/termico/oferta-semanal` - WeeklyDispatch ✅
- `/coleta/termico/restricao-combustivel` - FuelShortageRestriction ✅
- `/coleta/carga/carga` - Load ✅
- `/coleta/carga/consumo` - Consumption ✅
- `/coleta/eletrica/energia` - Energy ✅
- `/coleta/eletrica/programacao` - ProgramacaoEnergeticaPage ✅
- `/coleta/eletrica/programacao-eletrica` - ProgramacaoEletrica ✅
- `/coleta/eletrica/previsao-eolica` - PrevisaoEolica ✅
- `/coleta/restricoes/restricao-ug` - UnitRestriction ✅
- `/coleta/outros/gec` - GEC ✅
- `/coleta/outros/energia-reposicao` - ReplacementEnergyPage ✅
- `/coleta/outros/usina-conversora` - PlantConverterPage ✅
- `/coleta/insumos` - Insumos ✅

#### ❌ Páginas Pendentes (Ainda em {URL_BASE})
- `{URL_BASE}/PDPProgSemanal.aspx` - Programação Semanal ❌
- `{URL_BASE}/PDPProgDiaria.aspx` - Programação Diária ❌
- `{URL_BASE}/frmRelOfertaReducaoSemana.aspx` - Relatório Oferta Redução Semanal ❌
- `{URL_BASE}/frmUpload.aspx` - Upload ❌
- `{URL_BASE}/frmRecuperarDados.aspx` - Recuperar Dados Dia Anterior ❌

**Ação:** Criar componentes React para estas páginas

---

### 2. CONSULTA (Status: 48% OK) ⬆️ **ATUALIZADO**

#### ✅ Páginas OK (14 implementadas)
- `/consulta/carga` - CargaQuery ✅ **NOVO**
- `/consulta/geracao` - GeracaoQuery ✅ **NOVO**
- `/consulta/vazao` - VazaoQuery ✅ **NOVO**
- `/consulta/inflexibilidade` - InflexibilidadeQuery ✅ **NOVO**
- `/consulta/disponibilidade` - DisponibilidadeQuery ✅ **NOVO**
- `/consulta/maquinas-paradas` - MaquinasParadasQuery ✅ **NOVO**
- `/consulta/maquinas-operando` - MaquinasOperandoQuery ✅ **NOVO**
- `/consulta/maquinas-gerando` - MaquinasGerandoQuery ✅ **NOVO**
- `/consulta/parada-ug` - ParadaUGQuery ✅ **NOVO**
- `/consulta/razao-energetica` - RazaoEnergeticaQuery ✅ **NOVO**
- `/consulta/razao-eletrica` - RazaoEletricaQuery ✅ **NOVO**
- `/consulta/exportacao` - ExportacaoQuery ✅ **NOVO**
- `/consulta/importacao` - ImportacaoQuery ✅ **NOVO**
- `/consulta/consumo` - ConsumoQuery ✅ **NOVO**
- `/consulta/hidraulico/disponibilidade` - AvailabilityQuery ✅
- `/consulta/outros/rro` - RROQuery ✅
- `/consulta/dessem/comentarios` - Comments ✅
- `/consulta/outros/observacao` - ObservationQuery ✅
- `/consulta/outros/marcos-programacao` - ProgrammingMilestoneQuery ✅

#### ❌ Páginas Pendentes (15 restantes)
- `{URL_BASE}/frmCnsDespInflex.aspx` - Unit Commitment ❌
- `{URL_BASE}/frmCnsDespRE.aspx` - Motivo Despacho RE ❌
- `{URL_BASE}/frmCnsCompensacao.aspx` - Compensação Lastro ❌
- `{URL_BASE}/frmCnsResFaltaComb.aspx` - Restrição Combustível ❌
- `{URL_BASE}/frmCnsRampa.aspx` - Garantia Energética ❌
- `{URL_BASE}/frmCnsEnvioEmp.aspx` - Envio Dados Empresa ❌
- `{URL_BASE}/frmCnsCreForaMerito.aspx` - Crédito Fora Mérito ❌
- `{URL_BASE}/frmCnsSom.aspx` - Suprimento Ordem Mérito ❌
- `{URL_BASE}/frmCnsGEC.aspx` - GE Crédito ❌
- `{URL_BASE}/frmCnsGES.aspx` - GE Substituição ❌

**Ação:** Criar 10 componentes de consulta React restantes

---

### 3. FERRAMENTAS (Status: 20% OK)

#### ✅ Páginas OK
- `/gerar/arquivos-modelos` - GenerateModelFiles ✅

#### ❌ Páginas Pendentes
- `{URL_BASE}/frmCnsArquivo.aspx` - Download de arquivos ❌
- `{URL_BASE}/frmEnviaDados.aspx` - Envio de dados ❌
- `{URL_BASE}/frmCnsRecibo.aspx` - Visualização Recibo ❌

**Ação:** Criar 3 componentes de ferramentas

---

### 4. CADASTRO (Status: 100% ✅)

#### ✅ Páginas OK (Todas implementadas)
- `/admin/empresas` - CompanyManagement ✅
- `/admin/usinas` - PlantManagement ✅
- `/admin/motivos-despacho-eletrica` - ElectricalDispatchReasonPage ✅
- `/admin/motivos-despacho-inflexibilidade` - InflexibilityDispatchReasonPage ✅
- `/admin/inflexibilidade-contratada` - ContractedInflexibility ✅
- `/admin/usuarios` - UserManagementPage ✅
- `/admin/associacao-usuario-empresa` - UserAssociation ✅

**Status:** ✅ Menu 100% funcional

---

### 5. EXPORTAÇÃO ENERGIA (Status: 75% OK)

#### ✅ Páginas OK
- `/coleta/outros/usina-conversora` - PlantConverterPage ✅
- `/coleta/termico/oferta-exportacao` - ExportOffer ✅
- `/coleta/termico/analise-oferta-exportacao` - ExportOfferAnalysis ✅

#### ❌ Página Pendente
- Análise Oferta Exportação ONS (query string AnaliseONS=S) ⚠️ (Mesma página, só query)

**Ação:** Validar query string na página existente

---

### 6. DADOS DESSEM (Status: 20% OK)

#### ✅ Páginas OK
- `/consulta/dessem/comentarios` - Comments ✅

#### ❌ Páginas Pendentes (Legado)
- `{URL_BASE}/frmUpload.aspx?dessem=1` - Upload DESSEM ❌
- `{URL_BASE}/frmGerArquivo.aspx?dessem=1` - Gerar Arquivo DESSEM ❌
- `{URL_BASE}/frmCnsArquivo.aspx?dessem=1` - Download DESSEM ❌
- `{URL_BASE}/frmRampasUsinasTerm.aspx` - Rampas Térmicas ❌

**Ação:** Criar 4 componentes DESSEM

---

### 7. MANUTENÇÃO (Status: 0% - Admin Only)

#### ❌ Todas Pendentes
- `{URL_BASE}/frmManDiretorio.aspx` - Diretório Temporário ❌
- `{URL_BASE}/frmAberturaDia.aspx` - Abrir Dia ❌
- `{URL_BASE}/frmControleAgente.aspx` - Controle Agentes ❌

**Ação:** Criar 3 componentes de manutenção (baixa prioridade)

---

## 📈 RESUMO GERAL

| Menu | Total Páginas | Implementadas | Pendentes | % Completo |
|------|---------------|---------------|-----------|------------|
| Coleta | 29 | 24 | 5 | 83% |
| Consulta | 29 | 19 | 10 | **66%** ⬆️ |
| Ferramentas | 4 | 1 | 3 | 25% |
| Cadastro | 7 | 7 | 0 | **100%** |
| Exportação | 4 | 3 | 1 | 75% |
| DESSEM | 5 | 1 | 4 | 20% |
| Manutenção | 3 | 0 | 3 | 0% |
| **TOTAL** | **81** | **55** | **26** | **68%** ⬆️ |

---

## 🚀 PLANO DE IMPLEMENTAÇÃO

### Fase 1: Páginas Críticas (Prioridade Alta - 2 dias) ✅ **90% CONCLUÍDO**
1. ✅ Criar componentes de Consulta mais usados (14 páginas) ✅ **CONCLUÍDO**
   - ✅ Carga, Geração, Vazão, Inflexibilidade
   - ✅ Razão Energética, Razão Elétrica
   - ✅ Exportação, Importação
   - ✅ Disponibilidade, Consumo
   - ✅ Máquinas Paradas, Operando, Gerando
   - ✅ Parada UG

### Fase 2: Ferramentas e Upload (Prioridade Alta - 1 dia)
2. ✅ Implementar Upload/Download de arquivos (4 páginas)
   - Upload geral
   - Upload DESSEM
   - Download arquivos
   - Envio dados

### Fase 3: Consultas Especializadas (Prioridade Média - 1.5 dias)
3. ✅ Consultas Hidráulicas e Térmicas (10 páginas)
   - Máquinas (paradas, operando, gerando)
   - Unit Commitment
   - Restrições diversas
   - GE Crédito/Substituição

### Fase 4: Programação e Ferramentas (Prioridade Média - 1 dia)
4. ✅ Programação Semanal/Diária
5. ✅ Relatórios e Recibos
6. ✅ Recuperação de dados

### Fase 5: DESSEM (Prioridade Baixa - 1 dia)
7. ✅ Gerar arquivos DESSEM
8. ✅ Rampas térmicas

### Fase 6: Manutenção (Prioridade Baixa - 0.5 dia)
9. ✅ Ferramentas administrativas

---

## 🔧 ESTRATÉGIA TÉCNICA

### 1. Criar Template Base para Consultas
```typescript
// frontend/src/pages/Query/BaseQuery.tsx
- Layout padrão com filtros (data, empresa, usina)
- Grid de resultados
- Exportação Excel/PDF
- Integração com apiClient
```

### 2. Padronizar Services
```typescript
// Todos os services seguem padrão:
- get(filters): Promise<T[]>
- getById(id): Promise<T>
- export(filters): Promise<Blob>
```

### 3. Componentes Reutilizáveis
- `<DateFilter />`
- `<CompanyFilter />`
- `<PlantFilter />`
- `<DataGrid />`
- `<ExportButton />`

---

## ✅ CRITÉRIOS DE ACEITAÇÃO

Para cada página ser considerada "100% funcional":

1. ✅ Rota configurada em `App.tsx`
2. ✅ Componente React criado
3. ✅ Service integrado com backend via `apiClient`
4. ✅ Filtros funcionando
5. ✅ Grid exibindo dados do banco
6. ✅ CRUD completo (quando aplicável)
7. ✅ Tratamento de erros
8. ✅ Loading states
9. ✅ Responsive design
10. ✅ Testado em Docker

---

## 📝 PRÓXIMOS PASSOS IMEDIATOS

1. ✅ Começar Fase 1: Consultas críticas
2. ✅ Criar template `BaseQuery`
3. ✅ Implementar 5 consultas mais urgentes
4. ✅ Testar integração backend
5. ✅ Commit e push

---

**Estimativa Total:** 6-7 dias de desenvolvimento
**Prioridade:** Consultas > Upload > Ferramentas > DESSEM > Manutenção

**Objetivo Final:** 81/81 páginas funcionais (100%) ✅
