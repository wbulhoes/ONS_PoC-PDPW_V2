using AutoMapper;
using PDPW.Application.DTOs.Usina;
using PDPW.Domain.Entities;

namespace PDPW.Application.Mappings;

/// <summary>
/// Profile base do AutoMapper
/// Profiles específicos devem herdar desta classe
/// </summary>
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Configurações globais de mapeamento
        AllowNullCollections = true;

        // === USINA MAPPINGS ===
        
        // Usina ? UsinaDto
        CreateMap<Usina, UsinaDto>()
            .ForMember(dest => dest.TipoUsina, opt => opt.MapFrom(src => src.TipoUsina!.Nome))
            .ForMember(dest => dest.Empresa, opt => opt.MapFrom(src => src.Empresa!.Nome));

        // CreateUsinaDto ? Usina
        CreateMap<CreateUsinaDto, Usina>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.TipoUsina, opt => opt.Ignore())
            .ForMember(dest => dest.Empresa, opt => opt.Ignore())
            .ForMember(dest => dest.UnidadesGeradoras, opt => opt.Ignore())
            .ForMember(dest => dest.Restricoes, opt => opt.Ignore())
            .ForMember(dest => dest.GeracoesForaMerito, opt => opt.Ignore())
            .ForMember(dest => dest.InflexibilidadesContratadas, opt => opt.Ignore())
            .ForMember(dest => dest.RampasTermicas, opt => opt.Ignore())
            .ForMember(dest => dest.Conversoras, opt => opt.Ignore());

        // UpdateUsinaDto ? Usina (atualização de propriedades existentes)
        CreateMap<UpdateUsinaDto, Usina>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.TipoUsina, opt => opt.Ignore())
            .ForMember(dest => dest.Empresa, opt => opt.Ignore())
            .ForMember(dest => dest.UnidadesGeradoras, opt => opt.Ignore())
            .ForMember(dest => dest.Restricoes, opt => opt.Ignore())
            .ForMember(dest => dest.GeracoesForaMerito, opt => opt.Ignore())
            .ForMember(dest => dest.InflexibilidadesContratadas, opt => opt.Ignore())
            .ForMember(dest => dest.RampasTermicas, opt => opt.Ignore())
            .ForMember(dest => dest.Conversoras, opt => opt.Ignore());
    }
}
