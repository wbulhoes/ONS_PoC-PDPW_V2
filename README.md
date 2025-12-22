# PDPw - Programação Diária de Produção (POC - Migração .NET 8 + React)

**Versão**: 2.0 - POC  
**Status**: 🟢 85% Concluído  
**Cliente**: ONS (Operador Nacional do Sistema Elétrico)  
**Prazo**: 29/12/2024

---

## 📋 Sobre o Projeto

**Prova de Conceito (POC)** para migração do sistema PDPw de um legado .NET Framework 4.8/VB.NET com WebForms para uma arquitetura moderna usando:

- **Back-end**: .NET 8 com C# e ASP.NET Core Web API ✅ **85% CONCLUÍDO**
- **Front-end**: React 18 com TypeScript 🚧 **0% - Início 24/12**
- **Banco de Dados**: SQL Server 2019 Express ✅ **100% CONFIGURADO**
- **Testes**: xUnit + Moq 🟡 **10% - Meta: 60%**
- **CI/CD**: GitHub Actions 🔴 **0% - Meta 27/12**

---

## 🚀 Início Rápido

### Pré-requisitos
```yaml
- .NET 8 SDK
- SQL Server 2019 Express ou superior
- Visual Studio 2022 / VS Code / Rider
- Git
```

### Setup em 5 minutos
```powershell
# 1. Clonar repositório
git clone https://github.com/wbulhoes/ONS_PoC-PDPW_V2.git
cd ONS_PoC-PDPW_V2

# 2. Restaurar pacotes
dotnet restore

# 3. Aplicar migrations
dotnet ef database update --project src/PDPW.Infrastructure --startup-project src/PDPW.API

# 4. Executar aplicação
dotnet run --project src/PDPW.API/PDPW.API.csproj

# 5. Acessar Swagger
# https://localhost:5001/swagger
```

### Credenciais
```yaml
SQL Server:
  Servidor: .\SQLEXPRESS
  Banco: PDPW_DB
  Usuário: sa
  Senha: Pdpw@2024!Strong

Swagger:
  URL: https://localhost:5001/swagger
```

---

## 📊 Progresso da POC

### ✅ APIs Backend (15 APIs - 107 Endpoints)

#### **Grupo 1: Cadastros Base (10 APIs) - 100%**
| # | API | Endpoints | Status |
|---|-----|-----------|--------|
| 1 | Usinas | 8 | ✅ |
| 2 | Empresas | 6 | ✅ |
| 3 | TiposUsina | 6 | ✅ |
| 4 | SemanasPMO | 7 | ✅ |
| 5 | EquipesPDP | 6 | ✅ |
| 6 | Cargas | 7 | ✅ |
| 7 | ArquivosDadger | 8 | ✅ |
| 8 | RestricoesUG | 7 | ✅ |
| 9 | DadosEnergeticos | 6 | ✅ |
| 10 | Usuarios | 6 | ✅ |

**Subtotal: 67 endpoints**

#### **Grupo 2: Operação Energética (5 APIs) - 100%** ⭐ **NOVO**
| # | API | Endpoints | Status |
|---|-----|-----------|--------|
| 11 | UnidadesGeradoras | 8 | ✅ |
| 12 | ParadasUG | 9 | ✅ |
| 13 | MotivosRestricao | 6 | ✅ |
| 14 | Balancos | 8 | ✅ |
| 15 | Intercambios | 9 | ✅ |

**Subtotal: 40 novos endpoints** ⭐

### **TOTAL GERAL: 107 ENDPOINTS REST** 🎉

---

## 🗄️ Banco de Dados

### Configuração
- **Servidor**: `.\SQLEXPRESS`
- **Banco**: `PDPW_DB`
- **Autenticação**: SQL Server (sa)
- **Tabelas**: 31 tabelas
- **Dados**: ~550 registros realistas

### Dados Populados
- ✅ 30 Empresas (CEMIG, COPEL, Itaipu, FURNAS, etc.)
- ✅ 50 Usinas (Itaipu, Belo Monte, Tucuruí, etc.)
- ✅ 100 Unidades Geradoras
- ✅ 10 Motivos de Restrição
- ✅ 50 Paradas UG
- ✅ 120 Balanços Energéticos
- ✅ 240 Intercâmbios
- ✅ 25 Semanas PMO
- ✅ 11 Equipes PDP

