namespace PDPW.Application.DTOs.Carga;

/// <summary>
/// DTO de leitura de Carga
/// </summary>
public class CargaDto
{
    public int Id { get; set; }
    public DateTime DataReferencia { get; set; }
    public string SubsistemaId { get; set; } = string.Empty;
    public decimal CargaMWmed { get; set; }
    public decimal CargaVerificada { get; set; }
    public decimal PrevisaoCarga { get; set; }
    public string? Observacoes { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
}
