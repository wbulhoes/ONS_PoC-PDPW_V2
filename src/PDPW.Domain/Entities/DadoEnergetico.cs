namespace PDPW.Domain.Entities;

/// <summary>
/// Representa os dados energéticos coletados pelo sistema PDPW
/// </summary>
public class DadoEnergetico : BaseEntity
{
    public DateTime DataReferencia { get; set; }
    public string? CodigoUsina { get; set; }
    public decimal ProducaoMWh { get; set; }
    public decimal CapacidadeDisponivel { get; set; }
    public string? Status { get; set; }
    public string? Observacoes { get; set; }
    
    /// <summary>
    /// Energia vertida (não turbinada) em MWh
    /// </summary>
    public decimal? EnergiaVertida { get; set; }
    
    /// <summary>
    /// Energia turbinável que não foi utilizada em MWh
    /// </summary>
    public decimal? EnergiaTurbinavelNaoUtilizada { get; set; }
    
    /// <summary>
    /// Motivo do vertimento de energia
    /// </summary>
    public string? MotivoVertimento { get; set; }
}
