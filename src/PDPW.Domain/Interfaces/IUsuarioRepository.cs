using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Interface do repositório de Usuários
/// </summary>
public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> GetAllAsync();
    Task<Usuario?> GetByIdAsync(int id);
    Task<IEnumerable<Usuario>> GetByPerfilAsync(string perfil);
    Task<IEnumerable<Usuario>> GetByEquipeAsync(int equipePdpId);
    Task<Usuario?> GetByEmailAsync(string email);
    Task<Usuario> CreateAsync(Usuario usuario);
    Task<Usuario> UpdateAsync(Usuario usuario);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
