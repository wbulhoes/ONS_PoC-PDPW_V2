using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.RestricaoUG;
using PDPW.Application.Interfaces;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller para gerenciamento de Restrições de Unidades Geradoras
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RestricoesUGController : BaseController
{
    private readonly IRestricaoUGService _service;
    private readonly ILogger<RestricoesUGController> _logger;

    public RestricoesUGController(
        IRestricaoUGService service,
        ILogger<RestricoesUGController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todas as restrições de unidades geradoras
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RestricaoUGDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GET api/restricoesug - Buscando todas as restrições UG");
        var restricoes = await _service.GetAllAsync();
        return Ok(restricoes);
    }

    /// <summary>
    /// Obtém uma restrição UG por ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RestricaoUGDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GET api/restricoesug/{Id} - Buscando restrição UG por ID", id);
        var restricao = await _service.GetByIdAsync(id);

        if (restricao == null)
            return NotFound(new { message = $"Restrição UG com ID {id} não encontrada" });

        return Ok(restricao);
    }

    /// <summary>
    /// Obtém restrições por unidade geradora
    /// </summary>
    [HttpGet("unidade/{unidadeGeradoraId:int}")]
    [ProducesResponseType(typeof(IEnumerable<RestricaoUGDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUnidadeGeradora(int unidadeGeradoraId)
    {
        _logger.LogInformation("GET api/restricoesug/unidade/{UnidadeGeradoraId} - Buscando restrições por unidade", unidadeGeradoraId);
        var restricoes = await _service.GetByUnidadeGeradoraAsync(unidadeGeradoraId);
        return Ok(restricoes);
    }

    /// <summary>
    /// Obtém restrições ativas em uma data específica
    /// </summary>
    [HttpGet("ativas")]
    [ProducesResponseType(typeof(IEnumerable<RestricaoUGDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAtivas([FromQuery] DateTime dataReferencia)
    {
        _logger.LogInformation("GET api/restricoesug/ativas?dataReferencia={DataReferencia} - Buscando restrições ativas", dataReferencia);
        var restricoes = await _service.GetAtivasAsync(dataReferencia);
        return Ok(restricoes);
    }

    /// <summary>
    /// Obtém restrições por período
    /// </summary>
    [HttpGet("periodo")]
    [ProducesResponseType(typeof(IEnumerable<RestricaoUGDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPeriodo([FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim)
    {
        _logger.LogInformation("GET api/restricoesug/periodo - Buscando restrições entre {DataInicio} e {DataFim}", dataInicio, dataFim);
        var restricoes = await _service.GetByPeriodoAsync(dataInicio, dataFim);
        return Ok(restricoes);
    }

    /// <summary>
    /// Obtém restrições por motivo
    /// </summary>
    [HttpGet("motivo/{motivoRestricaoId:int}")]
    [ProducesResponseType(typeof(IEnumerable<RestricaoUGDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByMotivoRestricao(int motivoRestricaoId)
    {
        _logger.LogInformation("GET api/restricoesug/motivo/{MotivoRestricaoId} - Buscando restrições por motivo", motivoRestricaoId);
        var restricoes = await _service.GetByMotivoRestricaoAsync(motivoRestricaoId);
        return Ok(restricoes);
    }

    /// <summary>
    /// Cria uma nova restrição UG
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(RestricaoUGDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRestricaoUGDto dto)
    {
        _logger.LogInformation("POST api/restricoesug - Criando nova restrição UG");

        try
        {
            var restricao = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = restricao.Id }, restricao);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar restrição UG");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza uma restrição UG existente
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(RestricaoUGDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRestricaoUGDto dto)
    {
        _logger.LogInformation("PUT api/restricoesug/{Id} - Atualizando restrição UG", id);

        try
        {
            var restricao = await _service.UpdateAsync(id, dto);
            return Ok(restricao);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Restrição UG com ID {id} não encontrada" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar restrição UG");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Remove uma restrição UG (soft delete)
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("DELETE api/restricoesug/{Id} - Removendo restrição UG", id);
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = $"Restrição UG com ID {id} não encontrada" });

        return NoContent();
    }
}
