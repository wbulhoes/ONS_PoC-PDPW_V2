using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Interface de repositório para Motivos de Restrição
/// </summary>
public interface IMotivoRestricaoRepository
{
    Task<IEnumerable<MotivoRestricao>> GetAllAsync();
    Task<MotivoRestricao?> GetByIdAsync(int id);
    Task<IEnumerable<MotivoRestricao>> GetByCategoriaAsync(string categoria);
    Task<IEnumerable<MotivoRestricao>> GetAtivasAsync();
    Task AddAsync(MotivoRestricao motivo);
    Task UpdateAsync(MotivoRestricao motivo);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
