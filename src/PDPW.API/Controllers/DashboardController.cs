using Microsoft.AspNetCore.Mvc;
using PDPW.Application.DTOs.Dashboard;
using PDPW.Application.Interfaces;
using PDPW.API.Extensions;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller para Dashboard e métricas do sistema
/// </summary>
[ApiController]
[Route("api/dashboard")]
[Produces("application/json")]
public class DashboardController : BaseController
{
    private readonly IDashboardService _service;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(
        IDashboardService service,
        ILogger<DashboardController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Obtém resumo geral do sistema para dashboard
    /// </summary>
    [HttpGet("resumo")]
    [ProducesResponseType(typeof(DashboardResumoDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetResumoGeral()
    {
        _logger.LogInformation("GET api/dashboard/resumo - Dashboard principal");
        var result = await _service.GetResumoGeralAsync();
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém métricas por categoria
    /// </summary>
    /// <param name="categoria">Categoria (ofertas, programacao, previsoes)</param>
    [HttpGet("metricas/{categoria}")]
    [ProducesResponseType(typeof(IEnumerable<MetricaCategoriaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMetricasPorCategoria(string categoria)
    {
        _logger.LogInformation("GET api/dashboard/metricas/{Categoria}", categoria);
        var result = await _service.GetMetricasPorCategoriaAsync(categoria);
        return result.ToActionResult(this);
    }

    /// <summary>
    /// Obtém alertas do sistema
    /// </summary>
    /// <param name="prioridade">Filtro de prioridade (Alta, Media, Baixa)</param>
    [HttpGet("alertas")]
    [ProducesResponseType(typeof(IEnumerable<AlertaSistemaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAlertas([FromQuery] string? prioridade = null)
    {
        _logger.LogInformation("GET api/dashboard/alertas - Prioridade: {Prioridade}", 
            prioridade ?? "Todas");
        var result = await _service.GetAlertasAsync(prioridade);
        return result.ToActionResult(this);
    }
}
