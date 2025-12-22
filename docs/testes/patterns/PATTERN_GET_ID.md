# ?? PATTERN - GET POR ID

**Pattern:** GET /api/{entidade}/{id}  
**Descri��o:** Retorna registro espec�fico por ID  
**Aplic�vel a:** Todas as APIs

---

## ?? OBJETIVO

Testar endpoint que retorna **um** registro espec�fico.

---

## ?? SWAGGER

### Passos
1. Expandir endpoint `GET /api/{entidade}/{id}`
2. Clicar em **"Try it out"**
3. Preencher par�metro:
   - `id` (integer): **1**
4. Clicar em **"Execute"**

---

## ? RESULTADO ESPERADO

### Response (ID existente)
```json
{
  "id": 1,
  "campo1": "valor1",
  "campo2": "valor2",
  ...
}
```

**Status Code:** **200 OK**

### Response (ID inexistente)
```json
{
  "message": "Recurso n�o encontrado"
}
```

**Status Code:** **404 Not Found**

---

## ? VALIDA��ES

### Cen�rio: ID Existente
- [ ] Status Code: **200 OK**
- [ ] Retorna objeto (n�o array)
- [ ] ID corresponde ao solicitado
- [ ] Todos os campos preenchidos
- [ ] Relacionamentos carregados

### Cen�rio: ID Inexistente
- [ ] Status Code: **404 Not Found**
- [ ] Mensagem de erro descritiva
- [ ] N�o retorna null (retorna 404)

### Cen�rio: ID Inv�lido
- [ ] `id = 0`: **404 Not Found**
- [ ] `id = -1`: **400 Bad Request** ou **404**
- [ ] `id = "abc"`: **400 Bad Request** (valida��o autom�tica)

### Edge Cases
- [ ] Registro inativo (soft delete): **404 Not Found**
- [ ] Registro de outra tenant: **404 Not Found** (se multi-tenant)

---

## ?? PROBLEMAS COMUNS

### Retornar null ao inv�s de 404
```csharp
// ? ERRADO
public async Task<IActionResult> GetById(int id)
{
    var entity = await _service.GetByIdAsync(id);
    return Ok(entity); // Se null, retorna 200 com body null
}

// ? CORRETO
public async Task<IActionResult> GetById(int id)
{
    var entity = await _service.GetByIdAsync(id);
    if (entity == null)
        return NotFound(new { message = $"{EntityName} com ID {id} n�o encontrado" });
    
    return Ok(entity);
}

// ? MELHOR (usando helper)
public async Task<IActionResult> GetById(int id)
{
    var entity = await _service.GetByIdAsync(id);
    return HandleResult(entity); // BaseController helper
}
```

### N+1 Query Problem
```csharp
// ? ERRADO
public async Task<Usina?> GetByIdAsync(int id)
{
    return await _dbSet.FindAsync(id);
    // Lazy loading pode causar N+1
}

// ? CORRETO
public async Task<Usina?> GetByIdAsync(int id)
{
    return await _dbSet
        .Include(u => u.TipoUsina)
        .Include(u => u.Empresa)
        .FirstOrDefaultAsync(u => u.Id == id && u.Ativo);
}
```

### N�o filtrar por Ativo (soft delete)
```csharp
// ? ERRADO - retorna registros inativos
public async Task<Usina?> GetByIdAsync(int id)
{
    return await _dbSet
        .Include(u => u.TipoUsina)
        .FirstOrDefaultAsync(u => u.Id == id);
}

// ? CORRETO
public async Task<Usina?> GetByIdAsync(int id)
{
    return await _dbSet
        .Include(u => u.TipoUsina)
        .FirstOrDefaultAsync(u => u.Id == id && u.Ativo);
}
```

---

## ?? EXEMPLOS POR API

### API Usina
```bash
# Buscar Itaipu (ID 1)
curl http://localhost:5000/api/usinas/1

# Response:
{
  "id": 1,
  "codigo": "UHE-ITAIPU",
  "nome": "Usina Hidrel�trica de Itaipu",
  "tipoUsina": "Hidrel�trica",
  "empresa": "Itaipu Binacional",
  "capacidadeInstalada": 14000.00
}
```

### API TipoUsina
```bash
# Buscar tipo Hidrel�trica (ID 1)
curl http://localhost:5000/api/tiposusina/1

# Response:
{
  "id": 1,
  "nome": "Hidrel�trica",
  "fonteEnergia": "H�drica"
}
```

### API Empresa
```bash
# Buscar Itaipu Binacional (ID 1)
curl http://localhost:5000/api/empresas/1

# Response:
{
  "id": 1,
  "nome": "Itaipu Binacional",
  "cnpj": "00.341.583/0001-71"
}
```

---

## ?? CASOS DE TESTE

### Teste 1: ID V�lido Existente
```
Input: id = 1
Expected: 200 OK + objeto completo
```

### Teste 2: ID V�lido Inexistente
```
Input: id = 999
Expected: 404 Not Found
```

### Teste 3: ID Zero
```
Input: id = 0
Expected: 404 Not Found ou 400 Bad Request
```

### Teste 4: ID Negativo
```
Input: id = -1
Expected: 400 Bad Request ou 404 Not Found
```

### Teste 5: ID String (valida��o autom�tica)
```
Input: id = "abc"
Expected: 400 Bad Request
```

### Teste 6: Registro Inativo (soft delete)
```
Setup: Deletar registro ID 1
Input: id = 1
Expected: 404 Not Found
```

---

## ?? IMPLEMENTA��O RECOMENDADA

### Repository
```csharp
public async Task<T?> GetByIdAsync(int id)
{
    return await _dbSet
        .Where(e => e.Ativo) // Soft delete
        .Include(/* relacionamentos */)
        .FirstOrDefaultAsync(e => e.Id == id);
}
```

### Service
```csharp
public async Task<TDto?> GetByIdAsync(int id)
{
    var entity = await _repository.GetByIdAsync(id);
    return _mapper.Map<TDto?>(entity);
}
```

### Controller
```csharp
[HttpGet("{id:int}", Name = nameof(GetById))]
[ProducesResponseType(typeof(TDto), StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public async Task<IActionResult> GetById(int id)
{
    var entity = await _service.GetByIdAsync(id);
    return HandleResult(entity); // 200 ou 404 automaticamente
}
```

---

## ?? PERFORMANCE

### Benchmarks Esperados
```
< 100ms - Sem relacionamentos
< 200ms - Com 1-2 relacionamentos (Include)
< 300ms - Com 3+ relacionamentos
```

### Otimiza��es
```csharp
// Cache para entidades que mudam pouco
[ResponseCache(Duration = 300)] // 5 minutos
public async Task<IActionResult> GetById(int id)
{
    // ...
}

// AsNoTracking para read-only
public async Task<T?> GetByIdAsync(int id)
{
    return await _dbSet
        .AsNoTracking() // Mais r�pido
        .FirstOrDefaultAsync(e => e.Id == id);
}
```

---

**Aplic�vel a:** 29/29 APIs  
**Implementado em:** Usina ?
