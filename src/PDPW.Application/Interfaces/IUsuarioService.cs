using PDPW.Application.DTOs.Usuario;

namespace PDPW.Application.Interfaces;

/// <summary>
/// Interface do serviço de Usuários
/// </summary>
public interface IUsuarioService
{
    Task<IEnumerable<UsuarioDto>> GetAllAsync();
    Task<UsuarioDto?> GetByIdAsync(int id);
    Task<IEnumerable<UsuarioDto>> GetByPerfilAsync(string perfil);
    Task<IEnumerable<UsuarioDto>> GetByEquipeAsync(int equipePdpId);
    Task<UsuarioDto?> GetByEmailAsync(string email);
    Task<UsuarioDto> CreateAsync(CreateUsuarioDto dto);
    Task<UsuarioDto> UpdateAsync(int id, UpdateUsuarioDto dto);
    Task<bool> DeleteAsync(int id);
}
