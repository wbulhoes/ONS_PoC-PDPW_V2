using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;

namespace PDPW.Infrastructure.Data.Seed;

/// <summary>
/// Seed data para Usinas
/// Principais usinas do Sistema Interligado Nacional (SIN)
/// </summary>
public static class UsinaSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var usinas = new List<Usina>
        {
            // ========== HIDRELÉTRICAS ==========
            new Usina
            {
                Id = 1,
                Codigo = "UHE-ITAIPU",
                Nome = "Usina Hidrelétrica de Itaipu",
                TipoUsinaId = 1, // Hidrelétrica
                EmpresaId = 1,   // Itaipu Binacional
                CapacidadeInstalada = 14000.00m,
                Localizacao = "Foz do Iguaçu, PR - Fronteira Brasil/Paraguai",
                DataOperacao = DateTime.Parse("1984-05-05"),
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },
            new Usina
            {
                Id = 2,
                Codigo = "UHE-BELO-MONTE",
                Nome = "Usina Hidrelétrica Belo Monte",
                TipoUsinaId = 1, // Hidrelétrica
                EmpresaId = 2,   // Eletronorte
                CapacidadeInstalada = 11233.00m,
                Localizacao = "Altamira, PA - Rio Xingu",
                DataOperacao = DateTime.Parse("2016-04-29"),
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },
            new Usina
            {
                Id = 3,
                Codigo = "UHE-TUCURUI",
                Nome = "Usina Hidrelétrica de Tucuruí",
                TipoUsinaId = 1, // Hidrelétrica
                EmpresaId = 2,   // Eletronorte
                CapacidadeInstalada = 8370.00m,
                Localizacao = "Tucuruí, PA - Rio Tocantins",
                DataOperacao = DateTime.Parse("1984-11-22"),
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },
            new Usina
            {
                Id = 4,
                Codigo = "UHE-SAO-SIMAO",
                Nome = "Usina Hidrelétrica de São Simão",
                TipoUsinaId = 1, // Hidrelétrica
                EmpresaId = 3,   // Furnas
                CapacidadeInstalada = 1710.00m,
                Localizacao = "São Simão, GO - Rio Paranaíba",
                DataOperacao = DateTime.Parse("1978-02-13"),
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },
            new Usina
            {
                Id = 5,
                Codigo = "UHE-SOBRADINHO",
                Nome = "Usina Hidrelétrica de Sobradinho",
                TipoUsinaId = 1, // Hidrelétrica
                EmpresaId = 4,   // Chesf
                CapacidadeInstalada = 1050.40m,
                Localizacao = "Sobradinho, BA - Rio São Francisco",
                DataOperacao = DateTime.Parse("1979-01-17"),
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },
            new Usina
            {
                Id = 6,
                Codigo = "UHE-ITUMBIARA",
                Nome = "Usina Hidrelétrica de Itumbiara",
                TipoUsinaId = 1, // Hidrelétrica
                EmpresaId = 3,   // Furnas
                CapacidadeInstalada = 2082.00m,
                Localizacao = "Itumbiara, GO - Rio Paranaíba",
                DataOperacao = DateTime.Parse("1980-10-15"),
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },

            // ========== TÉRMICAS ==========
            new Usina
            {
                Id = 7,
                Codigo = "UTE-TERMO-MARANHAO",
                Nome = "Usina Termelétrica do Maranhão",
                TipoUsinaId = 2, // Térmica
                EmpresaId = 2,   // Eletronorte
                CapacidadeInstalada = 338.00m,
                Localizacao = "Miranda do Norte, MA",
                DataOperacao = DateTime.Parse("2013-08-01"),
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },
            new Usina
            {
                Id = 8,
                Codigo = "UTE-TERMO-PECEM",
                Nome = "Usina Termelétrica de Pecém",
                TipoUsinaId = 2, // Térmica
                EmpresaId = 4,   // Chesf
                CapacidadeInstalada = 720.00m,
                Localizacao = "São Gonçalo do Amarante, CE",
                DataOperacao = DateTime.Parse("2012-12-01"),
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },

            // ========== NUCLEAR ==========
            new Usina
            {
                Id = 9,
                Codigo = "UTN-ANGRA-I",
                Nome = "Usina Termonuclear Angra I",
                TipoUsinaId = 5, // Nuclear
                EmpresaId = 7,   // Eletronuclear
                CapacidadeInstalada = 640.00m,
                Localizacao = "Angra dos Reis, RJ",
                DataOperacao = DateTime.Parse("1985-01-01"),
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },
            new Usina
            {
                Id = 10,
                Codigo = "UTN-ANGRA-II",
                Nome = "Usina Termonuclear Angra II",
                TipoUsinaId = 5, // Nuclear
                EmpresaId = 7,   // Eletronuclear
                CapacidadeInstalada = 1350.00m,
                Localizacao = "Angra dos Reis, RJ",
                DataOperacao = DateTime.Parse("2001-02-01"),
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            }
        };

        modelBuilder.Entity<Usina>().HasData(usinas);
    }
}
