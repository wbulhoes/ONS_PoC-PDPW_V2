namespace PDPW.Domain.Entities;

/// <summary>
/// Modalidade de operação de usinas térmicas
/// </summary>
public class ModalidadeOpTermica : BaseEntity
{
    /// <summary>
    /// Nome da modalidade
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Descrição da modalidade
    /// </summary>
    public string? Descricao { get; set; }

    /// <summary>
    /// Custo operacional (R$/MWh)
    /// </summary>
    public decimal CustoOperacional { get; set; }

    /// <summary>
    /// Potência mínima (MW)
    /// </summary>
    public decimal PotenciaMinima { get; set; }

    /// <summary>
    /// Potência máxima (MW)
    /// </summary>
    public decimal PotenciaMaxima { get; set; }
}
