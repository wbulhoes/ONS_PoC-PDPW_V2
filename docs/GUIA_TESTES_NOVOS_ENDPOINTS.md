# 🧪 GUIA DE TESTES - NOVOS ENDPOINTS

**Data**: 27/12/2024  
**Objetivo**: Testar os 4 endpoints corrigidos/implementados

---

## 🚀 PRÉ-REQUISITOS

1. **Docker rodando**:
   ```powershell
   docker-compose up -d
   ```

2. **Verificar saúde da API**:
   ```powershell
   curl http://localhost:5001/health
   ```

3. **Acessar Swagger** (opcional):
   ```
   http://localhost:5001/swagger
   ```

---

## ✅ TESTE 1: TiposUsina `/buscar`

### **Cenário**: Buscar tipos de usina que contenham "Hidro"

**PowerShell**:
```powershell
Invoke-RestMethod -Uri "http://localhost:5001/api/tiposusina/buscar?termo=Hidro" -Method GET | ConvertTo-Json
```

**cURL**:
```bash
curl "http://localhost:5001/api/tiposusina/buscar?termo=Hidro"
```

**Resultado Esperado**:
```json
[
  {
    "id": 1,
    "nome": "Hidrelétrica",
    "descricao": "Usina Hidrelétrica",
    "dataCriacao": "2024-12-26T00:00:00"
  }
]
```

**Validações**:
- ✅ Status Code: 200 OK
- ✅ Retorna array com pelo menos 1 registro
- ✅ Todos os registros contêm "Hidro" no nome ou descrição

---

## ✅ TESTE 2: Empresas `/buscar`

### **Cenário 1**: Buscar empresas por nome "Itaipu"

**PowerShell**:
```powershell
Invoke-RestMethod -Uri "http://localhost:5001/api/empresas/buscar?termo=Itaipu" -Method GET | ConvertTo-Json
```

**cURL**:
```bash
curl "http://localhost:5001/api/empresas/buscar?termo=Itaipu"
```

**Resultado Esperado**:
```json
[
  {
    "id": 1,
    "nome": "Itaipu Binacional",
    "cnpj": "00341583000171",
    "telefone": "(45) 3520-5252",
    "email": "contato@itaipu.gov.br"
  }
]
```

---

### **Cenário 2**: Buscar empresas por CNPJ parcial "00341583"

**PowerShell**:
```powershell
Invoke-RestMethod -Uri "http://localhost:5001/api/empresas/buscar?termo=00341583" -Method GET | ConvertTo-Json
```

**cURL**:
```bash
curl "http://localhost:5001/api/empresas/buscar?termo=00341583"
```

**Resultado Esperado**:
```json
[
  {
    "id": 1,
    "nome": "Itaipu Binacional",
    "cnpj": "00341583000171",
    ...
  }
]
```

**Validações**:
- ✅ Status Code: 200 OK
- ✅ Retorna empresas que contêm o termo no nome OU CNPJ
- ✅ Busca case-insensitive

---

## ✅ TESTE 3: Intercambios `/subsistema`

### **Cenário 1**: Filtrar apenas por origem "SE"

**PowerShell**:
```powershell
$intercambios = Invoke-RestMethod -Uri "http://localhost:5001/api/intercambios/subsistema?origem=SE" -Method GET
Write-Host "Total encontrado: $($intercambios.Count)"
$intercambios[0] | ConvertTo-Json
```

**cURL**:
```bash
curl "http://localhost:5001/api/intercambios/subsistema?origem=SE"
```

**Resultado Esperado**:
```json
[
  {
    "id": 1,
    "subsistemaOrigem": "SE",
    "subsistemaDestino": "S",
    "energiaIntercambiada": 1500.50,
    "dataReferencia": "2024-12-26T00:00:00",
    ...
  },
  ...
]
```

**Validações**:
- ✅ Status Code: 200 OK
- ✅ Todos os registros têm `subsistemaOrigem = "SE"`
- ✅ Retorna pelo menos 60 registros (30 dias × 2 destinos possíveis)

---

### **Cenário 2**: Filtrar por origem "SE" E destino "S"

