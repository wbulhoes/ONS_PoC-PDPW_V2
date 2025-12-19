using PDPW.Application.DTOs.Empresa;

namespace PDPW.Application.Interfaces;

/// <summary>
/// Interface do serviço de Empresa
/// </summary>
public interface IEmpresaService
{
    /// <summary>
    /// Obtém todas as empresas
    /// </summary>
    Task<List<EmpresaDto>> GetAllAsync();

    /// <summary>
    /// Obtém empresa por ID
    /// </summary>
    Task<EmpresaDto?> GetByIdAsync(int id);

    /// <summary>
    /// Obtém empresa por nome
    /// </summary>
    Task<EmpresaDto?> GetByNomeAsync(string nome);

    /// <summary>
    /// Obtém empresa por CNPJ
    /// </summary>
    Task<EmpresaDto?> GetByCnpjAsync(string cnpj);

    /// <summary>
    /// Cria uma nova empresa
    /// </summary>
    Task<EmpresaDto> CreateAsync(CreateEmpresaDto dto);

    /// <summary>
    /// Atualiza uma empresa existente
    /// </summary>
    Task<EmpresaDto?> UpdateAsync(int id, UpdateEmpresaDto dto);

    /// <summary>
    /// Remove uma empresa (soft delete)
    /// </summary>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// Verifica se já existe uma empresa com o nome informado
    /// </summary>
    Task<bool> ExisteNomeAsync(string nome, int? empresaIdExcluir = null);

    /// <summary>
    /// Verifica se já existe uma empresa com o CNPJ informado
    /// </summary>
    Task<bool> ExisteCnpjAsync(string cnpj, int? empresaIdExcluir = null);
}
