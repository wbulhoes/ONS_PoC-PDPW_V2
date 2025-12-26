namespace PDPW.Domain.Entities;

/// <summary>
/// Representa uma oferta de resposta voluntária da demanda
/// Programa de redução voluntária de consumo de energia
/// </summary>
public class OfertaRespostaVoluntaria : BaseEntity
{
    /// <summary>
    /// ID da Empresa que faz a oferta
    /// </summary>
    public int EmpresaId { get; set; }
    
    /// <summary>
    /// Empresa relacionada à oferta
    /// </summary>
    public virtual Empresa? Empresa { get; set; }
    
    /// <summary>
    /// Data da oferta
    /// </summary>
    public DateTime DataOferta { get; set; }
    
    /// <summary>
    /// Data do Programa Diário de Produção (PDP)
    /// </summary>
    public DateTime DataPDP { get; set; }
    
    /// <summary>
    /// Hora inicial da redução
    /// </summary>
    public TimeSpan HoraInicial { get; set; }
    
    /// <summary>
    /// Hora final da redução
    /// </summary>
    public TimeSpan HoraFinal { get; set; }
    
    /// <summary>
    /// Redução de demanda ofertada em MW
    /// </summary>
    public decimal ReducaoDemandaMW { get; set; }
    
    /// <summary>
    /// Preço da redução em R$/MWh
    /// </summary>
    public decimal PrecoMWh { get; set; }
    
    /// <summary>
    /// Tipo de programa (Interruptível, Redução Voluntária, etc)
    /// </summary>
    public string TipoPrograma { get; set; } = string.Empty;
    
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
