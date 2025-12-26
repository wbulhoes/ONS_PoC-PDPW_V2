namespace PDPW.Application.DTOs.Usuario;

/// <summary>
/// DTO de leitura de Usuário
/// </summary>
public class UsuarioDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public int? EquipePDPId { get; set; }
    public string? EquipePDPNome { get; set; }
    public string? Perfil { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
}
