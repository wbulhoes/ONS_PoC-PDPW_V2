using AutoMapper;
using PDPW.Application.DTOs.ArquivoDadger;
using PDPW.Application.Interfaces;
using PDPW.Domain.Common;
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
    private readonly IMapper _mapper;

    public ArquivoDadgerService(
        IArquivoDadgerRepository repository,
        ISemanaPMORepository semanaPMORepository,
        IMapper _mapper)
    {
        _repository = repository;
        _semanaPMORepository = semanaPMORepository;
        this._mapper = _mapper;
    }

    public async Task<Result<IEnumerable<ArquivoDadgerDto>>> GetAllAsync()
    {
        var arquivos = await _repository.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<ArquivoDadgerDto>>(arquivos);
        return Result<IEnumerable<ArquivoDadgerDto>>.Success(dtos);
    }

    public async Task<Result<ArquivoDadgerDto>> GetByIdAsync(int id)
    {
        var arquivo = await _repository.GetByIdAsync(id);
        if (arquivo == null)
        {
            return Result<ArquivoDadgerDto>.NotFound("Arquivo DADGER", id);
        }
        
        var dto = _mapper.Map<ArquivoDadgerDto>(arquivo);
        return Result<ArquivoDadgerDto>.Success(dto);
    }

    public async Task<Result<ArquivoDadgerDto>> CreateAsync(CreateArquivoDadgerDto dto)
    {
        // Validações conforme ArquivoDadgerValorDAO.vb
        if (string.IsNullOrWhiteSpace(dto.NomeArquivo))
        {
            return Result<ArquivoDadgerDto>.Failure("Nome do arquivo não informado");
        }

        if (dto.SemanaPMOId <= 0)
        {
            return Result<ArquivoDadgerDto>.Failure("Semana PMO não informada");
        }

        // Validar se semana PMO existe
        var semanaPMO = await _semanaPMORepository.ObterPorIdAsync(dto.SemanaPMOId);
        if (semanaPMO == null)
        {
            return Result<ArquivoDadgerDto>.Failure($"Semana PMO com ID {dto.SemanaPMOId} não encontrada");
        }

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
        return Result<ArquivoDadgerDto>.Success(_mapper.Map<ArquivoDadgerDto>(created));
    }

    public async Task<Result<ArquivoDadgerDto>> UpdateAsync(int id, UpdateArquivoDadgerDto dto)
    {
        // Validações conforme ArquivoDadgerValorDAO.vb
        if (string.IsNullOrWhiteSpace(dto.NomeArquivo))
        {
            return Result<ArquivoDadgerDto>.Failure("Nome do arquivo não informado");
        }

        if (dto.SemanaPMOId <= 0)
        {
            return Result<ArquivoDadgerDto>.Failure("Semana PMO não informada");
        }

        var arquivo = await _repository.GetByIdAsync(id);
        if (arquivo == null)
            return Result<ArquivoDadgerDto>.NotFound("Arquivo DADGER", id);

        // Validar se semana PMO existe
        var semanaPMO = await _semanaPMORepository.ObterPorIdAsync(dto.SemanaPMOId);
        if (semanaPMO == null)
        {
            return Result<ArquivoDadgerDto>.Failure($"Semana PMO com ID {dto.SemanaPMOId} não encontrada");
        }

        arquivo.NomeArquivo = dto.NomeArquivo;
        arquivo.CaminhoArquivo = dto.CaminhoArquivo;
        arquivo.DataImportacao = dto.DataImportacao;
        arquivo.SemanaPMOId = dto.SemanaPMOId;
        arquivo.Observacoes = dto.Observacoes;
        arquivo.Processado = dto.Processado;
        arquivo.DataProcessamento = dto.DataProcessamento;
        arquivo.DataAtualizacao = DateTime.UtcNow;

        await _repository.UpdateAsync(arquivo);
        return Result<ArquivoDadgerDto>.Success(_mapper.Map<ArquivoDadgerDto>(arquivo));
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var arquivo = await _repository.GetByIdAsync(id);
        if (arquivo == null)
            return Result.Failure($"Arquivo DADGER com ID {id} não encontrado");

        await _repository.DeleteAsync(id);
        return Result.Success();
    }

    public async Task<Result<IEnumerable<ArquivoDadgerDto>>> GetBySemanaPMOAsync(int semanaPMOId)
    {
        if (semanaPMOId <= 0)
        {
            return Result<IEnumerable<ArquivoDadgerDto>>.Failure("Semana PMO não informada");
        }

        var arquivos = await _repository.GetBySemanaPMOAsync(semanaPMOId);
        var dtos = _mapper.Map<IEnumerable<ArquivoDadgerDto>>(arquivos);
        return Result<IEnumerable<ArquivoDadgerDto>>.Success(dtos);
    }

    public async Task<Result<IEnumerable<ArquivoDadgerDto>>> GetProcessadosAsync(bool processado)
    {
        var arquivos = await _repository.GetProcessadosAsync(processado);
        var dtos = _mapper.Map<IEnumerable<ArquivoDadgerDto>>(arquivos);
        return Result<IEnumerable<ArquivoDadgerDto>>.Success(dtos);
    }

    public async Task<Result<IEnumerable<ArquivoDadgerDto>>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        if (dataInicio > dataFim)
        {
            return Result<IEnumerable<ArquivoDadgerDto>>.Failure("Data inicial não pode ser maior que data final");
        }

        var arquivos = await _repository.GetByPeriodoAsync(dataInicio, dataFim);
        var dtos = _mapper.Map<IEnumerable<ArquivoDadgerDto>>(arquivos);
        return Result<IEnumerable<ArquivoDadgerDto>>.Success(dtos);
    }

    public async Task<Result<ArquivoDadgerDto>> GetByNomeArquivoAsync(string nomeArquivo)
    {
        var arquivo = await _repository.GetByNomeArquivoAsync(nomeArquivo);
        if (arquivo == null)
        {
            return Result<ArquivoDadgerDto>.Failure($"Arquivo '{nomeArquivo}' não encontrado");
        }
        
        var dto = _mapper.Map<ArquivoDadgerDto>(arquivo);
        return Result<ArquivoDadgerDto>.Success(dto);
    }

    public async Task<Result<ArquivoDadgerDto>> MarcarComoProcessadoAsync(int id)
    {
        var arquivo = await _repository.GetByIdAsync(id);
        if (arquivo == null)
        {
            return Result<ArquivoDadgerDto>.NotFound("Arquivo DADGER", id);
        }

        arquivo.Processado = true;
        arquivo.DataProcessamento = DateTime.Now;
        arquivo.DataAtualizacao = DateTime.Now;

        await _repository.UpdateAsync(arquivo);
        
        var dto = _mapper.Map<ArquivoDadgerDto>(arquivo);
        return Result<ArquivoDadgerDto>.Success(dto);
    }

    public async Task<Result<IEnumerable<ArquivoDadgerDto>>> GetByStatusAsync(string status)
    {
        var arquivos = await _repository.GetByStatusAsync(status);
        var dtos = _mapper.Map<IEnumerable<ArquivoDadgerDto>>(arquivos);
        return Result<IEnumerable<ArquivoDadgerDto>>.Success(dtos);
    }

    public async Task<Result<IEnumerable<ArquivoDadgerDto>>> GetPendentesAprovacaoAsync()
    {
        var arquivos = await _repository.GetPendentesAprovacaoAsync();
        var dtos = _mapper.Map<IEnumerable<ArquivoDadgerDto>>(arquivos);
        return Result<IEnumerable<ArquivoDadgerDto>>.Success(dtos);
    }

    public async Task<Result> FinalizarAsync(int id, FinalizarProgramacaoDto dto)
    {
        var arquivo = await _repository.GetByIdAsync(id);
        if (arquivo == null)
        {
            return Result.Failure($"Arquivo DADGER com ID {id} não encontrado");
        }

        // Validar status atual
        if (arquivo.Status != "Aberto")
        {
            return Result.Failure($"Apenas programações com status 'Aberto' podem ser finalizadas. Status atual: {arquivo.Status}");
        }

        await _repository.FinalizarAsync(id, dto.Usuario, dto.Observacao);
        return Result.Success();
    }

    public async Task<Result> AprovarAsync(int id, AprovarProgramacaoDto dto)
    {
        var arquivo = await _repository.GetByIdAsync(id);
        if (arquivo == null)
        {
            return Result.Failure($"Arquivo DADGER com ID {id} não encontrado");
        }

        // Validar status atual
        if (arquivo.Status != "EmAnalise")
        {
            return Result.Failure($"Apenas programações com status 'EmAnalise' podem ser aprovadas. Status atual: {arquivo.Status}");
        }

        await _repository.AprovarAsync(id, dto.Usuario, dto.Observacao);
        return Result.Success();
    }

    public async Task<Result> ReabrirAsync(int id, ReabrirProgramacaoDto dto)
    {
        var arquivo = await _repository.GetByIdAsync(id);
        if (arquivo == null)
        {
            return Result.Failure($"Arquivo DADGER com ID {id} não encontrado");
        }

        // Pode reabrir qualquer status, mas não pode reabrir o que já está aberto
        if (arquivo.Status == "Aberto")
        {
            return Result.Failure("Programação já está aberta");
        }

        await _repository.ReabrirAsync(id, dto.Usuario, dto.Observacao);
        return Result.Success();
    }
}
