# ?? DISTRIBUI��O DE APIS - POC PDPW

**Projeto:** PoC Moderniza��o Sistema ONS  
**Vers�o:** 1.0  
**Data:** 19/12/2024  
**Status:** ? Em Desenvolvimento

---

## ?? ARQUITETURA DA POC

### **80% BACKEND + 20% FRONTEND**

```
???????????????????????????????????????????
? BACKEND (80% do esfor�o)                ?
?                                         ?
? ? 29 APIs RESTful (.NET 8)             ?
?    ?? 154 endpoints total               ?
?                                         ?
? Respons�veis:                           ?
? � [Willian] - 15 APIs (52%)             ?
? � [George] - 12 APIs (41%)              ?
? � [Pendente] - 2 APIs (7%)              ?
?                                         ?
???????????????????????????????????????????
              ? (Fornece dados)
???????????????????????????????????????????
? FRONTEND (20% do esfor�o)               ?
?                                         ?
? ? 1 Tela React/TypeScript              ?
?    ?? CRUD Cadastro de Usinas           ?
?                                         ?
? Consome:                                ?
? � API Usinas (Backend)                  ?
?                                         ?
???????????????????????????????????????????
```

---

## ?? BACKEND - 29 APIS RESTFUL

### Caracter�sticas
- **Tecnologia:** .NET 8 + Entity Framework Core
- **Padr�o:** Clean Architecture
- **API:** RESTful com Swagger
- **Banco:** SQL Server
- **Total:** 29 APIs, 154 endpoints

---

## ????? DISTRIBUI��O POR DESENVOLVEDOR

### **DEV 1: [Willian] - 15 APIs (52%)**

#### ?? Gest�o de Ativos (3 APIs)
| API | Endpoints | Status |
|-----|-----------|--------|
| 1. **Usinas** | 8 | ? Completa |
| 2. **Empresas** | 5 | ? Pendente |
| 3. **Tipos de Usina** | 5 | ? Pendente |

#### ?? Arquivos e Dados (5 APIs)
| API | Endpoints | Status |
|-----|-----------|--------|
| 4. **Arquivos DADGER** | 6 | ? Pendente |
| 5. **Valores DADGER** | 5 | ? Pendente |
| 6. **Semanas PMO** | 6 | ? Pendente |
| 7. **Cargas** | 5 | ? Pendente |
| 8. **Uploads** | 5 | ? Pendente |

#### ?? Gest�o de Equipes (3 APIs)
| API | Endpoints | Status |
|-----|-----------|--------|
| 9. **Equipes PDP** | 5 | ? Pendente |
| 10. **Usu�rios** | 5 | ? Pendente |
| 11. **Respons�veis** | 5 | ? Pendente |

**Subtotal Willian:** 15 APIs, 60 endpoints

---

### **DEV 2: [George] - 12 APIs (41%)**

#### ?? Gest�o de Ativos (1 API)
| API | Endpoints | Status |
|-----|-----------|--------|
| 12. **Unidades Geradoras** | 7 | ? Pendente |

#### ?? Restri��es e Paradas (4 APIs)
| API | Endpoints | Status |
|-----|-----------|--------|
| 13. **Paradas UG** | 6 | ? Pendente |
| 14. **Restri��es UG** | 5 | ? Pendente |
| 15. **Restri��es US** | 5 | ? Pendente |
| 16. **Motivos de Restri��o** | 5 | ? Pendente |

#### ? Opera��o e Gera��o (4 APIs)
| API | Endpoints | Status |
|-----|-----------|--------|
| 17. **Interc�mbio** | 5 | ? Pendente |
| 18. **Balan�o** | 5 | ? Pendente |
| 19. **Gera��o Fora M�rito** | 5 | ? Pendente |
| 20. **PDOC** | 6 | ? Pendente |

#### ?? Dados Consolidados (2 APIs)
| API | Endpoints | Status |
|-----|-----------|--------|
| 21. **DCA - Dados Agregados** | 6 | ? Pendente |
| 22. **DCR - Dados Consolidados** | 6 | ? Pendente |

