namespace PDPW.Domain.Entities;

/// <summary>
/// Rampas de subida e descida de usinas térmicas
/// </summary>
public class RampasUsinaTermica : BaseEntity
{
    /// <summary>
    /// ID da usina
    /// </summary>
    public int UsinaId { get; set; }

    /// <summary>
    /// Usina térmica
    /// </summary>
    public Usina? Usina { get; set; }

    /// <summary>
    /// Rampa de subida em MW/min
    /// </summary>
    public decimal RampaSubida { get; set; }

    /// <summary>
    /// Rampa de descida em MW/min
    /// </summary>
    public decimal RampaDescida { get; set; }

    /// <summary>
    /// Tempo de partida em minutos
    /// </summary>
    public decimal TempoPartida { get; set; }

    /// <summary>
    /// Tempo de parada em minutos
    /// </summary>
    public decimal TempoParada { get; set; }
}
