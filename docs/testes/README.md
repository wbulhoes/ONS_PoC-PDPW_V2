# ?? GUIA DE TESTES - ESTRUTURA MODULAR

**Versão:** 1.0  
**Data:** 19/12/2024  
**Status:** ? Implementado

---

## ?? ESTRUTURA DE DIRETÓRIOS

```
docs/testes/
??? README.md (este arquivo)
??? _TEMPLATE_TESTES_API.md (template para criar novos testes)
?
??? patterns/ (patterns reutilizáveis)
?   ??? PATTERN_GET_LISTA.md
?   ??? PATTERN_GET_ID.md
?   ??? PATTERN_POST_CREATE.md (TODO)
?   ??? PATTERN_PUT_UPDATE.md (TODO)
?   ??? PATTERN_DELETE.md (TODO)
?
??? apis/ (testes específicos por API)
    ??? API_USINA_TESTES.md ?
    ??? API_TIPO_USINA_TESTES.md (TODO)
    ??? API_EMPRESA_TESTES.md (TODO)
    ??? ... (26 mais - TODO)
```

---

## ?? COMO USAR

### Para criar testes de uma nova API:

1. **Copiar template:**
   ```bash
   cp _TEMPLATE_TESTES_API.md apis/API_NOME_TESTES.md
   ```

2. **Substituir placeholders:**
   - `{NOME_API}` ? Nome da API (ex: "TipoUsina")
   - `{Entidade}` ? Nome da entidade (ex: "TipoUsina")
   - `{QUANTIDADE}` ? Número de endpoints (ex: "5")
   - `{DATA}` ? Data de criação
   - `{entidade}` ? nome em lowercase (ex: "tiposusina")

3. **Preencher seções específicas:**
   - Dados de teste disponíveis
   - Endpoints customizados
   - Validações específicas da API

4. **Referenciar patterns:**
   - Adicionar links para patterns aplicáveis
   - Exemplo: "Ver `patterns/PATTERN_GET_LISTA.md`"

---

## ?? PATTERNS DISPONÍVEIS

### 1. PATTERN_GET_LISTA.md
**Aplicável a:** Todas as APIs  
**Descrição:** GET /api/{entidade} - Lista completa  
**Status:** ? Documentado

### 2. PATTERN_GET_ID.md
**Aplicável a:** Todas as APIs  
**Descrição:** GET /api/{entidade}/{id} - Buscar por ID  
**Status:** ? Documentado

### 3. PATTERN_POST_CREATE.md
**Aplicável a:** Todas as APIs  
**Descrição:** POST /api/{entidade} - Criar registro  
**Status:** ? TODO

### 4. PATTERN_PUT_UPDATE.md
**Aplicável a:** Todas as APIs  
**Descrição:** PUT /api/{entidade}/{id} - Atualizar  
**Status:** ? TODO

### 5. PATTERN_DELETE.md
**Aplicável a:** Todas as APIs  
**Descrição:** DELETE /api/{entidade}/{id} - Deletar (soft)  
**Status:** ? TODO

---

## ?? APIS DOCUMENTADAS

### Completas
1. ? **API_USINA_TESTES.md** (9 testes, 8 endpoints)

### Pendentes (28)
2. ? API_TIPO_USINA_TESTES.md
3. ? API_EMPRESA_TESTES.md
4. ? API_SEMANA_PMO_TESTES.md
5. ? API_UNIDADE_GERADORA_TESTES.md
6. ? API_PARADA_UG_TESTES.md
7. ? API_MOTIVO_RESTRICAO_TESTES.md
... (22 mais)

---

## ?? FLUXO DE TRABALHO

### Ao criar nova API:

```
1. Implementar API (Controller, Service, Repository, DTOs)
   ?
2. Copiar template de testes
   cp _TEMPLATE_TESTES_API.md apis/API_NOVA_TESTES.md
   ?
3. Preencher template com dados específicos
   - Seed data
   - Endpoints customizados
   - Validações específicas
   ?
4. Referenciar patterns aplicáveis
   - PATTERN_GET_LISTA.md
   - PATTERN_GET_ID.md
   - etc...
   ?
5. Executar testes no Swagger
   ?
6. Marcar checklist como completo ?
```

---

## ?? EXEMPLO DE USO

### Cenário: Testar API Usina

1. **Abrir guia específico:**
   ```
   docs/testes/apis/API_USINA_TESTES.md
   ```

2. **Iniciar API:**
   ```bash
   cd src/PDPW.API
   dotnet run
   ```

3. **Abrir Swagger:**
   ```
   http://localhost:PORTA/swagger
   ```

4. **Seguir checklist:**
   - [ ] Teste 1: GET lista todas
   - [ ] Teste 2: GET por ID
   - [ ] Teste 3: GET por código
   - [ ] ... (6 mais)

5. **Consultar patterns se necessário:**
   - Dúvida sobre GET lista? Ver `patterns/PATTERN_GET_LISTA.md`
   - Dúvida sobre 404? Ver `patterns/PATTERN_GET_ID.md`

---

## ?? BENEFÍCIOS DESTA ESTRUTURA

### ? Organização
- Cada API tem seu arquivo dedicado
- Fácil de encontrar e navegar
- Histórico de testes documentado

