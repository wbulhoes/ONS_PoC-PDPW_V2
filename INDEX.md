# 📚 PDPw v2.0 - Índice de Documentação

## 🎯 Guia Rápido de Navegação

Bem-vindo ao sistema PDPw v2.0! Use este índice para encontrar rapidamente a documentação que você precisa.

---

## 📖 DOCUMENTAÇÃO PRINCIPAL

### 🚀 Para Começar

| Documento | Descrição | Público |
|-----------|-----------|---------|
| **[RESUMO_EXECUTIVO.md](RESUMO_EXECUTIVO.md)** | Visão geral completa do projeto | Todos |
| **[frontend/GUIA_RAPIDO.md](frontend/GUIA_RAPIDO.md)** | Como iniciar em 5 minutos | Desenvolvedores |
| **[COMANDOS_RAPIDOS.md](COMANDOS_RAPIDOS.md)** | Comandos úteis do dia a dia | Desenvolvedores |

### 📋 Documentação Técnica

| Documento | Descrição | Público |
|-----------|-----------|---------|
| **[FRONTEND_COMPLETO_9_ETAPAS.md](FRONTEND_COMPLETO_9_ETAPAS.md)** | Documentação técnica completa | Desenvolvedores |
| **[frontend/README.md](frontend/README.md)** | README do frontend | Desenvolvedores Frontend |
| **[README.md](README.md)** | README principal do projeto | Todos |

### ✅ Validação e Testes

| Documento | Descrição | Público |
|-----------|-----------|---------|
| **[CHECKLIST_VALIDACAO.md](CHECKLIST_VALIDACAO.md)** | Checklist de testes completo | QA / Testadores |
| **[verificar-sistema.sh](verificar-sistema.sh)** | Script de validação automática | DevOps |

---

## 🎯 POR TIPO DE USUÁRIO

### 👨‍💼 Gestores / Product Owners

**Leia primeiro:**
1. [RESUMO_EXECUTIVO.md](RESUMO_EXECUTIVO.md) - Visão geral do que foi entregue
2. [FRONTEND_COMPLETO_9_ETAPAS.md](FRONTEND_COMPLETO_9_ETAPAS.md) - Detalhes técnicos

**Principais informações:**
- ✅ 9 etapas implementadas
- ✅ 90+ endpoints REST
- ✅ Sistema 100% funcional
- ✅ Docker configurado
- ✅ Pronto para produção

---

### 👨‍💻 Desenvolvedores Frontend

**Leia primeiro:**
1. [frontend/README.md](frontend/README.md) - README do frontend
2. [frontend/GUIA_RAPIDO.md](frontend/GUIA_RAPIDO.md) - Como começar
3. [COMANDOS_RAPIDOS.md](COMANDOS_RAPIDOS.md) - Comandos úteis

**Estrutura de código:**
```
frontend/
├── src/
│   ├── pages/          # 9 páginas React
│   ├── services/       # 14 serviços API
│   ├── types/          # TypeScript interfaces
│   └── App.tsx         # Rotas
```

**Tecnologias:**
- React 18.3.1
- TypeScript 5.6.2
- Vite 6.0.11
- React Router 7.1.4

---

### 👨‍💻 Desenvolvedores Backend

**Leia primeiro:**
1. [FRONTEND_COMPLETO_9_ETAPAS.md](FRONTEND_COMPLETO_9_ETAPAS.md) - Visão completa
2. [COMANDOS_RAPIDOS.md](COMANDOS_RAPIDOS.md) - Comandos .NET

**Estrutura de código:**
```
src/
├── PDPW.API/           # Controllers e endpoints
├── PDPW.Application/   # Services e DTOs
├── PDPW.Domain/        # Entidades
└── PDPW.Infrastructure/# Repositórios
```

**Tecnologias:**
- .NET 8
- C# 12
- Entity Framework Core
- SQL Server 2022

---

### 🧪 Testadores / QA

**Leia primeiro:**
1. [CHECKLIST_VALIDACAO.md](CHECKLIST_VALIDACAO.md) - Checklist completo
2. [COMANDOS_RAPIDOS.md](COMANDOS_RAPIDOS.md) - Como executar

**Como testar:**
```bash
# 1. Iniciar backend
cd src/PDPW.API && dotnet run

# 2. Iniciar frontend
cd frontend && npm run dev

# 3. Acessar
Frontend: http://localhost:5173
Swagger: http://localhost:5001/swagger
```

