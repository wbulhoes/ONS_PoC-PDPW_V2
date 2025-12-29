using AutoMapper;
using PDPW.Application.DTOs.SemanaPmo;
using PDPW.Application.Interfaces;
using PDPW.Domain.Common;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

/// <summary>
/// Serviço de Semana PMO
/// </summary>
public class SemanaPmoService : ISemanaPmoService
{
    private readonly ISemanaPMORepository _repository;
    private readonly IMapper _mapper;

    public SemanaPmoService(ISemanaPMORepository repository, IMapper _mapper)
    {
        _repository = repository;
        this._mapper = _mapper;
    }

    public async Task<IEnumerable<SemanaPmoDto>> GetAllAsync()
    {
        var semanas = await _repository.ObterTodosAsync();
        return _mapper.Map<IEnumerable<SemanaPmoDto>>(semanas);
    }

    public async Task<SemanaPmoDto?> GetByIdAsync(int id)
    {
        var semana = await _repository.ObterPorIdAsync(id);
        return semana != null ? _mapper.Map<SemanaPmoDto>(semana) : null;
    }

    public async Task<IEnumerable<SemanaPmoDto>> GetByAnoAsync(int ano)
    {
        var semanas = await _repository.ObterPorAnoAsync(ano);
        return _mapper.Map<IEnumerable<SemanaPmoDto>>(semanas);
    }

    public async Task<SemanaPmoDto?> GetByNumeroAnoAsync(int numero, int ano)
    {
        var semana = await _repository.ObterPorNumeroEAnoAsync(numero, ano);
        return semana != null ? _mapper.Map<SemanaPmoDto>(semana) : null;
    }

    public async Task<SemanaPmoDto?> GetByDataAsync(DateTime data)
    {
        var semanas = await _repository.ObterPorPeriodoAsync(data, data);
        var semana = semanas.FirstOrDefault();
        return semana != null ? _mapper.Map<SemanaPmoDto>(semana) : null;
    }

    public async Task<Result<SemanaPmoDto>> CreateAsync(CreateSemanaPmoDto dto)
    {
        if (dto.DataFim <= dto.DataInicio)
        {
            return Result<SemanaPmoDto>.Failure("A data de fim deve ser maior que a data de início");
        }

        var dias = (dto.DataFim - dto.DataInicio).Days + 1;
        if (dias != 7)
        {
            return Result<SemanaPmoDto>.Failure($"O período deve ser de 7 dias. Período informado: {dias} dias");
        }

        if (dto.DataInicio.Year != dto.Ano && dto.DataFim.Year != dto.Ano)
        {
            return Result<SemanaPmoDto>.Failure("O ano deve corresponder ao ano das datas de início ou fim");
        }

        if (await _repository.ExisteNumeroAnoAsync(dto.Numero, dto.Ano))
        {
            return Result<SemanaPmoDto>.Failure($"Já existe uma semana PMO com número {dto.Numero} no ano {dto.Ano}");
        }

        var semana = _mapper.Map<SemanaPMO>(dto);
        var semanaCriada = await _repository.AdicionarAsync(semana);
        return Result<SemanaPmoDto>.Success(_mapper.Map<SemanaPmoDto>(semanaCriada));
    }

    public async Task<Result<SemanaPmoDto>> UpdateAsync(int id, UpdateSemanaPmoDto dto)
    {
        var semanaExistente = await _repository.ObterPorIdAsync(id);
        if (semanaExistente == null)
        {
            return Result<SemanaPmoDto>.NotFound("Semana PMO", id);
        }

        if (dto.DataFim <= dto.DataInicio)
        {
            return Result<SemanaPmoDto>.Failure("A data de fim deve ser maior que a data de início");
        }

        var dias = (dto.DataFim - dto.DataInicio).Days + 1;
        if (dias != 7)
        {
            return Result<SemanaPmoDto>.Failure($"O período deve ser de 7 dias. Período informado: {dias} dias");
        }

        if (dto.DataInicio.Year != dto.Ano && dto.DataFim.Year != dto.Ano)
        {
            return Result<SemanaPmoDto>.Failure("O ano deve corresponder ao ano das datas de início ou fim");
        }

        if (await _repository.ExisteNumeroAnoAsync(dto.Numero, dto.Ano, id))
        {
            return Result<SemanaPmoDto>.Failure($"Já existe outra semana PMO com número {dto.Numero} no ano {dto.Ano}");
        }

        _mapper.Map(dto, semanaExistente);
        await _repository.AtualizarAsync(semanaExistente);
        return Result<SemanaPmoDto>.Success(_mapper.Map<SemanaPmoDto>(semanaExistente));
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var semana = await _repository.ObterPorIdAsync(id);
        if (semana == null)
        {
            return Result.Failure($"Semana PMO com ID {id} não encontrada");
        }

        if (semana.ArquivosDadger?.Any(a => a.Ativo) == true)
        {
            return Result.Failure("Não é possível remover uma semana PMO com arquivos DADGER vinculados");
        }

        await _repository.RemoverAsync(id);
        return Result.Success();
    }

    public async Task<bool> ExisteNumeroAnoAsync(int numero, int ano, int? excluirId = null)
    {
        return await _repository.ExisteNumeroAnoAsync(numero, ano, excluirId);
    }
}
