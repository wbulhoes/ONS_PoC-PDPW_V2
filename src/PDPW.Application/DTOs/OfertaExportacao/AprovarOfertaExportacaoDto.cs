using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.OfertaExportacao;

/// <summary>
/// DTO para aprovação de Oferta de Exportação pelo ONS
/// </summary>
public class AprovarOfertaExportacaoDto
{
    [Required(ErrorMessage = "Usuário do ONS é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome do usuário deve ter no máximo 100 caracteres")]
    public string UsuarioONS { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Observação deve ter no máximo 500 caracteres")]
    public string? Observacao { get; set; }
}
