# Cronograma de Desenvolvimento - PDPW PoC

**Período:** 17/12/2025 a 26/12/2025 (9 dias úteis)  
**Entrega:** 26/12/2025  
**Apresentação:** 05/01/2026

---

## 📅 Dia 1-2: Setup e Análise (17-18/12)

### ✅ Concluído
- [x] Estrutura de pastas criada
- [x] Backend .NET 8 com Clean Architecture
- [x] Frontend React + TypeScript
- [x] Docker Compose configurado
- [x] Documentação inicial

### 🎯 Tarefas Restantes
- [ ] Obter código legado VB.NET/WebForms do ONS
- [ ] Analisar banco de dados atual
- [ ] Documentar principais fluxos de negócio
- [ ] Executar aplicação legada localmente (se possível)
- [ ] Identificar telas/funcionalidades prioritárias

### 📝 Entregas do Dia
- Documentação de análise do legado
- Mapeamento de entidades/tabelas
- Definição de escopo (vertical slice)

---

## 💻 Dia 3-4: Backend Core (19-20/12)

### 🎯 Objetivos
- [ ] Migrar modelo de dados completo
- [ ] Implementar migrations do EF Core
- [ ] Criar todos os repositórios necessários
- [ ] Implementar serviços de negócio
- [ ] Adicionar validações de regras de negócio

### 📋 Checklist Backend
- [ ] Entidades de domínio completas
- [ ] Interfaces de repositório
- [ ] DTOs com validações (DataAnnotations)
- [ ] Serviços de aplicação com lógica migrada do VB.NET
- [ ] Controllers com todos os endpoints
- [ ] Testes unitários básicos (xUnit)

### 📝 Entregas do Dia
- API funcional com CRUD completo
- Documentação Swagger atualizada
- Collection do Postman para testes

---

## 🎨 Dia 5-6: Frontend (21-22/12)

### 🎯 Objetivos
- [ ] Replicar telas do WebForms original
- [ ] Implementar todos os componentes React
- [ ] Integrar com API backend
- [ ] Adicionar validações de formulário
- [ ] Implementar tratamento de erros
- [ ] Melhorar responsividade

### 📋 Checklist Frontend
- [ ] Tela de listagem com filtros
- [ ] Formulário de criação/edição
- [ ] Modal de confirmação
- [ ] Indicadores de loading
- [ ] Mensagens de erro/sucesso
- [ ] Navegação entre páginas
- [ ] CSS próximo ao visual legado

### 📝 Entregas do Dia
- Interface completa e funcional
- Capturas de tela comparando com o sistema legado
- Manual básico de usuário

---

## 🐳 Dia 7: Containerização (23/12)

### 🎯 Objetivos
- [ ] Testar build Windows Container
- [ ] Otimizar Dockerfiles
- [ ] Configurar docker-compose completo
- [ ] Adicionar healthchecks
- [ ] Testar em ambiente limpo
- [ ] Documentar processo de deploy

### 📋 Checklist Docker
- [ ] Backend containerizado funcionando
- [ ] Frontend containerizado com Nginx
- [ ] SQL Server em container
- [ ] Rede Docker configurada
- [ ] Volumes persistentes
- [ ] Scripts de inicialização

### 📝 Entregas do Dia
- Docker Compose funcional
- Guia de implantação (deploy)
- Solução de problemas comum documentada

---

## 🧪 Dia 8: Testes e Refinamento (24/12)

### 🎯 Objetivos
- [ ] Testes unitários (mínimo 60% cobertura)
- [ ] Testes de integração
- [ ] Testes E2E (fim a fim) básicos
- [ ] Corrigir bugs encontrados
- [ ] Validar regras de negócio
- [ ] Performance tuning

### 📋 Checklist Testes
- [ ] Testes unitários de serviços
- [ ] Testes de repositórios
- [ ] Testes de controllers
- [ ] Testes de componentes React
- [ ] Testes de integração API
- [ ] Cenários de erro testados

### 📝 Entregas do Dia
- Suite de testes executando
- Relatório de cobertura
- Lista de bugs corrigidos

---

## 📄 Dia 9: Documentação e Preparação (26/12)

### 🎯 Objetivos
- [ ] Documentação técnica completa
- [ ] Preparar apresentação
- [ ] Criar demonstração gravada (backup)
- [ ] README detalhado
- [ ] Guia de manutenção
- [ ] Commit final no GitHub

### 📋 Checklist Final
- [ ] README.md completo
- [ ] Arquitetura documentada (diagramas)
- [ ] Decisões técnicas justificadas
- [ ] Comparativo antes/depois
- [ ] Estimativa de projeto completo
- [ ] Apresentação em slides (PPT/PDF)

### 📝 Entregas do Dia
- **Código no GitHub**
- **Documentação completa**
- **Apresentação preparada**
- **Vídeo demo (opcional)**

---

## 🎯 Entrega Final (26/12/2025)

### 📦 Pacote de Entrega
1. **Código Fonte**
   - Repositório GitHub organizado
   - Branches: main, develop
   - Tags: v1.0-poc

2. **Documentação**
   - README.md
   - SETUP.md
   - ARCHITECTURE.md
   - MIGRATION_GUIDE.md

3. **Apresentação**
   - Slides (15-20 páginas)
   - Demo ao vivo (10 min)
   - Vídeo backup (5 min)

4. **Extras**
   - Comparativo de tecnologias
   - Estimativa de projeto completo
   - Riscos e mitigações
   - Roadmap futuro

---

## 📊 Apresentação (05/01/2026)

### 🎤 Estrutura Sugerida (30 min)
1. **Contexto** (5 min)
   - Sistema legado vs. moderno
   - Desafios técnicos
   - Objetivos da PoC

2. **Solução Técnica** (10 min)
   - Arquitetura escolhida
   - Stack tecnológico
   - Decisões importantes

3. **Demonstração** (10 min)
   - Sistema funcionando
   - Fluxos implementados
   - Comparativo com legado

4. **Próximos Passos** (5 min)
   - Estimativa de projeto completo
   - Cronograma sugerido
   - Riscos e considerações

---

## ⚠️ Riscos e Contingências

### Alto Risco
- **Acesso ao código legado atrasado**
   - Contingência: usar sistema exemplo genérico
  
- **Contêineres Windows com problemas**
   - Contingência: demonstrar em contêiner Linux

### Médio Risco
- **Regras de negócio complexas**
   - Contingência: simplificar para PoC
  
- **Integração com sistemas externos**
   - Contingência: simular dependências (mock)

---

## 📈 KPIs de Sucesso

- ✅ Aplicação funcional end-to-end
- ✅ Código no GitHub até 26/12
- ✅ Docker Compose executando
- ✅ Documentação clara
- ✅ Apresentação preparada
- ✅ Demonstração sem erros
- ✅ Estimativa de projeto real entregue (até 12/01)

---

**Status Atual:** ✅ Estrutura base criada (17/12)  
**Próximo Marco:** 🔄 Análise do legado e setup (18/12)
