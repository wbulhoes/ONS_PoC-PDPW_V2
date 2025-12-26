using AutoMapper;
using PDPW.Application.DTOs.PrevisaoEolica;
using PDPW.Application.Interfaces;
using PDPW.Domain.Common;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

public class PrevisaoEolicaService : IPrevisaoEolicaService
{
    private readonly IPrevisaoEolicaRepository _repository;
    private readonly IUsinaRepository _usinaRepository;
    private readonly IMapper _mapper;

    public PrevisaoEolicaService(
        IPrevisaoEolicaRepository repository,
        IUsinaRepository usinaRepository,
        IMapper mapper)
    {
        _repository = repository;
        _usinaRepository = usinaRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<PrevisaoEolicaDto>>> GetAllAsync()
    {
        var previsoes = await _repository.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<PrevisaoEolicaDto>>(previsoes);
        return Result<IEnumerable<PrevisaoEolicaDto>>.Success(dtos);
    }

    public async Task<Result<PrevisaoEolicaDto>> GetByIdAsync(int id)
    {
        var previsao = await _repository.GetByIdAsync(id);
        if (previsao == null)
        {
            return Result<PrevisaoEolicaDto>.NotFound("Previsão Eólica", id);
        }

        var dto = _mapper.Map<PrevisaoEolicaDto>(previsao);
        return Result<PrevisaoEolicaDto>.Success(dto);
    }

    public async Task<Result<IEnumerable<PrevisaoEolicaDto>>> GetByUsinaAsync(int usinaId)
    {
        var usina = await _usinaRepository.GetByIdAsync(usinaId);
        if (usina == null)
        {
            return Result<IEnumerable<PrevisaoEolicaDto>>.Failure($"Usina com ID {usinaId} não encontrada");
        }

        var previsoes = await _repository.GetByUsinaAsync(usinaId);
        var dtos = _mapper.Map<IEnumerable<PrevisaoEolicaDto>>(previsoes);
        return Result<IEnumerable<PrevisaoEolicaDto>>.Success(dtos);
    }

    public async Task<Result<IEnumerable<PrevisaoEolicaDto>>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        if (dataInicio > dataFim)
        {
            return Result<IEnumerable<PrevisaoEolicaDto>>.Failure("Data inicial não pode ser maior que data final");
        }

        var previsoes = await _repository.GetByPeriodoAsync(dataInicio, dataFim);
        var dtos = _mapper.Map<IEnumerable<PrevisaoEolicaDto>>(previsoes);
        return Result<IEnumerable<PrevisaoEolicaDto>>.Success(dtos);
    }

    public async Task<Result<IEnumerable<PrevisaoEolicaDto>>> GetByModeloAsync(string modelo)
    {
        var previsoes = await _repository.GetByModeloAsync(modelo);
        var dtos = _mapper.Map<IEnumerable<PrevisaoEolicaDto>>(previsoes);
        return Result<IEnumerable<PrevisaoEolicaDto>>.Success(dtos);
    }

    public async Task<Result<IEnumerable<PrevisaoEolicaDto>>> GetUltimasPrevisoes(int usinaId, int quantidade = 10)
    {
        var usina = await _usinaRepository.GetByIdAsync(usinaId);
        if (usina == null)
        {
            return Result<IEnumerable<PrevisaoEolicaDto>>.Failure($"Usina com ID {usinaId} não encontrada");
        }

        var previsoes = await _repository.GetUltimasPrevisoes(usinaId, quantidade);
        var dtos = _mapper.Map<IEnumerable<PrevisaoEolicaDto>>(previsoes);
        return Result<IEnumerable<PrevisaoEolicaDto>>.Success(dtos);
    }

    public async Task<Result<PrevisaoEolicaDto>> CreateAsync(CreatePrevisaoEolicaDto dto)
    {
        var usina = await _usinaRepository.GetByIdAsync(dto.UsinaId);
        if (usina == null)
        {
            return Result<PrevisaoEolicaDto>.Failure($"Usina com ID {dto.UsinaId} não encontrada");
        }

        if (dto.DataHoraPrevista <= dto.DataHoraReferencia)
        {
            return Result<PrevisaoEolicaDto>.Failure("Data/hora prevista deve ser posterior à data de referência");
        }

        var previsao = _mapper.Map<PrevisaoEolica>(dto);
        var created = await _repository.AddAsync(previsao);

        var result = await _repository.GetByIdAsync(created.Id);
        var resultDto = _mapper.Map<PrevisaoEolicaDto>(result);

        return Result<PrevisaoEolicaDto>.Success(resultDto);
    }

    public async Task<Result<PrevisaoEolicaDto>> UpdateAsync(int id, UpdatePrevisaoEolicaDto dto)
    {
        var previsao = await _repository.GetByIdAsync(id);
        if (previsao == null)
        {
            return Result<PrevisaoEolicaDto>.NotFound("Previsão Eólica", id);
        }

        var usina = await _usinaRepository.GetByIdAsync(dto.UsinaId);
        if (usina == null)
        {
            return Result<PrevisaoEolicaDto>.Failure($"Usina com ID {dto.UsinaId} não encontrada");
        }

        if (dto.DataHoraPrevista <= dto.DataHoraReferencia)
        {
            return Result<PrevisaoEolicaDto>.Failure("Data/hora prevista deve ser posterior à data de referência");
        }

        _mapper.Map(dto, previsao);
        await _repository.UpdateAsync(previsao);

        var result = await _repository.GetByIdAsync(id);
        var resultDto = _mapper.Map<PrevisaoEolicaDto>(result);

        return Result<PrevisaoEolicaDto>.Success(resultDto);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var previsao = await _repository.GetByIdAsync(id);
        if (previsao == null)
        {
            return Result.Failure($"Previsão Eólica com ID {id} não encontrada");
        }

        await _repository.DeleteAsync(id);
        return Result.Success();
    }

    public async Task<Result> AtualizarGeracaoRealAsync(int id, AtualizarGeracaoRealDto dto)
    {
        var previsao = await _repository.GetByIdAsync(id);
        if (previsao == null)
        {
            return Result.Failure($"Previsão Eólica com ID {id} não encontrada");
        }

        await _repository.AtualizarGeracaoRealAsync(id, dto.GeracaoRealMWmed);
        return Result.Success();
    }

    public async Task<Result<EstatisticasPrevisaoDto>> GetEstatisticasAsync(int usinaId, DateTime dataInicio, DateTime dataFim)
    {
        var usina = await _usinaRepository.GetByIdAsync(usinaId);
        if (usina == null)
        {
            return Result<EstatisticasPrevisaoDto>.Failure($"Usina com ID {usinaId} não encontrada");
        }

        var previsoesComErro = await _repository.GetPrevisoesComErroAsync(usinaId);
        var mae = await _repository.CalcularMAE(usinaId, dataInicio, dataFim);
        var rmse = await _repository.CalcularRMSE(usinaId, dataInicio, dataFim);

        var estatisticas = new EstatisticasPrevisaoDto
        {
            UsinaId = usinaId,
            UsinaNome = usina.Nome,
            TotalPrevisoes = previsoesComErro.Count(),
            PrevisoesComErro = previsoesComErro.Count(p => p.ErroAbsolutoMW.HasValue),
            MAE = mae,
            RMSE = rmse,
            ErroMedioPercentual = previsoesComErro.Any() 
                ? previsoesComErro.Average(p => p.ErroPercentual ?? 0) 
                : 0,
            PeriodoInicio = dataInicio,
            PeriodoFim = dataFim
        };

        return Result<EstatisticasPrevisaoDto>.Success(estatisticas);
    }
}
