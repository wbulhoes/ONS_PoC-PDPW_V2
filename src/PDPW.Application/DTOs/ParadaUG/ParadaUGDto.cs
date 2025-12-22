namespace PDPW.Application.DTOs.ParadaUG;

/// <summary>
/// DTO de leitura de Parada de Unidade Geradora
/// </summary>
public class ParadaUGDto
{
    public int Id { get; set; }
    public int UnidadeGeradoraId { get; set; }
    public string UnidadeGeradora { get; set; } = string.Empty;
    public string CodigoUnidade { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public string MotivoParada { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
    public bool Programada { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}
