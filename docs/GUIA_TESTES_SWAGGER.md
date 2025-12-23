# 🧪 GUIA DE TESTES MANUAIS - SWAGGER

**URL**: http://localhost:5001/swagger/index.html  
**Data**: 23/12/2025  
**Objetivo**: Validar funcionamento de todas as APIs REST

---

## 📋 CHECKLIST DE TESTES

### ✅ **1. API Usinas** - Cadastro Base

**Endpoint**: `/api/usinas`

#### **Teste 1.1: Listar Todas as Usinas**

```http
GET /api/usinas
```

**Resultado Esperado**:
- Status: `200 OK`
- Retorna lista de usinas (50 usinas seed)
- Deve incluir: Itaipu, Belo Monte, Tucuruí

**Como Testar no Swagger**:
1. Expandir `Usinas` → `GET /api/usinas`
2. Clicar em `Try it out`
3. Clicar em `Execute`
4. Verificar Response Body

---

#### **Teste 1.2: Buscar Usina por ID**

```http
GET /api/usinas/1
```

**Resultado Esperado**:
- Status: `200 OK`
- Retorna dados da Usina Itaipu
- Campos: `id`, `codigo`, `nome`, `tipoUsinaId`, `empresaId`

**Como Testar**:
1. `GET /api/usinas/{id}` → Try it out
2. Informar `id = 1`
3. Execute
4. Verificar se retorna Itaipu

---

#### **Teste 1.3: Criar Nova Usina**

```http
POST /api/usinas
```

**Request Body**:
```json
{
  "codigo": "UHE_TESTE",
  "nome": "Usina Hidrelétrica Teste POC",
  "tipoUsinaId": 1,
  "empresaId": 1,
  "capacidadeInstalada": 1500.00,
  "localizacao": "São Paulo, SP",
  "dataOperacao": "2024-01-01"
}
```

**Resultado Esperado**:
- Status: `201 Created`
- Header `Location` com URL do recurso criado
- Body com a usina criada (incluindo `id` gerado)

**Como Testar**:
1. `POST /api/usinas` → Try it out
2. Colar o JSON acima no Request body
3. Execute
4. Verificar Response

---

#### **Teste 1.4: Buscar por Código**

```http
GET /api/usinas/codigo/UHE_TESTE
```

**Resultado Esperado**:
- Status: `200 OK`
- Retorna a usina criada no teste 1.3

---

#### **Teste 1.5: Atualizar Usina**

```http
PUT /api/usinas/{id}
```

**Request Body**:
```json
{
  "codigo": "UHE_TESTE_UPDATED",
  "nome": "Usina Hidrelétrica Teste POC - Atualizada",
  "tipoUsinaId": 1,
  "empresaId": 1,
  "capacidadeInstalada": 2000.00,
  "localizacao": "São Paulo, SP",
  "dataOperacao": "2024-01-01"
}
```

**Resultado Esperado**:
- Status: `200 OK` ou `204 No Content`
- Usina atualizada

---

#### **Teste 1.6: Deletar Usina (Soft Delete)**

```http
DELETE /api/usinas/{id}
```

**Resultado Esperado**:
- Status: `204 No Content`
- Usina marcada como `Ativo = false`

---

### ✅ **2. API Cargas** - Dados Operacionais

**Endpoint**: `/api/cargas`

#### **Teste 2.1: Listar Todas as Cargas**

```http
GET /api/cargas
```

**Resultado Esperado**:
- Status: `200 OK`
- Lista de cargas (120 registros seed)
- Campos: `id`, `dataReferencia`, `subsistemaId`, `cargaMWmed`

---

#### **Teste 2.2: Criar Nova Carga**

```http
POST /api/cargas
```

**Request Body**:
```json
{
  "dataReferencia": "2025-01-23",
  "subsistemaId": "SE",
  "cargaMWmed": 45000.50,
  "cargaVerificada": 44800.00,
  "previsaoCarga": 45500.00,
  "observacoes": "Teste POC Swagger"
}
```

**Resultado Esperado**:
- Status: `201 Created`
- Carga criada com ID gerado

**Validações Esperadas** (devem FALHAR se violadas):

