using PDPW.Application.DTOs.RestricaoUG;

namespace PDPW.Application.Interfaces;

/// <summary>
/// Interface do serviço de Restrição de Unidade Geradora
/// </summary>
public interface IRestricaoUGService
{
    Task<IEnumerable<RestricaoUGDto>> GetAllAsync();
    Task<RestricaoUGDto?> GetByIdAsync(int id);
    Task<RestricaoUGDto> CreateAsync(CreateRestricaoUGDto dto);
    Task<RestricaoUGDto> UpdateAsync(int id, UpdateRestricaoUGDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<RestricaoUGDto>> GetByUnidadeGeradoraAsync(int unidadeGeradoraId);
    Task<IEnumerable<RestricaoUGDto>> GetAtivasAsync(DateTime dataReferencia);
    Task<IEnumerable<RestricaoUGDto>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<IEnumerable<RestricaoUGDto>> GetByMotivoRestricaoAsync(int motivoRestricaoId);
}
