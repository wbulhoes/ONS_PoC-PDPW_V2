using PDPW.Application.DTOs.OfertaRespostaVoluntaria;
using PDPW.Domain.Common;

namespace PDPW.Application.Interfaces;

public interface IOfertaRespostaVoluntariaService
{
    Task<Result<IEnumerable<OfertaRespostaVoluntariaDto>>> GetAllAsync();
    Task<Result<OfertaRespostaVoluntariaDto>> GetByIdAsync(int id);
    Task<Result<IEnumerable<OfertaRespostaVoluntariaDto>>> GetByEmpresaAsync(int empresaId);
    Task<Result<IEnumerable<OfertaRespostaVoluntariaDto>>> GetByDataPDPAsync(DateTime dataPDP);
    Task<Result<IEnumerable<OfertaRespostaVoluntariaDto>>> GetPendentesAsync();
    Task<Result<IEnumerable<OfertaRespostaVoluntariaDto>>> GetByTipoProgramaAsync(string tipoPrograma);
    Task<Result<IEnumerable<OfertaRespostaVoluntariaDto>>> GetAprovadasAsync();
    Task<Result<IEnumerable<OfertaRespostaVoluntariaDto>>> GetRejeitadasAsync();
    Task<Result<OfertaRespostaVoluntariaDto>> CreateAsync(CreateOfertaRespostaVoluntariaDto dto);
    Task<Result<OfertaRespostaVoluntariaDto>> UpdateAsync(int id, UpdateOfertaRespostaVoluntariaDto dto);
    Task<Result> DeleteAsync(int id);
    Task<Result> AprovarAsync(int id, AprovarOfertaRespostaVoluntariaDto dto);
    Task<Result> RejeitarAsync(int id, RejeitarOfertaRespostaVoluntariaDto dto);
}
