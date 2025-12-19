# ?? DISTRIBUIÇÃO DE APIS - POC PDPW

**Projeto:** PoC Modernização Sistema ONS  
**Versão:** 1.0  
**Data:** 19/12/2024  
**Status:** ? Em Desenvolvimento

---

## ?? ARQUITETURA DA POC

### **80% BACKEND + 20% FRONTEND**

```
???????????????????????????????????????????
? BACKEND (80% do esforço)                ?
?                                         ?
? ? 29 APIs RESTful (.NET 8)             ?
?    ?? 154 endpoints total               ?
?                                         ?
? Responsáveis:                           ?
? • [Willian] - 15 APIs (52%)             ?
? • [George] - 12 APIs (41%)              ?
? • [Pendente] - 2 APIs (7%)              ?
?                                         ?
???????????????????????????????????????????
              ? (Fornece dados)
???????????????????????????????????????????
? FRONTEND (20% do esforço)               ?
?                                         ?
? ? 1 Tela React/TypeScript              ?
?    ?? CRUD Cadastro de Usinas           ?
?                                         ?
? Consome:                                ?
? • API Usinas (Backend)                  ?
?                                         ?
???????????????????????????????????????????
```

---

## ?? BACKEND - 29 APIS RESTFUL

### Características
- **Tecnologia:** .NET 8 + Entity Framework Core
- **Padrão:** Clean Architecture
- **API:** RESTful com Swagger
- **Banco:** SQL Server
- **Total:** 29 APIs, 154 endpoints

---

## ????? DISTRIBUIÇÃO POR DESENVOLVEDOR

### **DEV 1: [Willian] - 15 APIs (52%)**

#### ?? Gestão de Ativos (3 APIs)
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

#### ?? Gestão de Equipes (3 APIs)
| API | Endpoints | Status |
|-----|-----------|--------|
| 9. **Equipes PDP** | 5 | ? Pendente |
| 10. **Usuários** | 5 | ? Pendente |
| 11. **Responsáveis** | 5 | ? Pendente |

**Subtotal Willian:** 15 APIs, 60 endpoints

---

### **DEV 2: [George] - 12 APIs (41%)**

#### ?? Gestão de Ativos (1 API)
| API | Endpoints | Status |
|-----|-----------|--------|
| 12. **Unidades Geradoras** | 7 | ? Pendente |

#### ?? Restrições e Paradas (4 APIs)
| API | Endpoints | Status |
|-----|-----------|--------|
| 13. **Paradas UG** | 6 | ? Pendente |
| 14. **Restrições UG** | 5 | ? Pendente |
| 15. **Restrições US** | 5 | ? Pendente |
| 16. **Motivos de Restrição** | 5 | ? Pendente |

#### ? Operação e Geração (4 APIs)
| API | Endpoints | Status |
|-----|-----------|--------|
| 17. **Intercâmbio** | 5 | ? Pendente |
| 18. **Balanço** | 5 | ? Pendente |
| 19. **Geração Fora Mérito** | 5 | ? Pendente |
| 20. **PDOC** | 6 | ? Pendente |

#### ?? Dados Consolidados (2 APIs)
| API | Endpoints | Status |
|-----|-----------|--------|
| 21. **DCA - Dados Agregados** | 6 | ? Pendente |
| 22. **DCR - Dados Consolidados** | 6 | ? Pendente |

**Subtotal George:** 12 APIs, 61 endpoints

---

### **[PENDENTE - Sem Responsável] - 2 APIs (7%)**

#### ?? Gestão de Ativos (2 APIs)
| API | Endpoints | Status |
|-----|-----------|--------|
| 23. **Usinas Conversoras** | 5 | ? Sem responsável |
| 24. **Rampas Usina Térmica** | 5 | ? Sem responsável |

#### ?? Restrições e Paradas (2 APIs)
| API | Endpoints | Status |
|-----|-----------|--------|
| 25. **Inflexibilidade Contratada** | 5 | ? Sem responsável |
| 26. **Modalidade Op. Térmica** | 5 | ? Sem responsável |

