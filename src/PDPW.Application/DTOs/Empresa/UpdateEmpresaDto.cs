using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.Empresa;

/// <summary>
/// DTO para atualização de Empresa
/// </summary>
public class UpdateEmpresaDto
{
    /// <summary>
    /// Nome da empresa
    /// </summary>
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// CNPJ da empresa (formato: 00.000.000/0000-00)
    /// </summary>
    [RegularExpression(@"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$", ErrorMessage = "CNPJ inválido. Formato esperado: 00.000.000/0000-00")]
    [StringLength(18, ErrorMessage = "CNPJ deve ter 18 caracteres")]
    public string? CNPJ { get; set; }

    /// <summary>
    /// Telefone de contato
    /// </summary>
    [Phone(ErrorMessage = "Telefone inválido")]
    [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
    public string? Telefone { get; set; }

    /// <summary>
    /// Email de contato
    /// </summary>
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(100, ErrorMessage = "Email deve ter no máximo 100 caracteres")]
    public string? Email { get; set; }

    /// <summary>
    /// Endereço completo
    /// </summary>
    [StringLength(500, ErrorMessage = "Endereço deve ter no máximo 500 caracteres")]
    public string? Endereco { get; set; }

    /// <summary>
    /// Indica se a empresa está ativa
    /// </summary>
    public bool Ativo { get; set; } = true;
}
