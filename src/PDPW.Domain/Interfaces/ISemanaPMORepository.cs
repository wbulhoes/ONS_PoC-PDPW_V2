using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Interface do repositório de Semana PMO
/// </summary>
public interface ISemanaPMORepository
{
    /// <summary>
    /// Obtém todas as semanas PMO ativas
    /// </summary>
    Task<IEnumerable<SemanaPMO>> ObterTodosAsync();

    /// <summary>
    /// Obtém semana PMO por ID
    /// </summary>
    Task<SemanaPMO?> ObterPorIdAsync(int id);

    /// <summary>
    /// Adiciona nova semana PMO
    /// </summary>
    Task<SemanaPMO> AdicionarAsync(SemanaPMO semana);

    /// <summary>
    /// Atualiza semana PMO existente
    /// </summary>
    Task AtualizarAsync(SemanaPMO semana);

    /// <summary>
    /// Remove semana PMO (soft delete)
    /// </summary>
    Task RemoverAsync(int id);

    /// <summary>
    /// Obtém todas as semanas de um ano específico
    /// </summary>
    Task<IEnumerable<SemanaPMO>> ObterPorAnoAsync(int ano);

    /// <summary>
    /// Obtém a semana PMO atual (baseada na data de hoje)
    /// </summary>
    Task<SemanaPMO?> ObterSemanaAtualAsync();

    /// <summary>
    /// Obtém semana por número e ano
    /// </summary>
    Task<SemanaPMO?> ObterPorNumeroEAnoAsync(int numero, int ano);

    /// <summary>
    /// Verifica se já existe uma semana com o mesmo número e ano
    /// </summary>
    Task<bool> ExisteNumeroAnoAsync(int numero, int ano, int? excluirId = null);

    /// <summary>
    /// Obtém semanas em um período específico
    /// </summary>
    Task<IEnumerable<SemanaPMO>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);

    /// <summary>
    /// Obtém as próximas N semanas a partir de hoje
    /// </summary>
    Task<IEnumerable<SemanaPMO>> ObterProximasSemanasAsync(int quantidade);
}
