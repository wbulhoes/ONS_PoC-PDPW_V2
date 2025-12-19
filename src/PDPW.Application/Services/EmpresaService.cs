using AutoMapper;
using PDPW.Application.DTOs.Empresa;
using PDPW.Application.Interfaces;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

/// <summary>
/// Serviço de Empresa
/// </summary>
public class EmpresaService : IEmpresaService
{
    private readonly IEmpresaRepository _repository;
    private readonly IMapper _mapper;

    public EmpresaService(
        IEmpresaRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtém todas as empresas
    /// </summary>
    public async Task<List<EmpresaDto>> GetAllAsync()
    {
        var empresas = await _repository.GetAllAsync();
        return _mapper.Map<List<EmpresaDto>>(empresas);
    }

    /// <summary>
    /// Obtém empresa por ID
    /// </summary>
    public async Task<EmpresaDto?> GetByIdAsync(int id)
    {
        var empresa = await _repository.GetByIdAsync(id);
        return _mapper.Map<EmpresaDto?>(empresa);
    }

    /// <summary>
    /// Obtém empresa por nome
    /// </summary>
    public async Task<EmpresaDto?> GetByNomeAsync(string nome)
    {
        var empresa = await _repository.GetByNomeAsync(nome);
        return _mapper.Map<EmpresaDto?>(empresa);
    }

    /// <summary>
    /// Obtém empresa por CNPJ
    /// </summary>
    public async Task<EmpresaDto?> GetByCnpjAsync(string cnpj)
    {
        var empresa = await _repository.GetByCnpjAsync(cnpj);
        return _mapper.Map<EmpresaDto?>(empresa);
    }

    /// <summary>
    /// Cria uma nova empresa
    /// </summary>
    public async Task<EmpresaDto> CreateAsync(CreateEmpresaDto dto)
    {
        // Validar se já existe empresa com o mesmo nome
        if (await ExisteNomeAsync(dto.Nome))
        {
            throw new InvalidOperationException($"Já existe uma empresa com o nome '{dto.Nome}'");
        }

        // Validar se já existe empresa com o mesmo CNPJ (se informado)
        if (!string.IsNullOrWhiteSpace(dto.CNPJ) && await ExisteCnpjAsync(dto.CNPJ))
        {
            throw new InvalidOperationException($"Já existe uma empresa com o CNPJ '{dto.CNPJ}'");
        }

        var empresa = _mapper.Map<Empresa>(dto);
        empresa.DataCriacao = DateTime.UtcNow;
        empresa.Ativo = true;

        await _repository.AddAsync(empresa);
        await _repository.SaveChangesAsync();

        // Recarregar para incluir relacionamentos
        var empresaCriada = await _repository.GetByIdAsync(empresa.Id);
        return _mapper.Map<EmpresaDto>(empresaCriada);
    }

    /// <summary>
    /// Atualiza uma empresa existente
    /// </summary>
    public async Task<EmpresaDto?> UpdateAsync(int id, UpdateEmpresaDto dto)
    {
        var empresa = await _repository.GetByIdAsync(id);
        if (empresa == null)
        {
            return null;
        }

        // Validar se já existe outra empresa com o mesmo nome
        if (await ExisteNomeAsync(dto.Nome, id))
        {
            throw new InvalidOperationException($"Já existe outra empresa com o nome '{dto.Nome}'");
        }

        // Validar se já existe outra empresa com o mesmo CNPJ (se informado)
        if (!string.IsNullOrWhiteSpace(dto.CNPJ) && await ExisteCnpjAsync(dto.CNPJ, id))
        {
            throw new InvalidOperationException($"Já existe outra empresa com o CNPJ '{dto.CNPJ}'");
        }

        _mapper.Map(dto, empresa);
        empresa.DataAtualizacao = DateTime.UtcNow;

        await _repository.UpdateAsync(empresa);
        await _repository.SaveChangesAsync();

        // Recarregar para incluir relacionamentos
        var empresaAtualizada = await _repository.GetByIdAsync(id);
        return _mapper.Map<EmpresaDto>(empresaAtualizada);
    }

    /// <summary>
    /// Remove uma empresa (soft delete)
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var empresa = await _repository.GetByIdAsync(id);
        if (empresa == null)
        {
            return false;
        }

        // Verificar se existem usinas vinculadas
        if (empresa.Usinas?.Any(u => u.Ativo) == true)
        {
            var quantidadeUsinas = empresa.Usinas.Count(u => u.Ativo);
            throw new InvalidOperationException($"Não é possível excluir esta empresa pois existem {quantidadeUsinas} usinas ativas vinculadas a ela");
        }

        await _repository.DeleteAsync(empresa);
        await _repository.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Verifica se já existe uma empresa com o nome informado
    /// </summary>
    public async Task<bool> ExisteNomeAsync(string nome, int? empresaIdExcluir = null)
    {
        return await _repository.ExisteNomeAsync(nome, empresaIdExcluir);
    }

    /// <summary>
    /// Verifica se já existe uma empresa com o CNPJ informado
    /// </summary>
    public async Task<bool> ExisteCnpjAsync(string cnpj, int? empresaIdExcluir = null)
    {
        return await _repository.ExisteCnpjAsync(cnpj, empresaIdExcluir);
    }
}
