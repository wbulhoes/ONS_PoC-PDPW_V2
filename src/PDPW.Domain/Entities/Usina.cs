namespace PDPW.Domain.Entities;

/// <summary>
/// Usina geradora de energia elétrica
/// </summary>
public class Usina : BaseEntity
{
    /// <summary>
    /// Código único da usina
    /// </summary>
    public string Codigo { get; set; } = string.Empty;

    /// <summary>
    /// Nome da usina
    /// </summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// ID do tipo de usina
    /// </summary>
    public int TipoUsinaId { get; set; }

    /// <summary>
    /// Tipo da usina
    /// </summary>
    public TipoUsina? TipoUsina { get; set; }

    /// <summary>
    /// ID da empresa responsável
    /// </summary>
    public int EmpresaId { get; set; }

    /// <summary>
    /// Empresa responsável
    /// </summary>
    public Empresa? Empresa { get; set; }

    /// <summary>
    /// Capacidade instalada em MW
    /// </summary>
    public decimal CapacidadeInstalada { get; set; }

    /// <summary>
    /// Localização geográfica
    /// </summary>
    public string? Localizacao { get; set; }

    /// <summary>
    /// Data de início da operação
    /// </summary>
    public DateTime DataOperacao { get; set; }

    // Relacionamentos
    public ICollection<UnidadeGeradora>? UnidadesGeradoras { get; set; }
    public ICollection<RestricaoUS>? Restricoes { get; set; }
    public ICollection<GerForaMerito>? GeracoesForaMerito { get; set; }
    public ICollection<InflexibilidadeContratada>? InflexibilidadesContratadas { get; set; }
    public ICollection<RampasUsinaTermica>? RampasTermicas { get; set; }
    public ICollection<UsinaConversora>? Conversoras { get; set; }
}
