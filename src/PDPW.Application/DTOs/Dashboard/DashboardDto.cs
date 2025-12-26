namespace PDPW.Application.DTOs.Dashboard;

/// <summary>
/// DTO com resumo geral do sistema
/// </summary>
public class DashboardResumoDto
{
    public DateTime DataHoraAtualizacao { get; set; }
    
    // Métricas de Ofertas
    public int TotalOfertasExportacao { get; set; }
    public int OfertasExportacaoPendentes { get; set; }
    public int OfertasExportacaoAprovadas { get; set; }
    public decimal TotalMWOfertado { get; set; }
    
    public int TotalOfertasRespostaVoluntaria { get; set; }
    public int OfertasRespostaVoluntariaPendentes { get; set; }
    public decimal TotalReducaoDemandaMW { get; set; }
    
    // Métricas de Programação
    public int TotalArquivosDadger { get; set; }
    public int ArquivosEmAnalise { get; set; }
    public int ArquivosAprovados { get; set; }
    public int ArquivosAbertos { get; set; }
    
    // Métricas de Usinas
    public int TotalUsinas { get; set; }
    public int UsinasEolicas { get; set; }
    public decimal CapacidadeTotalInstalada { get; set; }
    
    // Previsões Eólicas
    public int TotalPrevisoesEolicas { get; set; }
    public decimal MAEMedio { get; set; }
    public decimal RMSEMedio { get; set; }
    
    // Submissões de Agentes
    public int SubmissoesHoje { get; set; }
    public int SubmissoesForaJanela { get; set; }
    public int JanelasAbertas { get; set; }
    
    // Notificações
    public int NotificacoesNaoLidas { get; set; }
    public int AlertasAlta { get; set; }
    
    // Empresas/Agentes
    public int TotalEmpresas { get; set; }
    public int EmpresasAtivas { get; set; }
}

/// <summary>
/// DTO com métricas detalhadas por categoria
/// </summary>
public class MetricaCategoriaDto
{
    public string Categoria { get; set; } = string.Empty;
    public string NomeMetrica { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string Unidade { get; set; } = string.Empty;
    public decimal? Meta { get; set; }
    public decimal? PercentualMeta { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Tendencia { get; set; }
}

/// <summary>
/// DTO com alertas do sistema
/// </summary>
public class AlertaSistemaDto
{
    public string Tipo { get; set; } = string.Empty;
    public string Prioridade { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public DateTime DataHora { get; set; }
    public string? Acao { get; set; }
}
