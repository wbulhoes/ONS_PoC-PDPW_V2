using AutoMapper;
using PDPW.Application.DTOs.TipoUsina;
using PDPW.Application.Interfaces;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

/// <summary>
/// Serviço de Tipo de Usina
/// </summary>
public class TipoUsinaService : ITipoUsinaService
{
    private readonly ITipoUsinaRepository _repository;
    private readonly IMapper _mapper;

    public TipoUsinaService(
        ITipoUsinaRepository repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtém todos os tipos de usina
    /// </summary>
    public async Task<List<TipoUsinaDto>> GetAllAsync()
    {
        var tiposUsina = await _repository.GetAllAsync();
        return _mapper.Map<List<TipoUsinaDto>>(tiposUsina);
    }

    /// <summary>
    /// Obtém tipo de usina por ID
    /// </summary>
    public async Task<TipoUsinaDto?> GetByIdAsync(int id)
    {
        var tipoUsina = await _repository.GetByIdAsync(id);
        return _mapper.Map<TipoUsinaDto?>(tipoUsina);
    }

    /// <summary>
    /// Obtém tipo de usina por nome
    /// </summary>
    public async Task<TipoUsinaDto?> GetByNomeAsync(string nome)
    {
        var tipoUsina = await _repository.GetByNomeAsync(nome);
        return _mapper.Map<TipoUsinaDto?>(tipoUsina);
    }

    /// <summary>
    /// Cria um novo tipo de usina
    /// </summary>
    public async Task<TipoUsinaDto> CreateAsync(CreateTipoUsinaDto dto)
    {
        // Validar se já existe tipo com o mesmo nome
        if (await ExisteNomeAsync(dto.Nome))
        {
            throw new InvalidOperationException($"Já existe um tipo de usina com o nome '{dto.Nome}'");
        }

        var tipoUsina = _mapper.Map<TipoUsina>(dto);
        tipoUsina.DataCriacao = DateTime.UtcNow;
        tipoUsina.Ativo = true;

        await _repository.AddAsync(tipoUsina);
        await _repository.SaveChangesAsync();

        // Recarregar para incluir relacionamentos
        var tipoUsinaCriado = await _repository.GetByIdAsync(tipoUsina.Id);
        return _mapper.Map<TipoUsinaDto>(tipoUsinaCriado);
    }

    /// <summary>
    /// Atualiza um tipo de usina existente
    /// </summary>
    public async Task<TipoUsinaDto?> UpdateAsync(int id, UpdateTipoUsinaDto dto)
    {
        var tipoUsina = await _repository.GetByIdAsync(id);
        if (tipoUsina == null)
        {
            return null;
        }

        // Validar se já existe outro tipo com o mesmo nome
        if (await ExisteNomeAsync(dto.Nome, id))
        {
            throw new InvalidOperationException($"Já existe outro tipo de usina com o nome '{dto.Nome}'");
        }

        _mapper.Map(dto, tipoUsina);
        tipoUsina.DataAtualizacao = DateTime.UtcNow;

        await _repository.UpdateAsync(tipoUsina);
        await _repository.SaveChangesAsync();

        // Recarregar para incluir relacionamentos
        var tipoUsinaAtualizado = await _repository.GetByIdAsync(id);
        return _mapper.Map<TipoUsinaDto>(tipoUsinaAtualizado);
    }

    /// <summary>
    /// Remove um tipo de usina (soft delete)
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var tipoUsina = await _repository.GetByIdAsync(id);
        if (tipoUsina == null)
        {
            return false;
        }

        // Verificar se existem usinas vinculadas
        if (tipoUsina.Usinas?.Any(u => u.Ativo) == true)
        {
            var quantidadeUsinas = tipoUsina.Usinas.Count(u => u.Ativo);
            throw new InvalidOperationException($"Não é possível excluir este tipo de usina pois existem {quantidadeUsinas} usinas ativas vinculadas a ele");
        }

        await _repository.DeleteAsync(tipoUsina);
        await _repository.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Verifica se já existe um tipo com o nome informado
    /// </summary>
    public async Task<bool> ExisteNomeAsync(string nome, int? tipoUsinaIdExcluir = null)
    {
        return await _repository.ExisteNomeAsync(nome, tipoUsinaIdExcluir);
    }
}
