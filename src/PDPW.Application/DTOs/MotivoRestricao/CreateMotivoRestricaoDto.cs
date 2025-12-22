using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.MotivoRestricao;

/// <summary>
/// DTO para criação de Motivo de Restrição
/// </summary>
public class CreateMotivoRestricaoDto
{
    [Required(ErrorMessage = "O nome do motivo de restrição é obrigatório")]
    [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo 1000 caracteres")]
    public string? Descricao { get; set; }

    [StringLength(100, ErrorMessage = "A categoria deve ter no máximo 100 caracteres")]
    public string? Categoria { get; set; }
}
