using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;
using System.Security.Cryptography;
using System.Text;

namespace PDPW.Infrastructure.Repositories;

public class SubmissaoAgenteRepository : BaseRepository<SubmissaoAgente>, ISubmissaoAgenteRepository
{
    public SubmissaoAgenteRepository(PdpwDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<SubmissaoAgente>> GetAllAsync()
    {
        return await _dbSet
            .Where(s => s.Ativo)
            .Include(s => s.Empresa)
            .Include(s => s.JanelaEnvio)
            .OrderByDescending(s => s.DataHoraSubmissao)
            .ToListAsync();
    }

    public async Task<IEnumerable<SubmissaoAgente>> GetByEmpresaAsync(int empresaId)
    {
        return await _dbSet
            .Where(s => s.EmpresaId == empresaId && s.Ativo)
            .Include(s => s.Empresa)
            .Include(s => s.JanelaEnvio)
            .OrderByDescending(s => s.DataHoraSubmissao)
            .ToListAsync();
    }

    public async Task<IEnumerable<SubmissaoAgente>> GetByTipoDadoAsync(string tipoDado)
    {
        return await _dbSet
            .Where(s => s.TipoDado == tipoDado && s.Ativo)
            .Include(s => s.Empresa)
            .OrderByDescending(s => s.DataHoraSubmissao)
            .ToListAsync();
    }

    public async Task<IEnumerable<SubmissaoAgente>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        return await _dbSet
            .Where(s => s.DataHoraSubmissao >= dataInicio && 
                       s.DataHoraSubmissao <= dataFim && 
                       s.Ativo)
            .Include(s => s.Empresa)
            .OrderByDescending(s => s.DataHoraSubmissao)
            .ToListAsync();
    }

    public async Task<IEnumerable<SubmissaoAgente>> GetForaJanelaAsync()
    {
        return await _dbSet
            .Where(s => !s.DentroJanela && s.Ativo)
            .Include(s => s.Empresa)
            .Include(s => s.JanelaEnvio)
            .OrderByDescending(s => s.DataHoraSubmissao)
            .ToListAsync();
    }

    public async Task<IEnumerable<SubmissaoAgente>> GetRejeitadasAsync()
    {
        return await _dbSet
            .Where(s => s.StatusSubmissao == "Rejeitada" && s.Ativo)
            .Include(s => s.Empresa)
            .OrderByDescending(s => s.DataHoraSubmissao)
            .ToListAsync();
    }

    public async Task<SubmissaoAgente> RegistrarSubmissaoAsync(
        int empresaId,
        string tipoDado,
        int registroId,
        DateTime dataReferencia,
        string usuarioEnvio,
        string? ipOrigem = null,
        bool dentroJanela = true)
    {
        var submissao = new SubmissaoAgente
        {
            EmpresaId = empresaId,
            TipoDado = tipoDado,
            RegistroId = registroId,
            DataReferencia = dataReferencia,
            DataHoraSubmissao = DateTime.Now,
            UsuarioEnvio = usuarioEnvio,
            IpOrigem = ipOrigem,
            DentroJanela = dentroJanela,
            StatusSubmissao = "Aceita",
            Ativo = true,
            DataCriacao = DateTime.Now
        };

        await _dbSet.AddAsync(submissao);
        await _context.SaveChangesAsync();
        
        return submissao;
    }

    public async Task RejeitarSubmissaoAsync(int id, string motivo)
    {
        var submissao = await _dbSet.FindAsync(id);
        if (submissao != null)
        {
            submissao.StatusSubmissao = "Rejeitada";
            submissao.MotivoRejeicao = motivo;
            submissao.DataAtualizacao = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    public async Task AceitarSubmissaoAsync(int id)
    {
        var submissao = await _dbSet.FindAsync(id);
        if (submissao != null)
        {
            submissao.StatusSubmissao = "Aceita";
            submissao.MotivoRejeicao = null;
            submissao.DataAtualizacao = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExisteSubmissaoDuplicadaAsync(string hashDados)
    {
        return await _dbSet.AnyAsync(s => s.HashDados == hashDados && s.Ativo);
    }

    public async Task<int> ContarSubmissoesPorEmpresaAsync(int empresaId, DateTime dataInicio, DateTime dataFim)
    {
        return await _dbSet
            .CountAsync(s => s.EmpresaId == empresaId &&
                           s.DataHoraSubmissao >= dataInicio &&
                           s.DataHoraSubmissao <= dataFim &&
                           s.Ativo);
    }
}
