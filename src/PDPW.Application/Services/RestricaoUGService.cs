using PDPW.Application.DTOs.RestricaoUG;
using PDPW.Application.Interfaces;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

/// <summary>
/// Serviço de Restrição de Unidade Geradora
/// </summary>
public class RestricaoUGService : IRestricaoUGService
{
    private readonly IRestricaoUGRepository _repository;

    public RestricaoUGService(IRestricaoUGRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RestricaoUGDto>> GetAllAsync()
    {
        var restricoes = await _repository.GetAllAsync();
        return restricoes.Select(MapToDto);
    }

    public async Task<RestricaoUGDto?> GetByIdAsync(int id)
    {
        var restricao = await _repository.GetByIdAsync(id);
        return restricao != null ? MapToDto(restricao) : null;
    }

    public async Task<RestricaoUGDto> CreateAsync(CreateRestricaoUGDto dto)
    {
        // Validar datas
        if (dto.DataFim.HasValue && dto.DataFim < dto.DataInicio)
            throw new InvalidOperationException("Data fim não pode ser menor que data início");

        var restricao = new RestricaoUG
        {
            UnidadeGeradoraId = dto.UnidadeGeradoraId,
            DataInicio = dto.DataInicio,
            DataFim = dto.DataFim,
            MotivoRestricaoId = dto.MotivoRestricaoId,
            PotenciaRestrita = dto.PotenciaRestrita,
            Observacoes = dto.Observacoes,
            DataCriacao = DateTime.UtcNow,
            Ativo = true
        };

        var created = await _repository.AddAsync(restricao);
        return MapToDto(created);
    }

    public async Task<RestricaoUGDto> UpdateAsync(int id, UpdateRestricaoUGDto dto)
    {
        var restricao = await _repository.GetByIdAsync(id);
        if (restricao == null)
            throw new KeyNotFoundException($"Restrição UG com ID {id} não encontrada");

        // Validar datas
        if (dto.DataFim.HasValue && dto.DataFim < dto.DataInicio)
            throw new InvalidOperationException("Data fim não pode ser menor que data início");

        restricao.UnidadeGeradoraId = dto.UnidadeGeradoraId;
        restricao.DataInicio = dto.DataInicio;
        restricao.DataFim = dto.DataFim;
        restricao.MotivoRestricaoId = dto.MotivoRestricaoId;
        restricao.PotenciaRestrita = dto.PotenciaRestrita;
        restricao.Observacoes = dto.Observacoes;
        restricao.DataAtualizacao = DateTime.UtcNow;

        await _repository.UpdateAsync(restricao);
        return MapToDto(restricao);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var restricao = await _repository.GetByIdAsync(id);
        if (restricao == null)
            return false;

        await _repository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<RestricaoUGDto>> GetByUnidadeGeradoraAsync(int unidadeGeradoraId)
    {
        var restricoes = await _repository.GetByUnidadeGeradoraAsync(unidadeGeradoraId);
        return restricoes.Select(MapToDto);
    }

    public async Task<IEnumerable<RestricaoUGDto>> GetAtivasAsync(DateTime dataReferencia)
    {
        var restricoes = await _repository.GetAtivasAsync(dataReferencia);
        return restricoes.Select(MapToDto);
    }

    public async Task<IEnumerable<RestricaoUGDto>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        var restricoes = await _repository.GetByPeriodoAsync(dataInicio, dataFim);
        return restricoes.Select(MapToDto);
    }

    public async Task<IEnumerable<RestricaoUGDto>> GetByMotivoRestricaoAsync(int motivoRestricaoId)
    {
        var restricoes = await _repository.GetByMotivoRestricaoAsync(motivoRestricaoId);
        return restricoes.Select(MapToDto);
    }

    private static RestricaoUGDto MapToDto(RestricaoUG restricao)
    {
        return new RestricaoUGDto
        {
            Id = restricao.Id,
            UnidadeGeradoraId = restricao.UnidadeGeradoraId,
            UnidadeGeradora = restricao.UnidadeGeradora?.Nome ?? string.Empty,
            CodigoUnidade = restricao.UnidadeGeradora?.Codigo ?? string.Empty,
            DataInicio = restricao.DataInicio,
            DataFim = restricao.DataFim,
            MotivoRestricaoId = restricao.MotivoRestricaoId,
            MotivoRestricao = restricao.MotivoRestricao?.Nome ?? string.Empty,
            CategoriaMotivoRestricao = restricao.MotivoRestricao?.Categoria ?? string.Empty,
            PotenciaRestrita = restricao.PotenciaRestrita,
            Observacoes = restricao.Observacoes,
            Ativo = restricao.Ativo,
            DataCriacao = restricao.DataCriacao
        };
    }
}
