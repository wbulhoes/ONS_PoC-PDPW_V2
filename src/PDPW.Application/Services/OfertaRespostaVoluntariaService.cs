using AutoMapper;
using PDPW.Application.DTOs.OfertaRespostaVoluntaria;
using PDPW.Application.Interfaces;
using PDPW.Domain.Common;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

public class OfertaRespostaVoluntariaService : IOfertaRespostaVoluntariaService
{
    private readonly IOfertaRespostaVoluntariaRepository _repository;
    private readonly IEmpresaRepository _empresaRepository;
    private readonly IMapper _mapper;

    public OfertaRespostaVoluntariaService(
        IOfertaRespostaVoluntariaRepository repository,
        IEmpresaRepository empresaRepository,
        IMapper mapper)
    {
        _repository = repository;
        _empresaRepository = empresaRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<OfertaRespostaVoluntariaDto>>> GetAllAsync()
    {
        var ofertas = await _repository.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<OfertaRespostaVoluntariaDto>>(ofertas);
        return Result<IEnumerable<OfertaRespostaVoluntariaDto>>.Success(dtos);
    }

    public async Task<Result<OfertaRespostaVoluntariaDto>> GetByIdAsync(int id)
    {
        var oferta = await _repository.GetByIdAsync(id);
        if (oferta == null)
        {
            return Result<OfertaRespostaVoluntariaDto>.NotFound("Oferta de Resposta Voluntária", id);
        }

        var dto = _mapper.Map<OfertaRespostaVoluntariaDto>(oferta);
        return Result<OfertaRespostaVoluntariaDto>.Success(dto);
    }

    public async Task<Result<IEnumerable<OfertaRespostaVoluntariaDto>>> GetByEmpresaAsync(int empresaId)
    {
        var empresa = await _empresaRepository.GetByIdAsync(empresaId);
        if (empresa == null)
        {
            return Result<IEnumerable<OfertaRespostaVoluntariaDto>>.Failure($"Empresa com ID {empresaId} não encontrada");
        }

        var ofertas = await _repository.GetByEmpresaAsync(empresaId);
        var dtos = _mapper.Map<IEnumerable<OfertaRespostaVoluntariaDto>>(ofertas);
        return Result<IEnumerable<OfertaRespostaVoluntariaDto>>.Success(dtos);
    }

    public async Task<Result<IEnumerable<OfertaRespostaVoluntariaDto>>> GetByDataPDPAsync(DateTime dataPDP)
    {
        var ofertas = await _repository.GetByDataPDPAsync(dataPDP);
        var dtos = _mapper.Map<IEnumerable<OfertaRespostaVoluntariaDto>>(ofertas);
        return Result<IEnumerable<OfertaRespostaVoluntariaDto>>.Success(dtos);
    }

    public async Task<Result<IEnumerable<OfertaRespostaVoluntariaDto>>> GetPendentesAsync()
    {
        var ofertas = await _repository.GetPendentesAnaliseONSAsync();
        var dtos = _mapper.Map<IEnumerable<OfertaRespostaVoluntariaDto>>(ofertas);
        return Result<IEnumerable<OfertaRespostaVoluntariaDto>>.Success(dtos);
    }

    public async Task<Result<IEnumerable<OfertaRespostaVoluntariaDto>>> GetByTipoProgramaAsync(string tipoPrograma)
    {
        var ofertas = await _repository.GetByTipoProgramaAsync(tipoPrograma);
        var dtos = _mapper.Map<IEnumerable<OfertaRespostaVoluntariaDto>>(ofertas);
        return Result<IEnumerable<OfertaRespostaVoluntariaDto>>.Success(dtos);
    }

    public async Task<Result<IEnumerable<OfertaRespostaVoluntariaDto>>> GetAprovadasAsync()
    {
        var ofertas = await _repository.GetAprovadasAsync();
        var dtos = _mapper.Map<IEnumerable<OfertaRespostaVoluntariaDto>>(ofertas);
        return Result<IEnumerable<OfertaRespostaVoluntariaDto>>.Success(dtos);
    }

    public async Task<Result<IEnumerable<OfertaRespostaVoluntariaDto>>> GetRejeitadasAsync()
    {
        var ofertas = await _repository.GetRejeitadasAsync();
        var dtos = _mapper.Map<IEnumerable<OfertaRespostaVoluntariaDto>>(ofertas);
        return Result<IEnumerable<OfertaRespostaVoluntariaDto>>.Success(dtos);
    }

    public async Task<Result<OfertaRespostaVoluntariaDto>> CreateAsync(CreateOfertaRespostaVoluntariaDto dto)
    {
        // Validar empresa existe
        var empresa = await _empresaRepository.GetByIdAsync(dto.EmpresaId);
        if (empresa == null)
        {
            return Result<OfertaRespostaVoluntariaDto>.Failure($"Empresa com ID {dto.EmpresaId} não encontrada");
        }

        // Validar hora final maior que hora inicial
        if (dto.HoraFinal <= dto.HoraInicial)
        {
            return Result<OfertaRespostaVoluntariaDto>.Failure("Hora final deve ser maior que hora inicial");
        }

        // Validar data PDP não pode ser no passado
        if (dto.DataPDP.Date < DateTime.Now.Date)
        {
            return Result<OfertaRespostaVoluntariaDto>.Failure("Data do PDP não pode ser no passado");
        }

        var oferta = _mapper.Map<OfertaRespostaVoluntaria>(dto);
        var created = await _repository.AddAsync(oferta);

        var result = await _repository.GetByIdAsync(created.Id);
        var resultDto = _mapper.Map<OfertaRespostaVoluntariaDto>(result);
        
        return Result<OfertaRespostaVoluntariaDto>.Success(resultDto);
    }

    public async Task<Result<OfertaRespostaVoluntariaDto>> UpdateAsync(int id, UpdateOfertaRespostaVoluntariaDto dto)
    {
        var oferta = await _repository.GetByIdAsync(id);
        if (oferta == null)
        {
            return Result<OfertaRespostaVoluntariaDto>.NotFound("Oferta de Resposta Voluntária", id);
        }

        // Validar se oferta já foi analisada
        if (oferta.FlgAprovadoONS.HasValue)
        {
            return Result<OfertaRespostaVoluntariaDto>.Failure("Não é possível atualizar oferta que já foi analisada pelo ONS");
        }

        // Validar empresa existe
        var empresa = await _empresaRepository.GetByIdAsync(dto.EmpresaId);
        if (empresa == null)
        {
            return Result<OfertaRespostaVoluntariaDto>.Failure($"Empresa com ID {dto.EmpresaId} não encontrada");
        }

        // Validar hora final maior que hora inicial
        if (dto.HoraFinal <= dto.HoraInicial)
        {
            return Result<OfertaRespostaVoluntariaDto>.Failure("Hora final deve ser maior que hora inicial");
        }

        // Validar data PDP não pode ser no passado
        if (dto.DataPDP.Date < DateTime.Now.Date)
        {
            return Result<OfertaRespostaVoluntariaDto>.Failure("Data do PDP não pode ser no passado");
        }

        _mapper.Map(dto, oferta);
        await _repository.UpdateAsync(oferta);

        var result = await _repository.GetByIdAsync(id);
        var resultDto = _mapper.Map<OfertaRespostaVoluntariaDto>(result);
        
        return Result<OfertaRespostaVoluntariaDto>.Success(resultDto);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var oferta = await _repository.GetByIdAsync(id);
        if (oferta == null)
        {
            return Result.Failure($"Oferta de Resposta Voluntária com ID {id} não foi encontrada");
        }

        // Validar se oferta já foi analisada
        if (oferta.FlgAprovadoONS.HasValue)
        {
            return Result.Failure("Não é possível excluir oferta que já foi analisada pelo ONS");
        }

        await _repository.DeleteAsync(id);
        return Result.Success();
    }

    public async Task<Result> AprovarAsync(int id, AprovarOfertaRespostaVoluntariaDto dto)
    {
        var oferta = await _repository.GetByIdAsync(id);
        if (oferta == null)
        {
            return Result.Failure($"Oferta de Resposta Voluntária com ID {id} não foi encontrada");
        }

        // Validar se já foi analisada
        if (oferta.FlgAprovadoONS.HasValue)
        {
            var status = oferta.FlgAprovadoONS.Value ? "aprovada" : "rejeitada";
            return Result.Failure($"Oferta já foi {status} pelo ONS em {oferta.DataAnaliseONS:dd/MM/yyyy HH:mm}");
        }

        await _repository.AprovarAsync(id, dto.UsuarioONS, dto.Observacao);
        return Result.Success();
    }

    public async Task<Result> RejeitarAsync(int id, RejeitarOfertaRespostaVoluntariaDto dto)
    {
        var oferta = await _repository.GetByIdAsync(id);
        if (oferta == null)
        {
            return Result.Failure($"Oferta de Resposta Voluntária com ID {id} não foi encontrada");
        }

        // Validar se já foi analisada
        if (oferta.FlgAprovadoONS.HasValue)
        {
            var status = oferta.FlgAprovadoONS.Value ? "aprovada" : "rejeitada";
            return Result.Failure($"Oferta já foi {status} pelo ONS em {oferta.DataAnaliseONS:dd/MM/yyyy HH:mm}");
        }

        await _repository.RejeitarAsync(id, dto.UsuarioONS, dto.Observacao);
        return Result.Success();
    }
}
