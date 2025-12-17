namespace PDPW.Application.DTOs;

/// <summary>
/// DTO para transferência de dados energéticos
/// </summary>
public class DadoEnergeticoDto
{
    public int Id { get; set; }
    public DateTime DataReferencia { get; set; }
    public string CodigoUsina { get; set; } = string.Empty;
    public decimal ProducaoMWh { get; set; }
    public decimal CapacidadeDisponivel { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
}

public class CriarDadoEnergeticoDto
{
    public DateTime DataReferencia { get; set; }
    public string CodigoUsina { get; set; } = string.Empty;
    public decimal ProducaoMWh { get; set; }
    public decimal CapacidadeDisponivel { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
}

public class AtualizarDadoEnergeticoDto
{
    public DateTime DataReferencia { get; set; }
    public string CodigoUsina { get; set; } = string.Empty;
    public decimal ProducaoMWh { get; set; }
    public decimal CapacidadeDisponivel { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
}
