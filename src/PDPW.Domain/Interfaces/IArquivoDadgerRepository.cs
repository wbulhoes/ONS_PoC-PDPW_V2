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
    
    /// <summary>
    /// Obtém arquivos por status
    /// </summary>
    Task<IEnumerable<ArquivoDadger>> GetByStatusAsync(string status);
    
    /// <summary>
    /// Obtém arquivos pendentes de aprovação (status = EmAnalise)
    /// </summary>
    Task<IEnumerable<ArquivoDadger>> GetPendentesAprovacaoAsync();
    
    /// <summary>
    /// Finaliza programação
    /// </summary>
    Task FinalizarAsync(int id, string usuario, string? observacao = null);
    
    /// <summary>
    /// Aprova programação
    /// </summary>
    Task AprovarAsync(int id, string usuario, string? observacao = null);
    
    /// <summary>
    /// Reabre programação
    /// </summary>
    Task ReabrirAsync(int id, string usuario, string? observacao = null);
    
    /// <summary>
    /// Verifica se existe arquivo
    /// </summary>
    Task<bool> ExistsAsync(int id);
}
