using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PDPW.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Empresas",
                columns: new[] { "Id", "Ativo", "CNPJ", "DataAtualizacao", "DataCriacao", "Email", "Endereco", "Nome", "Telefone" },
                values: new object[,]
                {
                    { 1, true, "00.341.583/0001-71", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "contato@itaipu.gov.br", "Av. Tancredo Neves, 6731 - Foz do Iguaçu, PR", "Itaipu Binacional", "(45) 3520-5252" },
                    { 2, true, "00.357.038/0001-16", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "contato@eletronorte.gov.br", "SCN - Quadra 6 - Conjunto A - Bloco C - Brasília, DF", "Eletronorte - Centrais Elétricas do Norte do Brasil", "(61) 3429-5151" },
                    { 3, true, "23.047.150/0001-13", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ouvidoria@furnas.com.br", "Rua Real Grandeza, 219 - Rio de Janeiro, RJ", "Furnas Centrais Elétricas", "(21) 2528-5600" },
                    { 4, true, "33.541.368/0001-16", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "faleconosco@chesf.gov.br", "Rua Delmiro Gouveia, 333 - Recife, PE", "Chesf - Companhia Hidro Elétrica do São Francisco", "(81) 3229-2300" },
                    { 5, true, "00.073.957/0001-46", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "comunicacao@eletrosul.gov.br", "Av. Prefeito Osmar Cunha, 183 - Florianópolis, SC", "Eletrosul Centrais Elétricas", "(48) 3231-7000" },
                    { 6, true, "60.933.603/0001-78", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ouvidoria@cesp.com.br", "Av. Nossa Senhora do Sabará, 5312 - São Paulo, SP", "CESP - Companhia Energética de São Paulo", "(11) 3138-7000" },
                    { 7, true, "42.540.211/0001-67", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "eletronuclear@eletronuclear.gov.br", "Rua da Candelária, 65 - Rio de Janeiro, RJ", "Eletronuclear - Eletrobrás Termonuclear", "(21) 2588-1000" },
                    { 8, true, "76.483.817/0001-20", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "copel@copel.com", "Rua José Izidoro Biazetto, 158 - Curitiba, PR", "COPEL - Companhia Paranaense de Energia", "(41) 3331-4011" }
                });

            migrationBuilder.InsertData(
                table: "TiposUsina",
                columns: new[] { "Id", "Ativo", "DataAtualizacao", "DataCriacao", "Descricao", "FonteEnergia", "Nome" },
                values: new object[,]
                {
                    { 1, true, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Usina que gera energia através da força da água", "Hídrica", "Hidrelétrica" },
                    { 2, true, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Usina que gera energia através da queima de combustíveis", "Combustíveis Fósseis / Biomassa", "Térmica" },
                    { 3, true, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Usina que gera energia através da força dos ventos", "Eólica", "Eólica" },
                    { 4, true, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Usina que gera energia através da luz solar", "Solar", "Solar" },
                    { 5, true, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Usina que gera energia através da fissão nuclear", "Nuclear", "Nuclear" }
                });

            migrationBuilder.InsertData(
                table: "Usinas",
                columns: new[] { "Id", "Ativo", "CapacidadeInstalada", "Codigo", "DataAtualizacao", "DataCriacao", "DataOperacao", "EmpresaId", "Localizacao", "Nome", "TipoUsinaId" },
                values: new object[,]
                {
                    { 1, true, 14000.00m, "UHE-ITAIPU", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Foz do Iguaçu, PR - Fronteira Brasil/Paraguai", "Usina Hidrelétrica de Itaipu", 1 },
                    { 2, true, 11233.00m, "UHE-BELO-MONTE", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2016, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Altamira, PA - Rio Xingu", "Usina Hidrelétrica Belo Monte", 1 },
                    { 3, true, 8370.00m, "UHE-TUCURUI", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1984, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Tucuruí, PA - Rio Tocantins", "Usina Hidrelétrica de Tucuruí", 1 },
                    { 4, true, 1710.00m, "UHE-SAO-SIMAO", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1978, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "São Simão, GO - Rio Paranaíba", "Usina Hidrelétrica de São Simão", 1 },
                    { 5, true, 1050.40m, "UHE-SOBRADINHO", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1979, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Sobradinho, BA - Rio São Francisco", "Usina Hidrelétrica de Sobradinho", 1 },
                    { 6, true, 2082.00m, "UHE-ITUMBIARA", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1980, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Itumbiara, GO - Rio Paranaíba", "Usina Hidrelétrica de Itumbiara", 1 },
                    { 7, true, 338.00m, "UTE-TERMO-MARANHAO", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2013, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Miranda do Norte, MA", "Usina Termelétrica do Maranhão", 2 },
                    { 8, true, 720.00m, "UTE-TERMO-PECEM", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2012, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "São Gonçalo do Amarante, CE", "Usina Termelétrica de Pecém", 2 },
                    { 9, true, 640.00m, "UTN-ANGRA-I", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1985, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "Angra dos Reis, RJ", "Usina Termonuclear Angra I", 5 },
                    { 10, true, 1350.00m, "UTN-ANGRA-II", null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2001, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, "Angra dos Reis, RJ", "Usina Termonuclear Angra II", 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Empresas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Empresas",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Empresas",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "TiposUsina",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TiposUsina",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Usinas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Usinas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Usinas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Usinas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Usinas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Usinas",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Usinas",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Usinas",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Usinas",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Usinas",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Empresas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Empresas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Empresas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Empresas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Empresas",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "TiposUsina",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TiposUsina",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TiposUsina",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
