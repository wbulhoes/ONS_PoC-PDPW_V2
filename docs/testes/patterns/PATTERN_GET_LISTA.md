# ?? PATTERN - GET LISTA COMPLETA

**Pattern:** GET /api/{entidade}  
**Descrição:** Retorna lista completa de registros  
**Aplicável a:** Todas as APIs

---

## ?? OBJETIVO

Testar endpoint que retorna **todos** os registros de uma entidade.

---

## ?? SWAGGER

### Passos
1. Expandir endpoint `GET /api/{entidade}`
2. Clicar em **"Try it out"**
3. (Não há parâmetros geralmente)
4. Clicar em **"Execute"**

---

## ? RESULTADO ESPERADO

### Response
```json
[
  {
    "id": 1,
    "campo1": "valor1",
    "campo2": "valor2",
    ...
  },
  {
    "id": 2,
    "campo1": "valor1",
    "campo2": "valor2",
    ...
  }
]
```

### Status Code
- **200 OK** - Lista retornada (pode estar vazia)

---

## ? VALIDAÇÕES

### Básicas
- [ ] Status Code: **200 OK**
- [ ] Response é um array `[...]`
- [ ] Cada item tem todos os campos esperados
- [ ] IDs são únicos

### Relacionamentos
- [ ] Campos de FK são resolvidos (ex: `tipoUsina` ao invés de `tipoUsinaId`)
- [ ] Não traz objetos completos aninhados (só nomes/códigos)

### Performance
- [ ] < 500ms para até 100 registros
- [ ] < 1000ms para até 1000 registros
- [ ] Paginação implementada (se > 1000 registros)

### Edge Cases
- [ ] Lista vazia retorna `[]` e não erro
- [ ] Registros inativos NÃO aparecem (se soft delete)
- [ ] Ordenação padrão funciona (geralmente por nome ou data)

---

## ?? PROBLEMAS COMUNS

### N+1 Query Problem
```csharp
// ? ERRADO - N+1 queries
public async Task<List<Usina>> GetAllAsync()
{
    return await _dbSet.ToListAsync();
    // Lazy loading vai buscar TipoUsina depois
}

// ? CORRETO - Eager loading
public async Task<List<Usina>> GetAllAsync()
{
    return await _dbSet
        .Include(u => u.TipoUsina)
        .Include(u => u.Empresa)
        .ToListAsync();
}
```

### Lista muito grande
```csharp
// ? ERRADO - Pode retornar 10.000 registros
public async Task<List<T>> GetAllAsync()
{
    return await _dbSet.ToListAsync();
}

// ? CORRETO - Com paginação
public async Task<List<T>> GetAllAsync(int page = 1, int pageSize = 50)
{
    return await _dbSet
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
}
```

---

## ?? EXEMPLOS POR API

### API Usina
```bash
curl http://localhost:5000/api/usinas
# Retorna: 10 usinas com TipoUsina e Empresa
```

### API TipoUsina
```bash
curl http://localhost:5000/api/tiposusina
# Retorna: 5 tipos (Hidrelétrica, Térmica, etc)
```

### API Empresa
```bash
curl http://localhost:5000/api/empresas
# Retorna: 8 empresas
```

---

## ?? QUANDO USAR

? **Use este pattern quando:**
- Quantidade de registros é pequena (< 1000)
- Frontend precisa de lista completa (ex: dropdowns)
- Dados mudam pouco (podem ser cacheados)

? **NÃO use quando:**
- Muitos registros (> 1000)
- Dados grandes (ex: com BLOBs)
- Atualizações frequentes

**Alternativa:** Implementar paginação, filtros ou busca

---

**Aplicável a:** 29/29 APIs  
**Implementado em:** Usina ?
