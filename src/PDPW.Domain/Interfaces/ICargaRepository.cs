using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Interface do repositório de Carga
/// </summary>
public interface ICargaRepository
{
    Task<IEnumerable<Carga>> GetAllAsync();
    Task<Carga?> GetByIdAsync(int id);
    Task<Carga> AddAsync(Carga carga);
    Task UpdateAsync(Carga carga);
    Task DeleteAsync(int id);
    Task<IEnumerable<Carga>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<IEnumerable<Carga>> GetBySubsistemaAsync(string subsistemaId);
    Task<IEnumerable<Carga>> GetByDataReferenciaAsync(DateTime dataReferencia);
}
