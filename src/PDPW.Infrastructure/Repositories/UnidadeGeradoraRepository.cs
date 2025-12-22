using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

/// <summary>
/// Repositório para Unidades Geradoras
/// </summary>
public class UnidadeGeradoraRepository : IUnidadeGeradoraRepository
{
    private readonly PdpwDbContext _context;

    public UnidadeGeradoraRepository(PdpwDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UnidadeGeradora>> GetAllAsync()
    {
        return await _context.UnidadesGeradoras
            .Where(u => u.Ativo)
            .OrderBy(u => u.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<UnidadeGeradora>> GetAllWithUsinaAsync()
    {
        return await _context.UnidadesGeradoras
            .Include(u => u.Usina)
            .Where(u => u.Ativo)
            .OrderBy(u => u.Nome)
            .ToListAsync();
    }

    public async Task<UnidadeGeradora?> GetByIdAsync(int id)
    {
        return await _context.UnidadesGeradoras
            .FirstOrDefaultAsync(u => u.Id == id && u.Ativo);
    }

    public async Task<UnidadeGeradora?> GetByIdWithUsinaAsync(int id)
    {
        return await _context.UnidadesGeradoras
            .Include(u => u.Usina)
            .FirstOrDefaultAsync(u => u.Id == id && u.Ativo);
    }

    public async Task<UnidadeGeradora?> GetByCodigoAsync(string codigo)
    {
        return await _context.UnidadesGeradoras
            .Include(u => u.Usina)
            .FirstOrDefaultAsync(u => u.Codigo == codigo && u.Ativo);
    }

    public async Task<IEnumerable<UnidadeGeradora>> GetByUsinaIdAsync(int usinaId)
    {
        return await _context.UnidadesGeradoras
            .Include(u => u.Usina)
            .Where(u => u.UsinaId == usinaId && u.Ativo)
            .OrderBy(u => u.Codigo)
            .ToListAsync();
    }

    public async Task<IEnumerable<UnidadeGeradora>> GetByStatusAsync(string status)
    {
        return await _context.UnidadesGeradoras
            .Include(u => u.Usina)
            .Where(u => u.Status == status && u.Ativo)
            .OrderBy(u => u.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<UnidadeGeradora>> GetAtivasAsync()
    {
        return await _context.UnidadesGeradoras
            .Include(u => u.Usina)
            .Where(u => u.Ativo)
            .OrderBy(u => u.Nome)
            .ToListAsync();
    }

    public async Task AddAsync(UnidadeGeradora unidade)
    {
        await _context.UnidadesGeradoras.AddAsync(unidade);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(UnidadeGeradora unidade)
    {
        _context.UnidadesGeradoras.Update(unidade);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var unidade = await GetByIdAsync(id);
        if (unidade != null)
        {
            unidade.Ativo = false;
            unidade.DataAtualizacao = DateTime.UtcNow;
            await UpdateAsync(unidade);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.UnidadesGeradoras.AnyAsync(u => u.Id == id && u.Ativo);
    }

    public async Task<bool> CodigoExistsAsync(string codigo, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await _context.UnidadesGeradoras
                .AnyAsync(u => u.Codigo == codigo && u.Id != excludeId.Value && u.Ativo);
        }

        return await _context.UnidadesGeradoras
            .AnyAsync(u => u.Codigo == codigo && u.Ativo);
    }
}
