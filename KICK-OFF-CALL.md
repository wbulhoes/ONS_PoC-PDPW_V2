# 📋 Kick-off PDPW PoC - Guia de Reunião (< 1h)

**Data:** 17/12/2025  
**Participantes:** ONS (Cliente), 2 Devs, Arquiteto, SDM, QA  
**Duração:** 50 minutos  
**Saída esperada:** Escopo, riscos, acesso a recursos, cronograma confirmado

---

## 🎯 **Top 5 perguntas (primeiros 15 min)**

### SDM / Arquiteto - Escopo Executivo
**Tempo: 3 min**

```
□ Qual ÚNICO fluxo devemos demonstrar até 26/12?
  ➜ ☐ Coleta dados
  ➜ ☐ Processamento/validação
  ➜ ☐ Consulta/relatório
  ➜ ☐ Outro: _________________

□ Esse fluxo precisa funcionar COMPLETO (backend + frontend)?
  ➜ ☐ Sim, full-stack
  ➜ ☐ Só backend + Swagger é suficiente
  ➜ ☐ Flexível, depende do que couber

□ Qual é o risco #1 que pode derrubar este PoC?
  ➜ Resposta: _________________
```

### Arquiteto - Infraestrutura
**Tempo: 2 min**

```
□ Vocês preferem Windows ou Linux Containers?
  ➜ ☐ Windows (já assumimos)
  ➜ ☐ Linux é aceitável
  ➜ ☐ Indiferente

□ Banco de dados: SQL Server local ou precisa rodar em container?
  ➜ ☐ Container (docker-compose)
  ➜ ☐ SQL Server já instalado
  ➜ ☐ Mockar (fase 1)
```

### Dev Lead / SDM - Acesso
**Tempo: 5 min**

```
□ CRÍTICO: Quando temos acesso ao código legado PDPW?
  ➜ Data: _________________ 
  ➜ Como: ☐ GitHub ☐ ZIP ☐ Outro: _____
  ➜ Quem dá permissão: _________________

□ Temos acesso a um ambiente do banco de dados LEGADO?
  ➜ ☐ Sim, com dados reais
  ➜ ☐ Sim, mas vazio
  ➜ ☐ Não, começamos do zero
  ➜ Contato para acesso: _________________

□ Alguém do ONS está disponível para validar funcionamento?
  ➜ ☐ Sim, quem: _________________ (telefone/email)
  ➜ ☐ Não, você (SDM) valida
  ➜ Disponibilidade para testes: ________________
```

### QA - Requisitos de teste
**Tempo: 3 min**

```
□ Até 26/12, qual é a expectativa de testes?
  ➜ ☐ Testes básicos (teste de fumaça / smoke test)
  ➜ ☐ Testes completos de funcionalidade
  ➜ ☐ Sem testes automatizados (manual apenas)
  ➜ ☐ Testes de performance

□ Sistema precisa de autenticação/segurança no PoC?
  ➜ ☐ Sim, é crítico
  ➜ ☐ Não, deixa sem (fase 2)
  ➜ ☐ Básico (login simples)
```

---

## 🔧 **PERGUNTAS DETALHADAS (Próximos 25 min)**

### Dev #1 - Banco de Dados & Backend
**Tempo: 10 min**

```
LEGADO
□ Qual é a versão do SQL Server atual?
  ➜ ________________

□ Quantas tabelas/stored procedures o sistema tem?
  ➜ Tabelas: _____ | Procedures: _____
  ➜ Quem é o DBA: _________________

□ Quais são as TOP 3 tabelas que precisamos migrar para o PoC?
  ➜ 1) _________________
  ➜ 2) _________________
  ➜ 3) _________________

NOVOS REQUISITOS
□ Há validações/regras de negócio específicas?
  ➜ ☐ Sim. Quais (ou documentar separado):
     _________________
  ➜ ☐ Não, usar bom senso

□ Precisa integrar com outros sistemas AGORA?
  ➜ ☐ Sim. Quais e como: _________________
  ➜ ☐ Não, mockar tudo

□ Relatórios/exportação são necessários no PoC?
  ➜ ☐ Sim (CSV/PDF/Excel) → incluir
  ➜ ☐ Não, deixa para fase 2
```

### Dev #2 - Frontend e UI
**Tempo: 8 min**

