using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;

namespace PDPW.Infrastructure.Data;

/// <summary>
/// Contexto do banco de dados para o PDPW
/// </summary>
public class PdpwDbContext : DbContext
{
    public PdpwDbContext(DbContextOptions<PdpwDbContext> options) : base(options)
    {
    }

    public DbSet<DadoEnergetico> DadosEnergeticos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DadoEnergetico>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CodigoUsina).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.ProducaoMWh).HasPrecision(18, 2);
            entity.Property(e => e.CapacidadeDisponivel).HasPrecision(18, 2);
            entity.HasIndex(e => e.DataReferencia);
        });
    }
}