**Subtotal George:** 12 APIs, 61 endpoints

---

### **[PENDENTE - Sem Respons�vel] - 2 APIs (7%)**

#### ?? Gest�o de Ativos (2 APIs)
| API | Endpoints | Status |
|-----|-----------|--------|
| 23. **Usinas Conversoras** | 5 | ? Sem respons�vel |
| 24. **Rampas Usina T�rmica** | 5 | ? Sem respons�vel |

#### ?? Restri��es e Paradas (2 APIs)
| API | Endpoints | Status |
|-----|-----------|--------|
| 25. **Inflexibilidade Contratada** | 5 | ? Sem respons�vel |
| 26. **Modalidade Op. T�rmica** | 5 | ? Sem respons�vel |

#### ?? Documentos e Relat�rios (4 APIs)
| API | Endpoints | Status |
|-----|-----------|--------|
| 27. **Diret�rios** | 5 | ? Sem respons�vel |
| 28. **Arquivos** | 5 | ? Sem respons�vel |
| 29. **Relat�rios** | 5 | ? Sem respons�vel |

**Subtotal Pendente:** 9 APIs, 33 endpoints

---

## ?? FRONTEND - 1 TELA DEMONSTRATIVA

### **DEV 3: [Frontend Developer]**

#### **Tela: Cadastro de Usinas** (React + TypeScript)

**Funcionalidades:**
- ? Listagem de usinas (tabela paginada)
- ? Visualizar detalhes
- ? Criar nova usina
- ? Editar usina existente
- ? Excluir usina (soft delete)

**Componentes:**
```
frontend/
??? src/
?   ??? pages/
?   ?   ??? Usinas/
?   ?       ??? UsinasList.tsx       (Listagem)
?   ?       ??? UsinaForm.tsx        (Criar/Editar)
?   ?       ??? UsinaDetail.tsx      (Detalhes)
?   ??? services/
?   ?   ??? usinaService.ts          (Integra��o API)
?   ??? types/
?   ?   ??? usina.types.ts           (TypeScript Types)
?   ??? components/
?       ??? Loading.tsx              (Componente Loading)
?       ??? ErrorMessage.tsx         (Tratamento erros)
?       ??? Table.tsx                (Tabela gen�rica)
```

**Integra��o com Backend:**
```typescript
// usinaService.ts
const API_URL = 'http://localhost:5000/api';

export const usinaService = {
  getAll: () => axios.get(`${API_URL}/usinas`),
  getById: (id) => axios.get(`${API_URL}/usinas/${id}`),
  create: (data) => axios.post(`${API_URL}/usinas`, data),
  update: (id, data) => axios.put(`${API_URL}/usinas/${id}`, data),
  delete: (id) => axios.delete(`${API_URL}/usinas/${id}`)
};
```

---

## ?? RESUMO ESTAT�STICO

### Backend
```
Total de APIs:        29
Total de Endpoints:   154
M�dia por API:        5.3 endpoints

Distribui��o por Dev:
?? [Willian]: 15 APIs (52%) - 60 endpoints
?? [George]:  12 APIs (41%) - 61 endpoints
?? [Pendente]: 2 APIs (7%)  - 33 endpoints

Status Atual:
?? Completas:    1 API (Usinas) ?
?? Em progresso: 26 APIs ?
?? Pendentes:    2 APIs ?
```

### Frontend
```
Total de Telas:       1
Funcionalidade:       CRUD Usinas
APIs Consumidas:      1 (API Usinas)
Componentes:          ~10
Tecnologia:           React 18 + TypeScript
```

---

## ?? PROGRESSO ATUAL

### Backend (29 APIs)
```
DIA 1:  ??????????  1/29 APIs (3.4%)
        ?? Usina completa ?

META:   ??????????  29/29 APIs (100%)
        ?? 27/12/2024
```

### Frontend (1 Tela)
```
DIA 1:  ??????????  0/1 Tela (0%)
        ?? N�o iniciado

META:   ??????????  1/1 Tela (100%)
        ?? 27/12/2024
```

---

## ?? INTEGRA��O BACKEND ? FRONTEND

