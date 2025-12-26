using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.Usuario;

/// <summary>
/// DTO para criação de Usuário
/// </summary>
public class CreateUsuarioDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Telefone inválido")]
    public string? Telefone { get; set; }

    public int? EquipePDPId { get; set; }

    [StringLength(50, ErrorMessage = "Perfil deve ter no máximo 50 caracteres")]
    public string? Perfil { get; set; }
}
