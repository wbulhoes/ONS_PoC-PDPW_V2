# ?? TESTES - API USINA

**API:** Usinas  
**Entidade:** Usina  
**Endpoints:** 8  
**Data criação:** 19/12/2024  
**Status:** ? Implementada

---

## ?? CHECKLIST RÁPIDO

- [x] ? GET /usinas - Listar todas
- [x] ? GET /usinas/{id} - Buscar por ID
- [x] ? GET /usinas/codigo/{codigo} - Buscar por código
- [x] ? GET /usinas/tipo/{tipoUsinaId} - Listar por tipo
- [x] ? GET /usinas/empresa/{empresaId} - Listar por empresa
- [x] ? POST /usinas - Criar
- [x] ? PUT /usinas/{id} - Atualizar
- [x] ? DELETE /usinas/{id} - Deletar
- [x] ? GET /usinas/verificar-codigo/{codigo} - Verificar código

---

## ?? DADOS DE TESTE

### Seed Data Disponível
```
Usinas no banco: 10 registros (41.493 MW total)
```

### Registros para Testar
1. **ID 1:** Itaipu (14.000 MW) - Hidrelétrica
2. **ID 2:** Belo Monte (11.233 MW) - Hidrelétrica
3. **ID 9:** Angra I (640 MW) - Nuclear
4. **ID 10:** Angra II (1.350 MW) - Nuclear

### Códigos Únicos
- `UHE-ITAIPU`
- `UHE-BELO-MONTE`
- `UHE-TUCURUI`
- `UTN-ANGRA-I`
- `UTN-ANGRA-II`

---

## ?? TESTE 1: LISTAR TODAS AS USINAS

### Endpoint
**GET /api/usinas**

### Passos no Swagger
1. Expandir `GET /api/usinas`
2. Clicar em **"Try it out"**
3. Clicar em **"Execute"**

### ? Resultado Esperado
```json
[
  {
    "id": 1,
    "codigo": "UHE-ITAIPU",
    "nome": "Usina Hidrelétrica de Itaipu",
    "tipoUsinaId": 1,
    "tipoUsina": "Hidrelétrica",
    "empresaId": 1,
    "empresa": "Itaipu Binacional",
    "capacidadeInstalada": 14000.00,
    "localizacao": "Foz do Iguaçu, PR - Fronteira Brasil/Paraguai",
    "dataOperacao": "1984-05-05T00:00:00",
    "ativo": true,
    "dataCriacao": "2024-01-01T00:00:00",
    "dataAtualizacao": null
  },
  ... (9 mais usinas)
]
```

### Validações
- [ ] Status Code: **200 OK**
- [ ] Retornou **10 usinas**
- [ ] Todos os campos preenchidos
- [ ] `tipoUsina` é nome (não ID)
- [ ] `empresa` é nome (não ID)
- [ ] Ordenado por nome
- [ ] Apenas usinas ativas

**Pattern:** Ver `patterns/PATTERN_GET_LISTA.md`

---

## ?? TESTE 2: BUSCAR ITAIPU POR ID

### Endpoint
**GET /api/usinas/{id}**

### Passos no Swagger
1. Expandir `GET /api/usinas/{id}`
2. Clicar em **"Try it out"**
3. Preencher:
   - `id`: **1**
4. Clicar em **"Execute"**

### ? Resultado Esperado
```json
{
  "id": 1,
  "codigo": "UHE-ITAIPU",
  "nome": "Usina Hidrelétrica de Itaipu",
  "tipoUsina": "Hidrelétrica",
  "empresa": "Itaipu Binacional",
  "capacidadeInstalada": 14000.00,
  "localizacao": "Foz do Iguaçu, PR - Fronteira Brasil/Paraguai",
  "dataOperacao": "1984-05-05T00:00:00"
}
```

### Validações
- [ ] Status Code: **200 OK**
- [ ] Itaipu retornada
- [ ] Capacidade: **14.000 MW**
- [ ] Empresa: **Itaipu Binacional**
- [ ] Data operação: **1984-05-05**

### Teste Negativo
- `id = 999`: **404 Not Found**
- `id = 0`: **404 Not Found**

**Pattern:** Ver `patterns/PATTERN_GET_ID.md`

---

## ?? TESTE 3: BUSCAR BELO MONTE POR CÓDIGO

### Endpoint
**GET /api/usinas/codigo/{codigo}**

### Passos no Swagger
1. Expandir `GET /api/usinas/codigo/{codigo}`
2. Clicar em **"Try it out"**
3. Preencher:
   - `codigo`: **UHE-BELO-MONTE**
4. Clicar em **"Execute"**

