using AutoMapper;
using PDPW.Application.DTOs.EquipePdp;
using PDPW.Application.Interfaces;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

/// <summary>
/// Serviço de Equipe PDP
/// </summary>
public class EquipePdpService : IEquipePdpService
{
    private readonly IEquipePDPRepository _repository;
    private readonly IMapper _mapper;

    public EquipePdpService(IEquipePDPRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EquipePdpDto>> GetAllAsync()
    {
        var equipes = await _repository.ObterTodasAsync();
        return _mapper.Map<IEnumerable<EquipePdpDto>>(equipes);
    }

    public async Task<EquipePdpDto?> GetByIdAsync(int id)
    {
        var equipe = await _repository.ObterPorIdAsync(id);
        return equipe != null ? _mapper.Map<EquipePdpDto>(equipe) : null;
    }

    public async Task<EquipePdpDto?> GetByNomeAsync(string nome)
    {
        var equipe = await _repository.ObterPorNomeAsync(nome);
        return equipe != null ? _mapper.Map<EquipePdpDto>(equipe) : null;
    }

    public async Task<EquipePdpDto?> GetComMembrosAsync(int id)
    {
        var equipe = await _repository.ObterComMembrosAsync(id);
        return equipe != null ? _mapper.Map<EquipePdpDto>(equipe) : null;
    }

    public async Task<EquipePdpDto> CreateAsync(CreateEquipePdpDto dto)
    {
        // Validar nome único
        if (await _repository.ExisteNomeAsync(dto.Nome))
        {
            throw new InvalidOperationException($"Já existe uma equipe PDP com o nome '{dto.Nome}'");
        }

        var equipe = _mapper.Map<EquipePDP>(dto);
        var equipeCriada = await _repository.AdicionarAsync(equipe);
        return _mapper.Map<EquipePdpDto>(equipeCriada);
    }

    public async Task<EquipePdpDto?> UpdateAsync(int id, UpdateEquipePdpDto dto)
    {
        var equipeExistente = await _repository.ObterPorIdAsync(id);
        if (equipeExistente == null)
        {
            return null;
        }

        // Validar nome único (excluindo o próprio registro)
        if (await _repository.ExisteNomeAsync(dto.Nome, id))
        {
            throw new InvalidOperationException($"Já existe outra equipe PDP com o nome '{dto.Nome}'");
        }

        _mapper.Map(dto, equipeExistente);
        await _repository.AtualizarAsync(equipeExistente);
        return _mapper.Map<EquipePdpDto>(equipeExistente);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var equipe = await _repository.ObterComMembrosAsync(id);
        if (equipe == null)
        {
            return false;
        }

        // Verificar se tem membros ativos vinculados
        if (equipe.Membros?.Any(m => m.Ativo) == true)
        {
            throw new InvalidOperationException(
                $"Não é possível remover a equipe '{equipe.Nome}' pois possui {equipe.Membros.Count(m => m.Ativo)} membro(s) ativo(s) vinculado(s)");
        }

        await _repository.RemoverAsync(id);
        return true;
    }

    public async Task<bool> ExisteNomeAsync(string nome, int? excluirId = null)
    {
        return await _repository.ExisteNomeAsync(nome, excluirId);
    }
}
