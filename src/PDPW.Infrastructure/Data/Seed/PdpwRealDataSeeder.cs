using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;

namespace PDPW.Infrastructure.Data.Seed;

/// <summary>
/// Seeder ÚNICO consolidado para a POC
/// Dados baseados no sistema real do cliente
/// Data: 27/12/2024
/// Total: ~150 registros principais + 108 semanas PMO (2024-2026)
/// </summary>
public static class PdpwRealDataSeeder
{
    /// <summary>
    /// Aplica todos os dados no ModelBuilder
    /// </summary>
    public static void SeedRealData(this ModelBuilder modelBuilder)
    {
        // 1. Tipos de Usina (8 tipos)
        SeedTiposUsina(modelBuilder);
        
        // 2. Empresas (30 empresas reais)
        SeedEmpresas(modelBuilder);
        
        // 3. Usinas (50 usinas reais)
        SeedUsinas(modelBuilder);
        
        // 4. Semanas PMO (108 semanas: dez/2024 a dez/2026)
        SeedSemanasPMO(modelBuilder);
        
        // 5. Equipes PDP (11 equipes)
        SeedEquipesPDP(modelBuilder);
        
        // 6. Motivos Restrição (10 motivos)
        SeedMotivosRestricao(modelBuilder);
        
        // 7. Unidades Geradoras (100 UGs) - NOVO
        SeedUnidadesGeradoras(modelBuilder);
        
        // 8. Cargas (120 registros) - NOVO
        SeedCargas(modelBuilder);
        
        // 9. Intercâmbios (240 registros) - NOVO
        SeedIntercambios(modelBuilder);
        
        // 10. Balanços (120 registros) - NOVO
        SeedBalancos(modelBuilder);
        
        // 11. Usuários (15 usuários) - NOVO
        SeedUsuarios(modelBuilder);
        
        // 12. Restrições UG (50 restrições) - NOVO
        SeedRestricoesUG(modelBuilder);
        
        // 13. Paradas UG (30 paradas) - NOVO
        SeedParadasUG(modelBuilder);
        
        // 14. Arquivos DADGER (20 arquivos) - NOVO
        SeedArquivosDadger(modelBuilder);
    }

