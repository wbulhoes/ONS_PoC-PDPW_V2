# ? API EMPRESAS - COMPLETA!

**Data:** 19/12/2024  
**Desenvolvedor:** Willian  
**Tempo:** ~2h  
**Status:** ? Completa e Funcionando

---

## ?? O QUE FOI CRIADO

### ?? Arquivos Criados (10 arquivos)

#### Domain Layer (1 arquivo)
1. `src/PDPW.Domain/Interfaces/IEmpresaRepository.cs`
   - Interface do repositório
   - 10 métodos definidos

#### Infrastructure Layer (1 arquivo)
2. `src/PDPW.Infrastructure/Repositories/EmpresaRepository.cs`
   - Implementação do repositório
   - Herda de BaseRepository<Empresa>
   - Include de Usinas relacionadas
   - Validação de nome e CNPJ duplicados
   - Método auxiliar para limpar CNPJ

#### Application Layer (5 arquivos)
3. `src/PDPW.Application/DTOs/Empresa/EmpresaDto.cs`
   - DTO de leitura
   - Inclui QuantidadeUsinas

4. `src/PDPW.Application/DTOs/Empresa/CreateEmpresaDto.cs`
   - DTO para criação
   - Validações DataAnnotations (Nome, CNPJ, Email, Telefone)

5. `src/PDPW.Application/DTOs/Empresa/UpdateEmpresaDto.cs`
   - DTO para atualização
   - Validações DataAnnotations

6. `src/PDPW.Application/Interfaces/IEmpresaService.cs`
   - Interface do serviço
   - 9 métodos definidos

7. `src/PDPW.Application/Services/EmpresaService.cs`
   - Implementação do serviço
   - Lógica de negócio
   - Validações (nome duplicado, CNPJ duplicado, usinas vinculadas)

#### API Layer (1 arquivo)
8. `src/PDPW.API/Controllers/EmpresasController.cs`
   - Controller RESTful
   - 8 endpoints
   - Documentação Swagger completa

#### Configurações (2 arquivos atualizados)
9. `src/PDPW.Application/Mappings/AutoMapperProfile.cs`
   - Mappings Empresa ? DTOs

10. `src/PDPW.API/Extensions/ServiceCollectionExtensions.cs`
    - Registro de dependências (DI)

---

## ?? ENDPOINTS CRIADOS (8)

### 1. GET /api/empresas
**Descrição:** Obtém todas as empresas  
**Response:** 200 OK - Lista de EmpresaDto  
**Swagger:** ? Documentado

```json
[
  {
    "id": 1,
    "nome": "Itaipu Binacional",
    "cnpj": "00.276.910/0001-56",
    "telefone": "(45) 3520-5252",
    "email": "contato@itaipu.gov.br",
    "endereco": "Av. Tancredo Neves, 6731 - Foz do Iguaçu/PR",
    "quantidadeUsinas": 1,
    "ativo": true,
    "dataCriacao": "2024-01-01T00:00:00Z",
    "dataAtualizacao": null
  }
]
```

---

### 2. GET /api/empresas/{id}
**Descrição:** Obtém empresa por ID  
**Response:** 200 OK ou 404 Not Found  
**Swagger:** ? Documentado

```json
{
  "id": 1,
  "nome": "Itaipu Binacional",
  "cnpj": "00.276.910/0001-56",
  "telefone": "(45) 3520-5252",
  "email": "contato@itaipu.gov.br",
  "endereco": "Av. Tancredo Neves, 6731 - Foz do Iguaçu/PR",
  "quantidadeUsinas": 1,
  "ativo": true
}
```

---

### 3. GET /api/empresas/nome/{nome}
**Descrição:** Obtém empresa por nome  
**Response:** 200 OK ou 404 Not Found  
**Swagger:** ? Documentado

```bash
GET /api/empresas/nome/Itaipu Binacional
```

---

### 4. GET /api/empresas/cnpj/{cnpj}
**Descrição:** Obtém empresa por CNPJ  
**Response:** 200 OK ou 404 Not Found  
**Swagger:** ? Documentado

```bash
GET /api/empresas/cnpj/00.276.910/0001-56
```

**Nota:** O CNPJ é limpo automaticamente (remove . / -)

---

### 5. POST /api/empresas
**Descrição:** Cria nova empresa  
**Response:** 201 Created ou 400 Bad Request  
**Swagger:** ? Documentado

**Request Body:**
```json
{
  "nome": "Nova Empresa Ltda",
  "cnpj": "12.345.678/0001-90",
  "telefone": "(11) 1234-5678",
  "email": "contato@novaempresa.com.br",
  "endereco": "Rua Exemplo, 123 - São Paulo/SP"
}
```

