using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Repository para PrevisaoEolica
/// </summary>
public interface IPrevisaoEolicaRepository
{
    Task<IEnumerable<PrevisaoEolica>> GetAllAsync();
    Task<PrevisaoEolica?> GetByIdAsync(int id);
    Task<IEnumerable<PrevisaoEolica>> GetByUsinaAsync(int usinaId);
    Task<IEnumerable<PrevisaoEolica>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<IEnumerable<PrevisaoEolica>> GetByModeloAsync(string modelo);
    Task<IEnumerable<PrevisaoEolica>> GetByTipoPrevisaoAsync(string tipoPrevisao);
    Task<IEnumerable<PrevisaoEolica>> GetUltimasPrevisoes(int usinaId, int quantidade = 10);
    Task<PrevisaoEolica?> GetPrevisaoMaisRecenteAsync(int usinaId);
    Task<IEnumerable<PrevisaoEolica>> GetPrevisoesComErroAsync(int usinaId);
    Task<decimal> CalcularMAE(int usinaId, DateTime dataInicio, DateTime dataFim); // Mean Absolute Error
    Task<decimal> CalcularRMSE(int usinaId, DateTime dataInicio, DateTime dataFim); // Root Mean Square Error
    Task<PrevisaoEolica> AddAsync(PrevisaoEolica previsao);
    Task UpdateAsync(PrevisaoEolica previsao);
    Task DeleteAsync(int id);
    Task AtualizarGeracaoRealAsync(int id, decimal geracaoReal);
    Task<bool> ExistsAsync(int id);
}
