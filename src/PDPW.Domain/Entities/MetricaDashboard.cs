namespace PDPW.Domain.Entities;

/// <summary>
/// Snapshot de métricas do sistema para dashboard
/// Armazena estatísticas em momentos específicos
/// </summary>
public class MetricaDashboard : BaseEntity
{
    /// <summary>
    /// Data/hora de referência do snapshot
    /// </summary>
    public DateTime DataHoraReferencia { get; set; }
    
    /// <summary>
    /// Categoria da métrica (Geral, Usina, Empresa, Sistema)
    /// </summary>
    public string Categoria { get; set; } = "Geral";
    
    /// <summary>
    /// Nome da métrica
    /// </summary>
    public string NomeMetrica { get; set; } = string.Empty;
    
    /// <summary>
    /// Valor da métrica
    /// </summary>
    public decimal Valor { get; set; }
    
    /// <summary>
    /// Unidade de medida (%, MW, quantidade, etc)
    /// </summary>
    public string Unidade { get; set; } = string.Empty;
    
    /// <summary>
    /// ID da entidade relacionada (opcional)
    /// </summary>
    public int? EntidadeId { get; set; }
    
    /// <summary>
    /// Tipo da entidade relacionada
    /// </summary>
    public string? TipoEntidade { get; set; }
    
    /// <summary>
    /// Descrição adicional
    /// </summary>
    public string? Descricao { get; set; }
    
    /// <summary>
    /// Meta/alvo esperado
    /// </summary>
    public decimal? Meta { get; set; }
    
    /// <summary>
    /// Percentual de atingimento da meta
    /// </summary>
    public decimal? PercentualMeta { get; set; }
    
    /// <summary>
    /// Tendência (Subindo, Descendo, Estavel)
    /// </summary>
    public string? Tendencia { get; set; }
    
    /// <summary>
    /// Status (OK, Alerta, Critico)
    /// </summary>
    public string Status { get; set; } = "OK";
    
    /// <summary>
    /// Calcula percentual de atingimento da meta
    /// </summary>
    public void CalcularPercentualMeta()
    {
        if (Meta.HasValue && Meta.Value != 0)
        {
            PercentualMeta = (Valor / Meta.Value) * 100;
        }
    }
}
