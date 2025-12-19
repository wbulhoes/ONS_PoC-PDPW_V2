# ?? TEMPLATE - TESTES DE API

**API:** {NOME_API}  
**Entidade:** {Entidade}  
**Endpoints:** {QUANTIDADE}  
**Data criação:** {DATA}

---

## ?? CHECKLIST RÁPIDO

- [ ] GET /{entidade} - Listar todos
- [ ] GET /{entidade}/{id} - Buscar por ID
- [ ] GET /{entidade}/{campo}/{valor} - Buscas específicas
- [ ] POST /{entidade} - Criar
- [ ] PUT /{entidade}/{id} - Atualizar
- [ ] DELETE /{entidade}/{id} - Deletar

---

## ?? DADOS DE TESTE

### Seed Data Disponível
```
{ENTIDADE}s no banco: X registros
```

### Registros para Testar
1. **ID 1:** {DESCRICAO}
2. **ID 2:** {DESCRICAO}
3. **ID X:** {DESCRICAO}

---

## ?? TESTE 1: LISTAR TODOS

### Endpoint
**GET /api/{entidade}**

### Passos no Swagger
1. Expandir `GET /api/{entidade}`
2. Clicar em **"Try it out"**
3. Clicar em **"Execute"**

### ? Resultado Esperado
```json
[
  {
    "id": 1,
    "{campo1}": "{valor1}",
    "{campo2}": "{valor2}",
    ...
  },
  ...
]
```

### Validações
- [ ] Status Code: **200 OK**
- [ ] Retornou **X registros**
- [ ] Todos os campos preenchidos
- [ ] Relacionamentos carregados (se houver)

---

## ?? TESTE 2: BUSCAR POR ID

### Endpoint
**GET /api/{entidade}/{id}**

### Passos no Swagger
1. Expandir `GET /api/{entidade}/{id}`
2. Clicar em **"Try it out"**
3. Preencher:
   - `id`: **1**
4. Clicar em **"Execute"**

### ? Resultado Esperado
```json
{
  "id": 1,
  "{campo1}": "{valor1}",
  "{campo2}": "{valor2}",
  ...
}
```

### Validações
- [ ] Status Code: **200 OK**
- [ ] Dados corretos retornados
- [ ] Relacionamentos carregados

### Teste Negativo
- ID inexistente (999): **404 Not Found**

---

## ?? TESTE 3: CRIAR NOVO

### Endpoint
**POST /api/{entidade}**

### Request Body
```json
{
  "{campo1}": "{valor_novo}",
  "{campo2}": "{valor_novo}",
  ...
}
```

### Passos no Swagger
1. Expandir `POST /api/{entidade}`
2. Clicar em **"Try it out"**
3. Colar JSON acima
4. Clicar em **"Execute"**

### ? Resultado Esperado
```json
{
  "id": X,
  "{campo1}": "{valor_novo}",
  "dataCriacao": "2024-12-19T...",
  ...
}
```

### Validações
- [ ] Status Code: **201 Created**
- [ ] ID gerado automaticamente
- [ ] DataCriacao preenchida
- [ ] Location header presente

### Testes de Validação
- [ ] Campo obrigatório vazio: **400 Bad Request**
- [ ] Valor inválido: **400 Bad Request**
- [ ] Duplicidade: **400 Bad Request** (se aplicável)

---

## ?? TESTE 4: ATUALIZAR

### Endpoint
**PUT /api/{entidade}/{id}**

### Request Body
```json
{
  "{campo1}": "{valor_atualizado}",
  "{campo2}": "{valor_atualizado}",
  ...
}
```

### Passos no Swagger
1. Expandir `PUT /api/{entidade}/{id}`
2. Clicar em **"Try it out"**
3. Preencher `id`: **X** (criado no teste anterior)
4. Colar JSON atualizado
5. Clicar em **"Execute"**

### ? Resultado Esperado
```json
{
  "id": X,
  "{campo1}": "{valor_atualizado}",
  "dataAtualizacao": "2024-12-19T...",
  ...
}
```

### Validações
- [ ] Status Code: **200 OK**
- [ ] Dados atualizados
- [ ] DataAtualizacao preenchida

### Teste Negativo
- ID inexistente (999): **404 Not Found**

---

## ?? TESTE 5: DELETAR

### Endpoint
**DELETE /api/{entidade}/{id}**

### Passos no Swagger
1. Expandir `DELETE /api/{entidade}/{id}`
2. Clicar em **"Try it out"**
3. Preencher `id`: **X** (criado nos testes)
4. Clicar em **"Execute"**

### ? Resultado Esperado
```
Status Code: 204 No Content
(Sem corpo na resposta)
```

### Validações
- [ ] Status Code: **204 No Content**
- [ ] GET retorna **404 Not Found** depois
- [ ] Soft delete (Ativo = false no banco)

### Teste Negativo
- ID inexistente (999): **404 Not Found**

---

## ?? TESTES ESPECÍFICOS

### {DESCREVER ENDPOINTS CUSTOMIZADOS}

Exemplo:
- GET /api/{entidade}/tipo/{tipoId}
- GET /api/{entidade}/codigo/{codigo}
- etc...

---

## ?? COMANDOS CURL

### Listar todos
```bash
curl -X GET "http://localhost:5000/api/{entidade}" -H "accept: application/json"
```

### Buscar por ID
```bash
curl -X GET "http://localhost:5000/api/{entidade}/1" -H "accept: application/json"
```

### Criar
```bash
curl -X POST "http://localhost:5000/api/{entidade}" \
  -H "Content-Type: application/json" \
  -d '{JSON_AQUI}'
```

### Atualizar
```bash
curl -X PUT "http://localhost:5000/api/{entidade}/1" \
  -H "Content-Type: application/json" \
  -d '{JSON_AQUI}'
```

### Deletar
```bash
curl -X DELETE "http://localhost:5000/api/{entidade}/1"
```

---

## ? CHECKLIST COMPLETO

### Testes Básicos
- [ ] GET lista todos
- [ ] GET por ID (existente)
- [ ] GET por ID (inexistente) ? 404
- [ ] POST criar (válido) ? 201
- [ ] POST criar (inválido) ? 400
- [ ] PUT atualizar (existente) ? 200
- [ ] PUT atualizar (inexistente) ? 404
- [ ] DELETE (existente) ? 204
- [ ] DELETE (inexistente) ? 404

### Testes de Validação
- [ ] Campo obrigatório vazio
- [ ] Valor fora do range
- [ ] String muito grande
- [ ] Data inválida
- [ ] FK inexistente

### Testes Específicos
- [ ] {ENDPOINT_CUSTOMIZADO_1}
- [ ] {ENDPOINT_CUSTOMIZADO_2}
- [ ] ...

---

## ?? OBSERVAÇÕES

### Relacionamentos
- Esta API depende de: {ENTIDADES_PAI}
- Esta API é usada por: {ENTIDADES_FILHO}

### Performance
- Tempo esperado GET lista: < 500ms
- Tempo esperado GET ID: < 200ms
- Tempo esperado POST: < 1000ms

### Segurança
- [ ] Autenticação funcionando (se implementada)
- [ ] Autorização funcionando (se implementada)

---

**Criado por:** Desenvolvedor  
**Data:** {DATA}  
**API Version:** v1  
**Status:** ? Validado
