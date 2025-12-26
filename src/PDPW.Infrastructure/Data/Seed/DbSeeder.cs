using Microsoft.EntityFrameworkCore;

namespace PDPW.Infrastructure.Data.Seed;

/// <summary>
/// Orquestrador de seeds - Aplica todos os dados iniciais
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Aplica seed data no ModelBuilder
    /// SEEDER ÚNICO CONSOLIDADO
    /// </summary>
    public static void Seed(ModelBuilder modelBuilder)
    {
        // Único seeder com TODOS os dados
        // Fonte: Dados reais do sistema do cliente
        modelBuilder.SeedRealData();
    }
}
