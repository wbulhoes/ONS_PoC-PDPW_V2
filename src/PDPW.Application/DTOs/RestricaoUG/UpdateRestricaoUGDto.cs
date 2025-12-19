using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.RestricaoUG;

/// <summary>
/// DTO para atualização de Restrição de Unidade Geradora
/// </summary>
public class UpdateRestricaoUGDto
{
    [Required]
    public int UnidadeGeradoraId { get; set; }

    [Required]
    public DateTime DataInicio { get; set; }

    public DateTime? DataFim { get; set; }

    [Required]
    public int MotivoRestricaoId { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal PotenciaRestrita { get; set; }

    public string? Observacoes { get; set; }
}
