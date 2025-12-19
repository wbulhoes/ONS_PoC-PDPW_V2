using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;

namespace PDPW.Infrastructure.Data.Seed;

/// <summary>
/// Seed data para Equipes PDP
/// </summary>
public static class EquipePdpSeed
{
    public static void SeedEquipesPdp(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EquipePDP>().HasData(
            new EquipePDP
            {
                Id = 1,
                Nome = "Equipe de Operação Nordeste",
                Descricao = "Responsável pela programação diária de produção da região Nordeste",
                Coordenador = "João Silva Santos",
                Email = "operacao.ne@ons.org.br",
                Telefone = "(81) 3421-5000",
                Ativo = true,
                DataCriacao = new DateTime(2024, 1, 1)
            },
            new EquipePDP
            {
                Id = 2,
                Nome = "Equipe de Operação Sudeste",
                Descricao = "Responsável pela programação diária de produção da região Sudeste/Centro-Oeste",
                Coordenador = "Maria Oliveira Costa",
                Email = "operacao.se@ons.org.br",
                Telefone = "(21) 3444-5500",
                Ativo = true,
                DataCriacao = new DateTime(2024, 1, 1)
            },
            new EquipePDP
            {
                Id = 3,
                Nome = "Equipe de Operação Sul",
                Descricao = "Responsável pela programação diária de produção da região Sul",
                Coordenador = "Carlos Eduardo Ferreira",
                Email = "operacao.sul@ons.org.br",
                Telefone = "(41) 3333-4400",
                Ativo = true,
                DataCriacao = new DateTime(2024, 1, 1)
            },
            new EquipePDP
            {
                Id = 4,
                Nome = "Equipe de Operação Norte",
                Descricao = "Responsável pela programação diária de produção da região Norte",
                Coordenador = "Ana Paula Rodrigues",
                Email = "operacao.norte@ons.org.br",
                Telefone = "(92) 3232-1100",
                Ativo = true,
                DataCriacao = new DateTime(2024, 1, 1)
            },
            new EquipePDP
            {
                Id = 5,
                Nome = "Equipe de Planejamento Energético",
                Descricao = "Responsável pelo planejamento de médio e longo prazo da operação",
                Coordenador = "Roberto Mendes Lima",
                Email = "planejamento@ons.org.br",
                Telefone = "(21) 3444-5600",
                Ativo = true,
                DataCriacao = new DateTime(2024, 1, 1)
            }
        );
    }
}
