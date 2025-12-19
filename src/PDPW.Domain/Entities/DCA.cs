namespace PDPW.Domain.Entities;

/// <summary>
/// DCA - Dados Consolidados Anterior
/// </summary>
public class DCA : BaseEntity
{
    /// <summary>
    /// Data de referência
    /// </summary>
    public DateTime DataReferencia { get; set; }

    /// <summary>
    /// ID da semana PMO
    /// </summary>
    public int SemanaPMOId { get; set; }

    /// <summary>
    /// Semana PMO relacionada
    /// </summary>
    public SemanaPMO? SemanaPMO { get; set; }

    /// <summary>
    /// Dados consolidados em formato JSON
    /// </summary>
    public string? DadosConsolidados { get; set; }

    /// <summary>
    /// Indica se foi aprovado
    /// </summary>
    public bool Aprovado { get; set; }

    /// <summary>
    /// Data de aprovação
    /// </summary>
    public DateTime? DataAprovacao { get; set; }

    /// <summary>
    /// Usuário que aprovou
    /// </summary>
    public string? UsuarioAprovacao { get; set; }

    /// <summary>
    /// Observações
    /// </summary>
    public string? Observacoes { get; set; }

    // Relacionamentos
    public ICollection<DCR>? DCRs { get; set; }
}
