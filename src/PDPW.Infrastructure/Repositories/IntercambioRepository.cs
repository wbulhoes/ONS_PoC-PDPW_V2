using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

/// <summary>
/// Repositório para Intercâmbios de Energia
/// </summary>
public class IntercambioRepository : IIntercambioRepository
{
    private readonly PdpwDbContext _context;

    public IntercambioRepository(PdpwDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Intercambio>> GetAllAsync()
    {
        return await _context.Intercambios
            .Where(i => i.Ativo)
            .OrderByDescending(i => i.DataReferencia)
            .ThenBy(i => i.SubsistemaOrigem)
            .ThenBy(i => i.SubsistemaDestino)
            .ToListAsync();
    }

    public async Task<Intercambio?> GetByIdAsync(int id)
    {
        return await _context.Intercambios
            .FirstOrDefaultAsync(i => i.Id == id && i.Ativo);
    }

    public async Task<IEnumerable<Intercambio>> GetBySubsistemaOrigemAsync(string subsistemaOrigem)
    {
        return await _context.Intercambios
            .Where(i => i.SubsistemaOrigem == subsistemaOrigem && i.Ativo)
            .OrderByDescending(i => i.DataReferencia)
            .ThenBy(i => i.SubsistemaDestino)
            .ToListAsync();
    }

    public async Task<IEnumerable<Intercambio>> GetBySubsistemaDestinoAsync(string subsistemaDestino)
    {
        return await _context.Intercambios
            .Where(i => i.SubsistemaDestino == subsistemaDestino && i.Ativo)
            .OrderByDescending(i => i.DataReferencia)
            .ThenBy(i => i.SubsistemaOrigem)
            .ToListAsync();
    }

    public async Task<IEnumerable<Intercambio>> GetByDataAsync(DateTime dataReferencia)
    {
        var data = dataReferencia.Date;
        return await _context.Intercambios
            .Where(i => i.DataReferencia.Date == data && i.Ativo)
            .OrderBy(i => i.SubsistemaOrigem)
            .ThenBy(i => i.SubsistemaDestino)
            .ToListAsync();
    }

    public async Task<IEnumerable<Intercambio>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        var inicio = dataInicio.Date;
        var fim = dataFim.Date;

        return await _context.Intercambios
            .Where(i => i.DataReferencia.Date >= inicio && 
                       i.DataReferencia.Date <= fim && 
                       i.Ativo)
            .OrderByDescending(i => i.DataReferencia)
            .ThenBy(i => i.SubsistemaOrigem)
            .ThenBy(i => i.SubsistemaDestino)
            .ToListAsync();
    }

    public async Task<Intercambio?> GetBySubsistemasDataAsync(
        string subsistemaOrigem, 
        string subsistemaDestino, 
        DateTime dataReferencia)
    {
        var data = dataReferencia.Date;
        return await _context.Intercambios
            .FirstOrDefaultAsync(i => i.SubsistemaOrigem == subsistemaOrigem && 
                                     i.SubsistemaDestino == subsistemaDestino &&
                                     i.DataReferencia.Date == data && 
                                     i.Ativo);
    }

    public async Task AddAsync(Intercambio intercambio)
    {
        await _context.Intercambios.AddAsync(intercambio);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Intercambio intercambio)
    {
        _context.Intercambios.Update(intercambio);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var intercambio = await GetByIdAsync(id);
        if (intercambio != null)
        {
            intercambio.Ativo = false;
            intercambio.DataAtualizacao = DateTime.UtcNow;
            await UpdateAsync(intercambio);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Intercambios.AnyAsync(i => i.Id == id && i.Ativo);
    }
}
