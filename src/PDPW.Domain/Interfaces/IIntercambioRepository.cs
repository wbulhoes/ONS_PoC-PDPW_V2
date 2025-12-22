using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Interface de repositório para Intercâmbios de Energia
/// </summary>
public interface IIntercambioRepository
{
    Task<IEnumerable<Intercambio>> GetAllAsync();
    Task<Intercambio?> GetByIdAsync(int id);
    Task<IEnumerable<Intercambio>> GetBySubsistemaOrigemAsync(string subsistemaOrigem);
    Task<IEnumerable<Intercambio>> GetBySubsistemaDestinoAsync(string subsistemaDestino);
    Task<IEnumerable<Intercambio>> GetByDataAsync(DateTime dataReferencia);
    Task<IEnumerable<Intercambio>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<Intercambio?> GetBySubsistemasDataAsync(string subsistemaOrigem, string subsistemaDestino, DateTime dataReferencia);
    Task AddAsync(Intercambio intercambio);
    Task UpdateAsync(Intercambio intercambio);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