**Teste 2.2.1: Data Obrigatória**
```json
{
  "subsistemaId": "SE",
  "cargaMWmed": 45000.50
}
```
**Esperado**: `400 Bad Request` - "Data de referência é obrigatória"

**Teste 2.2.2: Subsistema Obrigatório**
```json
{
  "dataReferencia": "2025-01-23",
  "cargaMWmed": 45000.50
}
```
**Esperado**: `400 Bad Request` - "Subsistema é obrigatório"

---

#### **Teste 2.3: Filtrar por Subsistema**

```http
GET /api/cargas/subsistema/SE
```

**Resultado Esperado**:
- Status: `200 OK`
- Apenas cargas do subsistema Sudeste (SE)

---

#### **Teste 2.4: Filtrar por Período**

```http
GET /api/cargas/periodo?dataInicio=2025-01-01&dataFim=2025-01-31
```

**Resultado Esperado**:
- Status: `200 OK`
- Cargas dentro do período especificado

---

### ✅ **3. API Arquivos DADGER**

**Endpoint**: `/api/arquivosdadger`

#### **Teste 3.1: Listar Arquivos**

```http
GET /api/arquivosdadger
```

**Resultado Esperado**:
- Status: `200 OK`
- Lista de arquivos DADGER (10 registros seed)

---

#### **Teste 3.2: Criar Arquivo DADGER**

```http
POST /api/arquivosdadger
```

**Request Body**:
```json
{
  "nomeArquivo": "dadger_2025_semana04.dat",
  "caminhoArquivo": "/uploads/2025/dadger_2025_semana04.dat",
  "dataImportacao": "2025-01-23T10:30:00",
  "semanaPMOId": 1,
  "observacoes": "Teste POC Swagger"
}
```

**Resultado Esperado**:
- Status: `201 Created`
- Arquivo criado com `processado = false`

**Validações Esperadas**:

**Teste 3.2.1: Validar SemanaPMO Existente**
```json
{
  "nomeArquivo": "dadger_teste.dat",
  "caminhoArquivo": "/uploads/teste.dat",
  "dataImportacao": "2025-01-23T10:30:00",
  "semanaPMOId": 999
}
```
**Esperado**: `400 Bad Request` - "Semana PMO com ID 999 não encontrada"

---

#### **Teste 3.3: Marcar como Processado**

```http
PATCH /api/arquivosdadger/{id}/processar
```

**Resultado Esperado**:
- Status: `200 OK`
- Arquivo com `processado = true` e `dataProcessamento` preenchida

---

#### **Teste 3.4: Filtrar por Semana PMO**

```http
GET /api/arquivosdadger/semana/1
```

**Resultado Esperado**:
- Status: `200 OK`
- Arquivos da Semana PMO 1

---

### ✅ **4. API Intercâmbios**

**Endpoint**: `/api/intercambios`

#### **Teste 4.1: Listar Intercâmbios**

```http
GET /api/intercambios
```

**Resultado Esperado**:
- Status: `200 OK`
- Lista de intercâmbios (240 registros seed)

---

#### **Teste 4.2: Criar Intercâmbio**

```http
POST /api/intercambios
```

**Request Body**:
```json
{
  "dataReferencia": "2025-01-23",
  "subsistemaOrigem": "SE",
  "subsistemaDestino": "S",
  "energiaIntercambiada": 500.00,
  "observacoes": "Teste POC Swagger"
}
```

**Resultado Esperado**:
- Status: `201 Created`
- Intercâmbio criado

**Validações Esperadas**:

**Teste 4.2.1: Origem ≠ Destino**
```json
{
  "dataReferencia": "2025-01-23",
  "subsistemaOrigem": "SE",
  "subsistemaDestino": "SE",
  "energiaIntercambiada": 500.00
}
```
**Esperado**: `400 Bad Request` - "O subsistema de origem deve ser diferente do subsistema de destino"

---

#### **Teste 4.3: Filtrar por Período**

```http
GET /api/intercambios/periodo?dataInicio=2025-01-01&dataFim=2025-01-31
```

**Resultado Esperado**:
- Status: `200 OK`
- Intercâmbios no período

---

### ✅ **5. API Restrições UG**

