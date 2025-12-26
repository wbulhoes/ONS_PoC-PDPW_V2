using AutoMapper;
using PDPW.Application.DTOs.Usuario;
using PDPW.Domain.Entities;

namespace PDPW.Application.Mappings;

/// <summary>
/// Profile do AutoMapper para Usuário
/// </summary>
public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<Usuario, UsuarioDto>()
            .ForMember(dest => dest.EquipePDPNome, opt => opt.MapFrom(src => src.EquipePDP != null ? src.EquipePDP.Nome : null));

        CreateMap<CreateUsuarioDto, Usuario>();
        CreateMap<UpdateUsuarioDto, Usuario>();
    }
}
