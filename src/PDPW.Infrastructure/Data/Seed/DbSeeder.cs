using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;

namespace PDPW.Infrastructure.Data.Seed;

/// <summary>
/// Classe responsável por popular dados iniciais no banco
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Aplica seed data no ModelBuilder
    /// </summary>
    public static void Seed(ModelBuilder modelBuilder)
    {
        // Seed será implementado conforme APIs forem criadas
        // Exemplo de estrutura:
        
        // SeedTiposUsina(modelBuilder);
        // SeedEmpresas(modelBuilder);
        // SeedUsinas(modelBuilder);
        // etc...
    }

    // Exemplo de método de seed
    private static void SeedTiposUsina(ModelBuilder modelBuilder)
    {
        // Será implementado quando criar a entidade TipoUsina
    }

    // Método helper para gerar IDs sequenciais
    private static int GetNextId(int startId) => startId;
}
