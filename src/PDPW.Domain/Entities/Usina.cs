namespace PDPW.Domain.Entities;

/// <summary>
/// Usina Geradora de energia elétrica do SIN (Sistema Interligado Nacional).
/// Também conhecida como Central Geradora ou simplesmente Usina.
/// Representa uma instalação destinada à produção de energia elétrica.
/// </summary>
/// <remarks>
/// Nomenclatura ubíqua: UsinaGeradora
/// Entidade central do sistema PDPW (Programação Diária de Produção).
/// Relacionada a um AgenteSetorEletrico (Empresa) e possui um tipo específico.
/// </remarks>
public class Usina : BaseEntity
{
    /// <summary>
    /// Código único da usina geradora no sistema ONS
    /// </summary>
    /// <example>UTE001, UHE045, EOL123</example>
    public string Codigo { get; set; } = string.Empty;

    /// <summary>
    /// Nome oficial da usina geradora
    /// </summary>
    /// <example>Usina Termelétrica de Angra 1</example>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// ID do tipo de usina geradora (UHE, UTE, EOL, UFV, etc)
    /// </summary>
    public int TipoUsinaId { get; set; }

    /// <summary>
    /// Tipo da usina geradora (navegação)
    /// </summary>
    public TipoUsina? TipoUsina { get; set; }

    /// <summary>
    /// ID do agente do setor elétrico responsável pela usina
    /// </summary>
    /// <remarks>
    /// Nomenclatura ubíqua: AgenteSetorEletricoId (mantido como EmpresaId por compatibilidade)
    /// </remarks>
    public int EmpresaId { get; set; }

    /// <summary>
    /// Agente do setor elétrico responsável pela operação da usina (navegação)
    /// </summary>
    /// <remarks>
    /// Nomenclatura ubíqua: AgenteSetorEletrico (mantido como Empresa por compatibilidade)
    /// </remarks>
    public Empresa? Empresa { get; set; }

    /// <summary>
    /// Potência instalada (capacidade máxima) da usina em MW (megawatts)
    /// </summary>
    public decimal CapacidadeInstalada { get; set; }

    /// <summary>
    /// Localização geográfica da usina geradora
    /// </summary>
    /// <example>Angra dos Reis, RJ</example>
    public string? Localizacao { get; set; }

    /// <summary>
    /// Data de início da operação comercial da usina geradora
    /// </summary>
    public DateTime DataOperacao { get; set; }

    // Relacionamentos
    /// <summary>
    /// Unidades geradoras (UGs) da usina
    /// </summary>
    /// <remarks>
    /// Uma usina pode ter múltiplas unidades geradoras (turbinas + geradores)
    /// </remarks>
    public ICollection<UnidadeGeradora>? UnidadesGeradoras { get; set; }
    
    /// <summary>
    /// Restrições operacionais da usina
    /// </summary>
    public ICollection<RestricaoUS>? Restricoes { get; set; }
    
    /// <summary>
    /// Gerações fora da ordem de mérito (despacho não econômico)
    /// </summary>
    public ICollection<GerForaMerito>? GeracoesForaMerito { get; set; }
    
    /// <summary>
    /// Inflexibilidades contratuais da usina (geração mínima obrigatória)
    /// </summary>
    public ICollection<InflexibilidadeContratada>? InflexibilidadesContratadas { get; set; }
    
    /// <summary>
    /// Rampas operacionais (taxas de variação de geração) para usinas térmicas
    /// </summary>
    public ICollection<RampasUsinaTermica>? RampasTermicas { get; set; }
    
    /// <summary>
    /// Configurações de conversão de energia (ex: solar + bateria)
    /// </summary>
    public ICollection<UsinaConversora>? Conversoras { get; set; }
}
