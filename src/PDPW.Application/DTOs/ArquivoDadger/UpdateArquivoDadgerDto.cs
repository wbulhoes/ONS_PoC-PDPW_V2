using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.ArquivoDadger;

/// <summary>
/// DTO para atualização de Arquivo DADGER
/// </summary>
public class UpdateArquivoDadgerDto
{
    [Required]
    [StringLength(500)]
    public string NomeArquivo { get; set; } = string.Empty;

    [Required]
    [StringLength(1000)]
    public string CaminhoArquivo { get; set; } = string.Empty;

    [Required]
    public DateTime DataImportacao { get; set; }

    [Required]
    public int SemanaPMOId { get; set; }

    public string? Observacoes { get; set; }

    public bool Processado { get; set; }

    public DateTime? DataProcessamento { get; set; }
}
