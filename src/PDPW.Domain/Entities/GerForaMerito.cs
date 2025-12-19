namespace PDPW.Domain.Entities;

/// <summary>
/// Geração fora da ordem de mérito
/// </summary>
public class GerForaMerito : BaseEntity
{
    /// <summary>
    /// Data de referência
    /// </summary>
    public DateTime DataReferencia { get; set; }

    /// <summary>
    /// ID da usina
    /// </summary>
    public int UsinaId { get; set; }

    /// <summary>
    /// Usina gerando fora de mérito
    /// </summary>
    public Usina? Usina { get; set; }

    /// <summary>
    /// Geração média em MWmed
    /// </summary>
    public decimal GeracaoMWmed { get; set; }

    /// <summary>
    /// Motivo da geração fora de mérito
    /// </summary>
    public string? Motivo { get; set; }

    /// <summary>
    /// Observações
    /// </summary>
    public string? Observacoes { get; set; }
}
