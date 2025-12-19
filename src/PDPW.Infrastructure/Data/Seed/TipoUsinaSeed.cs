using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;

namespace PDPW.Infrastructure.Data.Seed;

/// <summary>
/// Seed data para TiposUsina
/// Baseado no Sistema Elétrico Brasileiro (ONS)
/// </summary>
public static class TipoUsinaSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var tiposUsina = new List<TipoUsina>
        {
            new TipoUsina
            {
                Id = 1,
                Nome = "Hidrelétrica",
                Descricao = "Usina que gera energia através da força da água",
                FonteEnergia = "Hídrica",
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },
            new TipoUsina
            {
                Id = 2,
                Nome = "Térmica",
                Descricao = "Usina que gera energia através da queima de combustíveis",
                FonteEnergia = "Combustíveis Fósseis / Biomassa",
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },
            new TipoUsina
            {
                Id = 3,
                Nome = "Eólica",
                Descricao = "Usina que gera energia através da força dos ventos",
                FonteEnergia = "Eólica",
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },
            new TipoUsina
            {
                Id = 4,
                Nome = "Solar",
                Descricao = "Usina que gera energia através da luz solar",
                FonteEnergia = "Solar",
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },
            new TipoUsina
            {
                Id = 5,
                Nome = "Nuclear",
                Descricao = "Usina que gera energia através da fissão nuclear",
                FonteEnergia = "Nuclear",
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            }
        };

        modelBuilder.Entity<TipoUsina>().HasData(tiposUsina);
    }
}
