using AutoMapper;
using PDPW.Application.DTOs.Usina;
using PDPW.Application.DTOs.TipoUsina;
using PDPW.Application.DTOs.Empresa;
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

        // === TIPO USINA MAPPINGS ===
        
        // TipoUsina ? TipoUsinaDto
        CreateMap<TipoUsina, TipoUsinaDto>()
            .ForMember(dest => dest.QuantidadeUsinas, opt => opt.MapFrom(src => src.Usinas != null ? src.Usinas.Count(u => u.Ativo) : 0));

        // CreateTipoUsinaDto ? TipoUsina
        CreateMap<CreateTipoUsinaDto, TipoUsina>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.Ignore())
            .ForMember(dest => dest.Usinas, opt => opt.Ignore());

        // UpdateTipoUsinaDto ? TipoUsina
        CreateMap<UpdateTipoUsinaDto, TipoUsina>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.Usinas, opt => opt.Ignore());

        // === EMPRESA MAPPINGS ===
        
        // Empresa ? EmpresaDto
        CreateMap<Empresa, EmpresaDto>()
            .ForMember(dest => dest.QuantidadeUsinas, opt => opt.MapFrom(src => src.Usinas != null ? src.Usinas.Count(u => u.Ativo) : 0));

        // CreateEmpresaDto ? Empresa
        CreateMap<CreateEmpresaDto, Empresa>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.Ignore())
            .ForMember(dest => dest.Usinas, opt => opt.Ignore());

        // UpdateEmpresaDto ? Empresa
        CreateMap<UpdateEmpresaDto, Empresa>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.Usinas, opt => opt.Ignore());
    }
}
