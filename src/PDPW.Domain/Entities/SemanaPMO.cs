namespace PDPW.Domain.Entities;

/// <summary>
/// Semana do PMO (Programa Mensal de Operação) utilizada no planejamento energético do SIN.
/// Representa o período de referência para programação e despacho de energia.
/// </summary>
/// <remarks>
/// Nomenclatura ubíqua: SemanaProgramaMensalOperacao (mantido como SemanaPMO por ser sigla amplamente conhecida)
/// O PMO é elaborado pelo ONS e define o planejamento mensal da operação do Sistema Interligado Nacional.
/// Cada mês é dividido em 4-5 semanas operativas (sábado a sexta-feira).
/// </remarks>
public class SemanaPMO : BaseEntity
{
    /// <summary>
    /// Número sequencial da semana dentro do PMO mensal
    /// </summary>
    /// <example>1, 2, 3, 4, 5</example>
    public int Numero { get; set; }

    /// <summary>
    /// Data de início da semana operativa (geralmente sábado 00:00)
    /// </summary>
    public DateTime DataInicio { get; set; }

    /// <summary>
    /// Data de fim da semana operativa (geralmente sexta-feira 23:59)
    /// </summary>
    public DateTime DataFim { get; set; }

    /// <summary>
    /// Ano de referência do PMO
    /// </summary>
    public int Ano { get; set; }

    /// <summary>
    /// Mês de referência do PMO
    /// </summary>
    public int Mes { get; set; }

    /// <summary>
    /// Observações operacionais sobre a semana
    /// </summary>
    /// <remarks>
    /// Pode conter informações sobre eventos especiais, restrições previstas, etc.
    /// </remarks>
    public string? Observacoes { get; set; }

    // Relacionamentos
    /// <summary>
    /// Arquivos DADGER (Dados Gerais) associados a esta semana
    /// </summary>
    public ICollection<ArquivoDadger>? ArquivosDadger { get; set; }
    
    /// <summary>
    /// Declarações de Carga Agregada (DCA) da semana
    /// </summary>
    public ICollection<DCA>? DCAs { get; set; }
    
    /// <summary>
    /// Declarações de Carga Revisada (DCR) da semana
    /// </summary>
    public ICollection<DCR>? DCRs { get; set; }
}
