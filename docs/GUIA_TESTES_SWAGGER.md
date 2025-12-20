# ?? GUIA RÁPIDO - TESTANDO DADOS REAIS NO SWAGGER

## ?? **ACESSO RÁPIDO**

### URL do Swagger:
```
http://localhost:5001/swagger
```

---

## ? **TESTES RECOMENDADOS**

### 1?? **LISTAR TODAS AS EMPRESAS REAIS**

**Endpoint:** `GET /api/empresas`

**Como testar:**
1. Acesse Swagger: http://localhost:5001/swagger
2. Localize `GET /api/empresas`
3. Clique em **"Try it out"**
4. Clique em **"Execute"**

**Resultado esperado:**
- ? **24 empresas** (4 de seed + 20 reais)
- ? Empresas como: Furnas, CEMIG, COPEL, CHESF, Itaipu, ENGIE, etc.
- ? CNPJs válidos
- ? Telefones e emails reais

**Exemplo de resposta:**
```json
{
  "id": 100,
  "nome": "Furnas Centrais Elétricas S.A.",
  "cnpj": "23274194000144",
  "telefone": "(21) 2528-5000",
  "email": "contato@furnas.com.br",
  "endereco": "Rua Real Grandeza, 219 - Botafogo, Rio de Janeiro - RJ",
  "quantidadeUsinas": 9,
  "ativo": true
}
```

---

### 2?? **LISTAR TODAS AS USINAS REAIS**

**Endpoint:** `GET /api/usinas`

**Como testar:**
1. Localize `GET /api/usinas`
2. Clique em **"Try it out"**
3. (Opcional) Defina `pageSize=50` para ver mais resultados
4. Clique em **"Execute"**

**Resultado esperado:**
- ? **40 usinas** (10 de seed + 30 reais)
- ? Usinas como: Itaipu (14.000 MW), Belo Monte (11.233 MW), Tucuruí (8.370 MW)
- ? Localizações reais
- ? Potências instaladas corretas

**Exemplo de resposta:**
```json
{
  "id": 200,
  "codigo": "ITAIPU",
  "nome": "Usina Hidrelétrica de Itaipu",
  "tipoUsinaId": 1,
  "tipoUsina": "Hidrelétrica",
  "empresaId": 106,
  "empresa": "Itaipu Binacional",
  "capacidadeInstalada": 14000.00,
  "localizacao": "Foz do Iguaçu, PR",
  "ativo": true
}
```

---

### 3?? **BUSCAR EMPRESA POR ID**

**Endpoint:** `GET /api/empresas/{id}`

**Como testar:**
1. Localize `GET /api/empresas/{id}`
2. Clique em **"Try it out"**
3. Digite um ID (ex: `100` para Furnas)
4. Clique em **"Execute"**

**IDs interessantes para testar:**
- `100` - Furnas
- `101` - CEMIG
- `102` - COPEL
- `103` - CHESF
- `106` - Itaipu Binacional
- `108` - ENGIE Brasil

---

### 4?? **BUSCAR USINA POR ID**

**Endpoint:** `GET /api/usinas/{id}`

**Como testar:**
1. Localize `GET /api/usinas/{id}`
2. Clique em **"Try it out"**
3. Digite um ID (ex: `200` para Itaipu)
4. Clique em **"Execute"**

**IDs interessantes para testar:**
- `200` - Itaipu (14.000 MW)
- `201` - Belo Monte (11.233 MW)
- `202` - Tucuruí (8.370 MW)
- `203` - Santo Antônio (3.568 MW)
- `204` - Jirau (3.750 MW)

---

### 5?? **FILTRAR USINAS POR EMPRESA**

**Endpoint:** `GET /api/usinas`

**Como testar:**
1. Localize `GET /api/usinas`
2. Clique em **"Try it out"**
3. Em **Parameters**, adicione filtro (se disponível)
4. Ou observe o campo `empresaId` nas respostas

**Empresas com mais usinas:**
- Furnas (ID: 100) - 9 usinas
- Itaipu (ID: 106) - 1 usina (Itaipu)
- ENGIE (ID: 108) - 2 usinas

