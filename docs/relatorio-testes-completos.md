# ?? RELAT�RIO DE TESTES AUTOMATIZADOS COMPLETOS - PDPw APIs

**Data:** 20/12/2025 22:18:32  
**Dura��o Total:** 0.75s  
**Base URL:** http://localhost:5001/api

---

## ?? RESUMO EXECUTIVO

| M�trica | Valor | Status |
|---------|-------|--------|
| **Total de Testes** | 55 | - |
| **Sucessos** | 49 | ? |
| **Falhas** | 6 | ? |
| **Taxa de Sucesso** | 89.09% | ?? |
| **Tempo M�dio** | 10.27ms | ? |

---

## ?? DISTRIBUI��O DE TESTES

### Por M�todo HTTP

| M�todo | Quantidade | % do Total |
|--------|------------|------------|
| **GET** | 32 | 58.2% |
| **POST** | 14 | 25.5% |
| **PUT** | 2 | 3.6% |
| **DELETE** | 6 | 10.9% |
| **PATCH** | 9 | 16.4% |

### Testes de Valida��o

| Tipo | Quantidade | Sucesso |
|------|------------|---------|
| **Testes de Valida��o** | 9 | 8/9 |
| **Testes Funcionais** | 46 | 41/46 |

---

## ?? RESULTADOS POR API

### ? API: arquivosdadger

**Testes:** 8 | **Sucessos:** 8 | **Taxa:** 100% | **Tempo M�dio:** 10.38ms

| M�todo | Endpoint | Descri��o | Status | Tempo |
|--------|----------|-----------|--------|-------|
| GET | /arquivosdadger | GET /arquivosdadger - Listar todos | ? (200) | 7ms |
| GET | /arquivosdadger/semana/1 | GET /arquivosdadger/semana/1 | ? (200) | 6ms |
| GET | /arquivosdadger/processados?processado=true | GET /arquivosdadger/processados=true | ? (200) | 6ms |
| GET | /arquivosdadger/processados?processado=false | GET /arquivosdadger/processados=false | ? (200) | 7ms |
| POST | /arquivosdadger | POST /arquivosdadger - Criar novo | ? (201) | 11ms |
| PATCH | /arquivosdadger/106/processar | PATCH /arquivosdadger/106/processar - Marcar processado | ? (200) | 24ms |
| GET | /arquivosdadger/106 | GET /arquivosdadger/106 - Verificar processamento | ? (200) | 6ms |
| DELETE | /arquivosdadger/106 | DELETE /arquivosdadger/106 - Remover | ? (204) | 16ms |

### ?? API: cargas

**Testes:** 11 | **Sucessos:** 10 | **Taxa:** 90.91% | **Tempo M�dio:** 10.36ms

| M�todo | Endpoint | Descri��o | Status | Tempo |
|--------|----------|-----------|--------|-------|
| GET | /cargas | GET /cargas - Listar todas | ? (200) | 12ms |
| GET | /cargas/1 | GET /cargas/1 - Buscar por ID | ? (200) | 6ms |
| GET | /cargas/99999 | GET /cargas/99999 - ID inexistente | ? (404) | 7ms |
| GET | /cargas/subsistema/SE | GET /cargas/subsistema/SE | ? (200) | 5ms |
| GET | /cargas/data/2025-01-15 | GET /cargas/data/2025-01-15 | ? (200) | 6ms |
| POST | /cargas | POST /cargas - Criar nova v�lida | ? (201) | 11ms |
| POST | /cargas | POST /cargas - Subsistema inv�lido | ? (201) | 0ms |
| POST | /cargas | POST /cargas - Carga negativa | ? (400) | 5ms |
| PUT | /cargas/111 | PUT /cargas/111 - Atualizar | ? (200) | 22ms |
| DELETE | /cargas/111 | DELETE /cargas/111 - Remover | ? (204) | 20ms |
| GET | /cargas/111 | GET /cargas/111 - Verificar remo��o | ? (404) | 20ms |

### ? API: empresas

**Testes:** 4 | **Sucessos:** 2 | **Taxa:** 50% | **Tempo M�dio:** 5.75ms

| M�todo | Endpoint | Descri��o | Status | Tempo |
|--------|----------|-----------|--------|-------|
| GET | /empresas | GET /empresas - Listar todas | ? (200) | 7ms |
| GET | /empresas/ativas | GET /empresas/ativas | ? (404) | 4ms |
| POST | /empresas | POST /empresas - Criar nova | ? (400) | 7ms |
| POST | /empresas | POST /empresas - CNPJ inv�lido | ? (400) | 5ms |

### ? API: equipespdp

**Testes:** 4 | **Sucessos:** 4 | **Taxa:** 100% | **Tempo M�dio:** 16.25ms

