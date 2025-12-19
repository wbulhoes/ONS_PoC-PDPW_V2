namespace PDPW.Domain.Entities;

/// <summary>
/// Parada de unidade geradora
/// </summary>
public class ParadaUG : BaseEntity
{
    /// <summary>
    /// ID da unidade geradora
    /// </summary>
    public int UnidadeGeradoraId { get; set; }

    /// <summary>
    /// Unidade geradora em parada
    /// </summary>
    public UnidadeGeradora? UnidadeGeradora { get; set; }

    /// <summary>
    /// Data de início da parada
    /// </summary>
    public DateTime DataInicio { get; set; }

    /// <summary>
    /// Data de fim da parada (null se ainda em parada)
    /// </summary>
    public DateTime? DataFim { get; set; }

    /// <summary>
    /// Motivo da parada
    /// </summary>
    public string MotivoParada { get; set; } = string.Empty;

    /// <summary>
    /// Observações sobre a parada
    /// </summary>
    public string? Observacoes { get; set; }

    /// <summary>
    /// Indica se a parada foi programada
    /// </summary>
    public bool Programada { get; set; }
}
