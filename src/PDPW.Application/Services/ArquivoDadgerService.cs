using PDPW.Application.DTOs.ArquivoDadger;
using PDPW.Application.Interfaces;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

/// <summary>
/// Serviço de Arquivo DADGER
/// </summary>
public class ArquivoDadgerService : IArquivoDadgerService
{
    private readonly IArquivoDadgerRepository _repository;
    private readonly ISemanaPMORepository _semanaPMORepository;

    public ArquivoDadgerService(
        IArquivoDadgerRepository repository,
        ISemanaPMORepository semanaPMORepository)
    {
        _repository = repository;
        _semanaPMORepository = semanaPMORepository;
    }

    public async Task<IEnumerable<ArquivoDadgerDto>> GetAllAsync()
    {
        var arquivos = await _repository.GetAllAsync();
        return arquivos.Select(MapToDto);
    }

    public async Task<ArquivoDadgerDto?> GetByIdAsync(int id)
    {
        var arquivo = await _repository.GetByIdAsync(id);
        return arquivo != null ? MapToDto(arquivo) : null;
    }

    public async Task<ArquivoDadgerDto> CreateAsync(CreateArquivoDadgerDto dto)
    {
        var arquivo = new ArquivoDadger
        {
            NomeArquivo = dto.NomeArquivo,
            CaminhoArquivo = dto.CaminhoArquivo,
            DataImportacao = dto.DataImportacao,
            SemanaPMOId = dto.SemanaPMOId,
            Observacoes = dto.Observacoes,
            Processado = false,
            DataCriacao = DateTime.UtcNow,
            Ativo = true
        };

        var created = await _repository.AddAsync(arquivo);
        return MapToDto(created);
    }

    public async Task<ArquivoDadgerDto> UpdateAsync(int id, UpdateArquivoDadgerDto dto)
    {
        var arquivo = await _repository.GetByIdAsync(id);
        if (arquivo == null)
            throw new KeyNotFoundException($"Arquivo DADGER com ID {id} não encontrado");

        arquivo.NomeArquivo = dto.NomeArquivo;
        arquivo.CaminhoArquivo = dto.CaminhoArquivo;
        arquivo.DataImportacao = dto.DataImportacao;
        arquivo.SemanaPMOId = dto.SemanaPMOId;
        arquivo.Observacoes = dto.Observacoes;
        arquivo.Processado = dto.Processado;
        arquivo.DataProcessamento = dto.DataProcessamento;
        arquivo.DataAtualizacao = DateTime.UtcNow;

        await _repository.UpdateAsync(arquivo);
        return MapToDto(arquivo);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var arquivo = await _repository.GetByIdAsync(id);
        if (arquivo == null)
            return false;

        await _repository.DeleteAsync(id);
        return true;
    }

    public async Task<IEnumerable<ArquivoDadgerDto>> GetBySemanaPMOAsync(int semanaPMOId)
    {
        var arquivos = await _repository.GetBySemanaPMOAsync(semanaPMOId);
        return arquivos.Select(MapToDto);
    }

    public async Task<IEnumerable<ArquivoDadgerDto>> GetProcessadosAsync(bool processado)
    {
        var arquivos = await _repository.GetProcessadosAsync(processado);
        return arquivos.Select(MapToDto);
    }

    public async Task<IEnumerable<ArquivoDadgerDto>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        var arquivos = await _repository.GetByPeriodoAsync(dataInicio, dataFim);
        return arquivos.Select(MapToDto);
    }

    public async Task<ArquivoDadgerDto?> GetByNomeArquivoAsync(string nomeArquivo)
    {
        var arquivo = await _repository.GetByNomeArquivoAsync(nomeArquivo);
        return arquivo != null ? MapToDto(arquivo) : null;
    }

    public async Task<ArquivoDadgerDto> MarcarComoProcessadoAsync(int id)
    {
        var arquivo = await _repository.GetByIdAsync(id);
        if (arquivo == null)
            throw new KeyNotFoundException($"Arquivo DADGER com ID {id} não encontrado");

        arquivo.Processado = true;
        arquivo.DataProcessamento = DateTime.UtcNow;
        arquivo.DataAtualizacao = DateTime.UtcNow;

        await _repository.UpdateAsync(arquivo);
        return MapToDto(arquivo);
    }

    private static ArquivoDadgerDto MapToDto(ArquivoDadger arquivo)
    {
        return new ArquivoDadgerDto
        {
            Id = arquivo.Id,
            NomeArquivo = arquivo.NomeArquivo,
            CaminhoArquivo = arquivo.CaminhoArquivo,
            DataImportacao = arquivo.DataImportacao,
            SemanaPMOId = arquivo.SemanaPMOId,
            SemanaPMO = arquivo.SemanaPMO != null 
                ? $"Semana {arquivo.SemanaPMO.Numero}/{arquivo.SemanaPMO.Ano}" 
                : string.Empty,
            Observacoes = arquivo.Observacoes,
            Processado = arquivo.Processado,
            DataProcessamento = arquivo.DataProcessamento,
            Ativo = arquivo.Ativo,
            DataCriacao = arquivo.DataCriacao
        };
    }
}
