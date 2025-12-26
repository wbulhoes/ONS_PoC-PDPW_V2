using PDPW.Application.DTOs.Dashboard;
using PDPW.Application.Interfaces;
using PDPW.Domain.Common;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IOfertaExportacaoRepository _ofertaExportacaoRepo;
    private readonly IOfertaRespostaVoluntariaRepository _ofertaRespostaVoluntariaRepo;
    private readonly IArquivoDadgerRepository _arquivoDadgerRepo;
    private readonly IUsinaRepository _usinaRepo;
    private readonly IPrevisaoEolicaRepository _previsaoEolicaRepo;
    private readonly ISubmissaoAgenteRepository _submissaoAgenteRepo;
    private readonly IJanelaEnvioAgenteRepository _janelaEnvioRepo;
    private readonly IEmpresaRepository _empresaRepo;

    public DashboardService(
        IOfertaExportacaoRepository ofertaExportacaoRepo,
        IOfertaRespostaVoluntariaRepository ofertaRespostaVoluntariaRepo,
        IArquivoDadgerRepository arquivoDadgerRepo,
        IUsinaRepository usinaRepo,
        IPrevisaoEolicaRepository previsaoEolicaRepo,
        ISubmissaoAgenteRepository submissaoAgenteRepo,
        IJanelaEnvioAgenteRepository janelaEnvioRepo,
        IEmpresaRepository empresaRepo)
    {
        _ofertaExportacaoRepo = ofertaExportacaoRepo;
        _ofertaRespostaVoluntariaRepo = ofertaRespostaVoluntariaRepo;
        _arquivoDadgerRepo = arquivoDadgerRepo;
        _usinaRepo = usinaRepo;
        _previsaoEolicaRepo = previsaoEolicaRepo;
        _submissaoAgenteRepo = submissaoAgenteRepo;
        _janelaEnvioRepo = janelaEnvioRepo;
        _empresaRepo = empresaRepo;
    }

    public async Task<Result<DashboardResumoDto>> GetResumoGeralAsync()
    {
        var hoje = DateTime.Today;
        var amanha = hoje.AddDays(1);

        // Ofertas Exportação
        var ofertasExportacao = await _ofertaExportacaoRepo.GetAllAsync();
        var ofertasResposta = await _ofertaRespostaVoluntariaRepo.GetAllAsync();
        
        // Arquivos DADGER
        var arquivos = await _arquivoDadgerRepo.GetAllAsync();
        
        // Usinas
        var usinas = await _usinaRepo.GetAllAsync();
        
        // Previsões
        var previsoes = await _previsaoEolicaRepo.GetAllAsync();
        
        // Submissões
        var submissoes = await _submissaoAgenteRepo.GetAllAsync();
        var submissoesHoje = submissoes.Where(s => s.DataHoraSubmissao >= hoje && s.DataHoraSubmissao < amanha);
        
        // Janelas
        var janelas = await _janelaEnvioRepo.GetAbertas();
        
        // Empresas
        var empresas = await _empresaRepo.GetAllAsync();

        var resumo = new DashboardResumoDto
        {
            DataHoraAtualizacao = DateTime.Now,
            
            // Ofertas de Exportação
            TotalOfertasExportacao = ofertasExportacao.Count(),
            OfertasExportacaoPendentes = ofertasExportacao.Count(o => o.FlgAprovadoONS == null),
            OfertasExportacaoAprovadas = ofertasExportacao.Count(o => o.FlgAprovadoONS == true),
            TotalMWOfertado = ofertasExportacao.Sum(o => o.ValorMW),
            
            // Ofertas Resposta Voluntária
            TotalOfertasRespostaVoluntaria = ofertasResposta.Count(),
            OfertasRespostaVoluntariaPendentes = ofertasResposta.Count(o => o.FlgAprovadoONS == null),
            TotalReducaoDemandaMW = ofertasResposta.Sum(o => o.ReducaoDemandaMW),
            
            // Arquivos DADGER
            TotalArquivosDadger = arquivos.Count(),
            ArquivosEmAnalise = arquivos.Count(a => a.Status == "EmAnalise"),
            ArquivosAprovados = arquivos.Count(a => a.Status == "Aprovado"),
            ArquivosAbertos = arquivos.Count(a => a.Status == "Aberto"),
            
            // Usinas
            TotalUsinas = usinas.Count(),
            UsinasEolicas = usinas.Count(u => u.TipoUsina != null && u.TipoUsina.Nome.Contains("Eólica")),
            CapacidadeTotalInstalada = usinas.Sum(u => u.CapacidadeInstalada),
            
            // Previsões Eólicas
            TotalPrevisoesEolicas = previsoes.Count(),
            
            // Submissões de Agentes
            SubmissoesHoje = submissoesHoje.Count(),
            SubmissoesForaJanela = submissoes.Count(s => !s.DentroJanela),
            JanelasAbertas = janelas.Count(),
            
            // Empresas
            TotalEmpresas = empresas.Count(),
            EmpresasAtivas = empresas.Count()
        };

        return Result<DashboardResumoDto>.Success(resumo);
    }

    public async Task<Result<IEnumerable<MetricaCategoriaDto>>> GetMetricasPorCategoriaAsync(string categoria)
    {
        var metricas = new List<MetricaCategoriaDto>();

        switch (categoria.ToLower())
        {
            case "ofertas":
                var ofertas = await _ofertaExportacaoRepo.GetAllAsync();
                var totalOfertas = ofertas.Count();
                var ofertasAprovadas = ofertas.Count(o => o.FlgAprovadoONS == true);
                
                metricas.Add(new MetricaCategoriaDto
                {
                    Categoria = "Ofertas",
                    NomeMetrica = "Taxa de Aprovação",
                    Valor = totalOfertas > 0 ? (decimal)ofertasAprovadas / totalOfertas * 100 : 0,
                    Unidade = "%",
                    Meta = 80,
                    Status = ofertasAprovadas * 100 / (totalOfertas > 0 ? totalOfertas : 1) >= 80 ? "OK" : "Alerta"
                });
                break;

            case "programacao":
                var arquivos = await _arquivoDadgerRepo.GetAllAsync();
                var totalArquivos = arquivos.Count();
                var arquivosAprovados = arquivos.Count(a => a.Status == "Aprovado");
                
                metricas.Add(new MetricaCategoriaDto
                {
                    Categoria = "Programação",
                    NomeMetrica = "Programações Aprovadas",
                    Valor = arquivosAprovados,
                    Unidade = "arquivos",
                    Status = "OK"
                });
                break;

            case "previsoes":
                var previsoes = await _previsaoEolicaRepo.GetAllAsync();
                var previsoesComErro = previsoes.Where(p => p.ErroAbsolutoMW.HasValue).ToList();
                
                if (previsoesComErro.Any())
                {
                    var mae = previsoesComErro.Average(p => Math.Abs(p.ErroAbsolutoMW!.Value));
                    
                    metricas.Add(new MetricaCategoriaDto
                    {
                        Categoria = "Previsões",
                        NomeMetrica = "MAE Médio",
                        Valor = mae,
                        Unidade = "MW",
                        Meta = 50,
                        Status = mae <= 50 ? "OK" : "Alerta"
                    });
                }
                break;
        }

        return Result<IEnumerable<MetricaCategoriaDto>>.Success(metricas);
    }

    public async Task<Result<IEnumerable<AlertaSistemaDto>>> GetAlertasAsync(string? prioridade = null)
    {
        var alertas = new List<AlertaSistemaDto>();

        // Alerta: Janelas prestes a fechar
        var janelasAbertas = await _janelaEnvioRepo.GetAbertas();
        var janelasProximasFechamento = janelasAbertas
            .Where(j => j.DataHoraFim <= DateTime.Now.AddHours(2))
            .ToList();

        foreach (var janela in janelasProximasFechamento)
        {
            alertas.Add(new AlertaSistemaDto
            {
                Tipo = "JanelaEnvio",
                Prioridade = "Alta",
                Mensagem = $"Janela de {janela.TipoDado} fecha em breve: {janela.DataHoraFim:dd/MM/yyyy HH:mm}",
                DataHora = DateTime.Now,
                Acao = "Verificar envios pendentes"
            });
        }

        // Alerta: Ofertas pendentes de análise há muito tempo
        var ofertas = await _ofertaExportacaoRepo.GetPendentesAnaliseONSAsync();
        var ofertasAntigas = ofertas.Count(o => o.DataOferta < DateTime.Now.AddDays(-3));

        if (ofertasAntigas > 0)
        {
            alertas.Add(new AlertaSistemaDto
            {
                Tipo = "OfertasPendentes",
                Prioridade = "Media",
                Mensagem = $"{ofertasAntigas} ofertas pendentes de análise há mais de 3 dias",
                DataHora = DateTime.Now,
                Acao = "Analisar ofertas pendentes"
            });
        }

        // Filtrar por prioridade se especificado
        if (!string.IsNullOrEmpty(prioridade))
        {
            alertas = alertas.Where(a => a.Prioridade.Equals(prioridade, StringComparison.OrdinalIgnoreCase))
                           .ToList();
        }

        return Result<IEnumerable<AlertaSistemaDto>>.Success(alertas);
    }
}
