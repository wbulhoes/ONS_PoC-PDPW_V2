using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Repositories;

/// <summary>
/// Repositório de Ofertas de Exportação
/// </summary>
public class OfertaExportacaoRepository : BaseRepository<OfertaExportacao>, IOfertaExportacaoRepository
{
    public OfertaExportacaoRepository(PdpwDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Obtém todas as ofertas de exportação com relacionamentos
    /// </summary>
    public override async Task<IEnumerable<OfertaExportacao>> GetAllAsync()
    {
        return await _dbSet
            .Where(e => e.Ativo)
            .Include(o => o.Usina)
                .ThenInclude(u => u!.Empresa)
            .Include(o => o.SemanaPMO)
            .OrderByDescending(o => o.DataOferta)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém oferta de exportação por ID com relacionamentos
    /// </summary>
    public override async Task<OfertaExportacao?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(o => o.Usina)
                .ThenInclude(u => u!.Empresa)
            .Include(o => o.SemanaPMO)
            .FirstOrDefaultAsync(o => o.Id == id && o.Ativo);
    }

    /// <summary>
    /// Obtém ofertas de exportação por usina
    /// </summary>
    public async Task<IEnumerable<OfertaExportacao>> GetByUsinaAsync(int usinaId)
    {
        return await _dbSet
            .Where(o => o.UsinaId == usinaId && o.Ativo)
            .Include(o => o.Usina)
                .ThenInclude(u => u!.Empresa)
            .Include(o => o.SemanaPMO)
            .OrderByDescending(o => o.DataOferta)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém ofertas de exportação por data PDP
    /// </summary>
    public async Task<IEnumerable<OfertaExportacao>> GetByDataPDPAsync(DateTime dataPDP)
    {
        var dataInicio = dataPDP.Date;
        var dataFim = dataPDP.Date.AddDays(1);

        return await _dbSet
            .Where(o => o.DataPDP >= dataInicio && o.DataPDP < dataFim && o.Ativo)
            .Include(o => o.Usina)
                .ThenInclude(u => u!.Empresa)
            .Include(o => o.SemanaPMO)
            .OrderBy(o => o.Usina!.Nome)
            .ThenBy(o => o.HoraInicial)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém ofertas de exportação pendentes de análise do ONS
    /// </summary>
    public async Task<IEnumerable<OfertaExportacao>> GetPendentesAnaliseONSAsync()
    {
        return await _dbSet
            .Where(o => o.FlgAprovadoONS == null && o.Ativo)
            .Include(o => o.Usina)
                .ThenInclude(u => u!.Empresa)
            .Include(o => o.SemanaPMO)
            .OrderBy(o => o.DataPDP)
            .ThenBy(o => o.Usina!.Nome)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém ofertas de exportação pendentes para uma data PDP específica
    /// </summary>
    public async Task<IEnumerable<OfertaExportacao>> GetPendentesAnaliseONSByDataPDPAsync(DateTime dataPDP)
    {
        var dataInicio = dataPDP.Date;
        var dataFim = dataPDP.Date.AddDays(1);

        return await _dbSet
            .Where(o => o.FlgAprovadoONS == null && 
                       o.DataPDP >= dataInicio && 
                       o.DataPDP < dataFim && 
                       o.Ativo)
            .Include(o => o.Usina)
                .ThenInclude(u => u!.Empresa)
            .Include(o => o.SemanaPMO)
            .OrderBy(o => o.Usina!.Nome)
            .ThenBy(o => o.HoraInicial)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém ofertas de exportação por período
    /// </summary>
    public async Task<IEnumerable<OfertaExportacao>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
    {
        return await _dbSet
            .Where(o => o.DataPDP >= dataInicio && o.DataPDP <= dataFim && o.Ativo)
            .Include(o => o.Usina)
                .ThenInclude(u => u!.Empresa)
            .Include(o => o.SemanaPMO)
            .OrderBy(o => o.DataPDP)
            .ThenBy(o => o.Usina!.Nome)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém ofertas de exportação aprovadas
    /// </summary>
    public async Task<IEnumerable<OfertaExportacao>> GetAprovadasAsync()
    {
        return await _dbSet
            .Where(o => o.FlgAprovadoONS == true && o.Ativo)
            .Include(o => o.Usina)
                .ThenInclude(u => u!.Empresa)
            .Include(o => o.SemanaPMO)
            .OrderByDescending(o => o.DataAnaliseONS)
            .ToListAsync();
    }

    /// <summary>
    /// Obtém ofertas de exportação rejeitadas
    /// </summary>
    public async Task<IEnumerable<OfertaExportacao>> GetRejeitadasAsync()
    {
        return await _dbSet
            .Where(o => o.FlgAprovadoONS == false && o.Ativo)
            .Include(o => o.Usina)
                .ThenInclude(u => u!.Empresa)
            .Include(o => o.SemanaPMO)
            .OrderByDescending(o => o.DataAnaliseONS)
            .ToListAsync();
    }

    /// <summary>
    /// Aprova oferta de exportação
    /// </summary>
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

    /// <summary>
    /// Rejeita oferta de exportação
    /// </summary>
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

    /// <summary>
    /// Verifica se existe oferta de exportação não analisada para uma data PDP
    /// </summary>
    public async Task<bool> ExisteOfertaPendenteAnaliseONSAsync(DateTime dataPDP)
    {
        var dataInicio = dataPDP.Date;
        var dataFim = dataPDP.Date.AddDays(1);

        return await _dbSet.AnyAsync(o => 
            o.FlgAprovadoONS == null && 
            o.DataPDP >= dataInicio && 
            o.DataPDP < dataFim && 
            o.Ativo);
    }

    /// <summary>
    /// Verifica se permite exclusão de ofertas (data PDP >= D+1)
    /// </summary>
    public async Task<bool> PermiteExclusaoAsync(DateTime dataPDP)
    {
        // Permite exclusão se a data do PDP for maior ou igual a amanhã
        var amanha = DateTime.Now.Date.AddDays(1);
        return dataPDP.Date >= amanha;
    }
}
