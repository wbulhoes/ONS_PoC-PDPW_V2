using PDPW.Application.DTOs.Dashboard;
using PDPW.Domain.Common;

namespace PDPW.Application.Interfaces;

public interface IDashboardService
{
    Task<Result<DashboardResumoDto>> GetResumoGeralAsync();
    Task<Result<IEnumerable<MetricaCategoriaDto>>> GetMetricasPorCategoriaAsync(string categoria);
    Task<Result<IEnumerable<AlertaSistemaDto>>> GetAlertasAsync(string? prioridade = null);
}
