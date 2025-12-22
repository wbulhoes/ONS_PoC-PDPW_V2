using AutoMapper;
using Microsoft.Extensions.Logging;
using PDPW.Application.DTOs.UnidadeGeradora;
using PDPW.Application.Interfaces;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

/// <summary>
/// Serviço de negócio para Unidades Geradoras
/// </summary>
public class UnidadeGeradoraService : IUnidadeGeradoraService
{
    private readonly IUnidadeGeradoraRepository _repository;
    private readonly IUsinaRepository _usinaRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UnidadeGeradoraService> _logger;

    public UnidadeGeradoraService(
        IUnidadeGeradoraRepository repository,
        IUsinaRepository usinaRepository,
        IMapper mapper,
        ILogger<UnidadeGeradoraService> logger)
    {
        _repository = repository;
        _usinaRepository = usinaRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<UnidadeGeradoraDto>> GetAllAsync()
    {
        try
        {
            var unidades = await _repository.GetAllWithUsinaAsync();
            return _mapper.Map<IEnumerable<UnidadeGeradoraDto>>(unidades);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar todas as unidades geradoras");
            throw;
        }
    }

    public async Task<UnidadeGeradoraDto?> GetByIdAsync(int id)
    {
        try
        {
            var unidade = await _repository.GetByIdWithUsinaAsync(id);
            return unidade == null ? null : _mapper.Map<UnidadeGeradoraDto>(unidade);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar unidade geradora com ID {Id}", id);
            throw;
        }
    }

    public async Task<UnidadeGeradoraDto?> GetByCodigoAsync(string codigo)
    {
        try
        {
            var unidade = await _repository.GetByCodigoAsync(codigo);
            return unidade == null ? null : _mapper.Map<UnidadeGeradoraDto>(unidade);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar unidade geradora com código {Codigo}", codigo);
            throw;
        }
    }

    public async Task<IEnumerable<UnidadeGeradoraDto>> GetByUsinaAsync(int usinaId)
    {
        try
        {
            var unidades = await _repository.GetByUsinaIdAsync(usinaId);
            return _mapper.Map<IEnumerable<UnidadeGeradoraDto>>(unidades);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar unidades geradoras da usina {UsinaId}", usinaId);
            throw;
        }
    }

    public async Task<IEnumerable<UnidadeGeradoraDto>> GetByStatusAsync(string status)
    {
        try
        {
            var unidades = await _repository.GetByStatusAsync(status);
            return _mapper.Map<IEnumerable<UnidadeGeradoraDto>>(unidades);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar unidades geradoras com status {Status}", status);
            throw;
        }
    }

    public async Task<IEnumerable<UnidadeGeradoraDto>> GetAtivasAsync()
    {
        try
        {
            var unidades = await _repository.GetAtivasAsync();
            return _mapper.Map<IEnumerable<UnidadeGeradoraDto>>(unidades);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar unidades geradoras ativas");
            throw;
        }
    }

    public async Task<UnidadeGeradoraDto> CreateAsync(CreateUnidadeGeradoraDto dto)
    {
        try
        {
            // Validar se a usina existe
            if (!await _usinaRepository.ExistsAsync(dto.UsinaId))
            {
                throw new ArgumentException($"Usina com ID {dto.UsinaId} não encontrada");
            }

            // Validar se o código já existe
            if (await _repository.CodigoExistsAsync(dto.Codigo))
            {
                throw new ArgumentException($"Já existe uma unidade geradora com o código {dto.Codigo}");
            }

            // Validar potências
            if (dto.PotenciaMinima > dto.PotenciaNominal)
            {
                throw new ArgumentException("A potência mínima não pode ser maior que a potência nominal");
            }

            var unidade = _mapper.Map<UnidadeGeradora>(dto);
            unidade.DataCriacao = DateTime.UtcNow;
            unidade.Ativo = true;

            await _repository.AddAsync(unidade);

            _logger.LogInformation("Unidade geradora {Codigo} criada com sucesso", unidade.Codigo);

            return _mapper.Map<UnidadeGeradoraDto>(unidade);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar unidade geradora");
            throw;
        }
    }

    public async Task<UnidadeGeradoraDto> UpdateAsync(int id, UpdateUnidadeGeradoraDto dto)
    {
        try
        {
            var unidade = await _repository.GetByIdAsync(id);
            if (unidade == null)
            {
                throw new KeyNotFoundException($"Unidade geradora com ID {id} não encontrada");
            }

            // Validar se a usina existe
            if (!await _usinaRepository.ExistsAsync(dto.UsinaId))
            {
                throw new ArgumentException($"Usina com ID {dto.UsinaId} não encontrada");
            }

            // Validar se o código já existe (exceto para a própria unidade)
            if (await _repository.CodigoExistsAsync(dto.Codigo, id))
            {
                throw new ArgumentException($"Já existe outra unidade geradora com o código {dto.Codigo}");
            }

            // Validar potências
            if (dto.PotenciaMinima > dto.PotenciaNominal)
            {
                throw new ArgumentException("A potência mínima não pode ser maior que a potência nominal");
            }

            _mapper.Map(dto, unidade);
            unidade.DataAtualizacao = DateTime.UtcNow;

            await _repository.UpdateAsync(unidade);

            _logger.LogInformation("Unidade geradora {Id} atualizada com sucesso", id);

            return _mapper.Map<UnidadeGeradoraDto>(unidade);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar unidade geradora {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var unidade = await _repository.GetByIdAsync(id);
            if (unidade == null)
            {
                return false;
            }

            // Soft delete
            unidade.Ativo = false;
            unidade.DataAtualizacao = DateTime.UtcNow;
            await _repository.UpdateAsync(unidade);

            _logger.LogInformation("Unidade geradora {Id} removida com sucesso (soft delete)", id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover unidade geradora {Id}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _repository.ExistsAsync(id);
    }

    public async Task<bool> CodigoExistsAsync(string codigo, int? excludeId = null)
    {
        return await _repository.CodigoExistsAsync(codigo, excludeId);
    }
}
