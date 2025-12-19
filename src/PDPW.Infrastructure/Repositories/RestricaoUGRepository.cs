using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

/// <summary>
/// Repositório de Restrição de Unidade Geradora
/// </summary>
public class RestricaoUGRepository : BaseRepository<RestricaoUG>, IRestricaoUGRepository
{
    public RestricaoUGRepository(PdpwDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<RestricaoUG>> GetAllAsync()
    {
        return await _context.RestricoesUG
            .Include(r => r.UnidadeGeradora)
            .Include(r => r.MotivoRestricao)
            .Where(r => r.Ativo)
            .OrderByDescending(r => r.DataInicio)
            .ToListAsync();
    }

    public override async Task<RestricaoUG?> GetByIdAsync(int id)
    {
        return await _context.RestricoesUG
            .Include(r => r.UnidadeGeradora)
                .ThenInclude(ug => ug!.Usina)
            .Include(r => r.MotivoRestricao)
            .FirstOrDefaultAsync(r => r.Id == id && r.Ativo);
    }

    public async Task<IEnumerable<RestricaoUG>> GetByUnidadeGeradoraAsync(int unidadeGeradoraId)
    {
        return await _context.RestricoesUG
            .Include(r => r.UnidadeGeradora)
            .Include(r => r.MotivoRestricao)
            .Where(r => r.UnidadeGeradoraId == unidadeGeradoraId && r.Ativo)
            .OrderByDescending(r => r.DataInicio)
            .ToListAsync();
    }

    public async Task<IEnumerable<RestricaoUG>> GetAtivasAsync(DateTime dataReferencia)
    {
        return await _context.RestricoesUG
            .Include(r => r.UnidadeGeradora)
            .Include(r => r.MotivoRestricao)
            .Where(r => r.DataInicio <= dataReferencia 
                     && (r.DataFim == null || r.DataFim >= dataReferencia)
                     && r.Ativo)
            .OrderBy(r => r.UnidadeGeradoraId)
            .ToListAsync();
    }

    public async Task<IEnumerable<RestricaoUG>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        return await _context.RestricoesUG
            .Include(r => r.UnidadeGeradora)
            .Include(r => r.MotivoRestricao)
            .Where(r => r.DataInicio <= dataFim 
                     && (r.DataFim == null || r.DataFim >= dataInicio)
                     && r.Ativo)
            .OrderByDescending(r => r.DataInicio)
            .ToListAsync();
    }

    public async Task<IEnumerable<RestricaoUG>> GetByMotivoRestricaoAsync(int motivoRestricaoId)
    {
        return await _context.RestricoesUG
            .Include(r => r.UnidadeGeradora)
            .Include(r => r.MotivoRestricao)
            .Where(r => r.MotivoRestricaoId == motivoRestricaoId && r.Ativo)
            .OrderByDescending(r => r.DataInicio)
            .ToListAsync();
    }
}
