using PDPW.Application.DTOs.TipoUsina;

namespace PDPW.Application.Interfaces;

/// <summary>
/// Interface do serviço de Tipo de Usina
/// </summary>
public interface ITipoUsinaService
{
    /// <summary>
    /// Obtém todos os tipos de usina
    /// </summary>
    Task<List<TipoUsinaDto>> GetAllAsync();

    /// <summary>
    /// Obtém tipo de usina por ID
    /// </summary>
    Task<TipoUsinaDto?> GetByIdAsync(int id);

    /// <summary>
    /// Obtém tipo de usina por nome
    /// </summary>
    Task<TipoUsinaDto?> GetByNomeAsync(string nome);

    /// <summary>
    /// Cria um novo tipo de usina
    /// </summary>
    Task<TipoUsinaDto> CreateAsync(CreateTipoUsinaDto dto);

    /// <summary>
    /// Atualiza um tipo de usina existente
    /// </summary>
    Task<TipoUsinaDto?> UpdateAsync(int id, UpdateTipoUsinaDto dto);

    /// <summary>
    /// Remove um tipo de usina (soft delete)
    /// </summary>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Verifica se já existe um tipo com o nome informado
    /// </summary>
    Task<bool> ExisteNomeAsync(string nome, int? tipoUsinaIdExcluir = null);
}
