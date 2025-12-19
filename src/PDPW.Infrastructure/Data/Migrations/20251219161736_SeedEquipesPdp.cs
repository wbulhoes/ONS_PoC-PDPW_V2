using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PDPW.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedEquipesPdp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EquipesPDP",
                columns: new[] { "Id", "Ativo", "Coordenador", "DataAtualizacao", "DataCriacao", "Descricao", "Email", "Nome", "Telefone" },
                values: new object[,]
                {
                    { 1, true, "João Silva Santos", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Responsável pela programação diária de produção da região Nordeste", "operacao.ne@ons.org.br", "Equipe de Operação Nordeste", "(81) 3421-5000" },
                    { 2, true, "Maria Oliveira Costa", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Responsável pela programação diária de produção da região Sudeste/Centro-Oeste", "operacao.se@ons.org.br", "Equipe de Operação Sudeste", "(21) 3444-5500" },
                    { 3, true, "Carlos Eduardo Ferreira", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Responsável pela programação diária de produção da região Sul", "operacao.sul@ons.org.br", "Equipe de Operação Sul", "(41) 3333-4400" },
                    { 4, true, "Ana Paula Rodrigues", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Responsável pela programação diária de produção da região Norte", "operacao.norte@ons.org.br", "Equipe de Operação Norte", "(92) 3232-1100" },
                    { 5, true, "Roberto Mendes Lima", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Responsável pelo planejamento de médio e longo prazo da operação", "planejamento@ons.org.br", "Equipe de Planejamento Energético", "(21) 3444-5600" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EquipesPDP",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EquipesPDP",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EquipesPDP",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EquipesPDP",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "EquipesPDP",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
