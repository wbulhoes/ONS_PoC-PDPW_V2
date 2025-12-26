using PDPW.Application.DTOs.PrevisaoEolica;
using PDPW.Domain.Common;

namespace PDPW.Application.Interfaces;

public interface IPrevisaoEolicaService
{
    Task<Result<IEnumerable<PrevisaoEolicaDto>>> GetAllAsync();
    Task<Result<PrevisaoEolicaDto>> GetByIdAsync(int id);
    Task<Result<IEnumerable<PrevisaoEolicaDto>>> GetByUsinaAsync(int usinaId);
    Task<Result<IEnumerable<PrevisaoEolicaDto>>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<Result<IEnumerable<PrevisaoEolicaDto>>> GetByModeloAsync(string modelo);
    Task<Result<IEnumerable<PrevisaoEolicaDto>>> GetUltimasPrevisoes(int usinaId, int quantidade = 10);
    Task<Result<PrevisaoEolicaDto>> CreateAsync(CreatePrevisaoEolicaDto dto);
    Task<Result<PrevisaoEolicaDto>> UpdateAsync(int id, UpdatePrevisaoEolicaDto dto);
    Task<Result> DeleteAsync(int id);
    Task<Result> AtualizarGeracaoRealAsync(int id, AtualizarGeracaoRealDto dto);
    Task<Result<EstatisticasPrevisaoDto>> GetEstatisticasAsync(int usinaId, DateTime dataInicio, DateTime dataFim);
}
