using PDPW.Application.DTOs.Usina;

namespace PDPW.Application.Interfaces;

/// <summary>
/// Interface para serviço de Usinas
/// </summary>
public interface IUsinaService
{
    /// <summary>
    /// Obtém todas as usinas
    /// </summary>
    Task<IEnumerable<UsinaDto>> GetAllAsync();

    /// <summary>
    /// Obtém usina por ID
    /// </summary>
    Task<UsinaDto?> GetByIdAsync(int id);

    /// <summary>
    /// Obtém usina por código
    /// </summary>
    Task<UsinaDto?> GetByCodigoAsync(string codigo);

    /// <summary>
    /// Obtém usinas por tipo
    /// </summary>
    Task<IEnumerable<UsinaDto>> GetByTipoAsync(int tipoUsinaId);

    /// <summary>
    /// Obtém usinas por empresa
    /// </summary>
    Task<IEnumerable<UsinaDto>> GetByEmpresaAsync(int empresaId);

    /// <summary>
    /// Cria nova usina
    /// </summary>
    Task<UsinaDto> CreateAsync(CreateUsinaDto createDto);

    /// <summary>
    /// Atualiza usina existente
    /// </summary>
    Task<UsinaDto> UpdateAsync(int id, UpdateUsinaDto updateDto);

    /// <summary>
    /// Remove usina
    /// </summary>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Verifica se código já existe
    /// </summary>
    Task<bool> CodigoExisteAsync(string codigo, int? usinaIdExcluir = null);
}
