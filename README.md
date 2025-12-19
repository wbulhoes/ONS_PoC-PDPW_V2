# PDPw - Programação Diária de Produção (Migração .NET 8 + React)

**Versão**: 2.0  
**Status**: 🚧 Em Desenvolvimento  
**Cliente**: ONS (Operador Nacional do Sistema Elétrico)

---

## 📋 Sobre o Projeto

Migração incremental do sistema PDPw de um legado .NET Framework 4.8/VB.NET com WebForms para uma arquitetura moderna usando:

- **Back-end**: .NET 8 com C# e ASP.NET Core Web API
- **Front-end**: React com TypeScript
- **Banco de Dados**: SQL Server (Entity Framework Core)
- **Infraestrutura**: Docker e Docker Compose
- **Testes**: xUnit (backend) + Jest (frontend)

---

## 🚀 Início Rápido

### Via Docker (Recomendado)
```bash
docker-compose up -d
# Backend: http://localhost:5000/swagger
# Frontend: http://localhost:3000
```

### Via Local
Consulte [QUICKSTART.md](QUICKSTART.md)

---

## 📊 Progresso

### Backend APIs
- ✅ Usinas (8 endpoints)
- ✅ TiposUsina (6 endpoints)
- ✅ Empresas (8 endpoints)
- ✅ SemanasPMO (9 endpoints)
- ✅ EquipesPDP (8 endpoints)
- ✅ Cargas (8 endpoints)
- ✅ ArquivosDadger (9 endpoints)
- ✅ RestricoesUG (9 endpoints)
- 🔄 DadosEnergeticos (parcial)
- ⏳ 20 APIs restantes

**Total**: 9/29 APIs (31%) | 65/154 endpoints (42%)

### Frontend
- 🚧 Em desenvolvimento

### Testes
- ✅ 15 testes unitários implementados
- ✅ 100% cobertura CargaService

---

## 🎯 APIs Implementadas

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
- [Swagger UI](http://localhost:5000/swagger) - Documentação interativa das APIs

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
