using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.Usina;

/// <summary>
/// DTO para atualização de Usina
/// </summary>
public class UpdateUsinaDto
{
    [Required(ErrorMessage = "O código é obrigatório")]
    [StringLength(50, ErrorMessage = "O código deve ter no máximo 50 caracteres")]
    public string Codigo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tipo de usina é obrigatório")]
    [Range(1, int.MaxValue, ErrorMessage = "Tipo de usina inválido")]
    public int TipoUsinaId { get; set; }

    [Required(ErrorMessage = "A empresa é obrigatória")]
    [Range(1, int.MaxValue, ErrorMessage = "Empresa inválida")]
    public int EmpresaId { get; set; }

    [Required(ErrorMessage = "A capacidade instalada é obrigatória")]
    [Range(0.01, double.MaxValue, ErrorMessage = "A capacidade deve ser maior que zero")]
    public decimal CapacidadeInstalada { get; set; }

    [StringLength(500, ErrorMessage = "A localização deve ter no máximo 500 caracteres")]
    public string? Localizacao { get; set; }

    [Required(ErrorMessage = "A data de operação é obrigatória")]
    public DateTime DataOperacao { get; set; }

    public bool Ativo { get; set; }
}