**Etapas a testar:**
1. Dashboard
2. Dados Energéticos
3. Programação Elétrica
4. Previsão Eólica
5. Geração de Arquivos
6. Finalização da Programação
7. Insumos dos Agentes
8. Ofertas de Exportação
9. Ofertas Resposta Voluntária
10. Energia Vertida

---

### 🐳 DevOps / SysAdmins

**Leia primeiro:**
1. [COMANDOS_RAPIDOS.md](COMANDOS_RAPIDOS.md) - Comandos Docker
2. [docker-compose.yml](docker-compose.yml) - Configuração

**Como subir ambiente:**
```bash
# Opção 1: Docker Compose (recomendado)
docker-compose up -d

# Opção 2: Manual
cd src/PDPW.API && dotnet run
cd frontend && npm run dev
```

**Portas:**
- Backend: 5001
- Frontend: 5173
- SQL Server: 1433
- Swagger: 5001/swagger

---

## 📁 ESTRUTURA DE ARQUIVOS

### 📂 Raiz do Projeto

```
C:\temp\_ONS_PoC-PDPW_V2\
├── 📄 RESUMO_EXECUTIVO.md          # ⭐ Visão geral
├── 📄 FRONTEND_COMPLETO_9_ETAPAS.md # ⭐ Documentação técnica
├── 📄 CHECKLIST_VALIDACAO.md       # ✅ Checklist de testes
├── 📄 COMANDOS_RAPIDOS.md          # ⚡ Comandos úteis
├── 📄 INDEX.md                     # 📚 Este arquivo
├── 📄 README.md                    # 📖 README principal
├── 📄 docker-compose.yml           # 🐳 Orquestração
├── 📄 Dockerfile                   # 🐳 Imagem Docker
├── 📄 verificar-sistema.sh         # 🔍 Script de validação
│
├── 📂 frontend/                    # Frontend React
│   ├── 📄 README.md                # README do frontend
│   ├── 📄 GUIA_RAPIDO.md           # Guia rápido
│   ├── 📄 package.json             # Dependências
│   ├── 📄 .env                     # Variáveis de ambiente
│   └── 📂 src/
│       ├── 📂 pages/               # 9 páginas React
│       ├── 📂 services/            # 14 serviços API
│       ├── 📂 types/               # TypeScript types
│       └── 📄 App.tsx              # Rotas
│
├── 📂 src/                         # Backend .NET
│   ├── 📂 PDPW.API/
│   ├── 📂 PDPW.Application/
│   ├── 📂 PDPW.Domain/
│   └── 📂 PDPW.Infrastructure/
│
└── 📂 tests/                       # Testes
    └── 📂 PDPW.IntegrationTests/
```

---

## 🎯 CASOS DE USO COMUNS

### 1️⃣ "Quero entender o projeto rapidamente"
➡️ Leia: [RESUMO_EXECUTIVO.md](RESUMO_EXECUTIVO.md)

### 2️⃣ "Quero rodar o sistema agora"
➡️ Leia: [frontend/GUIA_RAPIDO.md](frontend/GUIA_RAPIDO.md)

### 3️⃣ "Preciso de comandos específicos"
➡️ Leia: [COMANDOS_RAPIDOS.md](COMANDOS_RAPIDOS.md)

### 4️⃣ "Vou testar o sistema"
➡️ Leia: [CHECKLIST_VALIDACAO.md](CHECKLIST_VALIDACAO.md)

### 5️⃣ "Quero detalhes técnicos completos"
➡️ Leia: [FRONTEND_COMPLETO_9_ETAPAS.md](FRONTEND_COMPLETO_9_ETAPAS.md)

### 6️⃣ "Sou novo no projeto"
➡️ Leia na ordem:
1. [RESUMO_EXECUTIVO.md](RESUMO_EXECUTIVO.md)
2. [frontend/GUIA_RAPIDO.md](frontend/GUIA_RAPIDO.md)
3. [COMANDOS_RAPIDOS.md](COMANDOS_RAPIDOS.md)

---

## 🔗 LINKS ÚTEIS

### URLs do Sistema

| Serviço | URL | Descrição |
|---------|-----|-----------|
| **Frontend** | http://localhost:5173 | Aplicação React |
| **API** | http://localhost:5001/api | Backend REST |
| **Swagger** | http://localhost:5001/swagger | Documentação interativa |
| **Health** | http://localhost:5001/health | Status da API |

