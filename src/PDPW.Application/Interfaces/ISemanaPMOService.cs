using PDPW.Application.DTOs.SemanaPmo;

namespace PDPW.Application.Interfaces;

/// <summary>
/// Interface do serviço de Semana PMO
/// </summary>
public interface ISemanaPmoService
{
    /// <summary>
    /// Obtém todas as semanas PMO
    /// </summary>
    Task<IEnumerable<SemanaPmoDto>> GetAllAsync();

    /// <summary>
    /// Obtém semana PMO por ID
    /// </summary>
    Task<SemanaPmoDto?> GetByIdAsync(int id);

    /// <summary>
    /// Obtém semanas de um ano específico
    /// </summary>
    Task<IEnumerable<SemanaPmoDto>> GetByAnoAsync(int ano);

    /// <summary>
    /// Obtém semana PMO por número e ano
    /// </summary>
    Task<SemanaPmoDto?> GetByNumeroAnoAsync(int numero, int ano);

    /// <summary>
    /// Obtém semana PMO que contém uma data
    /// </summary>
    Task<SemanaPmoDto?> GetByDataAsync(DateTime data);

    /// <summary>
    /// Cria uma nova semana PMO
    /// </summary>
    Task<SemanaPmoDto> CreateAsync(CreateSemanaPmoDto dto);

    /// <summary>
    /// Atualiza uma semana PMO existente
    /// </summary>
    Task<SemanaPmoDto?> UpdateAsync(int id, UpdateSemanaPmoDto dto);

    /// <summary>
    /// Remove uma semana PMO (soft delete)
    /// </summary>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Verifica se número e ano já existem
    /// </summary>
    Task<bool> ExisteNumeroAnoAsync(int numero, int ano, int? excluirId = null);
}
