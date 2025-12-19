namespace PDPW.Domain.Entities;

/// <summary>
/// Usina conversora (AC-DC ou DC-AC)
/// </summary>
public class UsinaConversora : BaseEntity
{
    /// <summary>
    /// ID da usina
    /// </summary>
    public int UsinaId { get; set; }

    /// <summary>
    /// Usina conversora
    /// </summary>
    public Usina? Usina { get; set; }

    /// <summary>
    /// Capacidade de conversão em MW
    /// </summary>
    public decimal CapacidadeConversao { get; set; }

    /// <summary>
    /// Tipo de conversão (AC-DC, DC-AC)
    /// </summary>
    public string TipoConversao { get; set; } = string.Empty;

    /// <summary>
    /// Eficiência da conversão (0-1)
    /// </summary>
    public decimal Eficiencia { get; set; }

    /// <summary>
    /// Observações
    /// </summary>
    public string? Observacoes { get; set; }
}
