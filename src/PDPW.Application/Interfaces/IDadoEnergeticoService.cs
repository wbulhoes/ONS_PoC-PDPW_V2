using PDPW.Application.DTOs;

namespace PDPW.Application.Interfaces;

/// <summary>
/// Interface do serviço de aplicação para dados energéticos
/// </summary>
public interface IDadoEnergeticoService
{
    Task<IEnumerable<DadoEnergeticoDto>> ObterTodosAsync();
    Task<DadoEnergeticoDto?> ObterPorIdAsync(int id);
    Task<DadoEnergeticoDto> CriarAsync(CriarDadoEnergeticoDto dto);
    Task AtualizarAsync(int id, AtualizarDadoEnergeticoDto dto);
    Task RemoverAsync(int id);
    Task<IEnumerable<DadoEnergeticoDto>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
}
