# Especifica��o T�cnica - PDPW (Programa��o Di�ria da Produ��o Web)

## ?? Informa��es do Projeto

| Campo | Valor |
|-------|-------|
| **Nome** | PDPW - Programa��o Di�ria da Produ��o Web |
| **Reposit�rio** | https://github.com/wbulhoes/ONS_PoC-PDPW |
| **Branch Principal** | develop |
| **Tecnologia** | .NET 8, ASP.NET Core, Entity Framework Core |
| **Status** | Em Desenvolvimento (PoC) |
| **Autor** | Willian Charantola Bulhoes |

---

## ?? Vis�o Geral

### Prop�sito

API REST moderna para gerenciamento de dados energ�ticos de usinas hidrel�tricas, substituindo sistemas legados e fornecendo uma interface web atualizada para a Programa��o Di�ria da Produ��o.

### Objetivos

- ? Modernizar sistema legado
- ? Fornecer API REST documentada
- ? Suportar m�ltiplas op��es de banco de dados
- ? Facilitar integra��o com frontend moderno
- ? Implementar Clean Architecture

---

## ??? Arquitetura

### Padr�o Arquitetural

**Clean Architecture** com separa��o em 4 camadas:

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
- **Responsabilidade**: Camada de apresenta��o
- **Tecnologias**: ASP.NET Core 8, Swagger
- **Componentes**:
  - Controllers REST
  - Configura��o de CORS
  - Health Checks
  - Middleware de exce��es

#### 2. PDPW.Application
- **Responsabilidade**: L�gica de neg�cio
- **Padr�es**: Service Layer, DTOs
- **Componentes**:
  - Services de dom�nio
  - Mapeamento de DTOs
  - Valida��es de neg�cio

#### 3. PDPW.Domain
- **Responsabilidade**: Core do dom�nio
- **Componentes**:
  - Entidades de dom�nio
  - Interfaces de reposit�rios
  - Regras de neg�cio

#### 4. PDPW.Infrastructure
- **Responsabilidade**: Persist�ncia de dados
- **Tecnologias**: Entity Framework Core 8
- **Componentes**:
  - DbContext
  - Reposit�rios concretos
  - Migrations
  - Configura��es de entidades

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

- Entidade �nica (por enquanto)
- Indexado por `DataReferencia`
- Soft delete implementado

---

## ?? API Endpoints

### Base URL
```
Development: https://localhost:65417
Production: [A definir]
```

### Endpoints Dispon�veis

| M�todo | Endpoint | Descri��o | Autentica��o |
|--------|----------|-----------|--------------|
| GET | `/api/DadosEnergeticos` | Listar todos os dados | N�o |
| GET | `/api/DadosEnergeticos/{id}` | Buscar por ID | N�o |
| GET | `/api/DadosEnergeticos/periodo` | Buscar por per�odo | N�o |
| POST | `/api/DadosEnergeticos` | Criar novo dado | N�o |
| PUT | `/api/DadosEnergeticos/{id}` | Atualizar dado | N�o |
| DELETE | `/api/DadosEnergeticos/{id}` | Remover dado (soft delete) | N�o |
| GET | `/health` | Health check | N�o |
| GET | `/` | Status da API | N�o |

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
    "observacoes": "Produ��o normal"
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
  "observacoes": "Produ��o normal"
}

Response 201 Created:
{
  "id": 1,
  "dataReferencia": "2025-01-17T00:00:00",
  "codigoUsina": "UHE-001",
  "producaoMWh": 1500.50,
  "capacidadeDisponivel": 2000.00,
  "status": "Operacional",
  "observacoes": "Produ��o normal"
}
```

---

## ?? Banco de Dados

### Op��es Suportadas

| Op��o | Configura��o | Persist�ncia | Uso Recomendado |
|-------|-------------|--------------|-----------------|
| InMemory | `UseInMemoryDatabase: true` | ? Tempor�ria | Desenvolvimento, Testes |
| LocalDB | Connection string LocalDB | ? Persistente | Desenvolvimento Local |
| SQL Server | Connection string SQL | ? Persistente | Produ��o, Staging |

### Connection Strings

```json
// LocalDB
"Server=(localdb)\\mssqllocaldb;Database=PDPW_DB_Dev;Trusted_Connection=True;MultipleActiveResultSets=true"

