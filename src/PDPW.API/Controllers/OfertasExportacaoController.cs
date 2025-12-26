using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.OfertaExportacao;
using PDPW.Application.Interfaces;
using PDPW.API.Extensions;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller para gerenciamento de Ofertas de Exportação de Energia Térmica
/// </summary>
[ApiController]
[Route("api/ofertas-exportacao")]
[Produces("application/json")]
public class OfertasExportacaoController : BaseController
{
    private readonly IOfertaExportacaoService _service;
    private readonly ILogger<OfertasExportacaoController> _logger;

    public OfertasExportacaoController(
        IOfertaExportacaoService service, 
        ILogger<OfertasExportacaoController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Obtém todas as ofertas de exportação
    /// </summary>
    /// <returns>Lista de ofertas de exportação</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OfertaExportacaoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém oferta de exportação por ID
    /// </summary>
    /// <param name="id">ID da oferta</param>
    /// <returns>Dados da oferta de exportação</returns>
    /// <response code="200">Oferta encontrada</response>
    /// <response code="404">Oferta não encontrada</response>
    [HttpGet("{id:int}", Name = nameof(GetOfertaById))]
    [ProducesResponseType(typeof(OfertaExportacaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOfertaById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém ofertas de exportação pendentes de análise do ONS
    /// </summary>
    /// <returns>Lista de ofertas pendentes</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("pendentes")]
    [ProducesResponseType(typeof(IEnumerable<OfertaExportacaoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPendentes()
    {
        var result = await _service.GetPendentesAsync();
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém ofertas de exportação aprovadas pelo ONS
    /// </summary>
    /// <returns>Lista de ofertas aprovadas</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("aprovadas")]
    [ProducesResponseType(typeof(IEnumerable<OfertaExportacaoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAprovadas()
    {
        var result = await _service.GetAprovadasAsync();
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém ofertas de exportação rejeitadas pelo ONS
    /// </summary>
    /// <returns>Lista de ofertas rejeitadas</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("rejeitadas")]
    [ProducesResponseType(typeof(IEnumerable<OfertaExportacaoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRejeitadas()
    {
        var result = await _service.GetRejeitadasAsync();
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém ofertas de exportação por usina
    /// </summary>
    /// <param name="usinaId">ID da usina</param>
    /// <returns>Lista de ofertas da usina</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    /// <response code="400">Usina não encontrada</response>
    [HttpGet("usina/{usinaId:int}")]
    [ProducesResponseType(typeof(IEnumerable<OfertaExportacaoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByUsina(int usinaId)
    {
        var result = await _service.GetByUsinaAsync(usinaId);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém ofertas de exportação por data PDP
    /// </summary>
    /// <param name="dataPDP">Data do Programa Diário de Produção (formato: yyyy-MM-dd)</param>
    /// <returns>Lista de ofertas para a data especificada</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    [HttpGet("data-pdp/{dataPDP:datetime}")]
    [ProducesResponseType(typeof(IEnumerable<OfertaExportacaoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByDataPDP(DateTime dataPDP)
    {
        var result = await _service.GetByDataPDPAsync(dataPDP);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém ofertas de exportação por período
    /// </summary>
    /// <param name="dataInicio">Data inicial do período</param>
    /// <param name="dataFim">Data final do período</param>
    /// <returns>Lista de ofertas no período</returns>
    /// <response code="200">Lista retornada com sucesso</response>
    /// <response code="400">Período inválido</response>
    [HttpGet("periodo")]
    [ProducesResponseType(typeof(IEnumerable<OfertaExportacaoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByPeriodo(
        [FromQuery] DateTime dataInicio, 
        [FromQuery] DateTime dataFim)
    {
        var result = await _service.GetByPeriodoAsync(dataInicio, dataFim);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Cria nova oferta de exportação
    /// </summary>
    /// <param name="createDto">Dados da oferta</param>
    /// <returns>Oferta criada</returns>
    /// <response code="201">Oferta criada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    [ProducesResponseType(typeof(OfertaExportacaoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOfertaExportacaoDto createDto)
    {
        var result = await _service.CreateAsync(createDto);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation(
                "Oferta de Exportação {OfertaId} criada com sucesso para Usina {UsinaId} - Data PDP: {DataPDP}", 
                result.Value!.Id, 
                result.Value.UsinaId, 
                result.Value.DataPDP);
        }
        
        return result.ToCreatedAtActionResult(this, nameof(GetOfertaById), new { id = result.Value?.Id });
    }

    /// <summary>
    /// Atualiza oferta de exportação existente
    /// </summary>
    /// <param name="id">ID da oferta</param>
    /// <param name="updateDto">Dados atualizados</param>
    /// <returns>Oferta atualizada</returns>
    /// <response code="200">Oferta atualizada com sucesso</response>
    /// <response code="404">Oferta não encontrada</response>
    /// <response code="400">Dados inválidos ou oferta já analisada</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(OfertaExportacaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOfertaExportacaoDto updateDto)
    {
        var result = await _service.UpdateAsync(id, updateDto);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Oferta de Exportação {OfertaId} atualizada com sucesso", id);
        }
        
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Remove oferta de exportação
    /// </summary>
    /// <param name="id">ID da oferta</param>
    /// <returns>Confirmação de remoção</returns>
    /// <response code="204">Oferta removida com sucesso</response>
    /// <response code="404">Oferta não encontrada</response>
    /// <response code="400">Oferta já analisada ou data PDP inválida</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation("Oferta de Exportação {OfertaId} removida com sucesso", id);
        }
        
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Aprova oferta de exportação (Análise do ONS)
    /// </summary>
    /// <param name="id">ID da oferta</param>
    /// <param name="aprovarDto">Dados da aprovação</param>
    /// <returns>Confirmação da aprovação</returns>
    /// <response code="200">Oferta aprovada com sucesso</response>
    /// <response code="404">Oferta não encontrada</response>
    /// <response code="400">Oferta já foi analisada</response>
    [HttpPost("{id:int}/aprovar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Aprovar(int id, [FromBody] AprovarOfertaExportacaoDto aprovarDto)
    {
        var result = await _service.AprovarAsync(id, aprovarDto);
        
        if (result.IsSuccess)
        {
            _logger.LogInformation(
                "Oferta de Exportação {OfertaId} APROVADA pelo ONS - Usuário: {Usuario}", 
                id, 
                aprovarDto.UsuarioONS);
        }
        
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Rejeita oferta de exportação (Análise do ONS)
    /// </summary>
    /// <param name="id">ID da oferta</param>
    /// <param name="rejeitarDto">Dados da rejeição</param>
    /// <returns>Confirmação da rejeição</returns>
    /// <response code="200">Oferta rejeitada com sucesso</response>
    /// <response code="404">Oferta não encontrada</response>
    /// <response code="400">Oferta já foi analisada ou observação obrigatória</response>
    [HttpPost("{id:int}/rejeitar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Rejeitar(int id, [FromBody] RejeitarOfertaExportacaoDto rejeitarDto)
    {
        var result = await _service.RejeitarAsync(id, rejeitarDto);
        
        if (result.IsSuccess)
        {
            _logger.LogWarning(
                "Oferta de Exportação {OfertaId} REJEITADA pelo ONS - Usuário: {Usuario} - Motivo: {Motivo}", 
                id, 
                rejeitarDto.UsuarioONS,
                rejeitarDto.Observacao);
        }
        
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Verifica se existe oferta de exportação pendente para uma data PDP
    /// </summary>
    /// <param name="dataPDP">Data do PDP</param>
    /// <returns>True se existe oferta pendente, False caso contrário</returns>
    /// <response code="200">Verificação realizada</response>
    [HttpGet("validar-pendente/{dataPDP:datetime}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> ValidarPendente(DateTime dataPDP)
    {
        var existe = await _service.ExisteOfertaPendenteAsync(dataPDP);
        return Ok(new { existeOfertaPendente = existe });
    }

    /// <summary>
    /// Verifica se permite exclusão de ofertas para uma data PDP (data >= D+1)
    /// </summary>
    /// <param name="dataPDP">Data do PDP</param>
    /// <returns>True se permite exclusão, False caso contrário</returns>
    /// <response code="200">Verificação realizada</response>
    [HttpGet("permite-exclusao/{dataPDP:datetime}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> PermiteExclusao(DateTime dataPDP)
    {
        var permite = await _service.PermiteExclusaoAsync(dataPDP);
        return Ok(new { permiteExclusao = permite });
    }
}
