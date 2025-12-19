namespace PDPW.Domain.Entities;

/// <summary>
/// Diretório para organização de arquivos
/// </summary>
public class Diretorio : BaseEntity
{
    /// <summary>
    /// Nome do diretório
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Caminho completo do diretório
    /// </summary>
    public string Caminho { get; set; } = string.Empty;

    /// <summary>
    /// ID do diretório pai (null se for raiz)
    /// </summary>
    public int? DiretorioPaiId { get; set; }

    /// <summary>
    /// Diretório pai
    /// </summary>
    public Diretorio? DiretorioPai { get; set; }

    /// <summary>
    /// Descrição do diretório
    /// </summary>
    public string? Descricao { get; set; }

    // Relacionamentos
    public ICollection<Arquivo>? Arquivos { get; set; }
    public ICollection<Diretorio>? Subdiretorios { get; set; }
}
