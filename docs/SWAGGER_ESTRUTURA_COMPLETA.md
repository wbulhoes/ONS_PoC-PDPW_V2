# ?? SWAGGER - Estrutura Completa da Documenta��o

**Projeto:** PDPW PoC - Moderniza��o ONS  
**Objetivo:** Documenta��o interativa de 29 APIs com 154+ endpoints  
**Acesso:** http://localhost:5000/swagger

---

## ?? VIS�O GERAL

### O que � o Swagger?

Swagger (OpenAPI) � uma **documenta��o interativa** da API que permite:
- ? **Visualizar** todos os endpoints dispon�veis
- ? **Testar** APIs diretamente no navegador (sem Postman)
- ? **Entender** schemas de Request/Response
- ? **Gerar** c�digo cliente automaticamente
- ? **Exportar** especifica��o OpenAPI JSON/YAML

---

## ??? ESTRUTURA PROPOSTA

### Layout de Navega��o

```
???????????????????????????????????????????????????????????????
? PDPW API - PoC Moderniza��o ONS                             ?
? Vers�o 1.0                                                  ?
?                                                             ?
? Migrado de .NET Framework/VB.NET/WebForms                  ?
? para .NET 8/C#/Clean Architecture                          ?
?                                                             ?
? 29 APIs | 154+ Endpoints | Cobertura > 60%                ?
???????????????????????????????????????????????????????????????

?? Gest�o de Ativos (6 APIs, 31 endpoints) ?
   ?? Arquivos e Dados (5 APIs, 31 endpoints) ?
   ?? Restri��es e Paradas (6 APIs, 30 endpoints) ?
   ? Opera��o e Gera��o (4 APIs, 21 endpoints) ?
   ?? Dados Consolidados (2 APIs, 12 endpoints) ?
   ?? Gest�o de Equipes (3 APIs, 15 endpoints) ?
   ?? Documentos e Relat�rios (4 APIs, 19 endpoints) ?
```

---

## ?? CATEGORIAS E APIs

### ?? 1. GEST�O DE ATIVOS (31 endpoints)

#### 1.1 Usinas
**Descri��o:** Cadastro e consulta de usinas do sistema el�trico  
**C�digo Legado:** `pdpw_act/pdpw/Dao/UsinaDAO.vb`

| M�todo | Endpoint | Descri��o | Request | Response |
|--------|----------|-----------|---------|----------|
| GET | `/api/usinas` | Lista todas as usinas | - | `UsinaResponseDTO[]` |
| GET | `/api/usinas/{id}` | Busca usina por ID | `int id` | `UsinaResponseDTO` |
| GET | `/api/usinas/codigo/{codigo}` | Busca por c�digo | `string codigo` | `UsinaResponseDTO[]` |
| GET | `/api/usinas/empresa/{codEmpre}` | Busca por empresa | `string codEmpre` | `UsinaResponseDTO[]` |
| POST | `/api/usinas` | Cria nova usina | `UsinaRequestDTO` | `UsinaResponseDTO` |
| PUT | `/api/usinas/{id}` | Atualiza usina | `int id, UsinaUpdateDTO` | `204 No Content` |
| DELETE | `/api/usinas/{id}` | Remove usina (soft) | `int id` | `204 No Content` |

**DTOs:**
```json
// UsinaRequestDTO
{
  "codUsina": "UTE001",
  "nomeUsina": "Angra 1",
  "tpUsinaId": "UTE",
  "codEmpre": "EMP001",
  "potInstalada": 640.0,
  "observacoes": "Usina nuclear"
}

// UsinaResponseDTO
{
  "id": 1,
  "codUsina": "UTE001",
  "nomeUsina": "Angra 1",
  "tpUsinaId": "UTE",
  "tipoUsina": {
    "id": "UTE",
    "descricao": "Usina Termel�trica"
  },
  "empresa": {
    "codEmpre": "EMP001",
    "nomeEmpre": "Eletronuclear"
  },
  "potInstalada": 640.0,
  "ativo": true,
  "dataCriacao": "2024-12-19T10:00:00Z"
}
```

