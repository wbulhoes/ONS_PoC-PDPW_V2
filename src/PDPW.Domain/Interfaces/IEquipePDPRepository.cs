using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Interface do repositório de Equipe PDP
/// </summary>
public interface IEquipePDPRepository
{
    /// <summary>
    /// Obtém todas as equipes PDP ativas
    /// </summary>
    Task<IEnumerable<EquipePDP>> ObterTodasAsync();

    /// <summary>
    /// Obtém equipe PDP por ID
    /// </summary>
    Task<EquipePDP?> ObterPorIdAsync(int id);

    /// <summary>
    /// Adiciona nova equipe PDP
    /// </summary>
    Task<EquipePDP> AdicionarAsync(EquipePDP equipe);

    /// <summary>
    /// Atualiza equipe PDP existente
    /// </summary>
    Task AtualizarAsync(EquipePDP equipe);

    /// <summary>
    /// Remove equipe PDP (soft delete)
    /// </summary>
    Task RemoverAsync(int id);

    /// <summary>
    /// Obtém equipe por nome
    /// </summary>
    Task<EquipePDP?> ObterPorNomeAsync(string nome);

    /// <summary>
    /// Verifica se já existe uma equipe com o mesmo nome
    /// </summary>
    Task<bool> ExisteNomeAsync(string nome, int? excluirId = null);

    /// <summary>
    /// Obtém equipe com membros incluídos
    /// </summary>
    Task<EquipePDP?> ObterComMembrosAsync(int id);
}