---

### 6?? **CRIAR NOVA EMPRESA** (Teste de POST)

**Endpoint:** `POST /api/empresas`

**Como testar:**
1. Localize `POST /api/empresas`
2. Clique em **"Try it out"**
3. Cole o JSON de exemplo abaixo
4. Clique em **"Execute"**

**Payload de exemplo:**
```json
{
  "nome": "Teste Energia S.A.",
  "cnpj": "12345678000190",
  "telefone": "(11) 1234-5678",
  "email": "teste@teste.com.br",
  "endereco": "Rua Teste, 123 - São Paulo, SP"
}
```

**Resultado esperado:**
- ? Status: `201 Created`
- ? Empresa criada com ID gerado automaticamente

---

### 7?? **CRIAR NOVA USINA** (Teste de POST)

**Endpoint:** `POST /api/usinas`

**Como testar:**
1. Localize `POST /api/usinas`
2. Clique em **"Try it out"**
3. Cole o JSON de exemplo abaixo
4. Clique em **"Execute"**

**Payload de exemplo:**
```json
{
  "codigo": "UHE-TESTE",
  "nome": "Usina Teste de Exemplo",
  "tipoUsinaId": 1,
  "empresaId": 100,
  "capacidadeInstalada": 500.00,
  "localizacao": "Rio de Janeiro, RJ",
  "dataOperacao": "2024-01-01T00:00:00"
}
```

**Resultado esperado:**
- ? Status: `201 Created`
- ? Usina criada vinculada à Furnas (ID: 100)

---

### 8?? **ATUALIZAR EMPRESA** (Teste de PUT)

**Endpoint:** `PUT /api/empresas/{id}`

**Como testar:**
1. Primeiro, execute `GET /api/empresas/{id}` para obter dados atuais
2. Copie o JSON retornado
3. Localize `PUT /api/empresas/{id}`
4. Clique em **"Try it out"**
5. Cole o JSON e altere algum campo (ex: telefone)
6. Clique em **"Execute"**

**Resultado esperado:**
- ? Status: `204 No Content` (sucesso)
- ? Ao buscar novamente, os dados estarão atualizados

---

### 9?? **DELETAR EMPRESA** (Teste de DELETE)

**Endpoint:** `DELETE /api/empresas/{id}`

?? **CUIDADO:** Este comando remove permanentemente (enquanto o container estiver ativo)

**Como testar:**
1. Crie uma empresa de teste primeiro (POST)
2. Anote o ID retornado
3. Localize `DELETE /api/empresas/{id}`
4. Clique em **"Try it out"**
5. Digite o ID da empresa de teste
6. Clique em **"Execute"**

**Resultado esperado:**
- ? Status: `204 No Content` (sucesso)
- ? Ao buscar novamente, retornará `404 Not Found`

---

### ?? **LISTAR SEMANAS PMO** ? **NOVO**

**Endpoint:** `GET /api/semanaspmo`

**Como testar:**
1. Localize `GET /api/semanaspmo`
2. Clique em **"Try it out"**
3. Clique em **"Execute"**

**Resultado esperado:**
- ? **16 semanas PMO** (3 de seed + 13 reais)
- ? Semanas de Nov/2024 a Jan/2025
- ? Datas de início e fim corretas
- ? Ordenadas por ano e número

**Exemplo de resposta:**
```json
{
  "id": 59,
  "numero": 1,
  "ano": 2025,
  "dataInicio": "2025-01-04",
  "dataFim": "2025-01-10",
  "quantidadeArquivos": 0,
  "ativo": true
}
```

**IDs interessantes para testar:**
- `59` - Semana 1/2025
- `60` - Semana 2/2025
- `61` - Semana 3/2025
- `50` - Semana 44/2024
- `57` - Semana 51/2024 (última do ano)

---

### 1??1?? **BUSCAR SEMANA PMO POR ANO** ? **NOVO**

**Endpoint:** `GET /api/semanaspmo/ano/{ano}`

