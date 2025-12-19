using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.ArquivoDadger;
using PDPW.Application.Interfaces;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller para gerenciamento de Arquivos DADGER
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ArquivosDadgerController : BaseController
{
    private readonly IArquivoDadgerService _service;
    private readonly ILogger<ArquivosDadgerController> _logger;

    public ArquivosDadgerController(
        IArquivoDadgerService service,
        ILogger<ArquivosDadgerController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todos os arquivos DADGER
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ArquivoDadgerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("GET api/arquivosdadger - Buscando todos os arquivos DADGER");
        var arquivos = await _service.GetAllAsync();
        return Ok(arquivos);
    }

    /// <summary>
    /// Obtém um arquivo DADGER por ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ArquivoDadgerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GET api/arquivosdadger/{Id} - Buscando arquivo DADGER por ID", id);
        var arquivo = await _service.GetByIdAsync(id);

        if (arquivo == null)
            return NotFound(new { message = $"Arquivo DADGER com ID {id} não encontrado" });

        return Ok(arquivo);
    }

    /// <summary>
    /// Obtém arquivos DADGER por semana PMO
    /// </summary>
    [HttpGet("semana/{semanaPMOId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ArquivoDadgerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBySemanaPMO(int semanaPMOId)
    {
        _logger.LogInformation("GET api/arquivosdadger/semana/{SemanaPMOId} - Buscando arquivos por semana PMO", semanaPMOId);
        var arquivos = await _service.GetBySemanaPMOAsync(semanaPMOId);
        return Ok(arquivos);
    }

    /// <summary>
    /// Obtém arquivos DADGER por status de processamento
    /// </summary>
    [HttpGet("processados")]
    [ProducesResponseType(typeof(IEnumerable<ArquivoDadgerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProcessados([FromQuery] bool processado = true)
    {
        _logger.LogInformation("GET api/arquivosdadger/processados?processado={Processado} - Buscando arquivos por status", processado);
        var arquivos = await _service.GetProcessadosAsync(processado);
        return Ok(arquivos);
    }

    /// <summary>
    /// Obtém arquivos DADGER por período
    /// </summary>
    [HttpGet("periodo")]
    [ProducesResponseType(typeof(IEnumerable<ArquivoDadgerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPeriodo([FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim)
    {
        _logger.LogInformation("GET api/arquivosdadger/periodo - Buscando arquivos entre {DataInicio} e {DataFim}", dataInicio, dataFim);
        var arquivos = await _service.GetByPeriodoAsync(dataInicio, dataFim);
        return Ok(arquivos);
    }

    /// <summary>
    /// Obtém arquivo DADGER por nome
    /// </summary>
    [HttpGet("nome/{nomeArquivo}")]
    [ProducesResponseType(typeof(ArquivoDadgerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByNomeArquivo(string nomeArquivo)
    {
        _logger.LogInformation("GET api/arquivosdadger/nome/{NomeArquivo} - Buscando arquivo por nome", nomeArquivo);
        var arquivo = await _service.GetByNomeArquivoAsync(nomeArquivo);

        if (arquivo == null)
            return NotFound(new { message = $"Arquivo DADGER '{nomeArquivo}' não encontrado" });

        return Ok(arquivo);
    }

    /// <summary>
    /// Cria um novo arquivo DADGER
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ArquivoDadgerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateArquivoDadgerDto dto)
    {
        _logger.LogInformation("POST api/arquivosdadger - Criando novo arquivo DADGER: {NomeArquivo}", dto.NomeArquivo);

        try
        {
            var arquivo = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = arquivo.Id }, arquivo);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar arquivo DADGER");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza um arquivo DADGER existente
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ArquivoDadgerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateArquivoDadgerDto dto)
    {
        _logger.LogInformation("PUT api/arquivosdadger/{Id} - Atualizando arquivo DADGER", id);

        try
        {
            var arquivo = await _service.UpdateAsync(id, dto);
            return Ok(arquivo);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Arquivo DADGER com ID {id} não encontrado" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar arquivo DADGER");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Marca um arquivo DADGER como processado
    /// </summary>
    [HttpPatch("{id:int}/processar")]
    [ProducesResponseType(typeof(ArquivoDadgerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarcarComoProcessado(int id)
    {
        _logger.LogInformation("PATCH api/arquivosdadger/{Id}/processar - Marcando arquivo como processado", id);

        try
        {
            var arquivo = await _service.MarcarComoProcessadoAsync(id);
            return Ok(arquivo);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Arquivo DADGER com ID {id} não encontrado" });
        }
    }

    /// <summary>
    /// Remove um arquivo DADGER (soft delete)
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("DELETE api/arquivosdadger/{Id} - Removendo arquivo DADGER", id);
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = $"Arquivo DADGER com ID {id} não encontrado" });

        return NoContent();
    }
}
