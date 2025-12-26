using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.ArquivoDadger;
using PDPW.Application.Interfaces;
using PDPW.API.Extensions;

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
        var result = await _service.GetAllAsync();
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém um arquivo DADGER por ID
    /// </summary>
    [HttpGet("{id:int}", Name = "GetArquivoDadgerById")]
    [ProducesResponseType(typeof(ArquivoDadgerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("GET api/arquivosdadger/{Id} - Buscando arquivo DADGER por ID", id);
        var result = await _service.GetByIdAsync(id);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém arquivos DADGER por semana PMO
    /// </summary>
    [HttpGet("semana/{semanaPMOId:int}")]
    [ProducesResponseType(typeof(IEnumerable<ArquivoDadgerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBySemanaPMO(int semanaPMOId)
    {
        _logger.LogInformation("GET api/arquivosdadger/semana/{SemanaPMOId} - Buscando arquivos por semana PMO", semanaPMOId);
        var result = await _service.GetBySemanaPMOAsync(semanaPMOId);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém arquivos DADGER por status de processamento
    /// </summary>
    [HttpGet("processados")]
    [ProducesResponseType(typeof(IEnumerable<ArquivoDadgerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProcessados([FromQuery] bool processado = true)
    {
        _logger.LogInformation("GET api/arquivosdadger/processados?processado={Processado} - Buscando arquivos por status", processado);
        var result = await _service.GetProcessadosAsync(processado);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém arquivos DADGER por período
    /// </summary>
    [HttpGet("periodo")]
    [ProducesResponseType(typeof(IEnumerable<ArquivoDadgerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByPeriodo([FromQuery] DateTime dataInicio, [FromQuery] DateTime dataFim)
    {
        _logger.LogInformation("GET api/arquivosdadger/periodo - Buscando arquivos entre {DataInicio} e {DataFim}", dataInicio, dataFim);
        var result = await _service.GetByPeriodoAsync(dataInicio, dataFim);
        return result.ToActionResult(this);
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
        var result = await _service.GetByNomeArquivoAsync(nomeArquivo);
        return result.ToActionResult(this);
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
        var result = await _service.CreateAsync(dto);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Arquivo DADGER {Id} criado com sucesso", result.Value!.Id);
        }
        
        return result.ToCreatedAtActionResult(this, nameof(GetById), new { id = result.Value?.Id });
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
        var result = await _service.UpdateAsync(id, dto);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Arquivo DADGER {Id} atualizado com sucesso", id);
        }
        
        return result.ToActionResult(this);
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
        var result = await _service.MarcarComoProcessadoAsync(id);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Arquivo DADGER {Id} marcado como processado", id);
        }
        
        return result.ToActionResult(this);
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
        var result = await _service.DeleteAsync(id);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Arquivo DADGER {Id} removido com sucesso", id);
        }
        
        return result.ToActionResult(this);
    }

    // === NOVOS ENDPOINTS DE FINALIZAÇÃO E APROVAÇÃO ===

    /// <summary>
    /// Obtém arquivos DADGER por status
    /// </summary>
    /// <param name="status">Status da programação (Aberto, EmAnalise, Aprovado)</param>
    [HttpGet("status/{status}")]
    [ProducesResponseType(typeof(IEnumerable<ArquivoDadgerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByStatus(string status)
    {
        _logger.LogInformation("GET api/arquivosdadger/status/{Status} - Buscando arquivos por status", status);
        var result = await _service.GetByStatusAsync(status);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém arquivos DADGER pendentes de aprovação (status = EmAnalise)
    /// </summary>
    [HttpGet("pendentes-aprovacao")]
    [ProducesResponseType(typeof(IEnumerable<ArquivoDadgerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPendentesAprovacao()
    {
        _logger.LogInformation("GET api/arquivosdadger/pendentes-aprovacao - Buscando arquivos pendentes");
        var result = await _service.GetPendentesAprovacaoAsync();
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Finaliza uma programação (Aberto → EmAnalise)
    /// </summary>
    /// <param name="id">ID do arquivo DADGER</param>
    /// <param name="dto">Dados da finalização</param>
    [HttpPost("{id:int}/finalizar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Finalizar(int id, [FromBody] FinalizarProgramacaoDto dto)
    {
        _logger.LogInformation("POST api/arquivosdadger/{Id}/finalizar - Finalizando programação - Usuário: {Usuario}", 
            id, dto.Usuario);

        var result = await _service.FinalizarAsync(id, dto);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Programação {Id} FINALIZADA com sucesso por {Usuario}", id, dto.Usuario);
        }

        return result.ToActionResult(this);
    }

    /// <summary>
    /// Aprova uma programação (EmAnalise → Aprovado)
    /// </summary>
    /// <param name="id">ID do arquivo DADGER</param>
    /// <param name="dto">Dados da aprovação</param>
    [HttpPost("{id:int}/aprovar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Aprovar(int id, [FromBody] AprovarProgramacaoDto dto)
    {
        _logger.LogInformation("POST api/arquivosdadger/{Id}/aprovar - Aprovando programação - Usuário: {Usuario}", 
            id, dto.Usuario);

        var result = await _service.AprovarAsync(id, dto);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Programação {Id} APROVADA com sucesso por {Usuario}", id, dto.Usuario);
        }

        return result.ToActionResult(this);
    }

    /// <summary>
    /// Reabre uma programação (qualquer status → Aberto)
    /// </summary>
    /// <param name="id">ID do arquivo DADGER</param>
    /// <param name="dto">Dados da reabertura</param>
    [HttpPost("{id:int}/reabrir")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reabrir(int id, [FromBody] ReabrirProgramacaoDto dto)
    {
        _logger.LogWarning("POST api/arquivosdadger/{Id}/reabrir - Reabrindo programação - Usuário: {Usuario} - Motivo: {Motivo}", 
            id, dto.Usuario, dto.Observacao);

        var result = await _service.ReabrirAsync(id, dto);
        
        if (result.IsSuccess)
        {
            _logger.LogWarning("Programação {Id} REABERTA por {Usuario}", id, dto.Usuario);
        }

        return result.ToActionResult(this);
    }
}