**Dados baseados no setor elétrico brasileiro real!**

---

## 📅 Roadmap até 29/12/2024

```
┌─────────────────────────────────────────────────────┐
│ DIA   │ ATIVIDADE                    │ STATUS       │
├─────────────────────────────────────────────────────┤
│ 23/12 │ Testes Backend               │ 🟡 Pendente  │
│ 24/12 │ Setup React + 3 telas        │ 🔴 Pendente  │
│ 25/12 │ CRUD + Dashboard             │ 🔴 Pendente  │
│ 26/12 │ Integração + Testes E2E      │ 🔴 Pendente  │
│ 27/12 │ CI/CD + Deploy               │ 🔴 Pendente  │
│ 28/12 │ Documentação Final           │ 🔴 Pendente  │
│ 29/12 │ Entrega POC                  │ 🔴 Pendente  │
└─────────────────────────────────────────────────────┘
```

---

## 📚 Documentação Completa

| Documento | Localização | Status |
|-----------|-------------|--------|
| **Status e Roadmap** | [docs/POC_STATUS_E_ROADMAP.md](docs/POC_STATUS_E_ROADMAP.md) | ✅ ⭐ |
| **Apresentação Squad** | [docs/APRESENTACAO_SQUAD.md](docs/APRESENTACAO_SQUAD.md) | ✅ ⭐ |
| **Setup Banco de Dados** | [docs/SQL_SERVER_SETUP_SUMMARY.md](docs/SQL_SERVER_SETUP_SUMMARY.md) | ✅ |
| **Configuração Final** | [docs/SQL_SERVER_FINAL_SETUP.md](docs/SQL_SERVER_FINAL_SETUP.md) | ✅ |
| **Guia de Configuração** | [docs/DATABASE_CONFIG.md](docs/DATABASE_CONFIG.md) | ✅ |
| **Schema do Banco** | [docs/database_schema.sql](docs/database_schema.sql) | ✅ |
| **Quadro Resumo** | [docs/QUADRO_RESUMO_POC.md](docs/QUADRO_RESUMO_POC.md) | ✅ |
| **Guia Setup QA** | [docs/SETUP_GUIDE_QA.md](docs/SETUP_GUIDE_QA.md) | ✅ |

---

## 🏗️ Arquitetura

### Clean Architecture

```
┌───────────────────────────────────┐
│         FRONTEND (React)          │ ← Em desenvolvimento
└────────────┬──────────────────────┘
             │ REST API
┌────────────▼──────────────────────┐
│     PDPW.API (Controllers)        │ ← 15 Controllers ✅
└────────────┬──────────────────────┘
             │
┌────────────▼──────────────────────┐
│  PDPW.Application (Services)      │ ← 15 Services ✅
└────────────┬──────────────────────┘
             │
┌────────────▼──────────────────────┐
│    PDPW.Domain (Entities)         │ ← 31 Entities ✅
└────────────┬──────────────────────┘
             │
┌────────────▼──────────────────────┐
│ PDPW.Infrastructure (EF Core)     │ ← 15 Repositories ✅
└────────────┬──────────────────────┘
             │
┌────────────▼──────────────────────┐
│   SQL Server 2019 (PDPW_DB)       │ ← 31 Tabelas ✅
└───────────────────────────────────┘
```

---

## 🎯 APIs Projetadas

### 📌 **1. Empresas (Agentes do Setor Elétrico)**
Gerenciamento de empresas/agentes do setor elétrico brasileiro.

```http
GET    /api/empresas              # Lista todas as empresas
GET    /api/empresas/{id}         # Busca por ID
GET    /api/empresas/sigla/{sigla} # Busca por sigla
POST   /api/empresas              # Cria nova empresa
PUT    /api/empresas/{id}         # Atualiza empresa
DELETE /api/empresas/{id}         # Remove empresa (soft delete)
```

**Exemplo de Request:**
```json
POST /api/empresas
{
  "sigla": "CEMIG",
  "nomeCompleto": "Companhia Energética de Minas Gerais",
  "cnpj": "17155730000164",
  "ativo": true
}
```

---

### 📌 **2. Tipos de Usina**
Gerenciamento de tipos/categorias de usinas geradoras.

