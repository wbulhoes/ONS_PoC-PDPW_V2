namespace PDPW.Domain.Entities;

/// <summary>
/// Carga elétrica do sistema por subsistema
/// </summary>
public class Carga : BaseEntity
{
    /// <summary>
    /// Data de referência
    /// </summary>
    public DateTime DataReferencia { get; set; }

    /// <summary>
    /// Identificador do subsistema (SE, S, NE, N)
    /// </summary>
    public string SubsistemaId { get; set; } = string.Empty;

    /// <summary>
    /// Carga média em MWmed
    /// </summary>
    public decimal CargaMWmed { get; set; }

    /// <summary>
    /// Carga verificada
    /// </summary>
    public decimal CargaVerificada { get; set; }

    /// <summary>
    /// Previsão de carga
    /// </summary>
    public decimal PrevisaoCarga { get; set; }

    /// <summary>
    /// Observações
    /// </summary>
    public string? Observacoes { get; set; }
}
