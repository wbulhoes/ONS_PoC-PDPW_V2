using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Interface do repositório de Arquivo DADGER
/// </summary>
public interface IArquivoDadgerRepository
{
    Task<IEnumerable<ArquivoDadger>> GetAllAsync();
    Task<ArquivoDadger?> GetByIdAsync(int id);
    Task<ArquivoDadger> AddAsync(ArquivoDadger arquivo);
    Task UpdateAsync(ArquivoDadger arquivo);
    Task DeleteAsync(int id);
    Task<IEnumerable<ArquivoDadger>> GetBySemanaPMOAsync(int semanaPMOId);
    Task<IEnumerable<ArquivoDadger>> GetProcessadosAsync(bool processado);
    Task<IEnumerable<ArquivoDadger>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<ArquivoDadger?> GetByNomeArquivoAsync(string nomeArquivo);
}
