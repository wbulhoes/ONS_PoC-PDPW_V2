using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.RestricaoUG;

/// <summary>
/// DTO para criação de Restrição de Unidade Geradora
/// </summary>
public class CreateRestricaoUGDto
{
    [Required(ErrorMessage = "Unidade Geradora é obrigatória")]
    public int UnidadeGeradoraId { get; set; }

    [Required(ErrorMessage = "Data de início é obrigatória")]
    public DateTime DataInicio { get; set; }

    public DateTime? DataFim { get; set; }

    [Required(ErrorMessage = "Motivo da restrição é obrigatório")]
    public int MotivoRestricaoId { get; set; }

    [Required(ErrorMessage = "Potência restrita é obrigatória")]
    [Range(0, double.MaxValue, ErrorMessage = "Potência restrita deve ser maior ou igual a zero")]
    public decimal PotenciaRestrita { get; set; }

    public string? Observacoes { get; set; }
}
