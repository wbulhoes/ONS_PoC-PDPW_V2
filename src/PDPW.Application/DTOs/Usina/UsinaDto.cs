namespace PDPW.Application.DTOs.Usina;

/// <summary>
/// DTO de leitura de Usina
/// </summary>
public class UsinaDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public int TipoUsinaId { get; set; }
    public string TipoUsina { get; set; } = string.Empty;
    public int EmpresaId { get; set; }
    public string Empresa { get; set; } = string.Empty;
    public decimal CapacidadeInstalada { get; set; }
    public string? Localizacao { get; set; }
    public DateTime DataOperacao { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}
