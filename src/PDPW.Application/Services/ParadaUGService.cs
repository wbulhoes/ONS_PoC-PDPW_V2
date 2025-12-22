using AutoMapper;
using Microsoft.Extensions.Logging;
using PDPW.Application.DTOs.ParadaUG;
using PDPW.Application.Interfaces;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

/// <summary>
/// Serviço de negócio para Paradas de Unidades Geradoras
/// </summary>
public class ParadaUGService : IParadaUGService
{
    private readonly IParadaUGRepository _repository;
    private readonly IUnidadeGeradoraRepository _unidadeGeradoraRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ParadaUGService> _logger;

    public ParadaUGService(
        IParadaUGRepository repository,
        IUnidadeGeradoraRepository unidadeGeradoraRepository,
        IMapper mapper,
        ILogger<ParadaUGService> logger)
    {
        _repository = repository;
        _unidadeGeradoraRepository = unidadeGeradoraRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ParadaUGDto>> GetAllAsync()
    {
        try
        {
            var paradas = await _repository.GetAllWithUnidadeAsync();
            return _mapper.Map<IEnumerable<ParadaUGDto>>(paradas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar todas as paradas de UG");
            throw;
        }
    }

    public async Task<ParadaUGDto?> GetByIdAsync(int id)
    {
        try
        {
            var parada = await _repository.GetByIdWithUnidadeAsync(id);
            return parada == null ? null : _mapper.Map<ParadaUGDto>(parada);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar parada de UG com ID {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<ParadaUGDto>> GetByUnidadeGeradoraAsync(int unidadeGeradoraId)
    {
        try
        {
            var paradas = await _repository.GetByUnidadeGeradoraIdAsync(unidadeGeradoraId);
            return _mapper.Map<IEnumerable<ParadaUGDto>>(paradas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar paradas da unidade geradora {UnidadeId}", unidadeGeradoraId);
            throw;
        }
    }

    public async Task<IEnumerable<ParadaUGDto>> GetProgramadasAsync()
    {
        try
        {
            var paradas = await _repository.GetProgramadasAsync();
            return _mapper.Map<IEnumerable<ParadaUGDto>>(paradas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar paradas programadas");
            throw;
        }
    }

    public async Task<IEnumerable<ParadaUGDto>> GetNaoProgramadasAsync()
    {
        try
        {
            var paradas = await _repository.GetNaoProgramadasAsync();
            return _mapper.Map<IEnumerable<ParadaUGDto>>(paradas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar paradas não programadas");
            throw;
        }
    }

    public async Task<IEnumerable<ParadaUGDto>> GetAtivasAsync(DateTime dataReferencia)
    {
        try
        {
            var paradas = await _repository.GetAtivasAsync(dataReferencia);
            return _mapper.Map<IEnumerable<ParadaUGDto>>(paradas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar paradas ativas na data {Data}", dataReferencia);
            throw;
        }
    }

    public async Task<IEnumerable<ParadaUGDto>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        try
        {
            var paradas = await _repository.GetByPeriodoAsync(dataInicio, dataFim);
            return _mapper.Map<IEnumerable<ParadaUGDto>>(paradas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar paradas no período {Inicio} a {Fim}", dataInicio, dataFim);
            throw;
        }
    }

    public async Task<ParadaUGDto> CreateAsync(CreateParadaUGDto dto)
    {
        try
        {
            // Validar se a unidade geradora existe
            if (!await _unidadeGeradoraRepository.ExistsAsync(dto.UnidadeGeradoraId))
            {
                throw new ArgumentException($"Unidade geradora com ID {dto.UnidadeGeradoraId} não encontrada");
            }

            // Validar datas
            if (dto.DataFim.HasValue && dto.DataFim.Value < dto.DataInicio)
            {
                throw new ArgumentException("A data de fim não pode ser anterior à data de início");
            }

            var parada = _mapper.Map<ParadaUG>(dto);
            parada.DataCriacao = DateTime.UtcNow;
            parada.Ativo = true;

            await _repository.AddAsync(parada);

            _logger.LogInformation("Parada de UG criada com sucesso para unidade {UnidadeId}", dto.UnidadeGeradoraId);

            return _mapper.Map<ParadaUGDto>(parada);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar parada de UG");
            throw;
        }
    }

    public async Task<ParadaUGDto> UpdateAsync(int id, UpdateParadaUGDto dto)
    {
        try
        {
            var parada = await _repository.GetByIdAsync(id);
            if (parada == null)
            {
                throw new KeyNotFoundException($"Parada de UG com ID {id} não encontrada");
            }

            // Validar se a unidade geradora existe
            if (!await _unidadeGeradoraRepository.ExistsAsync(dto.UnidadeGeradoraId))
            {
                throw new ArgumentException($"Unidade geradora com ID {dto.UnidadeGeradoraId} não encontrada");
            }

            // Validar datas
            if (dto.DataFim.HasValue && dto.DataFim.Value < dto.DataInicio)
            {
                throw new ArgumentException("A data de fim não pode ser anterior à data de início");
            }

            _mapper.Map(dto, parada);
            parada.DataAtualizacao = DateTime.UtcNow;

            await _repository.UpdateAsync(parada);

            _logger.LogInformation("Parada de UG {Id} atualizada com sucesso", id);

            return _mapper.Map<ParadaUGDto>(parada);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar parada de UG {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var parada = await _repository.GetByIdAsync(id);
            if (parada == null)
            {
                return false;
            }

            // Soft delete
            parada.Ativo = false;
            parada.DataAtualizacao = DateTime.UtcNow;
            await _repository.UpdateAsync(parada);

            _logger.LogInformation("Parada de UG {Id} removida com sucesso (soft delete)", id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover parada de UG {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _repository.ExistsAsync(id);
    }
}