**Endpoint**: `/api/restricoesug`

#### **Teste 5.1: Listar Restrições**

```http
GET /api/restricoesug
```

**Resultado Esperado**:
- Status: `200 OK`
- Lista de restrições de UG (8 registros seed)

---

#### **Teste 5.2: Criar Restrição**

```http
POST /api/restricoesug
```

**Request Body**:
```json
{
  "unidadeGeradoraId": 1,
  "dataInicio": "2025-01-23",
  "dataFim": "2025-01-30",
  "motivoRestricaoId": 1,
  "potenciaRestrita": 200.00,
  "observacoes": "Manutenção preventiva programada - Teste POC"
}
```

**Resultado Esperado**:
- Status: `201 Created`
- Restrição criada

---

#### **Teste 5.3: Buscar Restrições Ativas**

```http
GET /api/restricoesug/ativas?dataReferencia=2025-01-23
```

**Resultado Esperado**:
- Status: `200 OK`
- Apenas restrições ativas na data (DataInicio ≤ 2025-01-23 ≤ DataFim)

---

### ✅ **6. API Semanas PMO**

**Endpoint**: `/api/semanaspmo`

#### **Teste 6.1: Buscar Semana Atual**

```http
GET /api/semanaspmo/atual
```

**Resultado Esperado**:
- Status: `200 OK`
- Semana PMO que contém a data de hoje

---

#### **Teste 6.2: Buscar Próximas N Semanas**

```http
GET /api/semanaspmo/proximas?quantidade=4
```

**Resultado Esperado**:
- Status: `200 OK`
- As próximas 4 semanas PMO

---

### ✅ **7. API Balanços**

**Endpoint**: `/api/balancos`

#### **Teste 7.1: Listar Balanços**

```http
GET /api/balancos
```

**Resultado Esperado**:
- Status: `200 OK`
- Lista de balanços energéticos (120 registros seed)

---

#### **Teste 7.2: Filtrar por Subsistema**

```http
GET /api/balancos/subsistema/SE
```

**Resultado Esperado**:
- Status: `200 OK`
- Balanços do Sudeste

---

### ✅ **8. API Empresas**

**Endpoint**: `/api/empresas`

#### **Teste 8.1: Listar Empresas**

```http
GET /api/empresas
```

**Resultado Esperado**:
- Status: `200 OK`
- Lista de empresas (30 registros seed)
- Deve incluir: CEMIG, COPEL, Itaipu, FURNAS

---

#### **Teste 8.2: Criar Empresa**

```http
POST /api/empresas
```

**Request Body**:
```json
{
  "nome": "Empresa Teste POC",
  "cnpj": "12345678000199",
  "telefone": "(11) 98765-4321",
  "email": "contato@empresateste.com.br",
  "endereco": "Rua Teste, 123 - São Paulo, SP"
}
```

**Resultado Esperado**:
- Status: `201 Created`
- Empresa criada

---

### ✅ **9. API Tipos de Usina**

**Endpoint**: `/api/tiposusina`

#### **Teste 9.1: Listar Tipos**

```http
GET /api/tiposusina
```

**Resultado Esperado**:
- Status: `200 OK`
- 5 tipos: Hidrelétrica, Térmica, Eólica, Solar, Nuclear

---

### ✅ **10. API Equipes PDP**

**Endpoint**: `/api/equipespdp`

#### **Teste 10.1: Listar Equipes**

```http
GET /api/equipespdp
```

**Resultado Esperado**:
- Status: `200 OK`
- 11 equipes seed

---

## 📊 CENÁRIOS DE VALIDAÇÃO COMPLETOS

### **Cenário 1: Fluxo Completo de Usina**

1. ✅ Listar tipos de usina → Escolher `tipoUsinaId`
2. ✅ Listar empresas → Escolher `empresaId`
3. ✅ Criar nova usina com os IDs acima
4. ✅ Buscar a usina criada por código
5. ✅ Atualizar capacidade instalada
6. ✅ Verificar se foi atualizado
7. ✅ Deletar (soft delete)
8. ✅ Tentar buscar novamente (não deve aparecer na lista)

---

### **Cenário 2: Fluxo de Carga Diária**

