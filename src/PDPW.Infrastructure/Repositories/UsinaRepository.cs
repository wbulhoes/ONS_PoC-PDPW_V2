using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

/// <summary>
/// Repositório de Usinas
/// </summary>
public class UsinaRepository : BaseRepository<Usina>, IUsinaRepository
{
    public UsinaRepository(PdpwDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtém todas as usinas com relacionamentos básicos
    /// </summary>
    public override async Task<IEnumerable<Usina>> GetAllAsync()
    {
        return await _dbSet
            .Where(e => e.Ativo)
            .Include(u => u.TipoUsina)
            .Include(u => u.Empresa)
            .OrderBy(u => u.Nome)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém usina por ID com relacionamentos
    /// </summary>
    public override async Task<Usina?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(u => u.TipoUsina)
            .Include(u => u.Empresa)
            .FirstOrDefaultAsync(u => u.Id == id && u.Ativo);
    }

    /// <summary>
    /// Obtém usina por código
    /// </summary>
    public async Task<Usina?> GetByCodigoAsync(string codigo)
    {
        return await _dbSet
            .Include(u => u.TipoUsina)
            .Include(u => u.Empresa)
            .FirstOrDefaultAsync(u => u.Codigo == codigo && u.Ativo);
    }

    /// <summary>
    /// Obtém usinas por tipo
    /// </summary>
    public async Task<IEnumerable<Usina>> GetByTipoAsync(int tipoUsinaId)
    {
        return await _dbSet
            .Where(u => u.TipoUsinaId == tipoUsinaId && u.Ativo)
            .Include(u => u.TipoUsina)
            .Include(u => u.Empresa)
            .OrderBy(u => u.Nome)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém usinas por empresa
    /// </summary>
    public async Task<IEnumerable<Usina>> GetByEmpresaAsync(int empresaId)
    {
        return await _dbSet
            .Where(u => u.EmpresaId == empresaId && u.Ativo)
            .Include(u => u.TipoUsina)
            .Include(u => u.Empresa)
            .OrderBy(u => u.Nome)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém usina com suas unidades geradoras
    /// </summary>
    public async Task<Usina?> GetByIdWithUnidadesAsync(int id)
    {
        return await _dbSet
            .Include(u => u.TipoUsina)
            .Include(u => u.Empresa)
            .Include(u => u.UnidadesGeradoras)
            .FirstOrDefaultAsync(u => u.Id == id && u.Ativo);
    }

    /// <summary>
    /// Verifica se código já existe
    /// </summary>
    public async Task<bool> CodigoExisteAsync(string codigo, int? usinaIdExcluir = null)
    {
        if (usinaIdExcluir.HasValue)
        {
            return await _dbSet.AnyAsync(u => 
                u.Codigo == codigo && 
                u.Id != usinaIdExcluir.Value && 
                u.Ativo);
        }

        return await _dbSet.AnyAsync(u => u.Codigo == codigo && u.Ativo);
    }
}
