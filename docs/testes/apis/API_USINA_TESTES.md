# ?? TESTES - API USINA

**API:** Usinas  
**Entidade:** Usina  
**Endpoints:** 8  
**Data cria��o:** 19/12/2024  
**Status:** ? Implementada

---

## ?? CHECKLIST R�PIDO

- [x] ? GET /usinas - Listar todas
- [x] ? GET /usinas/{id} - Buscar por ID
- [x] ? GET /usinas/codigo/{codigo} - Buscar por c�digo
- [x] ? GET /usinas/tipo/{tipoUsinaId} - Listar por tipo
- [x] ? GET /usinas/empresa/{empresaId} - Listar por empresa
- [x] ? POST /usinas - Criar
- [x] ? PUT /usinas/{id} - Atualizar
- [x] ? DELETE /usinas/{id} - Deletar
- [x] ? GET /usinas/verificar-codigo/{codigo} - Verificar c�digo

---

## ?? DADOS DE TESTE

### Seed Data Dispon�vel
```
Usinas no banco: 10 registros (41.493 MW total)
```

### Registros para Testar
1. **ID 1:** Itaipu (14.000 MW) - Hidrel�trica
2. **ID 2:** Belo Monte (11.233 MW) - Hidrel�trica
3. **ID 9:** Angra I (640 MW) - Nuclear
4. **ID 10:** Angra II (1.350 MW) - Nuclear

### C�digos �nicos
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
    "nome": "Usina Hidrel�trica de Itaipu",
    "tipoUsinaId": 1,
    "tipoUsina": "Hidrel�trica",
    "empresaId": 1,
    "empresa": "Itaipu Binacional",
    "capacidadeInstalada": 14000.00,
    "localizacao": "Foz do Igua�u, PR - Fronteira Brasil/Paraguai",
    "dataOperacao": "1984-05-05T00:00:00",
    "ativo": true,
    "dataCriacao": "2024-01-01T00:00:00",
    "dataAtualizacao": null
  },
  ... (9 mais usinas)
]
```

### Valida��es
- [ ] Status Code: **200 OK**
- [ ] Retornou **10 usinas**
- [ ] Todos os campos preenchidos
- [ ] `tipoUsina` � nome (n�o ID)
- [ ] `empresa` � nome (n�o ID)
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
  "nome": "Usina Hidrel�trica de Itaipu",
  "tipoUsina": "Hidrel�trica",
  "empresa": "Itaipu Binacional",
  "capacidadeInstalada": 14000.00,
  "localizacao": "Foz do Igua�u, PR - Fronteira Brasil/Paraguai",
  "dataOperacao": "1984-05-05T00:00:00"
}
```

### Valida��es
- [ ] Status Code: **200 OK**
- [ ] Itaipu retornada
- [ ] Capacidade: **14.000 MW**
- [ ] Empresa: **Itaipu Binacional**
- [ ] Data opera��o: **1984-05-05**

### Teste Negativo
- `id = 999`: **404 Not Found**
- `id = 0`: **404 Not Found**

**Pattern:** Ver `patterns/PATTERN_GET_ID.md`

---

## ?? TESTE 3: BUSCAR BELO MONTE POR C�DIGO

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
  "nome": "Usina Hidrel�trica Belo Monte",
  "tipoUsina": "Hidrel�trica",
  "empresa": "Eletronorte - Centrais El�tricas do Norte do Brasil",
  "capacidadeInstalada": 11233.00,
  "localizacao": "Altamira, PA - Rio Xingu"
}
```

### Valida��es
- [ ] Status Code: **200 OK**
- [ ] Belo Monte retornada
- [ ] 3� maior hidrel�trica do mundo
- [ ] Capacidade: **11.233 MW**

### Outros c�digos para testar:
- **UHE-TUCURUI** ? Tucuru� (8.370 MW)
- **UTN-ANGRA-I** ? Angra I (640 MW)
- **UTN-ANGRA-II** ? Angra II (1.350 MW)
- **CODIGO-INVALIDO** ? 404 Not Found

---

## ?? TESTE 4: LISTAR HIDREL�TRICAS

### Endpoint
**GET /api/usinas/tipo/{tipoUsinaId}**

### Passos no Swagger
1. Expandir `GET /api/usinas/tipo/{tipoUsinaId}`
2. Clicar em **"Try it out"**
3. Preencher:
   - `tipoUsinaId`: **1** (Hidrel�trica)
4. Clicar em **"Execute"**

### ? Resultado Esperado
```json
[
  { "nome": "Usina Hidrel�trica de Itaipu", "capacidadeInstalada": 14000 },
  { "nome": "Usina Hidrel�trica Belo Monte", "capacidadeInstalada": 11233 },
  { "nome": "Usina Hidrel�trica de Tucuru�", "capacidadeInstalada": 8370 },
  { "nome": "Usina Hidrel�trica de S�o Sim�o", "capacidadeInstalada": 1710 },
  { "nome": "Usina Hidrel�trica de Sobradinho", "capacidadeInstalada": 1050.4 },
  { "nome": "Usina Hidrel�trica de Itumbiara", "capacidadeInstalada": 2082 }
]
```

### Valida��es
- [ ] Status Code: **200 OK**
- [ ] Retornou **6 usinas** hidrel�tricas
- [ ] Total: **38.445 MW**
- [ ] Ordenado por nome

### Outros tipos para testar:
- `tipoUsinaId = 2` (T�rmica): **2 usinas** (1.058 MW)
- `tipoUsinaId = 5` (Nuclear): **2 usinas** (1.990 MW)
- `tipoUsinaId = 3` (E�lica): **0 usinas**
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
  { "nome": "Usina Hidrel�trica Belo Monte", ... },
  { "nome": "Usina Hidrel�trica de Tucuru�", ... },
  { "nome": "Usina Termel�trica do Maranh�o", ... }
]
```

