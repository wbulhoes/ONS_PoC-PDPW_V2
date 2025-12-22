# ?? GUIA DE CORRE��O - TESTE DE APIs

## ? **ERRO IDENTIFICADO**

**Erro:** 500 Internal Server Error  
**Causa:** Foreign Key constraint violation  
**Tabela:** ArquivosDadger  
**Campo:** semanaPMOId  
**Problema:** Voc� usou `semanaPMOId: 0`, que n�o existe no banco

---

## ? **SOLU��O**

### **IDs V�lidos de SemanaPMO**

Consulte sempre os IDs dispon�veis antes de criar registros relacionados:

```sql
SELECT Id, Numero, Ano, DataInicio, DataFim 
FROM SemanasPMO 
WHERE Ativo = 1 
ORDER BY DataInicio DESC;
```

**IDs dispon�veis atualmente:** 1 a 69

**Semanas mais recentes:**
- ID **69**: Semana 11/2025 (15/03 a 21/03)
- ID **68**: Semana 10/2025 (08/03 a 14/03)
- ID **67**: Semana 9/2025 (01/03 a 07/03)
- ID **3**: Semana 3/2025 (18/01 a 24/01) ? **Semana atual**
- ID **1**: Semana 1/2025 (04/01 a 10/01)

---

## ?? **REQUEST CORRIGIDO - ArquivosDadger**

### **? Errado (seu teste):**
```json
{
  "nomeArquivo": "TESTE WILL",
  "caminhoArquivo": "string",
  "dataImportacao": "2025-12-21T01:03:49.698Z",
  "semanaPMOId": 0,  ? ERRO: ID 0 n�o existe
  "observacoes": "string"
}
```

### **? Correto:**
```json
{
  "nomeArquivo": "TESTE_WILL_CORRIGIDO",
  "caminhoArquivo": "/uploads/2025/12/TESTE_WILL_CORRIGIDO.dat",
  "dataImportacao": "2025-12-21T03:49:00.000Z",
  "semanaPMOId": 69,  ? CORRETO: ID v�lido
  "observacoes": "Teste via Swagger - Corrigido"
}
```

**Response esperado (201 Created):**
```json
{
  "id": 16,
  "nomeArquivo": "TESTE_WILL_CORRIGIDO",
  "caminhoArquivo": "/uploads/2025/12/TESTE_WILL_CORRIGIDO.dat",
  "dataImportacao": "2025-12-21T03:49:00Z",
  "semanaPMOId": 69,
  "semanaPMO": "Semana Operativa 11/2025",
  "observacoes": "Teste via Swagger - Corrigido",
  "processado": false,
  "dataProcessamento": null,
  "ativo": true,
  "dataCriacao": "2025-12-21T01:00:00Z"
}
```

---

## ?? **IDs V�LIDOS PARA OUTRAS APIs**

### **1. Empresas (IDs: 1-8, 101-117)**
```json
POST /api/empresas
{
  "nome": "Teste Empresa",
  "cnpj": "12345678000199",
  "telefone": "(11) 9999-9999",
  "email": "teste@empresa.com"
}
```

### **2. Usinas (IDs: 1-10, 201-230)**

**Para criar nova usina, use IDs v�lidos:**
```json
POST /api/usinas
{
  "codigo": "TESTE",
  "nome": "Usina Teste",
  "tipoUsinaId": 1,        ? IDs v�lidos: 1-8
  "empresaId": 101,        ? IDs v�lidos: 1-8, 101-117
  "capacidadeInstalada": 100.00,
  "localizacao": "S�o Paulo, SP",
  "dataOperacao": "2025-01-01"
}
```

### **3. Cargas (IDs: 1-20)**

```json
POST /api/cargas
{
  "dataReferencia": "2025-12-21",
  "subsistemaId": "SE",    ? Valores v�lidos: SE, S, NE, N
  "cargaMWmed": 45000.00,
  "cargaVerificada": 44500.00,
  "previsaoCarga": 46000.00,
  "observacoes": "Teste via Swagger"
}
```

