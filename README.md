# PDPW - Programação Diária da Produção

## 🎯 Sobre o Projeto

PoC de modernização do sistema legado PDPW para o ONS (Operador Nacional do Sistema Elétrico), migrando de .NET Framework/WebForms/VB.NET para uma arquitetura moderna com .NET 8, React e containerização.

---

## ⚡ ATUALIZAÇÃO IMPORTANTE (19/12/2024)

### 🚀 KICK-OFF DO SQUAD - DOCUMENTAÇÃO COMPLETA DISPONÍVEL!

**Status:** ✅ Ambiente de desenvolvimento preparado  
**Equipe:** 3 Devs + 1 QA  
**Início:** 19/12/2024 - 15:00h

#### 📚 Documentação para Reunião de Kick-off

**Para o Tech Lead:**
- 📋 [`docs/CHECKLIST_REUNIAO_EXECUTIVO.md`](docs/CHECKLIST_REUNIAO_EXECUTIVO.md) - Checklist executivo para conduzir a reunião
- 📊 [`docs/APRESENTACAO_REUNIAO_SQUAD.md`](docs/APRESENTACAO_REUNIAO_SQUAD.md) - Material completo de apresentação
- 📱 [`docs/RESUMO_VISUAL_APRESENTACAO.md`](docs/RESUMO_VISUAL_APRESENTACAO.md) - Slides visuais para projeção

**Para o Squad (Devs + QA):**
- 📄 [`docs/SQUAD_BRIEFING_19DEC.md`](docs/SQUAD_BRIEFING_19DEC.md) - Briefing completo com divisão de tarefas
- 🔍 [`docs/ANALISE_TECNICA_CODIGO_LEGADO.md`](docs/ANALISE_TECNICA_CODIGO_LEGADO.md) - Análise detalhada do código VB.NET
- 🛠️ [`docs/SETUP_AMBIENTE_GUIA.md`](docs/SETUP_AMBIENTE_GUIA.md) - Guia passo a passo de instalação

**Documentos Anteriores:**
- 📄 [`database/SCHEMA_ANALYSIS_FROM_CODE.md`](database/SCHEMA_ANALYSIS_FROM_CODE.md) - Análise do schema do banco
- 📄 [`VERTICAL_SLICES_DECISION.md`](VERTICAL_SLICES_DECISION.md) - Decisões técnicas dos slices
- 📄 [`RESUMO_EXECUTIVO.md`](RESUMO_EXECUTIVO.md) - Resumo executivo do projeto
- 📖 [`GLOSSARIO.md`](GLOSSARIO.md) - Glossário de termos técnicos

---

## 👥 DIVISÃO DO SQUAD

### 🟦 DEV 1 - Backend Lead
**Responsabilidade:** SLICE 1 - Cadastro de Usinas  
**Prazo:** 20/12/2024 (2 dias)  
**Entregáveis:**
- Entidade `Usina` no Domain
- Repository + Service + Controller
- 6 endpoints REST (GET/POST/PUT/DELETE)
- Testes unitários (> 70% cobertura)

### 🟩 DEV 2 - Backend
**Responsabilidade:** SLICE 2 - Consulta Arquivos DADGER  
**Prazo:** 22/12/2024 (4 dias)  
**Entregáveis:**
- 3 entidades relacionadas (ArquivoDadger, ArquivoDadgerValor, SemanaPMO)
- Repositórios com JOINs complexos
- Services com filtros (período, usina, semana)
- 5 endpoints REST
- Testes de integração

### 🟨 DEV 3 - Frontend Lead
**Responsabilidade:** Interfaces React para ambos slices  
**Prazo:** 21/12/2024 (3 dias)  
**Entregáveis:**
- Tela de listagem de Usinas + Formulário
- Tela de consulta DADGER + Filtros dinâmicos
- Integração completa com API
- UI responsiva e moderna

### 🟪 QA - Quality Assurance
**Responsabilidade:** Testes e documentação  
**Prazo:** Diário (19-24/12/2024)  
**Entregáveis:**
- Plano de testes documentado
- Casos de teste executados (API + UI)
- Relatório de bugs (se houver)
- Checklist de validação final

---

## 📅 CRONOGRAMA