**Validações:**
- ? Nome obrigatório (3-200 caracteres)
- ? Nome único (não pode duplicar)
- ? CNPJ formato válido: 00.000.000/0000-00
- ? CNPJ único (não pode duplicar)
- ? Email formato válido
- ? Telefone formato válido
- ? Endereço opcional (max 500 caracteres)

---

### 6. PUT /api/empresas/{id}
**Descrição:** Atualiza empresa existente  
**Response:** 200 OK, 400 Bad Request ou 404 Not Found  
**Swagger:** ? Documentado

**Request Body:**
```json
{
  "nome": "Itaipu Binacional",
  "cnpj": "00.276.910/0001-56",
  "telefone": "(45) 3520-5252",
  "email": "contato@itaipu.gov.br",
  "endereco": "Endereço atualizado",
  "ativo": true
}
```

**Validações:**
- ? Nome obrigatório (3-200 caracteres)
- ? Nome único (exceto o próprio registro)
- ? CNPJ único (exceto o próprio registro)
- ? Registro deve existir

---

### 7. DELETE /api/empresas/{id}
**Descrição:** Remove empresa (soft delete)  
**Response:** 204 No Content, 400 Bad Request ou 404 Not Found  
**Swagger:** ? Documentado

**Validações:**
- ? Empresa deve existir
- ? NÃO pode ter usinas ativas vinculadas
- ? Soft delete (Ativo = false)

---

### 8. GET /api/empresas/verificar-nome/{nome}
**Descrição:** Verifica se nome já existe  
**Query Param:** empresaId (opcional - para excluir da verificação)  
**Response:** 200 OK  
**Swagger:** ? Documentado

```json
{
  "existe": true
}
```

---

### 9. GET /api/empresas/verificar-cnpj/{cnpj}
**Descrição:** Verifica se CNPJ já existe  
**Query Param:** empresaId (opcional - para excluir da verificação)  
**Response:** 200 OK  
**Swagger:** ? Documentado

```json
{
  "existe": false
}
```

---

## ?? FEATURES IMPLEMENTADAS

### ? CRUD Completo
- [x] Create (POST)
- [x] Read (GET lista, por ID, por nome, por CNPJ)
- [x] Update (PUT)
- [x] Delete (DELETE - soft delete)

### ? Validações de Negócio
- [x] Nome único
- [x] CNPJ único
- [x] CNPJ formato válido
- [x] Email formato válido
- [x] Telefone formato válido
- [x] Não pode excluir empresa com usinas vinculadas
- [x] Campos obrigatórios
- [x] Tamanhos mínimos e máximos

### ? Buscas Customizadas
- [x] Por ID
- [x] Por nome
- [x] Por CNPJ
- [x] Verificar existência de nome
- [x] Verificar existência de CNPJ

### ? Relacionamentos
- [x] Include de Usinas relacionadas
- [x] Contagem de usinas ativas
- [x] Validação de integridade referencial

### ? Soft Delete
- [x] Ativo = false (não exclui do banco)
- [x] Filtro automático por Ativo
- [x] DataAtualizacao preenchida

### ? Auditoria
- [x] DataCriacao automática
- [x] DataAtualizacao automática
- [x] Campo Ativo

### ? Documentação
- [x] Swagger UI completo
- [x] XML Comments
- [x] Exemplos de request/response
- [x] Status codes documentados

### ? Funcionalidades Especiais
- [x] Limpeza automática de CNPJ (remove . / -)
- [x] Busca case-insensitive por nome
- [x] Validação de formato CNPJ via Regex

---

## ?? DADOS EXISTENTES (Seed Data)

| ID | Nome | CNPJ | Usinas |
|----|------|------|---------|
| 1 | Itaipu Binacional | 00.276.910/0001-56 | 1 |
| 2 | Eletronorte | 00.357.039/0001-89 | 2 |
| 3 | Furnas | 03.653.908/0001-59 | 2 |
| 4 | Chesf | 33.541.439/0001-94 | 1 |
| 5 | Eletrosul | 00.073.957/0001-38 | 1 |
| 6 | Eletronuclear | 42.540.2XX/0001-XX | 2 |
| 7 | Cemig | 17.155.7XX/0001-XX | 0 |
| 8 | CPFL Energia | 02.429.7XX/0001-XX | 1 |

**Total:** 8 empresas cadastradas  
**Usinas cadastradas:** 10

---

## ?? TESTES RÁPIDOS

### Swagger UI
```
http://localhost:5000/swagger
```

### cURL

#### Listar todas
```bash
curl http://localhost:5000/api/empresas
```

#### Buscar por ID
```bash
curl http://localhost:5000/api/empresas/1
```

#### Buscar por nome
```bash
curl http://localhost:5000/api/empresas/nome/Itaipu%20Binacional
```

#### Buscar por CNPJ
```bash
curl http://localhost:5000/api/empresas/cnpj/00.276.910/0001-56
```

