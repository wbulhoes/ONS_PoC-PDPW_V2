namespace PDPW.Application.DTOs.Balanco;

/// <summary>
/// DTO de leitura de Balanço Energético
/// </summary>
public class BalancoDto
{
    public int Id { get; set; }
    public DateTime DataReferencia { get; set; }
    public string SubsistemaId { get; set; } = string.Empty;
    public decimal Geracao { get; set; }
    public decimal Carga { get; set; }
    public decimal Intercambio { get; set; }
    public decimal Perdas { get; set; }
    public decimal Deficit { get; set; }
    public decimal BalancoCalculado { get; set; }
    public string? Observacoes { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}
