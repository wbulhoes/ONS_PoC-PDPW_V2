using AutoMapper;
using Microsoft.Extensions.Logging;
using PDPW.Application.DTOs.Intercambio;
using PDPW.Application.Interfaces;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

/// <summary>
/// Serviço de negócio para Intercâmbios de Energia
/// </summary>
public class IntercambioService : IIntercambioService
{
    private readonly IIntercambioRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<IntercambioService> _logger;

    public IntercambioService(
        IIntercambioRepository repository,
        IMapper mapper,
        ILogger<IntercambioService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<IntercambioDto>> GetAllAsync()
    {
        try
        {
            var intercambios = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<IntercambioDto>>(intercambios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar todos os intercâmbios");
            throw;
        }
    }

    public async Task<IntercambioDto?> GetByIdAsync(int id)
    {
        try
        {
            var intercambio = await _repository.GetByIdAsync(id);
            return intercambio == null ? null : _mapper.Map<IntercambioDto>(intercambio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar intercâmbio com ID {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<IntercambioDto>> GetBySubsistemaOrigemAsync(string subsistemaOrigem)
    {
        try
        {
            var intercambios = await _repository.GetBySubsistemaOrigemAsync(subsistemaOrigem);
            return _mapper.Map<IEnumerable<IntercambioDto>>(intercambios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar intercâmbios de origem {Subsistema}", subsistemaOrigem);
            throw;
        }
    }

    public async Task<IEnumerable<IntercambioDto>> GetBySubsistemaDestinoAsync(string subsistemaDestino)
    {
        try
        {
            var intercambios = await _repository.GetBySubsistemaDestinoAsync(subsistemaDestino);
            return _mapper.Map<IEnumerable<IntercambioDto>>(intercambios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar intercâmbios para destino {Subsistema}", subsistemaDestino);
            throw;
        }
    }

    public async Task<IEnumerable<IntercambioDto>> GetByDataAsync(DateTime dataReferencia)
    {
        try
        {
            // Validação conforme InterDAO.vb
            if (dataReferencia == default)
            {
                throw new ArgumentException("Data de referência não informada");
            }

            var intercambios = await _repository.GetByDataAsync(dataReferencia);
            return _mapper.Map<IEnumerable<IntercambioDto>>(intercambios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar intercâmbios da data {Data}", dataReferencia);
            throw;
        }
    }

    public async Task<IEnumerable<IntercambioDto>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        try
        {
            var intercambios = await _repository.GetByPeriodoAsync(dataInicio, dataFim);
            return _mapper.Map<IEnumerable<IntercambioDto>>(intercambios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar intercâmbios no período {Inicio} a {Fim}", dataInicio, dataFim);
            throw;
        }
    }

    public async Task<IntercambioDto?> GetBySubsistemasDataAsync(
        string subsistemaOrigem, 
        string subsistemaDestino, 
        DateTime dataReferencia)
    {
        try
        {
            var intercambio = await _repository.GetBySubsistemasDataAsync(
                subsistemaOrigem, subsistemaDestino, dataReferencia);
            return intercambio == null ? null : _mapper.Map<IntercambioDto>(intercambio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar intercâmbio {Origem} -> {Destino} na data {Data}", 
                subsistemaOrigem, subsistemaDestino, dataReferencia);
            throw;
        }
    }

    public async Task<IntercambioDto> CreateAsync(CreateIntercambioDto dto)
    {
        try
        {
            // Validações conforme InterDAO.vb
            if (dto.DataReferencia == default)
            {
                throw new ArgumentException("Data de referência não informada");
            }

            if (string.IsNullOrWhiteSpace(dto.SubsistemaOrigem))
            {
                throw new ArgumentException("Subsistema de origem não informado");
            }

            if (string.IsNullOrWhiteSpace(dto.SubsistemaDestino))
            {
                throw new ArgumentException("Subsistema de destino não informado");
            }

            // Validar se subsistemas são diferentes
            if (dto.SubsistemaOrigem == dto.SubsistemaDestino)
            {
                throw new ArgumentException("O subsistema de origem deve ser diferente do subsistema de destino");
            }

            // Validar se já existe intercâmbio para os subsistemas na data
            var existente = await _repository.GetBySubsistemasDataAsync(
                dto.SubsistemaOrigem, dto.SubsistemaDestino, dto.DataReferencia);
            if (existente != null)
            {
                throw new ArgumentException(
                    $"Já existe um intercâmbio de {dto.SubsistemaOrigem} para {dto.SubsistemaDestino} na data {dto.DataReferencia:yyyy-MM-dd}");
            }

            var intercambio = _mapper.Map<Intercambio>(dto);
            intercambio.DataCriacao = DateTime.UtcNow;
            intercambio.Ativo = true;

            await _repository.AddAsync(intercambio);

            _logger.LogInformation("Intercâmbio criado: {Origem} -> {Destino} em {Data}", 
                intercambio.SubsistemaOrigem, intercambio.SubsistemaDestino, intercambio.DataReferencia);

            return _mapper.Map<IntercambioDto>(intercambio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar intercâmbio");
            throw;
        }
    }

    public async Task<IntercambioDto> UpdateAsync(int id, UpdateIntercambioDto dto)
    {
        try
        {
            // Validações conforme InterDAO.vb
            if (dto.DataReferencia == default)
            {
                throw new ArgumentException("Data de referência não informada");
            }

            if (string.IsNullOrWhiteSpace(dto.SubsistemaOrigem))
            {
                throw new ArgumentException("Subsistema de origem não informado");
            }

            if (string.IsNullOrWhiteSpace(dto.SubsistemaDestino))
            {
                throw new ArgumentException("Subsistema de destino não informado");
            }

            var intercambio = await _repository.GetByIdAsync(id);
            if (intercambio == null)
            {
                throw new KeyNotFoundException($"Intercâmbio com ID {id} não encontrado");
            }

            // Validar se subsistemas são diferentes
            if (dto.SubsistemaOrigem == dto.SubsistemaDestino)
            {
                throw new ArgumentException("O subsistema de origem deve ser diferente do subsistema de destino");
            }

            // Validar se já existe outro intercâmbio para os subsistemas na data
            var existente = await _repository.GetBySubsistemasDataAsync(
                dto.SubsistemaOrigem, dto.SubsistemaDestino, dto.DataReferencia);
            if (existente != null && existente.Id != id)
            {
                throw new ArgumentException(
                    $"Já existe outro intercâmbio de {dto.SubsistemaOrigem} para {dto.SubsistemaDestino} na data {dto.DataReferencia:yyyy-MM-dd}");
            }

            _mapper.Map(dto, intercambio);
            intercambio.DataAtualizacao = DateTime.UtcNow;

            await _repository.UpdateAsync(intercambio);

            _logger.LogInformation("Intercâmbio {Id} atualizado com sucesso", id);

            return _mapper.Map<IntercambioDto>(intercambio);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar intercâmbio {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var intercambio = await _repository.GetByIdAsync(id);
            if (intercambio == null)
            {
                return false;
            }

            // Soft delete
            intercambio.Ativo = false;
            intercambio.DataAtualizacao = DateTime.UtcNow;
            await _repository.UpdateAsync(intercambio);

            _logger.LogInformation("Intercâmbio {Id} removido com sucesso (soft delete)", id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover intercâmbio {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _repository.ExistsAsync(id);
    }
}
