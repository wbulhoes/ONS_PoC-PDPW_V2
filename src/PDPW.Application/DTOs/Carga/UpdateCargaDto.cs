using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.Carga;

/// <summary>
/// DTO para atualização de Carga
/// </summary>
public class UpdateCargaDto
{
    [Required]
    public DateTime DataReferencia { get; set; }

    [Required]
    [StringLength(10)]
    public string SubsistemaId { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal CargaMWmed { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal CargaVerificada { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal PrevisaoCarga { get; set; }

    public string? Observacoes { get; set; }
}
