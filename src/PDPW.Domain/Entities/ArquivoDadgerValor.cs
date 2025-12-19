namespace PDPW.Domain.Entities;

/// <summary>
/// Valor/dado extraído de um arquivo DADGER
/// </summary>
public class ArquivoDadgerValor : BaseEntity
{
    /// <summary>
    /// ID do arquivo DADGER
    /// </summary>
    public int ArquivoDadgerId { get; set; }

    /// <summary>
    /// Arquivo DADGER de origem
    /// </summary>
    public ArquivoDadger? ArquivoDadger { get; set; }

    /// <summary>
    /// Chave identificadora do valor (ex: "CADGER.PROD.UTE.001")
    /// </summary>
    public string Chave { get; set; } = string.Empty;

    /// <summary>
    /// Valor extraído
    /// </summary>
    public string? Valor { get; set; }

    /// <summary>
    /// Tipo do valor (String, Number, Date, etc)
    /// </summary>
    public string? Tipo { get; set; }

    /// <summary>
    /// Número da linha no arquivo original
    /// </summary>
    public int? Linha { get; set; }

    /// <summary>
    /// Observações sobre o valor
    /// </summary>
    public string? Observacoes { get; set; }
}
