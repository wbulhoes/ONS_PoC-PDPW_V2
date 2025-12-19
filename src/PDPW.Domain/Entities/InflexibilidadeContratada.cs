namespace PDPW.Domain.Entities;

/// <summary>
/// Inflexibilidade contratada de usinas
/// </summary>
public class InflexibilidadeContratada : BaseEntity
{
    /// <summary>
    /// ID da usina
    /// </summary>
    public int UsinaId { get; set; }

    /// <summary>
    /// Usina com inflexibilidade
    /// </summary>
    public Usina? Usina { get; set; }

    /// <summary>
    /// Data de início do contrato
    /// </summary>
    public DateTime DataInicio { get; set; }

    /// <summary>
    /// Data de fim do contrato
    /// </summary>
    public DateTime DataFim { get; set; }

    /// <summary>
    /// Geração mínima contratada em MW
    /// </summary>
    public decimal GeracaoMinima { get; set; }

    /// <summary>
    /// Número ou referência do contrato
    /// </summary>
    public string? Contrato { get; set; }

    /// <summary>
    /// Observações
    /// </summary>
    public string? Observacoes { get; set; }
}
