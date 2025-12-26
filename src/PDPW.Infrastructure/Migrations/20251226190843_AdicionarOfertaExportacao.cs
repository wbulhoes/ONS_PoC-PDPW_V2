using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDPW.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarOfertaExportacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OfertasExportacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsinaId = table.Column<int>(type: "int", nullable: false),
                    DataOferta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataPDP = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValorMW = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoMWh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HoraInicial = table.Column<TimeSpan>(type: "time", nullable: false),
                    HoraFinal = table.Column<TimeSpan>(type: "time", nullable: false),
                    FlgAprovadoONS = table.Column<bool>(type: "bit", nullable: true),
                    DataAnaliseONS = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioAnaliseONS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ObservacaoONS = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SemanaPMOId = table.Column<int>(type: "int", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfertasExportacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfertasExportacao_SemanasPMO_SemanaPMOId",
                        column: x => x.SemanaPMOId,
                        principalTable: "SemanasPMO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_OfertasExportacao_Usinas_UsinaId",
                        column: x => x.UsinaId,
                        principalTable: "Usinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfertasExportacao_DataPDP",
                table: "OfertasExportacao",
                column: "DataPDP");

            migrationBuilder.CreateIndex(
                name: "IX_OfertasExportacao_FlgAprovadoONS",
                table: "OfertasExportacao",
                column: "FlgAprovadoONS");

            migrationBuilder.CreateIndex(
                name: "IX_OfertasExportacao_SemanaPMOId",
                table: "OfertasExportacao",
                column: "SemanaPMOId");

            migrationBuilder.CreateIndex(
                name: "IX_OfertasExportacao_UsinaId_DataPDP",
                table: "OfertasExportacao",
                columns: new[] { "UsinaId", "DataPDP" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfertasExportacao");
        }
    }
}
