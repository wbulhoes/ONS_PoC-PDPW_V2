using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;

namespace PDPW.Infrastructure.Data.Seed;

/// <summary>
/// Seed data para Empresas
/// Principais geradoras do Sistema Interligado Nacional (SIN)
/// </summary>
public static class EmpresaSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        var empresas = new List<Empresa>
        {
            new Empresa
            {
                Id = 1,
                Nome = "Itaipu Binacional",
                CNPJ = "00.341.583/0001-71",
                Telefone = "(45) 3520-5252",
                Email = "contato@itaipu.gov.br",
                Endereco = "Av. Tancredo Neves, 6731 - Foz do Iguaçu, PR",
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },
            new Empresa
            {
                Id = 2,
                Nome = "Eletronorte - Centrais Elétricas do Norte do Brasil",
                CNPJ = "00.357.038/0001-16",
                Telefone = "(61) 3429-5151",
                Email = "contato@eletronorte.gov.br",
                Endereco = "SCN - Quadra 6 - Conjunto A - Bloco C - Brasília, DF",
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },
            new Empresa
            {
                Id = 3,
                Nome = "Furnas Centrais Elétricas",
                CNPJ = "23.047.150/0001-13",
                Telefone = "(21) 2528-5600",
                Email = "ouvidoria@furnas.com.br",
                Endereco = "Rua Real Grandeza, 219 - Rio de Janeiro, RJ",
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },
            new Empresa
            {
                Id = 4,
                Nome = "Chesf - Companhia Hidro Elétrica do São Francisco",
                CNPJ = "33.541.368/0001-16",
                Telefone = "(81) 3229-2300",
                Email = "faleconosco@chesf.gov.br",
                Endereco = "Rua Delmiro Gouveia, 333 - Recife, PE",
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },
            new Empresa
            {
                Id = 5,
                Nome = "Eletrosul Centrais Elétricas",
                CNPJ = "00.073.957/0001-46",
                Telefone = "(48) 3231-7000",
                Email = "comunicacao@eletrosul.gov.br",
                Endereco = "Av. Prefeito Osmar Cunha, 183 - Florianópolis, SC",
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },
            new Empresa
            {
                Id = 6,
                Nome = "CESP - Companhia Energética de São Paulo",
                CNPJ = "60.933.603/0001-78",
                Telefone = "(11) 3138-7000",
                Email = "ouvidoria@cesp.com.br",
                Endereco = "Av. Nossa Senhora do Sabará, 5312 - São Paulo, SP",
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },
            new Empresa
            {
                Id = 7,
                Nome = "Eletronuclear - Eletrobrás Termonuclear",
                CNPJ = "42.540.211/0001-67",
                Telefone = "(21) 2588-1000",
                Email = "eletronuclear@eletronuclear.gov.br",
                Endereco = "Rua da Candelária, 65 - Rio de Janeiro, RJ",
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            },
            new Empresa
            {
                Id = 8,
                Nome = "COPEL - Companhia Paranaense de Energia",
                CNPJ = "76.483.817/0001-20",
                Telefone = "(41) 3331-4011",
                Email = "copel@copel.com",
                Endereco = "Rua José Izidoro Biazetto, 158 - Curitiba, PR",
                Ativo = true,
                DataCriacao = DateTime.Parse("2024-01-01")
            }
        };

        modelBuilder.Entity<Empresa>().HasData(empresas);
    }
}
