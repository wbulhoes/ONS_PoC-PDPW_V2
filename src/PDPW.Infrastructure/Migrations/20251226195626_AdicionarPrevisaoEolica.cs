using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDPW.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarPrevisaoEolica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrevisoesEolicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsinaId = table.Column<int>(type: "int", nullable: false),
                    DataHoraReferencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataHoraPrevista = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GeracaoPrevistaMWmed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VelocidadeVentoMS = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    DirecaoVentoGraus = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    TemperaturaC = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    PressaoAtmosfericaHPa = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    UmidadeRelativa = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ModeloPrevisao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VersaoModelo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HorizontePrevisaoHoras = table.Column<int>(type: "int", nullable: false),
                    IncertezaPercentual = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    LimiteInferiorMW = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LimiteSuperiorMW = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TipoPrevisao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SemanaPMOId = table.Column<int>(type: "int", nullable: true),
                    GeracaoRealMWmed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ErroAbsolutoMW = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ErroPercentual = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrevisoesEolicas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrevisoesEolicas_SemanasPMO_SemanaPMOId",
                        column: x => x.SemanaPMOId,
                        principalTable: "SemanasPMO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PrevisoesEolicas_Usinas_UsinaId",
                        column: x => x.UsinaId,
                        principalTable: "Usinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrevisoesEolicas_DataHoraPrevista",
                table: "PrevisoesEolicas",
                column: "DataHoraPrevista");

            migrationBuilder.CreateIndex(
                name: "IX_PrevisoesEolicas_DataHoraReferencia",
                table: "PrevisoesEolicas",
                column: "DataHoraReferencia");

            migrationBuilder.CreateIndex(
                name: "IX_PrevisoesEolicas_ModeloPrevisao",
                table: "PrevisoesEolicas",
                column: "ModeloPrevisao");

            migrationBuilder.CreateIndex(
                name: "IX_PrevisoesEolicas_SemanaPMOId",
                table: "PrevisoesEolicas",
                column: "SemanaPMOId");

            migrationBuilder.CreateIndex(
                name: "IX_PrevisoesEolicas_TipoPrevisao",
                table: "PrevisoesEolicas",
                column: "TipoPrevisao");

            migrationBuilder.CreateIndex(
                name: "IX_PrevisoesEolicas_UsinaId",
                table: "PrevisoesEolicas",
                column: "UsinaId");

            migrationBuilder.CreateIndex(
                name: "IX_PrevisoesEolicas_UsinaId_DataHoraPrevista",
                table: "PrevisoesEolicas",
                columns: new[] { "UsinaId", "DataHoraPrevista" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrevisoesEolicas");
        }
    }
}