#### ?? Documentos e Relatórios (4 APIs)
| API | Endpoints | Status |
|-----|-----------|--------|
| 27. **Diretórios** | 5 | ? Sem responsável |
| 28. **Arquivos** | 5 | ? Sem responsável |
| 29. **Relatórios** | 5 | ? Sem responsável |

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
?   ?   ??? usinaService.ts          (Integração API)
?   ??? types/
?   ?   ??? usina.types.ts           (TypeScript Types)
?   ??? components/
?       ??? Loading.tsx              (Componente Loading)
?       ??? ErrorMessage.tsx         (Tratamento erros)
?       ??? Table.tsx                (Tabela genérica)
```

**Integração com Backend:**
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

## ?? RESUMO ESTATÍSTICO

### Backend
```
Total de APIs:        29
Total de Endpoints:   154
Média por API:        5.3 endpoints

Distribuição por Dev:
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
        ?? Não iniciado

META:   ??????????  1/1 Tela (100%)
        ?? 27/12/2024
```

---

## ?? INTEGRAÇÃO BACKEND ? FRONTEND

```
BACKEND (.NET 8)               FRONTEND (React)
????????????????              ????????????????
?              ?              ?              ?
? API Usinas   ? ??? HTTP ??? ? Tela CRUD    ?
?              ?   JSON       ?              ?
? 8 endpoints  ?              ? 5 ações      ?
?              ?              ?              ?
????????????????              ????????????????
     ?                              ?
????????????????              ????????????????
? SQL Server   ?              ? Axios/Fetch  ?
? 30 tabelas   ?              ? State Mgmt   ?
????????????????              ????????????????
```

---

## ?? PADRÃO DE ENDPOINT

### Todos os endpoints seguem RESTful:

```
GET    /api/{entidade}                  - Lista todos
GET    /api/{entidade}/{id}             - Busca por ID
GET    /api/{entidade}/codigo/{codigo}  - Busca por código (quando aplicável)
POST   /api/{entidade}                  - Criar
PUT    /api/{entidade}/{id}             - Atualizar
DELETE /api/{entidade}/{id}             - Deletar (soft delete)
```

### Exemplo: API Usinas
```
GET    /api/usinas                      - Lista 10 usinas
GET    /api/usinas/1                    - Busca Itaipu
GET    /api/usinas/codigo/UHE-ITAIPU   - Busca por código
GET    /api/usinas/tipo/1               - Hidrelétricas
GET    /api/usinas/empresa/2            - Usinas da Eletronorte
POST   /api/usinas                      - Criar nova
PUT    /api/usinas/1                    - Atualizar Itaipu
DELETE /api/usinas/1                    - Remover Itaipu
```

---

## ?? AGRUPAMENTO DAS APIS

### ?? **Gestão de Ativos** (6 APIs, 31 endpoints)
- Usinas [Willian] ?
- Empresas [Willian] ?
- Tipos de Usina [Willian] ?
- Unidades Geradoras [George] ?
- Usinas Conversoras [Pendente] ?
- Rampas Usina Térmica [Pendente] ?

### ?? **Arquivos e Dados** (5 APIs, 31 endpoints)
- Arquivos DADGER [Willian] ?
- Valores DADGER [Willian] ?
- Semanas PMO [Willian] ?
- Cargas [Willian] ?
- Uploads [Willian] ?

### ?? **Restrições e Paradas** (6 APIs, 30 endpoints)
- Paradas UG [George] ?
- Restrições UG [George] ?
- Restrições US [George] ?
- Motivos de Restrição [George] ?
- Inflexibilidade Contratada [Pendente] ?
- Modalidade Op. Térmica [Pendente] ?

### ? **Operação e Geração** (4 APIs, 21 endpoints)
- Intercâmbio [George] ?
- Balanço [George] ?
- Geração Fora Mérito [George] ?
- PDOC [George] ?

### ?? **Dados Consolidados** (2 APIs, 12 endpoints)
- DCA - Dados Agregados [George] ?
- DCR - Dados Consolidados [George] ?

### ?? **Gestão de Equipes** (3 APIs, 15 endpoints)
- Equipes PDP [Willian] ?
- Usuários [Willian] ?
- Responsáveis [Willian] ?

### ?? **Documentos e Relatórios** (4 APIs, 19 endpoints)
- Diretórios [Pendente] ?
- Arquivos [Pendente] ?
- Relatórios [Pendente] ?
- Observações [Pendente] ?

---

## ?? CRONOGRAMA

### DIA 1 (19/12) - ? COMPLETO
```
? Infraestrutura 100%
? 29 Entidades Domain
? 30 Tabelas database
? API Usinas completa
? Documentação
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
DIA 6: 29 APIs + Apresentação final ?
```

---

## ?? PROPORÇÃO DA POC

```
Backend:  29 APIs  = 80% do esforço
Frontend:  1 Tela  = 20% do esforço

