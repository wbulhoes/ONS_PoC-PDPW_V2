using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.JanelaEnvio;

public class JanelaEnvioAgenteDto
{
    public int Id { get; set; }
    public string TipoDado { get; set; } = string.Empty;
    public DateTime DataReferencia { get; set; }
    public DateTime DataHoraInicio { get; set; }
    public DateTime DataHoraFim { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? SemanaPMOId { get; set; }
    public string? SemanaPMO { get; set; }
    public string? Observacoes { get; set; }
    public bool PermiteEnvioForaJanela { get; set; }
    public string? UsuarioAutorizacaoExcecao { get; set; }
    public DateTime? DataHoraAutorizacaoExcecao { get; set; }
    public bool EstaAberta { get; set; }
    public bool PermiteEnvio { get; set; }
}

public class CreateJanelaEnvioDto
{
    [Required(ErrorMessage = "Tipo de dado é obrigatório")]
    [StringLength(100)]
    public string TipoDado { get; set; } = string.Empty;

    [Required(ErrorMessage = "Data de referência é obrigatória")]
    public DateTime DataReferencia { get; set; }

    [Required(ErrorMessage = "Data/hora de início é obrigatória")]
    public DateTime DataHoraInicio { get; set; }

    [Required(ErrorMessage = "Data/hora de fim é obrigatória")]
    public DateTime DataHoraFim { get; set; }

    public int? SemanaPMOId { get; set; }

    [StringLength(500)]
    public string? Observacoes { get; set; }
}

public class AutorizarExcecaoDto
{
    [Required(ErrorMessage = "Usuário é obrigatório")]
    [StringLength(100)]
    public string Usuario { get; set; } = string.Empty;

    [Required(ErrorMessage = "Observação/Justificativa é obrigatória")]
    [StringLength(500)]
    public string Observacao { get; set; } = string.Empty;
}