**Como testar:**
1. Localize `GET /api/semanaspmo/ano/{ano}`
2. Clique em **"Try it out"**
3. Digite `2025`
4. Clique em **"Execute"**

**Resultado esperado:**
- ? Retorna apenas semanas de 2025
- ? Ordenadas por número

---

### 1??2?? **LISTAR EQUIPES PDP** ? **NOVO**

**Endpoint:** `GET /api/equipespdp`

**Como testar:**
1. Localize `GET /api/equipespdp`
2. Clique em **"Try it out"**
3. Clique em **"Execute"**

**Resultado esperado:**
- ? **11 equipes PDP** (5 de seed + 6 reais)
- ? Equipes regionais (Norte, Nordeste, Sudeste, Sul)
- ? Coordenadores e contatos válidos

**Exemplo de resposta:**
```json
{
  "id": 50,
  "nome": "Equipe Norte",
  "descricao": "Responsável pela região Norte do SIN",
  "coordenador": "João Silva Santos",
  "email": "norte@ons.org.br",
  "telefone": "(61) 3429-3000",
  "ativo": true
}
```

**IDs interessantes para testar:**
- `50` - Equipe Norte
- `51` - Equipe Nordeste
- `52` - Equipe Sudeste/Centro-Oeste
- `53` - Equipe Sul
- `54` - Equipe Operação em Tempo Real
- `55` - Equipe Planejamento da Operação

---

### 1??3?? **CRIAR NOVA SEMANA PMO** ? **NOVO**

**Endpoint:** `POST /api/semanaspmo`

**Como testar:**
1. Localize `POST /api/semanaspmo`
2. Clique em **"Try it out"**
3. Cole o JSON de exemplo abaixo
4. Clique em **"Execute"**

**Payload de exemplo:**
```json
{
  "numero": 5,
  "dataInicio": "2025-02-01",
  "dataFim": "2025-02-07",
  "ano": 2025,
  "observacoes": "Semana de teste"
}
```

**Resultado esperado:**
- ? Status: `201 Created`
- ? Semana PMO criada com ID gerado automaticamente

---

### 1??4?? **CRIAR NOVA EQUIPE PDP** ? **NOVO**

**Endpoint:** `POST /api/equipespdp`

**Como testar:**
1. Localize `POST /api/equipespdp`
2. Clique em **"Try it out"**
3. Cole o JSON de exemplo abaixo
4. Clique em **"Execute"**

**Payload de exemplo:**
```json
{
  "nome": "Equipe Teste",
  "descricao": "Equipe de testes do sistema",
  "coordenador": "Coordenador Teste",
  "email": "teste@ons.org.br",
  "telefone": "(61) 3429-9999"
}
```

**Resultado esperado:**
- ? Status: `201 Created`
- ? Equipe PDP criada com ID gerado automaticamente

---

## ?? **VALIDAÇÕES ÚTEIS**

### ? Verificar integridade dos dados:

#### 1. **Contar empresas:**
```bash
GET /api/empresas
```
Resultado esperado: **24 empresas**

#### 2. **Contar usinas:**
```bash
GET /api/usinas
```
Resultado esperado: **40 usinas**

#### 3. **Contar semanas PMO:** ? **NOVO**
```bash
GET /api/semanaspmo
```
Resultado esperado: **16 semanas** (3 seed + 13 reais)

#### 4. **Contar equipes PDP:** ? **NOVO**
```bash
GET /api/equipespdp
```
Resultado esperado: **11 equipes** (5 seed + 6 reais)

#### 5. **Verificar relacionamento Empresa ? Usinas:**
Busque a Furnas:
```bash
GET /api/empresas/100
```
Observe: `"quantidadeUsinas": 9`

#### 6. **Verificar relacionamento Usina ? Empresa:**
Busque Itaipu:
```bash
GET /api/usinas/200
```
Observe: `"empresaId": 106` e `"empresa": "Itaipu Binacional"`

#### 7. **Verificar semana PMO por ano:** ? **NOVO**
Busque semanas de 2025:
```bash
GET /api/semanaspmo/ano/2025
```
Observe: Deve retornar 4 semanas do ano 2025

