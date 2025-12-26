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

    /// <summary>
    /// Obtém arquivos por status
    /// </summary>
    public async Task<IEnumerable<ArquivoDadger>> GetByStatusAsync(string status)
    {
        return await _context.ArquivosDadger
            .Include(a => a.SemanaPMO)
            .Where(a => a.Status == status && a.Ativo)
            .OrderByDescending(a => a.DataImportacao)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém arquivos pendentes de aprovação
    /// </summary>
    public async Task<IEnumerable<ArquivoDadger>> GetPendentesAprovacaoAsync()
    {
        return await _context.ArquivosDadger
            .Include(a => a.SemanaPMO)
            .Where(a => a.Status == "EmAnalise" && a.Ativo)
            .OrderBy(a => a.DataFinalizacao)
            .ToListAsync();
    }

    /// <summary>
    /// Finaliza programação
    /// </summary>
    public async Task FinalizarAsync(int id, string usuario, string? observacao = null)
    {
        var arquivo = await _dbSet.FindAsync(id);
        if (arquivo != null)
        {
            arquivo.Status = "EmAnalise";
            arquivo.DataFinalizacao = DateTime.Now;
            arquivo.UsuarioFinalizacao = usuario;
            arquivo.ObservacaoFinalizacao = observacao;
            arquivo.DataAtualizacao = DateTime.Now;
            
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Aprova programação
    /// </summary>
    public async Task AprovarAsync(int id, string usuario, string? observacao = null)
    {
        var arquivo = await _dbSet.FindAsync(id);
        if (arquivo != null)
        {
            arquivo.Status = "Aprovado";
            arquivo.DataAprovacao = DateTime.Now;
            arquivo.UsuarioAprovacao = usuario;
            arquivo.ObservacaoAprovacao = observacao;
            arquivo.DataAtualizacao = DateTime.Now;
            
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Reabre programação
    /// </summary>
    public async Task ReabrirAsync(int id, string usuario, string? observacao = null)
    {
        var arquivo = await _dbSet.FindAsync(id);
        if (arquivo != null)
        {
            arquivo.Status = "Aberto";
            arquivo.DataFinalizacao = null;
            arquivo.UsuarioFinalizacao = null;
            arquivo.ObservacaoFinalizacao = null;
            arquivo.DataAprovacao = null;
            arquivo.UsuarioAprovacao = null;
            arquivo.ObservacaoAprovacao = observacao;
            arquivo.DataAtualizacao = DateTime.Now;
            
            await _context.SaveChangesAsync();
        }
    }
}
