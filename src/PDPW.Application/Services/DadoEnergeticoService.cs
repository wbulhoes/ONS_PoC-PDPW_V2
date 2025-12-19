using PDPW.Application.DTOs;
using PDPW.Application.Interfaces;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

/// <summary>
/// Serviço de aplicação para gerenciar dados energéticos
/// </summary>
public class DadoEnergeticoService : IDadoEnergeticoService
{
    private readonly IDadoEnergeticoRepository _repository;

    public DadoEnergeticoService(IDadoEnergeticoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DadoEnergeticoDto>> ObterTodosAsync()
    {
        var dados = await _repository.ObterTodosAsync();
        return dados.Select(MapearParaDto);
    }

    public async Task<DadoEnergeticoDto?> ObterPorIdAsync(int id)
    {
        var dado = await _repository.ObterPorIdAsync(id);
        return dado != null ? MapearParaDto(dado) : null;
    }

    public async Task<DadoEnergeticoDto> CriarAsync(CriarDadoEnergeticoDto dto)
    {
        var dado = new DadoEnergetico
        {
            DataReferencia = dto.DataReferencia,
            CodigoUsina = dto.CodigoUsina,
            ProducaoMWh = dto.ProducaoMWh,
            CapacidadeDisponivel = dto.CapacidadeDisponivel,
            Status = dto.Status,
            Observacoes = dto.Observacoes,
            DataCriacao = DateTime.UtcNow
        };

        var dadoCriado = await _repository.AdicionarAsync(dado);
        return MapearParaDto(dadoCriado);
    }

    public async Task AtualizarAsync(int id, AtualizarDadoEnergeticoDto dto)
    {
        var dado = await _repository.ObterPorIdAsync(id);
        if (dado == null)
            throw new KeyNotFoundException($"Dado energético com ID {id} não encontrado.");

        dado.DataReferencia = dto.DataReferencia;
        dado.CodigoUsina = dto.CodigoUsina;
        dado.ProducaoMWh = dto.ProducaoMWh;
        dado.CapacidadeDisponivel = dto.CapacidadeDisponivel;
        dado.Status = dto.Status;
        dado.Observacoes = dto.Observacoes;
        dado.DataAtualizacao = DateTime.UtcNow;

        await _repository.AtualizarAsync(dado);
    }

    public async Task RemoverAsync(int id)
    {
        await _repository.RemoverAsync(id);
    }

    public async Task<IEnumerable<DadoEnergeticoDto>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        var dados = await _repository.ObterPorPeriodoAsync(dataInicio, dataFim);
        return dados.Select(MapearParaDto);
    }

    private static DadoEnergeticoDto MapearParaDto(DadoEnergetico dado)
    {
        return new DadoEnergeticoDto
        {
            Id = dado.Id,
            DataReferencia = dado.DataReferencia,
            CodigoUsina = dado.CodigoUsina ?? string.Empty,
            ProducaoMWh = dado.ProducaoMWh,
            CapacidadeDisponivel = dado.CapacidadeDisponivel,
            Status = dado.Status ?? string.Empty,
            Observacoes = dado.Observacoes
        };
    }
}
