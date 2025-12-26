using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PDPW.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarRestricoesParadasArquivosDadger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ArquivosDadger",
                columns: new[] { "Id", "Ativo", "CaminhoArquivo", "DataAtualizacao", "DataCriacao", "DataImportacao", "DataProcessamento", "NomeArquivo", "Observacoes", "Processado", "SemanaPMOId" },
                values: new object[,]
                {
                    { 1, true, "/dados/2024/semana49/DADGER_2024_S49_REV0.DAT", null, new DateTime(2024, 11, 30, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 2, 0, 0, 0, DateTimeKind.Unspecified), "DADGER_2024_S49_REV0.DAT", "Revisão inicial (domingo)", true, 1 },
                    { 2, true, "/dados/2024/semana49/DADGER_2024_S49_REV1.DAT", null, new DateTime(2024, 12, 1, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 2, 2, 0, 0, 0, DateTimeKind.Unspecified), "DADGER_2024_S49_REV1.DAT", "Revisão 1 - atualização diária", true, 1 },
                    { 3, true, "/dados/2024/semana49/DADGER_2024_S49_REV2.DAT", null, new DateTime(2024, 12, 2, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 2, 0, 0, 0, DateTimeKind.Unspecified), "DADGER_2024_S49_REV2.DAT", "Revisão 2 - atualização diária", true, 1 },
                    { 4, true, "/dados/2024/semana49/DADGER_2024_S49_REV3.DAT", null, new DateTime(2024, 12, 3, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 2, 0, 0, 0, DateTimeKind.Unspecified), "DADGER_2024_S49_REV3.DAT", "Revisão 3 - atualização diária", true, 1 },
                    { 5, true, "/dados/2024/semana49/DADGER_2024_S49_REV4.DAT", null, new DateTime(2024, 12, 4, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 2, 0, 0, 0, DateTimeKind.Unspecified), "DADGER_2024_S49_REV4.DAT", "Revisão 4 - atualização diária", true, 1 },
                    { 6, true, "/dados/2024/semana50/DADGER_2024_S50_REV0.DAT", null, new DateTime(2024, 12, 7, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 2, 0, 0, 0, DateTimeKind.Unspecified), "DADGER_2024_S50_REV0.DAT", "Revisão inicial (domingo)", true, 2 },
                    { 7, true, "/dados/2024/semana50/DADGER_2024_S50_REV1.DAT", null, new DateTime(2024, 12, 8, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 2, 0, 0, 0, DateTimeKind.Unspecified), "DADGER_2024_S50_REV1.DAT", "Revisão 1 - atualização diária", true, 2 },
                    { 8, true, "/dados/2024/semana50/DADGER_2024_S50_REV2.DAT", null, new DateTime(2024, 12, 9, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 2, 0, 0, 0, DateTimeKind.Unspecified), "DADGER_2024_S50_REV2.DAT", "Revisão 2 - atualização diária", true, 2 },
                    { 9, true, "/dados/2024/semana50/DADGER_2024_S50_REV3.DAT", null, new DateTime(2024, 12, 10, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 2, 0, 0, 0, DateTimeKind.Unspecified), "DADGER_2024_S50_REV3.DAT", "Revisão 3 - atualização diária", true, 2 },
                    { 10, true, "/dados/2024/semana50/DADGER_2024_S50_REV4.DAT", null, new DateTime(2024, 12, 11, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 2, 0, 0, 0, DateTimeKind.Unspecified), "DADGER_2024_S50_REV4.DAT", "Revisão 4 - atualização diária", true, 2 },
                    { 11, true, "/dados/2024/semana51/DADGER_2024_S51_REV0.DAT", null, new DateTime(2024, 12, 14, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 2, 0, 0, 0, DateTimeKind.Unspecified), "DADGER_2024_S51_REV0.DAT", "Revisão inicial (domingo)", true, 3 },
                    { 12, true, "/dados/2024/semana51/DADGER_2024_S51_REV1.DAT", null, new DateTime(2024, 12, 15, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 2, 0, 0, 0, DateTimeKind.Unspecified), "DADGER_2024_S51_REV1.DAT", "Revisão 1 - atualização diária", true, 3 },
                    { 13, true, "/dados/2024/semana51/DADGER_2024_S51_REV2.DAT", null, new DateTime(2024, 12, 16, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 2, 0, 0, 0, DateTimeKind.Unspecified), "DADGER_2024_S51_REV2.DAT", "Revisão 2 - atualização diária", true, 3 },
                    { 14, true, "/dados/2024/semana51/DADGER_2024_S51_REV3.DAT", null, new DateTime(2024, 12, 17, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 2, 0, 0, 0, DateTimeKind.Unspecified), "DADGER_2024_S51_REV3.DAT", "Revisão 3 - atualização diária", true, 3 },
                    { 15, true, "/dados/2024/semana51/DADGER_2024_S51_REV4.DAT", null, new DateTime(2024, 12, 18, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "DADGER_2024_S51_REV4.DAT", "Revisão 4 - atualização diária", false, 3 },
                    { 16, true, "/dados/2024/semana52/DADGER_2024_S52_REV0.DAT", null, new DateTime(2024, 12, 21, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "DADGER_2024_S52_REV0.DAT", "Revisão inicial (domingo)", false, 4 },
                    { 17, true, "/dados/2024/semana52/DADGER_2024_S52_REV1.DAT", null, new DateTime(2024, 12, 22, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "DADGER_2024_S52_REV1.DAT", "Revisão 1 - atualização diária", false, 4 },
                    { 18, true, "/dados/2024/semana52/DADGER_2024_S52_REV2.DAT", null, new DateTime(2024, 12, 23, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "DADGER_2024_S52_REV2.DAT", "Revisão 2 - atualização diária", false, 4 },
                    { 19, true, "/dados/2024/semana52/DADGER_2024_S52_REV3.DAT", null, new DateTime(2024, 12, 24, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "DADGER_2024_S52_REV3.DAT", "Revisão 3 - atualização diária", false, 4 },
                    { 20, true, "/dados/2024/semana52/DADGER_2024_S52_REV4.DAT", null, new DateTime(2024, 12, 25, 23, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "DADGER_2024_S52_REV4.DAT", "Revisão 4 - atualização diária", false, 4 }
                });

            migrationBuilder.InsertData(
                table: "ParadasUG",
                columns: new[] { "Id", "Ativo", "DataAtualizacao", "DataCriacao", "DataFim", "DataInicio", "MotivoParada", "Observacoes", "Programada", "UnidadeGeradoraId" },
                values: new object[,]
                {
                    { 1, true, null, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Preventiva Anual", "Parada programada para revisão geral - UG 1", true, 1 },
                    { 2, true, null, new DateTime(2024, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Corretiva Programada", "Parada programada para revisão geral - UG 2", true, 2 },
                    { 3, true, null, new DateTime(2024, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Corretiva Programada", "Parada programada para revisão geral - UG 3", true, 3 },
                    { 4, true, null, new DateTime(2024, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Preventiva Anual", "Parada programada para revisão geral - UG 4", true, 4 },
                    { 5, true, null, new DateTime(2024, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Corretiva Programada", "Parada programada para revisão geral - UG 5", true, 5 },
                    { 6, true, null, new DateTime(2024, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Corretiva Programada", "Parada programada para revisão geral - UG 6", true, 6 },
                    { 7, true, null, new DateTime(2024, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Preventiva Anual", "Parada programada para revisão geral - UG 7", true, 7 },
                    { 8, true, null, new DateTime(2024, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Corretiva Programada", "Parada programada para revisão geral - UG 8", true, 8 },
                    { 9, true, null, new DateTime(2024, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Corretiva Programada", "Parada programada para revisão geral - UG 9", true, 9 },
                    { 10, true, null, new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Preventiva Anual", "Parada programada para revisão geral - UG 10", true, 10 },
                    { 11, true, null, new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Corretiva Programada", "Parada programada para revisão geral - UG 11", true, 11 },
                    { 12, true, null, new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Corretiva Programada", "Parada programada para revisão geral - UG 12", true, 12 },
                    { 13, true, null, new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Preventiva Anual", "Parada programada para revisão geral - UG 13", true, 13 },
                    { 14, true, null, new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Corretiva Programada", "Parada programada para revisão geral - UG 14", true, 14 },
                    { 15, true, null, new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Corretiva Programada", "Parada programada para revisão geral - UG 15", true, 15 },
                    { 16, true, null, new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Preventiva Anual", "Parada programada para revisão geral - UG 16", true, 16 },
                    { 17, true, null, new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Corretiva Programada", "Parada programada para revisão geral - UG 17", true, 17 },
                    { 18, true, null, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Corretiva Programada", "Parada programada para revisão geral - UG 18", true, 18 },
                    { 19, true, null, new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Preventiva Anual", "Parada programada para revisão geral - UG 19", true, 19 },
                    { 20, true, null, new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manutenção Corretiva Programada", "Parada programada para revisão geral - UG 20", true, 20 },
                    { 21, true, null, new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Falha no Sistema de Excitação", "Parada emergencial por falha - UG 10", false, 10 },
                    { 22, true, null, new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Problema no Gerador", "Parada emergencial por falha - UG 19", false, 19 },
                    { 23, true, null, new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Falha no Sistema de Excitação", "Parada emergencial por falha - UG 28", false, 28 },
                    { 24, true, null, new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Problema no Gerador", "Parada emergencial por falha - UG 37", false, 37 },
                    { 25, true, null, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Falha no Sistema de Excitação", "Parada emergencial por falha - UG 46", false, 46 },
                    { 26, true, null, new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "Problema no Gerador", "Parada emergencial por falha - UG 55", false, 55 },
                    { 27, true, null, new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Falha no Sistema de Excitação", "Parada emergencial por falha - UG 64", false, 64 },
                    { 28, true, null, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Problema no Gerador", "Parada emergencial por falha - UG 73", false, 73 },
                    { 29, true, null, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Falha no Sistema de Excitação", "Parada emergencial por falha - UG 82", false, 82 },
                    { 30, true, null, new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Problema no Gerador", "Parada emergencial por falha - UG 91", false, 91 }
                });

            migrationBuilder.InsertData(
                table: "RestricoesUG",
                columns: new[] { "Id", "Ativo", "DataAtualizacao", "DataCriacao", "DataFim", "DataInicio", "MotivoRestricaoId", "Observacoes", "PotenciaRestrita", "UnidadeGeradoraId" },
                values: new object[,]
                {
                    { 1, true, null, new DateTime(2024, 11, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 1", 100m, 1 },
                    { 2, true, null, new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 2", 110m, 2 },
                    { 3, true, null, new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 3", 120m, 3 },
                    { 4, true, null, new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 4", 130m, 4 },
                    { 5, true, null, new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 5", 140m, 5 },
                    { 6, true, null, new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 6", 150m, 6 },
                    { 7, true, null, new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 7", 160m, 7 },
                    { 8, true, null, new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 8", 170m, 8 },
                    { 9, true, null, new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 9", 180m, 9 },
                    { 10, true, null, new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 10", 190m, 10 },
                    { 11, true, null, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 11", 200m, 11 },
                    { 12, true, null, new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 12", 210m, 12 },
                    { 13, true, null, new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 13", 220m, 13 },
                    { 14, true, null, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 14", 230m, 14 },
                    { 15, true, null, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 15", 240m, 15 },
                    { 16, true, null, new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 16", 250m, 16 },
                    { 17, true, null, new DateTime(2024, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 17", 260m, 17 },
                    { 18, true, null, new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 18", 270m, 18 },
                    { 19, true, null, new DateTime(2024, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 19", 280m, 19 },
                    { 20, true, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Manutenção preventiva programada da UG 20", 290m, 20 },
                    { 21, true, null, new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Vazão reduzida no reservatório - UG 1", 50m, 1 },
                    { 22, true, null, new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Vazão reduzida no reservatório - UG 2", 55m, 2 },
                    { 23, true, null, new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Vazão reduzida no reservatório - UG 3", 60m, 3 },
                    { 24, true, null, new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Vazão reduzida no reservatório - UG 4", 65m, 4 },
                    { 25, true, null, new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Vazão reduzida no reservatório - UG 5", 70m, 5 },
                    { 26, true, null, new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Vazão reduzida no reservatório - UG 6", 75m, 6 },
                    { 27, true, null, new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Vazão reduzida no reservatório - UG 7", 80m, 7 },
                    { 28, true, null, new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Vazão reduzida no reservatório - UG 8", 85m, 8 },
                    { 29, true, null, new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Vazão reduzida no reservatório - UG 9", 90m, 9 },
                    { 30, true, null, new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(2024, 12, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Vazão reduzida no reservatório - UG 10", 95m, 10 },
                    { 31, true, null, new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Problema no sistema de refrigeração - UG 20", 200m, 20 },
                    { 32, true, null, new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Problema no sistema de refrigeração - UG 28", 220m, 28 },
                    { 33, true, null, new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Problema no sistema de refrigeração - UG 36", 240m, 36 },
                    { 34, true, null, new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Problema no sistema de refrigeração - UG 44", 260m, 44 },
                    { 35, true, null, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Problema no sistema de refrigeração - UG 52", 280m, 52 },
                    { 36, true, null, new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Problema no sistema de refrigeração - UG 60", 300m, 60 },
                    { 37, true, null, new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Problema no sistema de refrigeração - UG 68", 320m, 68 },
                    { 38, true, null, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Problema no sistema de refrigeração - UG 76", 340m, 76 },
                    { 39, true, null, new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Problema no sistema de refrigeração - UG 84", 360m, 84 },
                    { 40, true, null, new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Problema no sistema de refrigeração - UG 92", 380m, 92 },
                    { 41, true, null, new DateTime(2024, 12, 10, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Limitação na linha de transmissão - UG 30", 150m, 30 },
                    { 42, true, null, new DateTime(2024, 12, 11, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Limitação na linha de transmissão - UG 45", 160m, 45 },
                    { 43, true, null, new DateTime(2024, 12, 12, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Limitação na linha de transmissão - UG 60", 170m, 60 },
                    { 44, true, null, new DateTime(2024, 12, 13, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Limitação na linha de transmissão - UG 75", 180m, 75 },
                    { 45, true, null, new DateTime(2024, 12, 14, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Limitação na linha de transmissão - UG 90", 190m, 90 },
                    { 46, true, null, new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Teste de sistema de proteção - UG 50", 50m, 50 },
                    { 47, true, null, new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Teste de sistema de proteção - UG 60", 50m, 60 },
                    { 48, true, null, new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Teste de sistema de proteção - UG 70", 50m, 70 },
                    { 49, true, null, new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Teste de sistema de proteção - UG 80", 50m, 80 },
                    { 50, true, null, new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, "Teste de sistema de proteção - UG 90", 50m, 90 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "ParadasUG",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "RestricoesUG",
                keyColumn: "Id",
                keyValue: 50);
        }
    }
}
