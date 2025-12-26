namespace PDPW.Domain.Entities;

/// <summary>
/// Representa uma notificação do sistema para usuários/agentes
/// </summary>
public class Notificacao : BaseEntity
{
    /// <summary>
    /// ID do destinatário (Empresa ou Usuário)
    /// </summary>
    public int? DestinatarioId { get; set; }
    
    /// <summary>
    /// Tipo de destinatário (Empresa, Usuario, Sistema)
    /// </summary>
    public string TipoDestinatario { get; set; } = "Sistema";
    
    /// <summary>
    /// Tipo de notificação (Alerta, Aviso, Informacao, Vencimento)
    /// </summary>
    public string TipoNotificacao { get; set; } = "Informacao";
    
    /// <summary>
    /// Categoria (JanelaEnvio, Aprovacao, Sistema, etc)
    /// </summary>
    public string Categoria { get; set; } = string.Empty;
    
    /// <summary>
    /// Título da notificação
    /// </summary>
    public string Titulo { get; set; } = string.Empty;
    
    /// <summary>
    /// Mensagem completa
    /// </summary>
    public string Mensagem { get; set; } = string.Empty;
    
    /// <summary>
    /// Prioridade (Alta, Media, Baixa)
    /// </summary>
    public string Prioridade { get; set; } = "Media";
    
    /// <summary>
    /// Data/hora de envio da notificação
    /// </summary>
    public DateTime DataHoraEnvio { get; set; }
    
    /// <summary>
    /// Data/hora de leitura (null = não lida)
    /// </summary>
    public DateTime? DataHoraLeitura { get; set; }
    
    /// <summary>
    /// Flag indicando se foi lida
    /// </summary>
    public bool Lida { get; set; } = false;
    
    /// <summary>
    /// Data de vencimento/expiração da notificação
    /// </summary>
    public DateTime? DataVencimento { get; set; }
    
    /// <summary>
    /// Link relacionado (URL para ação)
    /// </summary>
    public string? LinkAcao { get; set; }
    
    /// <summary>
    /// Texto do botão de ação
    /// </summary>
    public string? TextoAcao { get; set; }
    
    /// <summary>
    /// ID da entidade relacionada
    /// </summary>
    public int? EntidadeRelacionadaId { get; set; }
    
    /// <summary>
    /// Tipo da entidade relacionada
    /// </summary>
    public string? TipoEntidadeRelacionada { get; set; }
    
    /// <summary>
    /// Marca a notificação como lida
    /// </summary>
    public void MarcarComoLida()
    {
        Lida = true;
        DataHoraLeitura = DateTime.Now;
    }
    
    /// <summary>
    /// Verifica se a notificação está vencida
    /// </summary>
    public bool EstaVencida()
    {
        return DataVencimento.HasValue && DataVencimento.Value < DateTime.Now;
    }
}
