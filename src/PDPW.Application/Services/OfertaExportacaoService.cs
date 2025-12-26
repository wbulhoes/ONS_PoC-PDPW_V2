using AutoMapper;
using PDPW.Application.DTOs.OfertaExportacao;
using PDPW.Application.Interfaces;
using PDPW.Domain.Common;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

/// <summary>
/// Serviço de Ofertas de Exportação de Energia Térmica
/// </summary>
public class OfertaExportacaoService : IOfertaExportacaoService
{
    private readonly IOfertaExportacaoRepository _repository;
    private readonly IUsinaRepository _usinaRepository;
    private readonly IMapper _mapper;

    public OfertaExportacaoService(
        IOfertaExportacaoRepository repository,
        IUsinaRepository usinaRepository,
        IMapper mapper)
    {
        _repository = repository;
        _usinaRepository = usinaRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtém todas as ofertas de exportação
    /// </summary>
    public async Task<Result<IEnumerable<OfertaExportacaoDto>>> GetAllAsync()
    {
        var ofertas = await _repository.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<OfertaExportacaoDto>>(ofertas);
        return Result<IEnumerable<OfertaExportacaoDto>>.Success(dtos);
    }

    /// <summary>
    /// Obtém oferta de exportação por ID
    /// </summary>
    public async Task<Result<OfertaExportacaoDto>> GetByIdAsync(int id)
    {
        var oferta = await _repository.GetByIdAsync(id);
        
        if (oferta == null)
        {
            return Result<OfertaExportacaoDto>.NotFound("Oferta de Exportação", id);
        }

        var dto = _mapper.Map<OfertaExportacaoDto>(oferta);
        return Result<OfertaExportacaoDto>.Success(dto);
    }

    /// <summary>
    /// Obtém ofertas de exportação pendentes de análise do ONS
    /// </summary>
    public async Task<Result<IEnumerable<OfertaExportacaoDto>>> GetPendentesAsync()
    {
        var ofertas = await _repository.GetPendentesAnaliseONSAsync();
        var dtos = _mapper.Map<IEnumerable<OfertaExportacaoDto>>(ofertas);
        return Result<IEnumerable<OfertaExportacaoDto>>.Success(dtos);
    }

    /// <summary>
    /// Obtém ofertas de exportação por usina
    /// </summary>
    public async Task<Result<IEnumerable<OfertaExportacaoDto>>> GetByUsinaAsync(int usinaId)
    {
        // Validar se usina existe
        if (!await _usinaRepository.ExistsAsync(usinaId))
        {
            return Result<IEnumerable<OfertaExportacaoDto>>.Failure($"Usina com ID {usinaId} não encontrada");
        }

        var ofertas = await _repository.GetByUsinaAsync(usinaId);
        var dtos = _mapper.Map<IEnumerable<OfertaExportacaoDto>>(ofertas);
        return Result<IEnumerable<OfertaExportacaoDto>>.Success(dtos);
    }

    /// <summary>
    /// Obtém ofertas de exportação por data PDP
    /// </summary>
    public async Task<Result<IEnumerable<OfertaExportacaoDto>>> GetByDataPDPAsync(DateTime dataPDP)
    {
        var ofertas = await _repository.GetByDataPDPAsync(dataPDP);
        var dtos = _mapper.Map<IEnumerable<OfertaExportacaoDto>>(ofertas);
        return Result<IEnumerable<OfertaExportacaoDto>>.Success(dtos);
    }

    /// <summary>
    /// Obtém ofertas de exportação por período
    /// </summary>
    public async Task<Result<IEnumerable<OfertaExportacaoDto>>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        // Validar período
        if (dataInicio > dataFim)
        {
            return Result<IEnumerable<OfertaExportacaoDto>>.Failure("Data inicial não pode ser maior que data final");
        }

        var ofertas = await _repository.GetByPeriodoAsync(dataInicio, dataFim);
        var dtos = _mapper.Map<IEnumerable<OfertaExportacaoDto>>(ofertas);
        return Result<IEnumerable<OfertaExportacaoDto>>.Success(dtos);
    }

    /// <summary>
    /// Obtém ofertas de exportação aprovadas
    /// </summary>
    public async Task<Result<IEnumerable<OfertaExportacaoDto>>> GetAprovadasAsync()
    {
        var ofertas = await _repository.GetAprovadasAsync();
        var dtos = _mapper.Map<IEnumerable<OfertaExportacaoDto>>(ofertas);
        return Result<IEnumerable<OfertaExportacaoDto>>.Success(dtos);
    }

    /// <summary>
    /// Obtém ofertas de exportação rejeitadas
    /// </summary>
    public async Task<Result<IEnumerable<OfertaExportacaoDto>>> GetRejeitadasAsync()
    {
        var ofertas = await _repository.GetRejeitadasAsync();
        var dtos = _mapper.Map<IEnumerable<OfertaExportacaoDto>>(ofertas);
        return Result<IEnumerable<OfertaExportacaoDto>>.Success(dtos);
    }

