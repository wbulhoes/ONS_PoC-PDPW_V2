using PDPW.Application.DTOs.ArquivoDadger;
using PDPW.Domain.Common;

namespace PDPW.Application.Interfaces;

/// <summary>
/// Interface do serviço de Arquivo DADGER
/// </summary>
public interface IArquivoDadgerService
{
    Task<Result<IEnumerable<ArquivoDadgerDto>>> GetAllAsync();
    Task<Result<ArquivoDadgerDto>> GetByIdAsync(int id);
    Task<Result<ArquivoDadgerDto>> CreateAsync(CreateArquivoDadgerDto dto);
    Task<Result<ArquivoDadgerDto>> UpdateAsync(int id, UpdateArquivoDadgerDto dto);
    Task<Result> DeleteAsync(int id);
    Task<Result<IEnumerable<ArquivoDadgerDto>>> GetBySemanaPMOAsync(int semanaPMOId);
    Task<Result<IEnumerable<ArquivoDadgerDto>>> GetProcessadosAsync(bool processado);
    Task<Result<IEnumerable<ArquivoDadgerDto>>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<Result<ArquivoDadgerDto>> GetByNomeArquivoAsync(string nomeArquivo);
    Task<Result<ArquivoDadgerDto>> MarcarComoProcessadoAsync(int id);
    
    /// <summary>
    /// Obtém arquivos por status
    /// </summary>
    Task<Result<IEnumerable<ArquivoDadgerDto>>> GetByStatusAsync(string status);
    
    /// <summary>
    /// Obtém arquivos pendentes de aprovação
    /// </summary>
    Task<Result<IEnumerable<ArquivoDadgerDto>>> GetPendentesAprovacaoAsync();
    
    /// <summary>
    /// Finaliza programação (Aberto → EmAnalise)
    /// </summary>
    Task<Result> FinalizarAsync(int id, FinalizarProgramacaoDto dto);
    
    /// <summary>
    /// Aprova programação (EmAnalise → Aprovado)
    /// </summary>
    Task<Result> AprovarAsync(int id, AprovarProgramacaoDto dto);
    
    /// <summary>
    /// Reabre programação (qualquer status → Aberto)
    /// </summary>
    Task<Result> ReabrirAsync(int id, ReabrirProgramacaoDto dto);
}
