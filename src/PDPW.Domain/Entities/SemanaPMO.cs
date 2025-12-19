namespace PDPW.Domain.Entities;

/// <summary>
/// Semana de planejamento da operação (PMO - Programa Mensal de Operação)
/// </summary>
public class SemanaPMO : BaseEntity
{
    /// <summary>
    /// Número da semana no PMO
    /// </summary>
    public int Numero { get; set; }

    /// <summary>
    /// Data de início da semana
    /// </summary>
    public DateTime DataInicio { get; set; }

    /// <summary>
    /// Data de fim da semana
    /// </summary>
    public DateTime DataFim { get; set; }

    /// <summary>
    /// Ano de referência
    /// </summary>
    public int Ano { get; set; }

    /// <summary>
    /// Observações sobre a semana
    /// </summary>
    public string? Observacoes { get; set; }

    // Relacionamentos
    public ICollection<ArquivoDadger>? ArquivosDadger { get; set; }
    public ICollection<DCA>? DCAs { get; set; }
    public ICollection<DCR>? DCRs { get; set; }
}
