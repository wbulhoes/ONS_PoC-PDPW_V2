namespace PDPW.Domain.Entities;

/// <summary>
/// Arquivo armazenado no sistema
/// </summary>
public class Arquivo : BaseEntity
{
    /// <summary>
    /// Nome do arquivo
    /// </summary>
    public string NomeArquivo { get; set; } = string.Empty;

    /// <summary>
    /// Caminho completo do arquivo
    /// </summary>
    public string CaminhoCompleto { get; set; } = string.Empty;

    /// <summary>
    /// ID do diretório
    /// </summary>
    public int DiretorioId { get; set; }

    /// <summary>
    /// Diretório onde está armazenado
    /// </summary>
    public Diretorio? Diretorio { get; set; }

    /// <summary>
    /// Tamanho em bytes
    /// </summary>
    public long TamanhoBytes { get; set; }

    /// <summary>
    /// Tipo do arquivo (extensão ou MIME)
    /// </summary>
    public string? TipoArquivo { get; set; }

    /// <summary>
    /// Data de criação do arquivo
    /// </summary>
    public DateTime DataCriacao { get; set; }

    /// <summary>
    /// Observações
    /// </summary>
    public string? Observacoes { get; set; }
}
