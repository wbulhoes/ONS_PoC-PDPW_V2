namespace PDPW.Application.DTOs.OfertaExportacao;

/// <summary>
/// DTO de leitura de Oferta de Exportação
/// </summary>
public class OfertaExportacaoDto
{
    public int Id { get; set; }
    public int UsinaId { get; set; }
    public string UsinaNome { get; set; } = string.Empty;
    public string EmpresaNome { get; set; } = string.Empty;
    public DateTime DataOferta { get; set; }
    public DateTime DataPDP { get; set; }
    public decimal ValorMW { get; set; }
    public decimal PrecoMWh { get; set; }
    public TimeSpan HoraInicial { get; set; }
    public TimeSpan HoraFinal { get; set; }
    public bool? FlgAprovadoONS { get; set; }
    public string StatusAnalise => FlgAprovadoONS == null ? "Pendente" : 
                                   FlgAprovadoONS == true ? "Aprovada" : "Rejeitada";
    public DateTime? DataAnaliseONS { get; set; }
    public string? UsuarioAnaliseONS { get; set; }
    public string? ObservacaoONS { get; set; }
    public string? Observacoes { get; set; }
    public int? SemanaPMOId { get; set; }
    public string? SemanaPMO { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}
