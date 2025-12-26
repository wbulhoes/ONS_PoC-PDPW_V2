using PDPW.Application.DTOs.OfertaExportacao;
using PDPW.Domain.Common;

namespace PDPW.Application.Interfaces;

/// <summary>
/// Interface para serviço de Ofertas de Exportação de Energia Térmica
/// </summary>
public interface IOfertaExportacaoService
{
    /// <summary>
    /// Obtém todas as ofertas de exportação ativas
    /// </summary>
    /// <returns>Result com lista de ofertas ou erro</returns>
    Task<Result<IEnumerable<OfertaExportacaoDto>>> GetAllAsync();

    /// <summary>
    /// Obtém oferta de exportação por ID
    /// </summary>
    /// <param name="id">ID da oferta</param>
    /// <returns>Result com oferta encontrada ou erro NotFound</returns>
    Task<Result<OfertaExportacaoDto>> GetByIdAsync(int id);

    /// <summary>
    /// Obtém ofertas de exportação pendentes de análise do ONS
    /// </summary>
    /// <returns>Result com lista de ofertas pendentes</returns>
    Task<Result<IEnumerable<OfertaExportacaoDto>>> GetPendentesAsync();

    /// <summary>
    /// Obtém ofertas de exportação por usina
    /// </summary>
    /// <param name="usinaId">ID da usina</param>
    /// <returns>Result com lista de ofertas da usina</returns>
    Task<Result<IEnumerable<OfertaExportacaoDto>>> GetByUsinaAsync(int usinaId);

    /// <summary>
    /// Obtém ofertas de exportação por data PDP
    /// </summary>
    /// <param name="dataPDP">Data do Programa Diário de Produção</param>
    /// <returns>Result com lista de ofertas para a data especificada</returns>
    Task<Result<IEnumerable<OfertaExportacaoDto>>> GetByDataPDPAsync(DateTime dataPDP);

    /// <summary>
    /// Obtém ofertas de exportação por período
    /// </summary>
    /// <param name="dataInicio">Data inicial do período</param>
    /// <param name="dataFim">Data final do período</param>
    /// <returns>Result com lista de ofertas no período</returns>
    Task<Result<IEnumerable<OfertaExportacaoDto>>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);

    /// <summary>
    /// Obtém ofertas de exportação aprovadas
    /// </summary>
    /// <returns>Result com lista de ofertas aprovadas</returns>
    Task<Result<IEnumerable<OfertaExportacaoDto>>> GetAprovadasAsync();

    /// <summary>
    /// Obtém ofertas de exportação rejeitadas
    /// </summary>
    /// <returns>Result com lista de ofertas rejeitadas</returns>
    Task<Result<IEnumerable<OfertaExportacaoDto>>> GetRejeitadasAsync();

    /// <summary>
    /// Cria nova oferta de exportação
    /// </summary>
    /// <param name="createDto">Dados da nova oferta</param>
    /// <returns>Result com oferta criada ou erro de validação</returns>
    Task<Result<OfertaExportacaoDto>> CreateAsync(CreateOfertaExportacaoDto createDto);

    /// <summary>
    /// Atualiza oferta de exportação existente
    /// </summary>
    /// <param name="id">ID da oferta</param>
    /// <param name="updateDto">Dados atualizados</param>
    /// <returns>Result com oferta atualizada ou erro NotFound/validação</returns>
    Task<Result<OfertaExportacaoDto>> UpdateAsync(int id, UpdateOfertaExportacaoDto updateDto);

    /// <summary>
    /// Remove oferta de exportação (soft delete)
    /// </summary>
    /// <param name="id">ID da oferta</param>
    /// <returns>Result de sucesso ou erro NotFound</returns>
    Task<Result> DeleteAsync(int id);

    /// <summary>
    /// Aprova oferta de exportação (análise do ONS)
    /// </summary>
    /// <param name="id">ID da oferta</param>
    /// <param name="aprovarDto">Dados da aprovação</param>
    /// <returns>Result de sucesso ou erro NotFound/validação</returns>
    Task<Result> AprovarAsync(int id, AprovarOfertaExportacaoDto aprovarDto);

    /// <summary>
    /// Rejeita oferta de exportação (análise do ONS)
    /// </summary>
    /// <param name="id">ID da oferta</param>
    /// <param name="rejeitarDto">Dados da rejeição</param>
    /// <returns>Result de sucesso ou erro NotFound/validação</returns>
    Task<Result> RejeitarAsync(int id, RejeitarOfertaExportacaoDto rejeitarDto);

    /// <summary>
    /// Verifica se existe oferta de exportação pendente para uma data PDP
    /// </summary>
    /// <param name="dataPDP">Data do PDP</param>
    /// <returns>True se existe oferta pendente, False caso contrário</returns>
    Task<bool> ExisteOfertaPendenteAsync(DateTime dataPDP);

    /// <summary>
    /// Verifica se permite exclusão de ofertas (data PDP >= D+1)
    /// </summary>
    /// <param name="dataPDP">Data do PDP</param>
    /// <returns>True se permite exclusão, False caso contrário</returns>
    Task<bool> PermiteExclusaoAsync(DateTime dataPDP);
}
