using PDPW.Application.DTOs.Intercambio;

namespace PDPW.Application.Interfaces;

/// <summary>
/// Interface de serviço para Intercâmbios de Energia
/// </summary>
public interface IIntercambioService
{
    Task<IEnumerable<IntercambioDto>> GetAllAsync();
    Task<IntercambioDto?> GetByIdAsync(int id);
    Task<IEnumerable<IntercambioDto>> GetBySubsistemaOrigemAsync(string subsistemaOrigem);
    Task<IEnumerable<IntercambioDto>> GetBySubsistemaDestinoAsync(string subsistemaDestino);
    Task<IEnumerable<IntercambioDto>> GetByDataAsync(DateTime dataReferencia);
    Task<IEnumerable<IntercambioDto>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<IntercambioDto?> GetBySubsistemasDataAsync(string subsistemaOrigem, string subsistemaDestino, DateTime dataReferencia);
    Task<IntercambioDto> CreateAsync(CreateIntercambioDto dto);
    Task<IntercambioDto> UpdateAsync(int id, UpdateIntercambioDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
