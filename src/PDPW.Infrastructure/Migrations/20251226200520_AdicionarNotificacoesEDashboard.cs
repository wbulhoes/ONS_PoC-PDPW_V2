using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDPW.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarNotificacoesEDashboard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MetricasDashboard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataHoraReferencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NomeMetrica = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unidade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntidadeId = table.Column<int>(type: "int", nullable: true),
                    TipoEntidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Meta = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PercentualMeta = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    Tendencia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetricasDashboard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notificacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DestinatarioId = table.Column<int>(type: "int", nullable: true),
                    TipoDestinatario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TipoNotificacao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Mensagem = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Prioridade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataHoraEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataHoraLeitura = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Lida = table.Column<bool>(type: "bit", nullable: false),
                    DataVencimento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LinkAcao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TextoAcao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EntidadeRelacionadaId = table.Column<int>(type: "int", nullable: true),
                    TipoEntidadeRelacionada = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MetricasDashboard_Categoria",
                table: "MetricasDashboard",
                column: "Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_MetricasDashboard_Categoria_DataHoraReferencia",
                table: "MetricasDashboard",
                columns: new[] { "Categoria", "DataHoraReferencia" });

            migrationBuilder.CreateIndex(
                name: "IX_MetricasDashboard_DataHoraReferencia",
                table: "MetricasDashboard",
                column: "DataHoraReferencia");

            migrationBuilder.CreateIndex(
                name: "IX_MetricasDashboard_NomeMetrica",
                table: "MetricasDashboard",
                column: "NomeMetrica");

            migrationBuilder.CreateIndex(
                name: "IX_MetricasDashboard_Status",
                table: "MetricasDashboard",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_Categoria",
                table: "Notificacoes",
                column: "Categoria");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_DataHoraEnvio",
                table: "Notificacoes",
                column: "DataHoraEnvio");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_DestinatarioId",
                table: "Notificacoes",
                column: "DestinatarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_DestinatarioId_Lida",
                table: "Notificacoes",
                columns: new[] { "DestinatarioId", "Lida" });

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_Lida",
                table: "Notificacoes",
                column: "Lida");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_Prioridade",
                table: "Notificacoes",
                column: "Prioridade");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_TipoDestinatario",
                table: "Notificacoes",
                column: "TipoDestinatario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MetricasDashboard");

            migrationBuilder.DropTable(
                name: "Notificacoes");
        }
    }
}
