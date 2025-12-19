using AutoMapper;

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
        
        // Ignorar propriedades de auditoria ao mapear para DTOs de criação
        AllowNullCollections = true;
        
        // Profiles individuais serão adicionados conforme APIs forem criadas
        // Exemplo:
        // CreateMap<Usina, UsinaDto>();
        // CreateMap<CreateUsinaDto, Usina>();
    }
}
