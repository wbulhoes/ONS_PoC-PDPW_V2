namespace PDPW.Domain.Entities;

/// <summary>
/// Representa a previsão de geração eólica para uma usina
/// Dados utilizados para programação energética
/// </summary>
public class PrevisaoEolica : BaseEntity
{
    /// <summary>
    /// ID da Usina eólica
    /// </summary>
    public int UsinaId { get; set; }
    
    /// <summary>
    /// Usina relacionada
    /// </summary>
    public virtual Usina? Usina { get; set; }
    
    /// <summary>
    /// Data/hora de referência da previsão
    /// </summary>
    public DateTime DataHoraReferencia { get; set; }
    
    /// <summary>
    /// Data/hora prevista (horizonte de previsão)
    /// </summary>
    public DateTime DataHoraPrevista { get; set; }
    
    /// <summary>
    /// Geração prevista em MW médio
    /// </summary>
    public decimal GeracaoPrevistaMWmed { get; set; }
    
    /// <summary>
    /// Velocidade do vento prevista em m/s
    /// </summary>
    public decimal? VelocidadeVentoMS { get; set; }
    
    /// <summary>
    /// Direção do vento em graus (0-360)
    /// </summary>
    public decimal? DirecaoVentoGraus { get; set; }
    
    /// <summary>
    /// Temperatura prevista em °C
    /// </summary>
    public decimal? TemperaturaC { get; set; }
    
    /// <summary>
    /// Pressão atmosférica em hPa
    /// </summary>
    public decimal? PressaoAtmosfericaHPa { get; set; }
    
    /// <summary>
    /// Umidade relativa do ar em %
    /// </summary>
    public decimal? UmidadeRelativa { get; set; }
    
    /// <summary>
    /// Modelo de previsão utilizado
    /// Ex: NWP (Numerical Weather Prediction), WRF, GFS, etc
    /// </summary>
    public string ModeloPrevisao { get; set; } = string.Empty;
    
    /// <summary>
    /// Versão do modelo
    /// </summary>
    public string? VersaoModelo { get; set; }
    
    /// <summary>
    /// Horizonte de previsão em horas
    /// </summary>
    public int HorizontePrevisaoHoras { get; set; }
    
    /// <summary>
    /// Incerteza da previsão em % (intervalo de confiança)
    /// </summary>
    public decimal? IncertezaPercentual { get; set; }
    
    /// <summary>
    /// Limite inferior da previsão (P10)
    /// </summary>
    public decimal? LimiteInferiorMW { get; set; }
    
    /// <summary>
    /// Limite superior da previsão (P90)
    /// </summary>
    public decimal? LimiteSuperiorMW { get; set; }
    
    /// <summary>
    /// Tipo de previsão (Curto Prazo, Médio Prazo, Longo Prazo)
    /// </summary>
    public string TipoPrevisao { get; set; } = "Curto Prazo";
    
    /// <summary>
    /// ID da Semana PMO relacionada (opcional)
    /// </summary>
    public int? SemanaPMOId { get; set; }
    
    /// <summary>
    /// Semana PMO relacionada
    /// </summary>
    public virtual SemanaPMO? SemanaPMO { get; set; }
    
    /// <summary>
    /// Geração real verificada (para comparação com previsão)
    /// </summary>
    public decimal? GeracaoRealMWmed { get; set; }
    
    /// <summary>
    /// Erro absoluto da previsão (Real - Previsto)
    /// </summary>
    public decimal? ErroAbsolutoMW { get; set; }
    
    /// <summary>
    /// Erro percentual da previsão
    /// </summary>
    public decimal? ErroPercentual { get; set; }
    
    /// <summary>
    /// Observações sobre a previsão
    /// </summary>
    public string? Observacoes { get; set; }
    
    /// <summary>
    /// Calcula o erro quando a geração real é informada
    /// </summary>
    public void CalcularErro()
    {
        if (GeracaoRealMWmed.HasValue)
        {
            ErroAbsolutoMW = GeracaoRealMWmed.Value - GeracaoPrevistaMWmed;
            
            if (GeracaoPrevistaMWmed != 0)
            {
                ErroPercentual = (ErroAbsolutoMW / GeracaoPrevistaMWmed) * 100;
            }
        }
    }
}