// SQL Server
"Server=localhost;Database=PDPW_DB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"

// SQL Server com autentica��o
"Server=localhost;Database=PDPW_DB;User Id=usuario;Password=senha;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

### Migrations

```powershell
# Criar migra��o
dotnet ef migrations add NomeDaMigracao --project src\PDPW.Infrastructure --startup-project src\PDPW.API

# Aplicar migra��es
dotnet ef database update --project src\PDPW.Infrastructure --startup-project src\PDPW.API

# Reverter migra��o
dotnet ef database update PreviousMigration --project src\PDPW.Infrastructure --startup-project src\PDPW.API
```

---

## ?? Configura��o

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

### Estrat�gia de Testes

- [ ] **Testes Unit�rios** - Services e Domain
- [ ] **Testes de Integra��o** - Repositories
- [ ] **Testes de API** - Endpoints
- [ ] **Testes E2E** - Fluxos completos

### Ferramentas

- xUnit
- Moq
- FluentAssertions
- Microsoft.AspNetCore.Mvc.Testing

---

## ?? Depend�ncias

### Pacotes NuGet Principais

| Pacote | Vers�o | Projeto | Uso |
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

## ?? Seguran�a

### Implementado

- ? CORS configurado
- ? HTTPS redirection
- ? Health checks
- ? Logging de exce��es
- ? Connection string em appsettings

### A Implementar

- [ ] Autentica��o JWT
- [ ] Autoriza��o por roles
- [ ] Rate limiting
- [ ] API Keys
- [ ] Secrets em Azure Key Vault

---

## ?? Monitoramento

### Logs

- **Provider**: Console, Debug, EventLog
- **N�veis**: Information, Warning, Error, Critical
- **Destino**: Application Insights (futuro)

### M�tricas

- Health check endpoint: `/health`
- Status endpoint: `/`
- Response times (a implementar)
- Error rates (a implementar)

---

## ?? Documenta��o

### Dispon�vel

| Documento | Descri��o |
|-----------|-----------|
| `README.md` | Vis�o geral do projeto |
| `DATABASE_SETUP.md` | Configura��o de banco de dados |
| `TROUBLESHOOTING.md` | Solu��o de problemas |
| `IMPROVEMENTS.md` | Melhorias implementadas |
| `SHARING_GUIDE.md` | Guia de compartilhamento |
| `QUICK_START_INMEMORY.md` | In�cio r�pido |

### Swagger/OpenAPI

- URL: https://localhost:65417/swagger
- Documenta��o interativa de todos os endpoints
- Try-it-out dispon�vel

---

## ??? Roadmap

### Fase 1 - MVP (Atual)
- [x] Estrutura Clean Architecture
- [x] CRUD de DadosEnergeticos
- [x] Suporte a m�ltiplos bancos
- [x] Documenta��o Swagger
- [x] Health checks
- [x] Logging

### Fase 2 - Melhorias
- [ ] Autentica��o/Autoriza��o
- [ ] Valida��es avan�adas
- [ ] Cache (Redis)
- [ ] Pagina��o
- [ ] Filtros avan�ados

### Fase 3 - Produ��o
- [ ] Testes automatizados
- [ ] CI/CD pipeline
- [ ] Containeriza��o (Docker)
- [ ] Monitoring (App Insights)
- [ ] Performance optimization

### Fase 4 - Expans�o
- [ ] Frontend React/Angular
- [ ] Relat�rios
- [ ] Exporta��o de dados
- [ ] APIs de integra��o
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
- **Documenta��o**: Ver arquivos `.md` na raiz do projeto

---

## ?? Changelog

### [Unreleased]
- Estrutura inicial do projeto
- CRUD de DadosEnergeticos
- Suporte InMemory/SQL Server
- Documenta��o completa

---

## ?? Licen�a

[Definir licen�a]

---

**�ltima atualiza��o:** 17/12/2025
**Vers�o da especifica��o:** 1.0
