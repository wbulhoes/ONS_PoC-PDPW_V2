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
        // Aplicar seeds na ordem correta (respeitando FKs)
        
        // 1. Tabelas sem dependências - Dados de exemplo
        TipoUsinaSeed.Seed(modelBuilder);
        EmpresaSeed.Seed(modelBuilder);
        EquipePdpSeed.SeedEquipesPdp(modelBuilder);
        
        // 2. Tabelas que dependem das anteriores - Dados de exemplo
        UsinaSeed.Seed(modelBuilder);
        SeedSemanasPMO(modelBuilder);
        SeedMotivosRestricao(modelBuilder);
        
        // 3. DADOS REAIS DO CLIENTE (50+ registros)
        modelBuilder.SeedLegacyData();
    }

    private static void SeedSemanasPMO(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SemanaPMO>().HasData(
            new SemanaPMO { Id = 1, Numero = 1, Ano = 2025, DataInicio = new DateTime(2025, 1, 4), DataFim = new DateTime(2025, 1, 10), DataCriacao = new DateTime(2024, 12, 1), Ativo = true },
            new SemanaPMO { Id = 2, Numero = 2, Ano = 2025, DataInicio = new DateTime(2025, 1, 11), DataFim = new DateTime(2025, 1, 17), DataCriacao = new DateTime(2024, 12, 1), Ativo = true },
            new SemanaPMO { Id = 3, Numero = 3, Ano = 2025, DataInicio = new DateTime(2025, 1, 18), DataFim = new DateTime(2025, 1, 24), DataCriacao = new DateTime(2024, 12, 1), Ativo = true }
        );
    }

    private static void SeedMotivosRestricao(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MotivoRestricao>().HasData(
            new MotivoRestricao { Id = 1, Nome = "Manutenção Programada", Descricao = "Parada para manutenção preventiva ou corretiva", Categoria = "Manutenção", DataCriacao = new DateTime(2024, 1, 1), Ativo = true },
            new MotivoRestricao { Id = 2, Nome = "Falha de Equipamento", Descricao = "Problema em equipamento que restringe a geração", Categoria = "Técnica", DataCriacao = new DateTime(2024, 1, 1), Ativo = true },
            new MotivoRestricao { Id = 3, Nome = "Restrição Hidráulica", Descricao = "Vazão insuficiente para geração plena", Categoria = "Hidráulica", DataCriacao = new DateTime(2024, 1, 1), Ativo = true },
            new MotivoRestricao { Id = 4, Nome = "Restrição de Transmissão", Descricao = "Limitação no sistema de transmissão", Categoria = "Elétrica", DataCriacao = new DateTime(2024, 1, 1), Ativo = true },
            new MotivoRestricao { Id = 5, Nome = "Teste Operacional", Descricao = "Teste de equipamentos ou sistemas", Categoria = "Operacional", DataCriacao = new DateTime(2024, 1, 1), Ativo = true }
        );
    }
}
