# ✅ CHECKLIST DE VALIDAÇÃO - QA

**Data**: ___/___/_____  
**Testador**: _________________________  
**Versão**: feature/backend (Docker)  
**Ambiente**: [ ] Local  [ ] Docker  [ ] Homolog

---

## 🚀 PRÉ-REQUISITOS

### 1. Ambiente Atualizado
- [ ] Executei `git pull origin feature/backend`
- [ ] Confirmei branch correta: `git branch` (deve mostrar `* feature/backend`)
- [ ] Verifiquei último commit: `git log -1`
- [ ] Executei `docker-compose down`
- [ ] Executei `docker-compose up --build -d`
- [ ] API está respondendo: http://localhost:5001
- [ ] Swagger está acessível: http://localhost:5001/swagger

---

## 📁 VALIDAÇÃO: ArquivosDadger API

### Teste 1: Criar Arquivo com SemanaPMO Válida
**Endpoint**: `POST /api/arquivosdadger`

**Payload**:
```json
{
  "nomeArquivo": "dadger_teste_qa.dat",
  "caminhoArquivo": "/uploads/teste_qa.dat",
  "dataImportacao": "2025-01-23T10:00:00",
  "semanaPMOId": 1,
  "observacoes": "Teste QA"
}
```

**Resultado Esperado**: Status 201 Created

- [ ] ✅ PASSOU - Status 201
- [ ] ❌ FALHOU - Status: _____ Erro: _________________

**ID Criado**: _______

---

### Teste 2: Validar SemanaPMO Inválida
**Endpoint**: `POST /api/arquivosdadger`

**Payload**:
```json
{
  "nomeArquivo": "dadger_invalido.dat",
  "caminhoArquivo": "/uploads/invalido.dat",
  "dataImportacao": "2025-01-23T10:00:00",
  "semanaPMOId": 999
}
```

**Resultado Esperado**: Status 400 Bad Request  
**Mensagem**: "Semana PMO com ID 999 não encontrada"

- [ ] ✅ PASSOU - Status 400 com mensagem correta
- [ ] ❌ FALHOU - Status: _____ Mensagem: _________________

---

### Teste 3: Marcar Como Processado
**Endpoint**: `PATCH /api/arquivosdadger/{ID DO TESTE 1}/processar`

**Resultado Esperado**: Status 200 OK  
**Campos validar**:
- `processado: true`
- `dataProcessamento: [timestamp atual]`

- [ ] ✅ PASSOU - Status 200 e campos corretos
- [ ] ❌ FALHOU - Status: _____ `processado`: _____ `dataProcessamento`: _____

---

### Teste 4: Filtrar por Semana PMO
**Endpoint**: `GET /api/arquivosdadger/semana/1`

**Resultado Esperado**: Status 200 OK  
**Validar**: Retorna apenas arquivos da semana PMO 1

- [ ] ✅ PASSOU - Status 200 e filtro correto
- [ ] ❌ FALHOU - Status: _____ Registros: _____

---

### Teste 5: Listar Todos os Arquivos
**Endpoint**: `GET /api/arquivosdadger`

**Resultado Esperado**: Status 200 OK

- [ ] ✅ PASSOU - Status 200
- [ ] ❌ FALHOU - Status: _____ Total: _____

---

### Teste 6: Soft Delete
**Endpoint**: `DELETE /api/arquivosdadger/{ID DO TESTE 1}`

**Resultado Esperado**: Status 204 No Content

**Validação adicional**: `GET /api/arquivosdadger/{ID}`  
**Esperado**: Status 404 Not Found

- [ ] ✅ PASSOU - Status 204 e depois 404
- [ ] ❌ FALHOU - Status: _____ Observação: _________________

---

## 🔧 VALIDAÇÃO: RestricoesUG API

### Teste 1: Criar Restrição com Datas Válidas
**Endpoint**: `POST /api/restricoesug`

**Payload**:
```json
{
  "unidadeGeradoraId": 1,
  "dataInicio": "2025-01-23",
  "dataFim": "2025-01-30",
  "motivoRestricaoId": 1,
  "potenciaRestrita": 150.00,
  "observacoes": "Teste QA - manutenção"
}
```

**Resultado Esperado**: Status 201 Created

- [ ] ✅ PASSOU - Status 201
- [ ] ❌ FALHOU - Status: _____ Erro: _________________

**ID Criado**: _______

---

### Teste 2: Validar dataFim < dataInicio
**Endpoint**: `POST /api/restricoesug`

