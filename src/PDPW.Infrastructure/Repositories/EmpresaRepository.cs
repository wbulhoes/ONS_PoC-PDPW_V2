using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

/// <summary>
/// Repositório de Empresa
/// </summary>
public class EmpresaRepository : BaseRepository<Empresa>, IEmpresaRepository
{
    public EmpresaRepository(PdpwDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtém todas as empresas ativas com contagem de usinas
    /// </summary>
    public new async Task<List<Empresa>> GetAllAsync()
    {
        return await _dbSet
            .Where(e => e.Ativo)
            .Include(e => e.Usinas!.Where(u => u.Ativo))
            .OrderBy(e => e.Nome)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém empresa por ID incluindo usinas relacionadas
    /// </summary>
    public new async Task<Empresa?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Where(e => e.Ativo)
            .Include(e => e.Usinas!.Where(u => u.Ativo))
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    /// <summary>
    /// Busca empresa por nome
    /// </summary>
    public async Task<Empresa?> GetByNomeAsync(string nome)
    {
        return await _dbSet
            .Where(e => e.Ativo)
            .FirstOrDefaultAsync(e => e.Nome.ToLower() == nome.ToLower());
    }

    /// <summary>
    /// Busca empresa por CNPJ
    /// </summary>
    public async Task<Empresa?> GetByCnpjAsync(string cnpj)
    {
        var cnpjLimpo = LimparCnpj(cnpj);
        
        return await _dbSet
            .Where(e => e.Ativo)
            .FirstOrDefaultAsync(e => e.CNPJ != null && e.CNPJ.Replace(".", "").Replace("/", "").Replace("-", "") == cnpjLimpo);
    }

    /// <summary>
    /// Verifica se já existe uma empresa com o nome informado
    /// </summary>
    public async Task<bool> ExisteNomeAsync(string nome, int? empresaIdExcluir = null)
    {
        var query = _dbSet.Where(e => e.Ativo && e.Nome.ToLower() == nome.ToLower());

        if (empresaIdExcluir.HasValue)
        {
            query = query.Where(e => e.Id != empresaIdExcluir.Value);
        }

        return await query.AnyAsync();
    }

    /// <summary>
    /// Verifica se já existe uma empresa com o CNPJ informado
    /// </summary>
    public async Task<bool> ExisteCnpjAsync(string cnpj, int? empresaIdExcluir = null)
    {
        var cnpjLimpo = LimparCnpj(cnpj);
        
        var query = _dbSet.Where(e => e.Ativo && 
                                      e.CNPJ != null && 
                                      e.CNPJ.Replace(".", "").Replace("/", "").Replace("-", "") == cnpjLimpo);

        if (empresaIdExcluir.HasValue)
        {
            query = query.Where(e => e.Id != empresaIdExcluir.Value);
        }

        return await query.AnyAsync();
    }

    /// <summary>
    /// Remove empresa (soft delete)
    /// </summary>
    public async Task DeleteAsync(Empresa empresa)
    {
        empresa.Ativo = false;
        empresa.DataAtualizacao = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Salva alterações no banco de dados
    /// </summary>
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Remove caracteres especiais do CNPJ
    /// </summary>
    private static string LimparCnpj(string cnpj)
    {
        return cnpj?.Replace(".", "").Replace("/", "").Replace("-", "").Trim() ?? string.Empty;
    }
}
