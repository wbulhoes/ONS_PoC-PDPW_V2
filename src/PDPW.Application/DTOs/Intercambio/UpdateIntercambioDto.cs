using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.Intercambio;

/// <summary>
/// DTO para atualização de Intercâmbio de Energia
/// </summary>
public class UpdateIntercambioDto
{
    [Required(ErrorMessage = "A data de referência é obrigatória")]
    public DateTime DataReferencia { get; set; }

    [Required(ErrorMessage = "O subsistema de origem é obrigatório")]
    [StringLength(10, ErrorMessage = "O subsistema de origem deve ter no máximo 10 caracteres")]
    public string SubsistemaOrigem { get; set; } = string.Empty;

    [Required(ErrorMessage = "O subsistema de destino é obrigatório")]
    [StringLength(10, ErrorMessage = "O subsistema de destino deve ter no máximo 10 caracteres")]
    public string SubsistemaDestino { get; set; } = string.Empty;

    [Required(ErrorMessage = "A energia intercambiada é obrigatória")]
    public decimal EnergiaIntercambiada { get; set; }

    [StringLength(2000, ErrorMessage = "As observações devem ter no máximo 2000 caracteres")]
    public string? Observacoes { get; set; }

    public bool Ativo { get; set; } = true;
}
