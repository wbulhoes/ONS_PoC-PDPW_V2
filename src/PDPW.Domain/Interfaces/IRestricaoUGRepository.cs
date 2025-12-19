using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Interface do repositório de Restrição de Unidade Geradora
/// </summary>
public interface IRestricaoUGRepository
{
    Task<IEnumerable<RestricaoUG>> GetAllAsync();
    Task<RestricaoUG?> GetByIdAsync(int id);
    Task<RestricaoUG> AddAsync(RestricaoUG restricao);
    Task UpdateAsync(RestricaoUG restricao);
    Task DeleteAsync(int id);
    Task<IEnumerable<RestricaoUG>> GetByUnidadeGeradoraAsync(int unidadeGeradoraId);
    Task<IEnumerable<RestricaoUG>> GetAtivasAsync(DateTime dataReferencia);
    Task<IEnumerable<RestricaoUG>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<IEnumerable<RestricaoUG>> GetByMotivoRestricaoAsync(int motivoRestricaoId);
}
