namespace PDPW.Domain.Entities;

/// <summary>
/// Entidade base para todas as entidades do domínio
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
    public bool Ativo { get; set; } = true;
}
