using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Repository para JanelaEnvioAgente
/// </summary>
public interface IJanelaEnvioAgenteRepository
{
    Task<IEnumerable<JanelaEnvioAgente>> GetAllAsync();
    Task<JanelaEnvioAgente?> GetByIdAsync(int id);
    Task<IEnumerable<JanelaEnvioAgente>> GetByTipoDadoAsync(string tipoDado);
    Task<IEnumerable<JanelaEnvioAgente>> GetAbertas();
    Task<JanelaEnvioAgente?> GetJanelaAtualAsync(string tipoDado, DateTime dataReferencia);
    Task<bool> ValidarEnvioPermitidoAsync(string tipoDado, DateTime dataReferencia);
    Task<JanelaEnvioAgente> AddAsync(JanelaEnvioAgente janela);
    Task UpdateAsync(JanelaEnvioAgente janela);
    Task DeleteAsync(int id);
    Task FecharJanelaAsync(int id, string usuario, string? observacao = null);
    Task AbrirJanelaAsync(int id, string usuario, string? observacao = null);
    Task AutorizarExcecaoAsync(int id, string usuario, string observacao);
    Task<bool> ExistsAsync(int id);
}
