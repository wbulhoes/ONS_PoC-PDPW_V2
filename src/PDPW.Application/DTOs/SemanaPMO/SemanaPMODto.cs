using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.SemanaPmo;

/// <summary>
/// DTO de leitura de Semana PMO
/// </summary>
public class SemanaPmoDto
{
    public int Id { get; set; }
    public int Numero { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public string? Observacoes { get; set; }
    public int QuantidadeArquivos { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}

/// <summary>
/// DTO para criação de Semana PMO
/// </summary>
public class CreateSemanaPmoDto
{
    [Required(ErrorMessage = "O número da semana é obrigatório")]
    [Range(1, 53, ErrorMessage = "O número da semana deve estar entre 1 e 53")]
    public int Numero { get; set; }

    [Required(ErrorMessage = "A data de início é obrigatória")]
    public DateTime DataInicio { get; set; }

    [Required(ErrorMessage = "A data de fim é obrigatória")]
    public DateTime DataFim { get; set; }

    [Required(ErrorMessage = "O ano é obrigatório")]
    [Range(2020, 2100, ErrorMessage = "O ano deve estar entre 2020 e 2100")]
    public int Ano { get; set; }

    [Required(ErrorMessage = "O mês é obrigatório")]
    [Range(1, 12, ErrorMessage = "O mês deve estar entre 1 e 12")]
    public int Mes { get; set; }

    [MaxLength(500, ErrorMessage = "As observações não podem ter mais de 500 caracteres")]
    public string? Observacoes { get; set; }
}

/// <summary>
/// DTO para atualização de Semana PMO
/// </summary>
public class UpdateSemanaPmoDto
{
    [Required(ErrorMessage = "O número da semana é obrigatório")]
    [Range(1, 53, ErrorMessage = "O número da semana deve estar entre 1 e 53")]
    public int Numero { get; set; }

    [Required(ErrorMessage = "A data de início é obrigatória")]
    public DateTime DataInicio { get; set; }

    [Required(ErrorMessage = "A data de fim é obrigatória")]
    public DateTime DataFim { get; set; }

    [Required(ErrorMessage = "O ano é obrigatório")]
    [Range(2020, 2100, ErrorMessage = "O ano deve estar entre 2020 e 2100")]
    public int Ano { get; set; }

    [Required(ErrorMessage = "O mês é obrigatório")]
    [Range(1, 12, ErrorMessage = "O mês deve estar entre 1 e 12")]
    public int Mes { get; set; }

    [MaxLength(500, ErrorMessage = "As observações não podem ter mais de 500 caracteres")]
    public string? Observacoes { get; set; }

    public bool Ativo { get; set; } = true;
}
