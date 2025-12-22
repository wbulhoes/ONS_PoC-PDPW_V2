# ?? GUIA DE TESTES - API USINA COM SWAGGER

**Data:** 19/12/2024  
**Status:** ? PRONTO PARA TESTAR  
**Dados:** 10 usinas reais do SIN

---

## ?? PASSO 1: INICIAR A API

### Via Terminal

```powershell
cd C:\temp\_ONS_PoC-PDPW\src\PDPW.API
dotnet run
```

### Via Visual Studio

1. Abrir solu��o: `C:\temp\_ONS_PoC-PDPW\PDPW.sln`
2. Definir `PDPW.API` como projeto de inicializa��o
3. Pressionar `F5` ou clicar em ?? Run

### Aguardar inicializa��o

```
? Conex�o com banco de dados estabelecida com sucesso!
?? Iniciando aplica��o PDPW API...
?? Ambiente: Development
?? Swagger: http://localhost:5000/swagger
Now listening on: https://localhost:XXXXX
Now listening on: http://localhost:XXXXX
```

**Anote as portas!** (ex: 65417 e 65418)

---

## ?? PASSO 2: ABRIR SWAGGER

### Op��es de URL:

1. **HTTP (recomendado para testes):**
   ```
   http://localhost:65418/swagger
   ```

2. **HTTPS:**
   ```
   https://localhost:65417/swagger
   ```

3. **Porta 5000 (se estiver configurado):**
   ```
   http://localhost:5000/swagger
   https://localhost:5001/swagger
   ```

**Dica:** Use CTRL+Click no link do terminal para abrir direto!

---

## ?? PASSO 3: TESTES NO SWAGGER

### Interface do Swagger

Voc� ver�:
```
PDPW API v1
API para Programa��o Di�ria da Produ��o de Energia

Usinas
  GET    /api/usinas
  GET    /api/usinas/{id}
  GET    /api/usinas/codigo/{codigo}
  GET    /api/usinas/tipo/{tipoUsinaId}
  GET    /api/usinas/empresa/{empresaId}
  POST   /api/usinas
  PUT    /api/usinas/{id}
  DELETE /api/usinas/{id}
  GET    /api/usinas/verificar-codigo/{codigo}
```

---

## ?? TESTE 1: LISTAR TODAS AS USINAS

### Endpoint
**GET /api/usinas**

### Como testar:
1. Clicar em `GET /api/usinas`
2. Clicar em **"Try it out"**
3. Clicar em **"Execute"**

