# ⚡ GUIA RÁPIDO - VALIDAÇÃO DE BUGS CORRIGIDOS

**Para**: QA Team  
**Data**: 29/12/2025  
**Tempo Estimado**: 15 minutos  

---

## 🎯 OBJETIVO

Validar rapidamente se os **bugs reportados** foram corrigidos.

---

## 🚀 PASSO A PASSO (15 MINUTOS)

### 1️⃣ Preparação (2 minutos)

```powershell
# Abrir PowerShell como Administrador
cd C:\temp\_ONS_PoC-PDPW_V2

# Verificar se API está rodando
Invoke-RestMethod http://localhost:5001/health
```

**Resultado Esperado**:
```json
"Healthy"
```

---

### 2️⃣ Executar Script de Validação (5 minutos)

```powershell
.\scripts\validar-bugs-qa.ps1
```

**Resultado Esperado (100% de sucesso)**:
```
🧪 VALIDAÇÃO DE BUGS - QA AUTOMATION
===================================

📁 [1/2] Validando API ArquivosDadger...

  [1/6] Criar arquivo DADGER com SemanaPMO válida... ✅ PASSOU
  [2/6] Validar SemanaPMO inválida (999)... ✅ PASSOU
  [3/6] Marcar arquivo como processado... ✅ PASSOU
  [4/6] Filtrar arquivos por semana PMO... ✅ PASSOU
  [5/6] Listar todos os arquivos... ✅ PASSOU
  [6/6] Soft delete do arquivo... ✅ PASSOU

🔧 [2/2] Validando API RestricoesUG...

  [1/4] Criar restrição com datas válidas... ✅ PASSOU
  [2/4] Validar dataFim < dataInicio (deve falhar)... ✅ PASSOU
  [3/4] Buscar restrições ativas... ✅ PASSOU
  [4/4] Soft delete da restrição... ✅ PASSOU

📊 RELATÓRIO FINAL
==================

Total de Testes:  10
Testes Passaram:  10 ✅
Testes Falharam:  0 ✅
Taxa de Sucesso:  100%

✅ VALIDAÇÃO CONCLUÍDA COM SUCESSO!
   Todos os bugs reportados foram corrigidos.
```

---

### 3️⃣ Testes Manuais no Swagger (5 minutos)

Abra: **http://localhost:5001/swagger**

#### Teste 1: ArquivosDadger - GET (Bug Corrigido ✅)

1. Expanda `ArquivosDadger` → `GET /api/arquivosdadger`
2. Clique em **"Try it out"**
3. Clique em **"Execute"**

**Resultado Esperado**:
- ✅ Status: `200 OK`
- ✅ Response retorna lista de arquivos
- ✅ Sem erro 500

#### Teste 2: ArquivosDadger - POST Válido

1. Expanda `POST /api/arquivosdadger`
2. Clique em **"Try it out"**
3. Cole o JSON:

```json
{
  "nomeArquivo": "dadger_teste_qa.dat",
  "caminhoArquivo": "/uploads/teste.dat",
  "dataImportacao": "2025-12-23T10:00:00",
  "semanaPMOId": 1,
  "observacoes": "Teste QA - Bug corrigido"
}
```

4. Clique em **"Execute"**

**Resultado Esperado**:
- ✅ Status: `201 Created`
- ✅ Response retorna arquivo criado com ID

#### Teste 3: ArquivosDadger - POST Inválido (Validação)

1. Mesma tela de POST
2. Cole o JSON com SemanaPMO inválida:

```json
{
  "nomeArquivo": "dadger_invalido.dat",
  "caminhoArquivo": "/uploads/invalido.dat",
  "dataImportacao": "2025-12-23T10:00:00",
  "semanaPMOId": 999,
  "observacoes": "Deve falhar - Semana não existe"
}
```

3. Clique em **"Execute"**

**Resultado Esperado**:
- ✅ Status: `400 Bad Request`
- ✅ Mensagem de erro clara: "Semana PMO com ID 999 não encontrada"

#### Teste 4: RestricoesUG - POST Inválido (Data Fim < Data Início)

1. Expanda `RestricoesUG` → `POST /api/restricoesug`
2. Clique em **"Try it out"**
3. Cole o JSON:

