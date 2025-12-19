using AutoMapper;
using PDPW.Application.DTOs.SemanaPmo;
using PDPW.Application.Interfaces;
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

    public async Task<SemanaPmoDto> CreateAsync(CreateSemanaPmoDto dto)
    {
        // Validar data fim maior que data início
        if (dto.DataFim <= dto.DataInicio)
        {
            throw new ArgumentException("A data de fim deve ser maior que a data de início");
        }

        // Validar período de 7 dias
        var dias = (dto.DataFim - dto.DataInicio).Days + 1;
        if (dias != 7)
        {
            throw new ArgumentException($"O período deve ser de 7 dias. Período informado: {dias} dias");
        }

        // Validar ano consistente com datas
        if (dto.DataInicio.Year != dto.Ano && dto.DataFim.Year != dto.Ano)
        {
            throw new ArgumentException("O ano deve corresponder ao ano das datas de início ou fim");
        }

        // Validar duplicidade número + ano
        if (await _repository.ExisteNumeroAnoAsync(dto.Numero, dto.Ano))
        {
            throw new InvalidOperationException($"Já existe uma semana PMO com número {dto.Numero} no ano {dto.Ano}");
        }

        var semana = _mapper.Map<SemanaPMO>(dto);
        var semanaCriada = await _repository.AdicionarAsync(semana);
        return _mapper.Map<SemanaPmoDto>(semanaCriada);
    }

    public async Task<SemanaPmoDto?> UpdateAsync(int id, UpdateSemanaPmoDto dto)
    {
        var semanaExistente = await _repository.ObterPorIdAsync(id);
        if (semanaExistente == null)
        {
            return null;
        }

        // Validar data fim maior que data início
        if (dto.DataFim <= dto.DataInicio)
        {
            throw new ArgumentException("A data de fim deve ser maior que a data de início");
        }

        // Validar período de 7 dias
        var dias = (dto.DataFim - dto.DataInicio).Days + 1;
        if (dias != 7)
        {
            throw new ArgumentException($"O período deve ser de 7 dias. Período informado: {dias} dias");
        }

        // Validar ano consistente com datas
        if (dto.DataInicio.Year != dto.Ano && dto.DataFim.Year != dto.Ano)
        {
            throw new ArgumentException("O ano deve corresponder ao ano das datas de início ou fim");
        }

        // Validar duplicidade número + ano (excluindo o próprio registro)
        if (await _repository.ExisteNumeroAnoAsync(dto.Numero, dto.Ano, id))
        {
            throw new InvalidOperationException($"Já existe outra semana PMO com número {dto.Numero} no ano {dto.Ano}");
        }

        _mapper.Map(dto, semanaExistente);
        await _repository.AtualizarAsync(semanaExistente);
        return _mapper.Map<SemanaPmoDto>(semanaExistente);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var semana = await _repository.ObterPorIdAsync(id);
        if (semana == null)
        {
            return false;
        }

        // Verificar se tem arquivos vinculados
        if (semana.ArquivosDadger?.Any(a => a.Ativo) == true)
        {
            throw new InvalidOperationException("Não é possível remover uma semana PMO com arquivos DADGER vinculados");
        }

        await _repository.RemoverAsync(id);
        return true;
    }

    public async Task<bool> ExisteNumeroAnoAsync(int numero, int ano, int? excluirId = null)
    {
        return await _repository.ExisteNumeroAnoAsync(numero, ano, excluirId);
    }
}
