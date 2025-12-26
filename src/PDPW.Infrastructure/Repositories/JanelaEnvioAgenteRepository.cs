using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

public class JanelaEnvioAgenteRepository : BaseRepository<JanelaEnvioAgente>, IJanelaEnvioAgenteRepository
{
    public JanelaEnvioAgenteRepository(PdpwDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<JanelaEnvioAgente>> GetAllAsync()
    {
        return await _dbSet
            .Where(j => j.Ativo)
            .Include(j => j.SemanaPMO)
            .OrderByDescending(j => j.DataHoraInicio)
            .ToListAsync();
    }

    public override async Task<JanelaEnvioAgente?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(j => j.SemanaPMO)
            .FirstOrDefaultAsync(j => j.Id == id && j.Ativo);
    }

    public async Task<IEnumerable<JanelaEnvioAgente>> GetByTipoDadoAsync(string tipoDado)
    {
        return await _dbSet
            .Where(j => j.TipoDado == tipoDado && j.Ativo)
            .Include(j => j.SemanaPMO)
            .OrderByDescending(j => j.DataHoraInicio)
            .ToListAsync();
    }

    public async Task<IEnumerable<JanelaEnvioAgente>> GetAbertas()
    {
        var agora = DateTime.Now;
        return await _dbSet
            .Where(j => j.Status == "Aberta" && 
                       j.DataHoraInicio <= agora && 
                       j.DataHoraFim >= agora && 
                       j.Ativo)
            .Include(j => j.SemanaPMO)
            .OrderBy(j => j.TipoDado)
            .ToListAsync();
    }

    public async Task<JanelaEnvioAgente?> GetJanelaAtualAsync(string tipoDado, DateTime dataReferencia)
    {
        var dataInicio = dataReferencia.Date;
        var dataFim = dataReferencia.Date.AddDays(1);

        return await _dbSet
            .Where(j => j.TipoDado == tipoDado &&
                       j.DataReferencia >= dataInicio &&
                       j.DataReferencia < dataFim &&
                       j.Ativo)
            .Include(j => j.SemanaPMO)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ValidarEnvioPermitidoAsync(string tipoDado, DateTime dataReferencia)
    {
        var janela = await GetJanelaAtualAsync(tipoDado, dataReferencia);
        
        if (janela == null)
            return false; // Sem janela configurada = não permite
        
        return janela.PermiteEnvio();
    }

    public async Task FecharJanelaAsync(int id, string usuario, string? observacao = null)
    {
        var janela = await _dbSet.FindAsync(id);
        if (janela != null)
        {
            janela.Status = "Fechada";
            janela.Observacoes = observacao;
            janela.DataAtualizacao = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    public async Task AbrirJanelaAsync(int id, string usuario, string? observacao = null)
    {
        var janela = await _dbSet.FindAsync(id);
        if (janela != null)
        {
            janela.Status = "Aberta";
            janela.Observacoes = observacao;
            janela.DataAtualizacao = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    public async Task AutorizarExcecaoAsync(int id, string usuario, string observacao)
    {
        var janela = await _dbSet.FindAsync(id);
        if (janela != null)
        {
            janela.PermiteEnvioForaJanela = true;
            janela.UsuarioAutorizacaoExcecao = usuario;
            janela.DataHoraAutorizacaoExcecao = DateTime.Now;
            janela.Observacoes = observacao;
            janela.DataAtualizacao = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}
