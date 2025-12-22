using PDPW.Application.DTOs.UnidadeGeradora;

namespace PDPW.Application.Interfaces;

/// <summary>
/// Interface de serviço para Unidades Geradoras
/// </summary>
public interface IUnidadeGeradoraService
{
    Task<IEnumerable<UnidadeGeradoraDto>> GetAllAsync();
    Task<UnidadeGeradoraDto?> GetByIdAsync(int id);
    Task<UnidadeGeradoraDto?> GetByCodigoAsync(string codigo);
    Task<IEnumerable<UnidadeGeradoraDto>> GetByUsinaAsync(int usinaId);
    Task<IEnumerable<UnidadeGeradoraDto>> GetByStatusAsync(string status);
    Task<IEnumerable<UnidadeGeradoraDto>> GetAtivasAsync();
    Task<UnidadeGeradoraDto> CreateAsync(CreateUnidadeGeradoraDto dto);
    Task<UnidadeGeradoraDto> UpdateAsync(int id, UpdateUnidadeGeradoraDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> CodigoExistsAsync(string codigo, int? excludeId = null);
}