1. ✅ Listar cargas existentes
2. ✅ Criar carga para subsistema SE
3. ✅ Criar carga para subsistema S
4. ✅ Filtrar por subsistema SE
5. ✅ Filtrar por período

---

### **Cenário 3: Fluxo de Arquivo DADGER**

1. ✅ Buscar semana PMO atual
2. ✅ Criar arquivo DADGER para a semana
3. ✅ Verificar que `processado = false`
4. ✅ Marcar como processado (PATCH)
5. ✅ Verificar que `processado = true` e `dataProcessamento` preenchida
6. ✅ Filtrar arquivos por semana PMO

---

### **Cenário 4: Fluxo de Intercâmbio**

1. ✅ Criar intercâmbio SE → S
2. ✅ Criar intercâmbio NE → SE
3. ✅ Tentar criar SE → SE (deve falhar)
4. ✅ Filtrar por período
5. ✅ Filtrar por subsistema origem

---

### **Cenário 5: Fluxo de Restrição de UG**

1. ✅ Listar unidades geradoras
2. ✅ Escolher uma UG
3. ✅ Listar motivos de restrição
4. ✅ Criar restrição para a UG
5. ✅ Buscar restrições ativas para hoje
6. ✅ Verificar se aparece na lista

---

## 🔍 VALIDAÇÕES DE ERRO (Devem FALHAR)

### **1. Campos Obrigatórios**

```json
POST /api/usinas
{
  "nome": "Sem código"
}
```
**Esperado**: `400 Bad Request` - Validação de `codigo` obrigatório

---

### **2. Valores Inválidos**

```json
POST /api/cargas
{
  "dataReferencia": "2025-01-23",
  "subsistemaId": "SE",
  "cargaMWmed": -100.00
}
```
**Esperado**: `400 Bad Request` - Carga não pode ser negativa

---

### **3. Entidades Não Encontradas**

```http
GET /api/usinas/99999
```
**Esperado**: `404 Not Found`

---

### **4. Relacionamentos Inválidos**

```json
POST /api/usinas
{
  "codigo": "TESTE",
  "nome": "Teste",
  "tipoUsinaId": 999,
  "empresaId": 999
}
```
**Esperado**: `400 Bad Request` - Tipo de usina ou empresa não existe

---

## ✅ CHECKLIST FINAL DE VALIDAÇÃO

Marque conforme testar:

- [ ] **Usinas**: Listar, Criar, Atualizar, Deletar
- [ ] **Cargas**: Listar, Criar, Filtrar por Subsistema
- [ ] **Arquivos DADGER**: Listar, Criar, Processar
- [ ] **Intercâmbios**: Listar, Criar, Validar Origem≠Destino
- [ ] **Restrições UG**: Listar, Criar, Buscar Ativas
- [ ] **Semanas PMO**: Atual, Próximas
- [ ] **Balanços**: Listar, Filtrar
- [ ] **Empresas**: Listar, Criar
- [ ] **Tipos Usina**: Listar
- [ ] **Equipes PDP**: Listar
- [ ] **Dados Energéticos**: Listar
- [ ] **Usuários**: Listar
- [ ] **Unidades Geradoras**: Listar
- [ ] **Paradas UG**: Listar
- [ ] **Motivos Restrição**: Listar

---

## 📝 RELATÓRIO DE TESTES

**Data**: _____________  
**Testador**: Willian Bulhões  
**APIs Testadas**: ___ de 15  
**Endpoints Testados**: ___ de 107  
**Bugs Encontrados**: ___

**Observações**:
```
_________________________________________________
_________________________________________________
_________________________________________________
```

---

## 🎯 PRÓXIMOS PASSOS APÓS TESTES

1. ✅ Validar que todos os endpoints respondem
2. ✅ Validar que validações estão funcionando
3. ✅ Validar que relacionamentos estão corretos
4. ✅ Documentar bugs encontrados
5. ✅ Criar issues no GitHub (se necessário)

---

**📅 Criado**: 23/12/2025   
**👤 Responsável**: Willian Bulhões  
**🔗 Swagger**: http://localhost:5001/swagger/index.html  
**✅ Status**: Pronto para Testes

**🧪 BOM TESTE! 🚀**