---

## ?? **CENÁRIOS DE TESTE AVANÇADOS**

### Cenário 1: **Criar usina vinculada a empresa real**
1. POST `/api/usinas` com `empresaId: 100` (Furnas)
2. GET `/api/empresas/100` - verificar que `quantidadeUsinas` aumentou
3. GET `/api/usinas/{novo_id}` - confirmar vínculo

### Cenário 2: **Testar validações**
1. POST `/api/empresas` com CNPJ inválido
2. Resultado esperado: erro de validação
3. POST `/api/usinas` com `tipoUsinaId` inexistente
4. Resultado esperado: erro de chave estrangeira

### Cenário 3: **Testar paginação**
1. GET `/api/usinas?pageNumber=1&pageSize=10`
2. GET `/api/usinas?pageNumber=2&pageSize=10`
3. Verificar que os dados são diferentes

### Cenário 4: **Criar semana PMO e validar conflito** ? **NOVO**
1. POST `/api/semanaspmo` com número e ano únicos (ex: semana 10/2025)
2. Tentar criar outra semana com mesmo número e ano
3. Resultado esperado: erro de duplicação

### Cenário 5: **Criar equipe PDP e validar nome único** ? **NOVO**
1. POST `/api/equipespdp` com nome único (ex: "Equipe Teste XYZ")
2. Tentar criar outra equipe com mesmo nome
3. Resultado esperado: erro de nome duplicado

### Cenário 6: **Buscar semana PMO por data** ? **NOVO**
1. GET `/api/semanaspmo/data/2025-01-20`
2. Resultado esperado: Retorna a semana 3/2025 (18/01 a 24/01)

---

## ?? **DADOS DE REFERÊNCIA**

### Empresas mais relevantes para testes:

| ID | Nome | CNPJ | Usinas |
|----|------|------|--------|
| 100 | Furnas | 23274194000144 | 9 |
| 101 | CEMIG | 17155730000164 | 4 |
| 102 | COPEL | 76483817000120 | 1 |
| 103 | CHESF | 33541368000192 | 3 |
| 106 | Itaipu | 00341657000189 | 2 |
| 108 | ENGIE | 02474103000119 | 2 |

### Usinas mais relevantes para testes:

| ID | Código | Nome | Potência | Empresa |
|----|--------|------|----------|---------|
| 200 | ITAIPU | Itaipu | 14.000 MW | Itaipu (106) |
| 201 | BELOMONTE | Belo Monte | 11.233 MW | Eletronorte (104) |
| 202 | TUCURUI | Tucuruí | 8.370 MW | Eletronorte (104) |
| 217 | FURNAS | Furnas | 1.216 MW | Furnas (100) |

### Semanas PMO mais relevantes para testes: ? **NOVO**

| ID | Semana | Ano | Data Início | Data Fim |
|----|--------|-----|-------------|----------|
| 50 | 44 | 2024 | 02/11/2024 | 08/11/2024 |
| 57 | 51 | 2024 | 21/12/2024 | 27/12/2024 |
| 59 | 1 | 2025 | 04/01/2025 | 10/01/2025 |
| 60 | 2 | 2025 | 11/01/2025 | 17/01/2025 |
| 61 | 3 | 2025 | 18/01/2025 | 24/01/2025 |
| 62 | 4 | 2025 | 25/01/2025 | 31/01/2025 |

### Equipes PDP mais relevantes para testes: ? **NOVO**

| ID | Nome | Coordenador | Email |
|----|------|-------------|-------|
| 50 | Equipe Norte | João Silva Santos | norte@ons.org.br |
| 51 | Equipe Nordeste | Maria Oliveira Costa | nordeste@ons.org.br |
| 52 | Equipe Sudeste/Centro-Oeste | Pedro Almeida Ferreira | sudeste@ons.org.br |
| 53 | Equipe Sul | Ana Paula Rodrigues | sul@ons.org.br |
| 54 | Equipe Operação em Tempo Real | Carlos Eduardo Lima | operacao@ons.org.br |
| 55 | Equipe Planejamento da Operação | Juliana Martins Souza | planejamento@ons.org.br |