```http
GET    /api/tiposusina           # Lista todos os tipos
GET    /api/tiposusina/{id}      # Busca por ID
GET    /api/tiposusina/codigo/{codigo} # Busca por código
POST   /api/tiposusina           # Cria novo tipo
PUT    /api/tiposusina/{id}      # Atualiza tipo
DELETE /api/tiposusina/{id}      # Remove tipo
```

**Exemplo de Response:**
```json
{
  "id": 1,
  "codigo": "UHE",
  "nome": "Usina Hidrelétrica",
  "descricao": "Geração hidráulica de energia",
  "ativo": true
}
```

---

### 📌 **3. Usinas Geradoras**
Gerenciamento de usinas geradoras de energia.

```http
GET    /api/usinas                # Lista todas as usinas
GET    /api/usinas/{id}           # Busca por ID
GET    /api/usinas/codigo/{codigo} # Busca por código ONS
GET    /api/usinas/tipo/{tipoId}  # Filtra por tipo
GET    /api/usinas/empresa/{empresaId} # Filtra por empresa
POST   /api/usinas                # Cria nova usina
PUT    /api/usinas/{id}           # Atualiza usina
DELETE /api/usinas/{id}           # Remove usina
```

**Exemplo de Request:**
```json
POST /api/usinas
{
  "codigo": "ITAIPU",
  "nome": "Usina Hidrelétrica de Itaipu",
  "tipoUsinaId": 1,
  "empresaId": 5,
  "potenciaInstalada": 14000.00,
  "latitude": -25.4078,
  "longitude": -54.5889,
  "municipio": "Foz do Iguaçu",
  "uf": "PR"
}
```

---

### 📌 **4. Semanas PMO**
Gerenciamento de semanas operativas do PMO (Programa Mensal de Operação).

```http
GET    /api/semanaspmo            # Lista todas as semanas
GET    /api/semanaspmo/{id}       # Busca por ID
GET    /api/semanaspmo/ano/{ano}  # Filtra por ano
GET    /api/semanaspmo/atual      # Semana atual
GET    /api/semanaspmo/proximas?quantidade=4 # Próximas N semanas
GET    /api/semanaspmo/numero/{numero}/ano/{ano} # Busca específica
POST   /api/semanaspmo            # Cria nova semana
PUT    /api/semanaspmo/{id}       # Atualiza semana
DELETE /api/semanaspmo/{id}       # Remove semana
```

**Exemplo de Response:**
```json
{
  "id": 1,
  "numero": 3,
  "ano": 2025,
  "dataInicio": "2025-01-18",
  "dataFim": "2025-01-24",
  "observacoes": "Semana operativa 3/2025",
  "ativo": true
}
```

---

### 📌 **5. Equipes PDP**
Gerenciamento de equipes responsáveis pela programação diária.

```http
GET    /api/equipespdp            # Lista todas as equipes
GET    /api/equipespdp/{id}       # Busca por ID
GET    /api/equipespdp/ativas     # Lista apenas ativas
POST   /api/equipespdp            # Cria nova equipe
PUT    /api/equipespdp/{id}       # Atualiza equipe
DELETE /api/equipespdp/{id}       # Remove equipe
```

---

### 📌 **6. Cargas Elétricas** ⭐ **NOVO**
Gerenciamento de dados de carga elétrica do sistema.

```http
GET    /api/cargas                # Lista todas as cargas
GET    /api/cargas/{id}           # Busca por ID
GET    /api/cargas/subsistema/{subsistemaId} # Filtra por subsistema
GET    /api/cargas/periodo?dataInicio=&dataFim= # Filtra por período
GET    /api/cargas/data/{data}    # Busca por data específica
POST   /api/cargas                # Cria nova carga
PUT    /api/cargas/{id}           # Atualiza carga
DELETE /api/cargas/{id}           # Remove carga
```

**Exemplo de Request:**
```json
POST /api/cargas
{
  "dataReferencia": "2025-01-20",
  "subsistemaId": "SE",
  "cargaMWmed": 45678.50,
  "cargaVerificada": 45234.20,
  "previsaoCarga": 46000.00,
  "observacoes": "Carga elevada devido a temperatura"
}
```

