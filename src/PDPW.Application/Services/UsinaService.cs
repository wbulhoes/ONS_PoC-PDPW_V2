using AutoMapper;
using PDPW.Application.DTOs.Usina;
using PDPW.Application.Interfaces;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

/// <summary>
/// Serviço de Usinas
/// </summary>
public class UsinaService : IUsinaService
{
    private readonly IUsinaRepository _repository;
    private readonly IMapper _mapper;

    public UsinaService(IUsinaRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtém todas as usinas
    /// </summary>
    public async Task<IEnumerable<UsinaDto>> GetAllAsync()
    {
        var usinas = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<UsinaDto>>(usinas);
    }

    /// <summary>
    /// Obtém usina por ID
    /// </summary>
    public async Task<UsinaDto?> GetByIdAsync(int id)
    {
        var usina = await _repository.GetByIdAsync(id);
        return _mapper.Map<UsinaDto?>(usina);
    }

    /// <summary>
    /// Obtém usina por código
    /// </summary>
    public async Task<UsinaDto?> GetByCodigoAsync(string codigo)
    {
        var usina = await _repository.GetByCodigoAsync(codigo);
        return _mapper.Map<UsinaDto?>(usina);
    }

    /// <summary>
    /// Obtém usinas por tipo
    /// </summary>
    public async Task<IEnumerable<UsinaDto>> GetByTipoAsync(int tipoUsinaId)
    {
        var usinas = await _repository.GetByTipoAsync(tipoUsinaId);
        return _mapper.Map<IEnumerable<UsinaDto>>(usinas);
    }

    /// <summary>
    /// Obtém usinas por empresa
    /// </summary>
    public async Task<IEnumerable<UsinaDto>> GetByEmpresaAsync(int empresaId)
    {
        var usinas = await _repository.GetByEmpresaAsync(empresaId);
        return _mapper.Map<IEnumerable<UsinaDto>>(usinas);
    }

    /// <summary>
    /// Cria nova usina
    /// </summary>
    public async Task<UsinaDto> CreateAsync(CreateUsinaDto createDto)
    {
        // Validar código único
        if (await _repository.CodigoExisteAsync(createDto.Codigo))
        {
            throw new InvalidOperationException($"Já existe uma usina com o código '{createDto.Codigo}'");
        }

        var usina = _mapper.Map<Usina>(createDto);
        var created = await _repository.AddAsync(usina);

        // Buscar novamente para incluir relacionamentos
        var result = await _repository.GetByIdAsync(created.Id);
        return _mapper.Map<UsinaDto>(result);
    }

    /// <summary>
    /// Atualiza usina existente
    /// </summary>
    public async Task<UsinaDto> UpdateAsync(int id, UpdateUsinaDto updateDto)
    {
        var usina = await _repository.GetByIdAsync(id);
        if (usina == null)
        {
            throw new KeyNotFoundException($"Usina com ID {id} não encontrada");
        }

        // Validar código único (exceto para a própria usina)
        if (await _repository.CodigoExisteAsync(updateDto.Codigo, id))
        {
            throw new InvalidOperationException($"Já existe outra usina com o código '{updateDto.Codigo}'");
        }

        // Mapear dados atualizados
        _mapper.Map(updateDto, usina);

        await _repository.UpdateAsync(usina);

        // Buscar novamente para incluir relacionamentos
        var result = await _repository.GetByIdAsync(id);
        return _mapper.Map<UsinaDto>(result);
    }

    /// <summary>
    /// Remove usina
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        if (!await _repository.ExistsAsync(id))
        {
            return false;
        }

        await _repository.DeleteAsync(id);
        return true;
    }

    /// <summary>
    /// Verifica se código já existe
    /// </summary>
    public async Task<bool> CodigoExisteAsync(string codigo, int? usinaIdExcluir = null)
    {
        return await _repository.CodigoExisteAsync(codigo, usinaIdExcluir);
    }
}