### ? Resultado Esperado
```json
{
  "id": 2,
  "codigo": "UHE-BELO-MONTE",
  "nome": "Usina Hidrelétrica Belo Monte",
  "tipoUsina": "Hidrelétrica",
  "empresa": "Eletronorte - Centrais Elétricas do Norte do Brasil",
  "capacidadeInstalada": 11233.00,
  "localizacao": "Altamira, PA - Rio Xingu"
}
```

### Validações
- [ ] Status Code: **200 OK**
- [ ] Belo Monte retornada
- [ ] 3ª maior hidrelétrica do mundo
- [ ] Capacidade: **11.233 MW**

### Outros códigos para testar:
- **UHE-TUCURUI** ? Tucuruí (8.370 MW)
- **UTN-ANGRA-I** ? Angra I (640 MW)
- **UTN-ANGRA-II** ? Angra II (1.350 MW)
- **CODIGO-INVALIDO** ? 404 Not Found

---

## ?? TESTE 4: LISTAR HIDRELÉTRICAS

### Endpoint
**GET /api/usinas/tipo/{tipoUsinaId}**

### Passos no Swagger
1. Expandir `GET /api/usinas/tipo/{tipoUsinaId}`
2. Clicar em **"Try it out"**
3. Preencher:
   - `tipoUsinaId`: **1** (Hidrelétrica)
4. Clicar em **"Execute"**

### ? Resultado Esperado
```json
[
  { "nome": "Usina Hidrelétrica de Itaipu", "capacidadeInstalada": 14000 },
  { "nome": "Usina Hidrelétrica Belo Monte", "capacidadeInstalada": 11233 },
  { "nome": "Usina Hidrelétrica de Tucuruí", "capacidadeInstalada": 8370 },
  { "nome": "Usina Hidrelétrica de São Simão", "capacidadeInstalada": 1710 },
  { "nome": "Usina Hidrelétrica de Sobradinho", "capacidadeInstalada": 1050.4 },
  { "nome": "Usina Hidrelétrica de Itumbiara", "capacidadeInstalada": 2082 }
]
```

### Validações
- [ ] Status Code: **200 OK**
- [ ] Retornou **6 usinas** hidrelétricas
- [ ] Total: **38.445 MW**
- [ ] Ordenado por nome

### Outros tipos para testar:
- `tipoUsinaId = 2` (Térmica): **2 usinas** (1.058 MW)
- `tipoUsinaId = 5` (Nuclear): **2 usinas** (1.990 MW)
- `tipoUsinaId = 3` (Eólica): **0 usinas**
- `tipoUsinaId = 4` (Solar): **0 usinas**
- `tipoUsinaId = 999` (inexistente): **0 usinas** (200 OK, array vazio)

---

## ?? TESTE 5: LISTAR USINAS DA ELETRONORTE

### Endpoint
**GET /api/usinas/empresa/{empresaId}**

### Passos no Swagger
1. Expandir `GET /api/usinas/empresa/{empresaId}`
2. Clicar em **"Try it out"**
3. Preencher:
   - `empresaId`: **2** (Eletronorte)
4. Clicar em **"Execute"**

### ? Resultado Esperado
```json
[
  { "nome": "Usina Hidrelétrica Belo Monte", ... },
  { "nome": "Usina Hidrelétrica de Tucuruí", ... },
  { "nome": "Usina Termelétrica do Maranhão", ... }
]
```

### Validações
- [ ] Status Code: **200 OK**
- [ ] Retornou **3 usinas**
- [ ] Todas da **Eletronorte**
- [ ] Total: **19.941 MW**

### Outras empresas para testar:
- `empresaId = 1` (Itaipu): **1 usina** (14.000 MW)
- `empresaId = 7` (Eletronuclear): **2 usinas** (Angra I e II)
- `empresaId = 3` (Furnas): **2 usinas**
- `empresaId = 999` (inexistente): **0 usinas**

---

## ?? TESTE 6: CRIAR NOVA USINA (JIRAU)

### Endpoint
**POST /api/usinas**

### Request Body
```json
{
  "codigo": "UHE-JIRAU",
  "nome": "Usina Hidrelétrica de Jirau",
  "tipoUsinaId": 1,
  "empresaId": 3,
  "capacidadeInstalada": 3750,
  "localizacao": "Porto Velho, RO - Rio Madeira",
  "dataOperacao": "2013-09-01",
  "ativo": true
}
```

### Passos no Swagger
1. Expandir `POST /api/usinas`
2. Clicar em **"Try it out"**
3. Colar JSON acima
4. Clicar em **"Execute"**

