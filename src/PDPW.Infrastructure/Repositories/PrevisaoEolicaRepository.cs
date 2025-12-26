using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

public class PrevisaoEolicaRepository : BaseRepository<PrevisaoEolica>, IPrevisaoEolicaRepository
{
    public PrevisaoEolicaRepository(PdpwDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<PrevisaoEolica>> GetAllAsync()
    {
        return await _dbSet
            .Where(p => p.Ativo)
            .Include(p => p.Usina)
            .Include(p => p.SemanaPMO)
            .OrderByDescending(p => p.DataHoraReferencia)
            .ToListAsync();
    }

    public override async Task<PrevisaoEolica?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(p => p.Usina)
            .Include(p => p.SemanaPMO)
            .FirstOrDefaultAsync(p => p.Id == id && p.Ativo);
    }

    public async Task<IEnumerable<PrevisaoEolica>> GetByUsinaAsync(int usinaId)
    {
        return await _dbSet
            .Where(p => p.UsinaId == usinaId && p.Ativo)
            .Include(p => p.Usina)
            .Include(p => p.SemanaPMO)
            .OrderByDescending(p => p.DataHoraReferencia)
            .ToListAsync();
    }

    public async Task<IEnumerable<PrevisaoEolica>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        return await _dbSet
            .Where(p => p.DataHoraPrevista >= dataInicio && 
                       p.DataHoraPrevista <= dataFim && 
                       p.Ativo)
            .Include(p => p.Usina)
            .OrderBy(p => p.DataHoraPrevista)
            .ToListAsync();
    }

    public async Task<IEnumerable<PrevisaoEolica>> GetByModeloAsync(string modelo)
    {
        return await _dbSet
            .Where(p => p.ModeloPrevisao == modelo && p.Ativo)
            .Include(p => p.Usina)
            .OrderByDescending(p => p.DataHoraReferencia)
            .ToListAsync();
    }

    public async Task<IEnumerable<PrevisaoEolica>> GetByTipoPrevisaoAsync(string tipoPrevisao)
    {
        return await _dbSet
            .Where(p => p.TipoPrevisao == tipoPrevisao && p.Ativo)
            .Include(p => p.Usina)
            .OrderByDescending(p => p.DataHoraReferencia)
            .ToListAsync();
    }

    public async Task<IEnumerable<PrevisaoEolica>> GetUltimasPrevisoes(int usinaId, int quantidade = 10)
    {
        return await _dbSet
            .Where(p => p.UsinaId == usinaId && p.Ativo)
            .Include(p => p.Usina)
            .OrderByDescending(p => p.DataHoraReferencia)
            .Take(quantidade)
            .ToListAsync();
    }

    public async Task<PrevisaoEolica?> GetPrevisaoMaisRecenteAsync(int usinaId)
    {
        return await _dbSet
            .Where(p => p.UsinaId == usinaId && p.Ativo)
            .Include(p => p.Usina)
            .OrderByDescending(p => p.DataHoraReferencia)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PrevisaoEolica>> GetPrevisoesComErroAsync(int usinaId)
    {
        return await _dbSet
            .Where(p => p.UsinaId == usinaId && 
                       p.GeracaoRealMWmed.HasValue && 
                       p.Ativo)
            .Include(p => p.Usina)
            .OrderByDescending(p => p.DataHoraPrevista)
            .ToListAsync();
    }

    public async Task<decimal> CalcularMAE(int usinaId, DateTime dataInicio, DateTime dataFim)
    {
        var previsoes = await _dbSet
            .Where(p => p.UsinaId == usinaId &&
                       p.DataHoraPrevista >= dataInicio &&
                       p.DataHoraPrevista <= dataFim &&
                       p.ErroAbsolutoMW.HasValue &&
                       p.Ativo)
            .ToListAsync();

        if (!previsoes.Any())
            return 0;

        return previsoes.Average(p => Math.Abs(p.ErroAbsolutoMW!.Value));
    }

    public async Task<decimal> CalcularRMSE(int usinaId, DateTime dataInicio, DateTime dataFim)
    {
        var previsoes = await _dbSet
            .Where(p => p.UsinaId == usinaId &&
                       p.DataHoraPrevista >= dataInicio &&
                       p.DataHoraPrevista <= dataFim &&
                       p.ErroAbsolutoMW.HasValue &&
                       p.Ativo)
            .ToListAsync();

        if (!previsoes.Any())
            return 0;

        var quadradoErros = previsoes.Select(p => p.ErroAbsolutoMW!.Value * p.ErroAbsolutoMW.Value);
        var mediaQuadrados = quadradoErros.Average();
        
        return (decimal)Math.Sqrt((double)mediaQuadrados);
    }

    public async Task AtualizarGeracaoRealAsync(int id, decimal geracaoReal)
    {
        var previsao = await _dbSet.FindAsync(id);
        if (previsao != null)
        {
            previsao.GeracaoRealMWmed = geracaoReal;
            previsao.CalcularErro();
            previsao.DataAtualizacao = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}
