using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.Balanco;
using PDPW.Application.Interfaces;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller para gerenciamento de Balanços Energéticos
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BalancosController : ControllerBase
{
    private readonly IBalancoService _service;
    private readonly ILogger<BalancosController> _logger;

    public BalancosController(
        IBalancoService service,
        ILogger<BalancosController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os balanços energéticos
    /// </summary>
    /// <returns>Lista de balanços energéticos</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BalancoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BalancoDto>>> GetAll()
    {
        try
        {
            var balancos = await _service.GetAllAsync();
            return Ok(balancos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar todos os balanços energéticos");
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Busca um balanço energético por ID
    /// </summary>
    /// <param name="id">ID do balanço</param>
    /// <returns>Balanço energético encontrado</returns>
    /// <response code="200">Balanço encontrado</response>
    /// <response code="404">Balanço não encontrado</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BalancoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BalancoDto>> GetById(int id)
    {
        try
        {
            var balanco = await _service.GetByIdAsync(id);
            if (balanco == null)
            {
                return NotFound($"Balanço energético com ID {id} não encontrado");
            }

            return Ok(balanco);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar balanço energético com ID {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Lista balanços energéticos de um subsistema
    /// </summary>
    /// <param name="subsistemaId">ID do subsistema (SE, S, NE, N)</param>
    /// <returns>Lista de balanços do subsistema</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("subsistema/{subsistemaId}")]
    [ProducesResponseType(typeof(IEnumerable<BalancoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BalancoDto>>> GetBySubsistema(string subsistemaId)
    {
        try
        {
            var balancos = await _service.GetBySubsistemaAsync(subsistemaId);
            return Ok(balancos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar balanços do subsistema {Subsistema}", subsistemaId);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Lista balanços energéticos de uma data específica
    /// </summary>
    /// <param name="data">Data de referência</param>
    /// <returns>Lista de balanços da data</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("data/{data:datetime}")]
    [ProducesResponseType(typeof(IEnumerable<BalancoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BalancoDto>>> GetByData(DateTime data)
    {
        try
        {
            var balancos = await _service.GetByDataAsync(data);
            return Ok(balancos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar balanços da data {Data}", data);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Lista balanços energéticos em um período
    /// </summary>
    /// <param name="dataInicio">Data inicial</param>
    /// <param name="dataFim">Data final</param>
    /// <returns>Lista de balanços do período</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("periodo")]
    [ProducesResponseType(typeof(IEnumerable<BalancoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BalancoDto>>> GetByPeriodo(
        [FromQuery] DateTime dataInicio,
        [FromQuery] DateTime dataFim)
    {
        try
        {
            var balancos = await _service.GetByPeriodoAsync(dataInicio, dataFim);
            return Ok(balancos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar balanços no período {Inicio} a {Fim}", dataInicio, dataFim);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Busca balanço energético de um subsistema em uma data específica
    /// </summary>
    /// <param name="subsistemaId">ID do subsistema</param>
    /// <param name="data">Data de referência</param>
    /// <returns>Balanço encontrado</returns>
    /// <response code="200">Balanço encontrado</response>
    /// <response code="404">Balanço não encontrado</response>
    [HttpGet("subsistema/{subsistemaId}/data/{data:datetime}")]
    [ProducesResponseType(typeof(BalancoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BalancoDto>> GetBySubsistemaData(string subsistemaId, DateTime data)
    {
        try
        {
            var balanco = await _service.GetBySubsistemaDataAsync(subsistemaId, data);
            if (balanco == null)
            {
                return NotFound($"Balanço para subsistema {subsistemaId} na data {data:yyyy-MM-dd} não encontrado");
            }

            return Ok(balanco);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar balanço do subsistema {Subsistema} na data {Data}", 
                subsistemaId, data);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Cria um novo balanço energético
    /// </summary>
    /// <param name="dto">Dados do novo balanço</param>
    /// <returns>Balanço criado</returns>
    /// <response code="201">Balanço criado com sucesso</response>
    /// <response code="400">Dados inválidos ou já existe balanço para o subsistema/data</response>
    [HttpPost]
    [ProducesResponseType(typeof(BalancoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BalancoDto>> Create([FromBody] CreateBalancoDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var balanco = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = balanco.Id }, balanco);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar balanço energético");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar balanço energético");
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Atualiza um balanço energético existente
    /// </summary>
    /// <param name="id">ID do balanço</param>
    /// <param name="dto">Dados atualizados</param>
    /// <returns>Balanço atualizado</returns>
    /// <response code="200">Balanço atualizado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="404">Balanço não encontrado</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(BalancoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BalancoDto>> Update(int id, [FromBody] UpdateBalancoDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var balanco = await _service.UpdateAsync(id, dto);
            return Ok(balanco);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Balanço energético {Id} não encontrado para atualização", id);
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar balanço energético {Id}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar balanço energético {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Remove um balanço energético (soft delete)
    /// </summary>
    /// <param name="id">ID do balanço</param>
    /// <returns>Confirmação de remoção</returns>
    /// <response code="204">Balanço removido com sucesso</response>
    /// <response code="404">Balanço não encontrado</response>
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
                return NotFound($"Balanço energético com ID {id} não encontrado");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover balanço energético {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }
}
