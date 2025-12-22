using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Interface de repositório para Paradas de Unidades Geradoras
/// </summary>
public interface IParadaUGRepository
{
    Task<IEnumerable<ParadaUG>> GetAllAsync();
    Task<IEnumerable<ParadaUG>> GetAllWithUnidadeAsync();
    Task<ParadaUG?> GetByIdAsync(int id);
    Task<ParadaUG?> GetByIdWithUnidadeAsync(int id);
    Task<IEnumerable<ParadaUG>> GetByUnidadeGeradoraIdAsync(int unidadeGeradoraId);
    Task<IEnumerable<ParadaUG>> GetProgramadasAsync();
    Task<IEnumerable<ParadaUG>> GetNaoProgramadasAsync();
    Task<IEnumerable<ParadaUG>> GetAtivasAsync(DateTime dataReferencia);
    Task<IEnumerable<ParadaUG>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task AddAsync(ParadaUG parada);
    Task UpdateAsync(ParadaUG parada);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
