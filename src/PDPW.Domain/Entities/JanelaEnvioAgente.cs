namespace PDPW.Domain.Entities;

/// <summary>
/// Representa o controle de janela temporal para envio de dados pelos agentes
/// Define quando cada tipo de dado pode ser submetido ao sistema
/// </summary>
public class JanelaEnvioAgente : BaseEntity
{
    /// <summary>
    /// Tipo de dado (DadosEnergeticos, Cargas, Restricoes, etc)
    /// </summary>
    public string TipoDado { get; set; } = string.Empty;
    
    /// <summary>
    /// Data de referência para o envio
    /// </summary>
    public DateTime DataReferencia { get; set; }
    
    /// <summary>
    /// Data/hora de início da janela de envio
    /// </summary>
    public DateTime DataHoraInicio { get; set; }
    
    /// <summary>
    /// Data/hora de fim da janela de envio
    /// </summary>
    public DateTime DataHoraFim { get; set; }
    
    /// <summary>
    /// Status da janela (Aberta, Fechada, Bloqueada)
    /// </summary>
    public string Status { get; set; } = "Aberta";
    
    /// <summary>
    /// ID da Semana PMO relacionada (opcional)
    /// </summary>
    public int? SemanaPMOId { get; set; }
    
    /// <summary>
    /// Semana PMO relacionada
    /// </summary>
    public virtual SemanaPMO? SemanaPMO { get; set; }
    
    /// <summary>
    /// Observações sobre a janela
    /// </summary>
    public string? Observacoes { get; set; }
    
    /// <summary>
    /// Indica se permite envio fora da janela (exceção)
    /// </summary>
    public bool PermiteEnvioForaJanela { get; set; } = false;
    
    /// <summary>
    /// Usuário que autorizou exceção
    /// </summary>
    public string? UsuarioAutorizacaoExcecao { get; set; }
    
    /// <summary>
    /// Data/hora da autorização de exceção
    /// </summary>
    public DateTime? DataHoraAutorizacaoExcecao { get; set; }
    
    /// <summary>
    /// Verifica se a janela está aberta no momento
    /// </summary>
    public bool EstaAberta()
    {
        var agora = DateTime.Now;
        return Status == "Aberta" && 
               agora >= DataHoraInicio && 
               agora <= DataHoraFim;
    }
    
    /// <summary>
    /// Verifica se permite envio (janela aberta ou exceção autorizada)
    /// </summary>
    public bool PermiteEnvio()
    {
        return EstaAberta() || PermiteEnvioForaJanela;
    }
}
