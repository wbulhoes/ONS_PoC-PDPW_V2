using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.Balanco;

/// <summary>
/// DTO para criação de Balanço Energético
/// </summary>
public class CreateBalancoDto
{
    [Required(ErrorMessage = "A data de referência é obrigatória")]
    public DateTime DataReferencia { get; set; }

    [Required(ErrorMessage = "O subsistema é obrigatório")]
    [StringLength(10, ErrorMessage = "O subsistema deve ter no máximo 10 caracteres")]
    public string SubsistemaId { get; set; } = string.Empty;

    [Required(ErrorMessage = "A geração é obrigatória")]
    [Range(0, double.MaxValue, ErrorMessage = "A geração não pode ser negativa")]
    public decimal Geracao { get; set; }

    [Required(ErrorMessage = "A carga é obrigatória")]
    [Range(0, double.MaxValue, ErrorMessage = "A carga não pode ser negativa")]
    public decimal Carga { get; set; }

    [Required(ErrorMessage = "O intercâmbio é obrigatório")]
    public decimal Intercambio { get; set; }

    [Required(ErrorMessage = "As perdas são obrigatórias")]
    [Range(0, double.MaxValue, ErrorMessage = "As perdas não podem ser negativas")]
    public decimal Perdas { get; set; }

    [Required(ErrorMessage = "O déficit é obrigatório")]
    [Range(0, double.MaxValue, ErrorMessage = "O déficit não pode ser negativo")]
    public decimal Deficit { get; set; }

    [StringLength(2000, ErrorMessage = "As observações devem ter no máximo 2000 caracteres")]
    public string? Observacoes { get; set; }
}
