using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

/// <summary>
/// Repositório de Equipe PDP
/// </summary>
public class EquipePDPRepository : IEquipePDPRepository
{
    private readonly PdpwDbContext _context;

    public EquipePDPRepository(PdpwDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EquipePDP>> ObterTodasAsync()
    {
        return await _context.EquipesPDP
            .Where(e => e.Ativo)
            .OrderBy(e => e.Nome)
            .ToListAsync();
    }

    public async Task<EquipePDP?> ObterPorIdAsync(int id)
    {
        return await _context.EquipesPDP
            .FirstOrDefaultAsync(e => e.Id == id && e.Ativo);
    }

    public async Task<EquipePDP> AdicionarAsync(EquipePDP equipe)
    {
        equipe.DataCriacao = DateTime.UtcNow;
        equipe.Ativo = true;
        
        _context.EquipesPDP.Add(equipe);
        await _context.SaveChangesAsync();
        
        return equipe;
    }

    public async Task AtualizarAsync(EquipePDP equipe)
    {
        equipe.DataAtualizacao = DateTime.UtcNow;
        
        _context.Entry(equipe).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var equipe = await ObterPorIdAsync(id);
        if (equipe != null)
        {
            equipe.Ativo = false;
            equipe.DataAtualizacao = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<EquipePDP?> ObterPorNomeAsync(string nome)
    {
        return await _context.EquipesPDP
            .FirstOrDefaultAsync(e => e.Nome.ToLower() == nome.ToLower() && e.Ativo);
    }

    public async Task<bool> ExisteNomeAsync(string nome, int? excluirId = null)
    {
        var query = _context.EquipesPDP
            .Where(e => e.Nome.ToLower() == nome.ToLower() && e.Ativo);

        if (excluirId.HasValue)
        {
            query = query.Where(e => e.Id != excluirId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<EquipePDP?> ObterComMembrosAsync(int id)
    {
        return await _context.EquipesPDP
            .Include(e => e.Membros!.Where(m => m.Ativo))
            .FirstOrDefaultAsync(e => e.Id == id && e.Ativo);
    }
}
