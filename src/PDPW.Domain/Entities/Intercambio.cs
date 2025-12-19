namespace PDPW.Domain.Entities;

/// <summary>
/// Intercâmbio de energia entre subsistemas
/// </summary>
public class Intercambio : BaseEntity
{
    /// <summary>
    /// Data de referência
    /// </summary>
    public DateTime DataReferencia { get; set; }

    /// <summary>
    /// Subsistema de origem
    /// </summary>
    public string SubsistemaOrigem { get; set; } = string.Empty;

    /// <summary>
    /// Subsistema de destino
    /// </summary>
    public string SubsistemaDestino { get; set; } = string.Empty;

    /// <summary>
    /// Energia intercambiada em MWmed
    /// </summary>
    public decimal EnergiaIntercambiada { get; set; }

    /// <summary>
    /// Observações
    /// </summary>
    public string? Observacoes { get; set; }
}
