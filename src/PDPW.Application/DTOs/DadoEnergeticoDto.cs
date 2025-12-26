using System.ComponentModel.DataAnnotations;

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
    [Required(ErrorMessage = "Data de referência é obrigatória")]
    public DateTime DataReferencia { get; set; }

    [Required(ErrorMessage = "Código da usina é obrigatório")]
    [StringLength(50, ErrorMessage = "Código da usina deve ter no máximo 50 caracteres")]
    public string CodigoUsina { get; set; } = string.Empty;

    [Required(ErrorMessage = "Produção em MWh é obrigatória")]
    [Range(0, double.MaxValue, ErrorMessage = "Produção deve ser um valor positivo")]
    public decimal ProducaoMWh { get; set; }

    [Required(ErrorMessage = "Capacidade disponível é obrigatória")]
    [Range(0, double.MaxValue, ErrorMessage = "Capacidade disponível deve ser um valor positivo")]
    public decimal CapacidadeDisponivel { get; set; }

    [Required(ErrorMessage = "Status é obrigatório")]
    [StringLength(50, ErrorMessage = "Status deve ter no máximo 50 caracteres")]
    public string Status { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
    public string? Observacoes { get; set; }
}

public class AtualizarDadoEnergeticoDto
{
    [Required(ErrorMessage = "Data de referência é obrigatória")]
    public DateTime DataReferencia { get; set; }

    [Required(ErrorMessage = "Código da usina é obrigatório")]
    [StringLength(50, ErrorMessage = "Código da usina deve ter no máximo 50 caracteres")]
    public string CodigoUsina { get; set; } = string.Empty;

    [Required(ErrorMessage = "Produção em MWh é obrigatória")]
    [Range(0, double.MaxValue, ErrorMessage = "Produção deve ser um valor positivo")]
    public decimal ProducaoMWh { get; set; }

    [Required(ErrorMessage = "Capacidade disponível é obrigatória")]
    [Range(0, double.MaxValue, ErrorMessage = "Capacidade disponível deve ser um valor positivo")]
    public decimal CapacidadeDisponivel { get; set; }

    [Required(ErrorMessage = "Status é obrigatório")]
    [StringLength(50, ErrorMessage = "Status deve ter no máximo 50 caracteres")]
    public string Status { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Observações devem ter no máximo 500 caracteres")]
    public string? Observacoes { get; set; }
}
