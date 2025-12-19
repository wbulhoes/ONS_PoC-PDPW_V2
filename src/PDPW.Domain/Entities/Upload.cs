namespace PDPW.Domain.Entities;

/// <summary>
/// Arquivo enviado para o sistema
/// </summary>
public class Upload : BaseEntity
{
    /// <summary>
    /// Nome do arquivo original
    /// </summary>
    public string NomeArquivo { get; set; } = string.Empty;

    /// <summary>
    /// Caminho onde o arquivo foi salvo
    /// </summary>
    public string CaminhoArquivo { get; set; } = string.Empty;

    /// <summary>
    /// Tamanho do arquivo em bytes
    /// </summary>
    public long TamanhoBytes { get; set; }

    /// <summary>
    /// Tipo MIME do arquivo
    /// </summary>
    public string? TipoArquivo { get; set; }

    /// <summary>
    /// Data do upload
    /// </summary>
    public DateTime DataUpload { get; set; }

    /// <summary>
    /// Usuário que fez o upload
    /// </summary>
    public string? UsuarioUpload { get; set; }

    /// <summary>
    /// Observações sobre o arquivo
    /// </summary>
    public string? Observacoes { get; set; }
}
