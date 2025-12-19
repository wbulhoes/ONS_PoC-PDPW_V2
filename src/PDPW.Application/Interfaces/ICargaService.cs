using PDPW.Application.DTOs.Carga;

namespace PDPW.Application.Interfaces;

/// <summary>
/// Interface do serviço de Carga
/// </summary>
public interface ICargaService
{
    Task<IEnumerable<CargaDto>> GetAllAsync();
    Task<CargaDto?> GetByIdAsync(int id);
    Task<CargaDto> CreateAsync(CreateCargaDto dto);
    Task<CargaDto> UpdateAsync(int id, UpdateCargaDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<CargaDto>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<IEnumerable<CargaDto>> GetBySubsistemaAsync(string subsistemaId);
    Task<IEnumerable<CargaDto>> GetByDataReferenciaAsync(DateTime dataReferencia);
}
