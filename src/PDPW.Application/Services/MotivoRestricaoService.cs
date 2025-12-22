using AutoMapper;
using Microsoft.Extensions.Logging;
using PDPW.Application.DTOs.MotivoRestricao;
using PDPW.Application.Interfaces;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

/// <summary>
/// Serviço de negócio para Motivos de Restrição
/// </summary>
public class MotivoRestricaoService : IMotivoRestricaoService
{
    private readonly IMotivoRestricaoRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<MotivoRestricaoService> _logger;

    public MotivoRestricaoService(
        IMotivoRestricaoRepository repository,
        IMapper mapper,
        ILogger<MotivoRestricaoService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<MotivoRestricaoDto>> GetAllAsync()
    {
        try
        {
            var motivos = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<MotivoRestricaoDto>>(motivos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar todos os motivos de restrição");
            throw;
        }
    }

    public async Task<MotivoRestricaoDto?> GetByIdAsync(int id)
    {
        try
        {
            var motivo = await _repository.GetByIdAsync(id);
            return motivo == null ? null : _mapper.Map<MotivoRestricaoDto>(motivo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar motivo de restrição com ID {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<MotivoRestricaoDto>> GetByCategoriaAsync(string categoria)
    {
        try
        {
            var motivos = await _repository.GetByCategoriaAsync(categoria);
            return _mapper.Map<IEnumerable<MotivoRestricaoDto>>(motivos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar motivos de restrição da categoria {Categoria}", categoria);
            throw;
        }
    }

    public async Task<IEnumerable<MotivoRestricaoDto>> GetAtivasAsync()
    {
        try
        {
            var motivos = await _repository.GetAtivasAsync();
            return _mapper.Map<IEnumerable<MotivoRestricaoDto>>(motivos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar motivos de restrição ativos");
            throw;
        }
    }

    public async Task<MotivoRestricaoDto> CreateAsync(CreateMotivoRestricaoDto dto)
    {
        try
        {
            var motivo = _mapper.Map<MotivoRestricao>(dto);
            motivo.DataCriacao = DateTime.UtcNow;
            motivo.Ativo = true;

            await _repository.AddAsync(motivo);

            _logger.LogInformation("Motivo de restrição '{Nome}' criado com sucesso", motivo.Nome);

            return _mapper.Map<MotivoRestricaoDto>(motivo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar motivo de restrição");
            throw;
        }
    }

    public async Task<MotivoRestricaoDto> UpdateAsync(int id, UpdateMotivoRestricaoDto dto)
    {
        try
        {
            var motivo = await _repository.GetByIdAsync(id);
            if (motivo == null)
            {
                throw new KeyNotFoundException($"Motivo de restrição com ID {id} não encontrado");
            }

            _mapper.Map(dto, motivo);
            motivo.DataAtualizacao = DateTime.UtcNow;

            await _repository.UpdateAsync(motivo);

            _logger.LogInformation("Motivo de restrição {Id} atualizado com sucesso", id);

            return _mapper.Map<MotivoRestricaoDto>(motivo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar motivo de restrição {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var motivo = await _repository.GetByIdAsync(id);
            if (motivo == null)
            {
                return false;
            }

            // Verificar se há restrições associadas
            var temRestricoesUG = motivo.RestricoesUG?.Any(r => r.Ativo) ?? false;
            var temRestricoesUS = motivo.RestricoesUS?.Any(r => r.Ativo) ?? false;

            if (temRestricoesUG || temRestricoesUS)
            {
                throw new InvalidOperationException(
                    "Não é possível remover este motivo pois existem restrições ativas associadas a ele");
            }

            // Soft delete
            motivo.Ativo = false;
            motivo.DataAtualizacao = DateTime.UtcNow;
            await _repository.UpdateAsync(motivo);

            _logger.LogInformation("Motivo de restrição {Id} removido com sucesso (soft delete)", id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover motivo de restrição {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _repository.ExistsAsync(id);
    }
}
