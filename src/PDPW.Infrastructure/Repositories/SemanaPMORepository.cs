using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

/// <summary>
/// Repositório de Semana PMO
/// </summary>
public class SemanaPMORepository : ISemanaPMORepository
{
    private readonly PdpwDbContext _context;

    public SemanaPMORepository(PdpwDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtém todas as semanas de um ano específico
    /// </summary>
    public async Task<IEnumerable<SemanaPMO>> ObterPorAnoAsync(int ano)
    {
        return await _context.SemanasPMO
            .Where(s => s.Ano == ano && s.Ativo)
            .Include(s => s.ArquivosDadger)
            .OrderBy(s => s.Numero)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém a semana PMO atual (baseada na data de hoje)
    /// </summary>
    public async Task<SemanaPMO?> ObterSemanaAtualAsync()
    {
        var hoje = DateTime.Today;
        return await _context.SemanasPMO
            .Where(s => s.DataInicio <= hoje && s.DataFim >= hoje && s.Ativo)
            .Include(s => s.ArquivosDadger)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Obtém semana por número e ano
    /// </summary>
    public async Task<SemanaPMO?> ObterPorNumeroEAnoAsync(int numero, int ano)
    {
        return await _context.SemanasPMO
            .Where(s => s.Numero == numero && s.Ano == ano && s.Ativo)
            .Include(s => s.ArquivosDadger)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Verifica se já existe uma semana com o mesmo número e ano
    /// </summary>
    public async Task<bool> ExisteNumeroAnoAsync(int numero, int ano, int? excluirId = null)
    {
        var query = _context.SemanasPMO
            .Where(s => s.Numero == numero && s.Ano == ano && s.Ativo);

        if (excluirId.HasValue)
        {
            query = query.Where(s => s.Id != excluirId.Value);
        }

        return await query.AnyAsync();
    }

    /// <summary>
    /// Obtém semanas em um período específico
    /// </summary>
    public async Task<IEnumerable<SemanaPMO>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        return await _context.SemanasPMO
            .Where(s => s.DataInicio <= dataFim && s.DataFim >= dataInicio && s.Ativo)
            .Include(s => s.ArquivosDadger)
            .OrderBy(s => s.DataInicio)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém as próximas N semanas a partir de hoje
    /// </summary>
    public async Task<IEnumerable<SemanaPMO>> ObterProximasSemanasAsync(int quantidade)
    {
        var hoje = DateTime.Today;
        return await _context.SemanasPMO
            .Where(s => s.DataInicio >= hoje && s.Ativo)
            .Include(s => s.ArquivosDadger)
            .OrderBy(s => s.DataInicio)
            .Take(quantidade)
            .ToListAsync();
    }

    public async Task<IEnumerable<SemanaPMO>> ObterTodosAsync()
    {
        return await _context.SemanasPMO
            .Where(s => s.Ativo)
            .Include(s => s.ArquivosDadger)
            .OrderByDescending(s => s.Ano)
            .ThenByDescending(s => s.Numero)
            .ToListAsync();
    }

    public async Task<SemanaPMO?> ObterPorIdAsync(int id)
    {
        return await _context.SemanasPMO
            .Include(s => s.ArquivosDadger)
            .FirstOrDefaultAsync(s => s.Id == id && s.Ativo);
    }

    public async Task<SemanaPMO> AdicionarAsync(SemanaPMO semana)
    {
        semana.DataCriacao = DateTime.UtcNow;
        semana.Ativo = true;

        _context.SemanasPMO.Add(semana);
        await _context.SaveChangesAsync();

        return semana;
    }

    public async Task AtualizarAsync(SemanaPMO semana)
    {
        semana.DataAtualizacao = DateTime.UtcNow;

        _context.Entry(semana).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var semana = await ObterPorIdAsync(id);
        if (semana != null)
        {
            semana.Ativo = false;
            semana.DataAtualizacao = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
