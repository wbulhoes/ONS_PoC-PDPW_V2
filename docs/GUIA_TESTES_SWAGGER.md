# 🧪 GUIA DE TESTES VIA SWAGGER - POC PDPW

**Sistema**: Programação Diária da Produção de Energia  
**Cliente**: ONS - Operador Nacional do Sistema Elétrico  
**URL Swagger**: http://localhost:5001/swagger/index.html  
**Versão API**: 1.0  
**Data**: Dezembro/2025  

---

## 📋 OBJETIVO

Este documento fornece instruções detalhadas para validação manual de todas as APIs REST do sistema PDPW através da interface Swagger UI, garantindo a funcionalidade correta dos 50 endpoints implementados.

---

## 🚀 PRÉ-REQUISITOS

### Ambiente Configurado

1. **Backend Executando**:
```bash
# Via .NET CLI
cd src/PDPW.API
dotnet run

# Via Docker
docker-compose up -d
```

2. **Verificar Health Check**:
```bash
curl http://localhost:5001/health
# Resposta esperada: "Healthy"
```

3. **Acessar Swagger**:
```
http://localhost:5001/swagger/index.html
```

### Dados de Teste Disponíveis

O banco de dados possui **857 registros** pré-carregados:

| Entidade | Quantidade | Exemplos |
|----------|-----------|----------|
| TiposUsina | 8 | UHE, UTE, EOL, UFV, UTN, PCH, CGH, BIO |
| Empresas | 10 | CEMIG, COPEL, Itaipu, FURNAS, Chesf |
| Usinas | 10 | Itaipu (14GW), Belo Monte (11GW), Tucuruí (8GW) |
| UnidadesGeradoras | 100 | Distribuídas nas usinas |
| SemanasPMO | 108 | 2024-2026 |
| Cargas | 120 | Por subsistema (SE, S, NE, N) |
| Intercambios | 240 | Entre subsistemas |

---

## 📚 ESTRUTURA DO SWAGGER

### Organização das APIs

```
PDPW API v1
├── 📁 TiposUsina (5 endpoints)
├── 📁 Empresas (6 endpoints)
├── 📁 Usinas (7 endpoints)
├── 📁 UnidadesGeradoras (7 endpoints)
├── 📁 SemanasPMO (6 endpoints)
├── 📁 EquipesPDP (5 endpoints)
├── 📁 Cargas (7 endpoints)
├── 📁 Intercambios (6 endpoints)
├── 📁 Balancos (6 endpoints)
├── 📁 RestricoesUG (6 endpoints)
├── 📁 ParadasUG (6 endpoints)
├── 📁 MotivosRestricao (5 endpoints)
├── 📁 ArquivosDadger (10 endpoints)
├── 📁 DadosEnergeticos (7 endpoints)
└── 📁 Usuarios (6 endpoints)
```

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

## 🎯 BOAS PRÁTICAS DE TESTE

### 1. Ordem de Testes Recomendada

1. **Cadastros Base** (requisitos para outros testes):
   - TiposUsina
   - Empresas
   - SemanasPMO
   - MotivosRestricao

2. **Cadastros Dependentes**:
   - Usinas (depende de TiposUsina e Empresas)
   - UnidadesGeradoras (depende de Usinas)

3. **Dados Operacionais**:
   - Cargas
   - Intercambios
   - Balancos

4. **Restrições e Paradas**:
   - RestricoesUG
   - ParadasUG

5. **Documentos e Arquivos**:
   - ArquivosDadger

### 2. Registro de Evidências

Para cada teste, documente:

```
✅ Endpoint: GET /api/usinas
✅ Status Code: 200 OK
✅ Response Time: 45ms
✅ Records Returned: 10
✅ Validation: All fields present
✅ Screenshot: evidence_001.png
```

### 3. Validações Críticas

Para cada endpoint, verificar:

- ✅ **Status Code**: Corresponde ao esperado (200, 201, 204, 400, 404)
- ✅ **Response Time**: < 500ms para GET, < 1000ms para POST/PUT
- ✅ **Data Integrity**: Todos os campos retornados corretamente
- ✅ **Relationships**: Dados de navegação (FKs) corretos
- ✅ **Error Handling**: Mensagens de erro claras e úteis
- ✅ **Validation Rules**: Regras de negócio aplicadas

---

## 📊 TEMPLATE DE RELATÓRIO DE TESTES

```markdown
# Relatório de Testes - API PDPW

**Data**: DD/MM/YYYY
**Testador**: [Nome]
**Ambiente**: [Desenvolvimento/Homologação]
**Versão**: 1.0

## Resumo Executivo

- **Total de APIs**: 15
- **Total de Endpoints**: 50
- **Endpoints Testados**: __/50
- **Sucessos**: __
- **Falhas**: __
- **Taxa de Sucesso**: __%

## Detalhamento por API

### 1. API Usinas

| Endpoint | Método | Status | Observações |
|----------|--------|--------|-------------|
| /api/usinas | GET | ✅ PASS | 10 registros retornados |
| /api/usinas/{id} | GET | ✅ PASS | - |
| /api/usinas | POST | ✅ PASS | Validações OK |
| /api/usinas/{id} | PUT | ✅ PASS | - |
| /api/usinas/{id} | DELETE | ✅ PASS | Soft delete funcional |

### 2. API Cargas

[Repetir estrutura acima]

## Bugs Identificados

| ID | Severidade | API | Descrição | Status |
|----|-----------|-----|-----------|--------|
| BUG-001 | Alta | Usinas | [Descrição] | Aberto |

## Evidências

- evidence_001.png - GET /api/usinas
- evidence_002.png - POST /api/usinas
[...]

## Conclusão

[Observações gerais sobre os testes]

## Assinaturas

**Testador**: _________________  
**Aprovador**: _________________
```

---

## ✅ CHECKLIST FINAL DE VALIDAÇÃO

### APIs Cadastros Base
- [ ] TiposUsina - Listar (GET)
- [ ] TiposUsina - Buscar por ID (GET)
- [ ] TiposUsina - Criar (POST)
- [ ] TiposUsina - Atualizar (PUT)
- [ ] TiposUsina - Deletar (DELETE)

### APIs Operacionais
- [ ] Cargas - Listar (GET)
- [ ] Cargas - Criar (POST)
- [ ] Cargas - Filtrar por Subsistema (GET)
- [ ] Cargas - Filtrar por Período (GET)

### APIs de Restrições
- [ ] RestricoesUG - Listar (GET)
- [ ] RestricoesUG - Criar (POST)
- [ ] RestricoesUG - Buscar Ativas (GET)

### APIs de Documentos
- [ ] ArquivosDadger - Listar (GET)
- [ ] ArquivosDadger - Criar (POST)
- [ ] ArquivosDadger - Processar (PATCH)

### Validações de Negócio
- [ ] Intercâmbios - Validar Origem ≠ Destino
- [ ] Cargas - Validar valores não negativos
- [ ] Usinas - Validar FKs existentes
- [ ] ArquivosDadger - Validar Semana PMO existente

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

**📅 Criado**: 29/12/2025   
**👤 Responsável**: Willian Bulhões  
**🔗 Swagger**: http://localhost:5001/swagger/index.html  
**✅ Status**: Pronto para Testes

**🧪 BOM TESTE! 🚀**
