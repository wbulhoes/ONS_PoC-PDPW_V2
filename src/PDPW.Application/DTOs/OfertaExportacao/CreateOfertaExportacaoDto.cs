using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.OfertaExportacao;

/// <summary>
/// DTO para criação de Oferta de Exportação
/// </summary>
public class CreateOfertaExportacaoDto
{
    [Required(ErrorMessage = "ID da Usina é obrigatório")]
    public int UsinaId { get; set; }

    [Required(ErrorMessage = "Data da oferta é obrigatória")]
    public DateTime DataOferta { get; set; }

    [Required(ErrorMessage = "Data do PDP é obrigatória")]
    public DateTime DataPDP { get; set; }

    [Required(ErrorMessage = "Valor em MW é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor em MW deve ser maior que zero")]
    public decimal ValorMW { get; set; }

    [Required(ErrorMessage = "Preço por MWh é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
    public decimal PrecoMWh { get; set; }

    [Required(ErrorMessage = "Hora inicial é obrigatória")]
    public TimeSpan HoraInicial { get; set; }

    [Required(ErrorMessage = "Hora final é obrigatória")]
    public TimeSpan HoraFinal { get; set; }

    [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
    public string? Observacoes { get; set; }

    public int? SemanaPMOId { get; set; }
}
