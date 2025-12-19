using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.Carga;

/// <summary>
/// DTO para criação de Carga
/// </summary>
public class CreateCargaDto
{
    [Required(ErrorMessage = "Data de referência é obrigatória")]
    public DateTime DataReferencia { get; set; }

    [Required(ErrorMessage = "Subsistema é obrigatório")]
    [StringLength(10)]
    public string SubsistemaId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Carga MWmed é obrigatória")]
    [Range(0, double.MaxValue, ErrorMessage = "Carga MWmed deve ser maior ou igual a zero")]
    public decimal CargaMWmed { get; set; }

    [Required(ErrorMessage = "Carga verificada é obrigatória")]
    [Range(0, double.MaxValue)]
    public decimal CargaVerificada { get; set; }

    [Required(ErrorMessage = "Previsão de carga é obrigatória")]
    [Range(0, double.MaxValue)]
    public decimal PrevisaoCarga { get; set; }

    public string? Observacoes { get; set; }
}
