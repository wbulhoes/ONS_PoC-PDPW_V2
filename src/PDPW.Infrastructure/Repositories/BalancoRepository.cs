using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

/// <summary>
/// Repositório para Balanços Energéticos
/// </summary>
public class BalancoRepository : IBalancoRepository
{
    private readonly PdpwDbContext _context;

    public BalancoRepository(PdpwDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Balanco>> GetAllAsync()
    {
        return await _context.Balancos
            .Where(b => b.Ativo)
            .OrderByDescending(b => b.DataReferencia)
            .ThenBy(b => b.SubsistemaId)
            .ToListAsync();
    }

    public async Task<Balanco?> GetByIdAsync(int id)
    {
        return await _context.Balancos
            .FirstOrDefaultAsync(b => b.Id == id && b.Ativo);
    }

    public async Task<IEnumerable<Balanco>> GetBySubsistemaAsync(string subsistemaId)
    {
        return await _context.Balancos
            .Where(b => b.SubsistemaId == subsistemaId && b.Ativo)
            .OrderByDescending(b => b.DataReferencia)
            .ToListAsync();
    }

    public async Task<IEnumerable<Balanco>> GetByDataAsync(DateTime dataReferencia)
    {
        var data = dataReferencia.Date;
        return await _context.Balancos
            .Where(b => b.DataReferencia.Date == data && b.Ativo)
            .OrderBy(b => b.SubsistemaId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Balanco>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        var inicio = dataInicio.Date;
        var fim = dataFim.Date;

        return await _context.Balancos
            .Where(b => b.DataReferencia.Date >= inicio && 
                       b.DataReferencia.Date <= fim && 
                       b.Ativo)
            .OrderByDescending(b => b.DataReferencia)
            .ThenBy(b => b.SubsistemaId)
            .ToListAsync();
    }

    public async Task<Balanco?> GetBySubsistemaDataAsync(string subsistemaId, DateTime dataReferencia)
    {
        var data = dataReferencia.Date;
        return await _context.Balancos
            .FirstOrDefaultAsync(b => b.SubsistemaId == subsistemaId && 
                                     b.DataReferencia.Date == data && 
                                     b.Ativo);
    }

    public async Task AddAsync(Balanco balanco)
    {
        await _context.Balancos.AddAsync(balanco);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Balanco balanco)
    {
        _context.Balancos.Update(balanco);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var balanco = await GetByIdAsync(id);
        if (balanco != null)
        {
            balanco.Ativo = false;
            balanco.DataAtualizacao = DateTime.UtcNow;
            await UpdateAsync(balanco);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Balancos.AnyAsync(b => b.Id == id && b.Ativo);
    }
}
