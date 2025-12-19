using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Interface do repositório de Empresa
/// </summary>
public interface IEmpresaRepository
{
    /// <summary>
    /// Obtém todas as empresas ativas
    /// </summary>
    Task<List<Empresa>> GetAllAsync();

    /// <summary>
    /// Obtém empresa por ID
    /// </summary>
    Task<Empresa?> GetByIdAsync(int id);

    /// <summary>
    /// Busca empresa por nome
    /// </summary>
    Task<Empresa?> GetByNomeAsync(string nome);

    /// <summary>
    /// Busca empresa por CNPJ
    /// </summary>
    Task<Empresa?> GetByCnpjAsync(string cnpj);

    /// <summary>
    /// Adiciona nova empresa
    /// </summary>
    Task<Empresa> AddAsync(Empresa empresa);

    /// <summary>
    /// Atualiza empresa existente
    /// </summary>
    Task UpdateAsync(Empresa empresa);

    /// <summary>
    /// Remove empresa (soft delete)
    /// </summary>
    Task DeleteAsync(Empresa empresa);

    /// <summary>
    /// Verifica se já existe uma empresa com o nome informado
    /// </summary>
    Task<bool> ExisteNomeAsync(string nome, int? empresaIdExcluir = null);

    /// <summary>
    /// Verifica se já existe uma empresa com o CNPJ informado
    /// </summary>
    Task<bool> ExisteCnpjAsync(string cnpj, int? empresaIdExcluir = null);

    /// <summary>
    /// Salva alterações no banco de dados
    /// </summary>
    Task<int> SaveChangesAsync();
}
