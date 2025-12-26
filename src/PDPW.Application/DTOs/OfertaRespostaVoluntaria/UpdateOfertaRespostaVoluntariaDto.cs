using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.OfertaRespostaVoluntaria;

public class UpdateOfertaRespostaVoluntariaDto
{
    [Required(ErrorMessage = "ID da Empresa é obrigatório")]
    public int EmpresaId { get; set; }

    [Required(ErrorMessage = "Data da oferta é obrigatória")]
    public DateTime DataOferta { get; set; }

    [Required(ErrorMessage = "Data do PDP é obrigatória")]
    public DateTime DataPDP { get; set; }

    [Required(ErrorMessage = "Hora inicial é obrigatória")]
    public TimeSpan HoraInicial { get; set; }

    [Required(ErrorMessage = "Hora final é obrigatória")]
    public TimeSpan HoraFinal { get; set; }

    [Required(ErrorMessage = "Redução de demanda em MW é obrigatória")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Redução deve ser maior que zero")]
    public decimal ReducaoDemandaMW { get; set; }

    [Required(ErrorMessage = "Preço por MWh é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
    public decimal PrecoMWh { get; set; }

    [Required(ErrorMessage = "Tipo de programa é obrigatório")]
    [StringLength(100, ErrorMessage = "Tipo de programa deve ter no máximo 100 caracteres")]
    public string TipoPrograma { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
    public string? Observacoes { get; set; }

    public int? SemanaPMOId { get; set; }
}