**Exemplo de Response:**
```json
{
  "id": 1,
  "dataReferencia": "2025-01-20",
  "subsistemaId": "SE",
  "subsistemaNome": "Sudeste",
  "cargaMWmed": 45678.50,
  "cargaVerificada": 45234.20,
  "previsaoCarga": 46000.00,
  "observacoes": "Carga elevada devido a temperatura",
  "ativo": true,
  "dataCriacao": "2025-01-20T10:30:00Z"
}
```

---

### 📌 **7. Arquivos DADGER** ⭐ **NOVO**
Gerenciamento de arquivos DADGER (Dados de Geração).

```http
GET    /api/arquivosdadger        # Lista todos os arquivos
GET    /api/arquivosdadger/{id}   # Busca por ID
GET    /api/arquivosdadger/semana/{semanaPMOId} # Filtra por semana PMO
GET    /api/arquivosdadger/processados?processado=true # Por status
GET    /api/arquivosdadger/periodo?dataInicio=&dataFim= # Por período
GET    /api/arquivosdadger/nome/{nomeArquivo} # Busca por nome
POST   /api/arquivosdadger        # Cria novo arquivo
PUT    /api/arquivosdadger/{id}   # Atualiza arquivo
PATCH  /api/arquivosdadger/{id}/processar # Marca como processado ⚡
DELETE /api/arquivosdadger/{id}   # Remove arquivo
```

**Exemplo de Request:**
```json
POST /api/arquivosdadger
{
  "nomeArquivo": "dadger_202501_semana03.dat",
  "caminhoArquivo": "/uploads/2025/01/dadger_202501_semana03.dat",
  "dataImportacao": "2025-01-20T08:00:00Z",
  "semanaPMOId": 3,
  "observacoes": "Arquivo importado automaticamente"
}
```

**Funcionalidade Especial:**
```http
PATCH /api/arquivosdadger/5/processar
```
Marca o arquivo como processado e registra a data de processamento.

---

### 📌 **8. Restrições de Unidades Geradoras** ⭐ **NOVO**
Gerenciamento de restrições operacionais de unidades geradoras.

```http
GET    /api/restricoesug          # Lista todas as restrições
GET    /api/restricoesug/{id}     # Busca por ID
GET    /api/restricoesug/unidade/{unidadeGeradoraId} # Por unidade
GET    /api/restricoesug/ativas?dataReferencia=2025-01-20 # Ativas em uma data
GET    /api/restricoesug/periodo?dataInicio=&dataFim= # Por período
GET    /api/restricoesug/motivo/{motivoRestricaoId} # Por motivo
POST   /api/restricoesug          # Cria nova restrição
PUT    /api/restricoesug/{id}     # Atualiza restrição
DELETE /api/restricoesug/{id}     # Remove restrição
```

**Exemplo de Request:**
```json
POST /api/restricoesug
{
  "unidadeGeradoraId": 15,
  "dataInicio": "2025-01-20",
  "dataFim": "2025-01-27",
  "motivoRestricaoId": 3,
  "potenciaRestrita": 150.00,
  "observacoes": "Manutenção preventiva programada"
}
```

**Exemplo de Response:**
```json
{
  "id": 1,
  "unidadeGeradoraId": 15,
  "unidadeGeradora": "UG-ITAIPU-01",
  "codigoUnidade": "ITU01",
  "dataInicio": "2025-01-20",
  "dataFim": "2025-01-27",
  "motivoRestricaoId": 3,
  "motivoRestricao": "Manutenção Preventiva",
  "categoriaMotivoRestricao": "PROGRAMADA",
  "potenciaRestrita": 150.00,
  "observacoes": "Manutenção preventiva programada",
  "ativo": true,
  "dataCriacao": "2025-01-19T14:20:00Z"
}
```

**Query Especial - Restrições Ativas:**
```http
GET /api/restricoesug/ativas?dataReferencia=2025-01-20
```
Retorna todas as restrições que estão ativas na data especificada (DataInicio <= data <= DataFim).

---

### 📌 **9. Dados Energéticos**
Gerenciamento de dados energéticos do sistema (em desenvolvimento).

```http
GET    /api/dadosenergeticos      # Lista todos os dados
GET    /api/dadosenergeticos/{id} # Busca por ID
POST   /api/dadosenergeticos      # Cria novo registro
PUT    /api/dadosenergeticos/{id} # Atualiza registro
DELETE /api/dadosenergeticos/{id} # Remove registro
```

---

## 🔧 Funcionalidades Comuns