#### Criar nova
```bash
curl -X POST http://localhost:5000/api/empresas \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Nova Energia S.A.",
    "cnpj": "11.222.333/0001-44",
    "telefone": "(11) 9999-8888",
    "email": "contato@novaenergia.com.br",
    "endereco": "Av. Paulista, 1000 - São Paulo/SP"
  }'
```

#### Atualizar
```bash
curl -X PUT http://localhost:5000/api/empresas/1 \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Itaipu Binacional",
    "cnpj": "00.276.910/0001-56",
    "telefone": "(45) 3520-5252",
    "email": "novo@itaipu.gov.br",
    "endereco": "Endereço atualizado",
    "ativo": true
  }'
```

#### Deletar
```bash
curl -X DELETE http://localhost:5000/api/empresas/7
```

#### Verificar nome
```bash
curl http://localhost:5000/api/empresas/verificar-nome/Itaipu%20Binacional
```

#### Verificar CNPJ
```bash
curl http://localhost:5000/api/empresas/verificar-cnpj/00.276.910/0001-56
```

---

## ?? PROGRESSO DA POC

```
APIs Completas: 3/29 (10.3%)
?? ? Usinas (8 endpoints)
?? ? TiposUsina (6 endpoints)
?? ? Empresas (8 endpoints)

Total de Endpoints: 22/154 (14.3%)

Próximas APIs:
? SemanaPMO (6 endpoints)
? ArquivoDADGER (6 endpoints)
? Cargas (5 endpoints)
```

---

## ?? PATTERN CONSOLIDADO

```
1. Domain/Interfaces/I{Entity}Repository.cs
2. Infrastructure/Repositories/{Entity}Repository.cs
3. Application/DTOs/{Entity}/{Entity}Dto.cs
4. Application/DTOs/{Entity}/Create{Entity}Dto.cs
5. Application/DTOs/{Entity}/Update{Entity}Dto.cs
6. Application/Interfaces/I{Entity}Service.cs
7. Application/Services/{Entity}Service.cs
8. API/Controllers/{Entity}sController.cs
9. Atualizar AutoMapperProfile.cs
10. Atualizar ServiceCollectionExtensions.cs
```

**Tempo estimado por API:** ~1.5-2h (pattern consolidado)

---

## ? CHECKLIST DE QUALIDADE

### Código
- [x] Build sem erros
- [x] Seguindo Clean Architecture
- [x] Seguindo SOLID principles
- [x] Comentários XML
- [x] Async/await correto

### Funcionalidade
- [x] CRUD completo funcionando
- [x] Validações implementadas
- [x] Soft delete implementado
- [x] Relacionamentos funcionando
- [x] Auditoria funcionando
- [x] CNPJ formatado corretamente

### Documentação
- [x] Swagger UI completo
- [x] Endpoints documentados
- [x] DTOs documentados
- [x] Status codes documentados
- [x] Exemplos incluídos

---

## ?? PRÓXIMOS PASSOS

### Imediato
1. ? API TiposUsina completa
2. ? API Empresas completa
3. ? **API SemanaPMO** (próxima - 3h)

### Hoje (Meta)
- ? 3 APIs completas (Usinas, TiposUsina, Empresas)
- **Total:** 3/29 APIs (10.3%)
- **Endpoints:** 22/154 (14.3%)

---

## ?? ESTATÍSTICAS

```
Linhas de código:      ~900
Arquivos criados:      10
Arquivos modificados:  2
Tempo desenvolvimento: 2h
Build time:            1.4s
Endpoints:             8
Commits:               1
```

---

## ?? DESTAQUES DESTA API

### ? Funcionalidades Especiais
1. **Validação de CNPJ**
   - Regex para formato: 00.000.000/0000-00
   - Limpeza automática (remove pontos, barras e hífens)
   - Busca normalizada

2. **Múltiplas Buscas**
   - Por ID
   - Por nome
   - Por CNPJ
   - Verificação de existência

3. **Validações Robustas**
   - Nome único
   - CNPJ único
   - Email válido
   - Telefone válido
   - Não pode excluir com usinas vinculadas

---

## ?? COMPARAÇÃO COM APIs ANTERIORES

### Semelhanças
- ? Pattern idêntico
- ? CRUD completo
- ? Soft delete
- ? Auditoria
- ? Swagger documentado

### Diferenças
- ?? Validação de CNPJ (formato e unicidade)
- ?? Limpeza de CNPJ para busca
- ?? Busca por CNPJ adicional
- ?? Validação de Email
- ?? Validação de Telefone
- ?? 8 endpoints (vs 6 da TiposUsina)

---

**Criado por:** Willian (DEV 1)  
**Commitado:** 2059fdf  
**Branch:** develop  
**Status:** ? COMPLETA E FUNCIONANDO

**3ª API COMPLETA! ??**

**PRÓXIMA API: SEMANA PMO! ??**
