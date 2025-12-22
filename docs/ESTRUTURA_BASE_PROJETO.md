# ??? ESTRUTURA BASE DO PROJETO - Clean Architecture

**Data:** 19/12/2024  
**Arquitetura:** Clean Architecture + MVC  
**Framework:** .NET 8  
**Status:** ? Base existente + Expans�o planejada

---

## ?? ARQUITETURA ATUAL

```
PDPW (Solution)
?
??? src/
?   ??? PDPW.API/              (Presentation Layer - MVC)
?   ??? PDPW.Application/      (Application Layer - Services)
?   ??? PDPW.Domain/           (Domain Layer - Entities)
?   ??? PDPW.Infrastructure/   (Infrastructure Layer - Data Access)
?
??? frontend/                  (React + TypeScript)
?
??? docs/                      (Documenta��o)
?
??? docker-compose.yml         (Docker orchestration)
```

---

## ?? CLEAN ARCHITECTURE - CAMADAS

### 1. DOMAIN LAYER (N�cleo)

**Responsabilidade:** Entidades de neg�cio, regras de dom�nio, interfaces

**Localiza��o:** `src/PDPW.Domain/`

**Estrutura:**
```
PDPW.Domain/
??? Entities/              (Entidades de dom�nio)
?   ??? BaseEntity.cs      ? (j� existe)
?   ??? DadoEnergetico.cs  ? (j� existe)
?   ??? Usina.cs           ?? (criar)
?   ??? Empresa.cs         ?? (criar)
?   ??? TipoUsina.cs       ?? (criar)
?   ??? UnidadeGeradora.cs ?? (criar)
?   ??? ... (29 entidades total)
?
??? Interfaces/            (Contratos de reposit�rio)
?   ??? IDadoEnergeticoRepository.cs  ? (j� existe)
?   ??? IUsinaRepository.cs           ?? (criar)
?   ??? IEmpresaRepository.cs         ?? (criar)
?   ??? ... (29 interfaces)
?
??? Enums/                 (Enumera��es do dom�nio)
?   ??? StatusUsina.cs     ?? (criar)
?   ??? TipoEnergia.cs     ?? (criar)
?   ??? ...
?
??? ValueObjects/          (Objetos de valor)
    ??? Potencia.cs        ?? (opcional)
    ??? ...
```

**Depend�ncias:** NENHUMA (camada mais interna)

---

### 2. APPLICATION LAYER (Servi�os)

**Responsabilidade:** L�gica de aplica��o, DTOs, orquestra��o

**Localiza��o:** `src/PDPW.Application/`

**Estrutura:**
```
PDPW.Application/
??? DTOs/                  (Data Transfer Objects)
?   ??? DadoEnergeticoDto.cs        ? (existe)
?   ??? Usina/
?   ?   ??? UsinaDto.cs             ?? (criar)
?   ?   ??? CreateUsinaDto.cs       ?? (criar)
?   ?   ??? UpdateUsinaDto.cs       ?? (criar)
?   ??? Empresa/
?   ?   ??? EmpresaDto.cs           ?? (criar)
?   ?   ??? ...
?   ??? ... (29 grupos de DTOs)
?
??? Services/              (L�gica de neg�cio)
?   ??? DadoEnergeticoService.cs    ? (existe)
?   ??? UsinaService.cs             ?? (criar)
?   ??? EmpresaService.cs           ?? (criar)
?   ??? ... (29 services)
?
??? Interfaces/            (Contratos de servi�o)
?   ??? IDadoEnergeticoService.cs   ? (existe)
?   ??? IUsinaService.cs            ?? (criar)
?   ??? ...
?
??? Mappings/              (AutoMapper profiles)
?   ??? DadoEnergeticoProfile.cs    ?? (criar)
?   ??? UsinaProfile.cs             ?? (criar)
?   ??? ...
?
??? Validators/            (FluentValidation)
    ??? CreateUsinaDtoValidator.cs  ?? (criar)
    ??? ...
```

**Depend�ncias:** PDPW.Domain

---

### 3. INFRASTRUCTURE LAYER (Infraestrutura)

**Responsabilidade:** Acesso a dados, EF Core, SQL Server

**Localiza��o:** `src/PDPW.Infrastructure/`