### Resultado esperado:
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
  {
    "id": 2,
    "codigo": "UHE-BELO-MONTE",
    "nome": "Usina Hidrel�trica Belo Monte",
    "tipoUsinaId": 1,
    "tipoUsina": "Hidrel�trica",
    "empresaId": 2,
    "empresa": "Eletronorte - Centrais El�tricas do Norte do Brasil",
    "capacidadeInstalada": 11233.00,
    ...
  },
  ...
]
```

### ? Valida��es:
- [ ] Status Code: **200 OK**
- [ ] Retornou **10 usinas**
- [ ] Cada usina tem todos os campos preenchidos
- [ ] Relacionamentos carregados (tipoUsina e empresa)

---

## ?? TESTE 2: BUSCAR USINA POR ID

### Endpoint
**GET /api/usinas/{id}**

### Como testar:
1. Clicar em `GET /api/usinas/{id}`
2. Clicar em **"Try it out"**
3. Preencher:
   - `id`: **1** (Itaipu)
4. Clicar em **"Execute"**

### Resultado esperado:
```json
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
  "ativo": true
}
```

### ? Valida��es:
- [ ] Status Code: **200 OK**
- [ ] Retornou **Itaipu** (maior usina do Brasil)
- [ ] Capacidade: **14.000 MW**
- [ ] Empresa: **Itaipu Binacional**

### Teste com ID inexistente:
1. Preencher `id`: **999**
2. Status Code esperado: **404 Not Found**

---

## ?? TESTE 3: BUSCAR POR C�DIGO

### Endpoint
**GET /api/usinas/codigo/{codigo}**

### Como testar:
1. Clicar em `GET /api/usinas/codigo/{codigo}`
2. Clicar em **"Try it out"**
3. Preencher:
   - `codigo`: **UHE-BELO-MONTE**
4. Clicar em **"Execute"**

### Resultado esperado:
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

### ? Valida��es:
- [ ] Status Code: **200 OK**
- [ ] Retornou **Belo Monte**
- [ ] Capacidade: **11.233 MW**
- [ ] 3� maior hidrel�trica do mundo

### Outros c�digos para testar:
- **UHE-TUCURUI** ? Tucuru�
- **UTN-ANGRA-I** ? Angra I (nuclear)
- **UTN-ANGRA-II** ? Angra II (nuclear)

---

## ?? TESTE 4: LISTAR POR TIPO

### Endpoint
**GET /api/usinas/tipo/{tipoUsinaId}**

### Teste 4.1: Hidrel�tricas
1. Clicar em `GET /api/usinas/tipo/{tipoUsinaId}`
2. Clicar em **"Try it out"**
3. Preencher:
   - `tipoUsinaId`: **1** (Hidrel�trica)
4. Clicar em **"Execute"**

### Resultado esperado:
```json
[
  { "nome": "Usina Hidrel�trica de Itaipu", ... },
  { "nome": "Usina Hidrel�trica Belo Monte", ... },
  { "nome": "Usina Hidrel�trica de Tucuru�", ... },
  { "nome": "Usina Hidrel�trica de S�o Sim�o", ... },
  { "nome": "Usina Hidrel�trica de Sobradinho", ... },
  { "nome": "Usina Hidrel�trica de Itumbiara", ... }
]
```

### ? Valida��es:
- [ ] Status Code: **200 OK**
- [ ] Retornou **6 usinas** hidrel�tricas
- [ ] Total: **38.445 MW**

### Teste 4.2: T�rmicas
- `tipoUsinaId`: **2**
- Esperado: **2 usinas** (Termo Maranh�o, Termo Pec�m)

### Teste 4.3: Nucleares
- `tipoUsinaId`: **5**
- Esperado: **2 usinas** (Angra I e II)

---

## ?? TESTE 5: LISTAR POR EMPRESA

### Endpoint
**GET /api/usinas/empresa/{empresaId}**

### Teste 5.1: Eletronorte
1. Clicar em `GET /api/usinas/empresa/{empresaId}`
2. Clicar em **"Try it out"**
3. Preencher:
   - `empresaId`: **2** (Eletronorte)
4. Clicar em **"Execute"**

### Resultado esperado:
```json
[
  { "nome": "Usina Hidrel�trica Belo Monte", ... },
  { "nome": "Usina Hidrel�trica de Tucuru�", ... },
  { "nome": "Usina Termel�trica do Maranh�o", ... }
]
```

### ? Valida��es:
- [ ] Status Code: **200 OK**
- [ ] Retornou **3 usinas**
- [ ] Todas da **Eletronorte**

### Teste 5.2: Itaipu
- `empresaId`: **1**
- Esperado: **1 usina** (Itaipu)

### Teste 5.3: Eletronuclear
- `empresaId`: **7**
- Esperado: **2 usinas** (Angra I e II)

---

## ?? TESTE 6: CRIAR NOVA USINA

### Endpoint
**POST /api/usinas**

### Como testar:
1. Clicar em `POST /api/usinas`
2. Clicar em **"Try it out"**
3. Colar JSON no Request Body:

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

4. Clicar em **"Execute"**

### Resultado esperado:
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

### ? Valida��es:
- [ ] Status Code: **201 Created**
- [ ] Retornou usina criada com **ID 11**
- [ ] `dataCriacao` preenchida automaticamente
- [ ] Location header aponta para `/api/usinas/11`

### Teste de valida��o (erro):
Tente criar com c�digo duplicado:
```json
{
  "codigo": "UHE-ITAIPU",
  ...
}
```

**Esperado:** 
- Status Code: **400 Bad Request**
- Mensagem: "J� existe uma usina com o c�digo 'UHE-ITAIPU'"

---

## ?? TESTE 7: ATUALIZAR USINA

### Endpoint
**PUT /api/usinas/{id}**

### Como testar:
1. Clicar em `PUT /api/usinas/{id}`
2. Clicar em **"Try it out"**
3. Preencher:
   - `id`: **11** (Jirau que acabamos de criar)
4. Colar JSON atualizado:

```json
{
  "codigo": "UHE-JIRAU",
  "nome": "Usina Hidrel�trica de Jirau (Atualizada)",
  "tipoUsinaId": 1,
  "empresaId": 3,
  "capacidadeInstalada": 3750,
  "localizacao": "Porto Velho, RO - Rio Madeira - ATUALIZADO",
  "dataOperacao": "2013-09-01",
  "ativo": true
}
```

5. Clicar em **"Execute"**

### Resultado esperado:
```json
{
  "id": 11,
  "nome": "Usina Hidrel�trica de Jirau (Atualizada)",
  "localizacao": "Porto Velho, RO - Rio Madeira - ATUALIZADO",
  "dataAtualizacao": "2024-12-19T..."
}
```

### ? Valida��es:
- [ ] Status Code: **200 OK**
- [ ] Nome atualizado
- [ ] Localiza��o atualizada
- [ ] `dataAtualizacao` preenchida automaticamente

---

## ?? TESTE 8: VERIFICAR C�DIGO

### Endpoint
**GET /api/usinas/verificar-codigo/{codigo}**

### Teste 8.1: C�digo existente
1. Clicar em `GET /api/usinas/verificar-codigo/{codigo}`
2. Clicar em **"Try it out"**
3. Preencher:
   - `codigo`: **UHE-ITAIPU**
4. Clicar em **"Execute"**

### Resultado esperado:
```json
{
  "existe": true
}
```

### Teste 8.2: C�digo n�o existente
- `codigo`: **UHE-NAO-EXISTE**
- Esperado: `{ "existe": false }`

### Teste 8.3: Excluir usina da verifica��o
- `codigo`: **UHE-ITAIPU**
- `usinaId` (query param): **1**
- Esperado: `{ "existe": false }` (porque excluiu o pr�prio ID 1)

---

## ?? TESTE 9: DELETAR USINA

### Endpoint
**DELETE /api/usinas/{id}**

### Como testar:
1. Clicar em `DELETE /api/usinas/{id}`
2. Clicar em **"Try it out"**
3. Preencher:
   - `id`: **11** (Jirau criada para teste)
4. Clicar em **"Execute"**

### Resultado esperado:
```
Status Code: 204 No Content
(Sem body na resposta)
```

### ? Valida��es:
- [ ] Status Code: **204 No Content**
- [ ] Sem corpo na resposta

### Verificar soft delete:
1. Tentar buscar novamente: `GET /api/usinas/11`
2. Esperado: **404 Not Found**
3. No banco, verificar: `SELECT * FROM Usinas WHERE Id = 11`
4. Registro existe mas `Ativo = 0`

---

## ?? CHECKLIST COMPLETO DE TESTES

### Endpoints GET
- [ ] ? GET /api/usinas (lista todas - 10 usinas)
- [ ] ? GET /api/usinas/1 (Itaipu)
- [ ] ? GET /api/usinas/999 (404 Not Found)
- [ ] ? GET /api/usinas/codigo/UHE-BELO-MONTE (Belo Monte)
- [ ] ? GET /api/usinas/tipo/1 (6 hidrel�tricas)
- [ ] ? GET /api/usinas/tipo/2 (2 t�rmicas)
- [ ] ? GET /api/usinas/tipo/5 (2 nucleares)
- [ ] ? GET /api/usinas/empresa/2 (3 da Eletronorte)
- [ ] ? GET /api/usinas/verificar-codigo/UHE-ITAIPU (existe)

### Endpoints POST
- [ ] ? POST /api/usinas (criar Jirau - 201 Created)
- [ ] ? POST /api/usinas c�digo duplicado (400 Bad Request)
- [ ] ? POST /api/usinas dados inv�lidos (400 Bad Request)

### Endpoints PUT
- [ ] ? PUT /api/usinas/11 (atualizar Jirau - 200 OK)
- [ ] ? PUT /api/usinas/999 (404 Not Found)

### Endpoints DELETE
- [ ] ? DELETE /api/usinas/11 (soft delete - 204 No Content)
- [ ] ? DELETE /api/usinas/999 (404 Not Found)

---

## ?? CEN�RIOS DE TESTE AVAN�ADOS

### 1. Valida��o de Campos Obrigat�rios

**POST /api/usinas** sem campos obrigat�rios:
```json
{
  "codigo": "",
  "nome": "",
  "tipoUsinaId": 0,
  "empresaId": 0,
  "capacidadeInstalada": 0
}
```

**Esperado:** 400 Bad Request com mensagens de valida��o

### 2. Valida��o de Tamanho de String

**POST /api/usinas** com c�digo muito grande:
```json
{
  "codigo": "CODIGO-MUITO-GRANDE-QUE-EXCEDE-O-LIMITE-DE-50-CARACTERES-CONFIGURADO",
  ...
}
```

**Esperado:** 400 Bad Request

### 3. Teste de Performance

Medir tempo de resposta:
- GET /api/usinas (deve ser < 500ms)
- GET /api/usinas/1 (deve ser < 200ms)
- POST /api/usinas (deve ser < 1000ms)

### 4. Teste de Relacionamentos

Verificar se os relacionamentos est�o carregados:
- Cada usina tem `tipoUsina` (nome do tipo)
- Cada usina tem `empresa` (nome da empresa)
- N�o deve retornar apenas IDs

---

## ?? PRINTS ESPERADOS

### Swagger UI
```
Usinas
???????????????????????????????????????????
? GET /api/usinas                         ?
? Obt�m todas as usinas                   ?
?                                         ?
? [Try it out]                            ?
?                                         ?
? [Execute]                               ?
?                                         ?
? Response:                               ?
? 200 OK                                  ?
? [                                       ?
?   { "id": 1, "codigo": "UHE-ITAIPU",... ?
? ]                                       ?
???????????????????????????????????????????
```

---

## ?? TROUBLESHOOTING

### API n�o inicia
```powershell
# Verificar porta em uso
netstat -ano | findstr :5000

# Matar processo se necess�rio
taskkill /F /PID <PID>

# Tentar portas diferentes
dotnet run --urls "http://localhost:5050"
```

### Erro 500 Internal Server Error
- Verificar logs no terminal
- Verificar conex�o com banco de dados
- Verificar se migration foi aplicada

### Swagger n�o abre
- Verificar URL correta (usar porta do terminal)
- Tentar HTTP ao inv�s de HTTPS
- Verificar se `app.UseSwagger()` est� no Program.cs

### Dados n�o aparecem
```sql
-- Verificar no SQL Server
USE PDPW_DB_Dev;
SELECT COUNT(*) FROM Usinas; -- Deve retornar 10
SELECT * FROM TiposUsina;    -- Deve retornar 5
SELECT * FROM Empresas;      -- Deve retornar 8
```

---

## ?? RESULTADOS ESPERADOS

### Resumo de Testes

```
Total de Endpoints: 8
GET:    5 endpoints
POST:   1 endpoint
PUT:    1 endpoint
DELETE: 1 endpoint

Dados no banco:
- TiposUsina:  5 registros
- Empresas:    8 registros
- Usinas:     10 registros

Capacidade Total: 41.493 MW
```

---

## ?? CONCLUS�O

Ap�s completar todos os testes, voc� ter� validado:

? CRUD completo funcionando  
? Valida��es de neg�cio (c�digo �nico)  
? Valida��es de entrada (Data Annotations)  
? Relacionamentos (TipoUsina, Empresa)  
? Soft delete  
? Dados realistas do setor el�trico  
? Performance adequada  
? Swagger bem documentado  

**API Usina 100% validada e funcionando! ??**

---

## ?? PR�XIMOS PASSOS

Ap�s validar a API Usina:

1. **Criar APIs TipoUsina e Empresa** (necess�rias para frontend)
2. **Criar API UnidadeGeradora** (relacionada com Usina)
3. **Implementar testes automatizados** (xUnit)
4. **Documentar mais exemplos** no Swagger

---

**Criado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Vers�o:** 1.0  
**Status:** ? PRONTO PARA USAR

**BOA SORTE NOS TESTES! ??**
