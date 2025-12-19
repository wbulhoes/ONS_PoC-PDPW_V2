namespace PDPW.Domain.Entities;

/// <summary>
/// Restrição de geração de unidade geradora
/// </summary>
public class RestricaoUG : BaseEntity
{
    /// <summary>
    /// ID da unidade geradora
    /// </summary>
    public int UnidadeGeradoraId { get; set; }

    /// <summary>
    /// Unidade geradora com restrição
    /// </summary>
    public UnidadeGeradora? UnidadeGeradora { get; set; }

    /// <summary>
    /// Data de início da restrição
    /// </summary>
    public DateTime DataInicio { get; set; }

    /// <summary>
    /// Data de fim da restrição
    /// </summary>
    public DateTime? DataFim { get; set; }

    /// <summary>
    /// ID do motivo da restrição
    /// </summary>
    public int MotivoRestricaoId { get; set; }

    /// <summary>
    /// Motivo da restrição
    /// </summary>
    public MotivoRestricao? MotivoRestricao { get; set; }

    /// <summary>
    /// Potência restrita em MW
    /// </summary>
    public decimal PotenciaRestrita { get; set; }

    /// <summary>
    /// Observações
    /// </summary>
    public string? Observacoes { get; set; }
}