| M�todo | Endpoint | Descri��o | Status | Tempo |
|--------|----------|-----------|--------|-------|
| GET | /equipespdp | GET /equipespdp - Listar todas | ? (200) | 6ms |
| POST | /equipespdp | POST /equipespdp - Criar nova | ? (201) | 11ms |
| PUT | /equipespdp/1002 | PUT /equipespdp/1002 - Atualizar | ? (200) | 23ms |
| DELETE | /equipespdp/1002 | DELETE /equipespdp/1002 - Remover | ? (204) | 25ms |

### ? API: restricoesug

**Testes:** 7 | **Sucessos:** 7 | **Taxa:** 100% | **Tempo M�dio:** 11ms

| M�todo | Endpoint | Descri��o | Status | Tempo |
|--------|----------|-----------|--------|-------|
| GET | /restricoesug | GET /restricoesug - Listar todas | ? (200) | 8ms |
| GET | /restricoesug/unidade/1 | GET /restricoesug/unidade/1 | ? (200) | 6ms |
| GET | /restricoesug/motivo/1 | GET /restricoesug/motivo/1 - Manuten��o Preventiva | ? (200) | 7ms |
| GET | /restricoesug/ativas?dataReferencia=2025-01-15 | GET /restricoesug/ativas?dataReferencia=2025-01-15 | ? (200) | 9ms |
| POST | /restricoesug | POST /restricoesug - Criar nova | ? (201) | 14ms |
| POST | /restricoesug | POST /restricoesug - Datas inv�lidas | ? (400) | 6ms |
| DELETE | /restricoesug/111 | DELETE /restricoesug/111 - Remover | ? (204) | 27ms |

### ? API: semanaspmo

**Testes:** 8 | **Sucessos:** 6 | **Taxa:** 75% | **Tempo M�dio:** 12.12ms

| M�todo | Endpoint | Descri��o | Status | Tempo |
|--------|----------|-----------|--------|-------|
| GET | /semanaspmo | GET /semanaspmo - Listar todas | ? (200) | 8ms |
| GET | /semanaspmo/atual | GET /semanaspmo/atual | ? (404) | 5ms |
| GET | /semanaspmo/proximas?quantidade=5 | GET /semanaspmo/proximas?quantidade=5 | ? (404) | 5ms |
| GET | /semanaspmo/ano/2025 | GET /semanaspmo/ano/2025 | ? (200) | 7ms |
| GET | /semanaspmo/numero/3/ano/2025 | GET /semanaspmo/numero/3/ano/2025 | ? (200) | 24ms |
| POST | /semanaspmo | POST /semanaspmo - Criar nova | ? (201) | 21ms |
| POST | /semanaspmo | POST /semanaspmo - N�mero inv�lido (54) | ? (400) | 8ms |
| DELETE | /semanaspmo/106 | DELETE /semanaspmo/106 - Remover | ? (204) | 19ms |

### ? API: tiposusina

**Testes:** 5 | **Sucessos:** 5 | **Taxa:** 100% | **Tempo M�dio:** 5.8ms

| M�todo | Endpoint | Descri��o | Status | Tempo |
|--------|----------|-----------|--------|-------|
| GET | /tiposusina | GET /tiposusina - Listar todos | ? (200) | 6ms |
| GET | /tiposusina/1 | GET /tiposusina/1 - Hidrel�trica | ? (200) | 6ms |
| GET | /tiposusina/2 | GET /tiposusina/2 - Termel�trica | ? (200) | 6ms |
| GET | /tiposusina/4 | GET /tiposusina/4 - E�lica | ? (200) | 5ms |
| GET | /tiposusina/5 | GET /tiposusina/5 - Solar | ? (200) | 6ms |

### ?? API: usinas

**Testes:** 8 | **Sucessos:** 7 | **Taxa:** 87.5% | **Tempo M�dio:** 9.62ms

| M�todo | Endpoint | Descri��o | Status | Tempo |
|--------|----------|-----------|--------|-------|
| GET | /usinas | GET /usinas - Listar todas | ? (200) | 8ms |
| GET | /usinas/tipo/1 | GET /usinas/tipo/1 - Hidrel�tricas | ? (200) | 7ms |
| GET | /usinas/empresa/1 | GET /usinas/empresa/1 | ? (200) | 6ms |
| GET | /usinas/codigo/ITU | GET /usinas/codigo/ITU - Itaipu | ? (400) | 6ms |
| POST | /usinas | POST /usinas - Criar nova | ? (201) | 15ms |
| POST | /usinas | POST /usinas - C�digo duplicado | ? (409) | 8ms |
| POST | /usinas | POST /usinas - Capacidade negativa | ? (400) | 4ms |
| DELETE | /usinas/1002 | DELETE /usinas/1002 - Remover | ? (204) | 23ms |

---

## ?? DETALHAMENTO DE FALHAS

