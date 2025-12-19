namespace PDPW.Domain.Entities;

/// <summary>
/// Tipo de usina geradora de energia
/// </summary>
public class TipoUsina : BaseEntity
{
    /// <summary>
    /// Nome do tipo (Hidrelétrica, Térmica, Eólica, Solar, Nuclear)
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Descrição detalhada do tipo
    /// </summary>
    public string? Descricao { get; set; }

    /// <summary>
    /// Fonte de energia primária
    /// </summary>
    public string? FonteEnergia { get; set; }

    // Relacionamentos
    public ICollection<Usina>? Usinas { get; set; }
}