```
19/12 (Qui) ━━━ Setup + Kick-off + Início desenvolvimento
20/12 (Sex) ━━━ SLICE 1 (Usinas) completo
21/12 (Sáb) ━━━ Integração SLICE 1 + Início SLICE 2
22/12 (Dom) ━━━ SLICE 2 (DADGER) completo
23/12 (Seg) ━━━ Integração SLICE 2 + Ajustes
24/12 (Ter) ━━━ Docker + Testes + Documentação
25/12 (Qua) ━━━ FERIADO 🎄
26/12 (Qui) ━━━ Apresentação + Entrega ✅
```

**📅 Entrega:** 26/12/2024  
**📅 Apresentação:** 05/01/2025  
**📅 Estimativa completa:** 12/01/2025

---

## 🎯 Vertical Slices Definidos

### **SLICE 1: Cadastro de Usinas** ⭐⭐⭐
- Entidade central do sistema (CRUD completo)
- Backend: API REST com 6 endpoints
- Frontend: Listagem + formulário + filtros
- **Código legado:** `pdpw_act/pdpw/Dao/UsinaDAO.vb`
- **Complexidade:** Média
- **Tempo:** 2 dias

### **SLICE 2: Consulta Arquivos DADGER** ⭐⭐⭐
- Funcionalidade core do PDPW
- 3 entidades relacionadas (ArquivoDadger, ArquivoDadgerValor, SemanaPMO)
- Backend: API REST com relacionamentos complexos
- Frontend: Consulta + filtros + grid de valores
- **Código legado:** `pdpw_act/pdpw/Dao/ArquivoDadgerValorDAO.vb`
- **Complexidade:** Alta
- **Tempo:** 3 dias

---

## 🛠️ Setup Rápido do Ambiente

### Backend Devs
```powershell
# Instalar .NET 8 SDK
winget install Microsoft.DotNet.SDK.8

# Instalar Visual Studio 2022
winget install Microsoft.VisualStudio.2022.Community

# Instalar Docker
winget install Docker.DockerDesktop

# Testar
cd src\PDPW.API
dotnet restore
dotnet run
# Abrir: http://localhost:5000/swagger
```

### Frontend Dev
```powershell
# Instalar Node.js 20
winget install OpenJS.NodeJS.LTS

# Instalar VS Code
winget install Microsoft.VisualStudioCode

# Testar
cd frontend
npm install
npm run dev
# Abrir: http://localhost:3000
```

### QA
```powershell
# Instalar Postman
winget install Postman.Postman

# Instalar Git
winget install Git.Git
```

**📄 Guia completo:** [`docs/SETUP_AMBIENTE_GUIA.md`](docs/SETUP_AMBIENTE_GUIA.md)

---

## 📊 Código Legado Analisado

### Estatísticas
- **473** arquivos VB.NET
- **168** páginas ASPX (WebForms)
- **.NET Framework 4.8** + SQL Server
- **Arquitetura:** 3 camadas (DAO/Business/DTO)

### Pontos Positivos
✅ Código bem estruturado com separação de responsabilidades  
✅ Padrão Repository implementado  
✅ Sistema de cache implementado  
✅ Testes unitários existentes

### Desafios
⚠️ WebForms legado (dificulta migração de UI)  
⚠️ VB.NET (requer conversão para C#)  
⚠️ SQL inline (sem ORM moderno)  
⚠️ Banco de 350GB (impossível restaurar - usaremos InMemory)

**📄 Análise completa:** [`docs/ANALISE_TECNICA_CODIGO_LEGADO.md`](docs/ANALISE_TECNICA_CODIGO_LEGADO.md)

---

## 📝 Observações Importantes

- Este é um projeto de **Proof of Concept (PoC)**
- Foco em **vertical slice**: um fluxo completo e funcional
- Prazo de entrega: **26/12/2025**
- Apresentação: **05/01/2026**

## 🤝 Contribuindo

1. Analise o código legado em VB.NET
2. Identifique funcionalidades críticas
3. Implemente usando Clean Architecture
4. Documente decisões técnicas
5. Teste extensivamente

## 📞 Suporte

Para dúvidas sobre o projeto, consulte:
- Documentação do código (comentários inline)
- Swagger da API: http://localhost:5000/swagger
- Issues do repositório

---

**Desenvolvido para ONS - Operador Nacional do Sistema Elétrico**  
**PoC de Modernização PDPW - Dezembro/2025**