#### 1.2 Empresas
**Endpoints:** 5 (GET all, GET by id, GET by codigo, POST, PUT, DELETE)  
**C�digo Legado:** `pdpw_act/pdpw/Dao/EmpresaDAO.vb`

#### 1.3 Tipos de Usina
**Endpoints:** 4 (GET all, GET by id, POST, PUT)  
**Valores:** UTE, UHE, EOL, UFV, PCH, etc.

#### 1.4 Unidades Geradoras (UG)
**Endpoints:** 5 (CRUD completo + filtros por usina)  
**C�digo Legado:** `pdpw_act/pdpw/Dao/UnidadeGeradoraDAO.vb`

#### 1.5 Usinas Conversoras
**Endpoints:** 5 (CRUD completo)

#### 1.6 Rampas Usina T�rmica
**Endpoints:** 5 (CRUD + consulta por usina)

---

### ?? 2. ARQUIVOS E DADOS (31 endpoints)

#### 2.1 Arquivos DADGER
**Descri��o:** Arquivos de dados de gera��o  
**C�digo Legado:** `pdpw_act/pdpw/Dao/ArquivoDadgerDAO.vb`

| M�todo | Endpoint | Descri��o |
|--------|----------|-----------|
| GET | `/api/arquivos-dadger` | Lista todos os arquivos |
| GET | `/api/arquivos-dadger/{id}` | Busca por ID |
| GET | `/api/arquivos-dadger/semana/{idSemana}` | Busca por semana PMO |
| POST | `/api/arquivos-dadger` | Importa novo arquivo |
| DELETE | `/api/arquivos-dadger/{id}` | Remove arquivo |

**DTO de Response:**
```json
{
  "idArquivoDadger": 1,
  "idSemanaPmo": 1,
  "semanaPmo": {
    "idSemanapmo": 1,
    "numeroSemana": 1,
    "anoMes": "2024-12",
    "dataInicio": "2024-12-01",
    "dataFim": "2024-12-07"
  },
  "dataImportacao": "2024-12-19T09:00:00Z",
  "usuarioImportacao": "admin",
  "quantidadeValores": 450,
  "ativo": true
}
```

#### 2.2 Valores DADGER
**Descri��o:** Valores de CVU, inflexibilidade por usina/semana  
**C�digo Legado:** `pdpw_act/pdpw/Dao/ArquivoDadgerValorDAO.vb`

| M�todo | Endpoint | Descri��o |
|--------|----------|-----------|
| GET | `/api/valores-dadger` | Lista todos os valores |
| GET | `/api/valores-dadger/{id}` | Busca por ID |
| GET | `/api/valores-dadger/arquivo/{idArquivo}` | Valores de um arquivo |
| GET | `/api/valores-dadger/usina/{codUsina}` | Valores de uma usina |
| GET | `/api/valores-dadger/periodo` | Consulta por per�odo (query) |
| POST | `/api/valores-dadger` | Cria novo valor |
| PUT | `/api/valores-dadger/{id}` | Atualiza valor |

**DTO Complexo:**
```json
{
  "idArquivoDadgerValor": 1,
  "idArquivoDadger": 1,
  "codUsina": "UTE001",
  "usina": {
    "codUsina": "UTE001",
    "nomeUsina": "Angra 1"
  },
  "valorCvu": 125.50,
  "valorInflexiLeve": 100.0,
  "valorInflexiMedia": 150.0,
  "valorInflexiPesada": 200.0,
  "numUGsInflexiLeve": 1,
  "numUGsInflexiMedia": 2,
  "numUGsInflexiPesada": 3,
  "dataCriacao": "2024-12-19T10:00:00Z"
}
```

#### 2.3 Semanas PMO
**Endpoints:** 5 (GET all, GET by id, GET by periodo, POST, PUT)

#### 2.4 Cargas
**Endpoints:** 5 (CRUD + consulta por per�odo)

#### 2.5 Uploads
**Endpoints:** 4 (POST file, GET list, GET by id, DELETE)

---

### ?? 3. RESTRI��ES E PARADAS (30 endpoints)

