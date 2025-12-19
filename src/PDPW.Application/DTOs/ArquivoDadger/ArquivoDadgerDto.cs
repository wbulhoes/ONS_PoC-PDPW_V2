namespace PDPW.Application.DTOs.ArquivoDadger;

/// <summary>
/// DTO de leitura de Arquivo DADGER
/// </summary>
public class ArquivoDadgerDto
{
    public int Id { get; set; }
    public string NomeArquivo { get; set; } = string.Empty;
    public string CaminhoArquivo { get; set; } = string.Empty;
    public DateTime DataImportacao { get; set; }
    public int SemanaPMOId { get; set; }
    public string SemanaPMO { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
    public bool Processado { get; set; }
    public DateTime? DataProcessamento { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
}
