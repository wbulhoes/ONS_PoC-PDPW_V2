using PDPW.Application.DTOs.EquipePdp;

namespace PDPW.Application.Interfaces;

/// <summary>
/// Interface do serviço de Equipe PDP
/// </summary>
public interface IEquipePdpService
{
    /// <summary>
    /// Obtém todas as equipes PDP
    /// </summary>
    Task<IEnumerable<EquipePdpDto>> GetAllAsync();

    /// <summary>
    /// Obtém equipe PDP por ID
    /// </summary>
    Task<EquipePdpDto?> GetByIdAsync(int id);

    /// <summary>
    /// Obtém equipe PDP por nome
    /// </summary>
    Task<EquipePdpDto?> GetByNomeAsync(string nome);

    /// <summary>
    /// Obtém equipe PDP com membros incluídos
    /// </summary>
    Task<EquipePdpDto?> GetComMembrosAsync(int id);

    /// <summary>
    /// Cria uma nova equipe PDP
    /// </summary>
    Task<EquipePdpDto> CreateAsync(CreateEquipePdpDto dto);

    /// <summary>
    /// Atualiza uma equipe PDP existente
    /// </summary>
    Task<EquipePdpDto?> UpdateAsync(int id, UpdateEquipePdpDto dto);

    /// <summary>
    /// Remove uma equipe PDP (soft delete)
    /// </summary>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Verifica se nome já existe
    /// </summary>
    Task<bool> ExisteNomeAsync(string nome, int? excluirId = null);
}