#### 3.1 Paradas UG
**Descri��o:** Paradas programadas de unidades geradoras

| M�todo | Endpoint | Descri��o |
|--------|----------|-----------|
| GET | `/api/paradas-ug` | Lista todas as paradas |
| GET | `/api/paradas-ug/{id}` | Busca por ID |
| GET | `/api/paradas-ug/usina/{codUsina}` | Paradas de uma usina |
| GET | `/api/paradas-ug/periodo` | Paradas em um per�odo |
| POST | `/api/paradas-ug` | Registra nova parada |
| PUT | `/api/paradas-ug/{id}` | Atualiza parada |
| DELETE | `/api/paradas-ug/{id}` | Remove parada |

#### 3.2 Restri��es UG
**Endpoints:** 5 (CRUD + filtros)

#### 3.3 Restri��es US (Usinas)
**Endpoints:** 5 (CRUD + filtros)

#### 3.4 Motivos de Restri��o
**Endpoints:** 4 (enumera��o + CRUD b�sico)

#### 3.5 Inflexibilidade Contratada
**Endpoints:** 6 (CRUD + consultas complexas)

#### 3.6 Modalidade Opera��o T�rmica
**Endpoints:** 5 (CRUD completo)

---

### ? 4. OPERA��O E GERA��O (21 endpoints)

#### 4.1 Interc�mbio
**Descri��o:** Interc�mbio de energia entre subsistemas

| M�todo | Endpoint | Descri��o |
|--------|----------|-----------|
| GET | `/api/intercambio` | Lista todos |
| GET | `/api/intercambio/{id}` | Busca por ID |
| GET | `/api/intercambio/periodo` | Consulta por per�odo |
| POST | `/api/intercambio` | Registra interc�mbio |
| PUT | `/api/intercambio/{id}` | Atualiza |

#### 4.2 Balan�o
**Endpoints:** 5 (CRUD + consulta por subsistema)

#### 4.3 Gera��o Fora de M�rito
**Endpoints:** 5 (CRUD + filtros)

#### 4.4 PDOC (Programa��o Di�ria de Opera��o)
**Endpoints:** 6 (CRUD + consultas especiais)

---

### ?? 5. DADOS CONSOLIDADOS (12 endpoints)

#### 5.1 DCA - Dados Agregados
**Descri��o:** Dados consolidados agregados

| M�todo | Endpoint | Descri��o |
|--------|----------|-----------|
| GET | `/api/dca` | Lista todos |
| GET | `/api/dca/{id}` | Busca por ID |
| GET | `/api/dca/periodo` | Consulta por per�odo |
| GET | `/api/dca/usina/{codUsina}` | DCA de uma usina |
| GET | `/api/dca/resumo` | Resumo estat�stico |
| POST | `/api/dca` | Cria DCA |

#### 5.2 DCR - Dados Consolidados Revisados
**Endpoints:** 6 (mesma estrutura do DCA)

---

### ?? 6. GEST�O DE EQUIPES (15 endpoints)

#### 6.1 Equipes PDP
**Endpoints:** 5 (CRUD completo)

#### 6.2 Usu�rios
**Endpoints:** 5 (CRUD sem autentica��o na PoC)

#### 6.3 Respons�veis
**Endpoints:** 5 (CRUD completo)

---

### ?? 7. DOCUMENTOS E RELAT�RIOS (19 endpoints)

#### 7.1 Diret�rios
**Endpoints:** 5 (estrutura de pastas)

#### 7.2 Arquivos
**Endpoints:** 5 (CRUD de arquivos)

#### 7.3 Relat�rios
**Endpoints:** 5 (gera��o de relat�rios)

#### 7.4 Observa��es
**Endpoints:** 4 (coment�rios gerais)

---

## ?? CONFIGURA��O DO SWAGGER

### Program.cs - Setup Completo

