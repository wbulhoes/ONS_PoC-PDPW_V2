using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.UnidadeGeradora;
using PDPW.Application.Interfaces;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller para gerenciamento de Unidades Geradoras
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UnidadesGeradorasController : ControllerBase
{
    private readonly IUnidadeGeradoraService _service;
    private readonly ILogger<UnidadesGeradorasController> _logger;

    public UnidadesGeradorasController(
        IUnidadeGeradoraService service,
        ILogger<UnidadesGeradorasController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Lista todas as unidades geradoras
    /// </summary>
    /// <returns>Lista de unidades geradoras</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UnidadeGeradoraDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UnidadeGeradoraDto>>> GetAll()
    {
        try
        {
            var unidades = await _service.GetAllAsync();
            return Ok(unidades);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar todas as unidades geradoras");
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Busca uma unidade geradora por ID
    /// </summary>
    /// <param name="id">ID da unidade geradora</param>
    /// <returns>Unidade geradora encontrada</returns>
    /// <response code="200">Unidade geradora encontrada</response>
    /// <response code="404">Unidade geradora não encontrada</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UnidadeGeradoraDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UnidadeGeradoraDto>> GetById(int id)
    {
        try
        {
            var unidade = await _service.GetByIdAsync(id);
            if (unidade == null)
            {
                return NotFound($"Unidade geradora com ID {id} não encontrada");
            }

            return Ok(unidade);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar unidade geradora com ID {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Busca uma unidade geradora por código
    /// </summary>
    /// <param name="codigo">Código da unidade geradora</param>
    /// <returns>Unidade geradora encontrada</returns>
    /// <response code="200">Unidade geradora encontrada</response>
    /// <response code="404">Unidade geradora não encontrada</response>
    [HttpGet("codigo/{codigo}")]
    [ProducesResponseType(typeof(UnidadeGeradoraDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UnidadeGeradoraDto>> GetByCodigo(string codigo)
    {
        try
        {
            var unidade = await _service.GetByCodigoAsync(codigo);
            if (unidade == null)
            {
                return NotFound($"Unidade geradora com código {codigo} não encontrada");
            }

            return Ok(unidade);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar unidade geradora com código {Codigo}", codigo);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Lista unidades geradoras de uma usina
    /// </summary>
    /// <param name="usinaId">ID da usina</param>
    /// <returns>Lista de unidades geradoras da usina</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("usina/{usinaId}")]
    [ProducesResponseType(typeof(IEnumerable<UnidadeGeradoraDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UnidadeGeradoraDto>>> GetByUsina(int usinaId)
    {
        try
        {
            var unidades = await _service.GetByUsinaAsync(usinaId);
            return Ok(unidades);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar unidades geradoras da usina {UsinaId}", usinaId);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Lista unidades geradoras por status
    /// </summary>
    /// <param name="status">Status da unidade geradora</param>
    /// <returns>Lista de unidades geradoras com o status especificado</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(IEnumerable<UnidadeGeradoraDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UnidadeGeradoraDto>>> GetByStatus(string status)
    {
        try
        {
            var unidades = await _service.GetByStatusAsync(status);
            return Ok(unidades);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar unidades geradoras com status {Status}", status);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Lista apenas unidades geradoras ativas
    /// </summary>
    /// <returns>Lista de unidades geradoras ativas</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("ativas")]
    [ProducesResponseType(typeof(IEnumerable<UnidadeGeradoraDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UnidadeGeradoraDto>>> GetAtivas()
    {
        try
        {
            var unidades = await _service.GetAtivasAsync();
            return Ok(unidades);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar unidades geradoras ativas");
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Cria uma nova unidade geradora
    /// </summary>
    /// <param name="dto">Dados da nova unidade geradora</param>
    /// <returns>Unidade geradora criada</returns>
    /// <response code="201">Unidade geradora criada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    [ProducesResponseType(typeof(UnidadeGeradoraDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UnidadeGeradoraDto>> Create([FromBody] CreateUnidadeGeradoraDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var unidade = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = unidade.Id }, unidade);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar unidade geradora");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar unidade geradora");
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Atualiza uma unidade geradora existente
    /// </summary>
    /// <param name="id">ID da unidade geradora</param>
    /// <param name="dto">Dados atualizados</param>
    /// <returns>Unidade geradora atualizada</returns>
    /// <response code="200">Unidade geradora atualizada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="404">Unidade geradora não encontrada</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UnidadeGeradoraDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UnidadeGeradoraDto>> Update(int id, [FromBody] UpdateUnidadeGeradoraDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var unidade = await _service.UpdateAsync(id, dto);
            return Ok(unidade);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Unidade geradora {Id} não encontrada para atualização", id);
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar unidade geradora {Id}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar unidade geradora {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Remove uma unidade geradora (soft delete)
    /// </summary>
    /// <param name="id">ID da unidade geradora</param>
    /// <returns>Confirmação de remoção</returns>
    /// <response code="204">Unidade geradora removida com sucesso</response>
    /// <response code="404">Unidade geradora não encontrada</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
            {
                return NotFound($"Unidade geradora com ID {id} não encontrada");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover unidade geradora {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }
}
