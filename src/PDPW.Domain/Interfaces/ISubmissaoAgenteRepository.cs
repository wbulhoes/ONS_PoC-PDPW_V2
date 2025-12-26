using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Repository para SubmissaoAgente
/// </summary>
public interface ISubmissaoAgenteRepository
{
    Task<IEnumerable<SubmissaoAgente>> GetAllAsync();
    Task<SubmissaoAgente?> GetByIdAsync(int id);
    Task<IEnumerable<SubmissaoAgente>> GetByEmpresaAsync(int empresaId);
    Task<IEnumerable<SubmissaoAgente>> GetByTipoDadoAsync(string tipoDado);
    Task<IEnumerable<SubmissaoAgente>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<IEnumerable<SubmissaoAgente>> GetForaJanelaAsync();
    Task<IEnumerable<SubmissaoAgente>> GetRejeitadasAsync();
    Task<SubmissaoAgente> RegistrarSubmissaoAsync(
        int empresaId,
        string tipoDado,
        int registroId,
        DateTime dataReferencia,
        string usuarioEnvio,
        string? ipOrigem = null,
        bool dentroJanela = true);
    Task RejeitarSubmissaoAsync(int id, string motivo);
    Task AceitarSubmissaoAsync(int id);
    Task<bool> ExisteSubmissaoDuplicadaAsync(string hashDados);
    Task<int> ContarSubmissoesPorEmpresaAsync(int empresaId, DateTime dataInicio, DateTime dataFim);
    Task<bool> ExistsAsync(int id);
}
