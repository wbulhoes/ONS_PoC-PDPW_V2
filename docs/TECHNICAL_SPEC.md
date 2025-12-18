# Especificação Técnica - PDPW (Programação Diária da Produção Web)

## ?? Informações do Projeto

| Campo | Valor |
|-------|-------|
| **Nome** | PDPW - Programação Diária da Produção Web |
| **Repositório** | https://github.com/wbulhoes/ONS_PoC-PDPW |
| **Branch Principal** | develop |
| **Tecnologia** | .NET 8, ASP.NET Core, Entity Framework Core |
| **Status** | Em Desenvolvimento (PoC) |
| **Autor** | Willian Charantola Bulhoes |

---

## ?? Visão Geral

### Propósito

API REST moderna para gerenciamento de dados energéticos de usinas hidrelétricas, substituindo sistemas legados e fornecendo uma interface web atualizada para a Programação Diária da Produção.

### Objetivos

- ? Modernizar sistema legado
- ? Fornecer API REST documentada
- ? Suportar múltiplas opções de banco de dados
- ? Facilitar integração com frontend moderno
- ? Implementar Clean Architecture

---

## ??? Arquitetura

### Padrão Arquitetural

**Clean Architecture** com separação em 4 camadas:

```
???????????????????????????????????????
?       PDPW.API (Presentation)       ?  ? Controllers, Endpoints
???????????????????????????????????????
?     PDPW.Application (Business)     ?  ? Services, DTOs, Interfaces
???????????????????????????????????????
?      PDPW.Domain (Core Domain)      ?  ? Entities, Domain Logic
???????????????????????????????????????
?  PDPW.Infrastructure (Data Access)  ?  ? Repositories, DbContext
???????????????????????????????????????
```

### Componentes Principais

#### 1. PDPW.API
- **Responsabilidade**: Camada de apresentação
- **Tecnologias**: ASP.NET Core 8, Swagger
- **Componentes**:
  - Controllers REST
  - Configuração de CORS
  - Health Checks
  - Middleware de exceções

#### 2. PDPW.Application
- **Responsabilidade**: Lógica de negócio
- **Padrões**: Service Layer, DTOs
- **Componentes**:
  - Services de domínio
  - Mapeamento de DTOs
  - Validações de negócio

#### 3. PDPW.Domain
- **Responsabilidade**: Core do domínio
- **Componentes**:
  - Entidades de domínio
  - Interfaces de repositórios
  - Regras de negócio

#### 4. PDPW.Infrastructure
- **Responsabilidade**: Persistência de dados
- **Tecnologias**: Entity Framework Core 8
- **Componentes**:
  - DbContext
  - Repositórios concretos
  - Migrations
  - Configurações de entidades

---

## ??? Modelo de Dados

### Entidade Principal: DadoEnergetico

```csharp
public class DadoEnergetico
{
    public int Id { get; set; }
    public DateTime DataReferencia { get; set; }
    public string CodigoUsina { get; set; }
    public decimal ProducaoMWh { get; set; }
    public decimal CapacidadeDisponivel { get; set; }
    public string Status { get; set; }
    public string? Observacoes { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}
```

### Relacionamentos

- Entidade única (por enquanto)
- Indexado por `DataReferencia`
- Soft delete implementado

---

## ?? API Endpoints

### Base URL
```
Development: https://localhost:65417
Production: [A definir]
```

### Endpoints Disponíveis

| Método | Endpoint | Descrição | Autenticação |
|--------|----------|-----------|--------------|
| GET | `/api/DadosEnergeticos` | Listar todos os dados | Não |
| GET | `/api/DadosEnergeticos/{id}` | Buscar por ID | Não |
| GET | `/api/DadosEnergeticos/periodo` | Buscar por período | Não |
| POST | `/api/DadosEnergeticos` | Criar novo dado | Não |
| PUT | `/api/DadosEnergeticos/{id}` | Atualizar dado | Não |
| DELETE | `/api/DadosEnergeticos/{id}` | Remover dado (soft delete) | Não |
| GET | `/health` | Health check | Não |
| GET | `/` | Status da API | Não |

### Exemplos de Request/Response

#### GET /api/DadosEnergeticos
```json
Response 200 OK:
[
  {
    "id": 1,
    "dataReferencia": "2025-01-17T00:00:00",
    "codigoUsina": "UHE-001",
    "producaoMWh": 1500.50,
    "capacidadeDisponivel": 2000.00,
    "status": "Operacional",
    "observacoes": "Produção normal"
  }
]
```

#### POST /api/DadosEnergeticos
```json
Request:
{
  "dataReferencia": "2025-01-17T00:00:00",
  "codigoUsina": "UHE-001",
  "producaoMWh": 1500.50,
  "capacidadeDisponivel": 2000.00,
  "status": "Operacional",
  "observacoes": "Produção normal"
}

Response 201 Created:
{
  "id": 1,
  "dataReferencia": "2025-01-17T00:00:00",
  "codigoUsina": "UHE-001",
  "producaoMWh": 1500.50,
  "capacidadeDisponivel": 2000.00,
  "status": "Operacional",
  "observacoes": "Produção normal"
}
```

---

## ?? Banco de Dados

### Opções Suportadas

| Opção | Configuração | Persistência | Uso Recomendado |
|-------|-------------|--------------|-----------------|
| InMemory | `UseInMemoryDatabase: true` | ? Temporária | Desenvolvimento, Testes |
| LocalDB | Connection string LocalDB | ? Persistente | Desenvolvimento Local |
| SQL Server | Connection string SQL | ? Persistente | Produção, Staging |

### Connection Strings

