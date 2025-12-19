namespace PDPW.Domain.Entities;

/// <summary>
/// Relatório gerado pelo sistema
/// </summary>
public class Relatorio : BaseEntity
{
    /// <summary>
    /// Nome do relatório
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Descrição do relatório
    /// </summary>
    public string? Descricao { get; set; }

    /// <summary>
    /// Tipo do relatório (Excel, PDF, CSV, etc)
    /// </summary>
    public string TipoRelatorio { get; set; } = string.Empty;

    /// <summary>
    /// Data de geração
    /// </summary>
    public DateTime DataGeracao { get; set; }

    /// <summary>
    /// Caminho do arquivo gerado
    /// </summary>
    public string? CaminhoArquivo { get; set; }

    /// <summary>
    /// Parâmetros usados na geração (JSON)
    /// </summary>
    public string? Parametros { get; set; }

    /// <summary>
    /// Usuário que gerou o relatório
    /// </summary>
    public string? UsuarioGeracao { get; set; }
}
