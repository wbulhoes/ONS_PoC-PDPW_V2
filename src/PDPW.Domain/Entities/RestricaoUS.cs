namespace PDPW.Domain.Entities;

/// <summary>
/// Restrição de geração de usina
/// </summary>
public class RestricaoUS : BaseEntity
{
    /// <summary>
    /// ID da usina
    /// </summary>
    public int UsinaId { get; set; }

    /// <summary>
    /// Usina com restrição
    /// </summary>
    public Usina? Usina { get; set; }

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
    /// Capacidade restrita em MW
    /// </summary>
    public decimal CapacidadeRestrita { get; set; }

    /// <summary>
    /// Observações
    /// </summary>
    public string? Observacoes { get; set; }
}