**Payload**:
```json
{
  "unidadeGeradoraId": 1,
  "dataInicio": "2025-01-30",
  "dataFim": "2025-01-23",
  "motivoRestricaoId": 1,
  "potenciaRestrita": 150.00
}
```

**Resultado Esperado**: Status 400 Bad Request  
**Mensagem**: "Data fim não pode ser menor que data início"

- [ ] ✅ PASSOU - Status 400 com mensagem correta
- [ ] ❌ FALHOU - Status: _____ Mensagem: _________________

---

### Teste 3: Buscar Restrições Ativas
**Endpoint**: `GET /api/restricoesug/ativas?dataReferencia=2025-01-25`

**Resultado Esperado**: Status 200 OK  
**Validar**: Retorna apenas restrições onde:
- dataInicio ≤ 2025-01-25
- dataFim ≥ 2025-01-25 (ou null)

- [ ] ✅ PASSOU - Status 200 e filtro correto
- [ ] ❌ FALHOU - Status: _____ Registros: _____

---

### Teste 4: Soft Delete
**Endpoint**: `DELETE /api/restricoesug/{ID DO TESTE 1}`

**Resultado Esperado**: Status 204 No Content

**Validação adicional**: `GET /api/restricoesug/{ID}`  
**Esperado**: Status 404 Not Found

- [ ] ✅ PASSOU - Status 204 e depois 404
- [ ] ❌ FALHOU - Status: _____ Observação: _________________

---

## 🧪 VALIDAÇÃO: Testes Automatizados

### Executar Testes Unitários
**Comando**: `dotnet test --filter "FullyQualifiedName~ArquivoDadger"`

**Resultado Esperado**: 14/14 testes passando

- [ ] ✅ PASSOU - 14/14 testes OK
- [ ] ❌ FALHOU - _____/14 OK, _____ falharam

**Erros**:
```
_______________________________________
_______________________________________
_______________________________________
```

---

### Script de Validação Automatizada
**Comando**: `.\scripts\validar-bugs-qa.ps1`

**Resultado Esperado**: 100% dos testes passando

- [ ] ✅ PASSOU - 100% sucesso
- [ ] ❌ FALHOU - Taxa: _____% Falhas: _____

---

## 📊 RESULTADO FINAL

### Resumo por API

| API | Testes | Passou | Falhou | Status |
|-----|--------|--------|--------|--------|
| **ArquivosDadger** | 6 | _____ | _____ | [ ] ✅ [ ] ❌ |
| **RestricoesUG** | 4 | _____ | _____ | [ ] ✅ [ ] ❌ |
| **Testes Automáticos** | 2 | _____ | _____ | [ ] ✅ [ ] ❌ |
| **TOTAL** | 12 | _____ | _____ | [ ] ✅ [ ] ❌ |

---

### Critério de Aceite

**Para considerar APROVADO, TODOS os itens abaixo devem ser ✅**:

- [ ] ArquivosDadger: 6/6 testes passando
- [ ] RestricoesUG: 4/4 testes passando
- [ ] Testes automatizados: 100% sucesso
- [ ] Script de validação: 100% sucesso

---

## 🎯 DECISÃO

### [ ] ✅ APROVADO - Bugs foram corrigidos

**Ações**:
1. Fechar tickets no Jira
2. Atualizar Confluence
3. Comunicar stakeholders

**Observações**:
```
_______________________________________
_______________________________________
_______________________________________
```

---

### [ ] ❌ REPROVADO - Alguns bugs persistem

**Bugs encontrados**:
```
1. _________________________________
2. _________________________________
3. _________________________________
```

**Próximos passos**:
1. Reportar bugs ao Dev Team
2. Aguardar correção
3. Reagendar validação

---

## 📝 ASSINATURAS

**Testador (QA)**:  
Nome: _______________________________  
Data: ___/___/_____  
Assinatura: _________________________

**Revisado por (QA Lead)**:  
Nome: _______________________________  
Data: ___/___/_____  
Assinatura: _________________________

**Aprovado por (Dev Lead)**:  
Nome: _______________________________  
Data: ___/___/_____  
Assinatura: _________________________

---

## 📎 ANEXOS

- [ ] Screenshots dos testes
- [ ] Logs de erro (se aplicável)
- [ ] Relatório do script automatizado
- [ ] Evidências adicionais

---

**✅ CHECKLIST COMPLETO - BOA SORTE NOS TESTES! 🚀**

---

**Versão**: 1.0  
**Última Atualização**: 23/12/2025  
**Próxima Revisão**: 27/12/2025