### ? Resultado Esperado
```json
{
  "id": 11,
  "codigo": "UHE-JIRAU",
  "nome": "Usina Hidrelétrica de Jirau",
  "tipoUsina": "Hidrelétrica",
  "empresa": "Furnas Centrais Elétricas",
  "capacidadeInstalada": 3750.00,
  "localizacao": "Porto Velho, RO - Rio Madeira",
  "dataOperacao": "2013-09-01T00:00:00",
  "ativo": true,
  "dataCriacao": "2024-12-19T..."
}
```

### Validações
- [ ] Status Code: **201 Created**
- [ ] ID gerado: **11**
- [ ] `dataCriacao` preenchida automaticamente
- [ ] Location header: `/api/usinas/11`
- [ ] Relacionamentos resolvidos (TipoUsina e Empresa)

### Testes de Validação (400 Bad Request)

**1. Código duplicado:**
```json
{
  "codigo": "UHE-ITAIPU",
  ...
}
```
**Esperado:** `"Já existe uma usina com o código 'UHE-ITAIPU'"`

**2. Código vazio:**
```json
{
  "codigo": "",
  ...
}
```
**Esperado:** `"O código é obrigatório"`

**3. Nome muito curto:**
```json
{
  "nome": "AB",
  ...
}
```
**Esperado:** `"O nome deve ter entre 3 e 200 caracteres"`

**4. Capacidade zero:**
```json
{
  "capacidadeInstalada": 0,
  ...
}
```
**Esperado:** `"A capacidade deve ser maior que zero"`

**5. TipoUsina inválido:**
```json
{
  "tipoUsinaId": 0,
  ...
}
```
**Esperado:** `"Tipo de usina inválido"`

**6. Empresa inválida:**
```json
{
  "empresaId": 999,
  ...
}
```
**Esperado:** FK constraint error ou validação de negócio

---

## ?? TESTE 7: ATUALIZAR USINA (JIRAU)

### Endpoint
**PUT /api/usinas/{id}**

### Request Body
```json
{
  "codigo": "UHE-JIRAU",
  "nome": "Usina Hidrelétrica de Jirau (ATUALIZADA)",
  "tipoUsinaId": 1,
  "empresaId": 3,
  "capacidadeInstalada": 3750,
  "localizacao": "Porto Velho, RO - Rio Madeira - ATUALIZADO",
  "dataOperacao": "2013-09-01",
  "ativo": true
}
```

### Passos no Swagger
1. Expandir `PUT /api/usinas/{id}`
2. Clicar em **"Try it out"**
3. Preencher:
   - `id`: **11**
4. Colar JSON atualizado acima
5. Clicar em **"Execute"**

### ? Resultado Esperado
```json
{
  "id": 11,
  "nome": "Usina Hidrelétrica de Jirau (ATUALIZADA)",
  "localizacao": "Porto Velho, RO - Rio Madeira - ATUALIZADO",
  "dataAtualizacao": "2024-12-19T..."
}
```

### Validações
- [ ] Status Code: **200 OK**
- [ ] Nome atualizado
- [ ] Localização atualizada
- [ ] `dataAtualizacao` preenchida
- [ ] `dataCriacao` não mudou

### Teste Negativo
- `id = 999`: **404 Not Found** - "Usina com ID 999 não encontrada"

### Teste de Validação
- Mudar código para um existente: **400 Bad Request**

---

## ?? TESTE 8: VERIFICAR SE CÓDIGO EXISTE

### Endpoint
**GET /api/usinas/verificar-codigo/{codigo}**

### Teste 8.1: Código existente
1. Preencher `codigo`: **UHE-ITAIPU**
2. **Resultado:** `{ "existe": true }`

### Teste 8.2: Código não existente
1. Preencher `codigo`: **UHE-NAO-EXISTE**
2. **Resultado:** `{ "existe": false }`

### Teste 8.3: Excluir usina da verificação
1. Preencher `codigo`: **UHE-ITAIPU**
2. Preencher `usinaId` (query param): **1**
3. **Resultado:** `{ "existe": false }` (porque excluiu o próprio ID 1)

### Validações
- [ ] Status Code: **200 OK** (sempre)
- [ ] Response é objeto com propriedade `existe`
- [ ] `existe` é boolean

---

## ?? TESTE 9: DELETAR USINA (JIRAU)

### Endpoint
**DELETE /api/usinas/{id}**

### Passos no Swagger
1. Expandir `DELETE /api/usinas/{id}`
2. Clicar em **"Try it out"**
3. Preencher:
   - `id`: **11** (Jirau criada nos testes)
4. Clicar em **"Execute"**

### ? Resultado Esperado
```
Status Code: 204 No Content
(Sem corpo na resposta)
```

