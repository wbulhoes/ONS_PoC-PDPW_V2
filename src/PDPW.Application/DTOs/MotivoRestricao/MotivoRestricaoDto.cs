namespace PDPW.Application.DTOs.MotivoRestricao;

/// <summary>
/// DTO de leitura de Motivo de Restrição
/// </summary>
public class MotivoRestricaoDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Categoria { get; set; }
    public int QuantidadeRestricoesUG { get; set; }
    public int QuantidadeRestricoesUS { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}
