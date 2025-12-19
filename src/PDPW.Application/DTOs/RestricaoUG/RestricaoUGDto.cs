namespace PDPW.Application.DTOs.RestricaoUG;

/// <summary>
/// DTO de leitura de Restrição de Unidade Geradora
/// </summary>
public class RestricaoUGDto
{
    public int Id { get; set; }
    public int UnidadeGeradoraId { get; set; }
    public string UnidadeGeradora { get; set; } = string.Empty;
    public string CodigoUnidade { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public int MotivoRestricaoId { get; set; }
    public string MotivoRestricao { get; set; } = string.Empty;
    public string CategoriaMotivoRestricao { get; set; } = string.Empty;
    public decimal PotenciaRestrita { get; set; }
    public string? Observacoes { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
}
