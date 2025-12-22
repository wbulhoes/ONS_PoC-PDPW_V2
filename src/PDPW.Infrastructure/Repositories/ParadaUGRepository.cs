using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

/// <summary>
/// Repositório para Paradas de Unidades Geradoras
/// </summary>
public class ParadaUGRepository : IParadaUGRepository
{
    private readonly PdpwDbContext _context;

    public ParadaUGRepository(PdpwDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ParadaUG>> GetAllAsync()
    {
        return await _context.ParadasUG
            .Where(p => p.Ativo)
            .OrderByDescending(p => p.DataInicio)
            .ToListAsync();
    }

    public async Task<IEnumerable<ParadaUG>> GetAllWithUnidadeAsync()
    {
        return await _context.ParadasUG
            .Include(p => p.UnidadeGeradora)
            .Where(p => p.Ativo)
            .OrderByDescending(p => p.DataInicio)
            .ToListAsync();
    }

    public async Task<ParadaUG?> GetByIdAsync(int id)
    {
        return await _context.ParadasUG
            .FirstOrDefaultAsync(p => p.Id == id && p.Ativo);
    }

    public async Task<ParadaUG?> GetByIdWithUnidadeAsync(int id)
    {
        return await _context.ParadasUG
            .Include(p => p.UnidadeGeradora)
            .FirstOrDefaultAsync(p => p.Id == id && p.Ativo);
    }

    public async Task<IEnumerable<ParadaUG>> GetByUnidadeGeradoraIdAsync(int unidadeGeradoraId)
    {
        return await _context.ParadasUG
            .Include(p => p.UnidadeGeradora)
            .Where(p => p.UnidadeGeradoraId == unidadeGeradoraId && p.Ativo)
            .OrderByDescending(p => p.DataInicio)
            .ToListAsync();
    }

    public async Task<IEnumerable<ParadaUG>> GetProgramadasAsync()
    {
        return await _context.ParadasUG
            .Include(p => p.UnidadeGeradora)
            .Where(p => p.Programada && p.Ativo)
            .OrderByDescending(p => p.DataInicio)
            .ToListAsync();
    }

    public async Task<IEnumerable<ParadaUG>> GetNaoProgramadasAsync()
    {
        return await _context.ParadasUG
            .Include(p => p.UnidadeGeradora)
            .Where(p => !p.Programada && p.Ativo)
            .OrderByDescending(p => p.DataInicio)
            .ToListAsync();
    }

    public async Task<IEnumerable<ParadaUG>> GetAtivasAsync(DateTime dataReferencia)
    {
        return await _context.ParadasUG
            .Include(p => p.UnidadeGeradora)
            .Where(p => p.DataInicio <= dataReferencia && 
                       (p.DataFim == null || p.DataFim >= dataReferencia) &&
                       p.Ativo)
            .OrderByDescending(p => p.DataInicio)
            .ToListAsync();
    }

    public async Task<IEnumerable<ParadaUG>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        return await _context.ParadasUG
            .Include(p => p.UnidadeGeradora)
            .Where(p => p.DataInicio <= dataFim &&
                       (p.DataFim == null || p.DataFim >= dataInicio) &&
                       p.Ativo)
            .OrderByDescending(p => p.DataInicio)
            .ToListAsync();
    }

    public async Task AddAsync(ParadaUG parada)
    {
        await _context.ParadasUG.AddAsync(parada);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ParadaUG parada)
    {
        _context.ParadasUG.Update(parada);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var parada = await GetByIdAsync(id);
        if (parada != null)
        {
            parada.Ativo = false;
            parada.DataAtualizacao = DateTime.UtcNow;
            await UpdateAsync(parada);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.ParadasUG.AnyAsync(p => p.Id == id && p.Ativo);
    }
}
