using PDPW.Application.DTOs.ArquivoDadger;

namespace PDPW.Application.Interfaces;

/// <summary>
/// Interface do serviço de Arquivo DADGER
/// </summary>
public interface IArquivoDadgerService
{
    Task<IEnumerable<ArquivoDadgerDto>> GetAllAsync();
    Task<ArquivoDadgerDto?> GetByIdAsync(int id);
    Task<ArquivoDadgerDto> CreateAsync(CreateArquivoDadgerDto dto);
    Task<ArquivoDadgerDto> UpdateAsync(int id, UpdateArquivoDadgerDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ArquivoDadgerDto>> GetBySemanaPMOAsync(int semanaPMOId);
    Task<IEnumerable<ArquivoDadgerDto>> GetProcessadosAsync(bool processado);
    Task<IEnumerable<ArquivoDadgerDto>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<ArquivoDadgerDto?> GetByNomeArquivoAsync(string nomeArquivo);
    Task<ArquivoDadgerDto> MarcarComoProcessadoAsync(int id);
}
