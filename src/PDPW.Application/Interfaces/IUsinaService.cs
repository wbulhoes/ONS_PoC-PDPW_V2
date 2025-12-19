using PDPW.Application.DTOs.Usina;
using PDPW.Domain.Common;

namespace PDPW.Application.Interfaces;

/// <summary>
/// Interface para serviço de Usinas Geradoras
/// </summary>
/// <remarks>
/// Nomenclatura ubíqua: IUsinaGeradoraService (mantido como IUsinaService por compatibilidade)
/// </remarks>
public interface IUsinaService
{
    /// <summary>
    /// Obtém todas as usinas ativas
    /// </summary>
    /// <returns>Result com lista de usinas ou erro</returns>
    Task<Result<IEnumerable<UsinaDto>>> GetAllAsync();

    /// <summary>
    /// Obtém usina por ID
    /// </summary>
    /// <param name="id">ID da usina</param>
    /// <returns>Result com usina encontrada ou erro NotFound</returns>
    Task<Result<UsinaDto>> GetByIdAsync(int id);

    /// <summary>
    /// Obtém usina por código único
    /// </summary>
    /// <param name="codigo">Código da usina (ex: UTE001)</param>
    /// <returns>Result com usina encontrada ou erro NotFound</returns>
    Task<Result<UsinaDto>> GetByCodigoAsync(string codigo);

    /// <summary>
    /// Obtém usinas por tipo
    /// </summary>
    /// <param name="tipoUsinaId">ID do tipo de usina</param>
    /// <returns>Result com lista de usinas do tipo especificado</returns>
    Task<Result<IEnumerable<UsinaDto>>> GetByTipoAsync(int tipoUsinaId);

    /// <summary>
    /// Obtém usinas por empresa/agente
    /// </summary>
    /// <param name="empresaId">ID do agente do setor elétrico</param>
    /// <returns>Result com lista de usinas do agente especificado</returns>
    Task<Result<IEnumerable<UsinaDto>>> GetByEmpresaAsync(int empresaId);

    /// <summary>
    /// Cria nova usina geradora
    /// </summary>
    /// <param name="createDto">Dados da nova usina</param>
    /// <returns>Result com usina criada ou erro de validação/conflito</returns>
    Task<Result<UsinaDto>> CreateAsync(CreateUsinaDto createDto);

    /// <summary>
    /// Atualiza usina existente
    /// </summary>
    /// <param name="id">ID da usina</param>
    /// <param name="updateDto">Dados atualizados</param>
    /// <returns>Result com usina atualizada ou erro NotFound/validação</returns>
    Task<Result<UsinaDto>> UpdateAsync(int id, UpdateUsinaDto updateDto);

    /// <summary>
    /// Remove usina (soft delete)
    /// </summary>
    /// <param name="id">ID da usina</param>
    /// <returns>Result de sucesso ou erro NotFound</returns>
    Task<Result> DeleteAsync(int id);

    /// <summary>
    /// Verifica se código já existe
    /// </summary>
    /// <param name="codigo">Código a verificar</param>
    /// <param name="usinaIdExcluir">ID da usina a excluir da verificação (opcional)</param>
    /// <returns>True se código existe, False caso contrário</returns>
    Task<bool> CodigoExisteAsync(string codigo, int? usinaIdExcluir = null);
}
