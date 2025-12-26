using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Interface para repositório de Ofertas de Exportação
/// </summary>
public interface IOfertaExportacaoRepository
{
    /// <summary>
    /// Obtém todas as ofertas de exportação ativas
    /// </summary>
    Task<IEnumerable<OfertaExportacao>> GetAllAsync();

    /// <summary>
    /// Obtém oferta de exportação por ID
    /// </summary>
    Task<OfertaExportacao?> GetByIdAsync(int id);

    /// <summary>
    /// Obtém ofertas de exportação por usina
    /// </summary>
    Task<IEnumerable<OfertaExportacao>> GetByUsinaAsync(int usinaId);

    /// <summary>
    /// Obtém ofertas de exportação por data PDP
    /// </summary>
    Task<IEnumerable<OfertaExportacao>> GetByDataPDPAsync(DateTime dataPDP);

    /// <summary>
    /// Obtém ofertas de exportação pendentes de análise do ONS
    /// </summary>
    Task<IEnumerable<OfertaExportacao>> GetPendentesAnaliseONSAsync();

    /// <summary>
    /// Obtém ofertas de exportação pendentes para uma data PDP específica
    /// </summary>
    Task<IEnumerable<OfertaExportacao>> GetPendentesAnaliseONSByDataPDPAsync(DateTime dataPDP);

    /// <summary>
    /// Obtém ofertas de exportação por período
    /// </summary>
    Task<IEnumerable<OfertaExportacao>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);

    /// <summary>
    /// Obtém ofertas de exportação aprovadas
    /// </summary>
    Task<IEnumerable<OfertaExportacao>> GetAprovadasAsync();

    /// <summary>
    /// Obtém ofertas de exportação rejeitadas
    /// </summary>
    Task<IEnumerable<OfertaExportacao>> GetRejeitadasAsync();

    /// <summary>
    /// Adiciona nova oferta de exportação
    /// </summary>
    Task<OfertaExportacao> AddAsync(OfertaExportacao ofertaExportacao);

    /// <summary>
    /// Atualiza oferta de exportação existente
    /// </summary>
    Task UpdateAsync(OfertaExportacao ofertaExportacao);

    /// <summary>
    /// Remove oferta de exportação (soft delete)
    /// </summary>
    Task DeleteAsync(int id);

    /// <summary>
    /// Aprova oferta de exportação
    /// </summary>
    Task AprovarAsync(int id, string usuarioONS, string? observacao = null);

    /// <summary>
    /// Rejeita oferta de exportação
    /// </summary>
    Task RejeitarAsync(int id, string usuarioONS, string observacao);

    /// <summary>
    /// Verifica se existe oferta de exportação não analisada para uma data PDP
    /// </summary>
    Task<bool> ExisteOfertaPendenteAnaliseONSAsync(DateTime dataPDP);

    /// <summary>
    /// Verifica se permite exclusão de ofertas (data PDP >= D+1)
    /// </summary>
    Task<bool> PermiteExclusaoAsync(DateTime dataPDP);

    /// <summary>
    /// Verifica se oferta de exportação existe
    /// </summary>
    Task<bool> ExistsAsync(int id);
}
