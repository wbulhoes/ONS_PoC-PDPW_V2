using PDPW.Application.DTOs.Balanco;

namespace PDPW.Application.Interfaces;

/// <summary>
/// Interface de serviço para Balanços Energéticos
/// </summary>
public interface IBalancoService
{
    Task<IEnumerable<BalancoDto>> GetAllAsync();
    Task<BalancoDto?> GetByIdAsync(int id);
    Task<IEnumerable<BalancoDto>> GetBySubsistemaAsync(string subsistemaId);
    Task<IEnumerable<BalancoDto>> GetByDataAsync(DateTime dataReferencia);
    Task<IEnumerable<BalancoDto>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim);
    Task<BalancoDto?> GetBySubsistemaDataAsync(string subsistemaId, DateTime dataReferencia);
    Task<BalancoDto> CreateAsync(CreateBalancoDto dto);
    Task<BalancoDto> UpdateAsync(int id, UpdateBalancoDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