### Repositórios

| Repositório | URL |
|-------------|-----|
| **Origin** | https://github.com/wbulhoes/ONS_PoC-PDPW_V2 |
| **Meu Fork** | https://github.com/wbulhoes/POCMigracaoPDPw |
| **Squad** | https://github.com/RafaelSuzanoACT/POCMigracaoPDPw |

---

## 📊 MÉTRICAS DO PROJETO

| Métrica | Valor |
|---------|-------|
| **Páginas Frontend** | 9 |
| **Serviços API** | 14 |
| **Endpoints REST** | 90+ |
| **Controllers** | 15 |
| **Testes** | 53 |
| **Registros BD** | 857 |
| **Linhas de Código** | ~8.000 |

---

## ✅ STATUS DAS ETAPAS

| # | Etapa | Status | Doc |
|---|-------|--------|-----|
| 1 | Dados Energéticos | ✅ 100% | [Ver](FRONTEND_COMPLETO_9_ETAPAS.md#etapa-1) |
| 2 | Programação Elétrica | ✅ 100% | [Ver](FRONTEND_COMPLETO_9_ETAPAS.md#etapa-2) |
| 3 | Previsão Eólica | ✅ 100% | [Ver](FRONTEND_COMPLETO_9_ETAPAS.md#etapa-3) |
| 4 | Geração de Arquivos | ✅ 100% | [Ver](FRONTEND_COMPLETO_9_ETAPAS.md#etapa-4) |
| 5 | Finalização ✨ | ✅ 100% | [Ver](FRONTEND_COMPLETO_9_ETAPAS.md#etapa-5) |
| 6 | Insumos Agentes ✨ | ✅ 100% | [Ver](FRONTEND_COMPLETO_9_ETAPAS.md#etapa-6) |
| 7 | Ofertas Exportação ✨ | ✅ 100% | [Ver](FRONTEND_COMPLETO_9_ETAPAS.md#etapa-7) |
| 8 | Ofertas RV ✨ | ✅ 100% | [Ver](FRONTEND_COMPLETO_9_ETAPAS.md#etapa-8) |
| 9 | Energia Vertida ✨ | ✅ 100% | [Ver](FRONTEND_COMPLETO_9_ETAPAS.md#etapa-9) |

**✨ = Implementadas nesta sprint**

---

## 🆘 SUPORTE

### Problemas Comuns

**Erro: "Port already in use"**
```bash
# Frontend
npx kill-port 5173

# Backend
npx kill-port 5001
```

**Erro: "CORS"**
- Verificar se backend está rodando
- Conferir arquivo `.env` do frontend
- Verificar CORS no `Program.cs`

**Erro: "Module not found"**
```bash
cd frontend
rm -rf node_modules package-lock.json
npm install
```

### Mais ajuda?
➡️ Consulte: [COMANDOS_RAPIDOS.md](COMANDOS_RAPIDOS.md#-comandos-de-emergência)

---

## 🎓 GLOSSÁRIO

| Termo | Significado |
|-------|-------------|
| **PDPw** | Programação Diária da Produção |
| **ONS** | Operador Nacional do Sistema Elétrico |
| **PMO** | Programa Mensal de Operação |
| **DADGER** | Dados Gerais (arquivo de entrada do modelo) |
| **DESSEM** | Despacho Eletroenergético Semanal |
| **RV** | Resposta Voluntária da Demanda |
| **UG** | Unidade Geradora |
| **SIN** | Sistema Interligado Nacional |

---

## 📅 HISTÓRICO

| Data | Versão | Descrição |
|------|--------|-----------|
| Dez 2025 | 2.0 | ✅ Implementação completa das 9 etapas |
| Dez 2025 | 1.0 | Início do projeto de migração |

---

## 🏆 CONCLUSÃO

```
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
   📚 DOCUMENTAÇÃO COMPLETA E ORGANIZADA
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

✅ 6 Documentos Principais
✅ Guias para Todos os Perfis
✅ Checklist de Validação
✅ Comandos Rápidos
✅ Scripts de Automação

NAVEGUE COM FACILIDADE! 🚀
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```

---

**PDPw v2.0 - Sistema de Programação Diária**  
**Operador Nacional do Sistema Elétrico - ONS**  
© 2025 - Todos os direitos reservados

**Use este índice como ponto de partida!** 📌
