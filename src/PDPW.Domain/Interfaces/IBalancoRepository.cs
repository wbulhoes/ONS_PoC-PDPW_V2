using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Interface de repositório para Balanços Energéticos
/// </summary>
public interface IBalancoRepository
{
    Task<IEnumerable<Balanco>> GetAllAsync();
    Task<Balanco?> GetByIdAsync(int id);
    Task<IEnumerable<Balanco>> GetBySubsistemaAsync(string subsistemaId);
    Task<IEnumerable<Balanco>> GetByDataAsync(DateTime dataReferencia);
    Task<IEnumerable<Balanco>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<Balanco?> GetBySubsistemaDataAsync(string subsistemaId, DateTime dataReferencia);
    Task AddAsync(Balanco balanco);
    Task UpdateAsync(Balanco balanco);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
