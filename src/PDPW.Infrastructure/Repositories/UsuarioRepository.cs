using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PDPW.Infrastructure.Repositories;

/// <summary>
/// Repositório de Usuários
/// </summary>
public class UsuarioRepository : IUsuarioRepository
{
    private readonly PdpwDbContext _context;

    public UsuarioRepository(PdpwDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return await _context.Usuarios
            .Include(u => u.EquipePDP)
            .Where(u => u.Ativo)
            .OrderBy(u => u.Nome)
            .ToListAsync();
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _context.Usuarios
            .Include(u => u.EquipePDP)
            .FirstOrDefaultAsync(u => u.Id == id && u.Ativo);
    }

    public async Task<IEnumerable<Usuario>> GetByPerfilAsync(string perfil)
    {
        return await _context.Usuarios
            .Include(u => u.EquipePDP)
            .Where(u => u.Perfil == perfil && u.Ativo)
            .OrderBy(u => u.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<Usuario>> GetByEquipeAsync(int equipePdpId)
    {
        return await _context.Usuarios
            .Include(u => u.EquipePDP)
            .Where(u => u.EquipePDPId == equipePdpId && u.Ativo)
            .OrderBy(u => u.Nome)
            .ToListAsync();
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _context.Usuarios
            .Include(u => u.EquipePDP)
            .FirstOrDefaultAsync(u => u.Email == email && u.Ativo);
    }

    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        usuario.DataCriacao = DateTime.Now;
        usuario.Ativo = true;
        
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        
        return usuario;
    }

    public async Task<Usuario> UpdateAsync(Usuario usuario)
    {
        usuario.DataAtualizacao = DateTime.Now;
        
        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();
        
        return usuario;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null) return false;

        usuario.Ativo = false;
        usuario.DataAtualizacao = DateTime.Now;
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Usuarios.AnyAsync(u => u.Id == id && u.Ativo);
    }
}
