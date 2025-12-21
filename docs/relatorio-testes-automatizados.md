# ?? RELAT�RIO DE TESTES AUTOMATIZADOS - APIs PDPw

**Data:** 20/12/2025 22:11:59  
**Dura��o Total:** 0.92s  
**Base URL:** http://localhost:5001/api

---

## ?? RESUMO EXECUTIVO

| M�trica | Valor |
|---------|-------|
| **Total de Testes** | 37 |
| **Sucessos** | ? 27 |
| **Falhas** | ? 10 |
| **Taxa de Sucesso** | 72.97% |
| **Tempo M�dio** | 20.73ms |

---

## ?? RESULTADOS POR API

### API: arquivosdadger

**Testes:** 5 | **Sucessos:** 5 | **Taxa:** 100%

| M�todo | Endpoint | Descri��o | Status | Tempo |
|--------|----------|-----------|--------|-------|
| GET | /arquivosdadger | GET /arquivosdadger - Listar todos | ? | 7ms |
| GET | /arquivosdadger/100 | GET /arquivosdadger/100 - Fict�cio | ? | 20ms |
| GET | /arquivosdadger/semana/100 | GET /arquivosdadger/semana/100 | ? | 17ms |
| GET | /arquivosdadger/processados?processado=true | GET /arquivosdadger/processados?processado=true | ? | 23ms |
| POST | /arquivosdadger | POST /arquivosdadger - Criar novo | ? | 10ms |

### API: cargas

**Testes:** 5 | **Sucessos:** 5 | **Taxa:** 100%

| M�todo | Endpoint | Descri��o | Status | Tempo |
|--------|----------|-----------|--------|-------|
| GET | /cargas | GET /cargas - Listar todas | ? | 7ms |
| GET | /cargas/100 | GET /cargas/100 - Fict�cia | ? | 13ms |
| GET | /cargas/subsistema/SE | GET /cargas/subsistema/SE | ? | 25ms |
| GET | /cargas/data/2025-12-21 | GET /cargas/data/2025-12-21 | ? | 16ms |
| POST | /cargas | POST /cargas - Criar nova | ? | 19ms |

### API: empresas

**Testes:** 4 | **Sucessos:** 8 | **Taxa:** 200%

| M�todo | Endpoint | Descri��o | Status | Tempo |
|--------|----------|-----------|--------|-------|
| GET | /empresas | GET /empresas - Listar todas | ? | 103ms |
| GET | /empresas/200 | GET /empresas/200 - Buscar fict�cia | ? | 43ms |
| POST | /empresas | POST /empresas - Criar nova | ? | 17ms |
| PUT | /empresas/200 | PUT /empresas/200 - Atualizar | ? | 10ms |

### API: equipespdp

**Testes:** 3 | **Sucessos:** 3 | **Taxa:** 100%

| M�todo | Endpoint | Descri��o | Status | Tempo |
|--------|----------|-----------|--------|-------|
| GET | /equipespdp | GET /equipespdp - Listar todas | ? | 6ms |
| GET | /equipespdp/100 | GET /equipespdp/100 - Fict�cia | ? | 14ms |
| POST | /equipespdp | POST /equipespdp - Criar nova | ? | 38ms |

### API: restricoesug

**Testes:** 6 | **Sucessos:** 4 | **Taxa:** 66.67%

| M�todo | Endpoint | Descri��o | Status | Tempo |
|--------|----------|-----------|--------|-------|
| GET | /restricoesug | GET /restricoesug - Listar todas | ? | 7ms |
| GET | /restricoesug/100 | GET /restricoesug/100 - Fict�cia | ? | 29ms |
| GET | /restricoesug/unidade/100 | GET /restricoesug/unidade/100 | ? | 17ms |
| GET | /restricoesug/motivo/1 | GET /restricoesug/motivo/1 | ? | 17ms |
| GET | /restricoesug/ativas?dataReferencia=2025-12-22 | GET /restricoesug/ativas?dataReferencia=2025-12-22 | ? | 28ms |
| POST | /restricoesug | POST /restricoesug - Criar nova | ? | 19ms |

### API: semanaspmo

**Testes:** 6 | **Sucessos:** 4 | **Taxa:** 66.67%

| M�todo | Endpoint | Descri��o | Status | Tempo |
|--------|----------|-----------|--------|-------|
| GET | /semanaspmo | GET /semanaspmo - Listar todas | ? | 7ms |
| GET | /semanaspmo/atual | GET /semanaspmo/atual | ? | 27ms |
| GET | /semanaspmo/proximas?quantidade=4 | GET /semanaspmo/proximas?quantidade=4 | ? | 5ms |
| GET | /semanaspmo/ano/2026 | GET /semanaspmo/ano/2026 | ? | 24ms |
| GET | /semanaspmo/100 | GET /semanaspmo/100 - Fict�cia | ? | 19ms |
| POST | /semanaspmo | POST /semanaspmo - Criar nova | ? | 28ms |