### **4. RestricoesUG (IDs: 1-25)**

**Para criar nova restri��o:**
```json
POST /api/restricoesug
{
  "unidadeGeradoraId": 1,     ? IDs v�lidos: 1-30
  "dataInicio": "2025-12-21",
  "dataFim": "2025-12-25",
  "motivoRestricaoId": 1,     ? IDs v�lidos: 1-10
  "potenciaRestrita": 350.00,
  "observacoes": "Teste de restri��o"
}
```

**IDs de UnidadesGeradoras dispon�veis:**
- 1-3: Itaipu (G01, G02, G03)
- 4-5: Belo Monte (G01, G02)
- 6-8: Tucuru� (G01, G02, G03)
- 9-10: Jirau (G01, G02)
- 11-12: Santo Ant�nio (G01, G02)
- 13-30: Outras usinas

**IDs de MotivosRestricao dispon�veis:**
- 1: Manuten��o Preventiva
- 2: Manuten��o Corretiva
- 3: Falha de Equipamento
- 4: Restri��o Hidr�ulica
- 5: Restri��o de Transmiss�o
- 6: Restri��o Ambiental
- 7: Restri��o de Combust�vel
- 8: Teste/Comissionamento
- 9: Desligamento Total
- 10: Restri��o Clim�tica

---

## ?? **COMO CONSULTAR IDs V�LIDOS**

### **Via SQL (Docker)**

```powershell
# Semanas PMO
docker exec pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong" -C -d PDPW_DB -Q "SELECT Id, Numero, Ano FROM SemanasPMO WHERE Ativo = 1"

# Empresas
docker exec pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong" -C -d PDPW_DB -Q "SELECT Id, Nome FROM Empresas WHERE Ativo = 1"

# Tipos de Usina
docker exec pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong" -C -d PDPW_DB -Q "SELECT Id, Nome FROM TiposUsina WHERE Ativo = 1"

# Unidades Geradoras
docker exec pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong" -C -d PDPW_DB -Q "SELECT Id, Codigo, Nome FROM UnidadesGeradoras WHERE Ativo = 1"

# Motivos Restri��o
docker exec pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong" -C -d PDPW_DB -Q "SELECT Id, Nome, Categoria FROM MotivosRestricao WHERE Ativo = 1"
```

### **Via APIs (Swagger/cURL)**

```bash
# Listar Semanas PMO
curl http://localhost:5001/api/semanaspmo

# Listar Empresas
curl http://localhost:5001/api/empresas

# Listar Tipos Usina
curl http://localhost:5001/api/tiposusina

# Listar Unidades Geradoras (via RestricoesUG)
# N�o h� endpoint direto, mas pode consultar via SQL
```

---

## ?? **TABELA DE REFER�NCIA R�PIDA**

| Entidade | Range de IDs | Total | Endpoint GET |
|----------|--------------|-------|--------------|
| **Empresas** | 1-8, 101-117 | 25 | `/api/empresas` |
| **Usinas** | 1-10, 201-230 | 40 | `/api/usinas` |
| **TiposUsina** | 1-8 | 8 | `/api/tiposusina` |
| **SemanasPMO** | 1-69 | 20 | `/api/semanaspmo` |
| **EquipesPDP** | 1-8, 51-53 | 8 | `/api/equipespdp` |
| **Cargas** | 1-20 | 20 | `/api/cargas` |
| **ArquivosDadger** | 1-15 | 15 | `/api/arquivosdadger` |
| **UnidadesGeradoras** | 1-30 | 30 | N/A (SQL direto) |
| **MotivosRestricao** | 1-10 | 10 | N/A (SQL direto) |
| **RestricoesUG** | 1-25 | 25 | `/api/restricoesug` |

---

## ?? **VALIDA��ES IMPORTANTES**

