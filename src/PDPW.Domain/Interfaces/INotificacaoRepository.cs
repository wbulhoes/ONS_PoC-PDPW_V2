using PDPW.Domain.Entities;

namespace PDPW.Domain.Interfaces;

public interface INotificacaoRepository
{
    Task<IEnumerable<Notificacao>> GetAllAsync();
    Task<Notificacao?> GetByIdAsync(int id);
    Task<IEnumerable<Notificacao>> GetByDestinatarioAsync(int destinatarioId, string tipoDestinatario);
    Task<IEnumerable<Notificacao>> GetNaoLidasAsync(int? destinatarioId = null);
    Task<IEnumerable<Notificacao>> GetPorCategoriaAsync(string categoria);
    Task<IEnumerable<Notificacao>> GetPorPrioridadeAsync(string prioridade);
    Task<IEnumerable<Notificacao>> GetVencendasAsync(int diasAntecedencia = 7);
    Task<Notificacao> AddAsync(Notificacao notificacao);
    Task MarcarComoLidaAsync(int id);
    Task MarcarVariasComoLidasAsync(IEnumerable<int> ids);
    Task DeleteAsync(int id);
    Task<int> ContarNaoLidasAsync(int? destinatarioId = null);
    Task<bool> ExistsAsync(int id);
}
