using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.PrevisaoEolica;
using PDPW.Application.Interfaces;
using PDPW.API.Extensions;

namespace PDPW.API.Controllers;

[ApiController]
[Route("api/previsoes-eolicas")]
[Produces("application/json")]
public class PrevisoesEolicasController : BaseController
{
    private readonly IPrevisaoEolicaService _service;
    private readonly ILogger<PrevisoesEolicasController> _logger;

    public PrevisoesEolicasController(
        IPrevisaoEolicaService service,
        ILogger<PrevisoesEolicasController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todas as previsões eólicas
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PrevisaoEolicaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GET api/previsoes-eolicas - Buscando todas as previsões");
        var result = await _service.GetAllAsync();
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém uma previsão por ID
    /// </summary>
    [HttpGet("{id:int}", Name = "GetPrevisaoEolicaById")]
    [ProducesResponseType(typeof(PrevisaoEolicaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPrevisaoById(int id)
    {
        _logger.LogInformation("GET api/previsoes-eolicas/{Id}", id);
        var result = await _service.GetByIdAsync(id);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém previsões por usina
    /// </summary>
    [HttpGet("usina/{usinaId:int}")]
    [ProducesResponseType(typeof(IEnumerable<PrevisaoEolicaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUsina(int usinaId)
    {
        _logger.LogInformation("GET api/previsoes-eolicas/usina/{UsinaId}", usinaId);
        var result = await _service.GetByUsinaAsync(usinaId);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém últimas previsões de uma usina
    /// </summary>
    [HttpGet("usina/{usinaId:int}/ultimas")]
    [ProducesResponseType(typeof(IEnumerable<PrevisaoEolicaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUltimas(int usinaId, [FromQuery] int quantidade = 10)
    {
        _logger.LogInformation("GET api/previsoes-eolicas/usina/{UsinaId}/ultimas?quantidade={Quantidade}", 
            usinaId, quantidade);
        var result = await _service.GetUltimasPrevisoes(usinaId, quantidade);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém previsões por período
    /// </summary>
    [HttpGet("periodo")]
    [ProducesResponseType(typeof(IEnumerable<PrevisaoEolicaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPeriodo(
        [FromQuery] DateTime dataInicio, 
        [FromQuery] DateTime dataFim)
    {
        _logger.LogInformation("GET api/previsoes-eolicas/periodo - {DataInicio} a {DataFim}", 
            dataInicio, dataFim);
        var result = await _service.GetByPeriodoAsync(dataInicio, dataFim);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém previsões por modelo
    /// </summary>
    [HttpGet("modelo/{modelo}")]
    [ProducesResponseType(typeof(IEnumerable<PrevisaoEolicaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByModelo(string modelo)
    {
        _logger.LogInformation("GET api/previsoes-eolicas/modelo/{Modelo}", modelo);
        var result = await _service.GetByModeloAsync(modelo);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém estatísticas de acurácia das previsões
    /// </summary>
    [HttpGet("usina/{usinaId:int}/estatisticas")]
    [ProducesResponseType(typeof(EstatisticasPrevisaoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEstatisticas(
        int usinaId,
        [FromQuery] DateTime dataInicio,
        [FromQuery] DateTime dataFim)
    {
        _logger.LogInformation("GET api/previsoes-eolicas/usina/{UsinaId}/estatisticas", usinaId);
        var result = await _service.GetEstatisticasAsync(usinaId, dataInicio, dataFim);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Cria uma nova previsão eólica
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PrevisaoEolicaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePrevisaoEolicaDto createDto)
    {
        _logger.LogInformation("POST api/previsoes-eolicas - Usina {UsinaId}", createDto.UsinaId);
        var result = await _service.CreateAsync(createDto);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Previsão {Id} criada - Usina {UsinaId}, Modelo {Modelo}", 
                result.Value!.Id, result.Value.UsinaId, result.Value.ModeloPrevisao);
        }
        
        return result.ToCreatedAtActionResult(this, nameof(GetPrevisaoById), new { id = result.Value?.Id });
    }

    /// <summary>
    /// Atualiza uma previsão eólica
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PrevisaoEolicaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePrevisaoEolicaDto updateDto)
    {
        _logger.LogInformation("PUT api/previsoes-eolicas/{Id}", id);
        var result = await _service.UpdateAsync(id, updateDto);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Previsão {Id} atualizada", id);
        }
        
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Atualiza a geração real verificada (para cálculo de erro)
    /// </summary>
    [HttpPatch("{id:int}/geracao-real")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarGeracaoReal(
        int id, 
        [FromBody] AtualizarGeracaoRealDto dto)
    {
        _logger.LogInformation("PATCH api/previsoes-eolicas/{Id}/geracao-real - {GeracaoReal} MWmed", 
            id, dto.GeracaoRealMWmed);
        
        var result = await _service.AtualizarGeracaoRealAsync(id, dto);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Geração real atualizada para previsão {Id}", id);
        }
        
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Remove uma previsão eólica
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("DELETE api/previsoes-eolicas/{Id}", id);
        var result = await _service.DeleteAsync(id);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Previsão {Id} removida", id);
        }
        
        return result.ToActionResult(this);
    }
}