```json
{
  "unidadeGeradoraId": 1,
  "dataInicio": "2025-12-30",
  "dataFim": "2025-12-23",
  "motivoRestricaoId": 1,
  "potenciaRestrita": 150.00,
  "observacoes": "Deve falhar - dataFim < dataInicio"
}
```

4. Clique em **"Execute"**

**Resultado Esperado**:
- ✅ Status: `400 Bad Request`
- ✅ Mensagem: "A data fim deve ser maior ou igual à data início"

---

### 4️⃣ Testes de Regressão (3 minutos)

Valide que os bugs corrigidos não quebraram outras funcionalidades:

```powershell
# Teste rápido de endpoints principais
Invoke-RestMethod http://localhost:5001/api/usinas | Select-Object -First 3
Invoke-RestMethod http://localhost:5001/api/empresas | Select-Object -First 3
Invoke-RestMethod http://localhost:5001/api/usuarios | Select-Object -First 3
```

**Resultado Esperado**:
- ✅ Todos retornam `200 OK`
- ✅ Dados exibidos corretamente

---

## ✅ CHECKLIST DE VALIDAÇÃO

- [ ] Script `validar-bugs-qa.ps1` retornou **100% de sucesso**
- [ ] Swagger `GET /api/arquivosdadger` retorna **200 OK**
- [ ] Swagger `POST /api/arquivosdadger` (válido) retorna **201 Created**
- [ ] Swagger `POST /api/arquivosdadger` (inválido) retorna **400 Bad Request**
- [ ] Swagger `POST /api/restricoesug` (datas inválidas) retorna **400 Bad Request**
- [ ] Endpoints de regressão retornam **200 OK**

---

## 🎯 CRITÉRIOS DE ACEITE

### ✅ APROVADO SE:

- Todos os itens do checklist marcados
- Nenhum endpoint retorna HTTP 500
- Validações de negócio funcionando (400 Bad Request quando esperado)

### ❌ REPROVADO SE:

- Qualquer endpoint retorna HTTP 500
- Validações não funcionam
- Dados não retornam corretamente

---

## 🚨 SE HOUVER FALHAS

### 1. Verificar Logs

```powershell
docker logs pdpw-backend --tail 50
```

### 2. Recriar Ambiente Docker

```powershell
docker-compose down -v
docker-compose build --no-cache
docker-compose up -d
Start-Sleep -Seconds 30
```

### 3. Re-executar Validação

```powershell
.\scripts\validar-bugs-qa.ps1
```

### 4. Reportar Falha

Se ainda houver falhas, reportar:
- Endpoint que falhou
- Status code recebido
- Mensagem de erro
- Logs do Docker (últimas 50 linhas)

---

## 📞 CONTATOS

| Situação | Contato |
|----------|---------|
| Dúvidas sobre testes | Willian Bulhões (PO) |
| Erro no Docker | DevOps Team |
| Bug não corrigido | Dev Backend Team |

---

## 📊 TEMPLATE DE RESPOSTA

Após validação, responda no Jira/Slack:

```
✅ VALIDAÇÃO CONCLUÍDA

Data: 29/12/2025
Bugs Validados:
- ArquivosDadger - AutoMapper: ✅ CORRIGIDO
- RestricoesUG - Validação Datas: ✅ CORRIGIDO
- Usuarios - AutoMapper: ✅ CORRIGIDO

Taxa de Sucesso: 100%
Regressão: OK

Status: APROVADO PARA PRODUÇÃO
```

OU (se houver falhas):

```
❌ VALIDAÇÃO COM FALHAS

Data: 23/12/2025
Bugs Encontrados:
- [Endpoint] - [Descrição do erro]
- [Endpoint] - [Descrição do erro]

Taxa de Sucesso: XX%
Evidências: [anexar screenshots/logs]

Status: AGUARDANDO CORREÇÕES
```

---

## ✅ CONCLUSÃO

**Tempo Total**: ~15 minutos  
**Complexidade**: Baixa  
**Pré-requisitos**: API rodando no Docker  

**Resultado Esperado**: 100% de sucesso em todos os testes

---

**🧪 BOM TESTE! 🚀**

*Gerado em: 29/12/2025*  
*Versão: 1.0*
