using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PDPW.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExpandirSemanasPMO_2025_2026 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SemanasPMO",
                columns: new[] { "Id", "Ano", "Ativo", "DataAtualizacao", "DataCriacao", "DataFim", "DataInicio", "Numero", "Observacoes" },
                values: new object[,]
                {
                    { 17, 2025, true, null, new DateTime(2025, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, null },
                    { 18, 2025, true, null, new DateTime(2025, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, null },
                    { 19, 2025, true, null, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, null },
                    { 20, 2025, true, null, new DateTime(2025, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, null },
                    { 21, 2025, true, null, new DateTime(2025, 4, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, null },
                    { 22, 2025, true, null, new DateTime(2025, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, null },
                    { 23, 2025, true, null, new DateTime(2025, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, null },
                    { 24, 2025, true, null, new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, null },
                    { 25, 2025, true, null, new DateTime(2025, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, null },
                    { 26, 2025, true, null, new DateTime(2025, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 22, null },
                    { 27, 2025, true, null, new DateTime(2025, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 23, null },
                    { 28, 2025, true, null, new DateTime(2025, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 24, null },
                    { 29, 2025, true, null, new DateTime(2025, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 25, null },
                    { 30, 2025, true, null, new DateTime(2025, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 26, null },
                    { 31, 2025, true, null, new DateTime(2025, 6, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 27, null },
                    { 32, 2025, true, null, new DateTime(2025, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 28, null },
                    { 33, 2025, true, null, new DateTime(2025, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 29, null },
                    { 34, 2025, true, null, new DateTime(2025, 7, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, null },
                    { 35, 2025, true, null, new DateTime(2025, 7, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 31, null },
                    { 36, 2025, true, null, new DateTime(2025, 8, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 32, null },
                    { 37, 2025, true, null, new DateTime(2025, 8, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 33, null },
                    { 38, 2025, true, null, new DateTime(2025, 8, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 34, null },
                    { 39, 2025, true, null, new DateTime(2025, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, null },
                    { 40, 2025, true, null, new DateTime(2025, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 36, null },
                    { 41, 2025, true, null, new DateTime(2025, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 37, null },
                    { 42, 2025, true, null, new DateTime(2025, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 38, null },
                    { 43, 2025, true, null, new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 39, null },
                    { 44, 2025, true, null, new DateTime(2025, 9, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, null },
                    { 45, 2025, true, null, new DateTime(2025, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 41, null },
                    { 46, 2025, true, null, new DateTime(2025, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 42, null },
                    { 47, 2025, true, null, new DateTime(2025, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 43, null },
                    { 48, 2025, true, null, new DateTime(2025, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 44, null },
                    { 49, 2025, true, null, new DateTime(2025, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, null },
                    { 50, 2025, true, null, new DateTime(2025, 11, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 46, null },
                    { 51, 2025, true, null, new DateTime(2025, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 47, null },
                    { 52, 2025, true, null, new DateTime(2025, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 48, null },
                    { 53, 2025, true, null, new DateTime(2025, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 49, null },
                    { 54, 2025, true, null, new DateTime(2025, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, null },
                    { 55, 2025, true, null, new DateTime(2025, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 51, null },
                    { 56, 2025, true, null, new DateTime(2025, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 52, null },
                    { 57, 2026, true, null, new DateTime(2025, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Primeira semana de 2026" },
                    { 58, 2026, true, null, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null },
                    { 59, 2026, true, null, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, null },
                    { 60, 2026, true, null, new DateTime(2026, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null },
                    { 61, 2026, true, null, new DateTime(2026, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, null },
                    { 62, 2026, true, null, new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null },
                    { 63, 2026, true, null, new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, null },
                    { 64, 2026, true, null, new DateTime(2026, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null },
                    { 65, 2026, true, null, new DateTime(2026, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, null },
                    { 66, 2026, true, null, new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 10, null },
                    { 67, 2026, true, null, new DateTime(2026, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, null },
                    { 68, 2026, true, null, new DateTime(2026, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 12, null },
                    { 69, 2026, true, null, new DateTime(2026, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 13, null },
                    { 70, 2026, true, null, new DateTime(2026, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 14, null },
                    { 71, 2026, true, null, new DateTime(2026, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 15, null },
                    { 72, 2026, true, null, new DateTime(2026, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, null },
                    { 73, 2026, true, null, new DateTime(2026, 4, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 17, null },
                    { 74, 2026, true, null, new DateTime(2026, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 18, null },
                    { 75, 2026, true, null, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 19, null },
                    { 76, 2026, true, null, new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 20, null },
                    { 77, 2026, true, null, new DateTime(2026, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 21, null },
                    { 78, 2026, true, null, new DateTime(2026, 5, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 22, null },
                    { 79, 2026, true, null, new DateTime(2026, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 23, null },
                    { 80, 2026, true, null, new DateTime(2026, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 24, null },
                    { 81, 2026, true, null, new DateTime(2026, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 25, null },
                    { 82, 2026, true, null, new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 26, null },
                    { 83, 2026, true, null, new DateTime(2026, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 27, null },
                    { 84, 2026, true, null, new DateTime(2026, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 28, null },
                    { 85, 2026, true, null, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 29, null },
                    { 86, 2026, true, null, new DateTime(2026, 7, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, null },
                    { 87, 2026, true, null, new DateTime(2026, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 31, null },
                    { 88, 2026, true, null, new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 32, null },
                    { 89, 2026, true, null, new DateTime(2026, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 33, null },
                    { 90, 2026, true, null, new DateTime(2026, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 34, null },
                    { 91, 2026, true, null, new DateTime(2026, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, null },
                    { 92, 2026, true, null, new DateTime(2026, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 36, null },
                    { 93, 2026, true, null, new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 37, null },
                    { 94, 2026, true, null, new DateTime(2026, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 38, null },
                    { 95, 2026, true, null, new DateTime(2026, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 39, null },
                    { 96, 2026, true, null, new DateTime(2026, 9, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, null },
                    { 97, 2026, true, null, new DateTime(2026, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 41, null },
                    { 98, 2026, true, null, new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 42, null },
                    { 99, 2026, true, null, new DateTime(2026, 10, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 43, null },
                    { 100, 2026, true, null, new DateTime(2026, 10, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 44, null },
                    { 101, 2026, true, null, new DateTime(2026, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, null },
                    { 102, 2026, true, null, new DateTime(2026, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 46, null },
                    { 103, 2026, true, null, new DateTime(2026, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 47, null },
                    { 104, 2026, true, null, new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 48, null },
                    { 105, 2026, true, null, new DateTime(2026, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 49, null },
                    { 106, 2026, true, null, new DateTime(2026, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 50, null },
                    { 107, 2026, true, null, new DateTime(2026, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 51, null },
                    { 108, 2026, true, null, new DateTime(2026, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 52, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "SemanasPMO",
                keyColumn: "Id",
                keyValue: 108);
        }
    }
}
