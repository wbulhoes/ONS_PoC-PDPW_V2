namespace PDPW.Domain.Entities;

/// <summary>
/// Usuário do sistema PDPW
/// </summary>
public class Usuario : BaseEntity
{
    /// <summary>
    /// Nome completo do usuário
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Email do usuário
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Telefone de contato
    /// </summary>
    public string? Telefone { get; set; }

    /// <summary>
    /// ID da equipe PDP (opcional)
    /// </summary>
    public int? EquipePDPId { get; set; }

    /// <summary>
    /// Equipe PDP do usuário
    /// </summary>
    public EquipePDP? EquipePDP { get; set; }

    /// <summary>
    /// Perfil de acesso (Admin, Operador, Consultor, etc)
    /// </summary>
    public string? Perfil { get; set; }
}
