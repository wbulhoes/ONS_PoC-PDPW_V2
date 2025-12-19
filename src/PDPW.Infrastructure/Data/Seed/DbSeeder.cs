using Microsoft.EntityFrameworkCore;

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
        // Aplicar seeds na ordem correta (respeitando FKs)
        
        // 1. Tabelas sem dependências
        TipoUsinaSeed.Seed(modelBuilder);
        EmpresaSeed.Seed(modelBuilder);
        
        // 2. Tabelas que dependem das anteriores
        UsinaSeed.Seed(modelBuilder);
        
        // Adicionar mais seeds conforme forem criados:
        // MotivosRestricaoSeed.Seed(modelBuilder);
        // SemanaPMOSeed.Seed(modelBuilder);
        // etc...
    }
}