```
BACKEND (.NET 8)               FRONTEND (React)
????????????????              ????????????????
?              ?              ?              ?
? API Usinas   ? ??? HTTP ??? ? Tela CRUD    ?
?              ?   JSON       ?              ?
? 8 endpoints  ?              ? 5 a��es      ?
?              ?              ?              ?
????????????????              ????????????????
     ?                              ?
????????????????              ????????????????
? SQL Server   ?              ? Axios/Fetch  ?
? 30 tabelas   ?              ? State Mgmt   ?
????????????????              ????????????????
```

---

## ?? PADR�O DE ENDPOINT

### Todos os endpoints seguem RESTful:

```
GET    /api/{entidade}                  - Lista todos
GET    /api/{entidade}/{id}             - Busca por ID
GET    /api/{entidade}/codigo/{codigo}  - Busca por c�digo (quando aplic�vel)
POST   /api/{entidade}                  - Criar
PUT    /api/{entidade}/{id}             - Atualizar
DELETE /api/{entidade}/{id}             - Deletar (soft delete)
```

### Exemplo: API Usinas
```
GET    /api/usinas                      - Lista 10 usinas
GET    /api/usinas/1                    - Busca Itaipu
GET    /api/usinas/codigo/UHE-ITAIPU   - Busca por c�digo
GET    /api/usinas/tipo/1               - Hidrel�tricas
GET    /api/usinas/empresa/2            - Usinas da Eletronorte
POST   /api/usinas                      - Criar nova
PUT    /api/usinas/1                    - Atualizar Itaipu
DELETE /api/usinas/1                    - Remover Itaipu
```

---

## ?? AGRUPAMENTO DAS APIS

### ?? **Gest�o de Ativos** (6 APIs, 31 endpoints)
- Usinas [Willian] ?
- Empresas [Willian] ?
- Tipos de Usina [Willian] ?
- Unidades Geradoras [George] ?
- Usinas Conversoras [Pendente] ?
- Rampas Usina T�rmica [Pendente] ?

### ?? **Arquivos e Dados** (5 APIs, 31 endpoints)
- Arquivos DADGER [Willian] ?
- Valores DADGER [Willian] ?
- Semanas PMO [Willian] ?
- Cargas [Willian] ?
- Uploads [Willian] ?

### ?? **Restri��es e Paradas** (6 APIs, 30 endpoints)
- Paradas UG [George] ?
- Restri��es UG [George] ?
- Restri��es US [George] ?
- Motivos de Restri��o [George] ?
- Inflexibilidade Contratada [Pendente] ?
- Modalidade Op. T�rmica [Pendente] ?

### ? **Opera��o e Gera��o** (4 APIs, 21 endpoints)
- Interc�mbio [George] ?
- Balan�o [George] ?
- Gera��o Fora M�rito [George] ?
- PDOC [George] ?

### ?? **Dados Consolidados** (2 APIs, 12 endpoints)
- DCA - Dados Agregados [George] ?
- DCR - Dados Consolidados [George] ?

### ?? **Gest�o de Equipes** (3 APIs, 15 endpoints)
- Equipes PDP [Willian] ?
- Usu�rios [Willian] ?
- Respons�veis [Willian] ?

### ?? **Documentos e Relat�rios** (4 APIs, 19 endpoints)
- Diret�rios [Pendente] ?
- Arquivos [Pendente] ?
- Relat�rios [Pendente] ?
- Observa��es [Pendente] ?

---

## ?? CRONOGRAMA

### DIA 1 (19/12) - ? COMPLETO
```
? Infraestrutura 100%
? 29 Entidades Domain
? 30 Tabelas database
? API Usinas completa
? Documenta��o
```

### DIA 2 (20/12) - ?? EM PROGRESSO
```
[Willian]:
? API TipoUsina
? API Empresa
? API SemanaPMO

[George]:
? API UnidadeGeradora
? API ParadaUG

Meta: 5 APIs funcionais
```

