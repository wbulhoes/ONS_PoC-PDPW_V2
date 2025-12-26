using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDPW.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarEnergiaVertida : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "EnergiaTurbinavelNaoUtilizada",
                table: "DadosEnergeticos",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "EnergiaVertida",
                table: "DadosEnergeticos",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoVertimento",
                table: "DadosEnergeticos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnergiaTurbinavelNaoUtilizada",
                table: "DadosEnergeticos");

            migrationBuilder.DropColumn(
                name: "EnergiaVertida",
                table: "DadosEnergeticos");

            migrationBuilder.DropColumn(
                name: "MotivoVertimento",
                table: "DadosEnergeticos");
        }
    }
}
