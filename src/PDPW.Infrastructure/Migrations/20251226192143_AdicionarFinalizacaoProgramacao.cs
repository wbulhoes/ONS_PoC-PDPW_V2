using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDPW.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarFinalizacaoProgramacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataAprovacao",
                table: "ArquivosDadger",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFinalizacao",
                table: "ArquivosDadger",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObservacaoAprovacao",
                table: "ArquivosDadger",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObservacaoFinalizacao",
                table: "ArquivosDadger",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ArquivosDadger",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioAprovacao",
                table: "ArquivosDadger",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioFinalizacao",
                table: "ArquivosDadger",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });

            migrationBuilder.UpdateData(
                table: "ArquivosDadger",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "DataAprovacao", "DataFinalizacao", "ObservacaoAprovacao", "ObservacaoFinalizacao", "Status", "UsuarioAprovacao", "UsuarioFinalizacao" },
                values: new object[] { null, null, null, null, "Aberto", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataAprovacao",
                table: "ArquivosDadger");

            migrationBuilder.DropColumn(
                name: "DataFinalizacao",
                table: "ArquivosDadger");

            migrationBuilder.DropColumn(
                name: "ObservacaoAprovacao",
                table: "ArquivosDadger");

            migrationBuilder.DropColumn(
                name: "ObservacaoFinalizacao",
                table: "ArquivosDadger");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ArquivosDadger");

            migrationBuilder.DropColumn(
                name: "UsuarioAprovacao",
                table: "ArquivosDadger");

            migrationBuilder.DropColumn(
                name: "UsuarioFinalizacao",
                table: "ArquivosDadger");
        }
    }
}