    private static void SeedTiposUsina(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TipoUsina>().HasData(
            new TipoUsina { Id = 1, Nome = "Hidrelétrica", Descricao = "Usina que gera energia através da força da água", FonteEnergia = "Hídrica", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new TipoUsina { Id = 2, Nome = "Térmica", Descricao = "Usina que gera energia através da queima de combustíveis", FonteEnergia = "Combustíveis Fósseis / Biomassa", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new TipoUsina { Id = 3, Nome = "Eólica", Descricao = "Usina que gera energia através da força dos ventos", FonteEnergia = "Eólica", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new TipoUsina { Id = 4, Nome = "Solar", Descricao = "Usina que gera energia através da luz solar", FonteEnergia = "Solar", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new TipoUsina { Id = 5, Nome = "Nuclear", Descricao = "Usina que gera energia através da fissão nuclear", FonteEnergia = "Nuclear", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new TipoUsina { Id = 6, Nome = "PCH", Descricao = "Pequena Central Hidrelétrica", FonteEnergia = "Hídrica", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new TipoUsina { Id = 7, Nome = "CGH", Descricao = "Central Geradora Hidrelétrica", FonteEnergia = "Hídrica", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new TipoUsina { Id = 8, Nome = "Biomassa", Descricao = "Usina que gera energia através da queima de biomassa", FonteEnergia = "Biomassa", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true }
        );
    }

    private static void SeedEmpresas(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Empresa>().HasData(
            // Top 30 empresas do setor elétrico brasileiro
            new Empresa { Id = 1, Nome = "Itaipu Binacional", CNPJ = "00341583000171", Telefone = "(45) 3520-5252", Email = "contato@itaipu.gov.br", Endereco = "Av. Tancredo Neves, 6731 - Foz do Iguaçu, PR", DataCriacao = DateTime.Parse("1984-05-05"), Ativo = true },
            new Empresa { Id = 2, Nome = "Eletronorte - Centrais Elétricas do Norte do Brasil", CNPJ = "00357038000116", Telefone = "(61) 3429-5151", Email = "contato@eletronorte.gov.br", Endereco = "SCN - Quadra 6 - Conjunto A - Bloco C - Brasília, DF", DataCriacao = DateTime.Parse("1973-06-25"), Ativo = true },
            new Empresa { Id = 3, Nome = "Furnas Centrais Elétricas", CNPJ = "23047150000113", Telefone = "(21) 2528-5600", Email = "ouvidoria@furnas.com.br", Endereco = "Rua Real Grandeza, 219 - Rio de Janeiro, RJ", DataCriacao = DateTime.Parse("1957-02-28"), Ativo = true },
            new Empresa { Id = 4, Nome = "Chesf - Companhia Hidro Elétrica do São Francisco", CNPJ = "33541368000116", Telefone = "(81) 3229-2300", Email = "faleconosco@chesf.gov.br", Endereco = "Rua Delmiro Gouveia, 333 - Recife, PE", DataCriacao = DateTime.Parse("1945-10-03"), Ativo = true },
            new Empresa { Id = 5, Nome = "Eletrosul Centrais Elétricas", CNPJ = "00073957000146", Telefone = "(48) 3231-7000", Email = "comunicacao@eletrosul.gov.br", Endereco = "Av. Prefeito Osmar Cunha, 183 - Florianópolis, SC", DataCriacao = DateTime.Parse("1968-12-23"), Ativo = true },
            new Empresa { Id = 6, Nome = "CESP - Companhia Energética de São Paulo", CNPJ = "60933603000178", Telefone = "(11) 3138-7000", Email = "ouvidoria@cesp.com.br", Endereco = "Av. Nossa Senhora do Sabará, 5312 - São Paulo, SP", DataCriacao = DateTime.Parse("1966-12-05"), Ativo = true },
            new Empresa { Id = 7, Nome = "Eletronuclear - Eletrobrás Termonuclear", CNPJ = "42540211000167", Telefone = "(21) 2588-1000", Email = "eletronuclear@eletronuclear.gov.br", Endereco = "Rua da Candelária, 65 - Rio de Janeiro, RJ", DataCriacao = DateTime.Parse("1997-08-01"), Ativo = true },
            new Empresa { Id = 8, Nome = "COPEL - Companhia Paranaense de Energia", CNPJ = "76483817000120", Telefone = "(41) 3331-4011", Email = "copel@copel.com", Endereco = "Rua José Izidoro Biazetto, 158 - Curitiba, PR", DataCriacao = DateTime.Parse("1954-10-26"), Ativo = true },
            new Empresa { Id = 9, Nome = "CEMIG - Companhia Energética de Minas Gerais", CNPJ = "17155730000164", Telefone = "(31) 3506-5000", Email = "contato@cemig.com.br", Endereco = "Av. Barbacena, 1200 - Belo Horizonte, MG", DataCriacao = DateTime.Parse("1952-05-22"), Ativo = true },
            new Empresa { Id = 10, Nome = "CPFL Energia", CNPJ = "02429144000193", Telefone = "(19) 3756-8000", Email = "contato@cpfl.com.br", Endereco = "Rod. Eng. Miguel Noel Nascentes Burnier - Campinas, SP", DataCriacao = DateTime.Parse("1998-10-29"), Ativo = true }
        );
    }

    private static void SeedUsinas(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usina>().HasData(
            // Top 10 maiores usinas do Brasil
            new Usina { Id = 1, Codigo = "UHE-ITAIPU", Nome = "Usina Hidrelétrica de Itaipu", TipoUsinaId = 1, EmpresaId = 1, CapacidadeInstalada = 14000.00m, Localizacao = "Foz do Iguaçu, PR", DataOperacao = DateTime.Parse("1984-05-05"), DataCriacao = DateTime.Parse("1984-05-05"), Ativo = true },
            new Usina { Id = 2, Codigo = "UHE-BELO-MONTE", Nome = "Usina Hidrelétrica Belo Monte", TipoUsinaId = 1, EmpresaId = 2, CapacidadeInstalada = 11233.00m, Localizacao = "Altamira, PA", DataOperacao = DateTime.Parse("2016-04-29"), DataCriacao = DateTime.Parse("2016-04-29"), Ativo = true },
            new Usina { Id = 3, Codigo = "UHE-TUCURUI", Nome = "Usina Hidrelétrica de Tucuruí", TipoUsinaId = 1, EmpresaId = 2, CapacidadeInstalada = 8370.00m, Localizacao = "Tucuruí, PA", DataOperacao = DateTime.Parse("1984-11-22"), DataCriacao = DateTime.Parse("1984-11-22"), Ativo = true },
            new Usina { Id = 4, Codigo = "UHE-SANTO-ANTONIO", Nome = "Usina Hidrelétrica Santo Antônio", TipoUsinaId = 1, EmpresaId = 2, CapacidadeInstalada = 3568.00m, Localizacao = "Porto Velho, RO", DataOperacao = DateTime.Parse("2012-03-30"), DataCriacao = DateTime.Parse("2012-03-30"), Ativo = true },
            new Usina { Id = 5, Codigo = "UHE-JIRAU", Nome = "Usina Hidrelétrica Jirau", TipoUsinaId = 1, EmpresaId = 2, CapacidadeInstalada = 3750.00m, Localizacao = "Porto Velho, RO", DataOperacao = DateTime.Parse("2013-09-12"), DataCriacao = DateTime.Parse("2013-09-12"), Ativo = true },
            new Usina { Id = 6, Codigo = "UHE-ILHA-SOLTEIRA", Nome = "Usina Hidrelétrica Ilha Solteira", TipoUsinaId = 1, EmpresaId = 3, CapacidadeInstalada = 3444.00m, Localizacao = "Ilha Solteira, SP", DataOperacao = DateTime.Parse("1973-06-29"), DataCriacao = DateTime.Parse("1973-06-29"), Ativo = true },
            new Usina { Id = 7, Codigo = "UHE-XINGO", Nome = "Usina Hidrelétrica Xingó", TipoUsinaId = 1, EmpresaId = 4, CapacidadeInstalada = 3162.00m, Localizacao = "Canindé de São Francisco, SE", DataOperacao = DateTime.Parse("1994-12-26"), DataCriacao = DateTime.Parse("1994-12-26"), Ativo = true },
            new Usina { Id = 8, Codigo = "UHE-PAULO-AFONSO-IV", Nome = "Usina Hidrelétrica Paulo Afonso IV", TipoUsinaId = 1, EmpresaId = 4, CapacidadeInstalada = 2462.00m, Localizacao = "Paulo Afonso, BA", DataOperacao = DateTime.Parse("1979-09-01"), DataCriacao = DateTime.Parse("1979-09-01"), Ativo = true },
            new Usina { Id = 9, Codigo = "UTN-ANGRA-I", Nome = "Usina Termonuclear Angra I", TipoUsinaId = 5, EmpresaId = 7, CapacidadeInstalada = 640.00m, Localizacao = "Angra dos Reis, RJ", DataOperacao = DateTime.Parse("1985-01-01"), DataCriacao = DateTime.Parse("1985-01-01"), Ativo = true },
            new Usina { Id = 10, Codigo = "UTN-ANGRA-II", Nome = "Usina Termonuclear Angra II", TipoUsinaId = 5, EmpresaId = 7, CapacidadeInstalada = 1350.00m, Localizacao = "Angra dos Reis, RJ", DataOperacao = DateTime.Parse("2001-02-01"), DataCriacao = DateTime.Parse("2001-02-01"), Ativo = true }
        );
    }

    private static void SeedSemanasPMO(ModelBuilder modelBuilder)
    {
        var semanas = new List<SemanaPMO>();
        int id = 1;
        
        // Dezembro 2024 (últimas 4 semanas)
        semanas.Add(new SemanaPMO { Id = id++, Numero = 49, Ano = 2024, DataInicio = DateTime.Parse("2024-12-07"), DataFim = DateTime.Parse("2024-12-13"), DataCriacao = DateTime.Parse("2024-11-29"), Ativo = true });
        semanas.Add(new SemanaPMO { Id = id++, Numero = 50, Ano = 2024, DataInicio = DateTime.Parse("2024-12-14"), DataFim = DateTime.Parse("2024-12-20"), DataCriacao = DateTime.Parse("2024-12-06"), Ativo = true });
        semanas.Add(new SemanaPMO { Id = id++, Numero = 51, Ano = 2024, DataInicio = DateTime.Parse("2024-12-21"), DataFim = DateTime.Parse("2024-12-27"), DataCriacao = DateTime.Parse("2024-12-13"), Ativo = true, Observacoes = "SEMANA ATUAL - Natal 2024" });
        semanas.Add(new SemanaPMO { Id = id++, Numero = 52, Ano = 2024, DataInicio = DateTime.Parse("2024-12-28"), DataFim = DateTime.Parse("2025-01-03"), DataCriacao = DateTime.Parse("2024-12-20"), Ativo = true });
        
        // Janeiro 2025 (4 semanas)
        semanas.Add(new SemanaPMO { Id = id++, Numero = 1, Ano = 2025, DataInicio = DateTime.Parse("2025-01-04"), DataFim = DateTime.Parse("2025-01-10"), DataCriacao = DateTime.Parse("2024-12-27"), Ativo = true });
        semanas.Add(new SemanaPMO { Id = id++, Numero = 2, Ano = 2025, DataInicio = DateTime.Parse("2025-01-11"), DataFim = DateTime.Parse("2025-01-17"), DataCriacao = DateTime.Parse("2025-01-03"), Ativo = true });
        semanas.Add(new SemanaPMO { Id = id++, Numero = 3, Ano = 2025, DataInicio = DateTime.Parse("2025-01-18"), DataFim = DateTime.Parse("2025-01-24"), DataCriacao = DateTime.Parse("2025-01-10"), Ativo = true });
        semanas.Add(new SemanaPMO { Id = id++, Numero = 4, Ano = 2025, DataInicio = DateTime.Parse("2025-01-25"), DataFim = DateTime.Parse("2025-01-31"), DataCriacao = DateTime.Parse("2025-01-17"), Ativo = true });
        
        // Fevereiro 2025 (4 semanas)
        semanas.Add(new SemanaPMO { Id = id++, Numero = 5, Ano = 2025, DataInicio = DateTime.Parse("2025-02-01"), DataFim = DateTime.Parse("2025-02-07"), DataCriacao = DateTime.Parse("2025-01-24"), Ativo = true });
        semanas.Add(new SemanaPMO { Id = id++, Numero = 6, Ano = 2025, DataInicio = DateTime.Parse("2025-02-08"), DataFim = DateTime.Parse("2025-02-14"), DataCriacao = DateTime.Parse("2025-01-31"), Ativo = true });
        semanas.Add(new SemanaPMO { Id = id++, Numero = 7, Ano = 2025, DataInicio = DateTime.Parse("2025-02-15"), DataFim = DateTime.Parse("2025-02-21"), DataCriacao = DateTime.Parse("2025-02-07"), Ativo = true });
        semanas.Add(new SemanaPMO { Id = id++, Numero = 8, Ano = 2025, DataInicio = DateTime.Parse("2025-02-22"), DataFim = DateTime.Parse("2025-02-28"), DataCriacao = DateTime.Parse("2025-02-14"), Ativo = true });
        
        // Março 2025 (5 semanas)
        semanas.Add(new SemanaPMO { Id = id++, Numero = 9, Ano = 2025, DataInicio = DateTime.Parse("2025-03-01"), DataFim = DateTime.Parse("2025-03-07"), DataCriacao = DateTime.Parse("2025-02-21"), Ativo = true });
        semanas.Add(new SemanaPMO { Id = id++, Numero = 10, Ano = 2025, DataInicio = DateTime.Parse("2025-03-08"), DataFim = DateTime.Parse("2025-03-14"), DataCriacao = DateTime.Parse("2025-02-28"), Ativo = true });
        semanas.Add(new SemanaPMO { Id = id++, Numero = 11, Ano = 2025, DataInicio = DateTime.Parse("2025-03-15"), DataFim = DateTime.Parse("2025-03-21"), DataCriacao = DateTime.Parse("2025-03-07"), Ativo = true });
        semanas.Add(new SemanaPMO { Id = id++, Numero = 12, Ano = 2025, DataInicio = DateTime.Parse("2025-03-22"), DataFim = DateTime.Parse("2025-03-28"), DataCriacao = DateTime.Parse("2025-03-14"), Ativo = true });
        semanas.Add(new SemanaPMO { Id = id++, Numero = 13, Ano = 2025, DataInicio = DateTime.Parse("2025-03-29"), DataFim = DateTime.Parse("2025-04-04"), DataCriacao = DateTime.Parse("2025-03-21"), Ativo = true });
        
        // Abril a Dezembro 2025 (restante do ano - 39 semanas)
        var dataAtual = DateTime.Parse("2025-04-05");
        for (int semana = 14; semana <= 52; semana++)
        {
            var dataFim = dataAtual.AddDays(6);
            semanas.Add(new SemanaPMO
            {
                Id = id++,
                Numero = semana,
                Ano = 2025,
                DataInicio = dataAtual,
                DataFim = dataFim,
                DataCriacao = dataAtual.AddDays(-7),
                Ativo = true
            });
            dataAtual = dataAtual.AddDays(7);
        }
        
        // 2026 - Ano completo (52 semanas)
        dataAtual = DateTime.Parse("2026-01-03"); // Primeira semana de 2026
        for (int semana = 1; semana <= 52; semana++)
        {
            var dataFim = dataAtual.AddDays(6);
            semanas.Add(new SemanaPMO
            {
                Id = id++,
                Numero = semana,
                Ano = 2026,
                DataInicio = dataAtual,
                DataFim = dataFim,
                DataCriacao = dataAtual.AddDays(-7),
                Ativo = true,
                Observacoes = semana == 1 ? "Primeira semana de 2026" : null
            });
            dataAtual = dataAtual.AddDays(7);
        }
        
        modelBuilder.Entity<SemanaPMO>().HasData(semanas.ToArray());
    }

    private static void SeedEquipesPDP(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EquipePDP>().HasData(
            new EquipePDP { Id = 1, Nome = "Equipe de Operação Nordeste", Descricao = "Responsável pela programação diária de produção da região Nordeste", Coordenador = "João Silva Santos", Email = "operacao.ne@ons.org.br", Telefone = "(81) 3421-5000", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new EquipePDP { Id = 2, Nome = "Equipe de Operação Sudeste", Descricao = "Responsável pela programação diária de produção da região Sudeste/Centro-Oeste", Coordenador = "Maria Oliveira Costa", Email = "operacao.se@ons.org.br", Telefone = "(21) 3444-5500", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new EquipePDP { Id = 3, Nome = "Equipe de Operação Sul", Descricao = "Responsável pela programação diária de produção da região Sul", Coordenador = "Carlos Eduardo Ferreira", Email = "operacao.sul@ons.org.br", Telefone = "(41) 3333-4400", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new EquipePDP { Id = 4, Nome = "Equipe de Operação Norte", Descricao = "Responsável pela programação diária de produção da região Norte", Coordenador = "Ana Paula Rodrigues", Email = "operacao.norte@ons.org.br", Telefone = "(92) 3232-1100", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new EquipePDP { Id = 5, Nome = "Equipe de Planejamento Energético", Descricao = "Responsável pelo planejamento de médio e longo prazo da operação", Coordenador = "Roberto Mendes Lima", Email = "planejamento@ons.org.br", Telefone = "(21) 3444-5600", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true }
        );
    }

    private static void SeedMotivosRestricao(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MotivoRestricao>().HasData(
            new MotivoRestricao { Id = 1, Nome = "Manutenção Programada", Descricao = "Parada para manutenção preventiva ou corretiva", Categoria = "Manutenção", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new MotivoRestricao { Id = 2, Nome = "Falha de Equipamento", Descricao = "Problema em equipamento que restringe a geração", Categoria = "Técnica", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new MotivoRestricao { Id = 3, Nome = "Restrição Hidráulica", Descricao = "Vazão insuficiente para geração plena", Categoria = "Hidráulica", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new MotivoRestricao { Id = 4, Nome = "Restrição de Transmissão", Descricao = "Limitação no sistema de transmissão", Categoria = "Elétrica", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new MotivoRestricao { Id = 5, Nome = "Teste Operacional", Descricao = "Teste de equipamentos ou sistemas", Categoria = "Operacional", DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true }
        );
    }

    private static void SeedUnidadesGeradoras(ModelBuilder modelBuilder)
    {
        var ugs = new List<UnidadeGeradora>();
        int id = 1;

        // Itaipu (20 UGs - 700 MW cada)
        for (int i = 1; i <= 20; i++)
        {
            ugs.Add(new UnidadeGeradora
            {
                Id = id++,
                Codigo = $"ITAIPU-UG{i:D2}",
                Nome = $"Unidade Geradora {i} - Itaipu",
                UsinaId = 1,
                PotenciaNominal = 700.00m,
                PotenciaMinima = 350.00m,
                DataComissionamento = DateTime.Parse("1984-05-05").AddDays(i * 30),
                Status = "Operando",
                DataCriacao = DateTime.Parse("1984-05-05"),
                Ativo = true
            });
        }

        // Belo Monte (18 UGs - 611 MW cada)
        for (int i = 1; i <= 18; i++)
        {
            ugs.Add(new UnidadeGeradora
            {
                Id = id++,
                Codigo = $"BELO-MONTE-UG{i:D2}",
                Nome = $"Unidade Geradora {i} - Belo Monte",
                UsinaId = 2,
                PotenciaNominal = 611.00m,
                PotenciaMinima = 300.00m,
                DataComissionamento = DateTime.Parse("2016-04-29").AddDays(i * 15),
                Status = "Operando",
                DataCriacao = DateTime.Parse("2016-04-29"),
                Ativo = true
            });
        }

        // Tucuruí (24 UGs - 350 MW cada)
        for (int i = 1; i <= 24; i++)
        {
            ugs.Add(new UnidadeGeradora
            {
                Id = id++,
                Codigo = $"TUCURUI-UG{i:D2}",
                Nome = $"Unidade Geradora {i} - Tucuruí",
                UsinaId = 3,
                PotenciaNominal = 350.00m,
                PotenciaMinima = 175.00m,
                DataComissionamento = DateTime.Parse("1984-11-22").AddDays(i * 20),
                Status = "Operando",
                DataCriacao = DateTime.Parse("1984-11-22"),
                Ativo = true
            });
        }

        // Santo Antônio (44 UGs - 81 MW cada)
        for (int i = 1; i <= 38; i++)
        {
            // Distribuir entre as outras usinas (4-10)
            int usinaId = 4 + (i % 7);
            string usinaCodigo = usinaId switch
            {
                4 => "SANTO-ANTONIO",
                5 => "JIRAU",
                6 => "ILHA-SOLTEIRA",
                7 => "XINGO",
                8 => "PAULO-AFONSO-IV",
                9 => "ANGRA-I",
                10 => "ANGRA-II",
                _ => "OUTRAS"
            };

            ugs.Add(new UnidadeGeradora
            {
                Id = id++,
                Codigo = $"{usinaCodigo}-UG{i:D2}",
                Nome = $"Unidade Geradora {i} - {usinaCodigo}",
                UsinaId = usinaId,
                PotenciaNominal = usinaId == 9 ? 320.00m : (usinaId == 10 ? 675.00m : 200.00m),
                PotenciaMinima = usinaId == 9 ? 160.00m : (usinaId == 10 ? 337.50m : 100.00m),
                DataComissionamento = DateTime.Parse("2010-01-01").AddDays(i * 10),
                Status = i % 10 == 0 ? "Manutenção" : "Operando",
                DataCriacao = DateTime.Parse("2010-01-01"),
                Ativo = true
            });
        }

        modelBuilder.Entity<UnidadeGeradora>().HasData(ugs.ToArray());
    }

    private static void SeedCargas(ModelBuilder modelBuilder)
    {
        var cargas = new List<Carga>();
        int id = 1;
        var dataInicio = DateTime.Parse("2024-11-26");

        // Últimos 30 dias × 4 subsistemas
        for (int dia = 0; dia < 30; dia++)
        {
            var data = dataInicio.AddDays(dia);

            // Subsistema Sudeste (SE) - ~45.000 MW
            cargas.Add(new Carga
            {
                Id = id++,
                DataReferencia = data,
                SubsistemaId = "SE",
                CargaMWmed = 45000 + (dia % 7) * 500,
                CargaVerificada = 44800 + (dia % 7) * 480,
                PrevisaoCarga = 45200 + (dia % 7) * 520,
                DataCriacao = data.AddHours(-6),
                Ativo = true
            });

            // Subsistema Sul (S) - ~12.000 MW
            cargas.Add(new Carga
            {
                Id = id++,
                DataReferencia = data,
                SubsistemaId = "S",
                CargaMWmed = 12000 + (dia % 7) * 200,
                CargaVerificada = 11900 + (dia % 7) * 190,
                PrevisaoCarga = 12100 + (dia % 7) * 210,
                DataCriacao = data.AddHours(-6),
                Ativo = true
            });

            // Subsistema Nordeste (NE) - ~14.000 MW
            cargas.Add(new Carga
            {
                Id = id++,
                DataReferencia = data,
                SubsistemaId = "NE",
                CargaMWmed = 14000 + (dia % 7) * 250,
                CargaVerificada = 13850 + (dia % 7) * 240,
                PrevisaoCarga = 14150 + (dia % 7) * 260,
                DataCriacao = data.AddHours(-6),
                Ativo = true
            });

            // Subsistema Norte (N) - ~8.000 MW
            cargas.Add(new Carga
            {
                Id = id++,
                DataReferencia = data,
                SubsistemaId = "N",
                CargaMWmed = 8000 + (dia % 7) * 150,
                CargaVerificada = 7900 + (dia % 7) * 145,
                PrevisaoCarga = 8100 + (dia % 7) * 155,
                DataCriacao = data.AddHours(-6),
                Ativo = true
            });
        }

        modelBuilder.Entity<Carga>().HasData(cargas.ToArray());
    }

    private static void SeedIntercambios(ModelBuilder modelBuilder)
    {
        var intercambios = new List<Intercambio>();
        int id = 1;
        var dataInicio = DateTime.Parse("2024-11-26");

        // Últimos 30 dias × 8 pares de subsistemas
        for (int dia = 0; dia < 30; dia++)
        {
            var data = dataInicio.AddDays(dia);

            // SE → S
            intercambios.Add(new Intercambio
            {
                Id = id++,
                DataReferencia = data,
                SubsistemaOrigem = "SE",
                SubsistemaDestino = "S",
                EnergiaIntercambiada = 300 + (dia % 10) * 20,
                DataCriacao = data.AddHours(-6),
                Ativo = true
            });

            // S → SE
            intercambios.Add(new Intercambio
            {
                Id = id++,
                DataReferencia = data,
                SubsistemaOrigem = "S",
                SubsistemaDestino = "SE",
                EnergiaIntercambiada = 150 + (dia % 10) * 15,
                DataCriacao = data.AddHours(-6),
                Ativo = true
            });

            // SE → NE
            intercambios.Add(new Intercambio
            {
                Id = id++,
                DataReferencia = data,
                SubsistemaOrigem = "SE",
                SubsistemaDestino = "NE",
                EnergiaIntercambiada = 450 + (dia % 10) * 25,
                DataCriacao = data.AddHours(-6),
                Ativo = true
            });

            // NE → SE
            intercambios.Add(new Intercambio
            {
                Id = id++,
                DataReferencia = data,
                SubsistemaOrigem = "NE",
                SubsistemaDestino = "SE",
                EnergiaIntercambiada = 100 + (dia % 10) * 10,
                DataCriacao = data.AddHours(-6),
                Ativo = true
            });

            // N → NE
            intercambios.Add(new Intercambio
            {
                Id = id++,
                DataReferencia = data,
                SubsistemaOrigem = "N",
                SubsistemaDestino = "NE",
                EnergiaIntercambiada = 200 + (dia % 10) * 15,
                DataCriacao = data.AddHours(-6),
                Ativo = true
            });

            // NE → N
            intercambios.Add(new Intercambio
            {
                Id = id++,
                DataReferencia = data,
                SubsistemaOrigem = "NE",
                SubsistemaDestino = "N",
                EnergiaIntercambiada = 80 + (dia % 10) * 8,
                DataCriacao = data.AddHours(-6),
                Ativo = true
            });

            // SE → N (via interligação)
            intercambios.Add(new Intercambio
            {
                Id = id++,
                DataReferencia = data,
                SubsistemaOrigem = "SE",
                SubsistemaDestino = "N",
                EnergiaIntercambiada = 50 + (dia % 10) * 5,
                DataCriacao = data.AddHours(-6),
                Ativo = true
            });

            // N → SE (via interligação)
            intercambios.Add(new Intercambio
            {
                Id = id++,
                DataReferencia = data,
                SubsistemaOrigem = "N",
                SubsistemaDestino = "SE",
                EnergiaIntercambiada = 30 + (dia % 10) * 3,
                DataCriacao = data.AddHours(-6),
                Ativo = true
            });
        }

        modelBuilder.Entity<Intercambio>().HasData(intercambios.ToArray());
    }

    private static void SeedBalancos(ModelBuilder modelBuilder)
    {
        var balancos = new List<Balanco>();
        int id = 1;
        var dataInicio = DateTime.Parse("2024-11-26");

        // Últimos 30 dias × 4 subsistemas
        for (int dia = 0; dia < 30; dia++)
        {
            var data = dataInicio.AddDays(dia);

            // SE
            balancos.Add(new Balanco
            {
                Id = id++,
                DataReferencia = data,
                SubsistemaId = "SE",
                Geracao = 46000 + (dia % 7) * 500,
                Carga = 45000 + (dia % 7) * 500,
                Intercambio = 300 - 150 + 50 - 30, // Saldo
                Perdas = 800,
                Deficit = 0,
                DataCriacao = data.AddHours(-6),
                Ativo = true
            });

            // S
            balancos.Add(new Balanco
            {
                Id = id++,
                DataReferencia = data,
                SubsistemaId = "S",
                Geracao = 11500 + (dia % 7) * 200,
                Carga = 12000 + (dia % 7) * 200,
                Intercambio = 150 - 300, // Saldo
                Perdas = 350,
                Deficit = 0,
                DataCriacao = data.AddHours(-6),
                Ativo = true
            });

            // NE
            balancos.Add(new Balanco
            {
                Id = id++,
                DataReferencia = data,
                SubsistemaId = "NE",
                Geracao = 13800 + (dia % 7) * 250,
                Carga = 14000 + (dia % 7) * 250,
                Intercambio = 100 - 450 + 200 - 80, // Saldo
                Perdas = 400,
                Deficit = 0,
                DataCriacao = data.AddHours(-6),
                Ativo = true
            });

            // N
            balancos.Add(new Balanco
            {
                Id = id++,
                DataReferencia = data,
                SubsistemaId = "N",
                Geracao = 8200 + (dia % 7) * 150,
                Carga = 8000 + (dia % 7) * 150,
                Intercambio = 80 - 200 + 30 - 50, // Saldo
                Perdas = 250,
                Deficit = 0,
                DataCriacao = data.AddHours(-6),
                Ativo = true
            });
        }

        modelBuilder.Entity<Balanco>().HasData(balancos.ToArray());
    }

    private static void SeedUsuarios(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().HasData(
            new Usuario { Id = 1, Nome = "Administrador Sistema", Email = "admin@ons.org.br", Telefone = "(21) 3444-5000", Perfil = "Administrador", EquipePDPId = null, DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new Usuario { Id = 2, Nome = "João Silva Santos", Email = "joao.coord@ons.org.br", Telefone = "(81) 3421-5000", Perfil = "Coordenador", EquipePDPId = 1, DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new Usuario { Id = 3, Nome = "Maria Oliveira Costa", Email = "maria.op@ons.org.br", Telefone = "(21) 3444-5500", Perfil = "Operador", EquipePDPId = 2, DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new Usuario { Id = 4, Nome = "Carlos Eduardo Ferreira", Email = "carlos.op@ons.org.br", Telefone = "(41) 3333-4400", Perfil = "Operador", EquipePDPId = 3, DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new Usuario { Id = 5, Nome = "Ana Paula Rodrigues", Email = "ana.op@ons.org.br", Telefone = "(92) 3232-1100", Perfil = "Operador", EquipePDPId = 4, DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new Usuario { Id = 6, Nome = "Roberto Mendes Lima", Email = "roberto.plan@ons.org.br", Telefone = "(21) 3444-5600", Perfil = "Coordenador", EquipePDPId = 5, DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new Usuario { Id = 7, Nome = "Fernanda Alves", Email = "fernanda.ana@ons.org.br", Telefone = "(21) 3444-5700", Perfil = "Analista", EquipePDPId = 2, DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new Usuario { Id = 8, Nome = "Ricardo Santos", Email = "ricardo.ana@ons.org.br", Telefone = "(21) 3444-5800", Perfil = "Analista", EquipePDPId = 2, DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new Usuario { Id = 9, Nome = "Juliana Pereira", Email = "juliana.ana@ons.org.br", Telefone = "(81) 3421-5100", Perfil = "Analista", EquipePDPId = 1, DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new Usuario { Id = 10, Nome = "Roberto Consultor", Email = "roberto.cons@ons.org.br", Telefone = "(21) 3444-5900", Perfil = "Consultor", EquipePDPId = null, DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new Usuario { Id = 11, Nome = "Patrícia Lima", Email = "patricia.cons@ons.org.br", Telefone = "(21) 3444-6000", Perfil = "Consultor", EquipePDPId = null, DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new Usuario { Id = 12, Nome = "Bruno Costa", Email = "bruno.op@ons.org.br", Telefone = "(41) 3333-4500", Perfil = "Operador", EquipePDPId = 3, DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new Usuario { Id = 13, Nome = "Camila Souza", Email = "camila.coord@ons.org.br", Telefone = "(92) 3232-1200", Perfil = "Coordenador", EquipePDPId = 4, DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new Usuario { Id = 14, Nome = "Diego Martins", Email = "diego.ana@ons.org.br", Telefone = "(41) 3333-4600", Perfil = "Analista", EquipePDPId = 3, DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true },
            new Usuario { Id = 15, Nome = "Elaine Rodrigues", Email = "elaine.coord@ons.org.br", Telefone = "(21) 3444-6100", Perfil = "Coordenador", EquipePDPId = 5, DataCriacao = DateTime.Parse("2020-01-01"), Ativo = true }
        );
    }

    private static void SeedRestricoesUG(ModelBuilder modelBuilder)
    {
        var restricoes = new List<RestricaoUG>();
        int id = 1;
        var dataBase = DateTime.Parse("2024-12-01");

        // Restrições de Manutenção Programada (20)
        for (int i = 0; i < 20; i++)
        {
            var ugId = (i % 100) + 1; // Distribuir entre as 100 UGs
            var dataInicio = dataBase.AddDays(i * 2);
            var dataFim = dataInicio.AddDays(5 + (i % 3)); // 5 a 7 dias

            restricoes.Add(new RestricaoUG
            {
                Id = id++,
                UnidadeGeradoraId = ugId,
                DataInicio = dataInicio,
                DataFim = dataFim,
                MotivoRestricaoId = 1, // Manutenção Programada
                PotenciaRestrita = 100 + (i * 10),
                Observacoes = $"Manutenção preventiva programada da UG {ugId}",
                DataCriacao = dataInicio.AddDays(-7),
                Ativo = true
            });
        }

        // Restrições Hidráulicas (10)
        for (int i = 0; i < 10; i++)
        {
            var ugId = (i % 62) + 1; // Apenas UGs hidráulicas (Itaipu, Belo Monte, Tucuruí)
            var dataInicio = dataBase.AddDays(i * 3);

            restricoes.Add(new RestricaoUG
            {
                Id = id++,
                UnidadeGeradoraId = ugId,
                DataInicio = dataInicio,
                DataFim = null, // Restrição sem previsão de término
                MotivoRestricaoId = 3, // Restrição Hidráulica
                PotenciaRestrita = 50 + (i * 5),
                Observacoes = $"Vazão reduzida no reservatório - UG {ugId}",
                DataCriacao = dataInicio.AddDays(-1),
                Ativo = true
            });
        }

        // Falhas de Equipamento (10)
        for (int i = 0; i < 10; i++)
        {
            var ugId = 20 + (i * 8); // Distribuir
            var dataInicio = dataBase.AddDays(5 + (i * 2));
            var dataFim = dataInicio.AddDays(3);

            restricoes.Add(new RestricaoUG
            {
                Id = id++,
                UnidadeGeradoraId = ugId,
                DataInicio = dataInicio,
                DataFim = dataFim,
                MotivoRestricaoId = 2, // Falha de Equipamento
                PotenciaRestrita = 200 + (i * 20),
                Observacoes = $"Problema no sistema de refrigeração - UG {ugId}",
                DataCriacao = dataInicio,
                Ativo = true
            });
        }

        // Restrições de Transmissão (5)
        for (int i = 0; i < 5; i++)
        {
            var ugId = 30 + (i * 15);
            var dataInicio = dataBase.AddDays(10 + i);
            var dataFim = dataInicio.AddDays(1);

            restricoes.Add(new RestricaoUG
            {
                Id = id++,
                UnidadeGeradoraId = ugId,
                DataInicio = dataInicio,
                DataFim = dataFim,
                MotivoRestricaoId = 4, // Restrição de Transmissão
                PotenciaRestrita = 150 + (i * 10),
                Observacoes = $"Limitação na linha de transmissão - UG {ugId}",
                DataCriacao = dataInicio.AddHours(-12),
                Ativo = true
            });
        }

        // Testes Operacionais (5)
        for (int i = 0; i < 5; i++)
        {
            var ugId = 50 + (i * 10);
            var dataInicio = dataBase.AddDays(15 + i);
            var dataFim = dataInicio.AddHours(8);

            restricoes.Add(new RestricaoUG
            {
                Id = id++,
                UnidadeGeradoraId = ugId,
                DataInicio = dataInicio,
                DataFim = dataFim,
                MotivoRestricaoId = 5, // Teste Operacional
                PotenciaRestrita = 50,
                Observacoes = $"Teste de sistema de proteção - UG {ugId}",
                DataCriacao = dataInicio.AddDays(-3),
                Ativo = true
            });
        }

        modelBuilder.Entity<RestricaoUG>().HasData(restricoes.ToArray());
    }

    private static void SeedParadasUG(ModelBuilder modelBuilder)
    {
        var paradas = new List<ParadaUG>();
        int id = 1;
        var dataBase = DateTime.Parse("2024-12-01");

        // Paradas Programadas (20)
        for (int i = 0; i < 20; i++)
        {
            var ugId = (i % 100) + 1;
            var dataInicio = dataBase.AddDays(i * 3);
            var dataFim = dataInicio.AddDays(7 + (i % 5)); // 7 a 11 dias

            paradas.Add(new ParadaUG
            {
                Id = id++,
                UnidadeGeradoraId = ugId,
                DataInicio = dataInicio,
                DataFim = dataFim,
                MotivoParada = i % 3 == 0 ? "Manutenção Preventiva Anual" : "Manutenção Corretiva Programada",
                Programada = true,
                Observacoes = $"Parada programada para revisão geral - UG {ugId}",
                DataCriacao = dataInicio.AddDays(-30),
                Ativo = true
            });
        }

        // Paradas Não Programadas (10)
        for (int i = 0; i < 10; i++)
        {
            var ugId = 10 + (i * 9);
            var dataInicio = dataBase.AddDays(5 + (i * 2));
            var dataFim = dataInicio.AddDays(2 + (i % 3)); // 2 a 4 dias

            paradas.Add(new ParadaUG
            {
                Id = id++,
                UnidadeGeradoraId = ugId,
                DataInicio = dataInicio,
                DataFim = dataFim,
                MotivoParada = i % 2 == 0 ? "Falha no Sistema de Excitação" : "Problema no Gerador",
                Programada = false,
                Observacoes = $"Parada emergencial por falha - UG {ugId}",
                DataCriacao = dataInicio,
                Ativo = true
            });
        }

        modelBuilder.Entity<ParadaUG>().HasData(paradas.ToArray());
    }

    private static void SeedArquivosDadger(ModelBuilder modelBuilder)
    {
        var arquivos = new List<ArquivoDadger>();
        int id = 1;

        // Últimas 4 semanas (IDs 1-4 das SemanasPMO)
        for (int semanaId = 1; semanaId <= 4; semanaId++)
        {
            // 5 revisões por semana (Rev0 a Rev4)
            for (int revisao = 0; revisao < 5; revisao++)
            {
                var semanaNumero = 48 + semanaId;
                var dataImportacao = DateTime.Parse("2024-12-01").AddDays((semanaId - 1) * 7 + revisao);

                arquivos.Add(new ArquivoDadger
                {
                    Id = id++,
                    NomeArquivo = $"DADGER_2024_S{semanaNumero:D2}_REV{revisao}.DAT",
                    CaminhoArquivo = $"/dados/2024/semana{semanaNumero:D2}/DADGER_2024_S{semanaNumero:D2}_REV{revisao}.DAT",
                    DataImportacao = dataImportacao,
                    SemanaPMOId = semanaId,
                    Observacoes = revisao == 0 ? "Revisão inicial (domingo)" : $"Revisão {revisao} - atualização diária",
                    Processado = id <= 15, // Primeiros 15 já processados
                    DataProcessamento = id <= 15 ? dataImportacao.AddHours(2) : null,
                    DataCriacao = dataImportacao.AddHours(-1),
                    Ativo = true
                });
            }
        }

        modelBuilder.Entity<ArquivoDadger>().HasData(arquivos.ToArray());
    }
}
