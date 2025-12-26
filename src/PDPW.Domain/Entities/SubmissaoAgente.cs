namespace PDPW.Domain.Entities;

/// <summary>
/// Registra cada submissão de dados realizada por um agente
/// Permite auditoria completa de envios
/// </summary>
public class SubmissaoAgente : BaseEntity
{
    /// <summary>
    /// ID da Empresa (Agente) que enviou
    /// </summary>
    public int EmpresaId { get; set; }
    
    /// <summary>
    /// Empresa relacionada
    /// </summary>
    public virtual Empresa? Empresa { get; set; }
    
    /// <summary>
    /// Tipo de dado enviado
    /// </summary>
    public string TipoDado { get; set; } = string.Empty;
    
    /// <summary>
    /// ID do registro enviado (referência genérica)
    /// </summary>
    public int RegistroId { get; set; }
    
    /// <summary>
    /// Data de referência dos dados
    /// </summary>
    public DateTime DataReferencia { get; set; }
    
    /// <summary>
    /// Data/hora da submissão
    /// </summary>
    public DateTime DataHoraSubmissao { get; set; }
    
    /// <summary>
    /// Usuário que realizou o envio
    /// </summary>
    public string UsuarioEnvio { get; set; } = string.Empty;
    
    /// <summary>
    /// IP de origem do envio
    /// </summary>
    public string? IpOrigem { get; set; }
    
    /// <summary>
    /// Indica se o envio foi dentro da janela permitida
    /// </summary>
    public bool DentroJanela { get; set; }
    
    /// <summary>
    /// ID da Janela de Envio relacionada
    /// </summary>
    public int? JanelaEnvioId { get; set; }
    
    /// <summary>
    /// Janela de Envio relacionada
    /// </summary>
    public virtual JanelaEnvioAgente? JanelaEnvio { get; set; }
    
    /// <summary>
    /// Status da submissão (Aceita, Rejeitada, EmAnalise)
    /// </summary>
    public string StatusSubmissao { get; set; } = "Aceita";
    
    /// <summary>
    /// Motivo de rejeição (se aplicável)
    /// </summary>
    public string? MotivoRejeicao { get; set; }
    
    /// <summary>
    /// Hash MD5 dos dados enviados (para controle de duplicidade)
    /// </summary>
    public string? HashDados { get; set; }
    
    /// <summary>
    /// Observações sobre a submissão
    /// </summary>
    public string? Observacoes { get; set; }
}
