using PDPW.Domain.Entities;

namespace PDPW.UnitTests.Helpers;

/// <summary>
/// Builder para criar dados de teste de forma fluente
/// </summary>
public static class TestDataBuilder
{
    public static class Usinas
    {
        public static Usina CreateValid(int id = 1, string? codigo = null, string? nome = null)
        {
            return new Usina
            {
                Id = id,
                Codigo = codigo ?? $"UTE{id:D3}",
                Nome = nome ?? $"Usina Teste {id}",
                TipoUsinaId = 1,
                EmpresaId = 1,
                CapacidadeInstalada = 1000,
                Localizacao = "São Paulo, SP",
                DataOperacao = DateTime.Now.AddYears(-5),
                DataCriacao = DateTime.Now,
                Ativo = true
            };
        }

        public static List<Usina> CreateList(int count = 5)
        {
            var usinas = new List<Usina>();
            for (int i = 1; i <= count; i++)
            {
                usinas.Add(CreateValid(i));
            }
            return usinas;
        }
    }

    public static class TiposUsina
    {
        public static TipoUsina CreateValid(int id = 1, string? nome = null)
        {
            return new TipoUsina
            {
                Id = id,
                Nome = nome ?? $"Tipo {id}",
                Descricao = $"Descrição do Tipo {id}",
                FonteEnergia = "Térmica",
                DataCriacao = DateTime.Now,
                Ativo = true
            };
        }

        public static List<TipoUsina> CreateList(int count = 3)
        {
            var tipos = new List<TipoUsina>();
            for (int i = 1; i <= count; i++)
            {
                tipos.Add(CreateValid(i));
            }
            return tipos;
        }
    }

    public static class Empresas
    {
        public static Empresa CreateValid(int id = 1, string? nome = null, string? cnpj = null)
        {
            return new Empresa
            {
                Id = id,
                Nome = nome ?? $"Empresa Teste {id}",
                CNPJ = cnpj ?? $"{id:D14}",
                Telefone = "(11) 1234-5678",
                Email = $"contato{id}@empresa.com",
                Endereco = "Rua Teste, 123",
                DataCriacao = DateTime.Now,
                Ativo = true
            };
        }

        public static List<Empresa> CreateList(int count = 3)
        {
            var empresas = new List<Empresa>();
            for (int i = 1; i <= count; i++)
            {
                empresas.Add(CreateValid(i));
            }
            return empresas;
        }
    }

    public static class SemanasPMO
    {
        public static SemanaPMO CreateValid(int id = 1, int numero = 1, int ano = 2025)
        {
            return new SemanaPMO
            {
                Id = id,
                Numero = numero,
                Ano = ano,
                DataInicio = new DateTime(ano, 1, 1).AddDays((numero - 1) * 7),
                DataFim = new DateTime(ano, 1, 1).AddDays(numero * 7 - 1),
                Mes = 1,
                DataCriacao = DateTime.Now,
                Ativo = true
            };
        }

        public static List<SemanaPMO> CreateList(int count = 4)
        {
            var semanas = new List<SemanaPMO>();
            for (int i = 1; i <= count; i++)
            {
                semanas.Add(CreateValid(i, i, 2025));
            }
            return semanas;
        }
    }

    public static class EquipesPDP
    {
        public static EquipePDP CreateValid(int id = 1, string? nome = null)
        {
            return new EquipePDP
            {
                Id = id,
                Nome = nome ?? $"Equipe {id}",
                Email = $"equipe{id}@ons.org.br",
                Telefone = "(21) 1234-5678",
                DataCriacao = DateTime.Now,
                Ativo = true
            };
        }

        public static List<EquipePDP> CreateList(int count = 3)
        {
            var equipes = new List<EquipePDP>();
            for (int i = 1; i <= count; i++)
            {
                equipes.Add(CreateValid(i));
            }
            return equipes;
        }
    }
}
