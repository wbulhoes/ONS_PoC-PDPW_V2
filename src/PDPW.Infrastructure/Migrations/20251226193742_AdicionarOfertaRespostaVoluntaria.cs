using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDPW.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarOfertaRespostaVoluntaria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OfertasRespostaVoluntaria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    DataOferta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataPDP = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraInicial = table.Column<TimeSpan>(type: "time", nullable: false),
                    HoraFinal = table.Column<TimeSpan>(type: "time", nullable: false),
                    ReducaoDemandaMW = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecoMWh = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TipoPrograma = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
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
                    table.PrimaryKey("PK_OfertasRespostaVoluntaria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfertasRespostaVoluntaria_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OfertasRespostaVoluntaria_SemanasPMO_SemanaPMOId",
                        column: x => x.SemanaPMOId,
                        principalTable: "SemanasPMO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfertasRespostaVoluntaria_DataPDP",
                table: "OfertasRespostaVoluntaria",
                column: "DataPDP");

            migrationBuilder.CreateIndex(
                name: "IX_OfertasRespostaVoluntaria_EmpresaId_DataPDP",
                table: "OfertasRespostaVoluntaria",
                columns: new[] { "EmpresaId", "DataPDP" });

            migrationBuilder.CreateIndex(
                name: "IX_OfertasRespostaVoluntaria_FlgAprovadoONS",
                table: "OfertasRespostaVoluntaria",
                column: "FlgAprovadoONS");

            migrationBuilder.CreateIndex(
                name: "IX_OfertasRespostaVoluntaria_SemanaPMOId",
                table: "OfertasRespostaVoluntaria",
                column: "SemanaPMOId");

            migrationBuilder.CreateIndex(
                name: "IX_OfertasRespostaVoluntaria_TipoPrograma",
                table: "OfertasRespostaVoluntaria",
                column: "TipoPrograma");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfertasRespostaVoluntaria");
        }
    }
}
