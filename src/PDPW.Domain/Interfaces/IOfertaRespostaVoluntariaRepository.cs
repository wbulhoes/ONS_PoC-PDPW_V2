using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

/// <summary>
/// Interface para repositório de Ofertas de Resposta Voluntária da Demanda
/// </summary>
public interface IOfertaRespostaVoluntariaRepository
{
    Task<IEnumerable<OfertaRespostaVoluntaria>> GetAllAsync();
    Task<OfertaRespostaVoluntaria?> GetByIdAsync(int id);
    Task<IEnumerable<OfertaRespostaVoluntaria>> GetByEmpresaAsync(int empresaId);
    Task<IEnumerable<OfertaRespostaVoluntaria>> GetByDataPDPAsync(DateTime dataPDP);
    Task<IEnumerable<OfertaRespostaVoluntaria>> GetPendentesAnaliseONSAsync();
    Task<IEnumerable<OfertaRespostaVoluntaria>> GetByTipoProgramaAsync(string tipoPrograma);
    Task<IEnumerable<OfertaRespostaVoluntaria>> GetAprovadasAsync();
    Task<IEnumerable<OfertaRespostaVoluntaria>> GetRejeitadasAsync();
    Task<OfertaRespostaVoluntaria> AddAsync(OfertaRespostaVoluntaria oferta);
    Task UpdateAsync(OfertaRespostaVoluntaria oferta);
    Task DeleteAsync(int id);
    Task AprovarAsync(int id, string usuarioONS, string? observacao = null);
    Task RejeitarAsync(int id, string usuarioONS, string observacao);
    Task<bool> ExistsAsync(int id);
}
