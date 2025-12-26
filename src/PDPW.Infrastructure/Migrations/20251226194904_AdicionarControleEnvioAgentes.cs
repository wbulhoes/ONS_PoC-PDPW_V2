using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDPW.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarControleEnvioAgentes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JanelasEnvioAgente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoDado = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DataReferencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataHoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataHoraFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SemanaPMOId = table.Column<int>(type: "int", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PermiteEnvioForaJanela = table.Column<bool>(type: "bit", nullable: false),
                    UsuarioAutorizacaoExcecao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DataHoraAutorizacaoExcecao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JanelasEnvioAgente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JanelasEnvioAgente_SemanasPMO_SemanaPMOId",
                        column: x => x.SemanaPMOId,
                        principalTable: "SemanasPMO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SubmissoesAgente",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    TipoDado = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RegistroId = table.Column<int>(type: "int", nullable: false),
                    DataReferencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataHoraSubmissao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioEnvio = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IpOrigem = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DentroJanela = table.Column<bool>(type: "bit", nullable: false),
                    JanelaEnvioId = table.Column<int>(type: "int", nullable: true),
                    StatusSubmissao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MotivoRejeicao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    HashDados = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubmissoesAgente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubmissoesAgente_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubmissoesAgente_JanelasEnvioAgente_JanelaEnvioId",
                        column: x => x.JanelaEnvioId,
                        principalTable: "JanelasEnvioAgente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JanelasEnvioAgente_DataReferencia",
                table: "JanelasEnvioAgente",
                column: "DataReferencia");

            migrationBuilder.CreateIndex(
                name: "IX_JanelasEnvioAgente_SemanaPMOId",
                table: "JanelasEnvioAgente",
                column: "SemanaPMOId");

            migrationBuilder.CreateIndex(
                name: "IX_JanelasEnvioAgente_Status",
                table: "JanelasEnvioAgente",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_JanelasEnvioAgente_TipoDado",
                table: "JanelasEnvioAgente",
                column: "TipoDado");

            migrationBuilder.CreateIndex(
                name: "IX_JanelasEnvioAgente_TipoDado_DataReferencia",
                table: "JanelasEnvioAgente",
                columns: new[] { "TipoDado", "DataReferencia" });

            migrationBuilder.CreateIndex(
                name: "IX_SubmissoesAgente_DataHoraSubmissao",
                table: "SubmissoesAgente",
                column: "DataHoraSubmissao");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissoesAgente_DataReferencia",
                table: "SubmissoesAgente",
                column: "DataReferencia");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissoesAgente_EmpresaId",
                table: "SubmissoesAgente",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissoesAgente_EmpresaId_TipoDado_DataReferencia",
                table: "SubmissoesAgente",
                columns: new[] { "EmpresaId", "TipoDado", "DataReferencia" });

            migrationBuilder.CreateIndex(
                name: "IX_SubmissoesAgente_HashDados",
                table: "SubmissoesAgente",
                column: "HashDados");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissoesAgente_JanelaEnvioId",
                table: "SubmissoesAgente",
                column: "JanelaEnvioId");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissoesAgente_StatusSubmissao",
                table: "SubmissoesAgente",
                column: "StatusSubmissao");

            migrationBuilder.CreateIndex(
                name: "IX_SubmissoesAgente_TipoDado",
                table: "SubmissoesAgente",
                column: "TipoDado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubmissoesAgente");

            migrationBuilder.DropTable(
                name: "JanelasEnvioAgente");
        }
    }
}
