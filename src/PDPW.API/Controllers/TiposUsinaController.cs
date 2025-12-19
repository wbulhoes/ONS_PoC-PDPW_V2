using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.TipoUsina;
using PDPW.Application.Interfaces;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller para gerenciamento de Tipos de Usina
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TiposUsinaController : BaseController
{
    private readonly ITipoUsinaService _service;
    private readonly ILogger<TiposUsinaController> _logger;

    public TiposUsinaController(
        ITipoUsinaService service,
        ILogger<TiposUsinaController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todos os tipos de usina
    /// </summary>
    /// <returns>Lista de tipos de usina</returns>
    /// <response code="200">Lista de tipos de usina retornada com sucesso</response>
    [HttpGet(Name = nameof(GetAllTiposUsina))]
    [ProducesResponseType(typeof(List<TipoUsinaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTiposUsina()
    {
        _logger.LogInformation("GET api/tiposusina - Buscando todos os tipos de usina");
        
        var tiposUsina = await _service.GetAllAsync();
        return Ok(tiposUsina);
    }

    /// <summary>
    /// Obtém um tipo de usina por ID
    /// </summary>
    /// <param name="id">ID do tipo de usina</param>
    /// <returns>Tipo de usina encontrado</returns>
    /// <response code="200">Tipo de usina encontrado</response>
    /// <response code="404">Tipo de usina não encontrado</response>
    [HttpGet("{id:int}", Name = nameof(GetTipoUsinaById))]
    [ProducesResponseType(typeof(TipoUsinaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTipoUsinaById(int id)
    {
        _logger.LogInformation("GET api/tiposusina/{Id} - Buscando tipo de usina por ID", id);
        
        var tipoUsina = await _service.GetByIdAsync(id);
        return HandleResult(tipoUsina);
    }

    /// <summary>
    /// Obtém um tipo de usina por nome
    /// </summary>
    /// <param name="nome">Nome do tipo de usina</param>
    /// <returns>Tipo de usina encontrado</returns>
    /// <response code="200">Tipo de usina encontrado</response>
    /// <response code="404">Tipo de usina não encontrado</response>
    [HttpGet("nome/{nome}", Name = nameof(GetTipoUsinaByNome))]
    [ProducesResponseType(typeof(TipoUsinaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTipoUsinaByNome(string nome)
    {
        _logger.LogInformation("GET api/tiposusina/nome/{Nome} - Buscando tipo de usina por nome", nome);
        
        var tipoUsina = await _service.GetByNomeAsync(nome);
        return HandleResult(tipoUsina);
    }

    /// <summary>
    /// Cria um novo tipo de usina
    /// </summary>
    /// <param name="dto">Dados do tipo de usina</param>
    /// <returns>Tipo de usina criado</returns>
    /// <response code="201">Tipo de usina criado com sucesso</response>
    /// <response code="400">Dados inválidos ou nome duplicado</response>
    [HttpPost(Name = nameof(CreateTipoUsina))]
    [ProducesResponseType(typeof(TipoUsinaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTipoUsina([FromBody] CreateTipoUsinaDto dto)
    {
        _logger.LogInformation("POST api/tiposusina - Criando novo tipo de usina: {Nome}", dto.Nome);

        try
        {
            var tipoUsina = await _service.CreateAsync(dto);
            return CreatedAtRoute(
                nameof(GetTipoUsinaById),
                new { id = tipoUsina.Id },
                tipoUsina);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar tipo de usina");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza um tipo de usina existente
    /// </summary>
    /// <param name="id">ID do tipo de usina</param>
    /// <param name="dto">Dados atualizados</param>
    /// <returns>Tipo de usina atualizado</returns>
    /// <response code="200">Tipo de usina atualizado com sucesso</response>
    /// <response code="400">Dados inválidos ou nome duplicado</response>
    /// <response code="404">Tipo de usina não encontrado</response>
    [HttpPut("{id:int}", Name = nameof(UpdateTipoUsina))]
    [ProducesResponseType(typeof(TipoUsinaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTipoUsina(int id, [FromBody] UpdateTipoUsinaDto dto)
    {
        _logger.LogInformation("PUT api/tiposusina/{Id} - Atualizando tipo de usina", id);

        try
        {
            var tipoUsina = await _service.UpdateAsync(id, dto);
            return HandleResult(tipoUsina);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar tipo de usina");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Remove um tipo de usina (soft delete)
    /// </summary>
    /// <param name="id">ID do tipo de usina</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Tipo de usina removido com sucesso</response>
    /// <response code="400">Não é possível remover tipo com usinas vinculadas</response>
    /// <response code="404">Tipo de usina não encontrado</response>
    [HttpDelete("{id:int}", Name = nameof(DeleteTipoUsina))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTipoUsina(int id)
    {
        _logger.LogInformation("DELETE api/tiposusina/{Id} - Removendo tipo de usina", id);

        try
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound(new { message = $"Tipo de usina com ID {id} não encontrado" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro ao tentar remover tipo de usina com usinas vinculadas");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Verifica se já existe um tipo de usina com o nome informado
    /// </summary>
    /// <param name="nome">Nome a verificar</param>
    /// <param name="tipoUsinaId">ID do tipo de usina a excluir da verificação (opcional)</param>
    /// <returns>Indica se o nome já existe</returns>
    /// <response code="200">Resultado da verificação</response>
    [HttpGet("verificar-nome/{nome}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerificarNomeExiste(string nome, [FromQuery] int? tipoUsinaId = null)
    {
        _logger.LogInformation("GET api/tiposusina/verificar-nome/{Nome} - Verificando existência de nome", nome);
        
        var existe = await _service.ExisteNomeAsync(nome, tipoUsinaId);
        return Ok(new { existe });
    }
}