```csharp
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// ... outros servi�os ...

builder.Services.AddSwaggerGen(options =>
{
    // Informa��es da API
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PDPW API - PoC Moderniza��o ONS",
        Version = "v1.0",
        Description = @"
# PDPW - Programa��o Di�ria da Produ��o

API REST para o sistema PDPW do ONS (Operador Nacional do Sistema El�trico).

## ?? Sobre o Projeto

Migra��o de sistema legado para arquitetura moderna:
- **Legado:** .NET Framework 4.8, VB.NET, WebForms, SQL Server
- **Modernizado:** .NET 8, C#, Clean Architecture, EF Core

## ?? Estat�sticas

- **29 APIs** completas
- **154+ endpoints** REST
- **InMemory Database** com seed data realista
- **Cobertura de testes** > 60%
- **C�digo Legado:** 473 arquivos VB.NET analisados

## ?? Como Usar

1. Expanda uma categoria (ex: ?? Gest�o de Ativos)
2. Escolha um endpoint (ex: GET /api/usinas)
3. Clique em **Try it out**
4. Preencha os par�metros (se necess�rio)
5. Clique em **Execute**
6. Veja a resposta em tempo real

## ?? Documenta��o Adicional

- [Reposit�rio GitHub](https://github.com/wbulhoes/ONS_PoC-PDPW)
- [An�lise do C�digo Legado](docs/ANALISE_TECNICA_CODIGO_LEGADO.md)
- [Decis�es Arquiteturais](VERTICAL_SLICES_DECISION.md)

## ?? Contato

**Equipe:** PDPW PoC Squad  
**Per�odo:** 19/12/2024 - 26/12/2024  
**Cliente:** ONS
        ",
        Contact = new OpenApiContact
        {
            Name = "Equipe PDPW PoC",
            Email = "equipe@exemplo.com",
            Url = new Uri("https://github.com/wbulhoes/ONS_PoC-PDPW")
        },
        License = new OpenApiLicense
        {
            Name = "Uso Interno ONS",
            Url = new Uri("https://www.ons.org.br")
        }
    });

    // XML Comments (documenta��o rica)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // Organizar por Tags (categorias)
    options.TagActionsBy(api =>
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }

        var controllerName = api.ActionDescriptor.RouteValues["controller"];
        return new[] { controllerName ?? "Outros" };
    });

    // Ordenar endpoints alfabeticamente
    options.OrderActionsBy(api => 
        $"{api.GroupName}_{api.ActionDescriptor.RouteValues["controller"]}_{api.HttpMethod}");

    // Schemas com exemplos
    options.EnableAnnotations();
    options.SchemaFilter<SwaggerSchemaExampleFilter>();

    // Suporte a enums como strings
    options.SchemaFilter<EnumSchemaFilter>();

    // Adicionar bot�o de autoriza��o (se implementar JWT futuramente)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    // Swagger sempre habilitado na PoC
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "PDPW API v1.0");
        options.RoutePrefix = "swagger"; // Acesso via /swagger
        
        // Configura��es de UI
        options.DocumentTitle = "PDPW API - Swagger";
        options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
        options.DefaultModelsExpandDepth(2);
        options.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Example);
        options.DisplayRequestDuration();
        options.EnableFilter();
        options.ShowExtensions();
        
        // Tema escuro (opcional)
        // options.InjectStylesheet("/swagger-ui/custom.css");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

---

## ??? EXEMPLO DE CONTROLLER COM TAGS

### UsinasController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs;
using PDPW.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace PDPW.API.Controllers;

/// <summary>
/// Gerenciamento de Usinas do Sistema El�trico
/// </summary>
/// <remarks>
/// Migrado de: pdpw_act/pdpw/Dao/UsinaDAO.vb
/// 
/// Funcionalidades:
/// - Cadastro de novas usinas (UTE, UHE, EOL, UFV, etc.)
/// - Consulta de usinas por diversos crit�rios
/// - Atualiza��o de dados cadastrais
/// - Remo��o l�gica (soft delete)
/// 
/// Regras de Neg�cio:
/// - C�digo da usina deve ser �nico
/// - Pot�ncia instalada deve ser maior que zero
/// - Empresa deve existir no cadastro
/// - Tipo de usina deve ser v�lido
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Tags("?? Gest�o de Ativos")]
public class UsinasController : ControllerBase
{
    private readonly IUsinaService _service;
    private readonly ILogger<UsinasController> _logger;

    public UsinasController(IUsinaService service, ILogger<UsinasController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Lista todas as usinas cadastradas
    /// </summary>
    /// <remarks>
    /// Exemplo de request:
    /// 
    ///     GET /api/usinas
    /// 
    /// Retorna lista completa de usinas ativas no sistema.
    /// </remarks>
    /// <returns>Lista de usinas</returns>
    /// <response code="200">Retorna a lista de usinas</response>
    /// <response code="500">Erro interno do servidor</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsinaResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(
        Summary = "Lista todas as usinas",
        Description = "Retorna lista completa de usinas ativas ordenadas por nome",
        OperationId = "Usinas_GetAll",
        Tags = new[] { "?? Gest�o de Ativos" }
    )]
    public async Task<ActionResult<IEnumerable<UsinaResponseDTO>>> GetAll()
    {
        try
        {
            var usinas = await _service.ObterTodasAsync();
            return Ok(usinas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar usinas");
            return StatusCode(500, new { message = "Erro ao listar usinas", detail = ex.Message });
        }
    }

    /// <summary>
    /// Busca usina por ID
    /// </summary>
    /// <param name="id">ID da usina</param>
    /// <returns>Dados da usina</returns>
    /// <response code="200">Retorna a usina encontrada</response>
    /// <response code="404">Usina n�o encontrada</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UsinaResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Busca usina por ID",
        Description = "Retorna os detalhes de uma usina espec�fica",
        OperationId = "Usinas_GetById"
    )]
    public async Task<ActionResult<UsinaResponseDTO>> GetById(int id)
    {
        var usina = await _service.ObterPorIdAsync(id);
        if (usina == null)
            return NotFound(new { message = $"Usina com ID {id} n�o encontrada" });

        return Ok(usina);
    }

    /// <summary>
    /// Busca usinas por c�digo
    /// </summary>
    /// <param name="codigo">C�digo da usina (ex: UTE001)</param>
    /// <returns>Lista de usinas com o c�digo especificado</returns>
    /// <response code="200">Retorna as usinas encontradas</response>
    [HttpGet("codigo/{codigo}")]
    [ProducesResponseType(typeof(IEnumerable<UsinaResponseDTO>), StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "Busca por c�digo",
        Description = "Busca usinas pelo c�digo (pode retornar m�ltiplas se houver hist�rico)",
        OperationId = "Usinas_GetByCodigo"
    )]
    [SwaggerResponse(200, "Usinas encontradas", typeof(IEnumerable<UsinaResponseDTO>))]
    public async Task<ActionResult<IEnumerable<UsinaResponseDTO>>> GetByCodigo(string codigo)
    {
        var usinas = await _service.ObterPorCodigoAsync(codigo);
        return Ok(usinas);
    }

    /// <summary>
    /// Cria uma nova usina
    /// </summary>
    /// <param name="request">Dados da usina</param>
    /// <returns>Usina criada</returns>
    /// <response code="201">Usina criada com sucesso</response>
    /// <response code="400">Dados inv�lidos</response>
    /// <response code="409">C�digo da usina j� existe</response>
    /// <remarks>
    /// Exemplo de request:
    /// 
    ///     POST /api/usinas
    ///     {
    ///       "codUsina": "UTE001",
    ///       "nomeUsina": "Angra 1",
    ///       "tpUsinaId": "UTE",
    ///       "codEmpre": "EMP001",
    ///       "potInstalada": 640.0,
    ///       "observacoes": "Usina nuclear"
    ///     }
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(typeof(UsinaResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [SwaggerOperation(
        Summary = "Cria nova usina",
        Description = "Registra uma nova usina no sistema",
        OperationId = "Usinas_Create"
    )]
    public async Task<ActionResult<UsinaResponseDTO>> Create([FromBody] UsinaRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var usina = await _service.CriarAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = usina.Id }, usina);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar usina");
            return StatusCode(500, new { message = "Erro ao criar usina", detail = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza dados de uma usina
    /// </summary>
    /// <param name="id">ID da usina</param>
    /// <param name="request">Novos dados</param>
    /// <returns>Sem conte�do</returns>
    /// <response code="204">Atualizado com sucesso</response>
    /// <response code="404">Usina n�o encontrada</response>
    /// <response code="400">Dados inv�lidos</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Atualiza usina",
        Description = "Atualiza os dados de uma usina existente",
        OperationId = "Usinas_Update"
    )]
    public async Task<IActionResult> Update(int id, [FromBody] UsinaUpdateDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            await _service.AtualizarAsync(id, request);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar usina {Id}", id);
            return StatusCode(500, new { message = "Erro ao atualizar usina", detail = ex.Message });
        }
    }

    /// <summary>
    /// Remove uma usina (soft delete)
    /// </summary>
    /// <param name="id">ID da usina</param>
    /// <returns>Sem conte�do</returns>
    /// <response code="204">Removido com sucesso</response>
    /// <response code="404">Usina n�o encontrada</response>
    /// <remarks>
    /// Realiza remo��o l�gica (soft delete).
    /// A usina n�o � exclu�da do banco, apenas marcada como inativa.
    /// </remarks>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Remove usina",
        Description = "Remove logicamente uma usina (soft delete)",
        OperationId = "Usinas_Delete"
    )]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.RemoverAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover usina {Id}", id);
            return StatusCode(500, new { message = "Erro ao remover usina", detail = ex.Message });
        }
    }
}
```

