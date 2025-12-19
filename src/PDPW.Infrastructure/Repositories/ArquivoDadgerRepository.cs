using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

/// <summary>
/// Repositório de Arquivo DADGER
/// </summary>
public class ArquivoDadgerRepository : BaseRepository<ArquivoDadger>, IArquivoDadgerRepository
{
    public ArquivoDadgerRepository(PdpwDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<ArquivoDadger>> GetAllAsync()
    {
        return await _context.ArquivosDadger
            .Include(a => a.SemanaPMO)
            .Where(a => a.Ativo)
            .OrderByDescending(a => a.DataImportacao)
            .ToListAsync();
    }

    public override async Task<ArquivoDadger?> GetByIdAsync(int id)
    {
        return await _context.ArquivosDadger
            .Include(a => a.SemanaPMO)
            .Include(a => a.Valores)
            .FirstOrDefaultAsync(a => a.Id == id && a.Ativo);
    }

    public async Task<IEnumerable<ArquivoDadger>> GetBySemanaPMOAsync(int semanaPMOId)
    {
        return await _context.ArquivosDadger
            .Include(a => a.SemanaPMO)
            .Where(a => a.SemanaPMOId == semanaPMOId && a.Ativo)
            .OrderByDescending(a => a.DataImportacao)
            .ToListAsync();
    }

    public async Task<IEnumerable<ArquivoDadger>> GetProcessadosAsync(bool processado)
    {
        return await _context.ArquivosDadger
            .Include(a => a.SemanaPMO)
            .Where(a => a.Processado == processado && a.Ativo)
            .OrderByDescending(a => a.DataImportacao)
            .ToListAsync();
    }

    public async Task<IEnumerable<ArquivoDadger>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        return await _context.ArquivosDadger
            .Include(a => a.SemanaPMO)
            .Where(a => a.DataImportacao >= dataInicio && a.DataImportacao <= dataFim && a.Ativo)
            .OrderByDescending(a => a.DataImportacao)
            .ToListAsync();
    }

    public async Task<ArquivoDadger?> GetByNomeArquivoAsync(string nomeArquivo)
    {
        return await _context.ArquivosDadger
            .Include(a => a.SemanaPMO)
            .FirstOrDefaultAsync(a => a.NomeArquivo == nomeArquivo && a.Ativo);
    }
}
