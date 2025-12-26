namespace PDPW.Domain.Entities;

/// <summary>
/// Arquivo DADGER (Dados de Geração)
/// </summary>
public class ArquivoDadger : BaseEntity
{
    /// <summary>
    /// Nome do arquivo
    /// </summary>
    public string NomeArquivo { get; set; } = string.Empty;

    /// <summary>
    /// Caminho completo do arquivo no servidor
    /// </summary>
    public string CaminhoArquivo { get; set; } = string.Empty;

    /// <summary>
    /// Data de importação do arquivo
    /// </summary>
    public DateTime DataImportacao { get; set; }

    /// <summary>
    /// ID da semana PMO
    /// </summary>
    public int SemanaPMOId { get; set; }

    /// <summary>
    /// Semana PMO relacionada
    /// </summary>
    public SemanaPMO? SemanaPMO { get; set; }

    /// <summary>
    /// Observações sobre o arquivo
    /// </summary>
    public string? Observacoes { get; set; }

    /// <summary>
    /// Indica se o arquivo foi processado
    /// </summary>
    public bool Processado { get; set; }

    /// <summary>
    /// Data de processamento
    /// </summary>
    public DateTime? DataProcessamento { get; set; }

    /// <summary>
    /// Status da programação
    /// Valores: Aberto, EmAnalise, Aprovado, Finalizado
    /// </summary>
    public string Status { get; set; } = "Aberto";

    /// <summary>
    /// Data de finalização da programação
    /// </summary>
    public DateTime? DataFinalizacao { get; set; }

    /// <summary>
    /// Usuário que finalizou a programação
    /// </summary>
    public string? UsuarioFinalizacao { get; set; }

    /// <summary>
    /// Observações da finalização
    /// </summary>
    public string? ObservacaoFinalizacao { get; set; }

    /// <summary>
    /// Data de aprovação da programação
    /// </summary>
    public DateTime? DataAprovacao { get; set; }

    /// <summary>
    /// Usuário que aprovou a programação
    /// </summary>
    public string? UsuarioAprovacao { get; set; }

    /// <summary>
    /// Observações da aprovação
    /// </summary>
    public string? ObservacaoAprovacao { get; set; }

    // Relacionamentos
    public ICollection<ArquivoDadgerValor>? Valores { get; set; }
}