**Estrutura:**
```
PDPW.Infrastructure/
??? Data/                  (Entity Framework Core)
?   ??? ApplicationDbContext.cs     ? (existe)
?   ??? Configurations/             (EF Configurations)
?   ?   ??? DadoEnergeticoConfiguration.cs  ?? (criar)
?   ?   ??? UsinaConfiguration.cs           ?? (criar)
?   ?   ??? ... (29 configurations)
?   ?
?   ??? Migrations/                 (EF Migrations)
?   ?   ??? ... (geradas automaticamente)
?   ?
?   ??? Seed/                       (Dados iniciais)
?       ??? DbSeeder.cs             ?? (criar)
?       ??? UsinaSeed.cs            ?? (criar)
?       ??? EmpresaSeed.cs          ?? (criar)
?       ??? ... (seed para todas as entidades)
?
??? Repositories/          (Implementa��es de reposit�rio)
?   ??? BaseRepository.cs           ?? (criar)
?   ??? DadoEnergeticoRepository.cs ? (existe)
?   ??? UsinaRepository.cs          ?? (criar)
?   ??? ... (29 repositories)
?
??? DependencyInjection.cs (Inje��o de depend�ncias)
```

**Depend�ncias:** PDPW.Domain, PDPW.Application

---

### 4. API LAYER (Presentation - MVC)

**Responsabilidade:** Controllers, Routing, Swagger, Startup

**Localiza��o:** `src/PDPW.API/`

**Estrutura:**
```
PDPW.API/
??? Controllers/           (Controllers MVC)
?   ??? DadosEnergeticosController.cs  ? (existe)
?   ??? UsinasController.cs            ?? (criar)
?   ??? EmpresasController.cs          ?? (criar)
?   ??? ... (29 controllers)
?
??? Filters/               (Action Filters)
?   ??? ValidationFilter.cs            ?? (criar)
?   ??? ExceptionFilter.cs             ?? (criar)
?
??? Middlewares/           (Custom Middlewares)
?   ??? ErrorHandlingMiddleware.cs     ?? (criar)
?   ??? RequestLoggingMiddleware.cs    ?? (criar)
?
??? Extensions/            (Extension Methods)
?   ??? ServiceCollectionExtensions.cs ?? (criar)
?   ??? ...
?
??? Program.cs             ? (existe - configurar)
??? appsettings.json       ? (existe - configurar)
??? appsettings.Development.json
```

**Depend�ncias:** PDPW.Application, PDPW.Infrastructure

---

## ?? ESTRUTURA COMPLETA EXPANDIDA

### Para 29 APIs, cada grupo ter�:

```
API: Usina (exemplo)
?
??? Domain/
?   ??? Entities/Usina.cs
?   ??? Interfaces/IUsinaRepository.cs
?
??? Application/
?   ??? DTOs/Usina/
?   ?   ??? UsinaDto.cs
?   ?   ??? CreateUsinaDto.cs
?   ?   ??? UpdateUsinaDto.cs
?   ??? Services/UsinaService.cs
?   ??? Interfaces/IUsinaService.cs
?   ??? Mappings/UsinaProfile.cs
?   ??? Validators/CreateUsinaDtoValidator.cs
?
??? Infrastructure/
?   ??? Data/
?   ?   ??? Configurations/UsinaConfiguration.cs
?   ?   ??? Seed/UsinaSeed.cs
?   ??? Repositories/UsinaRepository.cs
?
??? API/
    ??? Controllers/UsinasController.cs
```

**Multiplicar isso por 29 APIs!**

---

## ?? PADR�ES E CONVEN��ES

### Nomenclatura

```csharp
// Entidades (singular)
public class Usina : BaseEntity { }

// DTOs (sufixo Dto)
public class UsinaDto { }
public class CreateUsinaDto { }
public class UpdateUsinaDto { }

// Interfaces (prefixo I)
public interface IUsinaRepository { }
public interface IUsinaService { }

// Services (sufixo Service)
public class UsinaService : IUsinaService { }

// Repositories (sufixo Repository)
public class UsinaRepository : IUsinaRepository { }

// Controllers (plural + sufixo Controller)
public class UsinasController : ControllerBase { }

// Seed (sufixo Seed)
public class UsinaSeed { }

// Configuration (sufixo Configuration)
public class UsinaConfiguration : IEntityTypeConfiguration<Usina> { }
```

---

### Namespaces

```csharp
// Domain
namespace PDPW.Domain.Entities;
namespace PDPW.Domain.Interfaces;

// Application
namespace PDPW.Application.DTOs.Usina;
namespace PDPW.Application.Services;
namespace PDPW.Application.Interfaces;

// Infrastructure
namespace PDPW.Infrastructure.Data;
namespace PDPW.Infrastructure.Repositories;

// API
namespace PDPW.API.Controllers;
```

---

## ?? ARQUIVOS BASE ESSENCIAIS

