using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.Carga;
using PDPW.Application.Interfaces;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller para gerenciamento de Cargas Elétricas
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CargasController : BaseController
{
    private readonly ICargaService _service;
    private readonly ILogger<CargasController> _logger;

    public CargasController(ICargaService service, ILogger<CargasController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todas as cargas
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CargaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GET api/cargas - Buscando todas as cargas");
        var cargas = await _service.GetAllAsync();
        return Ok(cargas);
    }

    /// <summary>
    /// Obtém uma carga por ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CargaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GET api/cargas/{Id} - Buscando carga por ID", id);
        var carga = await _service.GetByIdAsync(id);
        
        if (carga == null)
            return NotFound(new { message = $"Carga com ID {id} não encontrada" });
        
        return Ok(carga);
    }

    /// <summary>
    /// Obtém cargas por período
    /// </summary>
    [HttpGet("periodo")]
    [ProducesResponseType(typeof(IEnumerable<CargaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPeriodo([FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim)
    {
        _logger.LogInformation("GET api/cargas/periodo - Buscando cargas entre {DataInicio} e {DataFim}", dataInicio, dataFim);
        var cargas = await _service.GetByPeriodoAsync(dataInicio, dataFim);
        return Ok(cargas);
    }

    /// <summary>
    /// Obtém cargas por subsistema
    /// </summary>
    [HttpGet("subsistema/{subsistemaId}")]
    [ProducesResponseType(typeof(IEnumerable<CargaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBySubsistema(string subsistemaId)
    {
        _logger.LogInformation("GET api/cargas/subsistema/{SubsistemaId} - Buscando cargas do subsistema", subsistemaId);
        var cargas = await _service.GetBySubsistemaAsync(subsistemaId);
        return Ok(cargas);
    }

    /// <summary>
    /// Obtém cargas por data de referência
    /// </summary>
    [HttpGet("data/{dataReferencia:datetime}")]
    [ProducesResponseType(typeof(IEnumerable<CargaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDataReferencia(DateTime dataReferencia)
    {
        _logger.LogInformation("GET api/cargas/data/{DataReferencia} - Buscando cargas pela data", dataReferencia);
        var cargas = await _service.GetByDataReferenciaAsync(dataReferencia);
        return Ok(cargas);
    }

    /// <summary>
    /// Cria uma nova carga
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CargaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCargaDto dto)
    {
        _logger.LogInformation("POST api/cargas - Criando nova carga");
        var carga = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = carga.Id }, carga);
    }

    /// <summary>
    /// Atualiza uma carga existente
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CargaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCargaDto dto)
    {
        _logger.LogInformation("PUT api/cargas/{Id} - Atualizando carga", id);

        try
        {
            var carga = await _service.UpdateAsync(id, dto);
            return Ok(carga);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Carga com ID {id} não encontrada" });
        }
    }

    /// <summary>
    /// Remove uma carga (soft delete)
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("DELETE api/cargas/{Id} - Removendo carga", id);
        var deleted = await _service.DeleteAsync(id);
        
        if (!deleted)
            return NotFound(new { message = $"Carga com ID {id} não encontrada" });
        
        return NoContent();
    }
}