### ? Reutilização
- Patterns eliminam duplicação
- Template acelera criação de novos testes
- Conhecimento centralizado

### ? Consistência
- Todos os testes seguem mesmo formato
- Checklist padronizado
- Validações consistentes

### ? Escalabilidade
- Estrutura suporta 29 APIs facilmente
- Adicionar nova API = copiar template
- Patterns crescem com necessidades

### ? Manutenção
- Atualizar pattern = atualiza todas as APIs
- Fácil identificar o que está documentado
- Status claro (? ou ?)

---

## ?? CHECKLIST DE PATTERNS

### Patterns Básicos (todos os CRUDs)
- [x] ? PATTERN_GET_LISTA.md
- [x] ? PATTERN_GET_ID.md
- [ ] ? PATTERN_POST_CREATE.md
- [ ] ? PATTERN_PUT_UPDATE.md
- [ ] ? PATTERN_DELETE.md

### Patterns Avançados (casos específicos)
- [ ] ? PATTERN_GET_BUSCA_CUSTOMIZADA.md (ex: por código, CPF, etc)
- [ ] ? PATTERN_GET_LISTA_FILTRADA.md (ex: por tipo, empresa, etc)
- [ ] ? PATTERN_POST_VALIDACAO.md (validações complexas)
- [ ] ? PATTERN_RELACIONAMENTOS.md (Include, navigation)
- [ ] ? PATTERN_PAGINACAO.md (quando implementar)
- [ ] ? PATTERN_AUTENTICACAO.md (quando implementar)
- [ ] ? PATTERN_AUTORIZACAO.md (quando implementar)

---

## ?? ESTATÍSTICAS

```
Patterns documentados:    2/12 (17%)
APIs documentadas:        1/29 (3%)
Endpoints testados:       8/200+ (4%)
Cobertura de testes:      0% (ainda não automatizado)

Próximas prioridades:
1. Completar 5 patterns básicos
2. Documentar APIs TipoUsina e Empresa
3. Criar testes automatizados (xUnit)
```

---

## ?? CONVENÇÕES

### Nomenclatura
```
Arquivo de API:       API_{NOME}_TESTES.md
Pattern:              PATTERN_{TIPO}.md
Template:             _TEMPLATE_{TIPO}.md
```

### Prefixos
```
? = Completo e validado
? = TODO / Pendente
?? = Em progresso
? = Bloqueado / Não aplicável
```

### Estrutura do arquivo de teste
```markdown
1. Header (API, Entidade, Endpoints, Data)
2. Checklist rápido
3. Dados de teste disponíveis
4. Testes numerados (1, 2, 3...)
5. Comandos curl
6. Checklist completo
7. Observações
```

---

## ?? LINKS ÚTEIS

### Documentação Geral
- [Guia Completo Swagger](../GUIA_TESTES_API_USINA_SWAGGER.md)
- [Testes Rápidos CURL](../TESTES_RAPIDOS_CURL.md)
- [Resumo DIA 1](../RESUMO_FINAL_DIA1.md)

### APIs Relacionadas
- [API Usina Completa](../API_USINA_COMPLETA.md)
- [Seed Data](../SEED_DATA_CRIADO.md)

### Patterns
- [Pattern GET Lista](patterns/PATTERN_GET_LISTA.md)
- [Pattern GET ID](patterns/PATTERN_GET_ID.md)

---

## ?? DICAS

### Para testar rapidamente:
1. Use o **Swagger UI** (mais visual)
2. Consulte a **API específica** em `apis/`
3. Para dúvidas de pattern, veja `patterns/`

### Para criar novos testes:
1. Copie o **template**
2. Substitua os **placeholders**
3. Adicione **casos específicos**
4. Referencie **patterns** aplicáveis

### Para manter atualizado:
1. Marque como ? quando validado
2. Adicione novos patterns conforme necessário
3. Atualize estatísticas neste README

---

## ?? PRÓXIMOS PASSOS

### Imediato (DIA 1)
- [x] ? Criar estrutura de diretórios
- [x] ? Criar template base
- [x] ? Documentar 2 patterns (GET)
- [x] ? Documentar API Usina completa
- [x] ? Criar este README

### Curto Prazo (DIA 2-3)
- [ ] Completar 5 patterns básicos (POST, PUT, DELETE)
- [ ] Documentar APIs TipoUsina e Empresa
- [ ] Criar 3 patterns avançados

### Médio Prazo (SEMANA 1)
- [ ] Documentar 10 APIs principais
- [ ] Criar testes automatizados (xUnit)
- [ ] Implementar CI/CD com testes

### Longo Prazo (POC)
- [ ] Documentar todas as 29 APIs
- [ ] 100% cobertura de testes automatizados
- [ ] Performance benchmarks

---

## ? VALIDAÇÃO

Este sistema de testes foi validado com:
- ? API Usina (8 endpoints testados)
- ? 10 usinas reais do SIN
- ? Swagger UI funcionando
- ? CRUD completo testado
- ? Validações de negócio testadas

---

**Criado por:** GitHub Copilot + Desenvolvedor  
**Data:** 19/12/2024  
**Versão:** 1.0  
**Status:** ? IMPLEMENTADO

**ESTRUTURA DE TESTES MODULAR E ESCALÁVEL! ??**
