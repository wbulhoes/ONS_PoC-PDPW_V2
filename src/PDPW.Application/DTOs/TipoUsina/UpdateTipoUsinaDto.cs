using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.TipoUsina;

/// <summary>
/// DTO para atualização de Tipo de Usina
/// </summary>
public class UpdateTipoUsinaDto
{
    /// <summary>
    /// Nome do tipo (ex: Hidrelétrica, Térmica, Eólica)
    /// </summary>
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Descrição detalhada do tipo
    /// </summary>
    [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
    public string? Descricao { get; set; }

    /// <summary>
    /// Fonte de energia primária
    /// </summary>
    [StringLength(100, ErrorMessage = "A fonte de energia deve ter no máximo 100 caracteres")]
    public string? FonteEnergia { get; set; }

    /// <summary>
    /// Indica se o tipo está ativo
    /// </summary>
    public bool Ativo { get; set; } = true;
}
