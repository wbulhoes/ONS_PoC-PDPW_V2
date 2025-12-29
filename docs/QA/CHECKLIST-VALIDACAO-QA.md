# ✅ CHECKLIST DE VALIDAÇÃO QA - BUGS CORRIGIDOS

**Data**: ___/___/2025  
**Executor**: __________________________  
**Ambiente**: Docker (localhost:5001)  
**Build**: #________  

---

## 🎯 PRÉ-REQUISITOS

- [ ] API rodando no Docker (`docker ps` mostra pdpw-backend)
- [ ] Health check responde: `Invoke-RestMethod http://localhost:5001/health`
- [ ] Swagger acessível: http://localhost:5001/swagger

---

## 🧪 TESTES AUTOMATIZADOS

### ✅ Script de Validação Rápida (5 min)

```powershell
cd C:\temp\_ONS_PoC-PDPW_V2
.\scripts\validar-bugs-qa.ps1
```

| Teste | Status | Observações |
|-------|--------|-------------|
| ArquivosDadger - Criar válido | ☐ PASSOU ☐ FALHOU | _________________ |
| ArquivosDadger - Validar SemanaPMO inválida | ☐ PASSOU ☐ FALHOU | _________________ |
| ArquivosDadger - Processar arquivo | ☐ PASSOU ☐ FALHOU | _________________ |
| ArquivosDadger - Filtrar por semana | ☐ PASSOU ☐ FALHOU | _________________ |
| ArquivosDadger - Listar todos | ☐ PASSOU ☐ FALHOU | _________________ |
| ArquivosDadger - Soft delete | ☐ PASSOU ☐ FALHOU | _________________ |
| RestricoesUG - Criar válida | ☐ PASSOU ☐ FALHOU | _________________ |
| RestricoesUG - Validar datas inválidas | ☐ PASSOU ☐ FALHOU | _________________ |
| RestricoesUG - Listar ativas | ☐ PASSOU ☐ FALHOU | _________________ |
| RestricoesUG - Soft delete | ☐ PASSOU ☐ FALHOU | _________________ |

**Taxa de Sucesso**: _____ de 10 (______%)

**Status Final**: ☐ ✅ APROVADO (100%)  ☐ ⚠️ APROVADO COM RESSALVAS (90-99%)  ☐ ❌ REPROVADO (<90%)

---

## 🌐 TESTES MANUAIS NO SWAGGER

### Bug #1: ArquivosDadger - AutoMapper

#### Teste 1.1: GET /api/arquivosdadger

1. ☐ Acessar http://localhost:5001/swagger
2. ☐ Expandir "ArquivosDadger" → "GET /api/arquivosdadger"
3. ☐ Clicar "Try it out"
4. ☐ Clicar "Execute"

**Resultado Esperado**:
- ☐ Status: `200 OK`
- ☐ Response retorna lista de arquivos
- ☐ Campos: `id`, `nomeArquivo`, `semanaPMO`, etc.
- ☐ Sem erro 500

**Status**: ☐ ✅ PASSOU  ☐ ❌ FALHOU

**Evidência**: Screenshot anexado? ☐ Sim ☐ Não  
**Arquivo**: `evidencia-1.1-GET-arquivos.png`

---

#### Teste 1.2: POST /api/arquivosdadger (VÁLIDO)

1. ☐ Expandir "POST /api/arquivosdadger"
2. ☐ Clicar "Try it out"
3. ☐ Colar JSON:

```json
{
  "nomeArquivo": "dadger_qa_valido.dat",
  "caminhoArquivo": "/uploads/qa_valido.dat",
  "dataImportacao": "2025-12-23T10:00:00",
  "semanaPMOId": 1,
  "observacoes": "Teste QA - Validação Bug Corrigido"
}
```

4. ☐ Clicar "Execute"

**Resultado Esperado**:
- ☐ Status: `201 Created`
- ☐ Response retorna arquivo criado com `id`
- ☐ Campo `processado: false`