---

## ?? **TROUBLESHOOTING**

### Problema: Swagger não carrega
**Solução:**
```bash
docker logs pdpw-backend --tail 50
docker-compose restart
```

### Problema: Não vejo os dados reais
**Solução:**
```bash
docker-compose down
docker-compose up --build -d
```

### Problema: Erro 404 em todos endpoints
**Solução:**
Verifique se o container está rodando:
```bash
docker ps
```

### Problema: Erro ao criar empresa/usina
**Solução:**
Verifique:
- CNPJ tem 14 dígitos
- Email é válido
- TipoUsinaId existe (1 a 6)
- EmpresaId existe

---

## ? **CHECKLIST DE TESTES**

### Básicos:
- [ ] ? GET /api/empresas retorna 24 empresas
- [ ] ? GET /api/usinas retorna 40 usinas
- [ ] ? GET /api/semanaspmo retorna 16 semanas ? **NOVO**
- [ ] ? GET /api/equipespdp retorna 11 equipes ? **NOVO**

### Busca por ID:
- [ ] ? GET /api/empresas/100 retorna Furnas com 9 usinas
- [ ] ? GET /api/usinas/200 retorna Itaipu com 14.000 MW
- [ ] ? GET /api/semanaspmo/59 retorna Semana 1/2025 ? **NOVO**
- [ ] ? GET /api/equipespdp/50 retorna Equipe Norte ? **NOVO**

### Buscas especiais:
- [ ] ? GET /api/semanaspmo/ano/2025 retorna 4 semanas ? **NOVO**
- [ ] ? GET /api/semanaspmo/data/2025-01-20 retorna Semana 3/2025 ? **NOVO**
- [ ] ? GET /api/equipespdp/nome/Equipe Norte retorna ID 50 ? **NOVO**

### CRUD - Create:
- [ ] ? POST /api/empresas cria nova empresa
- [ ] ? POST /api/usinas cria nova usina
- [ ] ? POST /api/semanaspmo cria nova semana ? **NOVO**
- [ ] ? POST /api/equipespdp cria nova equipe ? **NOVO**

### CRUD - Update:
- [ ] ? PUT /api/empresas/{id} atualiza empresa
- [ ] ? PUT /api/usinas/{id} atualiza usina
- [ ] ? PUT /api/semanaspmo/{id} atualiza semana ? **NOVO**
- [ ] ? PUT /api/equipespdp/{id} atualiza equipe ? **NOVO**

### CRUD - Delete:
- [ ] ? DELETE /api/empresas/{id} remove empresa
- [ ] ? DELETE /api/usinas/{id} remove usina
- [ ] ? DELETE /api/semanaspmo/{id} remove semana ? **NOVO**
- [ ] ? DELETE /api/equipespdp/{id} remove equipe ? **NOVO**

### Relacionamentos:
- [ ] ? Relacionamento Empresa ? Usinas funciona
- [ ] ? Validações de campos funcionam
- [ ] ? Validações de duplicação funcionam ? **NOVO**

---

## ?? **PRONTO PARA TESTAR!**

Acesse agora: **http://localhost:5001/swagger**

Todos os **69 registros reais** do cliente estão disponíveis para teste! ??

### ?? **APIs Disponíveis:**

? **Empresas** - 24 registros (8 endpoints)  
? **Usinas** - 40 registros (8 endpoints)  
? **Tipos de Usina** - 6 registros (6 endpoints)  
? **Semanas PMO** - 16 registros (9 endpoints) ? **NOVO**  
? **Equipes PDP** - 11 registros (8 endpoints) ? **NOVO**  
? **Cargas** - 8 endpoints  
? **Arquivos DADGER** - 9 endpoints  
? **Restrições UG** - 9 endpoints  

### ?? **Total de Endpoints Testáveis:**

**66+ endpoints** implementados e funcionando! ??

---

**Última atualização**: 20/12/2024 - 20:00  
**Status**: ? Tudo Funcionando  
**Versão**: POC PDPW V2  