---

## ?? EXEMPLOS DE DTOs DOCUMENTADOS

### UsinaRequestDTO.cs

```csharp
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace PDPW.Application.DTOs;

/// <summary>
/// DTO para cria��o de uma nova usina
/// </summary>
[SwaggerSchema(Description = "Dados necess�rios para cadastrar uma nova usina")]
public class UsinaRequestDTO
{
    /// <summary>
    /// C�digo �nico da usina
    /// </summary>
    /// <example>UTE001</example>
    [Required(ErrorMessage = "C�digo da usina � obrigat�rio")]
    [StringLength(10, MinimumLength = 3, ErrorMessage = "C�digo deve ter entre 3 e 10 caracteres")]
    [SwaggerSchema("C�digo identificador da usina (ex: UTE001, UHE002)")]
    public string CodUsina { get; set; } = string.Empty;

    /// <summary>
    /// Nome da usina
    /// </summary>
    /// <example>Angra 1</example>
    [Required(ErrorMessage = "Nome da usina � obrigat�rio")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Nome deve ter entre 3 e 100 caracteres")]
    [SwaggerSchema("Nome completo da usina")]
    public string NomeUsina { get; set; } = string.Empty;

    /// <summary>
    /// Tipo da usina
    /// </summary>
    /// <example>UTE</example>
    [Required(ErrorMessage = "Tipo da usina � obrigat�rio")]
    [SwaggerSchema("Tipo: UTE (Termel�trica), UHE (Hidrel�trica), EOL (E�lica), UFV (Fotovoltaica)")]
    public string TpUsinaId { get; set; } = string.Empty;

    /// <summary>
    /// C�digo da empresa propriet�ria
    /// </summary>
    /// <example>EMP001</example>
    [Required(ErrorMessage = "Empresa � obrigat�ria")]
    [SwaggerSchema("C�digo da empresa que opera a usina")]
    public string CodEmpre { get; set; } = string.Empty;

    /// <summary>
    /// Pot�ncia instalada em MW
    /// </summary>
    /// <example>640.0</example>
    [Range(0.1, 99999.99, ErrorMessage = "Pot�ncia deve ser maior que zero")]
    [SwaggerSchema("Capacidade instalada em megawatts (MW)")]
    public decimal PotInstalada { get; set; }

    /// <summary>
    /// Observa��es adicionais
    /// </summary>
    /// <example>Usina nuclear localizada em Angra dos Reis</example>
    [StringLength(500, ErrorMessage = "Observa��es n�o podem ultrapassar 500 caracteres")]
    [SwaggerSchema("Informa��es complementares sobre a usina", Nullable = true)]
    public string? Observacoes { get; set; }
}
```

