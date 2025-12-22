using PDPW.Application.DTOs.MotivoRestricao;

namespace PDPW.Application.Interfaces;

/// <summary>
/// Interface de serviço para Motivos de Restrição
/// </summary>
public interface IMotivoRestricaoService
{
    Task<IEnumerable<MotivoRestricaoDto>> GetAllAsync();
    Task<MotivoRestricaoDto?> GetByIdAsync(int id);
    Task<IEnumerable<MotivoRestricaoDto>> GetByCategoriaAsync(string categoria);
    Task<IEnumerable<MotivoRestricaoDto>> GetAtivasAsync();
    Task<MotivoRestricaoDto> CreateAsync(CreateMotivoRestricaoDto dto);
    Task<MotivoRestricaoDto> UpdateAsync(int id, UpdateMotivoRestricaoDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
