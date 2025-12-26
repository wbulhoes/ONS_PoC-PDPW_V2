using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

public class OfertaRespostaVoluntariaRepository : BaseRepository<OfertaRespostaVoluntaria>, IOfertaRespostaVoluntariaRepository
{
    public OfertaRespostaVoluntariaRepository(PdpwDbContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<OfertaRespostaVoluntaria>> GetAllAsync()
    {
        return await _dbSet
            .Where(e => e.Ativo)
            .Include(o => o.Empresa)
            .Include(o => o.SemanaPMO)
            .OrderByDescending(o => o.DataOferta)
            .ToListAsync();
    }

    public override async Task<OfertaRespostaVoluntaria?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(o => o.Empresa)
            .Include(o => o.SemanaPMO)
            .FirstOrDefaultAsync(o => o.Id == id && o.Ativo);
    }

    public async Task<IEnumerable<OfertaRespostaVoluntaria>> GetByEmpresaAsync(int empresaId)
    {
        return await _dbSet
            .Where(o => o.EmpresaId == empresaId && o.Ativo)
            .Include(o => o.Empresa)
            .Include(o => o.SemanaPMO)
            .OrderByDescending(o => o.DataOferta)
            .ToListAsync();
    }

    public async Task<IEnumerable<OfertaRespostaVoluntaria>> GetByDataPDPAsync(DateTime dataPDP)
    {
        var dataInicio = dataPDP.Date;
        var dataFim = dataPDP.Date.AddDays(1);

        return await _dbSet
            .Where(o => o.DataPDP >= dataInicio && o.DataPDP < dataFim && o.Ativo)
            .Include(o => o.Empresa)
            .Include(o => o.SemanaPMO)
            .OrderBy(o => o.Empresa!.Nome)
            .ThenBy(o => o.HoraInicial)
            .ToListAsync();
    }

    public async Task<IEnumerable<OfertaRespostaVoluntaria>> GetPendentesAnaliseONSAsync()
    {
        return await _dbSet
            .Where(o => o.FlgAprovadoONS == null && o.Ativo)
            .Include(o => o.Empresa)
            .Include(o => o.SemanaPMO)
            .OrderBy(o => o.DataPDP)
            .ThenBy(o => o.Empresa!.Nome)
            .ToListAsync();
    }

    public async Task<IEnumerable<OfertaRespostaVoluntaria>> GetByTipoProgramaAsync(string tipoPrograma)
    {
        return await _dbSet
            .Where(o => o.TipoPrograma == tipoPrograma && o.Ativo)
            .Include(o => o.Empresa)
            .Include(o => o.SemanaPMO)
            .OrderByDescending(o => o.DataOferta)
            .ToListAsync();
    }

    public async Task<IEnumerable<OfertaRespostaVoluntaria>> GetAprovadasAsync()
    {
        return await _dbSet
            .Where(o => o.FlgAprovadoONS == true && o.Ativo)
            .Include(o => o.Empresa)
            .Include(o => o.SemanaPMO)
            .OrderByDescending(o => o.DataAnaliseONS)
            .ToListAsync();
    }

    public async Task<IEnumerable<OfertaRespostaVoluntaria>> GetRejeitadasAsync()
    {
        return await _dbSet
            .Where(o => o.FlgAprovadoONS == false && o.Ativo)
            .Include(o => o.Empresa)
            .Include(o => o.SemanaPMO)
            .OrderByDescending(o => o.DataAnaliseONS)
            .ToListAsync();
    }

    public async Task AprovarAsync(int id, string usuarioONS, string? observacao = null)
    {
        var oferta = await _dbSet.FindAsync(id);
        if (oferta != null)
        {
            oferta.FlgAprovadoONS = true;
            oferta.DataAnaliseONS = DateTime.Now;
            oferta.UsuarioAnaliseONS = usuarioONS;
            oferta.ObservacaoONS = observacao;
            oferta.DataAtualizacao = DateTime.Now;
            
            await _context.SaveChangesAsync();
        }
    }

    public async Task RejeitarAsync(int id, string usuarioONS, string observacao)
    {
        var oferta = await _dbSet.FindAsync(id);
        if (oferta != null)
        {
            oferta.FlgAprovadoONS = false;
            oferta.DataAnaliseONS = DateTime.Now;
            oferta.UsuarioAnaliseONS = usuarioONS;
            oferta.ObservacaoONS = observacao;
            oferta.DataAtualizacao = DateTime.Now;
            
            await _context.SaveChangesAsync();
        }
    }
}
