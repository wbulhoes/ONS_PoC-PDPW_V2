using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.ParadaUG;

/// <summary>
/// DTO para criação de Parada de Unidade Geradora
/// </summary>
public class CreateParadaUGDto
{
    [Required(ErrorMessage = "O ID da unidade geradora é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "ID da unidade geradora inválido")]
    public int UnidadeGeradoraId { get; set; }

    [Required(ErrorMessage = "A data de início da parada é obrigatória")]
    public DateTime DataInicio { get; set; }

    public DateTime? DataFim { get; set; }

    [Required(ErrorMessage = "O motivo da parada é obrigatório")]
    [StringLength(500, ErrorMessage = "O motivo deve ter no máximo 500 caracteres")]
    public string MotivoParada { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "As observações devem ter no máximo 2000 caracteres")]
    public string? Observacoes { get; set; }

    public bool Programada { get; set; } = false;
}