---

## ?? TESTES PELO SWAGGER

### Como Testar uma API

#### 1. Criar uma Usina (POST)

1. Expanda **?? Gest�o de Ativos**
2. Clique em **POST /api/usinas**
3. Clique em **Try it out**
4. Cole o JSON de exemplo:
```json
{
  "codUsina": "UTE999",
  "nomeUsina": "Teste Swagger",
  "tpUsinaId": "UTE",
  "codEmpre": "EMP001",
  "potInstalada": 500.0,
  "observacoes": "Criado via Swagger UI"
}
```
5. Clique em **Execute**
6. Veja a resposta **201 Created** com o ID gerado

#### 2. Listar Todas (GET)

1. Clique em **GET /api/usinas**
2. Clique em **Try it out**
3. Clique em **Execute**
4. Veja a lista com a usina criada

#### 3. Buscar por ID (GET)

1. Clique em **GET /api/usinas/{id}**
2. Clique em **Try it out**
3. Digite o ID retornado no POST (ex: 6)
4. Clique em **Execute**
5. Veja os detalhes da usina

#### 4. Atualizar (PUT)

1. Clique em **PUT /api/usinas/{id}**
2. Clique em **Try it out**
3. Digite o ID
4. Atualize o JSON (ex: mudar potInstalada para 550.0)
5. Clique em **Execute**
6. Veja **204 No Content** (sucesso)

