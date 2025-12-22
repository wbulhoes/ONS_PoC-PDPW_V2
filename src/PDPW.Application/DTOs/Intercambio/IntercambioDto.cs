namespace PDPW.Application.DTOs.Intercambio;

/// <summary>
/// DTO de leitura de Intercâmbio de Energia
/// </summary>
public class IntercambioDto
{
    public int Id { get; set; }
    public DateTime DataReferencia { get; set; }
    public string SubsistemaOrigem { get; set; } = string.Empty;
    public string SubsistemaDestino { get; set; } = string.Empty;
    public decimal EnergiaIntercambiada { get; set; }
    public string? Observacoes { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}
