using AutoMapper;
using Microsoft.Extensions.Logging;
using PDPW.Application.DTOs.Balanco;
using PDPW.Application.Interfaces;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

/// <summary>
/// Serviço de negócio para Balanços Energéticos
/// </summary>
public class BalancoService : IBalancoService
{
    private readonly IBalancoRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<BalancoService> _logger;

    public BalancoService(
        IBalancoRepository repository,
        IMapper mapper,
        ILogger<BalancoService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<BalancoDto>> GetAllAsync()
    {
        try
        {
            var balancos = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<BalancoDto>>(balancos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar todos os balanços energéticos");
            throw;
        }
    }

    public async Task<BalancoDto?> GetByIdAsync(int id)
    {
        try
        {
            var balanco = await _repository.GetByIdAsync(id);
            return balanco == null ? null : _mapper.Map<BalancoDto>(balanco);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar balanço energético com ID {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<BalancoDto>> GetBySubsistemaAsync(string subsistemaId)
    {
        try
        {
            var balancos = await _repository.GetBySubsistemaAsync(subsistemaId);
            return _mapper.Map<IEnumerable<BalancoDto>>(balancos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar balanços do subsistema {Subsistema}", subsistemaId);
            throw;
        }
    }

    public async Task<IEnumerable<BalancoDto>> GetByDataAsync(DateTime dataReferencia)
    {
        try
        {
            var balancos = await _repository.GetByDataAsync(dataReferencia);
            return _mapper.Map<IEnumerable<BalancoDto>>(balancos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar balanços da data {Data}", dataReferencia);
            throw;
        }
    }

    public async Task<IEnumerable<BalancoDto>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        try
        {
            var balancos = await _repository.GetByPeriodoAsync(dataInicio, dataFim);
            return _mapper.Map<IEnumerable<BalancoDto>>(balancos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar balanços no período {Inicio} a {Fim}", dataInicio, dataFim);
            throw;
        }
    }

    public async Task<BalancoDto?> GetBySubsistemaDataAsync(string subsistemaId, DateTime dataReferencia)
    {
        try
        {
            var balanco = await _repository.GetBySubsistemaDataAsync(subsistemaId, dataReferencia);
            return balanco == null ? null : _mapper.Map<BalancoDto>(balanco);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar balanço do subsistema {Subsistema} na data {Data}", 
                subsistemaId, dataReferencia);
            throw;
        }
    }

    public async Task<BalancoDto> CreateAsync(CreateBalancoDto dto)
    {
        try
        {
            // Validar se já existe balanço para o subsistema na data
            var existente = await _repository.GetBySubsistemaDataAsync(dto.SubsistemaId, dto.DataReferencia);
            if (existente != null)
            {
                throw new ArgumentException(
                    $"Já existe um balanço para o subsistema {dto.SubsistemaId} na data {dto.DataReferencia:yyyy-MM-dd}");
            }

            var balanco = _mapper.Map<Balanco>(dto);
            balanco.DataCriacao = DateTime.UtcNow;
            balanco.Ativo = true;

            await _repository.AddAsync(balanco);

            _logger.LogInformation("Balanço energético criado para subsistema {Subsistema} em {Data}", 
                balanco.SubsistemaId, balanco.DataReferencia);

            return _mapper.Map<BalancoDto>(balanco);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar balanço energético");
            throw;
        }
    }

    public async Task<BalancoDto> UpdateAsync(int id, UpdateBalancoDto dto)
    {
        try
        {
            var balanco = await _repository.GetByIdAsync(id);
            if (balanco == null)
            {
                throw new KeyNotFoundException($"Balanço energético com ID {id} não encontrado");
            }

            // Validar se já existe outro balanço para o subsistema na data
            var existente = await _repository.GetBySubsistemaDataAsync(dto.SubsistemaId, dto.DataReferencia);
            if (existente != null && existente.Id != id)
            {
                throw new ArgumentException(
                    $"Já existe outro balanço para o subsistema {dto.SubsistemaId} na data {dto.DataReferencia:yyyy-MM-dd}");
            }

            _mapper.Map(dto, balanco);
            balanco.DataAtualizacao = DateTime.UtcNow;

            await _repository.UpdateAsync(balanco);

            _logger.LogInformation("Balanço energético {Id} atualizado com sucesso", id);

            return _mapper.Map<BalancoDto>(balanco);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar balanço energético {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var balanco = await _repository.GetByIdAsync(id);
            if (balanco == null)
            {
                return false;
            }

            // Soft delete
            balanco.Ativo = false;
            balanco.DataAtualizacao = DateTime.UtcNow;
            await _repository.UpdateAsync(balanco);

            _logger.LogInformation("Balanço energético {Id} removido com sucesso (soft delete)", id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover balanço energético {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _repository.ExistsAsync(id);
    }
}
