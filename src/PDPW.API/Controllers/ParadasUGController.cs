using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.ParadaUG;
using PDPW.Application.Interfaces;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller para gerenciamento de Paradas de Unidades Geradoras
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ParadasUGController : ControllerBase
{
    private readonly IParadaUGService _service;
    private readonly ILogger<ParadasUGController> _logger;

    public ParadasUGController(
        IParadaUGService service,
        ILogger<ParadasUGController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Lista todas as paradas de unidades geradoras
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ParadaUGDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ParadaUGDto>>> GetAll()
    {
        try
        {
            var paradas = await _service.GetAllAsync();
            return Ok(paradas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar todas as paradas de UG");
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Busca uma parada por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ParadaUGDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ParadaUGDto>> GetById(int id)
    {
        try
        {
            var parada = await _service.GetByIdAsync(id);
            if (parada == null)
            {
                return NotFound($"Parada com ID {id} não encontrada");
            }

            return Ok(parada);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar parada com ID {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Lista paradas de uma unidade geradora
    /// </summary>
    [HttpGet("unidade/{unidadeGeradoraId}")]
    [ProducesResponseType(typeof(IEnumerable<ParadaUGDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ParadaUGDto>>> GetByUnidadeGeradora(int unidadeGeradoraId)
    {
        try
        {
            var paradas = await _service.GetByUnidadeGeradoraAsync(unidadeGeradoraId);
            return Ok(paradas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar paradas da unidade {UnidadeId}", unidadeGeradoraId);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Lista paradas programadas
    /// </summary>
    [HttpGet("programadas")]
    [ProducesResponseType(typeof(IEnumerable<ParadaUGDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ParadaUGDto>>> GetProgramadas()
    {
        try
        {
            var paradas = await _service.GetProgramadasAsync();
            return Ok(paradas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar paradas programadas");
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Lista paradas não programadas (emergenciais)
    /// </summary>
    [HttpGet("nao-programadas")]
    [ProducesResponseType(typeof(IEnumerable<ParadaUGDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ParadaUGDto>>> GetNaoProgramadas()
    {
        try
        {
            var paradas = await _service.GetNaoProgramadasAsync();
            return Ok(paradas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar paradas não programadas");
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Lista paradas ativas em uma data específica
    /// </summary>
    [HttpGet("ativas")]
    [ProducesResponseType(typeof(IEnumerable<ParadaUGDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ParadaUGDto>>> GetAtivas([FromQuery] DateTime dataReferencia)
    {
        try
        {
            var paradas = await _service.GetAtivasAsync(dataReferencia);
            return Ok(paradas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar paradas ativas na data {Data}", dataReferencia);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Lista paradas em um período
    /// </summary>
    [HttpGet("periodo")]
    [ProducesResponseType(typeof(IEnumerable<ParadaUGDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ParadaUGDto>>> GetByPeriodo(
        [FromQuery] DateTime dataInicio,
        [FromQuery] DateTime dataFim)
    {
        try
        {
            var paradas = await _service.GetByPeriodoAsync(dataInicio, dataFim);
            return Ok(paradas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar paradas no período {Inicio} a {Fim}", dataInicio, dataFim);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Cria uma nova parada de unidade geradora
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ParadaUGDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ParadaUGDto>> Create([FromBody] CreateParadaUGDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parada = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = parada.Id }, parada);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar parada");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar parada");
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Atualiza uma parada existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ParadaUGDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ParadaUGDto>> Update(int id, [FromBody] UpdateParadaUGDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parada = await _service.UpdateAsync(id, dto);
            return Ok(parada);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Parada {Id} não encontrada para atualização", id);
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar parada {Id}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar parada {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Remove uma parada (soft delete)
    /// </summary>
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
                return NotFound($"Parada com ID {id} não encontrada");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover parada {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }
}
