namespace PDPW.Domain.Entities;

/// <summary>
/// Balanço energético do subsistema
/// </summary>
public class Balanco : BaseEntity
{
    /// <summary>
    /// Data de referência
    /// </summary>
    public DateTime DataReferencia { get; set; }

    /// <summary>
    /// Identificador do subsistema
    /// </summary>
    public string SubsistemaId { get; set; } = string.Empty;

    /// <summary>
    /// Geração total em MWmed
    /// </summary>
    public decimal Geracao { get; set; }

    /// <summary>
    /// Carga total em MWmed
    /// </summary>
    public decimal Carga { get; set; }

    /// <summary>
    /// Intercâmbio líquido em MWmed (positivo = exportação)
    /// </summary>
    public decimal Intercambio { get; set; }

    /// <summary>
    /// Perdas em MWmed
    /// </summary>
    public decimal Perdas { get; set; }

    /// <summary>
    /// Déficit em MWmed
    /// </summary>
    public decimal Deficit { get; set; }

    /// <summary>
    /// Observações
    /// </summary>
    public string? Observacoes { get; set; }
}
