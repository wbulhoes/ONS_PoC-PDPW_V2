namespace PDPW.Domain.Entities;

/// <summary>
/// DCR - Dados Consolidados Revisão
/// </summary>
public class DCR : BaseEntity
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
    /// ID do DCA relacionado (opcional)
    /// </summary>
    public int? DCAId { get; set; }

    /// <summary>
    /// DCA que está sendo revisado
    /// </summary>
    public DCA? DCA { get; set; }

    /// <summary>
    /// Dados revisados em formato JSON
    /// </summary>
    public string? DadosRevisados { get; set; }

    /// <summary>
    /// Motivo da revisão
    /// </summary>
    public string? MotivoRevisao { get; set; }

    /// <summary>
    /// Indica se foi aprovado
    /// </summary>
    public bool Aprovado { get; set; }

    /// <summary>
    /// Observações
    /// </summary>
    public string? Observacoes { get; set; }
}
