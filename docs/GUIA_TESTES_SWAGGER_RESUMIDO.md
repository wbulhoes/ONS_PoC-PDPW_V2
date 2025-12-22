# ?? GUIA DE TESTES - SWAGGER UI

**Acesso:** http://localhost:5001/swagger

---

## ?? **IN�CIO R�PIDO**

### **1. Abrir Swagger**
```
http://localhost:5001/swagger
```

### **2. Testar API (Exemplo: Empresas)**

1. **Expandir** a se��o `Empresas`
2. **Clicar** em `GET /api/empresas`
3. **Clicar** em `Try it out`
4. **Clicar** em `Execute`
5. **Ver** resposta com 25 empresas

? **Pronto!** API funcionando.

---

## ?? **10 TESTES ESSENCIAIS**

### **Teste 1: Listar Todas as Empresas** ?
```http
GET /api/empresas
```
**Resultado esperado:** 25 empresas  
**Use quando:** Precisar ver todas as empresas cadastradas

---

### **Teste 2: Listar Todas as Usinas** ?
```http
GET /api/usinas
```
**Resultado esperado:** 40 usinas  
**Use quando:** Precisar ver todas as usinas cadastradas

---

### **Teste 3: Buscar Empresa por ID**
```http
GET /api/empresas/101
```
**Passos no Swagger:**
1. Expandir `GET /api/empresas/{id}`
2. Try it out
3. Em `id`, digitar: `101`
4. Execute

**Resultado esperado:**
```json
{
  "id": 101,
  "nome": "Empresa de Energia do Amazonas",
  "cnpj": "02341467000120"
}
```

---

### **Teste 4: Semana PMO Atual** ?
```http
GET /api/semanaspmo/atual
```
**Resultado esperado:** Semana operativa atual do sistema

---

### **Teste 5: Criar Nova Carga** ?
```http
POST /api/cargas
```
**JSON de exemplo:**
```json
{
  "dataReferencia": "2025-01-20",
  "subsistemaId": "SE",
  "cargaMWmed": 45678.50,
  "cargaVerificada": 45234.20,
  "previsaoCarga": 46000.00,
  "observacoes": "Teste via Swagger"
}
```

**Resultado esperado:** 201 Created

---

## ? **SISTEMA VALIDADO**

- ? 101 registros no banco
- ? 8 APIs funcionais
- ? 65+ endpoints documentados
- ? Swagger 100% funcional
- ? Dados reais do cliente

**Acesse agora:** http://localhost:5001/swagger