### DIA 3-6 (23-27/12)
```
Objetivo: 29 APIs + 1 Frontend

DIA 3: 11 APIs + Frontend iniciado
DIA 4: 22 APIs + CRUD completo
DIA 5: 27 APIs + Frontend polido
DIA 6: 29 APIs + Apresenta��o final ?
```

---

## ?? PROPOR��O DA POC

```
Backend:  29 APIs  = 80% do esfor�o
Frontend:  1 Tela  = 20% do esfor�o

Justificativa:
? APIs = Base reutiliz�vel (Web, Mobile, Desktop)
? Backend robusto = Funda��o do sistema
? Frontend = Prova de conceito integra��o
? Swagger = Testes sem depender do frontend
```

---

## ?? OBSERVA��ES IMPORTANTES

### Por que TODAS as APIs s�o Backend?

1. ? S�o **RESTful Services** (.NET 8)
2. ? Retornam **JSON** via HTTP
3. ? N�o t�m **interface visual**
4. ? Acessam **banco de dados**
5. ? Implementam **l�gica de neg�cio**
6. ? Podem ser consumidas por **qualquer frontend**

### O Frontend � separado porque:

1. ? � **React/TypeScript**
2. ? Tem **interface visual** (HTML/CSS)
3. ? Roda no **navegador**
4. ? **Consome** APIs backend via HTTP
5. ? Gerencia **estado** do usu�rio
6. ? Respons�vel pela **UX/UI**

---

## ?? TECNOLOGIAS

### Backend
```
� .NET 8
� Entity Framework Core 8.0
� SQL Server 2022
� AutoMapper
� Swagger/OpenAPI
� Clean Architecture
� Repository Pattern
```

### Frontend
```
� React 18
� TypeScript
� Vite
� Axios
� React Router
� Styled Components (ou CSS Modules)
```

---

## ?? ESTAT�STICAS FINAIS

```
?????????????????????????????????????????????
?                                           ?
?  ?? TOTAL DA POC                          ?
?                                           ?
?  Backend:                                 ?
?  � 29 APIs RESTful                        ?
?  � 154 endpoints                          ?
?  � 30 tabelas database                    ?
?  � 29 entidades Domain                    ?
?                                           ?
?  Frontend:                                ?
?  � 1 Tela CRUD completa                   ?
?  � ~10 componentes React                  ?
?  � 1 API consumida                        ?
?                                           ?
?  Propor��o: 80% Backend / 20% Frontend    ?
?                                           ?
?????????????????????????????????????????????
```

---

## ?? PR�XIMOS PASSOS

### Imediato (Hoje - 20/12)
- [ ] [Willian] Iniciar API TipoUsina
- [ ] [Willian] Iniciar API Empresa
- [ ] [George] Iniciar API UnidadeGeradora
- [ ] [George] Iniciar API ParadaUG
- [ ] [Frontend] Setup React + Estrutura

### Curto Prazo (23-24/12)
- [ ] 11 APIs funcionais
- [ ] Frontend com listagem
- [ ] Integra��o backend-frontend

### Entrega Final (27/12)
- [ ] 29 APIs completas
- [ ] 1 Frontend completo
- [ ] Apresenta��o ao cliente
- [ ] Documenta��o finalizada

---

## ?? CONTATOS

| Dev | Responsabilidade | APIs |
|-----|------------------|------|
| **Willian** | Backend Senior | 15 APIs |
| **George** | Backend Pleno | 12 APIs |
| **Frontend Dev** | Frontend React | 1 Tela |

---

## ?? DOCUMENTA��O DISPON�VEL

- `docs/API_USINA_COMPLETA.md` - Documenta��o API Usinas
- `docs/GUIA_DOCKER_DAILY.md` - Docker para apresenta��es
- `docs/APRESENTACAO_DAILY_DIA1.md` - Apresenta��o Daily
- `docs/CRONOGRAMA_DETALHADO_V2.md` - Cronograma completo
- `docs/testes/` - Estrutura de testes modulares

---

**Criado por:** Squad PDPW  
**Data:** 19/12/2024  
**Vers�o:** 1.0  
**Status:** ? Em Desenvolvimento

**VAMOS FAZER ESSA POC ACONTECER! ??**
