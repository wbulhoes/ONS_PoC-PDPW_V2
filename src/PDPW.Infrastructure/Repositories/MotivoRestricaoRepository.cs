using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

/// <summary>
/// Repositório para Motivos de Restrição
/// </summary>
public class MotivoRestricaoRepository : IMotivoRestricaoRepository
{
    private readonly PdpwDbContext _context;

    public MotivoRestricaoRepository(PdpwDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MotivoRestricao>> GetAllAsync()
    {
        return await _context.MotivosRestricao
            .Include(m => m.RestricoesUG)
            .Include(m => m.RestricoesUS)
            .Where(m => m.Ativo)
            .OrderBy(m => m.Nome)
            .ToListAsync();
    }

    public async Task<MotivoRestricao?> GetByIdAsync(int id)
    {
        return await _context.MotivosRestricao
            .Include(m => m.RestricoesUG)
            .Include(m => m.RestricoesUS)
            .FirstOrDefaultAsync(m => m.Id == id && m.Ativo);
    }

    public async Task<IEnumerable<MotivoRestricao>> GetByCategoriaAsync(string categoria)
    {
        return await _context.MotivosRestricao
            .Include(m => m.RestricoesUG)
            .Include(m => m.RestricoesUS)
            .Where(m => m.Categoria == categoria && m.Ativo)
            .OrderBy(m => m.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<MotivoRestricao>> GetAtivasAsync()
    {
        return await _context.MotivosRestricao
            .Include(m => m.RestricoesUG)
            .Include(m => m.RestricoesUS)
            .Where(m => m.Ativo)
            .OrderBy(m => m.Nome)
            .ToListAsync();
    }

    public async Task AddAsync(MotivoRestricao motivo)
    {
        await _context.MotivosRestricao.AddAsync(motivo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MotivoRestricao motivo)
    {
        _context.MotivosRestricao.Update(motivo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var motivo = await GetByIdAsync(id);
        if (motivo != null)
        {
            motivo.Ativo = false;
            motivo.DataAtualizacao = DateTime.UtcNow;
            await UpdateAsync(motivo);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.MotivosRestricao.AnyAsync(m => m.Id == id && m.Ativo);
    }
}
