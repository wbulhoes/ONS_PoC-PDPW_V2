namespace PDPW.Domain.Entities;

/// <summary>
/// Representa uma oferta de exportação de energia térmica
/// </summary>
public class OfertaExportacao : BaseEntity
{
    /// <summary>
    /// ID da Usina que faz a oferta
    /// </summary>
    public int UsinaId { get; set; }
    
    /// <summary>
    /// Usina relacionada à oferta
    /// </summary>
    public virtual Usina? Usina { get; set; }
    
    /// <summary>
    /// Data da oferta de exportação
    /// </summary>
    public DateTime DataOferta { get; set; }
    
    /// <summary>
    /// Data do Programa Diário de Produção (PDP)
    /// </summary>
    public DateTime DataPDP { get; set; }
    
    /// <summary>
    /// Valor da oferta em MW
    /// </summary>
    public decimal ValorMW { get; set; }
    
    /// <summary>
    /// Preço da oferta em R$/MWh
    /// </summary>
    public decimal PrecoMWh { get; set; }
    
    /// <summary>
    /// Hora inicial da oferta
    /// </summary>
    public TimeSpan HoraInicial { get; set; }
    
    /// <summary>
    /// Hora final da oferta
    /// </summary>
    public TimeSpan HoraFinal { get; set; }
    
    /// <summary>
    /// Flag indicando se a oferta foi aprovada pelo ONS
    /// null = pendente, true = aprovada, false = rejeitada
    /// </summary>
    public bool? FlgAprovadoONS { get; set; }
    
    /// <summary>
    /// Data de análise da oferta pelo ONS
    /// </summary>
    public DateTime? DataAnaliseONS { get; set; }
    
    /// <summary>
    /// Usuário que analisou a oferta no ONS
    /// </summary>
    public string? UsuarioAnaliseONS { get; set; }
    
    /// <summary>
    /// Observações sobre a análise do ONS
    /// </summary>
    public string? ObservacaoONS { get; set; }
    
    /// <summary>
    /// Observações gerais da oferta
    /// </summary>
    public string? Observacoes { get; set; }
    
    /// <summary>
    /// ID da Semana PMO relacionada
    /// </summary>
    public int? SemanaPMOId { get; set; }
    
    /// <summary>
    /// Semana PMO relacionada
    /// </summary>
    public virtual SemanaPMO? SemanaPMO { get; set; }
}
