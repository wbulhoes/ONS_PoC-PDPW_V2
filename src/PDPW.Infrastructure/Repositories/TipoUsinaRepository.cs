using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

/// <summary>
/// Repositório de Tipo de Usina
/// </summary>
public class TipoUsinaRepository : BaseRepository<TipoUsina>, ITipoUsinaRepository
{
    public TipoUsinaRepository(PdpwDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtém todos os tipos de usina ativos com contagem de usinas
    /// </summary>
    public new async Task<List<TipoUsina>> GetAllAsync()
    {
        return await _dbSet
            .Where(e => e.Ativo)
            .Include(t => t.Usinas!.Where(u => u.Ativo))
            .OrderBy(t => t.Nome)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém tipo de usina por ID incluindo usinas relacionadas
    /// </summary>
    public new async Task<TipoUsina?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Where(e => e.Ativo)
            .Include(t => t.Usinas!.Where(u => u.Ativo))
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <summary>
    /// Busca tipo de usina por nome
    /// </summary>
    public async Task<TipoUsina?> GetByNomeAsync(string nome)
    {
        return await _dbSet
            .Where(t => t.Ativo)
            .FirstOrDefaultAsync(t => t.Nome.ToLower() == nome.ToLower());
    }

    /// <summary>
    /// Verifica se já existe um tipo de usina com o nome informado
    /// </summary>
    public async Task<bool> ExisteNomeAsync(string nome, int? tipoUsinaIdExcluir = null)
    {
        var query = _dbSet.Where(t => t.Ativo && t.Nome.ToLower() == nome.ToLower());

        if (tipoUsinaIdExcluir.HasValue)
        {
            query = query.Where(t => t.Id != tipoUsinaIdExcluir.Value);
        }

        return await query.AnyAsync();
    }

    /// <summary>
    /// Remove tipo de usina (soft delete)
    /// </summary>
    public async Task DeleteAsync(TipoUsina tipoUsina)
    {
        tipoUsina.Ativo = false;
        tipoUsina.DataAtualizacao = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Salva alterações no banco de dados
    /// </summary>
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
