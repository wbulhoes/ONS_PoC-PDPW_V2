using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de dados energéticos
/// </summary>
public class DadoEnergeticoRepository : IDadoEnergeticoRepository
{
    private readonly PdpwDbContext _context;

    public DadoEnergeticoRepository(PdpwDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DadoEnergetico>> ObterTodosAsync()
    {
        return await _context.DadosEnergeticos
            .Where(d => d.Ativo)
            .OrderByDescending(d => d.DataReferencia)
            .ToListAsync();
    }

    public async Task<DadoEnergetico?> ObterPorIdAsync(int id)
    {
        return await _context.DadosEnergeticos
            .FirstOrDefaultAsync(d => d.Id == id && d.Ativo);
    }

    public async Task<DadoEnergetico> AdicionarAsync(DadoEnergetico dado)
    {
        _context.DadosEnergeticos.Add(dado);
        await _context.SaveChangesAsync();
        return dado;
    }

    public async Task AtualizarAsync(DadoEnergetico dado)
    {
        _context.Entry(dado).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var dado = await ObterPorIdAsync(id);
        if (dado != null)
        {
            dado.Ativo = false;
            dado.DataAtualizacao = DateTime.UtcNow;
            await AtualizarAsync(dado);
        }
    }

    public async Task<IEnumerable<DadoEnergetico>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        return await _context.DadosEnergeticos
            .Where(d => d.Ativo && d.DataReferencia >= dataInicio && d.DataReferencia <= dataFim)
            .OrderBy(d => d.DataReferencia)
            .ToListAsync();
    }
}