**PowerShell**:
```powershell
$intercambios = Invoke-RestMethod -Uri "http://localhost:5001/api/intercambios/subsistema?origem=SE&destino=S" -Method GET
Write-Host "Total encontrado: $($intercambios.Count)"
Write-Host "Primeiro registro: $($intercambios[0] | ConvertTo-Json)"
```

**cURL**:
```bash
curl "http://localhost:5001/api/intercambios/subsistema?origem=SE&destino=S"
```

**Resultado Esperado**:
```json
[
  {
    "id": 1,
    "subsistemaOrigem": "SE",
    "subsistemaDestino": "S",
    "energiaIntercambiada": 1500.50,
    ...
  },
  ...
]
```

**Validações**:
- ✅ Status Code: 200 OK
- ✅ Todos têm `subsistemaOrigem = "SE"` E `subsistemaDestino = "S"`
- ✅ Retorna exatamente 30 registros (30 dias)

---

### **Cenário 3**: Filtrar apenas por destino "NE"

**PowerShell**:
```powershell
$intercambios = Invoke-RestMethod -Uri "http://localhost:5001/api/intercambios/subsistema?destino=NE" -Method GET
Write-Host "Total encontrado: $($intercambios.Count)"
```

**cURL**:
```bash
curl "http://localhost:5001/api/intercambios/subsistema?destino=NE"
```

**Validações**:
- ✅ Status Code: 200 OK
- ✅ Todos os registros têm `subsistemaDestino = "NE"`

---

### **Cenário 4**: Sem filtros (retorna todos)

**PowerShell**:
```powershell
$intercambios = Invoke-RestMethod -Uri "http://localhost:5001/api/intercambios/subsistema" -Method GET
Write-Host "Total de intercâmbios no banco: $($intercambios.Count)"
```

**cURL**:
```bash
curl "http://localhost:5001/api/intercambios/subsistema"
```

**Validações**:
- ✅ Status Code: 200 OK
- ✅ Retorna todos os 240 intercâmbios

---

## ✅ TESTE 4: SemanasPMO `/proximas`

### **Cenário 1**: Obter próximas 4 semanas (padrão)

**PowerShell**:
```powershell
$semanas = Invoke-RestMethod -Uri "http://localhost:5001/api/semanaspmo/proximas" -Method GET
Write-Host "Próximas semanas: $($semanas.Count)"
$semanas | ForEach-Object { Write-Host "Semana $($_.numero)/$($_.ano): $($_.dataInicio) - $($_.dataFim)" }
```

**cURL**:
```bash
curl "http://localhost:5001/api/semanaspmo/proximas"
```

**Resultado Esperado**:
```json
[
  {
    "id": 15,
    "numero": 52,
    "ano": 2024,
    "dataInicio": "2024-12-28T00:00:00",
    "dataFim": "2025-01-03T00:00:00"
  },
  {
    "id": 16,
    "numero": 1,
    "ano": 2025,
    "dataInicio": "2025-01-04T00:00:00",
    "dataFim": "2025-01-10T00:00:00"
  },
  ...
]
```

**Validações**:
- ✅ Status Code: 200 OK
- ✅ Retorna 4 semanas
- ✅ Todas as semanas têm `dataInicio > hoje`
- ✅ Ordenadas por data crescente

---

### **Cenário 2**: Obter próximas 10 semanas

**PowerShell**:
```powershell
$semanas = Invoke-RestMethod -Uri "http://localhost:5001/api/semanaspmo/proximas?quantidade=10" -Method GET
Write-Host "Próximas $($semanas.Count) semanas:"
$semanas | Select-Object numero, ano, dataInicio | Format-Table
```

**cURL**:
```bash
curl "http://localhost:5001/api/semanaspmo/proximas?quantidade=10"
```

**Validações**:
- ✅ Status Code: 200 OK
- ✅ Retorna até 10 semanas (ou menos se não houver)
- ✅ Ordenadas cronologicamente

---

### **Cenário 3**: Obter próxima 1 semana

