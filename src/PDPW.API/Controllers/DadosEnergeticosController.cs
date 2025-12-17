using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs;
using PDPW.Application.Interfaces;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller para gerenciamento de dados energéticos
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DadosEnergeticosController : ControllerBase
{
    private readonly IDadoEnergeticoService _service;
    private readonly ILogger<DadosEnergeticosController> _logger;

    public DadosEnergeticosController(
        IDadoEnergeticoService service,
        ILogger<DadosEnergeticosController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todos os dados energéticos
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DadoEnergeticoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DadoEnergeticoDto>>> ObterTodos()
    {
        var dados = await _service.ObterTodosAsync();
        return Ok(dados);
    }

    /// <summary>
    /// Obtém um dado energético por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DadoEnergeticoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DadoEnergeticoDto>> ObterPorId(int id)
    {
        var dado = await _service.ObterPorIdAsync(id);
        if (dado == null)
            return NotFound();

        return Ok(dado);
    }

    /// <summary>
    /// Obtém dados energéticos por período
    /// </summary>
    [HttpGet("periodo")]
    [ProducesResponseType(typeof(IEnumerable<DadoEnergeticoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DadoEnergeticoDto>>> ObterPorPeriodo(
        [FromQuery] DateTime dataInicio,
        [FromQuery] DateTime dataFim)
    {
        var dados = await _service.ObterPorPeriodoAsync(dataInicio, dataFim);
        return Ok(dados);
    }

    /// <summary>
    /// Cria um novo dado energético
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(DadoEnergeticoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DadoEnergeticoDto>> Criar([FromBody] CriarDadoEnergeticoDto dto)
    {
        try
        {
            var dado = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = dado.Id }, dado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar dado energético");
            return BadRequest(new { message = "Erro ao criar dado energético", error = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza um dado energético existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarDadoEnergeticoDto dto)
    {
        try
        {
            await _service.AtualizarAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar dado energético {Id}", id);
            return BadRequest(new { message = "Erro ao atualizar dado energético", error = ex.Message });
        }
    }

    /// <summary>
    /// Remove um dado energético (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id)
    {
        try
        {
            await _service.RemoverAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover dado energético {Id}", id);
            return BadRequest(new { message = "Erro ao remover dado energético", error = ex.Message });
        }
    }
}
