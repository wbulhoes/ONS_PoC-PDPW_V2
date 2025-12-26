using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.SemanaPmo;
using PDPW.Application.Interfaces;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller para gerenciamento de Semanas PMO
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SemanasPmoController : BaseController
{
    private readonly ISemanaPmoService _service;
    private readonly ILogger<SemanasPmoController> _logger;

    public SemanasPmoController(
        ISemanaPmoService service,
        ILogger<SemanasPmoController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todas as semanas PMO
    /// </summary>
    /// <returns>Lista de semanas PMO ordenadas por ano e número</returns>
    /// <response code="200">Lista de semanas PMO retornada com sucesso</response>
    [HttpGet(Name = nameof(GetAllSemanasPmo))]
    [ProducesResponseType(typeof(List<SemanaPmoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSemanasPmo()
    {
        _logger.LogInformation("GET api/semanaspmo - Buscando todas as semanas PMO");
        
        var semanas = await _service.GetAllAsync();
        return Ok(semanas);
    }

    /// <summary>
    /// Obtém uma semana PMO por ID
    /// </summary>
    /// <param name="id">ID da semana PMO</param>
    /// <returns>Semana PMO encontrada</returns>
    /// <response code="200">Semana PMO encontrada</response>
    /// <response code="404">Semana PMO não encontrada</response>
    [HttpGet("{id:int}", Name = nameof(GetSemanaPmoById))]
    [ProducesResponseType(typeof(SemanaPmoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSemanaPmoById(int id)
    {
        _logger.LogInformation("GET api/semanaspmo/{Id} - Buscando semana PMO por ID", id);
        
        var semana = await _service.GetByIdAsync(id);
        return HandleResult(semana);
    }

    /// <summary>
    /// Obtém uma semana PMO por número e ano
    /// </summary>
    /// <param name="numero">Número da semana (1-53)</param>
    /// <param name="ano">Ano de referência</param>
    /// <returns>Semana PMO encontrada</returns>
    /// <response code="200">Semana PMO encontrada</response>
    /// <response code="404">Semana PMO não encontrada</response>
    [HttpGet("numero/{numero}/ano/{ano}", Name = nameof(GetSemanaPmoByNumeroAno))]
    [ProducesResponseType(typeof(SemanaPmoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSemanaPmoByNumeroAno(int numero, int ano)
    {
        _logger.LogInformation("GET api/semanaspmo/numero/{Numero}/ano/{Ano} - Buscando semana PMO", numero, ano);
        
        var semana = await _service.GetByNumeroAnoAsync(numero, ano);
        return HandleResult(semana);
    }

    /// <summary>
    /// Obtém todas as semanas PMO de um ano específico
    /// </summary>
    /// <param name="ano">Ano de referência</param>
    /// <returns>Lista de semanas PMO do ano</returns>
    /// <response code="200">Lista de semanas PMO retornada com sucesso</response>
    [HttpGet("ano/{ano}", Name = nameof(GetSemanasPmoByAno))]
    [ProducesResponseType(typeof(List<SemanaPmoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSemanasPmoByAno(int ano)
    {
        _logger.LogInformation("GET api/semanaspmo/ano/{Ano} - Buscando semanas PMO por ano", ano);
        
        var semanas = await _service.GetByAnoAsync(ano);
        return Ok(semanas);
    }

    /// <summary>
    /// Obtém a semana PMO que contém uma data específica
    /// </summary>
    /// <param name="data">Data a ser pesquisada (formato: yyyy-MM-dd)</param>
    /// <returns>Semana PMO encontrada</returns>
    /// <response code="200">Semana PMO encontrada</response>
    /// <response code="404">Nenhuma semana PMO encontrada para esta data</response>
    [HttpGet("data/{data}", Name = nameof(GetSemanaPmoByData))]
    [ProducesResponseType(typeof(SemanaPmoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSemanaPmoByData(DateTime data)
    {
        _logger.LogInformation("GET api/semanaspmo/data/{Data} - Buscando semana PMO por data", data);
        
        var semana = await _service.GetByDataAsync(data);
        return HandleResult(semana);
    }

    /// <summary>
    /// Cria uma nova semana PMO
    /// </summary>
    /// <param name="dto">Dados da semana PMO</param>
    /// <returns>Semana PMO criada</returns>
    /// <response code="201">Semana PMO criada com sucesso</response>
    /// <response code="400">Dados inválidos, número/ano duplicado ou conflito de datas</response>
    [HttpPost(Name = nameof(CreateSemanaPmo))]
    [ProducesResponseType(typeof(SemanaPmoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSemanaPmo([FromBody] CreateSemanaPmoDto dto)
    {
        _logger.LogInformation("POST api/semanaspmo - Criando nova semana PMO: Semana {Numero}/{Ano}", dto.Numero, dto.Ano);

        try
        {
            var semana = await _service.CreateAsync(dto);
            return CreatedAtRoute(
                nameof(GetSemanaPmoById),
                new { id = semana.Id },
                semana);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar semana PMO");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza uma semana PMO existente
    /// </summary>
    /// <param name="id">ID da semana PMO</param>
    /// <param name="dto">Dados atualizados</param>
    /// <returns>Semana PMO atualizada</returns>
    /// <response code="200">Semana PMO atualizada com sucesso</response>
    /// <response code="400">Dados inválidos, número/ano duplicado ou conflito de datas</response>
    /// <response code="404">Semana PMO não encontrada</response>
    [HttpPut("{id:int}", Name = nameof(UpdateSemanaPmo))]
    [ProducesResponseType(typeof(SemanaPmoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSemanaPmo(int id, [FromBody] UpdateSemanaPmoDto dto)
    {
        _logger.LogInformation("PUT api/semanaspmo/{Id} - Atualizando semana PMO", id);

        try
        {
            var semana = await _service.UpdateAsync(id, dto);
            return HandleResult(semana);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar semana PMO");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Remove uma semana PMO (soft delete)
    /// </summary>
    /// <param name="id">ID da semana PMO</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Semana PMO removida com sucesso</response>
    /// <response code="400">Não é possível remover semana com arquivos vinculados</response>
    /// <response code="404">Semana PMO não encontrada</response>
    [HttpDelete("{id:int}", Name = nameof(DeleteSemanaPmo))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSemanaPmo(int id)
    {
        _logger.LogInformation("DELETE api/semanaspmo/{Id} - Removendo semana PMO", id);

        try
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound(new { message = $"Semana PMO com ID {id} não encontrada" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro ao tentar remover semana PMO com arquivos vinculados");
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Verifica se já existe uma semana PMO com o número e ano informados
    /// </summary>
    /// <param name="numero">Número da semana</param>
    /// <param name="ano">Ano de referência</param>
    /// <param name="semanaPmoId">ID da semana PMO a excluir da verificação (opcional)</param>
    /// <returns>Indica se já existe</returns>
    /// <response code="200">Resultado da verificação</response>
    [HttpGet("verificar-numero/{numero}/ano/{ano}", Name = nameof(VerificarNumeroAnoExiste))]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> VerificarNumeroAnoExiste(int numero, int ano, [FromQuery] int? semanaPmoId = null)
    {
        _logger.LogInformation("GET api/semanaspmo/verificar-numero/{Numero}/ano/{Ano}", numero, ano);
        
        var existe = await _service.ExisteNumeroAnoAsync(numero, ano, semanaPmoId);
        return Ok(new { existe });
    }

    /// <summary>
    /// Obtém a semana PMO atual (que contém a data de hoje)
    /// </summary>
    /// <returns>Semana PMO atual</returns>
    /// <response code="200">Semana PMO atual encontrada</response>
    /// <response code="404">Nenhuma semana PMO contém a data atual</response>
    [HttpGet("atual", Name = nameof(GetSemanaPmoAtual))]
    [ProducesResponseType(typeof(SemanaPmoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSemanaPmoAtual()
    {
        _logger.LogInformation("GET api/semanaspmo/atual - Buscando semana PMO atual");
        
        var semana = await _service.GetByDataAsync(DateTime.Today);
        return HandleResult(semana);
    }

    /// <summary>
    /// Obtém as próximas N semanas PMO a partir de hoje
    /// </summary>
    /// <param name="quantidade">Quantidade de semanas futuras (padrão: 4)</param>
    /// <returns>Lista de próximas semanas PMO</returns>
    /// <response code="200">Lista de próximas semanas retornada com sucesso</response>
    [HttpGet("proximas", Name = nameof(GetProximasSemanas))]
    [ProducesResponseType(typeof(List<SemanaPmoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProximasSemanas([FromQuery] int quantidade = 4)
    {
        _logger.LogInformation("GET api/semanaspmo/proximas?quantidade={Quantidade} - Buscando próximas semanas", quantidade);
        
        // Buscar todas as semanas e filtrar as próximas
        var todasSemanas = await _service.GetAllAsync();
        var hoje = DateTime.Today;
        
        var proximasSemanas = todasSemanas
            .Where(s => s.DataInicio > hoje)
            .OrderBy(s => s.DataInicio)
            .Take(quantidade)
            .ToList();
        
        return Ok(proximasSemanas);
    }
}
