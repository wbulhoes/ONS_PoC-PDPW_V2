using AutoMapper;
using PDPW.Application.DTOs.Usina;
using PDPW.Application.DTOs.TipoUsina;
using PDPW.Application.DTOs.Empresa;
using PDPW.Application.DTOs.SemanaPmo;
using PDPW.Application.DTOs.EquipePdp;
using PDPW.Application.DTOs.UnidadeGeradora;
using PDPW.Application.DTOs.ParadaUG;
using PDPW.Application.DTOs.MotivoRestricao;
using PDPW.Application.DTOs.Balanco;
using PDPW.Application.DTOs.Intercambio;
using PDPW.Application.DTOs.OfertaExportacao;
using PDPW.Application.DTOs.OfertaRespostaVoluntaria;
using PDPW.Application.DTOs.PrevisaoEolica;
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

        // === SEMANA PMO MAPPINGS ===
        
        // SemanaPMO ? SemanaPmoDto
        CreateMap<SemanaPMO, SemanaPmoDto>()
            .ForMember(dest => dest.QuantidadeArquivos, opt => opt.MapFrom(src => src.ArquivosDadger != null ? src.ArquivosDadger.Count(a => a.Ativo) : 0));

        // CreateSemanaPmoDto ? SemanaPMO
        CreateMap<CreateSemanaPmoDto, SemanaPMO>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.Ignore())
            .ForMember(dest => dest.ArquivosDadger, opt => opt.Ignore())
            .ForMember(dest => dest.DCAs, opt => opt.Ignore())
            .ForMember(dest => dest.DCRs, opt => opt.Ignore());

        // UpdateSemanaPmoDto ? SemanaPMO
        CreateMap<UpdateSemanaPmoDto, SemanaPMO>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.ArquivosDadger, opt => opt.Ignore())
            .ForMember(dest => dest.DCAs, opt => opt.Ignore())
            .ForMember(dest => dest.DCRs, opt => opt.Ignore());

        // === EQUIPE PDP MAPPINGS ===
        
        // EquipePDP ? EquipePdpDto
        CreateMap<EquipePDP, EquipePdpDto>()
            .ForMember(dest => dest.QuantidadeMembros, opt => opt.MapFrom(src => src.Membros != null ? src.Membros.Count(m => m.Ativo) : 0));

        // CreateEquipePdpDto ? EquipePDP
        CreateMap<CreateEquipePdpDto, EquipePDP>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.Ignore())
            .ForMember(dest => dest.Membros, opt => opt.Ignore());

        // UpdateEquipePdpDto ? EquipePDP
        CreateMap<UpdateEquipePdpDto, EquipePDP>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.Membros, opt => opt.Ignore());

        // === UNIDADE GERADORA MAPPINGS ===
        
        // UnidadeGeradora ? UnidadeGeradoraDto
        CreateMap<UnidadeGeradora, UnidadeGeradoraDto>()
            .ForMember(dest => dest.NomeUsina, opt => opt.MapFrom(src => src.Usina!.Nome))
            .ForMember(dest => dest.CodigoUsina, opt => opt.MapFrom(src => src.Usina!.Codigo));

        // CreateUnidadeGeradoraDto ? UnidadeGeradora
        CreateMap<CreateUnidadeGeradoraDto, UnidadeGeradora>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.Ignore())
            .ForMember(dest => dest.Usina, opt => opt.Ignore())
            .ForMember(dest => dest.Restricoes, opt => opt.Ignore())
            .ForMember(dest => dest.Paradas, opt => opt.Ignore());

        // UpdateUnidadeGeradoraDto ? UnidadeGeradora
        CreateMap<UpdateUnidadeGeradoraDto, UnidadeGeradora>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.Usina, opt => opt.Ignore())
            .ForMember(dest => dest.Restricoes, opt => opt.Ignore())
            .ForMember(dest => dest.Paradas, opt => opt.Ignore());

        // === PARADA UG MAPPINGS ===
        
        // ParadaUG ? ParadaUGDto
        CreateMap<ParadaUG, ParadaUGDto>()
            .ForMember(dest => dest.UnidadeGeradora, opt => opt.MapFrom(src => src.UnidadeGeradora!.Nome))
            .ForMember(dest => dest.CodigoUnidade, opt => opt.MapFrom(src => src.UnidadeGeradora!.Codigo));

        // CreateParadaUGDto ? ParadaUG
        CreateMap<CreateParadaUGDto, ParadaUG>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.Ignore())
            .ForMember(dest => dest.UnidadeGeradora, opt => opt.Ignore());

        // UpdateParadaUGDto ? ParadaUG
        CreateMap<UpdateParadaUGDto, ParadaUG>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.UnidadeGeradora, opt => opt.Ignore());

        // === MOTIVO RESTRIÇÃO MAPPINGS ===
        
        // MotivoRestricao ? MotivoRestricaoDto
        CreateMap<MotivoRestricao, MotivoRestricaoDto>()
            .ForMember(dest => dest.QuantidadeRestricoesUG, 
                opt => opt.MapFrom(src => src.RestricoesUG != null ? src.RestricoesUG.Count(r => r.Ativo) : 0))
            .ForMember(dest => dest.QuantidadeRestricoesUS, 
                opt => opt.MapFrom(src => src.RestricoesUS != null ? src.RestricoesUS.Count(r => r.Ativo) : 0));

        // CreateMotivoRestricaoDto ? MotivoRestricao
        CreateMap<CreateMotivoRestricaoDto, MotivoRestricao>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.Ignore())
            .ForMember(dest => dest.RestricoesUG, opt => opt.Ignore())
            .ForMember(dest => dest.RestricoesUS, opt => opt.Ignore());

        // UpdateMotivoRestricaoDto ? MotivoRestricao
        CreateMap<UpdateMotivoRestricaoDto, MotivoRestricao>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.RestricoesUG, opt => opt.Ignore())
            .ForMember(dest => dest.RestricoesUS, opt => opt.Ignore());

        // === BALANÇO MAPPINGS ===
        
        // Balanco ? BalancoDto
        CreateMap<Balanco, BalancoDto>()
            .ForMember(dest => dest.BalancoCalculado, 
                opt => opt.MapFrom(src => src.Geracao - src.Carga + src.Intercambio - src.Perdas - src.Deficit));

        // CreateBalancoDto ? Balanco
        CreateMap<CreateBalancoDto, Balanco>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.Ignore());

        // UpdateBalancoDto ? Balanco
        CreateMap<UpdateBalancoDto, Balanco>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore());

        // === INTERCÂMBIO MAPPINGS ===
        
        // Intercambio ? IntercambioDto
        CreateMap<Intercambio, IntercambioDto>();

        // CreateIntercambioDto ? Intercambio
        CreateMap<CreateIntercambioDto, Intercambio>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.Ignore());

        // UpdateIntercambioDto ? Intercambio
        CreateMap<UpdateIntercambioDto, Intercambio>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore());

        // === OFERTA EXPORTAÇÃO MAPPINGS ===
        
        // OfertaExportacao → OfertaExportacaoDto
        CreateMap<OfertaExportacao, OfertaExportacaoDto>()
            .ForMember(dest => dest.UsinaNome, opt => opt.MapFrom(src => src.Usina!.Nome))
            .ForMember(dest => dest.EmpresaNome, opt => opt.MapFrom(src => src.Usina!.Empresa!.Nome))
            .ForMember(dest => dest.SemanaPMO, opt => opt.MapFrom(src => 
                src.SemanaPMO != null ? $"Semana {src.SemanaPMO.Numero}/{src.SemanaPMO.Ano}" : null));

        // CreateOfertaExportacaoDto → OfertaExportacao
        CreateMap<CreateOfertaExportacaoDto, OfertaExportacao>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.Ignore())
            .ForMember(dest => dest.FlgAprovadoONS, opt => opt.Ignore())
            .ForMember(dest => dest.DataAnaliseONS, opt => opt.Ignore())
            .ForMember(dest => dest.UsuarioAnaliseONS, opt => opt.Ignore())
            .ForMember(dest => dest.ObservacaoONS, opt => opt.Ignore())
            .ForMember(dest => dest.Usina, opt => opt.Ignore())
            .ForMember(dest => dest.SemanaPMO, opt => opt.Ignore());

        // UpdateOfertaExportacaoDto → OfertaExportacao
        CreateMap<UpdateOfertaExportacaoDto, OfertaExportacao>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.FlgAprovadoONS, opt => opt.Ignore())
            .ForMember(dest => dest.DataAnaliseONS, opt => opt.Ignore())
            .ForMember(dest => dest.UsuarioAnaliseONS, opt => opt.Ignore())
            .ForMember(dest => dest.ObservacaoONS, opt => opt.Ignore())
            .ForMember(dest => dest.Usina, opt => opt.Ignore())
            .ForMember(dest => dest.SemanaPMO, opt => opt.Ignore());

        // === OFERTA RESPOSTA VOLUNTÁRIA MAPPINGS ===
        
        // OfertaRespostaVoluntaria → OfertaRespostaVoluntariaDto
        CreateMap<OfertaRespostaVoluntaria, OfertaRespostaVoluntariaDto>()
            .ForMember(dest => dest.EmpresaNome, opt => opt.MapFrom(src => src.Empresa!.Nome))
            .ForMember(dest => dest.SemanaPMO, opt => opt.MapFrom(src => 
                src.SemanaPMO != null ? $"Semana {src.SemanaPMO.Numero}/{src.SemanaPMO.Ano}" : null));

        // CreateOfertaRespostaVoluntariaDto → OfertaRespostaVoluntaria
        CreateMap<CreateOfertaRespostaVoluntariaDto, OfertaRespostaVoluntaria>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.Ignore())
            .ForMember(dest => dest.FlgAprovadoONS, opt => opt.Ignore())
            .ForMember(dest => dest.DataAnaliseONS, opt => opt.Ignore())
            .ForMember(dest => dest.UsuarioAnaliseONS, opt => opt.Ignore())
            .ForMember(dest => dest.ObservacaoONS, opt => opt.Ignore())
            .ForMember(dest => dest.Empresa, opt => opt.Ignore())
            .ForMember(dest => dest.SemanaPMO, opt => opt.Ignore());

        // UpdateOfertaRespostaVoluntariaDto → OfertaRespostaVoluntaria
        CreateMap<UpdateOfertaRespostaVoluntariaDto, OfertaRespostaVoluntaria>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.FlgAprovadoONS, opt => opt.Ignore())
            .ForMember(dest => dest.DataAnaliseONS, opt => opt.Ignore())
            .ForMember(dest => dest.UsuarioAnaliseONS, opt => opt.Ignore())
            .ForMember(dest => dest.ObservacaoONS, opt => opt.Ignore())
            .ForMember(dest => dest.Empresa, opt => opt.Ignore())
            .ForMember(dest => dest.SemanaPMO, opt => opt.Ignore());

        // === PREVISÃO EÓLICA MAPPINGS ===
        
        // PrevisaoEolica → PrevisaoEolicaDto
        CreateMap<PrevisaoEolica, PrevisaoEolicaDto>()
            .ForMember(dest => dest.UsinaNome, opt => opt.MapFrom(src => src.Usina!.Nome))
            .ForMember(dest => dest.SemanaPMO, opt => opt.MapFrom(src => 
                src.SemanaPMO != null ? $"Semana {src.SemanaPMO.Numero}/{src.SemanaPMO.Ano}" : null));

        // CreatePrevisaoEolicaDto → PrevisaoEolica
        CreateMap<CreatePrevisaoEolicaDto, PrevisaoEolica>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.Ativo, opt => opt.Ignore())
            .ForMember(dest => dest.GeracaoRealMWmed, opt => opt.Ignore())
            .ForMember(dest => dest.ErroAbsolutoMW, opt => opt.Ignore())
            .ForMember(dest => dest.ErroPercentual, opt => opt.Ignore())
            .ForMember(dest => dest.Usina, opt => opt.Ignore())
            .ForMember(dest => dest.SemanaPMO, opt => opt.Ignore());

        // UpdatePrevisaoEolicaDto → PrevisaoEolica
        CreateMap<UpdatePrevisaoEolicaDto, PrevisaoEolica>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
            .ForMember(dest => dest.DataAtualizacao, opt => opt.Ignore())
            .ForMember(dest => dest.GeracaoRealMWmed, opt => opt.Ignore())
            .ForMember(dest => dest.ErroAbsolutoMW, opt => opt.Ignore())
            .ForMember(dest => dest.ErroPercentual, opt => opt.Ignore())
            .ForMember(dest => dest.Usina, opt => opt.Ignore())
            .ForMember(dest => dest.SemanaPMO, opt => opt.Ignore());
    }
}