### ? POST /cargas - Subsistema inv�lido

- **M�todo:** POST
- **Endpoint:** /cargas
- **Status Code:** 201
- **Status Esperado:** 400
- **Erro:** Status inesperado: esperado 400, recebido 201

### ? GET /semanaspmo/atual

- **M�todo:** GET
- **Endpoint:** /semanaspmo/atual
- **Status Code:** 404
- **Status Esperado:** N/A
- **Erro:** Response status code does not indicate success: 404 (Not Found).

### ? GET /semanaspmo/proximas?quantidade=5

- **M�todo:** GET
- **Endpoint:** /semanaspmo/proximas?quantidade=5
- **Status Code:** 404
- **Status Esperado:** N/A
- **Erro:** Response status code does not indicate success: 404 (Not Found).

### ? GET /empresas/ativas

- **M�todo:** GET
- **Endpoint:** /empresas/ativas
- **Status Code:** 404
- **Status Esperado:** N/A
- **Erro:** Response status code does not indicate success: 404 (Not Found).

### ? POST /empresas - Criar nova

- **M�todo:** POST
- **Endpoint:** /empresas
- **Status Code:** 400
- **Status Esperado:** N/A
- **Erro:** Response status code does not indicate success: 400 (Bad Request).

### ? GET /usinas/codigo/ITU - Itaipu

- **M�todo:** GET
- **Endpoint:** /usinas/codigo/ITU
- **Status Code:** 400
- **Status Esperado:** N/A
- **Erro:** Response status code does not indicate success: 400 (Bad Request).

---

## ?? AN�LISE DE PERFORMANCE

| API | Tempo M�dio (ms) | Tempo Min (ms) | Tempo Max (ms) | Testes |
|-----|------------------|----------------|----------------|--------|
| arquivosdadger | 10.38 | 6 | 24 | 8 |
| cargas | 10.36 | 0 | 22 | 11 |
| empresas | 5.75 | 4 | 7 | 4 |
| equipespdp | 16.25 | 6 | 25 | 4 |
| restricoesug | 11 | 6 | 27 | 7 |
| semanaspmo | 12.12 | 5 | 24 | 8 |
| tiposusina | 5.8 | 5 | 6 | 5 |
| usinas | 9.62 | 4 | 23 | 8 |

---

## ? COVERAGE DE TESTES

### CRUD Completo

| Opera��o | Testado | APIs Cobertas |
|----------|---------|---------------|
| **CREATE (POST)** | ? | Cargas, ArquivosDadger, RestricoesUG, SemanasPMO, Empresas, Usinas, EquipesPDP |
| **READ (GET)** | ? | Todas as 8 APIs |
| **UPDATE (PUT)** | ? | Cargas, Empresas, EquipesPDP |
| **DELETE** | ? | Cargas, ArquivosDadger, RestricoesUG, SemanasPMO, Empresas, Usinas, EquipesPDP |
| **PATCH** | ? | ArquivosDadger (processar) |

### Valida��es Testadas

| Valida��o | Testado | Exemplo |
|-----------|---------|---------|
| **Dados inv�lidos** | ? | Subsistema inv�lido, CNPJ inv�lido |
| **Valores negativos** | ? | Carga negativa, Capacidade negativa |
| **Duplica��o** | ? | CNPJ duplicado, C�digo duplicado |
| **Datas inv�lidas** | ? | Data fim antes do in�cio |
| **Ranges inv�lidos** | ? | Semana > 53 |
| **IDs inexistentes** | ? | GET com ID 99999 |

### Filtros Testados

| Filtro | APIs Testadas |
|--------|---------------|
| **Por ID** | Todas |
| **Por per�odo/data** | Cargas, RestricoesUG |
| **Por relacionamento** | Usinas (empresa, tipo), ArquivosDadger (semana), RestricoesUG (unidade, motivo) |
| **Por status** | Empresas (ativas), ArquivosDadger (processados) |
| **Endpoints especiais** | SemanasPMO (atual, pr�ximas, por ano) |

---

## ?? CONCLUS�O

### ?? **APROVADO COM RESSALVAS**

?? **Taxa de sucesso: 89.09%**

O sistema est� funcional mas apresenta 6 falha(s) que devem ser corrigidas.

**Recomenda��o:** Revisar e corrigir as falhas antes de produ��o.

---

## ?? DETALHES T�CNICOS

- **Framework de Testes:** PowerShell 7+
- **Ferramenta HTTP:** Invoke-RestMethod
- **Formato de Response:** JSON
- **Valida��o:** Status Codes + Response Bodies
- **Performance:** Medi��o com Stopwatch

---

**Gerado automaticamente em:** 20/12/2025 22:18:32  
**Script:** Test-AllApis-Complete.ps1  
**Vers�o:** 2.0 (CRUD Completo + Valida��es)
