using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PDPW.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExpandirSeederDadosCompletos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Balancos",
                columns: new[] { "Id", "Ativo", "Carga", "DataAtualizacao", "DataCriacao", "DataReferencia", "Deficit", "Geracao", "Intercambio", "Observacoes", "Perdas", "SubsistemaId" },
                values: new object[,]
                {
                    { 1, true, 45000m, null, new DateTime(2024, 11, 25, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 46000m, 170m, null, 800m, "SE" },
                    { 2, true, 12000m, null, new DateTime(2024, 11, 25, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 11500m, -150m, null, 350m, "S" },
                    { 3, true, 14000m, null, new DateTime(2024, 11, 25, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 13800m, -230m, null, 400m, "NE" },
                    { 4, true, 8000m, null, new DateTime(2024, 11, 25, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8200m, -140m, null, 250m, "N" },
                    { 5, true, 45500m, null, new DateTime(2024, 11, 26, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 46500m, 170m, null, 800m, "SE" },
                    { 6, true, 12200m, null, new DateTime(2024, 11, 26, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 11700m, -150m, null, 350m, "S" },
                    { 7, true, 14250m, null, new DateTime(2024, 11, 26, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 14050m, -230m, null, 400m, "NE" },
                    { 8, true, 8150m, null, new DateTime(2024, 11, 26, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8350m, -140m, null, 250m, "N" },
                    { 9, true, 46000m, null, new DateTime(2024, 11, 27, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 47000m, 170m, null, 800m, "SE" },
                    { 10, true, 12400m, null, new DateTime(2024, 11, 27, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 11900m, -150m, null, 350m, "S" },
                    { 11, true, 14500m, null, new DateTime(2024, 11, 27, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 14300m, -230m, null, 400m, "NE" },
                    { 12, true, 8300m, null, new DateTime(2024, 11, 27, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8500m, -140m, null, 250m, "N" },
                    { 13, true, 46500m, null, new DateTime(2024, 11, 28, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 47500m, 170m, null, 800m, "SE" },
                    { 14, true, 12600m, null, new DateTime(2024, 11, 28, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 12100m, -150m, null, 350m, "S" },
                    { 15, true, 14750m, null, new DateTime(2024, 11, 28, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 14550m, -230m, null, 400m, "NE" },
                    { 16, true, 8450m, null, new DateTime(2024, 11, 28, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8650m, -140m, null, 250m, "N" },
                    { 17, true, 47000m, null, new DateTime(2024, 11, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 48000m, 170m, null, 800m, "SE" },
                    { 18, true, 12800m, null, new DateTime(2024, 11, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 12300m, -150m, null, 350m, "S" },
                    { 19, true, 15000m, null, new DateTime(2024, 11, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 14800m, -230m, null, 400m, "NE" },
                    { 20, true, 8600m, null, new DateTime(2024, 11, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8800m, -140m, null, 250m, "N" },
                    { 21, true, 47500m, null, new DateTime(2024, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 48500m, 170m, null, 800m, "SE" },
                    { 22, true, 13000m, null, new DateTime(2024, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 12500m, -150m, null, 350m, "S" },
                    { 23, true, 15250m, null, new DateTime(2024, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 15050m, -230m, null, 400m, "NE" },
                    { 24, true, 8750m, null, new DateTime(2024, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8950m, -140m, null, 250m, "N" },
                    { 25, true, 48000m, null, new DateTime(2024, 12, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 49000m, 170m, null, 800m, "SE" },
                    { 26, true, 13200m, null, new DateTime(2024, 12, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 12700m, -150m, null, 350m, "S" },
                    { 27, true, 15500m, null, new DateTime(2024, 12, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 15300m, -230m, null, 400m, "NE" },
                    { 28, true, 8900m, null, new DateTime(2024, 12, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 9100m, -140m, null, 250m, "N" },
                    { 29, true, 45000m, null, new DateTime(2024, 12, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 46000m, 170m, null, 800m, "SE" },
                    { 30, true, 12000m, null, new DateTime(2024, 12, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 11500m, -150m, null, 350m, "S" },
                    { 31, true, 14000m, null, new DateTime(2024, 12, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 13800m, -230m, null, 400m, "NE" },
                    { 32, true, 8000m, null, new DateTime(2024, 12, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8200m, -140m, null, 250m, "N" },
                    { 33, true, 45500m, null, new DateTime(2024, 12, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 46500m, 170m, null, 800m, "SE" },
                    { 34, true, 12200m, null, new DateTime(2024, 12, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 11700m, -150m, null, 350m, "S" },
                    { 35, true, 14250m, null, new DateTime(2024, 12, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 14050m, -230m, null, 400m, "NE" },
                    { 36, true, 8150m, null, new DateTime(2024, 12, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8350m, -140m, null, 250m, "N" },
                    { 37, true, 46000m, null, new DateTime(2024, 12, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 47000m, 170m, null, 800m, "SE" },
                    { 38, true, 12400m, null, new DateTime(2024, 12, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 11900m, -150m, null, 350m, "S" },
                    { 39, true, 14500m, null, new DateTime(2024, 12, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 14300m, -230m, null, 400m, "NE" },
                    { 40, true, 8300m, null, new DateTime(2024, 12, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8500m, -140m, null, 250m, "N" },
                    { 41, true, 46500m, null, new DateTime(2024, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 47500m, 170m, null, 800m, "SE" },
                    { 42, true, 12600m, null, new DateTime(2024, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 12100m, -150m, null, 350m, "S" },
                    { 43, true, 14750m, null, new DateTime(2024, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 14550m, -230m, null, 400m, "NE" },
                    { 44, true, 8450m, null, new DateTime(2024, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8650m, -140m, null, 250m, "N" },
                    { 45, true, 47000m, null, new DateTime(2024, 12, 6, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 48000m, 170m, null, 800m, "SE" },
                    { 46, true, 12800m, null, new DateTime(2024, 12, 6, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 12300m, -150m, null, 350m, "S" },
                    { 47, true, 15000m, null, new DateTime(2024, 12, 6, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 14800m, -230m, null, 400m, "NE" },
                    { 48, true, 8600m, null, new DateTime(2024, 12, 6, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8800m, -140m, null, 250m, "N" },
                    { 49, true, 47500m, null, new DateTime(2024, 12, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 48500m, 170m, null, 800m, "SE" },
                    { 50, true, 13000m, null, new DateTime(2024, 12, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 12500m, -150m, null, 350m, "S" },
                    { 51, true, 15250m, null, new DateTime(2024, 12, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 15050m, -230m, null, 400m, "NE" },
                    { 52, true, 8750m, null, new DateTime(2024, 12, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8950m, -140m, null, 250m, "N" },
                    { 53, true, 48000m, null, new DateTime(2024, 12, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 49000m, 170m, null, 800m, "SE" },
                    { 54, true, 13200m, null, new DateTime(2024, 12, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 12700m, -150m, null, 350m, "S" },
                    { 55, true, 15500m, null, new DateTime(2024, 12, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 15300m, -230m, null, 400m, "NE" },
                    { 56, true, 8900m, null, new DateTime(2024, 12, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 9100m, -140m, null, 250m, "N" },
                    { 57, true, 45000m, null, new DateTime(2024, 12, 9, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 46000m, 170m, null, 800m, "SE" },
                    { 58, true, 12000m, null, new DateTime(2024, 12, 9, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 11500m, -150m, null, 350m, "S" },
                    { 59, true, 14000m, null, new DateTime(2024, 12, 9, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 13800m, -230m, null, 400m, "NE" },
                    { 60, true, 8000m, null, new DateTime(2024, 12, 9, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8200m, -140m, null, 250m, "N" },
                    { 61, true, 45500m, null, new DateTime(2024, 12, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 46500m, 170m, null, 800m, "SE" },
                    { 62, true, 12200m, null, new DateTime(2024, 12, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 11700m, -150m, null, 350m, "S" },
                    { 63, true, 14250m, null, new DateTime(2024, 12, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 14050m, -230m, null, 400m, "NE" },
                    { 64, true, 8150m, null, new DateTime(2024, 12, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8350m, -140m, null, 250m, "N" },
                    { 65, true, 46000m, null, new DateTime(2024, 12, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 47000m, 170m, null, 800m, "SE" },
                    { 66, true, 12400m, null, new DateTime(2024, 12, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 11900m, -150m, null, 350m, "S" },
                    { 67, true, 14500m, null, new DateTime(2024, 12, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 14300m, -230m, null, 400m, "NE" },
                    { 68, true, 8300m, null, new DateTime(2024, 12, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8500m, -140m, null, 250m, "N" },
                    { 69, true, 46500m, null, new DateTime(2024, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 47500m, 170m, null, 800m, "SE" },
                    { 70, true, 12600m, null, new DateTime(2024, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 12100m, -150m, null, 350m, "S" },
                    { 71, true, 14750m, null, new DateTime(2024, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 14550m, -230m, null, 400m, "NE" },
                    { 72, true, 8450m, null, new DateTime(2024, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8650m, -140m, null, 250m, "N" },
                    { 73, true, 47000m, null, new DateTime(2024, 12, 13, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 48000m, 170m, null, 800m, "SE" },
                    { 74, true, 12800m, null, new DateTime(2024, 12, 13, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 12300m, -150m, null, 350m, "S" },
                    { 75, true, 15000m, null, new DateTime(2024, 12, 13, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 14800m, -230m, null, 400m, "NE" },
                    { 76, true, 8600m, null, new DateTime(2024, 12, 13, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8800m, -140m, null, 250m, "N" },
                    { 77, true, 47500m, null, new DateTime(2024, 12, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 48500m, 170m, null, 800m, "SE" },
                    { 78, true, 13000m, null, new DateTime(2024, 12, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 12500m, -150m, null, 350m, "S" },
                    { 79, true, 15250m, null, new DateTime(2024, 12, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 15050m, -230m, null, 400m, "NE" },
                    { 80, true, 8750m, null, new DateTime(2024, 12, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8950m, -140m, null, 250m, "N" },
                    { 81, true, 48000m, null, new DateTime(2024, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 49000m, 170m, null, 800m, "SE" },
                    { 82, true, 13200m, null, new DateTime(2024, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 12700m, -150m, null, 350m, "S" },
                    { 83, true, 15500m, null, new DateTime(2024, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 15300m, -230m, null, 400m, "NE" },
                    { 84, true, 8900m, null, new DateTime(2024, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 9100m, -140m, null, 250m, "N" },
                    { 85, true, 45000m, null, new DateTime(2024, 12, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 46000m, 170m, null, 800m, "SE" },
                    { 86, true, 12000m, null, new DateTime(2024, 12, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 11500m, -150m, null, 350m, "S" },
                    { 87, true, 14000m, null, new DateTime(2024, 12, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 13800m, -230m, null, 400m, "NE" },
                    { 88, true, 8000m, null, new DateTime(2024, 12, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8200m, -140m, null, 250m, "N" },
                    { 89, true, 45500m, null, new DateTime(2024, 12, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 46500m, 170m, null, 800m, "SE" },
                    { 90, true, 12200m, null, new DateTime(2024, 12, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 11700m, -150m, null, 350m, "S" },
                    { 91, true, 14250m, null, new DateTime(2024, 12, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 14050m, -230m, null, 400m, "NE" },
                    { 92, true, 8150m, null, new DateTime(2024, 12, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8350m, -140m, null, 250m, "N" },
                    { 93, true, 46000m, null, new DateTime(2024, 12, 18, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 47000m, 170m, null, 800m, "SE" },
                    { 94, true, 12400m, null, new DateTime(2024, 12, 18, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 11900m, -150m, null, 350m, "S" },
                    { 95, true, 14500m, null, new DateTime(2024, 12, 18, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 14300m, -230m, null, 400m, "NE" },
                    { 96, true, 8300m, null, new DateTime(2024, 12, 18, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8500m, -140m, null, 250m, "N" },
                    { 97, true, 46500m, null, new DateTime(2024, 12, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 47500m, 170m, null, 800m, "SE" },
                    { 98, true, 12600m, null, new DateTime(2024, 12, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 12100m, -150m, null, 350m, "S" },
                    { 99, true, 14750m, null, new DateTime(2024, 12, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 14550m, -230m, null, 400m, "NE" },
                    { 100, true, 8450m, null, new DateTime(2024, 12, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8650m, -140m, null, 250m, "N" },
                    { 101, true, 47000m, null, new DateTime(2024, 12, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 48000m, 170m, null, 800m, "SE" },
                    { 102, true, 12800m, null, new DateTime(2024, 12, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 12300m, -150m, null, 350m, "S" },
                    { 103, true, 15000m, null, new DateTime(2024, 12, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 14800m, -230m, null, 400m, "NE" },
                    { 104, true, 8600m, null, new DateTime(2024, 12, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8800m, -140m, null, 250m, "N" },
                    { 105, true, 47500m, null, new DateTime(2024, 12, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 48500m, 170m, null, 800m, "SE" },
                    { 106, true, 13000m, null, new DateTime(2024, 12, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 12500m, -150m, null, 350m, "S" },
                    { 107, true, 15250m, null, new DateTime(2024, 12, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 15050m, -230m, null, 400m, "NE" },
                    { 108, true, 8750m, null, new DateTime(2024, 12, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8950m, -140m, null, 250m, "N" },
                    { 109, true, 48000m, null, new DateTime(2024, 12, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 49000m, 170m, null, 800m, "SE" },
                    { 110, true, 13200m, null, new DateTime(2024, 12, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 12700m, -150m, null, 350m, "S" },
                    { 111, true, 15500m, null, new DateTime(2024, 12, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 15300m, -230m, null, 400m, "NE" },
                    { 112, true, 8900m, null, new DateTime(2024, 12, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 9100m, -140m, null, 250m, "N" },
                    { 113, true, 45000m, null, new DateTime(2024, 12, 23, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 46000m, 170m, null, 800m, "SE" },
                    { 114, true, 12000m, null, new DateTime(2024, 12, 23, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 11500m, -150m, null, 350m, "S" },
                    { 115, true, 14000m, null, new DateTime(2024, 12, 23, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 13800m, -230m, null, 400m, "NE" },
                    { 116, true, 8000m, null, new DateTime(2024, 12, 23, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8200m, -140m, null, 250m, "N" },
                    { 117, true, 45500m, null, new DateTime(2024, 12, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 46500m, 170m, null, 800m, "SE" },
                    { 118, true, 12200m, null, new DateTime(2024, 12, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 11700m, -150m, null, 350m, "S" },
                    { 119, true, 14250m, null, new DateTime(2024, 12, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 14050m, -230m, null, 400m, "NE" },
                    { 120, true, 8150m, null, new DateTime(2024, 12, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 8350m, -140m, null, 250m, "N" }
                });

            migrationBuilder.InsertData(
                table: "Cargas",
                columns: new[] { "Id", "Ativo", "CargaMWmed", "CargaVerificada", "DataAtualizacao", "DataCriacao", "DataReferencia", "Observacoes", "PrevisaoCarga", "SubsistemaId" },
                values: new object[,]
                {
                    { 1, true, 45000m, 44800m, null, new DateTime(2024, 11, 25, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 45200m, "SE" },
                    { 2, true, 12000m, 11900m, null, new DateTime(2024, 11, 25, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12100m, "S" },
                    { 3, true, 14000m, 13850m, null, new DateTime(2024, 11, 25, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14150m, "NE" },
                    { 4, true, 8000m, 7900m, null, new DateTime(2024, 11, 25, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8100m, "N" },
                    { 5, true, 45500m, 45280m, null, new DateTime(2024, 11, 26, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 45720m, "SE" },
                    { 6, true, 12200m, 12090m, null, new DateTime(2024, 11, 26, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12310m, "S" },
                    { 7, true, 14250m, 14090m, null, new DateTime(2024, 11, 26, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14410m, "NE" },
                    { 8, true, 8150m, 8045m, null, new DateTime(2024, 11, 26, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8255m, "N" },
                    { 9, true, 46000m, 45760m, null, new DateTime(2024, 11, 27, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 46240m, "SE" },
                    { 10, true, 12400m, 12280m, null, new DateTime(2024, 11, 27, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12520m, "S" },
                    { 11, true, 14500m, 14330m, null, new DateTime(2024, 11, 27, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14670m, "NE" },
                    { 12, true, 8300m, 8190m, null, new DateTime(2024, 11, 27, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8410m, "N" },
                    { 13, true, 46500m, 46240m, null, new DateTime(2024, 11, 28, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 46760m, "SE" },
                    { 14, true, 12600m, 12470m, null, new DateTime(2024, 11, 28, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12730m, "S" },
                    { 15, true, 14750m, 14570m, null, new DateTime(2024, 11, 28, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14930m, "NE" },
                    { 16, true, 8450m, 8335m, null, new DateTime(2024, 11, 28, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8565m, "N" },
                    { 17, true, 47000m, 46720m, null, new DateTime(2024, 11, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 47280m, "SE" },
                    { 18, true, 12800m, 12660m, null, new DateTime(2024, 11, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12940m, "S" },
                    { 19, true, 15000m, 14810m, null, new DateTime(2024, 11, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 15190m, "NE" },
                    { 20, true, 8600m, 8480m, null, new DateTime(2024, 11, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8720m, "N" },
                    { 21, true, 47500m, 47200m, null, new DateTime(2024, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 47800m, "SE" },
                    { 22, true, 13000m, 12850m, null, new DateTime(2024, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 13150m, "S" },
                    { 23, true, 15250m, 15050m, null, new DateTime(2024, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 15450m, "NE" },
                    { 24, true, 8750m, 8625m, null, new DateTime(2024, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8875m, "N" },
                    { 25, true, 48000m, 47680m, null, new DateTime(2024, 12, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 48320m, "SE" },
                    { 26, true, 13200m, 13040m, null, new DateTime(2024, 12, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 13360m, "S" },
                    { 27, true, 15500m, 15290m, null, new DateTime(2024, 12, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 15710m, "NE" },
                    { 28, true, 8900m, 8770m, null, new DateTime(2024, 12, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 9030m, "N" },
                    { 29, true, 45000m, 44800m, null, new DateTime(2024, 12, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 45200m, "SE" },
                    { 30, true, 12000m, 11900m, null, new DateTime(2024, 12, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12100m, "S" },
                    { 31, true, 14000m, 13850m, null, new DateTime(2024, 12, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14150m, "NE" },
                    { 32, true, 8000m, 7900m, null, new DateTime(2024, 12, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8100m, "N" },
                    { 33, true, 45500m, 45280m, null, new DateTime(2024, 12, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 45720m, "SE" },
                    { 34, true, 12200m, 12090m, null, new DateTime(2024, 12, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12310m, "S" },
                    { 35, true, 14250m, 14090m, null, new DateTime(2024, 12, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14410m, "NE" },
                    { 36, true, 8150m, 8045m, null, new DateTime(2024, 12, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8255m, "N" },
                    { 37, true, 46000m, 45760m, null, new DateTime(2024, 12, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 46240m, "SE" },
                    { 38, true, 12400m, 12280m, null, new DateTime(2024, 12, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12520m, "S" },
                    { 39, true, 14500m, 14330m, null, new DateTime(2024, 12, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14670m, "NE" },
                    { 40, true, 8300m, 8190m, null, new DateTime(2024, 12, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8410m, "N" },
                    { 41, true, 46500m, 46240m, null, new DateTime(2024, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 46760m, "SE" },
                    { 42, true, 12600m, 12470m, null, new DateTime(2024, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12730m, "S" },
                    { 43, true, 14750m, 14570m, null, new DateTime(2024, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14930m, "NE" },
                    { 44, true, 8450m, 8335m, null, new DateTime(2024, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8565m, "N" },
                    { 45, true, 47000m, 46720m, null, new DateTime(2024, 12, 6, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 47280m, "SE" },
                    { 46, true, 12800m, 12660m, null, new DateTime(2024, 12, 6, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12940m, "S" },
                    { 47, true, 15000m, 14810m, null, new DateTime(2024, 12, 6, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 15190m, "NE" },
                    { 48, true, 8600m, 8480m, null, new DateTime(2024, 12, 6, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8720m, "N" },
                    { 49, true, 47500m, 47200m, null, new DateTime(2024, 12, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 47800m, "SE" },
                    { 50, true, 13000m, 12850m, null, new DateTime(2024, 12, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 13150m, "S" },
                    { 51, true, 15250m, 15050m, null, new DateTime(2024, 12, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 15450m, "NE" },
                    { 52, true, 8750m, 8625m, null, new DateTime(2024, 12, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8875m, "N" },
                    { 53, true, 48000m, 47680m, null, new DateTime(2024, 12, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 48320m, "SE" },
                    { 54, true, 13200m, 13040m, null, new DateTime(2024, 12, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 13360m, "S" },
                    { 55, true, 15500m, 15290m, null, new DateTime(2024, 12, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 15710m, "NE" },
                    { 56, true, 8900m, 8770m, null, new DateTime(2024, 12, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 9030m, "N" },
                    { 57, true, 45000m, 44800m, null, new DateTime(2024, 12, 9, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 45200m, "SE" },
                    { 58, true, 12000m, 11900m, null, new DateTime(2024, 12, 9, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12100m, "S" },
                    { 59, true, 14000m, 13850m, null, new DateTime(2024, 12, 9, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14150m, "NE" },
                    { 60, true, 8000m, 7900m, null, new DateTime(2024, 12, 9, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8100m, "N" },
                    { 61, true, 45500m, 45280m, null, new DateTime(2024, 12, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 45720m, "SE" },
                    { 62, true, 12200m, 12090m, null, new DateTime(2024, 12, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12310m, "S" },
                    { 63, true, 14250m, 14090m, null, new DateTime(2024, 12, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14410m, "NE" },
                    { 64, true, 8150m, 8045m, null, new DateTime(2024, 12, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8255m, "N" },
                    { 65, true, 46000m, 45760m, null, new DateTime(2024, 12, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 46240m, "SE" },
                    { 66, true, 12400m, 12280m, null, new DateTime(2024, 12, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12520m, "S" },
                    { 67, true, 14500m, 14330m, null, new DateTime(2024, 12, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14670m, "NE" },
                    { 68, true, 8300m, 8190m, null, new DateTime(2024, 12, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8410m, "N" },
                    { 69, true, 46500m, 46240m, null, new DateTime(2024, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 46760m, "SE" },
                    { 70, true, 12600m, 12470m, null, new DateTime(2024, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12730m, "S" },
                    { 71, true, 14750m, 14570m, null, new DateTime(2024, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14930m, "NE" },
                    { 72, true, 8450m, 8335m, null, new DateTime(2024, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8565m, "N" },
                    { 73, true, 47000m, 46720m, null, new DateTime(2024, 12, 13, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 47280m, "SE" },
                    { 74, true, 12800m, 12660m, null, new DateTime(2024, 12, 13, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12940m, "S" },
                    { 75, true, 15000m, 14810m, null, new DateTime(2024, 12, 13, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 15190m, "NE" },
                    { 76, true, 8600m, 8480m, null, new DateTime(2024, 12, 13, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8720m, "N" },
                    { 77, true, 47500m, 47200m, null, new DateTime(2024, 12, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 47800m, "SE" },
                    { 78, true, 13000m, 12850m, null, new DateTime(2024, 12, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 13150m, "S" },
                    { 79, true, 15250m, 15050m, null, new DateTime(2024, 12, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 15450m, "NE" },
                    { 80, true, 8750m, 8625m, null, new DateTime(2024, 12, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8875m, "N" },
                    { 81, true, 48000m, 47680m, null, new DateTime(2024, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 48320m, "SE" },
                    { 82, true, 13200m, 13040m, null, new DateTime(2024, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 13360m, "S" },
                    { 83, true, 15500m, 15290m, null, new DateTime(2024, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 15710m, "NE" },
                    { 84, true, 8900m, 8770m, null, new DateTime(2024, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 9030m, "N" },
                    { 85, true, 45000m, 44800m, null, new DateTime(2024, 12, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 45200m, "SE" },
                    { 86, true, 12000m, 11900m, null, new DateTime(2024, 12, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12100m, "S" },
                    { 87, true, 14000m, 13850m, null, new DateTime(2024, 12, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14150m, "NE" },
                    { 88, true, 8000m, 7900m, null, new DateTime(2024, 12, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8100m, "N" },
                    { 89, true, 45500m, 45280m, null, new DateTime(2024, 12, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 45720m, "SE" },
                    { 90, true, 12200m, 12090m, null, new DateTime(2024, 12, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12310m, "S" },
                    { 91, true, 14250m, 14090m, null, new DateTime(2024, 12, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14410m, "NE" },
                    { 92, true, 8150m, 8045m, null, new DateTime(2024, 12, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8255m, "N" },
                    { 93, true, 46000m, 45760m, null, new DateTime(2024, 12, 18, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 46240m, "SE" },
                    { 94, true, 12400m, 12280m, null, new DateTime(2024, 12, 18, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12520m, "S" },
                    { 95, true, 14500m, 14330m, null, new DateTime(2024, 12, 18, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14670m, "NE" },
                    { 96, true, 8300m, 8190m, null, new DateTime(2024, 12, 18, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8410m, "N" },
                    { 97, true, 46500m, 46240m, null, new DateTime(2024, 12, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 46760m, "SE" },
                    { 98, true, 12600m, 12470m, null, new DateTime(2024, 12, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12730m, "S" },
                    { 99, true, 14750m, 14570m, null, new DateTime(2024, 12, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14930m, "NE" },
                    { 100, true, 8450m, 8335m, null, new DateTime(2024, 12, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8565m, "N" },
                    { 101, true, 47000m, 46720m, null, new DateTime(2024, 12, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 47280m, "SE" },
                    { 102, true, 12800m, 12660m, null, new DateTime(2024, 12, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12940m, "S" },
                    { 103, true, 15000m, 14810m, null, new DateTime(2024, 12, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 15190m, "NE" },
                    { 104, true, 8600m, 8480m, null, new DateTime(2024, 12, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8720m, "N" },
                    { 105, true, 47500m, 47200m, null, new DateTime(2024, 12, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 47800m, "SE" },
                    { 106, true, 13000m, 12850m, null, new DateTime(2024, 12, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 13150m, "S" },
                    { 107, true, 15250m, 15050m, null, new DateTime(2024, 12, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 15450m, "NE" },
                    { 108, true, 8750m, 8625m, null, new DateTime(2024, 12, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8875m, "N" },
                    { 109, true, 48000m, 47680m, null, new DateTime(2024, 12, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 48320m, "SE" },
                    { 110, true, 13200m, 13040m, null, new DateTime(2024, 12, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 13360m, "S" },
                    { 111, true, 15500m, 15290m, null, new DateTime(2024, 12, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 15710m, "NE" },
                    { 112, true, 8900m, 8770m, null, new DateTime(2024, 12, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 9030m, "N" },
                    { 113, true, 45000m, 44800m, null, new DateTime(2024, 12, 23, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 45200m, "SE" },
                    { 114, true, 12000m, 11900m, null, new DateTime(2024, 12, 23, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12100m, "S" },
                    { 115, true, 14000m, 13850m, null, new DateTime(2024, 12, 23, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14150m, "NE" },
                    { 116, true, 8000m, 7900m, null, new DateTime(2024, 12, 23, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8100m, "N" },
                    { 117, true, 45500m, 45280m, null, new DateTime(2024, 12, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 45720m, "SE" },
                    { 118, true, 12200m, 12090m, null, new DateTime(2024, 12, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 12310m, "S" },
                    { 119, true, 14250m, 14090m, null, new DateTime(2024, 12, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 14410m, "NE" },
                    { 120, true, 8150m, 8045m, null, new DateTime(2024, 12, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 8255m, "N" }
                });

            migrationBuilder.InsertData(
                table: "Intercambios",
                columns: new[] { "Id", "Ativo", "DataAtualizacao", "DataCriacao", "DataReferencia", "EnergiaIntercambiada", "Observacoes", "SubsistemaDestino", "SubsistemaOrigem" },
                values: new object[,]
                {
                    { 1, true, null, new DateTime(2024, 11, 25, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 300m, null, "S", "SE" },
                    { 2, true, null, new DateTime(2024, 11, 25, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 150m, null, "SE", "S" },
                    { 3, true, null, new DateTime(2024, 11, 25, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 450m, null, "NE", "SE" },
                    { 4, true, null, new DateTime(2024, 11, 25, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 100m, null, "SE", "NE" },
                    { 5, true, null, new DateTime(2024, 11, 25, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 200m, null, "NE", "N" },
                    { 6, true, null, new DateTime(2024, 11, 25, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 80m, null, "N", "NE" },
                    { 7, true, null, new DateTime(2024, 11, 25, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 50m, null, "N", "SE" },
                    { 8, true, null, new DateTime(2024, 11, 25, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), 30m, null, "SE", "N" },
                    { 9, true, null, new DateTime(2024, 11, 26, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 320m, null, "S", "SE" },
                    { 10, true, null, new DateTime(2024, 11, 26, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 165m, null, "SE", "S" },
                    { 11, true, null, new DateTime(2024, 11, 26, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 475m, null, "NE", "SE" },
                    { 12, true, null, new DateTime(2024, 11, 26, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 110m, null, "SE", "NE" },
                    { 13, true, null, new DateTime(2024, 11, 26, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 215m, null, "NE", "N" },
                    { 14, true, null, new DateTime(2024, 11, 26, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 88m, null, "N", "NE" },
                    { 15, true, null, new DateTime(2024, 11, 26, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 55m, null, "N", "SE" },
                    { 16, true, null, new DateTime(2024, 11, 26, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), 33m, null, "SE", "N" },
                    { 17, true, null, new DateTime(2024, 11, 27, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 340m, null, "S", "SE" },
                    { 18, true, null, new DateTime(2024, 11, 27, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 180m, null, "SE", "S" },
                    { 19, true, null, new DateTime(2024, 11, 27, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 500m, null, "NE", "SE" },
                    { 20, true, null, new DateTime(2024, 11, 27, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 120m, null, "SE", "NE" },
                    { 21, true, null, new DateTime(2024, 11, 27, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 230m, null, "NE", "N" },
                    { 22, true, null, new DateTime(2024, 11, 27, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 96m, null, "N", "NE" },
                    { 23, true, null, new DateTime(2024, 11, 27, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 60m, null, "N", "SE" },
                    { 24, true, null, new DateTime(2024, 11, 27, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 36m, null, "SE", "N" },
                    { 25, true, null, new DateTime(2024, 11, 28, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 360m, null, "S", "SE" },
                    { 26, true, null, new DateTime(2024, 11, 28, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 195m, null, "SE", "S" },
                    { 27, true, null, new DateTime(2024, 11, 28, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 525m, null, "NE", "SE" },
                    { 28, true, null, new DateTime(2024, 11, 28, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 130m, null, "SE", "NE" },
                    { 29, true, null, new DateTime(2024, 11, 28, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 245m, null, "NE", "N" },
                    { 30, true, null, new DateTime(2024, 11, 28, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 104m, null, "N", "NE" },
                    { 31, true, null, new DateTime(2024, 11, 28, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 65m, null, "N", "SE" },
                    { 32, true, null, new DateTime(2024, 11, 28, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 39m, null, "SE", "N" },
                    { 33, true, null, new DateTime(2024, 11, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 380m, null, "S", "SE" },
                    { 34, true, null, new DateTime(2024, 11, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 210m, null, "SE", "S" },
                    { 35, true, null, new DateTime(2024, 11, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 550m, null, "NE", "SE" },
                    { 36, true, null, new DateTime(2024, 11, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 140m, null, "SE", "NE" },
                    { 37, true, null, new DateTime(2024, 11, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 260m, null, "NE", "N" },
                    { 38, true, null, new DateTime(2024, 11, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 112m, null, "N", "NE" },
                    { 39, true, null, new DateTime(2024, 11, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 70m, null, "N", "SE" },
                    { 40, true, null, new DateTime(2024, 11, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 42m, null, "SE", "N" },
                    { 41, true, null, new DateTime(2024, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 400m, null, "S", "SE" },
                    { 42, true, null, new DateTime(2024, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 225m, null, "SE", "S" },
                    { 43, true, null, new DateTime(2024, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 575m, null, "NE", "SE" },
                    { 44, true, null, new DateTime(2024, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 150m, null, "SE", "NE" },
                    { 45, true, null, new DateTime(2024, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 275m, null, "NE", "N" },
                    { 46, true, null, new DateTime(2024, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 120m, null, "N", "NE" },
                    { 47, true, null, new DateTime(2024, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 75m, null, "N", "SE" },
                    { 48, true, null, new DateTime(2024, 11, 30, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 45m, null, "SE", "N" },
                    { 49, true, null, new DateTime(2024, 12, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 420m, null, "S", "SE" },
                    { 50, true, null, new DateTime(2024, 12, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 240m, null, "SE", "S" },
                    { 51, true, null, new DateTime(2024, 12, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 600m, null, "NE", "SE" },
                    { 52, true, null, new DateTime(2024, 12, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 160m, null, "SE", "NE" },
                    { 53, true, null, new DateTime(2024, 12, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 290m, null, "NE", "N" },
                    { 54, true, null, new DateTime(2024, 12, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 128m, null, "N", "NE" },
                    { 55, true, null, new DateTime(2024, 12, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 80m, null, "N", "SE" },
                    { 56, true, null, new DateTime(2024, 12, 1, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 48m, null, "SE", "N" },
                    { 57, true, null, new DateTime(2024, 12, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 440m, null, "S", "SE" },
                    { 58, true, null, new DateTime(2024, 12, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 255m, null, "SE", "S" },
                    { 59, true, null, new DateTime(2024, 12, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 625m, null, "NE", "SE" },
                    { 60, true, null, new DateTime(2024, 12, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 170m, null, "SE", "NE" },
                    { 61, true, null, new DateTime(2024, 12, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 305m, null, "NE", "N" },
                    { 62, true, null, new DateTime(2024, 12, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 136m, null, "N", "NE" },
                    { 63, true, null, new DateTime(2024, 12, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 85m, null, "N", "SE" },
                    { 64, true, null, new DateTime(2024, 12, 2, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 51m, null, "SE", "N" },
                    { 65, true, null, new DateTime(2024, 12, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 460m, null, "S", "SE" },
                    { 66, true, null, new DateTime(2024, 12, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 270m, null, "SE", "S" },
                    { 67, true, null, new DateTime(2024, 12, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 650m, null, "NE", "SE" },
                    { 68, true, null, new DateTime(2024, 12, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 180m, null, "SE", "NE" },
                    { 69, true, null, new DateTime(2024, 12, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 320m, null, "NE", "N" },
                    { 70, true, null, new DateTime(2024, 12, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 144m, null, "N", "NE" },
                    { 71, true, null, new DateTime(2024, 12, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 90m, null, "N", "SE" },
                    { 72, true, null, new DateTime(2024, 12, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 54m, null, "SE", "N" },
                    { 73, true, null, new DateTime(2024, 12, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 480m, null, "S", "SE" },
                    { 74, true, null, new DateTime(2024, 12, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 285m, null, "SE", "S" },
                    { 75, true, null, new DateTime(2024, 12, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 675m, null, "NE", "SE" },
                    { 76, true, null, new DateTime(2024, 12, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 190m, null, "SE", "NE" },
                    { 77, true, null, new DateTime(2024, 12, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 335m, null, "NE", "N" },
                    { 78, true, null, new DateTime(2024, 12, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 152m, null, "N", "NE" },
                    { 79, true, null, new DateTime(2024, 12, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 95m, null, "N", "SE" },
                    { 80, true, null, new DateTime(2024, 12, 4, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 57m, null, "SE", "N" },
                    { 81, true, null, new DateTime(2024, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 300m, null, "S", "SE" },
                    { 82, true, null, new DateTime(2024, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 150m, null, "SE", "S" },
                    { 83, true, null, new DateTime(2024, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 450m, null, "NE", "SE" },
                    { 84, true, null, new DateTime(2024, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 100m, null, "SE", "NE" },
                    { 85, true, null, new DateTime(2024, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 200m, null, "NE", "N" },
                    { 86, true, null, new DateTime(2024, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 80m, null, "N", "NE" },
                    { 87, true, null, new DateTime(2024, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 50m, null, "N", "SE" },
                    { 88, true, null, new DateTime(2024, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 30m, null, "SE", "N" },
                    { 89, true, null, new DateTime(2024, 12, 6, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 320m, null, "S", "SE" },
                    { 90, true, null, new DateTime(2024, 12, 6, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 165m, null, "SE", "S" },
                    { 91, true, null, new DateTime(2024, 12, 6, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 475m, null, "NE", "SE" },
                    { 92, true, null, new DateTime(2024, 12, 6, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 110m, null, "SE", "NE" },
                    { 93, true, null, new DateTime(2024, 12, 6, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 215m, null, "NE", "N" },
                    { 94, true, null, new DateTime(2024, 12, 6, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 88m, null, "N", "NE" },
                    { 95, true, null, new DateTime(2024, 12, 6, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 55m, null, "N", "SE" },
                    { 96, true, null, new DateTime(2024, 12, 6, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 33m, null, "SE", "N" },
                    { 97, true, null, new DateTime(2024, 12, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 340m, null, "S", "SE" },
                    { 98, true, null, new DateTime(2024, 12, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 180m, null, "SE", "S" },
                    { 99, true, null, new DateTime(2024, 12, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 500m, null, "NE", "SE" },
                    { 100, true, null, new DateTime(2024, 12, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 120m, null, "SE", "NE" },
                    { 101, true, null, new DateTime(2024, 12, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 230m, null, "NE", "N" },
                    { 102, true, null, new DateTime(2024, 12, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 96m, null, "N", "NE" },
                    { 103, true, null, new DateTime(2024, 12, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 60m, null, "N", "SE" },
                    { 104, true, null, new DateTime(2024, 12, 7, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 36m, null, "SE", "N" },
                    { 105, true, null, new DateTime(2024, 12, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 360m, null, "S", "SE" },
                    { 106, true, null, new DateTime(2024, 12, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 195m, null, "SE", "S" },
                    { 107, true, null, new DateTime(2024, 12, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 525m, null, "NE", "SE" },
                    { 108, true, null, new DateTime(2024, 12, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 130m, null, "SE", "NE" },
                    { 109, true, null, new DateTime(2024, 12, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 245m, null, "NE", "N" },
                    { 110, true, null, new DateTime(2024, 12, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 104m, null, "N", "NE" },
                    { 111, true, null, new DateTime(2024, 12, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 65m, null, "N", "SE" },
                    { 112, true, null, new DateTime(2024, 12, 8, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 39m, null, "SE", "N" },
                    { 113, true, null, new DateTime(2024, 12, 9, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 380m, null, "S", "SE" },
                    { 114, true, null, new DateTime(2024, 12, 9, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 210m, null, "SE", "S" },
                    { 115, true, null, new DateTime(2024, 12, 9, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 550m, null, "NE", "SE" },
                    { 116, true, null, new DateTime(2024, 12, 9, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 140m, null, "SE", "NE" },
                    { 117, true, null, new DateTime(2024, 12, 9, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 260m, null, "NE", "N" },
                    { 118, true, null, new DateTime(2024, 12, 9, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 112m, null, "N", "NE" },
                    { 119, true, null, new DateTime(2024, 12, 9, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 70m, null, "N", "SE" },
                    { 120, true, null, new DateTime(2024, 12, 9, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 42m, null, "SE", "N" },
                    { 121, true, null, new DateTime(2024, 12, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 400m, null, "S", "SE" },
                    { 122, true, null, new DateTime(2024, 12, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 225m, null, "SE", "S" },
                    { 123, true, null, new DateTime(2024, 12, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 575m, null, "NE", "SE" },
                    { 124, true, null, new DateTime(2024, 12, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 150m, null, "SE", "NE" },
                    { 125, true, null, new DateTime(2024, 12, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 275m, null, "NE", "N" },
                    { 126, true, null, new DateTime(2024, 12, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 120m, null, "N", "NE" },
                    { 127, true, null, new DateTime(2024, 12, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 75m, null, "N", "SE" },
                    { 128, true, null, new DateTime(2024, 12, 10, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 45m, null, "SE", "N" },
                    { 129, true, null, new DateTime(2024, 12, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 420m, null, "S", "SE" },
                    { 130, true, null, new DateTime(2024, 12, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 240m, null, "SE", "S" },
                    { 131, true, null, new DateTime(2024, 12, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 600m, null, "NE", "SE" },
                    { 132, true, null, new DateTime(2024, 12, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 160m, null, "SE", "NE" },
                    { 133, true, null, new DateTime(2024, 12, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 290m, null, "NE", "N" },
                    { 134, true, null, new DateTime(2024, 12, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 128m, null, "N", "NE" },
                    { 135, true, null, new DateTime(2024, 12, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 80m, null, "N", "SE" },
                    { 136, true, null, new DateTime(2024, 12, 11, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 48m, null, "SE", "N" },
                    { 137, true, null, new DateTime(2024, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 440m, null, "S", "SE" },
                    { 138, true, null, new DateTime(2024, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 255m, null, "SE", "S" },
                    { 139, true, null, new DateTime(2024, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 625m, null, "NE", "SE" },
                    { 140, true, null, new DateTime(2024, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 170m, null, "SE", "NE" },
                    { 141, true, null, new DateTime(2024, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 305m, null, "NE", "N" },
                    { 142, true, null, new DateTime(2024, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 136m, null, "N", "NE" },
                    { 143, true, null, new DateTime(2024, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 85m, null, "N", "SE" },
                    { 144, true, null, new DateTime(2024, 12, 12, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 51m, null, "SE", "N" },
                    { 145, true, null, new DateTime(2024, 12, 13, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 460m, null, "S", "SE" },
                    { 146, true, null, new DateTime(2024, 12, 13, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 270m, null, "SE", "S" },
                    { 147, true, null, new DateTime(2024, 12, 13, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 650m, null, "NE", "SE" },
                    { 148, true, null, new DateTime(2024, 12, 13, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 180m, null, "SE", "NE" },
                    { 149, true, null, new DateTime(2024, 12, 13, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 320m, null, "NE", "N" },
                    { 150, true, null, new DateTime(2024, 12, 13, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 144m, null, "N", "NE" },
                    { 151, true, null, new DateTime(2024, 12, 13, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 90m, null, "N", "SE" },
                    { 152, true, null, new DateTime(2024, 12, 13, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), 54m, null, "SE", "N" },
                    { 153, true, null, new DateTime(2024, 12, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 480m, null, "S", "SE" },
                    { 154, true, null, new DateTime(2024, 12, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 285m, null, "SE", "S" },
                    { 155, true, null, new DateTime(2024, 12, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 675m, null, "NE", "SE" },
                    { 156, true, null, new DateTime(2024, 12, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 190m, null, "SE", "NE" },
                    { 157, true, null, new DateTime(2024, 12, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 335m, null, "NE", "N" },
                    { 158, true, null, new DateTime(2024, 12, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 152m, null, "N", "NE" },
                    { 159, true, null, new DateTime(2024, 12, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 95m, null, "N", "SE" },
                    { 160, true, null, new DateTime(2024, 12, 14, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 57m, null, "SE", "N" },
                    { 161, true, null, new DateTime(2024, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 300m, null, "S", "SE" },
                    { 162, true, null, new DateTime(2024, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 150m, null, "SE", "S" },
                    { 163, true, null, new DateTime(2024, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 450m, null, "NE", "SE" },
                    { 164, true, null, new DateTime(2024, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 100m, null, "SE", "NE" },
                    { 165, true, null, new DateTime(2024, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 200m, null, "NE", "N" },
                    { 166, true, null, new DateTime(2024, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 80m, null, "N", "NE" },
                    { 167, true, null, new DateTime(2024, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 50m, null, "N", "SE" },
                    { 168, true, null, new DateTime(2024, 12, 15, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 30m, null, "SE", "N" },
                    { 169, true, null, new DateTime(2024, 12, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 320m, null, "S", "SE" },
                    { 170, true, null, new DateTime(2024, 12, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 165m, null, "SE", "S" },
                    { 171, true, null, new DateTime(2024, 12, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 475m, null, "NE", "SE" },
                    { 172, true, null, new DateTime(2024, 12, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 110m, null, "SE", "NE" },
                    { 173, true, null, new DateTime(2024, 12, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 215m, null, "NE", "N" },
                    { 174, true, null, new DateTime(2024, 12, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 88m, null, "N", "NE" },
                    { 175, true, null, new DateTime(2024, 12, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 55m, null, "N", "SE" },
                    { 176, true, null, new DateTime(2024, 12, 16, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 33m, null, "SE", "N" },
                    { 177, true, null, new DateTime(2024, 12, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 340m, null, "S", "SE" },
                    { 178, true, null, new DateTime(2024, 12, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 180m, null, "SE", "S" },
                    { 179, true, null, new DateTime(2024, 12, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 500m, null, "NE", "SE" },
                    { 180, true, null, new DateTime(2024, 12, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 120m, null, "SE", "NE" },
                    { 181, true, null, new DateTime(2024, 12, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 230m, null, "NE", "N" },
                    { 182, true, null, new DateTime(2024, 12, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 96m, null, "N", "NE" },
                    { 183, true, null, new DateTime(2024, 12, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 60m, null, "N", "SE" },
                    { 184, true, null, new DateTime(2024, 12, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), 36m, null, "SE", "N" },
                    { 185, true, null, new DateTime(2024, 12, 18, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 360m, null, "S", "SE" },
                    { 186, true, null, new DateTime(2024, 12, 18, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 195m, null, "SE", "S" },
                    { 187, true, null, new DateTime(2024, 12, 18, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 525m, null, "NE", "SE" },
                    { 188, true, null, new DateTime(2024, 12, 18, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 130m, null, "SE", "NE" },
                    { 189, true, null, new DateTime(2024, 12, 18, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 245m, null, "NE", "N" },
                    { 190, true, null, new DateTime(2024, 12, 18, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 104m, null, "N", "NE" },
                    { 191, true, null, new DateTime(2024, 12, 18, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 65m, null, "N", "SE" },
                    { 192, true, null, new DateTime(2024, 12, 18, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 39m, null, "SE", "N" },
                    { 193, true, null, new DateTime(2024, 12, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 380m, null, "S", "SE" },
                    { 194, true, null, new DateTime(2024, 12, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 210m, null, "SE", "S" },
                    { 195, true, null, new DateTime(2024, 12, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 550m, null, "NE", "SE" },
                    { 196, true, null, new DateTime(2024, 12, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 140m, null, "SE", "NE" },
                    { 197, true, null, new DateTime(2024, 12, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 260m, null, "NE", "N" },
                    { 198, true, null, new DateTime(2024, 12, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 112m, null, "N", "NE" },
                    { 199, true, null, new DateTime(2024, 12, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 70m, null, "N", "SE" },
                    { 200, true, null, new DateTime(2024, 12, 19, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 42m, null, "SE", "N" },
                    { 201, true, null, new DateTime(2024, 12, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 400m, null, "S", "SE" },
                    { 202, true, null, new DateTime(2024, 12, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 225m, null, "SE", "S" },
                    { 203, true, null, new DateTime(2024, 12, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 575m, null, "NE", "SE" },
                    { 204, true, null, new DateTime(2024, 12, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 150m, null, "SE", "NE" },
                    { 205, true, null, new DateTime(2024, 12, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 275m, null, "NE", "N" },
                    { 206, true, null, new DateTime(2024, 12, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 120m, null, "N", "NE" },
                    { 207, true, null, new DateTime(2024, 12, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 75m, null, "N", "SE" },
                    { 208, true, null, new DateTime(2024, 12, 20, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 45m, null, "SE", "N" },
                    { 209, true, null, new DateTime(2024, 12, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 420m, null, "S", "SE" },
                    { 210, true, null, new DateTime(2024, 12, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 240m, null, "SE", "S" },
                    { 211, true, null, new DateTime(2024, 12, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 600m, null, "NE", "SE" },
                    { 212, true, null, new DateTime(2024, 12, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 160m, null, "SE", "NE" },
                    { 213, true, null, new DateTime(2024, 12, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 290m, null, "NE", "N" },
                    { 214, true, null, new DateTime(2024, 12, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 128m, null, "N", "NE" },
                    { 215, true, null, new DateTime(2024, 12, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 80m, null, "N", "SE" },
                    { 216, true, null, new DateTime(2024, 12, 21, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 48m, null, "SE", "N" },
                    { 217, true, null, new DateTime(2024, 12, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 440m, null, "S", "SE" },
                    { 218, true, null, new DateTime(2024, 12, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 255m, null, "SE", "S" },
                    { 219, true, null, new DateTime(2024, 12, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 625m, null, "NE", "SE" },
                    { 220, true, null, new DateTime(2024, 12, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 170m, null, "SE", "NE" },
                    { 221, true, null, new DateTime(2024, 12, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 305m, null, "NE", "N" },
                    { 222, true, null, new DateTime(2024, 12, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 136m, null, "N", "NE" },
                    { 223, true, null, new DateTime(2024, 12, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 85m, null, "N", "SE" },
                    { 224, true, null, new DateTime(2024, 12, 22, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 51m, null, "SE", "N" },
                    { 225, true, null, new DateTime(2024, 12, 23, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 460m, null, "S", "SE" },
                    { 226, true, null, new DateTime(2024, 12, 23, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 270m, null, "SE", "S" },
                    { 227, true, null, new DateTime(2024, 12, 23, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 650m, null, "NE", "SE" },
                    { 228, true, null, new DateTime(2024, 12, 23, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 180m, null, "SE", "NE" },
                    { 229, true, null, new DateTime(2024, 12, 23, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 320m, null, "NE", "N" },
                    { 230, true, null, new DateTime(2024, 12, 23, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 144m, null, "N", "NE" },
                    { 231, true, null, new DateTime(2024, 12, 23, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 90m, null, "N", "SE" },
                    { 232, true, null, new DateTime(2024, 12, 23, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 54m, null, "SE", "N" },
                    { 233, true, null, new DateTime(2024, 12, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 480m, null, "S", "SE" },
                    { 234, true, null, new DateTime(2024, 12, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 285m, null, "SE", "S" },
                    { 235, true, null, new DateTime(2024, 12, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 675m, null, "NE", "SE" },
                    { 236, true, null, new DateTime(2024, 12, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 190m, null, "SE", "NE" },
                    { 237, true, null, new DateTime(2024, 12, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 335m, null, "NE", "N" },
                    { 238, true, null, new DateTime(2024, 12, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 152m, null, "N", "NE" },
                    { 239, true, null, new DateTime(2024, 12, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 95m, null, "N", "SE" },
                    { 240, true, null, new DateTime(2024, 12, 24, 18, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 57m, null, "SE", "N" }
                });

            migrationBuilder.InsertData(
                table: "UnidadesGeradoras",
                columns: new[] { "Id", "Ativo", "Codigo", "DataAtualizacao", "DataComissionamento", "DataCriacao", "Nome", "PotenciaMinima", "PotenciaNominal", "Status", "UsinaId" },
                values: new object[,]
                {
                    { 1, true, "ITAIPU-UG01", null, new DateTime(1984, 6, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 1 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 2, true, "ITAIPU-UG02", null, new DateTime(1984, 7, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 2 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 3, true, "ITAIPU-UG03", null, new DateTime(1984, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 3 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 4, true, "ITAIPU-UG04", null, new DateTime(1984, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 4 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 5, true, "ITAIPU-UG05", null, new DateTime(1984, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 5 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 6, true, "ITAIPU-UG06", null, new DateTime(1984, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 6 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 7, true, "ITAIPU-UG07", null, new DateTime(1984, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 7 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 8, true, "ITAIPU-UG08", null, new DateTime(1984, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 8 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 9, true, "ITAIPU-UG09", null, new DateTime(1985, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 9 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 10, true, "ITAIPU-UG10", null, new DateTime(1985, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 10 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 11, true, "ITAIPU-UG11", null, new DateTime(1985, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 11 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 12, true, "ITAIPU-UG12", null, new DateTime(1985, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 12 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 13, true, "ITAIPU-UG13", null, new DateTime(1985, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 13 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 14, true, "ITAIPU-UG14", null, new DateTime(1985, 6, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 14 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 15, true, "ITAIPU-UG15", null, new DateTime(1985, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 15 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 16, true, "ITAIPU-UG16", null, new DateTime(1985, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 16 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 17, true, "ITAIPU-UG17", null, new DateTime(1985, 9, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 17 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 18, true, "ITAIPU-UG18", null, new DateTime(1985, 10, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 18 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 19, true, "ITAIPU-UG19", null, new DateTime(1985, 11, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 19 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 20, true, "ITAIPU-UG20", null, new DateTime(1985, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 20 - Itaipu", 350.00m, 700.00m, "Operando", 1 },
                    { 21, true, "BELO-MONTE-UG01", null, new DateTime(2016, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 1 - Belo Monte", 300.00m, 611.00m, "Operando", 2 },
                    { 22, true, "BELO-MONTE-UG02", null, new DateTime(2016, 5, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 2 - Belo Monte", 300.00m, 611.00m, "Operando", 2 },
                    { 23, true, "BELO-MONTE-UG03", null, new DateTime(2016, 6, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 3 - Belo Monte", 300.00m, 611.00m, "Operando", 2 },
                    { 24, true, "BELO-MONTE-UG04", null, new DateTime(2016, 6, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 4 - Belo Monte", 300.00m, 611.00m, "Operando", 2 },
                    { 25, true, "BELO-MONTE-UG05", null, new DateTime(2016, 7, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 5 - Belo Monte", 300.00m, 611.00m, "Operando", 2 },
                    { 26, true, "BELO-MONTE-UG06", null, new DateTime(2016, 7, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 6 - Belo Monte", 300.00m, 611.00m, "Operando", 2 },
                    { 27, true, "BELO-MONTE-UG07", null, new DateTime(2016, 8, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 7 - Belo Monte", 300.00m, 611.00m, "Operando", 2 },
                    { 28, true, "BELO-MONTE-UG08", null, new DateTime(2016, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 8 - Belo Monte", 300.00m, 611.00m, "Operando", 2 },
                    { 29, true, "BELO-MONTE-UG09", null, new DateTime(2016, 9, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 9 - Belo Monte", 300.00m, 611.00m, "Operando", 2 },
                    { 30, true, "BELO-MONTE-UG10", null, new DateTime(2016, 9, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 10 - Belo Monte", 300.00m, 611.00m, "Operando", 2 },
                    { 31, true, "BELO-MONTE-UG11", null, new DateTime(2016, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 11 - Belo Monte", 300.00m, 611.00m, "Operando", 2 },
                    { 32, true, "BELO-MONTE-UG12", null, new DateTime(2016, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 12 - Belo Monte", 300.00m, 611.00m, "Operando", 2 },
                    { 33, true, "BELO-MONTE-UG13", null, new DateTime(2016, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 13 - Belo Monte", 300.00m, 611.00m, "Operando", 2 },
                    { 34, true, "BELO-MONTE-UG14", null, new DateTime(2016, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 14 - Belo Monte", 300.00m, 611.00m, "Operando", 2 },
                    { 35, true, "BELO-MONTE-UG15", null, new DateTime(2016, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 15 - Belo Monte", 300.00m, 611.00m, "Operando", 2 },
                    { 36, true, "BELO-MONTE-UG16", null, new DateTime(2016, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 16 - Belo Monte", 300.00m, 611.00m, "Operando", 2 },
                    { 37, true, "BELO-MONTE-UG17", null, new DateTime(2017, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 17 - Belo Monte", 300.00m, 611.00m, "Operando", 2 },
                    { 38, true, "BELO-MONTE-UG18", null, new DateTime(2017, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 18 - Belo Monte", 300.00m, 611.00m, "Operando", 2 },
                    { 39, true, "TUCURUI-UG01", null, new DateTime(1984, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 1 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 40, true, "TUCURUI-UG02", null, new DateTime(1985, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 2 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 41, true, "TUCURUI-UG03", null, new DateTime(1985, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 3 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 42, true, "TUCURUI-UG04", null, new DateTime(1985, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 4 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 43, true, "TUCURUI-UG05", null, new DateTime(1985, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 5 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 44, true, "TUCURUI-UG06", null, new DateTime(1985, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 6 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 45, true, "TUCURUI-UG07", null, new DateTime(1985, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 7 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 46, true, "TUCURUI-UG08", null, new DateTime(1985, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 8 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 47, true, "TUCURUI-UG09", null, new DateTime(1985, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 9 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 48, true, "TUCURUI-UG10", null, new DateTime(1985, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 10 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 49, true, "TUCURUI-UG11", null, new DateTime(1985, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 11 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 50, true, "TUCURUI-UG12", null, new DateTime(1985, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 12 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 51, true, "TUCURUI-UG13", null, new DateTime(1985, 8, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 13 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 52, true, "TUCURUI-UG14", null, new DateTime(1985, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 14 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 53, true, "TUCURUI-UG15", null, new DateTime(1985, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 15 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 54, true, "TUCURUI-UG16", null, new DateTime(1985, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 16 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 55, true, "TUCURUI-UG17", null, new DateTime(1985, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 17 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 56, true, "TUCURUI-UG18", null, new DateTime(1985, 11, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 18 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 57, true, "TUCURUI-UG19", null, new DateTime(1985, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 19 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 58, true, "TUCURUI-UG20", null, new DateTime(1985, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 20 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 59, true, "TUCURUI-UG21", null, new DateTime(1986, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 21 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 60, true, "TUCURUI-UG22", null, new DateTime(1986, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 22 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 61, true, "TUCURUI-UG23", null, new DateTime(1986, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 23 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 62, true, "TUCURUI-UG24", null, new DateTime(1986, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 24 - Tucuruí", 175.00m, 350.00m, "Operando", 3 },
                    { 63, true, "JIRAU-UG01", null, new DateTime(2010, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 1 - JIRAU", 100.00m, 200.00m, "Operando", 5 },
                    { 64, true, "ILHA-SOLTEIRA-UG02", null, new DateTime(2010, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 2 - ILHA-SOLTEIRA", 100.00m, 200.00m, "Operando", 6 },
                    { 65, true, "XINGO-UG03", null, new DateTime(2010, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 3 - XINGO", 100.00m, 200.00m, "Operando", 7 },
                    { 66, true, "PAULO-AFONSO-IV-UG04", null, new DateTime(2010, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 4 - PAULO-AFONSO-IV", 100.00m, 200.00m, "Operando", 8 },
                    { 67, true, "ANGRA-I-UG05", null, new DateTime(2010, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 5 - ANGRA-I", 160.00m, 320.00m, "Operando", 9 },
                    { 68, true, "ANGRA-II-UG06", null, new DateTime(2010, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 6 - ANGRA-II", 337.50m, 675.00m, "Operando", 10 },
                    { 69, true, "SANTO-ANTONIO-UG07", null, new DateTime(2010, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 7 - SANTO-ANTONIO", 100.00m, 200.00m, "Operando", 4 },
                    { 70, true, "JIRAU-UG08", null, new DateTime(2010, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 8 - JIRAU", 100.00m, 200.00m, "Operando", 5 },
                    { 71, true, "ILHA-SOLTEIRA-UG09", null, new DateTime(2010, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 9 - ILHA-SOLTEIRA", 100.00m, 200.00m, "Operando", 6 },
                    { 72, true, "XINGO-UG10", null, new DateTime(2010, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 10 - XINGO", 100.00m, 200.00m, "Manutenção", 7 },
                    { 73, true, "PAULO-AFONSO-IV-UG11", null, new DateTime(2010, 4, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 11 - PAULO-AFONSO-IV", 100.00m, 200.00m, "Operando", 8 },
                    { 74, true, "ANGRA-I-UG12", null, new DateTime(2010, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 12 - ANGRA-I", 160.00m, 320.00m, "Operando", 9 },
                    { 75, true, "ANGRA-II-UG13", null, new DateTime(2010, 5, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 13 - ANGRA-II", 337.50m, 675.00m, "Operando", 10 },
                    { 76, true, "SANTO-ANTONIO-UG14", null, new DateTime(2010, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 14 - SANTO-ANTONIO", 100.00m, 200.00m, "Operando", 4 },
                    { 77, true, "JIRAU-UG15", null, new DateTime(2010, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 15 - JIRAU", 100.00m, 200.00m, "Operando", 5 },
                    { 78, true, "ILHA-SOLTEIRA-UG16", null, new DateTime(2010, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 16 - ILHA-SOLTEIRA", 100.00m, 200.00m, "Operando", 6 },
                    { 79, true, "XINGO-UG17", null, new DateTime(2010, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 17 - XINGO", 100.00m, 200.00m, "Operando", 7 },
                    { 80, true, "PAULO-AFONSO-IV-UG18", null, new DateTime(2010, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 18 - PAULO-AFONSO-IV", 100.00m, 200.00m, "Operando", 8 },
                    { 81, true, "ANGRA-I-UG19", null, new DateTime(2010, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 19 - ANGRA-I", 160.00m, 320.00m, "Operando", 9 },
                    { 82, true, "ANGRA-II-UG20", null, new DateTime(2010, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 20 - ANGRA-II", 337.50m, 675.00m, "Manutenção", 10 },
                    { 83, true, "SANTO-ANTONIO-UG21", null, new DateTime(2010, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 21 - SANTO-ANTONIO", 100.00m, 200.00m, "Operando", 4 },
                    { 84, true, "JIRAU-UG22", null, new DateTime(2010, 8, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 22 - JIRAU", 100.00m, 200.00m, "Operando", 5 },
                    { 85, true, "ILHA-SOLTEIRA-UG23", null, new DateTime(2010, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 23 - ILHA-SOLTEIRA", 100.00m, 200.00m, "Operando", 6 },
                    { 86, true, "XINGO-UG24", null, new DateTime(2010, 8, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 24 - XINGO", 100.00m, 200.00m, "Operando", 7 },
                    { 87, true, "PAULO-AFONSO-IV-UG25", null, new DateTime(2010, 9, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 25 - PAULO-AFONSO-IV", 100.00m, 200.00m, "Operando", 8 },
                    { 88, true, "ANGRA-I-UG26", null, new DateTime(2010, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 26 - ANGRA-I", 160.00m, 320.00m, "Operando", 9 },
                    { 89, true, "ANGRA-II-UG27", null, new DateTime(2010, 9, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 27 - ANGRA-II", 337.50m, 675.00m, "Operando", 10 },
                    { 90, true, "SANTO-ANTONIO-UG28", null, new DateTime(2010, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 28 - SANTO-ANTONIO", 100.00m, 200.00m, "Operando", 4 },
                    { 91, true, "JIRAU-UG29", null, new DateTime(2010, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 29 - JIRAU", 100.00m, 200.00m, "Operando", 5 },
                    { 92, true, "ILHA-SOLTEIRA-UG30", null, new DateTime(2010, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 30 - ILHA-SOLTEIRA", 100.00m, 200.00m, "Manutenção", 6 },
                    { 93, true, "XINGO-UG31", null, new DateTime(2010, 11, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 31 - XINGO", 100.00m, 200.00m, "Operando", 7 },
                    { 94, true, "PAULO-AFONSO-IV-UG32", null, new DateTime(2010, 11, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 32 - PAULO-AFONSO-IV", 100.00m, 200.00m, "Operando", 8 },
                    { 95, true, "ANGRA-I-UG33", null, new DateTime(2010, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 33 - ANGRA-I", 160.00m, 320.00m, "Operando", 9 },
                    { 96, true, "ANGRA-II-UG34", null, new DateTime(2010, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 34 - ANGRA-II", 337.50m, 675.00m, "Operando", 10 },
                    { 97, true, "SANTO-ANTONIO-UG35", null, new DateTime(2010, 12, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 35 - SANTO-ANTONIO", 100.00m, 200.00m, "Operando", 4 },
                    { 98, true, "JIRAU-UG36", null, new DateTime(2010, 12, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 36 - JIRAU", 100.00m, 200.00m, "Operando", 5 },
                    { 99, true, "ILHA-SOLTEIRA-UG37", null, new DateTime(2011, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 37 - ILHA-SOLTEIRA", 100.00m, 200.00m, "Operando", 6 },
                    { 100, true, "XINGO-UG38", null, new DateTime(2011, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2010, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unidade Geradora 38 - XINGO", 100.00m, 200.00m, "Operando", 7 }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Ativo", "DataAtualizacao", "DataCriacao", "Email", "EquipePDPId", "Nome", "Perfil", "Telefone" },
                values: new object[,]
                {
                    { 1, true, null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@ons.org.br", null, "Administrador Sistema", "Administrador", "(21) 3444-5000" },
                    { 2, true, null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "joao.coord@ons.org.br", 1, "João Silva Santos", "Coordenador", "(81) 3421-5000" },
                    { 3, true, null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "maria.op@ons.org.br", 2, "Maria Oliveira Costa", "Operador", "(21) 3444-5500" },
                    { 4, true, null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "carlos.op@ons.org.br", 3, "Carlos Eduardo Ferreira", "Operador", "(41) 3333-4400" },
                    { 5, true, null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ana.op@ons.org.br", 4, "Ana Paula Rodrigues", "Operador", "(92) 3232-1100" },
                    { 6, true, null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "roberto.plan@ons.org.br", 5, "Roberto Mendes Lima", "Coordenador", "(21) 3444-5600" },
                    { 7, true, null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "fernanda.ana@ons.org.br", 2, "Fernanda Alves", "Analista", "(21) 3444-5700" },
                    { 8, true, null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ricardo.ana@ons.org.br", 2, "Ricardo Santos", "Analista", "(21) 3444-5800" },
                    { 9, true, null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "juliana.ana@ons.org.br", 1, "Juliana Pereira", "Analista", "(81) 3421-5100" },
                    { 10, true, null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "roberto.cons@ons.org.br", null, "Roberto Consultor", "Consultor", "(21) 3444-5900" },
                    { 11, true, null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "patricia.cons@ons.org.br", null, "Patrícia Lima", "Consultor", "(21) 3444-6000" },
                    { 12, true, null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bruno.op@ons.org.br", 3, "Bruno Costa", "Operador", "(41) 3333-4500" },
                    { 13, true, null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "camila.coord@ons.org.br", 4, "Camila Souza", "Coordenador", "(92) 3232-1200" },
                    { 14, true, null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "diego.ana@ons.org.br", 3, "Diego Martins", "Analista", "(41) 3333-4600" },
                    { 15, true, null, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "elaine.coord@ons.org.br", 5, "Elaine Rodrigues", "Coordenador", "(21) 3444-6100" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Balancos",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Cargas",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 181);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 187);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 191);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 192);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 198);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 199);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 200);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 226);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 227);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 228);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 229);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 230);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 231);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 232);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 233);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 234);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 235);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 236);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 237);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 238);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 239);

            migrationBuilder.DeleteData(
                table: "Intercambios",
                keyColumn: "Id",
                keyValue: 240);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 88);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 89);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 90);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 91);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 92);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 93);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 94);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 95);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 96);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 97);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 98);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 99);

            migrationBuilder.DeleteData(
                table: "UnidadesGeradoras",
                keyColumn: "Id",
                keyValue: 100);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 15);
        }
    }
}
