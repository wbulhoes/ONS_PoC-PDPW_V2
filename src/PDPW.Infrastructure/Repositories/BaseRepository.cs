using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

/// <summary>
/// Repositório base com operações CRUD genéricas
/// </summary>
/// <typeparam name="T">Entidade que herda de BaseEntity</typeparam>
public abstract class BaseRepository<T> where T : BaseEntity
{
    protected readonly PdpwDbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected BaseRepository(PdpwDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    /// <summary>
    /// Obtém todas as entidades ativas
    /// </summary>
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet
            .Where(e => e.Ativo)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém entidade por ID (apenas ativas)
    /// </summary>
    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet
            .FirstOrDefaultAsync(e => e.Id == id && e.Ativo);
    }

    /// <summary>
    /// Adiciona nova entidade
    /// </summary>
    public virtual async Task<T> AddAsync(T entity)
    {
        entity.DataCriacao = DateTime.UtcNow;
        entity.Ativo = true;
        
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        
        return entity;
    }

    /// <summary>
    /// Atualiza entidade existente
    /// </summary>
    public virtual async Task UpdateAsync(T entity)
    {
        entity.DataAtualizacao = DateTime.UtcNow;
        
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Remove entidade (soft delete)
    /// </summary>
    public virtual async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            entity.Ativo = false;
            entity.DataAtualizacao = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Remove entidade permanentemente (hard delete)
    /// </summary>
    public virtual async Task HardDeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Verifica se existe entidade com o ID
    /// </summary>
    public virtual async Task<bool> ExistsAsync(int id)
    {
        return await _dbSet.AnyAsync(e => e.Id == id && e.Ativo);
    }

    /// <summary>
    /// Conta total de entidades ativas
    /// </summary>
    public virtual async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync(e => e.Ativo);
    }
}
