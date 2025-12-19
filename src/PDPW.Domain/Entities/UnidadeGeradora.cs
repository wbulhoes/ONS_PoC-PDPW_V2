namespace PDPW.Domain.Entities;

/// <summary>
/// Unidade geradora de uma usina
/// </summary>
public class UnidadeGeradora : BaseEntity
{
    /// <summary>
    /// Código da unidade geradora
    /// </summary>
    public string Codigo { get; set; } = string.Empty;

    /// <summary>
    /// Nome da unidade geradora
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// ID da usina
    /// </summary>
    public int UsinaId { get; set; }

    /// <summary>
    /// Usina à qual pertence
    /// </summary>
    public Usina? Usina { get; set; }

    /// <summary>
    /// Potência nominal em MW
    /// </summary>
    public decimal PotenciaNominal { get; set; }

    /// <summary>
    /// Potência mínima em MW
    /// </summary>
    public decimal PotenciaMinima { get; set; }

    /// <summary>
    /// Data de comissionamento
    /// </summary>
    public DateTime DataComissionamento { get; set; }

    /// <summary>
    /// Status operacional (Operando, Parada, Manutenção, etc)
    /// </summary>
    public string? Status { get; set; }

    // Relacionamentos
    public ICollection<ParadaUG>? Paradas { get; set; }
    public ICollection<RestricaoUG>? Restricoes { get; set; }
}
