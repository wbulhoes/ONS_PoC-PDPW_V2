using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Interface para repositório de dados energéticos
/// </summary>
public interface IDadoEnergeticoRepository
{
    Task<IEnumerable<DadoEnergetico>> ObterTodosAsync();
    Task<DadoEnergetico?> ObterPorIdAsync(int id);
    Task<DadoEnergetico> AdicionarAsync(DadoEnergetico dado);
    Task AtualizarAsync(DadoEnergetico dado);
    Task RemoverAsync(int id);
    Task<IEnumerable<DadoEnergetico>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
}
