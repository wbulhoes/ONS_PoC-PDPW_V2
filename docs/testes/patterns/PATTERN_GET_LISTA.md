# ?? PATTERN - GET LISTA COMPLETA

**Pattern:** GET /api/{entidade}  
**Descri��o:** Retorna lista completa de registros  
**Aplic�vel a:** Todas as APIs

---

## ?? OBJETIVO

Testar endpoint que retorna **todos** os registros de uma entidade.

---

## ?? SWAGGER

### Passos
1. Expandir endpoint `GET /api/{entidade}`
2. Clicar em **"Try it out"**
3. (N�o h� par�metros geralmente)
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

## ? VALIDA��ES

### B�sicas
- [ ] Status Code: **200 OK**
- [ ] Response � um array `[...]`
- [ ] Cada item tem todos os campos esperados
- [ ] IDs s�o �nicos

### Relacionamentos
- [ ] Campos de FK s�o resolvidos (ex: `tipoUsina` ao inv�s de `tipoUsinaId`)
- [ ] N�o traz objetos completos aninhados (s� nomes/c�digos)

### Performance
- [ ] < 500ms para at� 100 registros
- [ ] < 1000ms para at� 1000 registros
- [ ] Pagina��o implementada (se > 1000 registros)

### Edge Cases
- [ ] Lista vazia retorna `[]` e n�o erro
- [ ] Registros inativos N�O aparecem (se soft delete)
- [ ] Ordena��o padr�o funciona (geralmente por nome ou data)

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

// ? CORRETO - Com pagina��o
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
# Retorna: 5 tipos (Hidrel�trica, T�rmica, etc)
```

### API Empresa
```bash
curl http://localhost:5000/api/empresas
# Retorna: 8 empresas
```

---

## ?? QUANDO USAR

? **Use este pattern quando:**
- Quantidade de registros � pequena (< 1000)
- Frontend precisa de lista completa (ex: dropdowns)
- Dados mudam pouco (podem ser cacheados)

? **N�O use quando:**
- Muitos registros (> 1000)
- Dados grandes (ex: com BLOBs)
- Atualiza��es frequentes

**Alternativa:** Implementar pagina��o, filtros ou busca

---

**Aplic�vel a:** 29/29 APIs  
**Implementado em:** Usina ?
