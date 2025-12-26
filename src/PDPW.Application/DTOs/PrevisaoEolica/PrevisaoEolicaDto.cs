using System.ComponentModel.DataAnnotations;

namespace PDPW.Application.DTOs.PrevisaoEolica;

public class PrevisaoEolicaDto
{
    public int Id { get; set; }
    public int UsinaId { get; set; }
    public string UsinaNome { get; set; } = string.Empty;
    public DateTime DataHoraReferencia { get; set; }
    public DateTime DataHoraPrevista { get; set; }
    public decimal GeracaoPrevistaMWmed { get; set; }
    public decimal? VelocidadeVentoMS { get; set; }
    public decimal? DirecaoVentoGraus { get; set; }
    public decimal? TemperaturaC { get; set; }
    public decimal? PressaoAtmosfericaHPa { get; set; }
    public decimal? UmidadeRelativa { get; set; }
    public string ModeloPrevisao { get; set; } = string.Empty;
    public string? VersaoModelo { get; set; }
    public int HorizontePrevisaoHoras { get; set; }
    public decimal? IncertezaPercentual { get; set; }
    public decimal? LimiteInferiorMW { get; set; }
    public decimal? LimiteSuperiorMW { get; set; }
    public string TipoPrevisao { get; set; } = string.Empty;
    public int? SemanaPMOId { get; set; }
    public string? SemanaPMO { get; set; }
    public decimal? GeracaoRealMWmed { get; set; }
    public decimal? ErroAbsolutoMW { get; set; }
    public decimal? ErroPercentual { get; set; }
    public string? Observacoes { get; set; }
    public DateTime DataCriacao { get; set; }
}

public class CreatePrevisaoEolicaDto
{
    [Required(ErrorMessage = "ID da Usina é obrigatório")]
    public int UsinaId { get; set; }

    [Required(ErrorMessage = "Data/hora de referência é obrigatória")]
    public DateTime DataHoraReferencia { get; set; }

    [Required(ErrorMessage = "Data/hora prevista é obrigatória")]
    public DateTime DataHoraPrevista { get; set; }

    [Required(ErrorMessage = "Geração prevista é obrigatória")]
    [Range(0, double.MaxValue, ErrorMessage = "Geração prevista deve ser positiva")]
    public decimal GeracaoPrevistaMWmed { get; set; }

    [Range(0, 100, ErrorMessage = "Velocidade do vento deve estar entre 0 e 100 m/s")]
    public decimal? VelocidadeVentoMS { get; set; }

    [Range(0, 360, ErrorMessage = "Direção do vento deve estar entre 0 e 360 graus")]
    public decimal? DirecaoVentoGraus { get; set; }

    [Range(-50, 50, ErrorMessage = "Temperatura deve estar entre -50 e 50 °C")]
    public decimal? TemperaturaC { get; set; }

    [Range(800, 1100, ErrorMessage = "Pressão deve estar entre 800 e 1100 hPa")]
    public decimal? PressaoAtmosfericaHPa { get; set; }

    [Range(0, 100, ErrorMessage = "Umidade deve estar entre 0 e 100%")]
    public decimal? UmidadeRelativa { get; set; }

    [Required(ErrorMessage = "Modelo de previsão é obrigatório")]
    [StringLength(100)]
    public string ModeloPrevisao { get; set; } = string.Empty;

    [StringLength(50)]
    public string? VersaoModelo { get; set; }

    [Required(ErrorMessage = "Horizonte de previsão é obrigatório")]
    [Range(1, 720, ErrorMessage = "Horizonte deve estar entre 1 e 720 horas")]
    public int HorizontePrevisaoHoras { get; set; }

    [Range(0, 100, ErrorMessage = "Incerteza deve estar entre 0 e 100%")]
    public decimal? IncertezaPercentual { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? LimiteInferiorMW { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? LimiteSuperiorMW { get; set; }

    [Required]
    [StringLength(50)]
    public string TipoPrevisao { get; set; } = "Curto Prazo";

    public int? SemanaPMOId { get; set; }

    [StringLength(500)]
    public string? Observacoes { get; set; }
}

public class UpdatePrevisaoEolicaDto
{
    [Required]
    public int UsinaId { get; set; }

    [Required]
    public DateTime DataHoraReferencia { get; set; }

    [Required]
    public DateTime DataHoraPrevista { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal GeracaoPrevistaMWmed { get; set; }

    [Range(0, 100)]
    public decimal? VelocidadeVentoMS { get; set; }

    [Range(0, 360)]
    public decimal? DirecaoVentoGraus { get; set; }

    [Range(-50, 50)]
    public decimal? TemperaturaC { get; set; }

    [Range(800, 1100)]
    public decimal? PressaoAtmosfericaHPa { get; set; }

    [Range(0, 100)]
    public decimal? UmidadeRelativa { get; set; }

    [Required]
    [StringLength(100)]
    public string ModeloPrevisao { get; set; } = string.Empty;

    [StringLength(50)]
    public string? VersaoModelo { get; set; }

    [Required]
    [Range(1, 720)]
    public int HorizontePrevisaoHoras { get; set; }

    [Range(0, 100)]
    public decimal? IncertezaPercentual { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? LimiteInferiorMW { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? LimiteSuperiorMW { get; set; }

    [Required]
    [StringLength(50)]
    public string TipoPrevisao { get; set; } = "Curto Prazo";

    public int? SemanaPMOId { get; set; }

    [StringLength(500)]
    public string? Observacoes { get; set; }
}

public class AtualizarGeracaoRealDto
{
    [Required(ErrorMessage = "Geração real é obrigatória")]
    [Range(0, double.MaxValue, ErrorMessage = "Geração real deve ser positiva")]
    public decimal GeracaoRealMWmed { get; set; }
}

public class EstatisticasPrevisaoDto
{
    public int UsinaId { get; set; }
    public string UsinaNome { get; set; } = string.Empty;
    public int TotalPrevisoes { get; set; }
    public int PrevisoesComErro { get; set; }
    public decimal MAE { get; set; } // Mean Absolute Error
    public decimal RMSE { get; set; } // Root Mean Square Error
    public decimal ErroMedioPercentual { get; set; }
    public DateTime PeriodoInicio { get; set; }
    public DateTime PeriodoFim { get; set; }
}
