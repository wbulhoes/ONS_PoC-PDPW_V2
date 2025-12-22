using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Interface de repositório para Unidades Geradoras
/// </summary>
public interface IUnidadeGeradoraRepository
{
    Task<IEnumerable<UnidadeGeradora>> GetAllAsync();
    Task<IEnumerable<UnidadeGeradora>> GetAllWithUsinaAsync();
    Task<UnidadeGeradora?> GetByIdAsync(int id);
    Task<UnidadeGeradora?> GetByIdWithUsinaAsync(int id);
    Task<UnidadeGeradora?> GetByCodigoAsync(string codigo);
    Task<IEnumerable<UnidadeGeradora>> GetByUsinaIdAsync(int usinaId);
    Task<IEnumerable<UnidadeGeradora>> GetByStatusAsync(string status);
    Task<IEnumerable<UnidadeGeradora>> GetAtivasAsync();
    Task AddAsync(UnidadeGeradora unidade);
    Task UpdateAsync(UnidadeGeradora unidade);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> CodigoExistsAsync(string codigo, int? excludeId = null);
}
