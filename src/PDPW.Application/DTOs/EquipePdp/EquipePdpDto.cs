using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.EquipePdp;

/// <summary>
/// DTO de leitura de Equipe PDP
/// </summary>
public class EquipePdpDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Coordenador { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public int QuantidadeMembros { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}

/// <summary>
/// DTO para criação de Equipe PDP
/// </summary>
public class CreateEquipePdpDto
{
    [Required(ErrorMessage = "O nome da equipe é obrigatório")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "A descrição não pode ter mais de 1000 caracteres")]
    public string? Descricao { get; set; }

    [MaxLength(200, ErrorMessage = "O nome do coordenador não pode ter mais de 200 caracteres")]
    public string? Coordenador { get; set; }

    [EmailAddress(ErrorMessage = "Email inválido")]
    [MaxLength(200, ErrorMessage = "O email não pode ter mais de 200 caracteres")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Telefone inválido")]
    [MaxLength(20, ErrorMessage = "O telefone não pode ter mais de 20 caracteres")]
    public string? Telefone { get; set; }
}

/// <summary>
/// DTO para atualização de Equipe PDP
/// </summary>
public class UpdateEquipePdpDto
{
    [Required(ErrorMessage = "O nome da equipe é obrigatório")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "A descrição não pode ter mais de 1000 caracteres")]
    public string? Descricao { get; set; }

    [MaxLength(200, ErrorMessage = "O nome do coordenador não pode ter mais de 200 caracteres")]
    public string? Coordenador { get; set; }

    [EmailAddress(ErrorMessage = "Email inválido")]
    [MaxLength(200, ErrorMessage = "O email não pode ter mais de 200 caracteres")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Telefone inválido")]
    [MaxLength(20, ErrorMessage = "O telefone não pode ter mais de 20 caracteres")]
    public string? Telefone { get; set; }

    public bool Ativo { get; set; } = true;
}
