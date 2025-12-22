using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.UnidadeGeradora;

/// <summary>
/// DTO para atualização de Unidade Geradora
/// </summary>
public class UpdateUnidadeGeradoraDto
{
    [Required(ErrorMessage = "O código da unidade geradora é obrigatório")]
    [StringLength(50, ErrorMessage = "O código deve ter no máximo 50 caracteres")]
    public string Codigo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nome da unidade geradora é obrigatório")]
    [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O ID da usina é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "ID da usina inválido")]
    public int UsinaId { get; set; }

    [Required(ErrorMessage = "A potência nominal é obrigatória")]
    [Range(0.01, double.MaxValue, ErrorMessage = "A potência nominal deve ser maior que zero")]
    public decimal PotenciaNominal { get; set; }

    [Required(ErrorMessage = "A potência mínima é obrigatória")]
    [Range(0, double.MaxValue, ErrorMessage = "A potência mínima não pode ser negativa")]
    public decimal PotenciaMinima { get; set; }

    [Required(ErrorMessage = "A data de comissionamento é obrigatória")]
    public DateTime DataComissionamento { get; set; }

    [StringLength(100, ErrorMessage = "O status deve ter no máximo 100 caracteres")]
    public string? Status { get; set; }

    public bool Ativo { get; set; } = true;
}
