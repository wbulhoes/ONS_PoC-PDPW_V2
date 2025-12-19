namespace PDPW.Domain.Entities;

/// <summary>
/// Tipo de usina geradora de energia elétrica classificada por sua fonte primária.
/// </summary>
/// <remarks>
/// Nomenclatura ubíqua: TipoUsinaGeradora (mantido como TipoUsina por compatibilidade)
/// Classificação conforme nomenclatura ONS: UHE, UTE, EOL, UFV, PCH, CGH, UNE
/// </remarks>
public class TipoUsina : BaseEntity
{
    /// <summary>
    /// Nome oficial do tipo de usina conforme classificação ONS
    /// </summary>
    /// <example>UHE (Usina Hidrelétrica), UTE (Usina Termelétrica), EOL (Usina Eólica), UFV (Usina Fotovoltaica), UNE (Usina Nuclear)</example>
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Descrição detalhada do tipo de usina e suas características operacionais
    /// </summary>
    public string? Descricao { get; set; }

    /// <summary>
    /// Fonte de energia primária utilizada na geração
    /// </summary>
    /// <example>Água, Gás Natural, Carvão, Vento, Solar, Nuclear, Biomassa</example>
    public string? FonteEnergia { get; set; }

    // Relacionamentos
    /// <summary>
    /// Usinas geradoras deste tipo
    /// </summary>
    public ICollection<Usina>? Usinas { get; set; }
}