```
SISTEMA LEGADO
□ Qual tela do WebForms é o "MVP" (mínimo viável) para replicar?
  ➜ Tela: _________________
  ➜ Quantos campos/funcionalidades: _____

□ Precisa parecer IGUAL ao legado ou só funcional?
  ➜ ☐ Igual (respeitar UI/UX)
  ➜ ☐ Modernizado (nova aparência)
  ➜ ☐ Funcional (design é secundário)

□ Há dados/menus que devem aparecer específicos?
  ➜ Detalhe: _________________

FUNCIONALIDADES
□ Quais são as 3 ações principais do usuário na tela?
  ➜ 1) _________________
  ➜ 2) _________________
  ➜ 3) _________________

□ Precisa de buscas/filtros avançados?
  ➜ ☐ Sim, detalhe: _________________
  ➜ ☐ Não
```

### Arquiteto - Decisões técnicas
**Tempo: 7 min**

```
STACK
□ .NET 8 + React + Docker é ACEITO ou há restrições?
  ➜ ☐ Sim, aprovado
  ➜ ☐ Precisa de aprovação ☐ De quem: ___
  ➜ ☐ Há alternativa preferida: _____

Implantação (deploy)
□ Como será feita a implantação (deploy) do PoC para apresentação?
  ➜ ☐ Docker Compose local
  ➜ ☐ Azure/Cloud (onde: ____________)
  ➜ ☐ On-premises
  ➜ Quem faz o deploy: _________________

□ Há CI/CD pipeline já em uso?
  ➜ ☐ GitHub Actions
  ➜ ☐ Azure DevOps
  ➜ ☐ Jenkins
  ➜ ☐ Não, configurar depois
  ➜ Contato tech: _________________

DOCUMENTAÇÃO
□ Qual nível de documentação é esperado?
  ➜ ☐ README + Swagger (básico)
  ➜ ☐ + Architecture Decision Records (ADR)
  ➜ ☐ + Guia de manutenção completo
```

---

## ⚠️ **Riscos e contingências (5 min)**

### Todos - Rápido

```
□ Se não conseguirmos tudo até 26/12, qual é a PRIORIDADE?
  ➜ Ordem: 
     1) Backend API funcional
     2) Frontend conectado
     3) Testes automatizados
     4) Docker rodando
     ☐ Outra ordem: _________________

□ Há 3 BLOQUEADORES que PRECISAMOS RESOLVER HOJE?
  ➜ 1) _________________
  ➜ 2) _________________
  ➜ 3) _________________

□ Equipe está 100% disponível até 26/12?
  ➜ ☐ Sim, full-time
  ➜ ☐ Parcial. Detalhe: _________________
  ➜ ☐ Há férias/ausências: _________________
```

---

## 📦 **Próximos passos (últimos 5 min)**

**Até amanhã 09h:**
```
☐ Enviar código legado ao Dev Lead (ou acesso ao repo)
☐ Enviar schema do banco de dados (DER/DDL)
☐ Confirmar contatos de suporte do ONS (validação)
☐ Confirmar acesso a SQL Server (local ou container?)
```

**Até próxima sexta (20/12):**
```
☐ Primeira versão funcional do backend (CRUD)
☐ Componentes React básicos conectados
☐ Docker Compose rodando sem erros
☐ Ata de reunião + checklist preenchido
```

**Até 26/12:**
```
☐ Sistema funcional end-to-end
☐ Testes passando
☐ Documentação completa
☐ GitHub com commit final
```

---

## 📞 **CONTATOS CRÍTICOS**

```
ONS - Cliente
├─ Product Owner: _____________ ( )
├─ DBA/Infraestrutura: _____________ ( )
└─ Validação Funcional: _____________ ( )

NOSSA EQUIPE
├─ Dev Backend: _____________ ( )
├─ Dev Frontend: _____________ ( )
├─ Arquiteto: _____________ ( )
├─ QA: _____________ ( )
└─ SDM: _____________ ( )
```

---

## ✅ **DECISÕES REGISTRADAS**

```
ESCOPO FINAL
Fluxo prioritário: _________________________________
Prazo: 26/12/2025
Responsável validação: _________________________________

ARQUITETURA
Stack aprovado: ☐ Sim | ☐ Sob aprovação
Deploy: _________________________________

BLOCKERS ELIMINADOS
1. _________________________________
2. _________________________________
3. _________________________________

PRÓXIMA REUNIÃO
Data: __________ Hora: __________ Duração: __________
```

---

**Gerado em:** 17/12/2025  
**Próxima atualização após call**
