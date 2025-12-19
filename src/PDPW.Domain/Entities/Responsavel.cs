namespace PDPW.Domain.Entities;

/// <summary>
/// Responsável técnico ou gerencial
/// </summary>
public class Responsavel : BaseEntity
{
    /// <summary>
    /// Nome completo
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Cargo ou função
    /// </summary>
    public string? Cargo { get; set; }

    /// <summary>
    /// Email de contato
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Telefone de contato
    /// </summary>
    public string? Telefone { get; set; }

    /// <summary>
    /// Área de atuação
    /// </summary>
    public string? Area { get; set; }
}
