using AutoMapper;
using PDPW.Application.DTOs.Usina;
using PDPW.Application.Interfaces;
using PDPW.Domain.Common;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

/// <summary>
/// Serviço de Usinas Geradoras
/// </summary>
/// <remarks>
/// Nomenclatura ubíqua: UsinaGeradoraService (mantido como UsinaService por compatibilidade)
/// </remarks>
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
    public async Task<Result<IEnumerable<UsinaDto>>> GetAllAsync()
    {
        var usinas = await _repository.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<UsinaDto>>(usinas);
        return Result<IEnumerable<UsinaDto>>.Success(dtos);
    }

    /// <summary>
    /// Obtém usina por ID
    /// </summary>
    public async Task<Result<UsinaDto>> GetByIdAsync(int id)
    {
        var usina = await _repository.GetByIdAsync(id);
        
        if (usina == null)
        {
            return Result<UsinaDto>.NotFound("Usina", id);
        }

        var dto = _mapper.Map<UsinaDto>(usina);
        return Result<UsinaDto>.Success(dto);
    }

    /// <summary>
    /// Obtém usina por código
    /// </summary>
    public async Task<Result<UsinaDto>> GetByCodigoAsync(string codigo)
    {
        var usina = await _repository.GetByCodigoAsync(codigo);
        
        if (usina == null)
        {
            return Result<UsinaDto>.Failure($"Usina com código '{codigo}' não foi encontrada");
        }

        var dto = _mapper.Map<UsinaDto>(usina);
        return Result<UsinaDto>.Success(dto);
    }

    /// <summary>
    /// Obtém usinas por tipo
    /// </summary>
    public async Task<Result<IEnumerable<UsinaDto>>> GetByTipoAsync(int tipoUsinaId)
    {
        var usinas = await _repository.GetByTipoAsync(tipoUsinaId);
        var dtos = _mapper.Map<IEnumerable<UsinaDto>>(usinas);
        return Result<IEnumerable<UsinaDto>>.Success(dtos);
    }

    /// <summary>
    /// Obtém usinas por empresa
    /// </summary>
    public async Task<Result<IEnumerable<UsinaDto>>> GetByEmpresaAsync(int empresaId)
    {
        // Validação: código de empresa não pode ser zero ou negativo
        if (empresaId <= 0)
        {
            return Result<IEnumerable<UsinaDto>>.Failure("Código da empresa não informado");
        }

        var usinas = await _repository.GetByEmpresaAsync(empresaId);
        var dtos = _mapper.Map<IEnumerable<UsinaDto>>(usinas);
        return Result<IEnumerable<UsinaDto>>.Success(dtos);
    }

    /// <summary>
    /// Cria nova usina
    /// </summary>
    public async Task<Result<UsinaDto>> CreateAsync(CreateUsinaDto createDto)
    {
        // Validações de campos obrigatórios (conforme UsinaDAO.vb)
        if (string.IsNullOrWhiteSpace(createDto.Codigo))
        {
            return Result<UsinaDto>.Failure("Código da usina é obrigatório");
        }

        if (string.IsNullOrWhiteSpace(createDto.Nome))
        {
            return Result<UsinaDto>.Failure("Nome da usina é obrigatório");
        }

        // Validar código único
        if (await _repository.CodigoExisteAsync(createDto.Codigo))
        {
            return Result<UsinaDto>.Conflict($"Já existe uma usina com o código '{createDto.Codigo}'");
        }

        var usina = _mapper.Map<Usina>(createDto);
        var created = await _repository.AddAsync(usina);

        // Buscar novamente para incluir relacionamentos
        var result = await _repository.GetByIdAsync(created.Id);
        var dto = _mapper.Map<UsinaDto>(result);
        
        return Result<UsinaDto>.Success(dto);
    }

    /// <summary>
    /// Atualiza usina existente
    /// </summary>
    public async Task<Result<UsinaDto>> UpdateAsync(int id, UpdateUsinaDto updateDto)
    {
        // Validações de campos obrigatórios
        if (string.IsNullOrWhiteSpace(updateDto.Codigo))
        {
            return Result<UsinaDto>.Failure("Código da usina é obrigatório");
        }

        if (string.IsNullOrWhiteSpace(updateDto.Nome))
        {
            return Result<UsinaDto>.Failure("Nome da usina é obrigatório");
        }

        var usina = await _repository.GetByIdAsync(id);
        if (usina == null)
        {
            return Result<UsinaDto>.NotFound("Usina", id);
        }

        // Validar código único (exceto para a própria usina)
        if (await _repository.CodigoExisteAsync(updateDto.Codigo, id))
        {
            return Result<UsinaDto>.Conflict($"Já existe outra usina com o código '{updateDto.Codigo}'");
        }

        // Mapear dados atualizados
        _mapper.Map(updateDto, usina);

        await _repository.UpdateAsync(usina);

        // Buscar novamente para incluir relacionamentos
        var result = await _repository.GetByIdAsync(id);
        var dto = _mapper.Map<UsinaDto>(result);
        
        return Result<UsinaDto>.Success(dto);
    }

    /// <summary>
    /// Remove usina
    /// </summary>
    public async Task<Result> DeleteAsync(int id)
    {
        if (!await _repository.ExistsAsync(id))
        {
            return Result.Failure($"Usina com ID {id} não foi encontrada");
        }

        await _repository.DeleteAsync(id);
        return Result.Success();
    }

    /// <summary>
    /// Verifica se código já existe
    /// </summary>
    public async Task<bool> CodigoExisteAsync(string codigo, int? usinaIdExcluir = null)
    {
        return await _repository.CodigoExisteAsync(codigo, usinaIdExcluir);
    }
}