### API: tiposusina

**Testes:** 2 | **Sucessos:** 2 | **Taxa:** 100%

| M�todo | Endpoint | Descri��o | Status | Tempo |
|--------|----------|-----------|--------|-------|
| GET | /tiposusina | GET /tiposusina - Listar todos | ? | 7ms |
| GET | /tiposusina/1 | GET /tiposusina/1 - Hidrel�trica | ? | 20ms |

### API: usinas

**Testes:** 6 | **Sucessos:** 3 | **Taxa:** 50%

| M�todo | Endpoint | Descri��o | Status | Tempo |
|--------|----------|-----------|--------|-------|
| GET | /usinas | GET /usinas - Listar todas | ? | 9ms |
| GET | /usinas/300 | GET /usinas/300 - Buscar fict�cia | ? | 19ms |
| GET | /usinas/codigo/TEST-H01 | GET /usinas/codigo/TEST-H01 | ? | 18ms |
| GET | /usinas/empresa/200 | GET /usinas/empresa/200 | ? | 17ms |
| GET | /usinas/tipo/1 | GET /usinas/tipo/1 - Hidrel�tricas | ? | 21ms |
| POST | /usinas | POST /usinas - Criar nova | ? | 41ms |

---

## ?? DETALHAMENTO DE FALHAS

### ? GET /empresas/200 - Buscar fict�cia

- **M�todo:** GET
- **Endpoint:** /empresas/200
- **Status Code:** 404
- **Erro:** Response status code does not indicate success: 404 (Not Found).

### ? POST /empresas - Criar nova

- **M�todo:** POST
- **Endpoint:** /empresas
- **Status Code:** 400
- **Erro:** Response status code does not indicate success: 400 (Bad Request).

### ? PUT /empresas/200 - Atualizar

- **M�todo:** PUT
- **Endpoint:** /empresas/200
- **Status Code:** 400
- **Erro:** Response status code does not indicate success: 400 (Bad Request).

### ? GET /usinas/300 - Buscar fict�cia

- **M�todo:** GET
- **Endpoint:** /usinas/300
- **Status Code:** 404
- **Erro:** Response status code does not indicate success: 404 (Not Found).

### ? GET /usinas/codigo/TEST-H01

- **M�todo:** GET
- **Endpoint:** /usinas/codigo/TEST-H01
- **Status Code:** 400
- **Erro:** Response status code does not indicate success: 400 (Bad Request).

### ? POST /usinas - Criar nova

- **M�todo:** POST
- **Endpoint:** /usinas
- **Status Code:** 500
- **Erro:** Response status code does not indicate success: 500 (Internal Server Error).

### ? GET /semanaspmo/atual

- **M�todo:** GET
- **Endpoint:** /semanaspmo/atual
- **Status Code:** 404
- **Erro:** Response status code does not indicate success: 404 (Not Found).

### ? GET /semanaspmo/proximas?quantidade=4

- **M�todo:** GET
- **Endpoint:** /semanaspmo/proximas?quantidade=4
- **Status Code:** 404
- **Erro:** Response status code does not indicate success: 404 (Not Found).

### ? GET /restricoesug/100 - Fict�cia

- **M�todo:** GET
- **Endpoint:** /restricoesug/100
- **Status Code:** 404
- **Erro:** Response status code does not indicate success: 404 (Not Found).

### ? POST /restricoesug - Criar nova

- **M�todo:** POST
- **Endpoint:** /restricoesug
- **Status Code:** 500
- **Erro:** Response status code does not indicate success: 500 (Internal Server Error).

---

## ?? AN�LISE DE PERFORMANCE

| API | Tempo M�dio (ms) | Testes |
|-----|------------------|--------|
| arquivosdadger | 15.4 | 5 |
| cargas | 16 | 5 |
| empresas | 43.25 | 4 |
| equipespdp | 19.33 | 3 |
| restricoesug | 19.5 | 6 |
| semanaspmo | 18.33 | 6 |
| tiposusina | 13.5 | 2 |
| usinas | 20.83 | 6 |

---

## ? CHECKLIST DE VALIDA��O

- [?] GET Endpoints
- [?] POST Endpoints
- [?] PUT Endpoints
- [?] Todas as APIs testadas
- [?] Taxa de sucesso >= 90%
- [?] Tempo m�dio < 1000ms

---

## ?? CONCLUS�O

?? **SISTEMA REQUER CORRE��ES**

Taxa de sucesso abaixo de 90%. Revisar e corrigir falhas antes de continuar.

---

**Gerado automaticamente em:** 20/12/2025 22:11:59  
**Script:** Test-AllApis.ps1
