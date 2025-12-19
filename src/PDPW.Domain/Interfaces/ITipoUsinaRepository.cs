using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Interface do repositório de Tipo de Usina
/// </summary>
public interface ITipoUsinaRepository
{
    /// <summary>
    /// Obtém todos os tipos de usina ativos
    /// </summary>
    Task<List<TipoUsina>> GetAllAsync();

    /// <summary>
    /// Obtém tipo de usina por ID
    /// </summary>
    Task<TipoUsina?> GetByIdAsync(int id);

    /// <summary>
    /// Busca tipo de usina por nome
    /// </summary>
    Task<TipoUsina?> GetByNomeAsync(string nome);

    /// <summary>
    /// Adiciona novo tipo de usina
    /// </summary>
    Task<TipoUsina> AddAsync(TipoUsina tipoUsina);

    /// <summary>
    /// Atualiza tipo de usina existente
    /// </summary>
    Task UpdateAsync(TipoUsina tipoUsina);

    /// <summary>
    /// Remove tipo de usina (soft delete)
    /// </summary>
    Task DeleteAsync(TipoUsina tipoUsina);

    /// <summary>
    /// Verifica se já existe um tipo de usina com o nome informado
    /// </summary>
    Task<bool> ExisteNomeAsync(string nome, int? tipoUsinaIdExcluir = null);

    /// <summary>
    /// Salva alterações no banco de dados
    /// </summary>
    Task<int> SaveChangesAsync();
}
