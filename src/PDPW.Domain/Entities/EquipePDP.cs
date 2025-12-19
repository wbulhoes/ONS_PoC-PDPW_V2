namespace PDPW.Domain.Entities;

/// <summary>
/// Equipe responsável pela Programação Diária da Produção
/// </summary>
public class EquipePDP : BaseEntity
{
    /// <summary>
    /// Nome da equipe
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Descrição da equipe e suas responsabilidades
    /// </summary>
    public string? Descricao { get; set; }

    /// <summary>
    /// Nome do coordenador da equipe
    /// </summary>
    public string? Coordenador { get; set; }

    /// <summary>
    /// Email da equipe
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Telefone de contato
    /// </summary>
    public string? Telefone { get; set; }

    // Relacionamentos
    public ICollection<Usuario>? Membros { get; set; }
}
