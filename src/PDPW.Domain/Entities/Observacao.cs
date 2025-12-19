namespace PDPW.Domain.Entities;

/// <summary>
/// Observação ou anotação do sistema
/// </summary>
public class Observacao : BaseEntity
{
    /// <summary>
    /// Data de referência da observação
    /// </summary>
    public DateTime DataReferencia { get; set; }

    /// <summary>
    /// Título da observação
    /// </summary>
    public string Titulo { get; set; } = string.Empty;

    /// <summary>
    /// Conteúdo da observação
    /// </summary>
    public string Conteudo { get; set; } = string.Empty;

    /// <summary>
    /// Categoria da observação
    /// </summary>
    public string? Categoria { get; set; }

    /// <summary>
    /// Usuário autor da observação
    /// </summary>
    public string? UsuarioAutor { get; set; }
}
