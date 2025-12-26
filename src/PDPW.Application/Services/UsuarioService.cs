using AutoMapper;
using PDPW.Application.DTOs.Usuario;
using PDPW.Application.Interfaces;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;

namespace PDPW.Application.Services;

/// <summary>
/// Serviço de Usuários
/// </summary>
public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repository;
    private readonly IMapper _mapper;

    public UsuarioService(IUsuarioRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
    {
        var usuarios = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
    }

    public async Task<UsuarioDto?> GetByIdAsync(int id)
    {
        var usuario = await _repository.GetByIdAsync(id);
        return usuario != null ? _mapper.Map<UsuarioDto>(usuario) : null;
    }

    public async Task<IEnumerable<UsuarioDto>> GetByPerfilAsync(string perfil)
    {
        var usuarios = await _repository.GetByPerfilAsync(perfil);
        return _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
    }

    public async Task<IEnumerable<UsuarioDto>> GetByEquipeAsync(int equipePdpId)
    {
        var usuarios = await _repository.GetByEquipeAsync(equipePdpId);
        return _mapper.Map<IEnumerable<UsuarioDto>>(usuarios);
    }

    public async Task<UsuarioDto?> GetByEmailAsync(string email)
    {
        var usuario = await _repository.GetByEmailAsync(email);
        return usuario != null ? _mapper.Map<UsuarioDto>(usuario) : null;
    }

    public async Task<UsuarioDto> CreateAsync(CreateUsuarioDto dto)
    {
        // Validar se email já existe
        var existente = await _repository.GetByEmailAsync(dto.Email);
        if (existente != null)
        {
            throw new InvalidOperationException($"Já existe um usuário com o email {dto.Email}");
        }

        var usuario = _mapper.Map<Usuario>(dto);
        var created = await _repository.CreateAsync(usuario);
        
        return _mapper.Map<UsuarioDto>(created);
    }

    public async Task<UsuarioDto> UpdateAsync(int id, UpdateUsuarioDto dto)
    {
        var usuario = await _repository.GetByIdAsync(id);
        if (usuario == null)
        {
            throw new KeyNotFoundException($"Usuário com ID {id} não encontrado");
        }

        // Verificar se o email já está sendo usado por outro usuário
        var usuarioComEmail = await _repository.GetByEmailAsync(dto.Email);
        if (usuarioComEmail != null && usuarioComEmail.Id != id)
        {
            throw new InvalidOperationException($"Email {dto.Email} já está sendo usado por outro usuário");
        }

        _mapper.Map(dto, usuario);
        var updated = await _repository.UpdateAsync(usuario);
        
        return _mapper.Map<UsuarioDto>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (!await _repository.ExistsAsync(id))
        {
            throw new KeyNotFoundException($"Usuário com ID {id} não encontrado");
        }

        return await _repository.DeleteAsync(id);
    }
}
