using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.ArquivoDadger;

/// <summary>
/// DTO para criação de Arquivo DADGER
/// </summary>
public class CreateArquivoDadgerDto
{
    [Required(ErrorMessage = "Nome do arquivo é obrigatório")]
    [StringLength(500)]
    public string NomeArquivo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Caminho do arquivo é obrigatório")]
    [StringLength(1000)]
    public string CaminhoArquivo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Data de importação é obrigatória")]
    public DateTime DataImportacao { get; set; }

    [Required(ErrorMessage = "Semana PMO é obrigatória")]
    public int SemanaPMOId { get; set; }

    public string? Observacoes { get; set; }
}