Justificativa:
? APIs = Base reutilizável (Web, Mobile, Desktop)
? Backend robusto = Fundação do sistema
? Frontend = Prova de conceito integração
? Swagger = Testes sem depender do frontend
```

---

## ?? OBSERVAÇÕES IMPORTANTES

### Por que TODAS as APIs são Backend?

1. ? São **RESTful Services** (.NET 8)
2. ? Retornam **JSON** via HTTP
3. ? Não têm **interface visual**
4. ? Acessam **banco de dados**
5. ? Implementam **lógica de negócio**
6. ? Podem ser consumidas por **qualquer frontend**

### O Frontend é separado porque:

1. ? É **React/TypeScript**
2. ? Tem **interface visual** (HTML/CSS)
3. ? Roda no **navegador**
4. ? **Consome** APIs backend via HTTP
5. ? Gerencia **estado** do usuário
6. ? Responsável pela **UX/UI**

---

## ?? TECNOLOGIAS

### Backend
```
• .NET 8
• Entity Framework Core 8.0
• SQL Server 2022
• AutoMapper
• Swagger/OpenAPI
• Clean Architecture
• Repository Pattern
```

### Frontend
```
• React 18
• TypeScript
• Vite
• Axios
• React Router
• Styled Components (ou CSS Modules)
```

---

## ?? ESTATÍSTICAS FINAIS

```
?????????????????????????????????????????????
?                                           ?
?  ?? TOTAL DA POC                          ?
?                                           ?
?  Backend:                                 ?
?  • 29 APIs RESTful                        ?
?  • 154 endpoints                          ?
?  • 30 tabelas database                    ?
?  • 29 entidades Domain                    ?
?                                           ?
?  Frontend:                                ?
?  • 1 Tela CRUD completa                   ?
?  • ~10 componentes React                  ?
?  • 1 API consumida                        ?
?                                           ?
?  Proporção: 80% Backend / 20% Frontend    ?
?                                           ?
?????????????????????????????????????????????
```

---

## ?? PRÓXIMOS PASSOS

### Imediato (Hoje - 20/12)
- [ ] [Willian] Iniciar API TipoUsina
- [ ] [Willian] Iniciar API Empresa
- [ ] [George] Iniciar API UnidadeGeradora
- [ ] [George] Iniciar API ParadaUG
- [ ] [Frontend] Setup React + Estrutura

### Curto Prazo (23-24/12)
- [ ] 11 APIs funcionais
- [ ] Frontend com listagem
- [ ] Integração backend-frontend

### Entrega Final (27/12)
- [ ] 29 APIs completas
- [ ] 1 Frontend completo
- [ ] Apresentação ao cliente
- [ ] Documentação finalizada

---

## ?? CONTATOS

| Dev | Responsabilidade | APIs |
|-----|------------------|------|
| **Willian** | Backend Senior | 15 APIs |
| **George** | Backend Pleno | 12 APIs |
| **Frontend Dev** | Frontend React | 1 Tela |

---

## ?? DOCUMENTAÇÃO DISPONÍVEL

- `docs/API_USINA_COMPLETA.md` - Documentação API Usinas
- `docs/GUIA_DOCKER_DAILY.md` - Docker para apresentações
- `docs/APRESENTACAO_DAILY_DIA1.md` - Apresentação Daily
- `docs/CRONOGRAMA_DETALHADO_V2.md` - Cronograma completo
- `docs/testes/` - Estrutura de testes modulares

---

**Criado por:** Squad PDPW  
**Data:** 19/12/2024  
**Versão:** 1.0  
**Status:** ? Em Desenvolvimento

**VAMOS FAZER ESSA POC ACONTECER! ??**