#### 5. Deletar (DELETE)

1. Clique em **DELETE /api/usinas/{id}**
2. Clique em **Try it out**
3. Digite o ID
4. Clique em **Execute**
5. Veja **204 No Content**

---

## ?? EXPORTAR ESPECIFICA��O

### Baixar OpenAPI JSON

1. Acesse: http://localhost:5000/swagger/v1/swagger.json
2. Salve o arquivo JSON
3. Importe no Postman, Insomnia ou outra ferramenta

### Gerar Cliente TypeScript (Exemplo)

```bash
# Instalar gerador
npm install -g @openapitools/openapi-generator-cli

# Gerar cliente
openapi-generator-cli generate \
  -i http://localhost:5000/swagger/v1/swagger.json \
  -g typescript-axios \
  -o ./frontend/src/api-client
```

---

## ?? POSTMAN COLLECTION

### Alternativa ao Swagger

Se preferir usar Postman:

1. Baixe: http://localhost:5000/swagger/v1/swagger.json
2. Abra Postman
3. **Import** ? Cole a URL ou selecione o arquivo
4. Cole��o completa criada automaticamente!

---

## ?? CHECKLIST DE VALIDA��O

### Para cada API, validar no Swagger:

- [ ] Todos os endpoints aparecem na documenta��o
- [ ] Descri��es est�o claras e completas
- [ ] Exemplos de Request est�o corretos
- [ ] Exemplos de Response correspondem aos DTOs
- [ ] C�digos de resposta HTTP est�o documentados
- [ ] **Try it out** funciona sem erros
- [ ] Valida��es de campos retornam 400 Bad Request
- [ ] Entidades n�o encontradas retornam 404 Not Found
- [ ] Respostas de sucesso retornam dados corretos

---

## ?? SUPORTE

**Problemas comuns:**

1. **Swagger n�o abre:**
   - Verifique se a API est� rodando: `dotnet run`
   - Acesse: http://localhost:5000/swagger (n�o /swagger/index.html)

2. **Endpoints n�o aparecem:**
   - Verifique se Controllers t�m `[ApiController]`
   - Verifique se `[Route]` est� correto
   - Rebuild o projeto: `dotnet build`

3. **DTOs sem descri��o:**
   - Habilite XML Documentation no .csproj
   - Adicione coment�rios `///` nas classes
   - Rebuild

4. **Try it out n�o funciona:**
   - Verifique CORS no backend
   - Veja console do navegador (F12)
   - Verifique logs da API

---

**Documento preparado por:** GitHub Copilot  
**Data:** 19/12/2024  
**Vers�o:** 1.0

**Este Swagger ser� a vitrine t�cnica da PoC!** ????
