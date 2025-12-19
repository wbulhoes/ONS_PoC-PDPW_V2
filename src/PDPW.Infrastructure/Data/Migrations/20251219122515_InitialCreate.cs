using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PDPW.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Balancos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataReferencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubsistemaId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Geracao = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Carga = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Intercambio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Perdas = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Deficit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balancos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cargas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataReferencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubsistemaId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CargaMWmed = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CargaVerificada = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PrevisaoCarga = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DadosEnergeticos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataReferencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CodigoUsina = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProducaoMWh = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CapacidadeDisponivel = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DadosEnergeticos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Diretorios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Caminho = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DiretorioPaiId = table.Column<int>(type: "int", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diretorios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diretorios_Diretorios_DiretorioPaiId",
                        column: x => x.DiretorioPaiId,
                        principalTable: "Diretorios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CNPJ = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: true),
                    Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Endereco = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EquipesPDP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Coordenador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipesPDP", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Intercambios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataReferencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubsistemaOrigem = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SubsistemaDestino = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    EnergiaIntercambiada = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Intercambios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModalidadesOpTermica",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustoOperacional = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PotenciaMinima = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PotenciaMaxima = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModalidadesOpTermica", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MotivosRestricao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Categoria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotivosRestricao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Observacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataReferencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Conteudo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioAutor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Relatorios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoRelatorio = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataGeracao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CaminhoArquivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Parametros = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioGeracao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relatorios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Responsaveis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cargo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responsaveis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SemanasPMO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemanasPMO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposUsina",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FonteEnergia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposUsina", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Uploads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeArquivo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CaminhoArquivo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    TamanhoBytes = table.Column<long>(type: "bigint", nullable: false),
                    TipoArquivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataUpload = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioUpload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uploads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Arquivos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeArquivo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CaminhoCompleto = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DiretorioId = table.Column<int>(type: "int", nullable: false),
                    TamanhoBytes = table.Column<long>(type: "bigint", nullable: false),
                    TipoArquivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arquivos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Arquivos_Diretorios_DiretorioId",
                        column: x => x.DiretorioId,
                        principalTable: "Diretorios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EquipePDPId = table.Column<int>(type: "int", nullable: true),
                    Perfil = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_EquipesPDP_EquipePDPId",
                        column: x => x.EquipePDPId,
                        principalTable: "EquipesPDP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ArquivosDadger",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeArquivo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CaminhoArquivo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DataImportacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SemanaPMOId = table.Column<int>(type: "int", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Processado = table.Column<bool>(type: "bit", nullable: false),
                    DataProcessamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArquivosDadger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArquivosDadger_SemanasPMO_SemanaPMOId",
                        column: x => x.SemanaPMOId,
                        principalTable: "SemanasPMO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DCAs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataReferencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SemanaPMOId = table.Column<int>(type: "int", nullable: false),
                    DadosConsolidados = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aprovado = table.Column<bool>(type: "bit", nullable: false),
                    DataAprovacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioAprovacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DCAs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DCAs_SemanasPMO_SemanaPMOId",
                        column: x => x.SemanaPMOId,
                        principalTable: "SemanasPMO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usinas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TipoUsinaId = table.Column<int>(type: "int", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    CapacidadeInstalada = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Localizacao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DataOperacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usinas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usinas_Empresas_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usinas_TiposUsina_TipoUsinaId",
                        column: x => x.TipoUsinaId,
                        principalTable: "TiposUsina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ArquivosDadgerValores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArquivoDadgerId = table.Column<int>(type: "int", nullable: false),
                    Chave = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Linha = table.Column<int>(type: "int", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArquivosDadgerValores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArquivosDadgerValores_ArquivosDadger_ArquivoDadgerId",
                        column: x => x.ArquivoDadgerId,
                        principalTable: "ArquivosDadger",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DCRs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataReferencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SemanaPMOId = table.Column<int>(type: "int", nullable: false),
                    DCAId = table.Column<int>(type: "int", nullable: true),
                    DadosRevisados = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotivoRevisao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aprovado = table.Column<bool>(type: "bit", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DCRs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DCRs_DCAs_DCAId",
                        column: x => x.DCAId,
                        principalTable: "DCAs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DCRs_SemanasPMO_SemanaPMOId",
                        column: x => x.SemanaPMOId,
                        principalTable: "SemanasPMO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GeracoesForaMerito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataReferencia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsinaId = table.Column<int>(type: "int", nullable: false),
                    GeracaoMWmed = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeracoesForaMerito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeracoesForaMerito_Usinas_UsinaId",
                        column: x => x.UsinaId,
                        principalTable: "Usinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InflexibilidadesContratadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsinaId = table.Column<int>(type: "int", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GeracaoMinima = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Contrato = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InflexibilidadesContratadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InflexibilidadesContratadas_Usinas_UsinaId",
                        column: x => x.UsinaId,
                        principalTable: "Usinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RampasUsinasTermicas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsinaId = table.Column<int>(type: "int", nullable: false),
                    RampaSubida = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    RampaDescida = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TempoPartida = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TempoParada = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RampasUsinasTermicas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RampasUsinasTermicas_Usinas_UsinaId",
                        column: x => x.UsinaId,
                        principalTable: "Usinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RestricoesUS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsinaId = table.Column<int>(type: "int", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MotivoRestricaoId = table.Column<int>(type: "int", nullable: false),
                    CapacidadeRestrita = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestricoesUS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestricoesUS_MotivosRestricao_MotivoRestricaoId",
                        column: x => x.MotivoRestricaoId,
                        principalTable: "MotivosRestricao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RestricoesUS_Usinas_UsinaId",
                        column: x => x.UsinaId,
                        principalTable: "Usinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UnidadesGeradoras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UsinaId = table.Column<int>(type: "int", nullable: false),
                    PotenciaNominal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PotenciaMinima = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DataComissionamento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadesGeradoras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnidadesGeradoras_Usinas_UsinaId",
                        column: x => x.UsinaId,
                        principalTable: "Usinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsinasConversoras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsinaId = table.Column<int>(type: "int", nullable: false),
                    CapacidadeConversao = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TipoConversao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Eficiencia = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsinasConversoras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsinasConversoras_Usinas_UsinaId",
                        column: x => x.UsinaId,
                        principalTable: "Usinas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParadasUG",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnidadeGeradoraId = table.Column<int>(type: "int", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MotivoParada = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Programada = table.Column<bool>(type: "bit", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParadasUG", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParadasUG_UnidadesGeradoras_UnidadeGeradoraId",
                        column: x => x.UnidadeGeradoraId,
                        principalTable: "UnidadesGeradoras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RestricoesUG",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnidadeGeradoraId = table.Column<int>(type: "int", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MotivoRestricaoId = table.Column<int>(type: "int", nullable: false),
                    PotenciaRestrita = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestricoesUG", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RestricoesUG_MotivosRestricao_MotivoRestricaoId",
                        column: x => x.MotivoRestricaoId,
                        principalTable: "MotivosRestricao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RestricoesUG_UnidadesGeradoras_UnidadeGeradoraId",
                        column: x => x.UnidadeGeradoraId,
                        principalTable: "UnidadesGeradoras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Arquivos_DiretorioId",
                table: "Arquivos",
                column: "DiretorioId");

            migrationBuilder.CreateIndex(
                name: "IX_ArquivosDadger_SemanaPMOId",
                table: "ArquivosDadger",
                column: "SemanaPMOId");

            migrationBuilder.CreateIndex(
                name: "IX_ArquivosDadgerValores_ArquivoDadgerId",
                table: "ArquivosDadgerValores",
                column: "ArquivoDadgerId");

            migrationBuilder.CreateIndex(
                name: "IX_ArquivosDadgerValores_Chave",
                table: "ArquivosDadgerValores",
                column: "Chave");

            migrationBuilder.CreateIndex(
                name: "IX_Balancos_DataReferencia_SubsistemaId",
                table: "Balancos",
                columns: new[] { "DataReferencia", "SubsistemaId" });

            migrationBuilder.CreateIndex(
                name: "IX_Cargas_DataReferencia_SubsistemaId",
                table: "Cargas",
                columns: new[] { "DataReferencia", "SubsistemaId" });

            migrationBuilder.CreateIndex(
                name: "IX_DadosEnergeticos_DataReferencia",
                table: "DadosEnergeticos",
                column: "DataReferencia");

            migrationBuilder.CreateIndex(
                name: "IX_DCAs_SemanaPMOId",
                table: "DCAs",
                column: "SemanaPMOId");

            migrationBuilder.CreateIndex(
                name: "IX_DCRs_DCAId",
                table: "DCRs",
                column: "DCAId");

            migrationBuilder.CreateIndex(
                name: "IX_DCRs_SemanaPMOId",
                table: "DCRs",
                column: "SemanaPMOId");

            migrationBuilder.CreateIndex(
                name: "IX_Diretorios_DiretorioPaiId",
                table: "Diretorios",
                column: "DiretorioPaiId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_CNPJ",
                table: "Empresas",
                column: "CNPJ",
                unique: true,
                filter: "[CNPJ] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_Nome",
                table: "Empresas",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_GeracoesForaMerito_UsinaId",
                table: "GeracoesForaMerito",
                column: "UsinaId");

            migrationBuilder.CreateIndex(
                name: "IX_InflexibilidadesContratadas_UsinaId",
                table: "InflexibilidadesContratadas",
                column: "UsinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Intercambios_DataReferencia_SubsistemaOrigem_SubsistemaDestino",
                table: "Intercambios",
                columns: new[] { "DataReferencia", "SubsistemaOrigem", "SubsistemaDestino" });

            migrationBuilder.CreateIndex(
                name: "IX_ParadasUG_UnidadeGeradoraId",
                table: "ParadasUG",
                column: "UnidadeGeradoraId");

            migrationBuilder.CreateIndex(
                name: "IX_RampasUsinasTermicas_UsinaId",
                table: "RampasUsinasTermicas",
                column: "UsinaId");

            migrationBuilder.CreateIndex(
                name: "IX_RestricoesUG_MotivoRestricaoId",
                table: "RestricoesUG",
                column: "MotivoRestricaoId");

            migrationBuilder.CreateIndex(
                name: "IX_RestricoesUG_UnidadeGeradoraId",
                table: "RestricoesUG",
                column: "UnidadeGeradoraId");

            migrationBuilder.CreateIndex(
                name: "IX_RestricoesUS_MotivoRestricaoId",
                table: "RestricoesUS",
                column: "MotivoRestricaoId");

            migrationBuilder.CreateIndex(
                name: "IX_RestricoesUS_UsinaId",
                table: "RestricoesUS",
                column: "UsinaId");

            migrationBuilder.CreateIndex(
                name: "IX_SemanasPMO_Numero_Ano",
                table: "SemanasPMO",
                columns: new[] { "Numero", "Ano" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TiposUsina_Nome",
                table: "TiposUsina",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_UnidadesGeradoras_Codigo",
                table: "UnidadesGeradoras",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnidadesGeradoras_UsinaId",
                table: "UnidadesGeradoras",
                column: "UsinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usinas_Codigo",
                table: "Usinas",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usinas_EmpresaId",
                table: "Usinas",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usinas_Nome",
                table: "Usinas",
                column: "Nome");

            migrationBuilder.CreateIndex(
                name: "IX_Usinas_TipoUsinaId",
                table: "Usinas",
                column: "TipoUsinaId");

            migrationBuilder.CreateIndex(
                name: "IX_UsinasConversoras_UsinaId",
                table: "UsinasConversoras",
                column: "UsinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_EquipePDPId",
                table: "Usuarios",
                column: "EquipePDPId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Arquivos");

            migrationBuilder.DropTable(
                name: "ArquivosDadgerValores");

            migrationBuilder.DropTable(
                name: "Balancos");

            migrationBuilder.DropTable(
                name: "Cargas");

            migrationBuilder.DropTable(
                name: "DadosEnergeticos");

            migrationBuilder.DropTable(
                name: "DCRs");

            migrationBuilder.DropTable(
                name: "GeracoesForaMerito");

            migrationBuilder.DropTable(
                name: "InflexibilidadesContratadas");

            migrationBuilder.DropTable(
                name: "Intercambios");

            migrationBuilder.DropTable(
                name: "ModalidadesOpTermica");

            migrationBuilder.DropTable(
                name: "Observacoes");

            migrationBuilder.DropTable(
                name: "ParadasUG");

            migrationBuilder.DropTable(
                name: "RampasUsinasTermicas");

            migrationBuilder.DropTable(
                name: "Relatorios");

            migrationBuilder.DropTable(
                name: "Responsaveis");

            migrationBuilder.DropTable(
                name: "RestricoesUG");

            migrationBuilder.DropTable(
                name: "RestricoesUS");

            migrationBuilder.DropTable(
                name: "Uploads");

            migrationBuilder.DropTable(
                name: "UsinasConversoras");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Diretorios");

            migrationBuilder.DropTable(
                name: "ArquivosDadger");

            migrationBuilder.DropTable(
                name: "DCAs");

            migrationBuilder.DropTable(
                name: "UnidadesGeradoras");

            migrationBuilder.DropTable(
                name: "MotivosRestricao");

            migrationBuilder.DropTable(
                name: "EquipesPDP");

            migrationBuilder.DropTable(
                name: "SemanasPMO");

            migrationBuilder.DropTable(
                name: "Usinas");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "TiposUsina");
        }
    }
}
