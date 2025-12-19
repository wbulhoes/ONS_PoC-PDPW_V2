using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

/// <summary>
/// Repositório de Carga
/// </summary>
public class CargaRepository : BaseRepository<Carga>, ICargaRepository
{
    public CargaRepository(PdpwDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Carga>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        return await _context.Cargas
            .Where(c => c.DataReferencia >= dataInicio && c.DataReferencia <= dataFim && c.Ativo)
            .OrderBy(c => c.DataReferencia)
            .ThenBy(c => c.SubsistemaId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Carga>> GetBySubsistemaAsync(string subsistemaId)
    {
        return await _context.Cargas
            .Where(c => c.SubsistemaId == subsistemaId && c.Ativo)
            .OrderByDescending(c => c.DataReferencia)
            .ToListAsync();
    }

    public async Task<IEnumerable<Carga>> GetByDataReferenciaAsync(DateTime dataReferencia)
    {
        return await _context.Cargas
            .Where(c => c.DataReferencia.Date == dataReferencia.Date && c.Ativo)
            .OrderBy(c => c.SubsistemaId)
            .ToListAsync();
    }
}
