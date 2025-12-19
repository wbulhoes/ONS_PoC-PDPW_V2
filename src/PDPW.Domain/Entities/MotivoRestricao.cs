namespace PDPW.Domain.Entities;

/// <summary>
/// Motivo de restrição de geração de usinas
/// </summary>
public class MotivoRestricao : BaseEntity
{
    /// <summary>
    /// Nome do motivo
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Descrição detalhada
    /// </summary>
    public string? Descricao { get; set; }

    /// <summary>
    /// Categoria do motivo (Técnico, Ambiental, Operacional, etc)
    /// </summary>
    public string? Categoria { get; set; }

    // Relacionamentos
    public ICollection<RestricaoUG>? RestricoesUG { get; set; }
    public ICollection<RestricaoUS>? RestricoesUS { get; set; }
}
