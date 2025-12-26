using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.Intercambio;
using PDPW.Application.Interfaces;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller para gerenciamento de Intercâmbios de Energia
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class IntercambiosController : ControllerBase
{
    private readonly IIntercambioService _service;
    private readonly ILogger<IntercambiosController> _logger;

    public IntercambiosController(
        IIntercambioService service,
        ILogger<IntercambiosController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Lista todos os intercâmbios de energia
    /// </summary>
    /// <returns>Lista de intercâmbios</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<IntercambioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IntercambioDto>>> GetAll()
    {
        try
        {
            var intercambios = await _service.GetAllAsync();
            return Ok(intercambios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar todos os intercâmbios");
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Busca um intercâmbio por ID
    /// </summary>
    /// <param name="id">ID do intercâmbio</param>
    /// <returns>Intercâmbio encontrado</returns>
    /// <response code="200">Intercâmbio encontrado</response>
    /// <response code="404">Intercâmbio não encontrado</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(IntercambioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IntercambioDto>> GetById(int id)
    {
        try
        {
            var intercambio = await _service.GetByIdAsync(id);
            if (intercambio == null)
            {
                return NotFound($"Intercâmbio com ID {id} não encontrado");
            }

            return Ok(intercambio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar intercâmbio com ID {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Lista intercâmbios com origem em um subsistema
    /// </summary>
    /// <param name="subsistemaOrigem">Subsistema de origem (SE, S, NE, N)</param>
    /// <returns>Lista de intercâmbios de origem</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("origem/{subsistemaOrigem}")]
    [ProducesResponseType(typeof(IEnumerable<IntercambioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IntercambioDto>>> GetByOrigem(string subsistemaOrigem)
    {
        try
        {
            var intercambios = await _service.GetBySubsistemaOrigemAsync(subsistemaOrigem);
            return Ok(intercambios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar intercâmbios de origem {Subsistema}", subsistemaOrigem);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Lista intercâmbios com destino em um subsistema
    /// </summary>
    /// <param name="subsistemaDestino">Subsistema de destino (SE, S, NE, N)</param>
    /// <returns>Lista de intercâmbios de destino</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("destino/{subsistemaDestino}")]
    [ProducesResponseType(typeof(IEnumerable<IntercambioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IntercambioDto>>> GetByDestino(string subsistemaDestino)
    {
        try
        {
            var intercambios = await _service.GetBySubsistemaDestinoAsync(subsistemaDestino);
            return Ok(intercambios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar intercâmbios para destino {Subsistema}", subsistemaDestino);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Lista intercâmbios filtrados por subsistemas de origem e/ou destino
    /// </summary>
    /// <param name="origem">Subsistema de origem (opcional)</param>
    /// <param name="destino">Subsistema de destino (opcional)</param>
    /// <returns>Lista de intercâmbios filtrados</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("subsistema")]
    [ProducesResponseType(typeof(IEnumerable<IntercambioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IntercambioDto>>> GetBySubsistemas(
        [FromQuery] string? origem = null,
        [FromQuery] string? destino = null)
    {
        try
        {
            var intercambios = await _service.GetAllAsync();
            
            if (!string.IsNullOrWhiteSpace(origem))
            {
                intercambios = intercambios.Where(i => 
                    i.SubsistemaOrigem.Equals(origem, StringComparison.OrdinalIgnoreCase));
            }
            
            if (!string.IsNullOrWhiteSpace(destino))
            {
                intercambios = intercambios.Where(i => 
                    i.SubsistemaDestino.Equals(destino, StringComparison.OrdinalIgnoreCase));
            }
            
            return Ok(intercambios.ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar intercâmbios por subsistemas origem={Origem} destino={Destino}", 
                origem, destino);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Lista intercâmbios de uma data específica
    /// </summary>
    /// <param name="data">Data de referência</param>
    /// <returns>Lista de intercâmbios da data</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("data/{data:datetime}")]
    [ProducesResponseType(typeof(IEnumerable<IntercambioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IntercambioDto>>> GetByData(DateTime data)
    {
        try
        {
            var intercambios = await _service.GetByDataAsync(data);
            return Ok(intercambios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar intercâmbios da data {Data}", data);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Lista intercâmbios em um período
    /// </summary>
    /// <param name="dataInicio">Data inicial</param>
    /// <param name="dataFim">Data final</param>
    /// <returns>Lista de intercâmbios do período</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("periodo")]
    [ProducesResponseType(typeof(IEnumerable<IntercambioDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IntercambioDto>>> GetByPeriodo(
        [FromQuery] DateTime dataInicio,
        [FromQuery] DateTime dataFim)
    {
        try
        {
            var intercambios = await _service.GetByPeriodoAsync(dataInicio, dataFim);
            return Ok(intercambios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar intercâmbios no período {Inicio} a {Fim}", dataInicio, dataFim);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Busca intercâmbio específico entre subsistemas em uma data
    /// </summary>
    /// <param name="subsistemaOrigem">Subsistema de origem</param>
    /// <param name="subsistemaDestino">Subsistema de destino</param>
    /// <param name="data">Data de referência</param>
    /// <returns>Intercâmbio encontrado</returns>
    /// <response code="200">Intercâmbio encontrado</response>
    /// <response code="404">Intercâmbio não encontrado</response>
    [HttpGet("origem/{subsistemaOrigem}/destino/{subsistemaDestino}/data/{data:datetime}")]
    [ProducesResponseType(typeof(IntercambioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IntercambioDto>> GetBySubsistemasData(
        string subsistemaOrigem, 
        string subsistemaDestino, 
        DateTime data)
    {
        try
        {
            var intercambio = await _service.GetBySubsistemasDataAsync(subsistemaOrigem, subsistemaDestino, data);
            if (intercambio == null)
            {
                return NotFound($"Intercâmbio de {subsistemaOrigem} para {subsistemaDestino} na data {data:yyyy-MM-dd} não encontrado");
            }

            return Ok(intercambio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar intercâmbio {Origem} -> {Destino} na data {Data}", 
                subsistemaOrigem, subsistemaDestino, data);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Cria um novo intercâmbio de energia
    /// </summary>
    /// <param name="dto">Dados do novo intercâmbio</param>
    /// <returns>Intercâmbio criado</returns>
    /// <response code="201">Intercâmbio criado com sucesso</response>
    /// <response code="400">Dados inválidos ou já existe intercâmbio para os subsistemas/data</response>
    [HttpPost]
    [ProducesResponseType(typeof(IntercambioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IntercambioDto>> Create([FromBody] CreateIntercambioDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var intercambio = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = intercambio.Id }, intercambio);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar intercâmbio");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar intercâmbio");
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Atualiza um intercâmbio existente
    /// </summary>
    /// <param name="id">ID do intercâmbio</param>
    /// <param name="dto">Dados atualizados</param>
    /// <returns>Intercâmbio atualizado</returns>
    /// <response code="200">Intercâmbio atualizado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="404">Intercâmbio não encontrado</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(IntercambioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IntercambioDto>> Update(int id, [FromBody] UpdateIntercambioDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var intercambio = await _service.UpdateAsync(id, dto);
            return Ok(intercambio);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Intercâmbio {Id} não encontrado para atualização", id);
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar intercâmbio {Id}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar intercâmbio {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }

    /// <summary>
    /// Remove um intercâmbio (soft delete)
    /// </summary>
    /// <param name="id">ID do intercâmbio</param>
    /// <returns>Confirmação de remoção</returns>
    /// <response code="204">Intercâmbio removido com sucesso</response>
    /// <response code="404">Intercâmbio não encontrado</response>
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
                return NotFound($"Intercâmbio com ID {id} não encontrado");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover intercâmbio {Id}", id);
            return StatusCode(500, "Erro interno ao processar a solicitação");
        }
    }
}