```json
// LocalDB
"Server=(localdb)\\mssqllocaldb;Database=PDPW_DB_Dev;Trusted_Connection=True;MultipleActiveResultSets=true"

// SQL Server
"Server=localhost;Database=PDPW_DB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"

// SQL Server com autenticação
"Server=localhost;Database=PDPW_DB;User Id=usuario;Password=senha;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

### Migrations

```powershell
# Criar migração
dotnet ef migrations add NomeDaMigracao --project src\PDPW.Infrastructure --startup-project src\PDPW.API

# Aplicar migrações
dotnet ef database update --project src\PDPW.Infrastructure --startup-project src\PDPW.API

# Reverter migração
dotnet ef database update PreviousMigration --project src\PDPW.Infrastructure --startup-project src\PDPW.API
```

---

## ?? Configuração

### appsettings.json

```json
{
  "UseInMemoryDatabase": false,
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PDPW_DB_Dev;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Information",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

### CORS

```csharp
// Frontend permitidos
"http://localhost:5173"  // Vite/React
"http://localhost:3000"  // Create React App
```

---

## ?? Testes

### Estratégia de Testes

- [ ] **Testes Unitários** - Services e Domain
- [ ] **Testes de Integração** - Repositories
- [ ] **Testes de API** - Endpoints
- [ ] **Testes E2E** - Fluxos completos

### Ferramentas

- xUnit
- Moq
- FluentAssertions
- Microsoft.AspNetCore.Mvc.Testing

---

## ?? Dependências

### Pacotes NuGet Principais

| Pacote | Versão | Projeto | Uso |
|--------|--------|---------|-----|
| Microsoft.EntityFrameworkCore | 8.0.0 | Infrastructure | ORM |
| Microsoft.EntityFrameworkCore.SqlServer | 8.0.0 | Infrastructure | Provider SQL Server |
| Microsoft.EntityFrameworkCore.InMemory | 8.0.0 | Infrastructure | Provider InMemory |
| Microsoft.EntityFrameworkCore.Tools | 8.0.0 | Infrastructure | Migrations |
| Microsoft.EntityFrameworkCore.Design | 8.0.0 | API | Design-time |
| Swashbuckle.AspNetCore | 6.5.0 | API | Swagger/OpenAPI |
| Microsoft.Extensions.Diagnostics.HealthChecks | 8.0.0 | API | Health Checks |

---

## ?? Deploy

### Ambientes

| Ambiente | URL | Branch | Banco |
|----------|-----|--------|-------|
| Development | localhost:65417 | develop | InMemory/LocalDB |
| Staging | [A definir] | staging | SQL Server |
| Production | [A definir] | main | SQL Server |

### Processo de Deploy

1. **Build**: `dotnet build --configuration Release`
2. **Publicar**: `dotnet publish --configuration Release`
3. **Migrations**: `dotnet ef database update`
4. **Iniciar**: Configurar IIS/Kestrel

---

## ?? Segurança

### Implementado

- ? CORS configurado
- ? HTTPS redirection
- ? Health checks
- ? Logging de exceções
- ? Connection string em appsettings

### A Implementar

- [ ] Autenticação JWT
- [ ] Autorização por roles
- [ ] Rate limiting
- [ ] API Keys
- [ ] Secrets em Azure Key Vault

---

## ?? Monitoramento

### Logs

- **Provider**: Console, Debug, EventLog
- **Níveis**: Information, Warning, Error, Critical
- **Destino**: Application Insights (futuro)

### Métricas

- Health check endpoint: `/health`
- Status endpoint: `/`
- Response times (a implementar)
- Error rates (a implementar)

---

## ?? Documentação

### Disponível

| Documento | Descrição |
|-----------|-----------|
| `README.md` | Visão geral do projeto |
| `DATABASE_SETUP.md` | Configuração de banco de dados |
| `TROUBLESHOOTING.md` | Solução de problemas |
| `IMPROVEMENTS.md` | Melhorias implementadas |
| `SHARING_GUIDE.md` | Guia de compartilhamento |
| `QUICK_START_INMEMORY.md` | Início rápido |

### Swagger/OpenAPI

- URL: https://localhost:65417/swagger
- Documentação interativa de todos os endpoints
- Try-it-out disponível

---

## ??? Roadmap

### Fase 1 - MVP (Atual)
- [x] Estrutura Clean Architecture
- [x] CRUD de DadosEnergeticos
- [x] Suporte a múltiplos bancos
- [x] Documentação Swagger
- [x] Health checks
- [x] Logging

### Fase 2 - Melhorias
- [ ] Autenticação/Autorização
- [ ] Validações avançadas
- [ ] Cache (Redis)
- [ ] Paginação
- [ ] Filtros avançados

### Fase 3 - Produção
- [ ] Testes automatizados
- [ ] CI/CD pipeline
- [ ] Containerização (Docker)
- [ ] Monitoring (App Insights)
- [ ] Performance optimization

### Fase 4 - Expansão
- [ ] Frontend React/Angular
- [ ] Relatórios
- [ ] Exportação de dados
- [ ] APIs de integração
- [ ] Dashboard analytics

---

## ?? Equipe

| Papel | Nome | Contato |
|-------|------|---------|
| Desenvolvedor | Willian Charantola Bulhoes | [GitHub](https://github.com/wbulhoes) |

---

## ?? Suporte

- **Issues**: https://github.com/wbulhoes/ONS_PoC-PDPW/issues
- **Pull Requests**: https://github.com/wbulhoes/ONS_PoC-PDPW/pulls
- **Documentação**: Ver arquivos `.md` na raiz do projeto

---

## ?? Changelog

### [Unreleased]
- Estrutura inicial do projeto
- CRUD de DadosEnergeticos
- Suporte InMemory/SQL Server
- Documentação completa

---

## ?? Licença

[Definir licença]

---

**Última atualização:** 17/12/2025
**Versão da especificação:** 1.0