### Valida��es
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
  "nome": "Usina Hidrel�trica de Jirau",
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
  "nome": "Usina Hidrel�trica de Jirau",
  "tipoUsina": "Hidrel�trica",
  "empresa": "Furnas Centrais El�tricas",
  "capacidadeInstalada": 3750.00,
  "localizacao": "Porto Velho, RO - Rio Madeira",
  "dataOperacao": "2013-09-01T00:00:00",
  "ativo": true,
  "dataCriacao": "2024-12-19T..."
}
```

### Valida��es
- [ ] Status Code: **201 Created**
- [ ] ID gerado: **11**
- [ ] `dataCriacao` preenchida automaticamente
- [ ] Location header: `/api/usinas/11`
- [ ] Relacionamentos resolvidos (TipoUsina e Empresa)

### Testes de Valida��o (400 Bad Request)

**1. C�digo duplicado:**
```json
{
  "codigo": "UHE-ITAIPU",
  ...
}
```
**Esperado:** `"J� existe uma usina com o c�digo 'UHE-ITAIPU'"`

**2. C�digo vazio:**
```json
{
  "codigo": "",
  ...
}
```
**Esperado:** `"O c�digo � obrigat�rio"`

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

**5. TipoUsina inv�lido:**
```json
{
  "tipoUsinaId": 0,
  ...
}
```
**Esperado:** `"Tipo de usina inv�lido"`

**6. Empresa inv�lida:**
```json
{
  "empresaId": 999,
  ...
}
```
**Esperado:** FK constraint error ou valida��o de neg�cio

---

## ?? TESTE 7: ATUALIZAR USINA (JIRAU)

### Endpoint
**PUT /api/usinas/{id}**

### Request Body
```json
{
  "codigo": "UHE-JIRAU",
  "nome": "Usina Hidrel�trica de Jirau (ATUALIZADA)",
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
  "nome": "Usina Hidrel�trica de Jirau (ATUALIZADA)",
  "localizacao": "Porto Velho, RO - Rio Madeira - ATUALIZADO",
  "dataAtualizacao": "2024-12-19T..."
}
```

### Valida��es
- [ ] Status Code: **200 OK**
- [ ] Nome atualizado
- [ ] Localiza��o atualizada
- [ ] `dataAtualizacao` preenchida
- [ ] `dataCriacao` n�o mudou

### Teste Negativo
- `id = 999`: **404 Not Found** - "Usina com ID 999 n�o encontrada"

### Teste de Valida��o
- Mudar c�digo para um existente: **400 Bad Request**

---

## ?? TESTE 8: VERIFICAR SE C�DIGO EXISTE

### Endpoint
**GET /api/usinas/verificar-codigo/{codigo}**

### Teste 8.1: C�digo existente
1. Preencher `codigo`: **UHE-ITAIPU**
2. **Resultado:** `{ "existe": true }`

### Teste 8.2: C�digo n�o existente
1. Preencher `codigo`: **UHE-NAO-EXISTE**
2. **Resultado:** `{ "existe": false }`

### Teste 8.3: Excluir usina da verifica��o
1. Preencher `codigo`: **UHE-ITAIPU**
2. Preencher `usinaId` (query param): **1**
3. **Resultado:** `{ "existe": false }` (porque excluiu o pr�prio ID 1)

### Valida��es
- [ ] Status Code: **200 OK** (sempre)
- [ ] Response � objeto com propriedade `existe`
- [ ] `existe` � boolean

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

### Valida��es
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

### Buscar por c�digo
```bash
curl http://localhost:5000/api/usinas/codigo/UHE-BELO-MONTE
```

### Listar hidrel�tricas
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
  -d '{"codigo":"UHE-JIRAU","nome":"Usina Hidrel�trica de Jirau","tipoUsinaId":1,"empresaId":3,"capacidadeInstalada":3750,"localizacao":"Porto Velho, RO","dataOperacao":"2013-09-01","ativo":true}'
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

### Verificar c�digo
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
- [ ] GET /usinas/tipo/1 (6 hidrel�tricas)
- [ ] GET /usinas/tipo/2 (2 t�rmicas)
- [ ] GET /usinas/tipo/5 (2 nucleares)
- [ ] GET /usinas/empresa/2 (3 da Eletronorte)
- [ ] GET /usinas/verificar-codigo/UHE-ITAIPU (existe)

### Endpoints POST
- [ ] POST /usinas (Jirau) ? 201
- [ ] POST c�digo duplicado ? 400
- [ ] POST c�digo vazio ? 400
- [ ] POST nome curto ? 400
- [ ] POST capacidade zero ? 400
- [ ] POST tipoUsina inv�lido ? 400
- [ ] POST empresa inv�lida ? FK error

### Endpoints PUT
- [ ] PUT /usinas/11 (atualizar Jirau) ? 200
- [ ] PUT /usinas/999 ? 404
- [ ] PUT c�digo duplicado ? 400

### Endpoints DELETE
- [ ] DELETE /usinas/11 (soft delete) ? 204
- [ ] DELETE /usinas/999 ? 404
- [ ] GET /usinas/11 ap�s delete ? 404

---

## ?? OBSERVA��ES

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
- 6 Hidrel�tricas (92.7%)
- 2 T�rmicas (2.5%)
- 2 Nucleares (4.8%)

---

**Criado por:** Desenvolvedor  
**Data:** 19/12/2024  
**API Version:** v1  
**Status:** ? Implementada e validada  
**Seed Data:** ? 10 usinas reais

**PRIMEIRA API COMPLETA DO SISTEMA! ??**
