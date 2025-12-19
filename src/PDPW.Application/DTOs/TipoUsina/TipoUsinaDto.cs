namespace PDPW.Application.DTOs.TipoUsina;

/// <summary>
/// DTO de leitura de Tipo de Usina
/// </summary>
public class TipoUsinaDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? FonteEnergia { get; set; }
    public int QuantidadeUsinas { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}