Todas as APIs implementam:

- ✅ **Validação de entrada** (Data Annotations + FluentValidation)
- ✅ **Soft Delete** (flag `Ativo` em vez de exclusão física)
- ✅ **Auditoria** (DataCriacao, DataAtualizacao)
- ✅ **Documentação Swagger** (XML Comments)
- ✅ **Logging estruturado** (ILogger)
- ✅ **Tratamento de erros** (try-catch com mensagens amigáveis)
- ✅ **DTOs separados** (Create, Update, Response)
- ✅ **Repository Pattern** (abstração de dados)
- ✅ **Clean Architecture** (Domain, Application, Infrastructure, API)

---

## 📦 Recursos Avançados

### Paginação (Preparado)
```csharp
// Estrutura pronta para uso
public class PaginationParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10; // Max: 100
    public string? OrderBy { get; set; }
    public string OrderDirection { get; set; } = "asc";
}

public class PagedResult<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
    public IEnumerable<T> Data { get; set; }
}
```

### Cache (Redis - Preparado)
```bash
# Instalação
dotnet add src/PDPW.API package Microsoft.Extensions.Caching.StackExchangeRedis

# Configuração em appsettings.json
"ConnectionStrings": {
  "Redis": "localhost:6379"
}
```

### Logging Estruturado (Serilog - Preparado)
```bash
# Instalação
dotnet add src/PDPW.API package Serilog.AspNetCore
dotnet add src/PDPW.API package Serilog.Sinks.Console
dotnet add src/PDPW.API package Serilog.Sinks.File
```

---

## 🧪 Testes

### Testes Unitários
```bash
# Rodar todos os testes
dotnet test

# Rodar com cobertura
dotnet test /p:CollectCoverage=true
```

**Cobertura Atual:**
- ✅ CargaService: 10 testes (100% cobertura)
- 🔄 Outros services: em desenvolvimento

---

## 🏗️ Arquitetura

Consulte [STRUCTURE.md](STRUCTURE.md) para detalhes da arquitetura.

```
src/
├── PDPW.API/              # Controllers, Middleware, Swagger
├── PDPW.Application/      # Services, DTOs, Interfaces
├── PDPW.Domain/           # Entities, Interfaces de Repositórios
└── PDPW.Infrastructure/   # Repositories, DbContext, Migrations

tests/
├── PDPW.UnitTests/        # Testes unitários (xUnit + Moq)
└── PDPW.IntegrationTests/ # Testes de integração
```

---

## 📚 Documentação

- [AGENTS.md](AGENTS.md) - Documentação para IA
- [STRUCTURE.md](STRUCTURE.md) - Estrutura do projeto
- [CONTRIBUTING.md](CONTRIBUTING.md) - Guia de contribuição
- [QUICKSTART.md](QUICKSTART.md) - Início rápido
- [docs/](docs/) - Documentação adicional
- [Swagger UI](http://localhost:5001/swagger) - Documentação interativa das APIs

---

## 🎯 Roadmap

### Fase Atual (Janeiro 2025)
- ✅ APIs de Cadastro (Empresas, Usinas, Tipos)
- ✅ APIs de Operação (Semanas PMO, Equipes)
- ✅ APIs de Dados (Cargas, DADGER, Restrições)
- 🚧 APIs de Processamento
- ⏳ Frontend React

### Próximas Fases
- ⏳ Autenticação e Autorização (JWT)
- ⏳ APIs de Relatórios
- ⏳ Migração de dados legados
- ⏳ Testes E2E
- ⏳ Deploy em produção

---

## 🤝 Contribuindo

Consulte [CONTRIBUTING.md](CONTRIBUTING.md)

---

## 📄 Licença

Propriedade intelectual do ONS (Operador Nacional do Sistema Elétrico Brasileiro).

---

## 🎓 Tecnologias Utilizadas

**Backend:**
- .NET 8.0
- ASP.NET Core Web API
- Entity Framework Core 8
- SQL Server
- Swagger/OpenAPI
- xUnit + Moq

**Infraestrutura:**
- Docker
- Docker Compose
- Git + GitHub

**Ferramentas:**
- Visual Studio 2022
- VS Code
- SQL Server Management Studio
- Postman

---

**Desenvolvido com ❤️ por Willian + GitHub Copilot**
