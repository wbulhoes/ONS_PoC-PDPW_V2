# ?? TESTES RÁPIDOS - API USINA (CURL)

**Para usar quando a API estiver rodando**

---

## ?? COMANDOS CURL

### 1. Listar todas as usinas
```bash
curl -X GET "http://localhost:5000/api/usinas" -H "accept: application/json"
```

### 2. Buscar Itaipu (ID 1)
```bash
curl -X GET "http://localhost:5000/api/usinas/1" -H "accept: application/json"
```

### 3. Buscar por código (Belo Monte)
```bash
curl -X GET "http://localhost:5000/api/usinas/codigo/UHE-BELO-MONTE" -H "accept: application/json"
```

### 4. Listar hidrelétricas (tipo 1)
```bash
curl -X GET "http://localhost:5000/api/usinas/tipo/1" -H "accept: application/json"
```

### 5. Listar usinas da Eletronorte (empresa 2)
```bash
curl -X GET "http://localhost:5000/api/usinas/empresa/2" -H "accept: application/json"
```

### 6. Criar nova usina (Jirau)
```bash
curl -X POST "http://localhost:5000/api/usinas" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d "{\"codigo\":\"UHE-JIRAU\",\"nome\":\"Usina Hidrelétrica de Jirau\",\"tipoUsinaId\":1,\"empresaId\":3,\"capacidadeInstalada\":3750,\"localizacao\":\"Porto Velho, RO\",\"dataOperacao\":\"2013-09-01\",\"ativo\":true}"
```

### 7. Atualizar usina (ID 11)
```bash
curl -X PUT "http://localhost:5000/api/usinas/11" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d "{\"codigo\":\"UHE-JIRAU\",\"nome\":\"UH Jirau Atualizada\",\"tipoUsinaId\":1,\"empresaId\":3,\"capacidadeInstalada\":3750,\"localizacao\":\"Porto Velho, RO\",\"dataOperacao\":\"2013-09-01\",\"ativo\":true}"
```

### 8. Deletar usina (ID 11)
```bash
curl -X DELETE "http://localhost:5000/api/usinas/11" -H "accept: application/json"
```

### 9. Verificar se código existe
```bash
curl -X GET "http://localhost:5000/api/usinas/verificar-codigo/UHE-ITAIPU" -H "accept: application/json"
```

---

## ?? POWERSHELL (Windows)

### 1. Listar todas
```powershell
Invoke-RestMethod -Uri "http://localhost:5000/api/usinas" -Method GET | ConvertTo-Json
```

### 2. Buscar por ID
```powershell
Invoke-RestMethod -Uri "http://localhost:5000/api/usinas/1" -Method GET | ConvertTo-Json
```

### 3. Criar nova
```powershell
$body = @{
    codigo = "UHE-JIRAU"
    nome = "Usina Hidrelétrica de Jirau"
    tipoUsinaId = 1
    empresaId = 3
    capacidadeInstalada = 3750
    localizacao = "Porto Velho, RO"
    dataOperacao = "2013-09-01"
    ativo = $true
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/usinas" -Method POST -Body $body -ContentType "application/json"
```

---

## ?? TESTAR PORTAS DINÂMICAS

Se a API estiver em porta diferente (ex: 65418):

```bash
# Substituir 5000 pela porta correta
curl -X GET "http://localhost:65418/api/usinas"
```

---

## ?? RESULTADOS ESPERADOS

### GET /api/usinas (10 usinas)
```json
[
  {
    "id": 1,
    "codigo": "UHE-ITAIPU",
    "nome": "Usina Hidrelétrica de Itaipu",
    "tipoUsina": "Hidrelétrica",
    "empresa": "Itaipu Binacional",
    "capacidadeInstalada": 14000.00
  },
  ...
]
```

### GET /api/usinas/1 (Itaipu)
```json
{
  "id": 1,
  "codigo": "UHE-ITAIPU",
  "nome": "Usina Hidrelétrica de Itaipu",
  "capacidadeInstalada": 14000.00
}
```

---

**Use o guia GUIA_TESTES_API_USINA_SWAGGER.md para testes detalhados via Swagger!**