### BaseEntity.cs (? J� existe)

```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    public bool Ativo { get; set; } = true;
}
```

### BaseRepository.cs (?? Criar)

```csharp
public abstract class BaseRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.Where(e => e.Ativo).ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id && e.Ativo);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        entity.DataCriacao = DateTime.UtcNow;
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        entity.DataAtualizacao = DateTime.UtcNow;
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            entity.Ativo = false;
            entity.DataAtualizacao = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
```

---

### BaseController.cs (?? Criar)

```csharp
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseController : ControllerBase
{
    protected IActionResult HandleResult<T>(T? result)
    {
        if (result == null)
            return NotFound();
        
        return Ok(result);
    }

    protected IActionResult HandleError(Exception ex)
    {
        // Log error
        return StatusCode(500, new { error = "Erro interno do servidor" });
    }
}
```

---

## ?? FLUXO DE DADOS

```
HTTP Request
    ?
Controller (API Layer)
    ?
Service (Application Layer)
    ? 
Repository (Infrastructure Layer)
    ?
DbContext (EF Core)
    ?
SQL Server Database
```

**Resposta:**
```
Database
    ?
Entity (Domain)
    ?
DTO (Application)
    ?
JSON (API)
    ?
HTTP Response
```

---

## ??? BANCO DE DADOS

### ApplicationDbContext

```csharp
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    // DbSets existentes
    public DbSet<DadoEnergetico> DadosEnergeticos { get; set; }
    
    // DbSets a criar (29 APIs)
    public DbSet<Usina> Usinas { get; set; }
    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<TipoUsina> TiposUsina { get; set; }
    public DbSet<UnidadeGeradora> UnidadesGeradoras { get; set; }
    // ... 25 mais

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Aplicar todas as configura��es
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        // Aplicar seed data
        DbSeeder.Seed(modelBuilder);
    }
}
```

---

## ?? DEPEND�NCIAS ENTRE CAMADAS

```
???????????????????????????????????????
?         PDPW.API                    ?  ? Presentation
?  (Controllers, Swagger, Startup)   ?
???????????????????????????????????????
             ? depende de
             ?
???????????????????????????????????????
?      PDPW.Application               ?  ? Application
?   (Services, DTOs, Validators)     ?
???????????????????????????????????????
             ? depende de
             ?
???????????????????????????????????????
?        PDPW.Domain                  ?  ? Domain (Core)
?  (Entities, Interfaces, Enums)     ?  ? SEM DEPEND�NCIAS
???????????????????????????????????????
             ?
             ? implementa interfaces
             ?
???????????????????????????????????????
?     PDPW.Infrastructure             ?  ? Infrastructure
? (Repositories, EF Core, SQL)       ?
???????????????????????????????????????
```

---

## ? CHECKLIST ESTRUTURA BASE

### Arquivos Base a Criar

- [ ] `Infrastructure/Repositories/BaseRepository.cs`
- [ ] `API/Controllers/BaseController.cs`
- [ ] `Infrastructure/Data/Seed/DbSeeder.cs`
- [ ] `Application/Mappings/AutoMapperProfile.cs`
- [ ] `API/Filters/ValidationFilter.cs`
- [ ] `API/Filters/ExceptionFilter.cs`
- [ ] `API/Middlewares/ErrorHandlingMiddleware.cs`
- [ ] `API/Extensions/ServiceCollectionExtensions.cs`

### Configura��es

- [ ] `appsettings.json` - Connection strings
- [ ] `Program.cs` - DI, Swagger, CORS
- [ ] NuGet packages instalados

---

## ?? PACOTES NUGET NECESS�RIOS

```xml
<!-- PDPW.Domain -->
<!-- Sem pacotes adicionais -->

<!-- PDPW.Application -->
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="FluentValidation" Version="11.9.0" />

<!-- PDPW.Infrastructure -->
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />

<!-- PDPW.API -->
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
```

---

## ?? PR�XIMOS PASSOS

1. ? **Estrutura base documentada**
2. ?? **Criar arquivos base** (BaseRepository, BaseController, etc.)
3. ?? **Configurar Program.cs** (DI, Swagger, CORS)
4. ?? **Come�ar primeira API** (Usina)
5. ?? **Distribuir APIs** entre DEV 1 e DEV 2

---

**Documenta��o criada por:** GitHub Copilot  
**Data:** 19/12/2024  
**Vers�o:** 1.0  
**Status:** ? COMPLETO

**Pr�ximo:** Criar distribui��o de APIs entre DEV 1 e DEV 2
