using PDPW.Application.DTOs.ParadaUG;

namespace PDPW.Application.Interfaces;

/// <summary>
/// Interface de serviço para Paradas de Unidades Geradoras
/// </summary>
public interface IParadaUGService
{
    Task<IEnumerable<ParadaUGDto>> GetAllAsync();
    Task<ParadaUGDto?> GetByIdAsync(int id);
    Task<IEnumerable<ParadaUGDto>> GetByUnidadeGeradoraAsync(int unidadeGeradoraId);
    Task<IEnumerable<ParadaUGDto>> GetProgramadasAsync();
    Task<IEnumerable<ParadaUGDto>> GetNaoProgramadasAsync();
    Task<IEnumerable<ParadaUGDto>> GetAtivasAsync(DateTime dataReferencia);
    Task<IEnumerable<ParadaUGDto>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<ParadaUGDto> CreateAsync(CreateParadaUGDto dto);
    Task<ParadaUGDto> UpdateAsync(int id, UpdateParadaUGDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