    /// <summary>
    /// Cria nova oferta de exportação
    /// </summary>
    public async Task<Result<OfertaExportacaoDto>> CreateAsync(CreateOfertaExportacaoDto createDto)
    {
        // Validar usina existe
        if (!await _usinaRepository.ExistsAsync(createDto.UsinaId))
        {
            return Result<OfertaExportacaoDto>.Failure($"Usina com ID {createDto.UsinaId} não encontrada");
        }

        // Validar hora final maior que hora inicial
        if (createDto.HoraFinal <= createDto.HoraInicial)
        {
            return Result<OfertaExportacaoDto>.Failure("Hora final deve ser maior que hora inicial");
        }

        // Validar data PDP não pode ser no passado
        if (createDto.DataPDP.Date < DateTime.Now.Date)
        {
            return Result<OfertaExportacaoDto>.Failure("Data do PDP não pode ser no passado");
        }

        var oferta = _mapper.Map<OfertaExportacao>(createDto);
        var created = await _repository.AddAsync(oferta);

        // Buscar novamente para incluir relacionamentos
        var result = await _repository.GetByIdAsync(created.Id);
        var dto = _mapper.Map<OfertaExportacaoDto>(result);
        
        return Result<OfertaExportacaoDto>.Success(dto);
    }

    /// <summary>
    /// Atualiza oferta de exportação existente
    /// </summary>
    public async Task<Result<OfertaExportacaoDto>> UpdateAsync(int id, UpdateOfertaExportacaoDto updateDto)
    {
        var oferta = await _repository.GetByIdAsync(id);
        if (oferta == null)
        {
            return Result<OfertaExportacaoDto>.NotFound("Oferta de Exportação", id);
        }

        // Validar se oferta já foi analisada
        if (oferta.FlgAprovadoONS.HasValue)
        {
            return Result<OfertaExportacaoDto>.Failure("Não é possível atualizar oferta que já foi analisada pelo ONS");
        }

        // Validar usina existe
        if (!await _usinaRepository.ExistsAsync(updateDto.UsinaId))
        {
            return Result<OfertaExportacaoDto>.Failure($"Usina com ID {updateDto.UsinaId} não encontrada");
        }

        // Validar hora final maior que hora inicial
        if (updateDto.HoraFinal <= updateDto.HoraInicial)
        {
            return Result<OfertaExportacaoDto>.Failure("Hora final deve ser maior que hora inicial");
        }

        // Validar data PDP não pode ser no passado
        if (updateDto.DataPDP.Date < DateTime.Now.Date)
        {
            return Result<OfertaExportacaoDto>.Failure("Data do PDP não pode ser no passado");
        }

        // Mapear dados atualizados
        _mapper.Map(updateDto, oferta);

        await _repository.UpdateAsync(oferta);

        // Buscar novamente para incluir relacionamentos
        var result = await _repository.GetByIdAsync(id);
        var dto = _mapper.Map<OfertaExportacaoDto>(result);
        
        return Result<OfertaExportacaoDto>.Success(dto);
    }

    /// <summary>
    /// Remove oferta de exportação
    /// </summary>
    public async Task<Result> DeleteAsync(int id)
    {
        var oferta = await _repository.GetByIdAsync(id);
        if (oferta == null)
        {
            return Result.Failure($"Oferta de Exportação com ID {id} não foi encontrada");
        }

        // Validar se permite exclusão (data PDP >= D+1)
        if (!await _repository.PermiteExclusaoAsync(oferta.DataPDP))
        {
            return Result.Failure("Não é possível excluir oferta com data PDP menor que D+1 (amanhã)");
        }

        // Validar se oferta já foi analisada
        if (oferta.FlgAprovadoONS.HasValue)
        {
            return Result.Failure("Não é possível excluir oferta que já foi analisada pelo ONS");
        }

        await _repository.DeleteAsync(id);
        return Result.Success();
    }

    /// <summary>
    /// Aprova oferta de exportação
    /// </summary>
    public async Task<Result> AprovarAsync(int id, AprovarOfertaExportacaoDto aprovarDto)
    {
        var oferta = await _repository.GetByIdAsync(id);
        if (oferta == null)
        {
            return Result.Failure($"Oferta de Exportação com ID {id} não foi encontrada");
        }

        // Validar se já foi analisada
        if (oferta.FlgAprovadoONS.HasValue)
        {
            var status = oferta.FlgAprovadoONS.Value ? "aprovada" : "rejeitada";
            return Result.Failure($"Oferta já foi {status} pelo ONS em {oferta.DataAnaliseONS:dd/MM/yyyy HH:mm}");
        }

        await _repository.AprovarAsync(id, aprovarDto.UsuarioONS, aprovarDto.Observacao);
        return Result.Success();
    }

    /// <summary>
    /// Rejeita oferta de exportação
    /// </summary>
    public async Task<Result> RejeitarAsync(int id, RejeitarOfertaExportacaoDto rejeitarDto)
    {
        var oferta = await _repository.GetByIdAsync(id);
        if (oferta == null)
        {
            return Result.Failure($"Oferta de Exportação com ID {id} não foi encontrada");
        }

        // Validar se já foi analisada
        if (oferta.FlgAprovadoONS.HasValue)
        {
            var status = oferta.FlgAprovadoONS.Value ? "aprovada" : "rejeitada";
            return Result.Failure($"Oferta já foi {status} pelo ONS em {oferta.DataAnaliseONS:dd/MM/yyyy HH:mm}");
        }

        await _repository.RejeitarAsync(id, rejeitarDto.UsuarioONS, rejeitarDto.Observacao);
        return Result.Success();
    }

    /// <summary>
    /// Verifica se existe oferta pendente para uma data PDP
    /// </summary>
    public async Task<bool> ExisteOfertaPendenteAsync(DateTime dataPDP)
    {
        return await _repository.ExisteOfertaPendenteAnaliseONSAsync(dataPDP);
    }

    /// <summary>
    /// Verifica se permite exclusão de ofertas
    /// </summary>
    public async Task<bool> PermiteExclusaoAsync(DateTime dataPDP)
    {
        return await _repository.PermiteExclusaoAsync(dataPDP);
    }
}
