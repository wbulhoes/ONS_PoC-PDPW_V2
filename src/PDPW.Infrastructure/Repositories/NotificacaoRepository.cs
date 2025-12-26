using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

public class NotificacaoRepository : BaseRepository<Notificacao>, INotificacaoRepository
{
    public NotificacaoRepository(PdpwDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<Notificacao>> GetAllAsync()
    {
        return await _dbSet
            .Where(n => n.Ativo)
            .OrderByDescending(n => n.DataHoraEnvio)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notificacao>> GetByDestinatarioAsync(int destinatarioId, string tipoDestinatario)
    {
        return await _dbSet
            .Where(n => n.DestinatarioId == destinatarioId && 
                       n.TipoDestinatario == tipoDestinatario && 
                       n.Ativo)
            .OrderByDescending(n => n.DataHoraEnvio)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notificacao>> GetNaoLidasAsync(int? destinatarioId = null)
    {
        var query = _dbSet.Where(n => !n.Lida && n.Ativo);

        if (destinatarioId.HasValue)
        {
            query = query.Where(n => n.DestinatarioId == destinatarioId);
        }

        return await query
            .OrderByDescending(n => n.Prioridade)
            .ThenByDescending(n => n.DataHoraEnvio)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notificacao>> GetPorCategoriaAsync(string categoria)
    {
        return await _dbSet
            .Where(n => n.Categoria == categoria && n.Ativo)
            .OrderByDescending(n => n.DataHoraEnvio)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notificacao>> GetPorPrioridadeAsync(string prioridade)
    {
        return await _dbSet
            .Where(n => n.Prioridade == prioridade && !n.Lida && n.Ativo)
            .OrderByDescending(n => n.DataHoraEnvio)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notificacao>> GetVencendasAsync(int diasAntecedencia = 7)
    {
        var dataLimite = DateTime.Now.AddDays(diasAntecedencia);
        
        return await _dbSet
            .Where(n => n.DataVencimento.HasValue && 
                       n.DataVencimento.Value <= dataLimite && 
                       n.DataVencimento.Value >= DateTime.Now &&
                       !n.Lida && 
                       n.Ativo)
            .OrderBy(n => n.DataVencimento)
            .ToListAsync();
    }

    public async Task MarcarComoLidaAsync(int id)
    {
        var notificacao = await _dbSet.FindAsync(id);
        if (notificacao != null)
        {
            notificacao.MarcarComoLida();
            notificacao.DataAtualizacao = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    public async Task MarcarVariasComoLidasAsync(IEnumerable<int> ids)
    {
        var notificacoes = await _dbSet.Where(n => ids.Contains(n.Id)).ToListAsync();
        
        foreach (var notificacao in notificacoes)
        {
            notificacao.MarcarComoLida();
            notificacao.DataAtualizacao = DateTime.Now;
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task<int> ContarNaoLidasAsync(int? destinatarioId = null)
    {
        var query = _dbSet.Where(n => !n.Lida && n.Ativo);

        if (destinatarioId.HasValue)
        {
            query = query.Where(n => n.DestinatarioId == destinatarioId);
        }

        return await query.CountAsync();
    }
}