### **ArquivosDadger**
- ? `nomeArquivo`: Obrigat�rio, �nico
- ? `caminhoArquivo`: Obrigat�rio
- ? `dataImportacao`: Obrigat�rio, formato ISO 8601
- ? `semanaPMOId`: **DEVE EXISTIR** na tabela SemanasPMO (IDs 1-69)
- ? `observacoes`: Opcional

### **Cargas**
- ? `subsistemaId`: Apenas "SE", "S", "NE", "N"
- ? `cargaMWmed`: Maior que zero
- ? `dataReferencia`: Data v�lida

### **RestricoesUG**
- ? `unidadeGeradoraId`: IDs 1-30
- ? `motivoRestricaoId`: IDs 1-10
- ? `dataInicio` < `dataFim`
- ? `potenciaRestrita`: Maior que zero

---

## ?? **TESTE VALIDADO - PASSO A PASSO**

### **1. Verifique IDs dispon�veis**
```bash
curl http://localhost:5001/api/semanaspmo
```

### **2. Use um ID v�lido no POST**
```json
{
  "nomeArquivo": "MEU_TESTE",
  "caminhoArquivo": "/uploads/2025/12/MEU_TESTE.dat",
  "dataImportacao": "2025-12-21T10:00:00.000Z",
  "semanaPMOId": 3,  ? Semana atual (3/2025)
  "observacoes": "Teste corrigido"
}
```

### **3. Execute o POST**
- Abra Swagger: http://localhost:5001/swagger
- V� em `ArquivosDadger`
- Expanda `POST /api/ArquivosDadger`
- Clique em `Try it out`
- Cole o JSON corrigido
- Clique em `Execute`

### **4. Valide o resultado**
- C�digo: **201 Created** ?
- Response cont�m o ID do novo registro
- `processado: false` (padr�o)
- `ativo: true` (padr�o)

---

## ?? **EXEMPLO COMPLETO - SUCESSO**

**Request:**
```http
POST /api/ArquivosDadger
Content-Type: application/json

{
  "nomeArquivo": "dadger_202512_sem03.dat",
  "caminhoArquivo": "/uploads/2025/12/dadger_202512_sem03.dat",
  "dataImportacao": "2025-12-21T08:00:00.000Z",
  "semanaPMOId": 3,
  "observacoes": "Importado via Swagger - Teste Will"
}
```

**Response (201 Created):**
```json
{
  "id": 16,
  "nomeArquivo": "dadger_202512_sem03.dat",
  "caminhoArquivo": "/uploads/2025/12/dadger_202512_sem03.dat",
  "dataImportacao": "2025-12-21T08:00:00Z",
  "semanaPMOId": 3,
  "semanaPMO": "Semana Operativa 3/2025",
  "observacoes": "Importado via Swagger - Teste Will",
  "processado": false,
  "dataProcessamento": null,
  "ativo": true,
  "dataCriacao": "2025-12-21T01:15:30.123456Z"
}
```

---

## ? **CHECKLIST DE TESTE**

Antes de fazer qualquer POST:

- [ ] Consultei os IDs dispon�veis via GET
- [ ] Usei um ID v�lido que existe no banco
- [ ] Validei o formato dos dados (datas, n�meros, strings)
- [ ] Testei primeiro com dados simples
- [ ] Verifiquei o response code (201 = sucesso, 400/500 = erro)

---

## ?? **SE O ERRO PERSISTIR**

1. **Verifique o log do backend:**
```powershell
docker logs pdpw-backend --tail 50
```

2. **Consulte o banco diretamente:**
```powershell
docker exec pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong" -C -d PDPW_DB -Q "SELECT * FROM SemanasPMO WHERE Id = 3"
```

3. **Teste via cURL primeiro:**
```bash
curl -X POST "http://localhost:5001/api/ArquivosDadger" \
  -H "Content-Type: application/json" \
  -d @test-dadger.json
```

---

**Agora voc� pode testar novamente com sucesso! ??**

Use `semanaPMOId: 3` (semana atual) ou qualquer ID de 1 a 69.