**Status**: ☐ ✅ PASSOU  ☐ ❌ FALHOU

**ID Criado**: ____________

**Evidência**: Screenshot anexado? ☐ Sim ☐ Não  
**Arquivo**: `evidencia-1.2-POST-valido.png`

---

#### Teste 1.3: POST /api/arquivosdadger (INVÁLIDO - SemanaPMO 999)

1. ☐ Mesma tela de POST
2. ☐ Colar JSON:

```json
{
  "nomeArquivo": "dadger_qa_invalido.dat",
  "caminhoArquivo": "/uploads/qa_invalido.dat",
  "dataImportacao": "2025-12-23T10:00:00",
  "semanaPMOId": 999,
  "observacoes": "Deve falhar - Semana não existe"
}
```

3. ☐ Clicar "Execute"

**Resultado Esperado**:
- ☐ Status: `400 Bad Request`
- ☐ Mensagem de erro: "Semana PMO com ID 999 não encontrada" (ou similar)
- ☐ Sem erro 500

**Status**: ☐ ✅ PASSOU  ☐ ❌ FALHOU

**Evidência**: Screenshot anexado? ☐ Sim ☐ Não  
**Arquivo**: `evidencia-1.3-POST-invalido.png`

---

### Bug #2: RestricoesUG - Validação de Datas

#### Teste 2.1: POST /api/restricoesug (VÁLIDO)

1. ☐ Expandir "RestricoesUG" → "POST /api/restricoesug"
2. ☐ Clicar "Try it out"
3. ☐ Colar JSON:

```json
{
  "unidadeGeradoraId": 1,
  "dataInicio": "2025-12-23",
  "dataFim": "2025-12-30",
  "motivoRestricaoId": 1,
  "potenciaRestrita": 150.00,
  "observacoes": "Teste QA - Datas válidas"
}
```

4. ☐ Clicar "Execute"

**Resultado Esperado**:
- ☐ Status: `201 Created`
- ☐ Response retorna restrição criada com `id`

**Status**: ☐ ✅ PASSOU  ☐ ❌ FALHOU

**ID Criado**: ____________

**Evidência**: Screenshot anexado? ☐ Sim ☐ Não  
**Arquivo**: `evidencia-2.1-POST-valido.png`

---

#### Teste 2.2: POST /api/restricoesug (INVÁLIDO - dataFim < dataInicio)

1. ☐ Mesma tela de POST
2. ☐ Colar JSON:

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

3. ☐ Clicar "Execute"

**Resultado Esperado**:
- ☐ Status: `400 Bad Request`
- ☐ Mensagem de erro: "A data fim deve ser maior ou igual à data início" (ou similar)
- ☐ Sem erro 500

**Status**: ☐ ✅ PASSOU  ☐ ❌ FALHOU

**Evidência**: Screenshot anexado? ☐ Sim ☐ Não  
**Arquivo**: `evidencia-2.2-POST-datas-invalidas.png`

---

### Bug #3: Usuarios - AutoMapper

#### Teste 3.1: GET /api/usuarios

1. ☐ Expandir "Usuarios" → "GET /api/usuarios"
2. ☐ Clicar "Try it out"
3. ☐ Clicar "Execute"

**Resultado Esperado**:
- ☐ Status: `200 OK`
- ☐ Response retorna lista de usuários
- ☐ Campos: `id`, `nome`, `email`, `equipePDP`, etc.
- ☐ Sem erro 500

**Status**: ☐ ✅ PASSOU  ☐ ❌ FALHOU

**Evidência**: Screenshot anexado? ☐ Sim ☐ Não  
**Arquivo**: `evidencia-3.1-GET-usuarios.png`

---

## 🔄 TESTES DE REGRESSÃO

Validar que correções não quebraram outras funcionalidades:

| Endpoint | Método | Esperado | Status | Observações |
|----------|--------|----------|--------|-------------|
| /api/usinas | GET | 200 OK | ☐ ✅ ☐ ❌ | _______________ |
| /api/empresas | GET | 200 OK | ☐ ✅ ☐ ❌ | _______________ |
| /api/tiposusina | GET | 200 OK | ☐ ✅ ☐ ❌ | _______________ |
| /api/semanaspmo | GET | 200 OK | ☐ ✅ ☐ ❌ | _______________ |
| /api/cargas | GET | 200 OK | ☐ ✅ ☐ ❌ | _______________ |
| /api/intercambios | GET | 200 OK | ☐ ✅ ☐ ❌ | _______________ |
| /api/balancos | GET | 200 OK | ☐ ✅ ☐ ❌ | _______________ |

**Taxa de Sucesso Regressão**: _____ de 7 (______%)

---

## 🧪 TESTES PLAYWRIGHT (OPCIONAL)

Se tiver suite Playwright configurada:

```bash
npm run test
```

**Resultado**: ☐ Passou ☐ Falhou

**Taxa de Sucesso**: _______% (_____ de _____ testes)

**Relatório HTML**: Anexado? ☐ Sim ☐ Não

---

## 📊 RESUMO FINAL

### Resultados

| Categoria | Testes | Passou | Falhou | Taxa |
|-----------|--------|--------|--------|------|
| **Testes Automatizados** | 10 | _____ | _____ | _____% |
| **Testes Manuais Swagger** | 6 | _____ | _____ | _____% |
| **Testes de Regressão** | 7 | _____ | _____ | _____% |
| **TOTAL** | 23 | _____ | _____ | _____% |

### Bugs Validados

| Bug | Status | Observações |
|-----|--------|-------------|
| ArquivosDadger - AutoMapper | ☐ ✅ CORRIGIDO ☐ ❌ FALHOU | _________________ |
| RestricoesUG - Validação Datas | ☐ ✅ CORRIGIDO ☐ ❌ FALHOU | _________________ |
| Usuarios - AutoMapper | ☐ ✅ CORRIGIDO ☐ ❌ FALHOU | _________________ |

---

## ✅ DECISÃO FINAL

### ☐ APROVADO ✅

- [ ] Taxa de sucesso >= 95%
- [ ] Todos os bugs corrigidos
- [ ] Testes de regressão passaram
- [ ] Nenhum novo erro 500 encontrado

**Assinatura QA**: __________________________  
**Data**: ___/___/2025

---

### ☐ APROVADO COM RESSALVAS ⚠️

- [ ] Taxa de sucesso entre 90-95%
- [ ] Bugs principais corrigidos
- [ ] Falhas menores documentadas

**Ressalvas**:
___________________________________________________________
___________________________________________________________

**Assinatura QA**: __________________________  
**Data**: ___/___/2025

---

### ☐ REPROVADO ❌

- [ ] Taxa de sucesso < 90%
- [ ] Bugs não corrigidos
- [ ] Novos erros 500 encontrados

**Motivo da Reprovação**:
___________________________________________________________
___________________________________________________________

**Assinatura QA**: __________________________  
**Data**: ___/___/2025

---

## 📎 ANEXOS

- [ ] Screenshots de todos os testes manuais
- [ ] Logs de testes automatizados
- [ ] Relatório Playwright (se aplicável)
- [ ] Logs Docker (se houver erros)

**Total de Anexos**: _______

---

## 📝 OBSERVAÇÕES ADICIONAIS

___________________________________________________________
___________________________________________________________
___________________________________________________________
___________________________________________________________
___________________________________________________________

---

**📅 Data de Validação**: ___/___/2025  
**⏰ Tempo Total**: _____ minutos  
**👤 Executor**: __________________________  
**✍️ Assinatura**: __________________________

---

*Checklist gerado em: 29/12/2025*  
*Versão: 1.0*
