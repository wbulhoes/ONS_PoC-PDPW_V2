namespace PDPW.Application.DTOs.UnidadeGeradora;

/// <summary>
/// DTO de leitura de Unidade Geradora
/// </summary>
public class UnidadeGeradoraDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public int UsinaId { get; set; }
    public string NomeUsina { get; set; } = string.Empty;
    public string CodigoUsina { get; set; } = string.Empty;
    public decimal PotenciaNominal { get; set; }
    public decimal PotenciaMinima { get; set; }
    public DateTime DataComissionamento { get; set; }
    public string? Status { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}