### Validações
- [ ] Status Code: **204 No Content**
- [ ] Sem body na resposta
- [ ] Soft delete (Ativo = false no banco)

### Verificar soft delete:
1. Tentar buscar novamente: `GET /api/usinas/11`
2. **Esperado:** **404 Not Found**

### No banco (SQL):
```sql
SELECT Id, Codigo, Nome, Ativo FROM Usinas WHERE Id = 11;
-- Resultado: Id=11, Ativo=0 (false)
```

### Teste Negativo
- `id = 999`: **404 Not Found**

---

## ?? COMANDOS CURL

### Listar todas
```bash
curl http://localhost:5000/api/usinas
```

### Buscar Itaipu
```bash
curl http://localhost:5000/api/usinas/1
```

### Buscar por código
```bash
curl http://localhost:5000/api/usinas/codigo/UHE-BELO-MONTE
```

### Listar hidrelétricas
```bash
curl http://localhost:5000/api/usinas/tipo/1
```

### Listar da Eletronorte
```bash
curl http://localhost:5000/api/usinas/empresa/2
```

### Criar Jirau
```bash
curl -X POST "http://localhost:5000/api/usinas" \
  -H "Content-Type: application/json" \
  -d '{"codigo":"UHE-JIRAU","nome":"Usina Hidrelétrica de Jirau","tipoUsinaId":1,"empresaId":3,"capacidadeInstalada":3750,"localizacao":"Porto Velho, RO","dataOperacao":"2013-09-01","ativo":true}'
```

### Atualizar Jirau
```bash
curl -X PUT "http://localhost:5000/api/usinas/11" \
  -H "Content-Type: application/json" \
  -d '{"codigo":"UHE-JIRAU","nome":"UH Jirau Atualizada","tipoUsinaId":1,"empresaId":3,"capacidadeInstalada":3750,"localizacao":"Porto Velho, RO","dataOperacao":"2013-09-01","ativo":true}'
```

### Deletar Jirau
```bash
curl -X DELETE "http://localhost:5000/api/usinas/11"
```

### Verificar código
```bash
curl "http://localhost:5000/api/usinas/verificar-codigo/UHE-ITAIPU"
```

---

## ? CHECKLIST COMPLETO

### Endpoints GET
- [ ] GET /usinas (10 usinas)
- [ ] GET /usinas/1 (Itaipu)
- [ ] GET /usinas/999 (404)
- [ ] GET /usinas/codigo/UHE-BELO-MONTE (Belo Monte)
- [ ] GET /usinas/codigo/INVALIDO (404)
- [ ] GET /usinas/tipo/1 (6 hidrelétricas)
- [ ] GET /usinas/tipo/2 (2 térmicas)
- [ ] GET /usinas/tipo/5 (2 nucleares)
- [ ] GET /usinas/empresa/2 (3 da Eletronorte)
- [ ] GET /usinas/verificar-codigo/UHE-ITAIPU (existe)

### Endpoints POST
- [ ] POST /usinas (Jirau) ? 201
- [ ] POST código duplicado ? 400
- [ ] POST código vazio ? 400
- [ ] POST nome curto ? 400
- [ ] POST capacidade zero ? 400
- [ ] POST tipoUsina inválido ? 400
- [ ] POST empresa inválida ? FK error

### Endpoints PUT
- [ ] PUT /usinas/11 (atualizar Jirau) ? 200
- [ ] PUT /usinas/999 ? 404
- [ ] PUT código duplicado ? 400

### Endpoints DELETE
- [ ] DELETE /usinas/11 (soft delete) ? 204
- [ ] DELETE /usinas/999 ? 404
- [ ] GET /usinas/11 após delete ? 404

---

## ?? OBSERVAÇÕES

### Relacionamentos
- **Depende de:** TipoUsina, Empresa
- **Usado por:** UnidadeGeradora, RestricaoUS, GerForaMerito, InflexibilidadeContratada, RampasUsinaTermica, UsinaConversora

### Performance
- GET lista: < 500ms (10 registros)
- GET ID: < 200ms
- POST: < 1000ms (insere + recarrega)
- PUT: < 1000ms (atualiza + recarrega)
- DELETE: < 200ms (soft delete simples)

### Dados Reais
- 10 usinas do Sistema Interligado Nacional (SIN)
- Total: **41.493 MW** de capacidade instalada
- 6 Hidrelétricas (92.7%)
- 2 Térmicas (2.5%)
- 2 Nucleares (4.8%)

---

**Criado por:** Desenvolvedor  
**Data:** 19/12/2024  
**API Version:** v1  
**Status:** ? Implementada e validada  
**Seed Data:** ? 10 usinas reais

**PRIMEIRA API COMPLETA DO SISTEMA! ??**
