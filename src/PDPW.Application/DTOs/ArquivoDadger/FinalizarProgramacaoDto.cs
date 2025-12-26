using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.ArquivoDadger;

/// <summary>
/// DTO para finalização de programação
/// </summary>
public class FinalizarProgramacaoDto
{
    [Required(ErrorMessage = "Usuário é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome do usuário deve ter no máximo 100 caracteres")]
    public string Usuario { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Observação deve ter no máximo 500 caracteres")]
    public string? Observacao { get; set; }
}