**PowerShell**:
```powershell
$semana = Invoke-RestMethod -Uri "http://localhost:5001/api/semanaspmo/proximas?quantidade=1" -Method GET
Write-Host "Próxima semana: Semana $($semana.numero)/$($semana.ano)"
Write-Host "Período: $($semana.dataInicio) a $($semana.dataFim)"
```

**cURL**:
```bash
curl "http://localhost:5001/api/semanaspmo/proximas?quantidade=1"
```

**Validações**:
- ✅ Status Code: 200 OK
- ✅ Retorna exatamente 1 semana
- ✅ É a semana imediatamente após hoje

---

## 🔍 VALIDAÇÃO COMPLETA (Script Automatizado)

### **Executar todas as validações de uma vez**:

**PowerShell**:
```powershell
.\scripts\powershell\validar-todas-apis.ps1
```

**Resultado Esperado**:
```
========================================
RELATÓRIO FINAL DA VALIDAÇÃO
========================================

📊 ESTATÍSTICAS:
   Total de Endpoints Testados: 50
   ✅ Sucessos: 50 (100%)
   ❌ Falhas: 0 (0%)

📋 DETALHES POR API:
   TiposUsina : 3/3 OK
   Empresas : 4/4 OK
   SemanasPMO : 5/5 OK
   Intercambios : 4/4 OK
   ...

✅ TODAS AS APIS ESTÃO FUNCIONANDO!
```

---

## 🐛 TROUBLESHOOTING

### **Problema**: `Connection refused` ou `500 Internal Server Error`

**Solução**:
```powershell
# Verificar se Docker está rodando
docker ps

# Reiniciar containers
docker-compose down
docker-compose up -d --build

# Aguardar 30 segundos
Start-Sleep -Seconds 30

# Testar novamente
curl http://localhost:5001/health
```

---

### **Problema**: Retorna array vazio `[]`

**Possíveis causas**:
1. Termo de busca não encontrado (normal)
2. Banco sem dados (problema)

**Solução**:
```powershell
# Verificar quantidade de registros no banco
docker exec pdpw-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "Pdpw@2024!Strong" -C -d PDPW_DB -Q "SELECT COUNT(*) FROM TiposUsina"

# Se retornar 0, refazer seed
docker-compose down -v
docker-compose up -d --build
```

---

### **Problema**: `404 Not Found`

**Solução**:
```powershell
# Verificar se a API está rodando
curl http://localhost:5001/swagger

# Verificar se endpoint está correto
# Correto: /api/tiposusina/buscar?termo=Hidro
# Errado: /api/tiposusina/search?termo=Hidro
```

---

## ✅ CHECKLIST DE VALIDAÇÃO

Marque cada item após testar com sucesso:

- [ ] **TiposUsina `/buscar`**
  - [ ] Buscar por "Hidro" → retorna resultados
  - [ ] Buscar por "XXX" → retorna array vazio
  
- [ ] **Empresas `/buscar`**
  - [ ] Buscar por nome "Itaipu" → retorna Itaipu Binacional
  - [ ] Buscar por CNPJ "00341583" → retorna Itaipu Binacional
  
- [ ] **Intercambios `/subsistema`**
  - [ ] Filtrar origem=SE → retorna ~60-90 registros
  - [ ] Filtrar destino=S → retorna ~60-90 registros
  - [ ] Filtrar origem=SE e destino=S → retorna ~30 registros
  - [ ] Sem filtros → retorna 240 registros
  
- [ ] **SemanasPMO `/proximas`**
  - [ ] Padrão (4 semanas) → retorna 4 semanas futuras
  - [ ] quantidade=10 → retorna até 10 semanas
  - [ ] quantidade=1 → retorna próxima semana

- [ ] **Validação Completa**
  - [ ] Script `validar-todas-apis.ps1` → 50/50 OK (100%)

---

## 🎉 CONCLUSÃO

Após validar todos os testes acima, você confirmará que:

✅ **Todos os 4 endpoints estão funcionando perfeitamente**  
✅ **POC está 100% completa**  
✅ **Sistema pronto para demonstração**

---

**Criado em**: 27/12/2024  
**Por**: GitHub Copilot  
**Para**: Willian Bulhões
