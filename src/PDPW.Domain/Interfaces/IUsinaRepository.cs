using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Interface para repositório de Usinas
/// </summary>
public interface IUsinaRepository
{
    /// <summary>
    /// Obtém todas as usinas ativas
    /// </summary>
    Task<IEnumerable<Usina>> GetAllAsync();

    /// <summary>
    /// Obtém usina por ID
    /// </summary>
    Task<Usina?> GetByIdAsync(int id);

    /// <summary>
    /// Obtém usina por código único
    /// </summary>
    Task<Usina?> GetByCodigoAsync(string codigo);

    /// <summary>
    /// Obtém usinas por tipo
    /// </summary>
    Task<IEnumerable<Usina>> GetByTipoAsync(int tipoUsinaId);

    /// <summary>
    /// Obtém usinas por empresa
    /// </summary>
    Task<IEnumerable<Usina>> GetByEmpresaAsync(int empresaId);

    /// <summary>
    /// Obtém usinas com suas unidades geradoras
    /// </summary>
    Task<Usina?> GetByIdWithUnidadesAsync(int id);

    /// <summary>
    /// Adiciona nova usina
    /// </summary>
    Task<Usina> AddAsync(Usina usina);

    /// <summary>
    /// Atualiza usina existente
    /// </summary>
    Task UpdateAsync(Usina usina);

    /// <summary>
    /// Remove usina (soft delete)
    /// </summary>
    Task DeleteAsync(int id);

    /// <summary>
    /// Verifica se código já existe
    /// </summary>
    Task<bool> CodigoExisteAsync(string codigo, int? usinaIdExcluir = null);

    /// <summary>
    /// Verifica se usina existe
    /// </summary>
    Task<bool> ExistsAsync(int id);
}
