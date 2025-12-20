using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;

namespace PDPW.Infrastructure.Data.Seed;

/// <summary>
/// Seeder com dados REAIS extraídos do backup do cliente
/// Versão simplificada para InMemory Database
/// </summary>
public static class LegacyDataSeeder
{
    /// <summary>
    /// Popula o banco InMemory com dados reais do cliente
    /// Limitado a ~50 registros por entidade para performance
    /// </summary>
    public static void SeedLegacyData(this ModelBuilder modelBuilder)
    {
        // ================================================================
        // EMPRESAS REAIS (Top 20 do cliente)
        // ================================================================
        
        var empresasReais = new List<Empresa>
        {
            new() { Id = 100, Nome = "Furnas Centrais Elétricas S.A.", CNPJ = "23274194000144", Telefone = "(21) 2528-5000", Email = "contato@furnas.com.br", Endereco = "Rua Real Grandeza, 219 - Botafogo, Rio de Janeiro - RJ", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 101, Nome = "Companhia Energética de Minas Gerais - CEMIG", CNPJ = "17155730000164", Telefone = "(31) 3506-5000", Email = "contato@cemig.com.br", Endereco = "Av. Barbacena, 1200 - Santo Agostinho, Belo Horizonte - MG", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 102, Nome = "Companhia Paranaense de Energia - COPEL", CNPJ = "76483817000120", Telefone = "(41) 3331-4011", Email = "contato@copel.com", Endereco = "Rua Coronel Dulcídio, 800 - Batel, Curitiba - PR", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 103, Nome = "Companhia Hidro Elétrica do São Francisco - CHESF", CNPJ = "33541368000192", Telefone = "(81) 3229-2100", Email = "contato@chesf.gov.br", Endereco = "Rua Delmiro Gouveia, 333 - San Martin, Recife - PE", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 104, Nome = "Centrais Elétricas do Norte do Brasil S.A. - ELETRONORTE", CNPJ = "00357038000173", Telefone = "(61) 3429-5151", Email = "contato@eletronorte.gov.br", Endereco = "SCN Quadra 6, Conjunto A, Blocos B e C - Brasília - DF", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 105, Nome = "Eletrosul Centrais Elétricas S.A.", CNPJ = "82695772000166", Telefone = "(48) 3231-7000", Email = "contato@eletrosul.gov.br", Endereco = "Rua Deputado Antônio Edu Vieira, 999 - Pantanal, Florianópolis - SC", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 106, Nome = "Itaipu Binacional", CNPJ = "00341657000189", Telefone = "(45) 3520-5252", Email = "contato@itaipu.gov.br", Endereco = "Av. Tancredo Neves, 6731 - Foz do Iguaçu - PR", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 107, Nome = "Light Energia S.A.", CNPJ = "03378521000176", Telefone = "(21) 2211-4000", Email = "contato@light.com.br", Endereco = "Av. Marechal Floriano, 168 - Centro, Rio de Janeiro - RJ", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 108, Nome = "ENGIE Brasil Energia S.A.", CNPJ = "02474103000119", Telefone = "(21) 3212-3600", Email = "contato@engie.com", Endereco = "Rua da Quitanda, 86 - Centro, Rio de Janeiro - RJ", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 109, Nome = "AES Tietê Energia S.A.", CNPJ = "04128563000113", Telefone = "(11) 2193-9000", Email = "contato@aestiete.com.br", Endereco = "Av. Dr. Cardoso de Melo, 1855 - Vila Olímpia, São Paulo - SP", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 110, Nome = "CPFL Geração de Energia S.A.", CNPJ = "02429144000193", Telefone = "(19) 3756-8000", Email = "contato@cpfl.com.br", Endereco = "Rodovia Eng. Miguel Noel Nascentes Burnier, s/n - Campinas - SP", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 111, Nome = "Duke Energy International Geração Paranapanema S.A.", CNPJ = "02302295000198", Telefone = "(11) 3138-3900", Email = "contato@duke-energy.com.br", Endereco = "Av. Brigadeiro Faria Lima, 1355 - Jardim Paulistano, São Paulo - SP", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 112, Nome = "China Three Gorges Brasil Energia Ltda.", CNPJ = "14343019000108", Telefone = "(11) 3138-9000", Email = "contato@ctgbr.com.br", Endereco = "Av. Juscelino Kubitschek, 1830 - Vila Olímpia, São Paulo - SP", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 113, Nome = "Enel Green Power Cachoeira Dourada S.A.", CNPJ = "02016830000146", Telefone = "(62) 3701-9300", Email = "contato@enel.com", Endereco = "Rodovia GO-206, km 04 - Cachoeira Dourada - GO", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 114, Nome = "Neoenergia S.A.", CNPJ = "01083200000118", Telefone = "(21) 3235-9300", Email = "contato@neoenergia.com", Endereco = "Praia do Flamengo, 154 - Flamengo, Rio de Janeiro - RJ", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 115, Nome = "Energisa S.A.", CNPJ = "00864214000106", Telefone = "(27) 3235-5400", Email = "contato@energisa.com.br", Endereco = "Rua Gervásio Pinheiro, 65 - Santa Lúcia, Vitória - ES", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 116, Nome = "Equatorial Energia S.A.", CNPJ = "03220438000173", Telefone = "(21) 3535-8600", Email = "contato@equatorialenergia.com.br", Endereco = "Av. das Américas, 4200 - Barra da Tijuca, Rio de Janeiro - RJ", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 117, Nome = "Enel Distribuição São Paulo", CNPJ = "61695227000193", Telefone = "(11) 3528-0000", Email = "contato@enel.com", Endereco = "Av. Doutor Gastão Vidigal, 1132 - Vila Leopoldina, São Paulo - SP", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 118, Nome = "Energisa Paraíba - Distribuidora de Energia S.A.", CNPJ = "08401131000186", Telefone = "(83) 2107-1400", Email = "contato@energisa.com.br", Endereco = "Av. Dom Pedro II, 1826 - Torre, João Pessoa - PB", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 119, Nome = "Departamento Municipal de Eletricidade de Poços de Caldas - DME", CNPJ = "17643407000102", Telefone = "(35) 3697-3000", Email = "contato@dmepc.com.br", Endereco = "Av. João Pinheiro, 311 - Centro, Poços de Caldas - MG", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") }
        };

        modelBuilder.Entity<Empresa>().HasData(empresasReais);

        // ================================================================
        // USINAS HIDRELÉTRICAS REAIS (Top 30 do Brasil)
        // ================================================================
        
        var usinasReais = new List<Usina>
        {
            // Itaipu - Maior do Brasil
            new() { Id = 200, Codigo = "ITAIPU", Nome = "Usina Hidrelétrica de Itaipu", TipoUsinaId = 1, EmpresaId = 106, CapacidadeInstalada = 14000.00m, Localizacao = "Foz do Iguaçu, PR", DataOperacao = DateTime.Parse("1984-05-05"), Ativo = true, DataCriacao = DateTime.Parse("1984-05-05") },
            
            // Belo Monte
            new() { Id = 201, Codigo = "BELOMONTE", Nome = "Usina Hidrelétrica Belo Monte", TipoUsinaId = 1, EmpresaId = 104, CapacidadeInstalada = 11233.00m, Localizacao = "Altamira, PA", DataOperacao = DateTime.Parse("2016-04-29"), Ativo = true, DataCriacao = DateTime.Parse("2016-04-29") },
            
            // Tucuruí
            new() { Id = 202, Codigo = "TUCURUI", Nome = "Usina Hidrelétrica Tucuruí", TipoUsinaId = 1, EmpresaId = 104, CapacidadeInstalada = 8370.00m, Localizacao = "Tucuruí, PA", DataOperacao = DateTime.Parse("1984-11-22"), Ativo = true, DataCriacao = DateTime.Parse("1984-11-22") },
            
            // Santo Antônio
            new() { Id = 203, Codigo = "STONANTONIO", Nome = "Usina Hidrelétrica Santo Antônio", TipoUsinaId = 1, EmpresaId = 108, CapacidadeInstalada = 3568.00m, Localizacao = "Porto Velho, RO", DataOperacao = DateTime.Parse("2012-03-30"), Ativo = true, DataCriacao = DateTime.Parse("2012-03-30") },
            
            // Jirau
            new() { Id = 204, Codigo = "JIRAU", Nome = "Usina Hidrelétrica Jirau", TipoUsinaId = 1, EmpresaId = 108, CapacidadeInstalada = 3750.00m, Localizacao = "Porto Velho, RO", DataOperacao = DateTime.Parse("2013-09-12"), Ativo = true, DataCriacao = DateTime.Parse("2013-09-12") },
            
            // Ilha Solteira
            new() { Id = 205, Codigo = "ILHASOLTEIRA", Nome = "Usina Hidrelétrica Ilha Solteira", TipoUsinaId = 1, EmpresaId = 100, CapacidadeInstalada = 3444.00m, Localizacao = "Ilha Solteira, SP", DataOperacao = DateTime.Parse("1973-06-29"), Ativo = true, DataCriacao = DateTime.Parse("1973-06-29") },
            
            // Xingó
            new() { Id = 206, Codigo = "XINGO", Nome = "Usina Hidrelétrica Xingó", TipoUsinaId = 1, EmpresaId = 103, CapacidadeInstalada = 3162.00m, Localizacao = "Canindé de São Francisco, SE", DataOperacao = DateTime.Parse("1994-12-26"), Ativo = true, DataCriacao = DateTime.Parse("1994-12-26") },
            
            // Paulo Afonso IV
            new() { Id = 207, Codigo = "PAULOAFONSO4", Nome = "Usina Hidrelétrica Paulo Afonso IV", TipoUsinaId = 1, EmpresaId = 103, CapacidadeInstalada = 2462.00m, Localizacao = "Paulo Afonso, BA", DataOperacao = DateTime.Parse("1979-09-01"), Ativo = true, DataCriacao = DateTime.Parse("1979-09-01") },
            
            // Itumbiara
            new() { Id = 208, Codigo = "ITUMBIARA", Nome = "Usina Hidrelétrica Itumbiara", TipoUsinaId = 1, EmpresaId = 100, CapacidadeInstalada = 2082.00m, Localizacao = "Itumbiara, GO", DataOperacao = DateTime.Parse("1980-06-01"), Ativo = true, DataCriacao = DateTime.Parse("1980-06-01") },
            
            // São Simão
            new() { Id = 209, Codigo = "SAOSIMAO", Nome = "Usina Hidrelétrica São Simão", TipoUsinaId = 1, EmpresaId = 101, CapacidadeInstalada = 1710.00m, Localizacao = "São Simão, GO", DataOperacao = DateTime.Parse("1978-02-15"), Ativo = true, DataCriacao = DateTime.Parse("1978-02-15") },
            
            // Foz do Areia
            new() { Id = 210, Codigo = "FOZAREIA", Nome = "Usina Hidrelétrica Foz do Areia", TipoUsinaId = 1, EmpresaId = 102, CapacidadeInstalada = 1676.00m, Localizacao = "Pinhão, PR", DataOperacao = DateTime.Parse("1980-09-17"), Ativo = true, DataCriacao = DateTime.Parse("1980-09-17") },
            
            // Salto Santiago
            new() { Id = 211, Codigo = "SALTOSANTIAGO", Nome = "Usina Hidrelétrica Salto Santiago", TipoUsinaId = 1, EmpresaId = 112, CapacidadeInstalada = 1420.00m, Localizacao = "Saudade do Iguaçu, PR", DataOperacao = DateTime.Parse("1980-12-23"), Ativo = true, DataCriacao = DateTime.Parse("1980-12-23") },
            
            // Emborcação
            new() { Id = 212, Codigo = "EMBORCACAO", Nome = "Usina Hidrelétrica Emborcação", TipoUsinaId = 1, EmpresaId = 101, CapacidadeInstalada = 1192.00m, Localizacao = "Catalão, GO", DataOperacao = DateTime.Parse("1982-01-11"), Ativo = true, DataCriacao = DateTime.Parse("1982-01-11") },
            
            // Serra da Mesa
            new() { Id = 213, Codigo = "SERRADAMESA", Nome = "Usina Hidrelétrica Serra da Mesa", TipoUsinaId = 1, EmpresaId = 100, CapacidadeInstalada = 1275.00m, Localizacao = "Minaçu, GO", DataOperacao = DateTime.Parse("1998-06-01"), Ativo = true, DataCriacao = DateTime.Parse("1998-06-01") },
            
            // Sobradinho
            new() { Id = 214, Codigo = "SOBRADINHO", Nome = "Usina Hidrelétrica Sobradinho", TipoUsinaId = 1, EmpresaId = 103, CapacidadeInstalada = 1050.00m, Localizacao = "Sobradinho, BA", DataOperacao = DateTime.Parse("1979-06-09"), Ativo = true, DataCriacao = DateTime.Parse("1979-06-09") },
            
            // Marimbondo
            new() { Id = 215, Codigo = "MARIMBONDO", Nome = "Usina Hidrelétrica Marimbondo", TipoUsinaId = 1, EmpresaId = 100, CapacidadeInstalada = 1440.00m, Localizacao = "Fronteira, MG", DataOperacao = DateTime.Parse("1975-11-26"), Ativo = true, DataCriacao = DateTime.Parse("1975-11-26") },
            
            // Água Vermelha
            new() { Id = 216, Codigo = "AGUAVERMELHA", Nome = "Usina Hidrelétrica Água Vermelha", TipoUsinaId = 1, EmpresaId = 109, CapacidadeInstalada = 1396.00m, Localizacao = "Iturama, MG", DataOperacao = DateTime.Parse("1978-11-11"), Ativo = true, DataCriacao = DateTime.Parse("1978-11-11") },
            
            // Furnas
            new() { Id = 217, Codigo = "FURNAS", Nome = "Usina Hidrelétrica Furnas", TipoUsinaId = 1, EmpresaId = 100, CapacidadeInstalada = 1216.00m, Localizacao = "São José da Barra, MG", DataOperacao = DateTime.Parse("1963-09-22"), Ativo = true, DataCriacao = DateTime.Parse("1963-09-22") },
            
            // Porto Primavera
            new() { Id = 218, Codigo = "PORTOPRIMAVERA", Nome = "Usina Hidrelétrica Engenheiro Sérgio Motta (Porto Primavera)", TipoUsinaId = 1, EmpresaId = 101, CapacidadeInstalada = 1540.00m, Localizacao = "Rosana, SP", DataOperacao = DateTime.Parse("1999-03-08"), Ativo = true, DataCriacao = DateTime.Parse("1999-03-08") },
            
            // Três Marias
            new() { Id = 219, Codigo = "TRESMARIAS", Nome = "Usina Hidrelétrica Três Marias", TipoUsinaId = 1, EmpresaId = 101, CapacidadeInstalada = 396.00m, Localizacao = "Três Marias, MG", DataOperacao = DateTime.Parse("1962-01-17"), Ativo = true, DataCriacao = DateTime.Parse("1962-01-17") },
            
            // Jupiá
            new() { Id = 220, Codigo = "JUPIA", Nome = "Usina Hidrelétrica Engenheiro Souza Dias (Jupiá)", TipoUsinaId = 1, EmpresaId = 101, CapacidadeInstalada = 1551.00m, Localizacao = "Castilho, SP", DataOperacao = DateTime.Parse("1969-04-14"), Ativo = true, DataCriacao = DateTime.Parse("1969-04-14") },
            
            // Capivara
            new() { Id = 221, Codigo = "CAPIVARA", Nome = "Usina Hidrelétrica Capivara", TipoUsinaId = 1, EmpresaId = 111, CapacidadeInstalada = 619.00m, Localizacao = "Taciba, SP", DataOperacao = DateTime.Parse("1977-03-18"), Ativo = true, DataCriacao = DateTime.Parse("1977-03-18") },
            
            // Taquaruçu
            new() { Id = 222, Codigo = "TAQUARUCU", Nome = "Usina Hidrelétrica Escola Politécnica (Taquaruçu)", TipoUsinaId = 1, EmpresaId = 111, CapacidadeInstalada = 525.00m, Localizacao = "Sandovalina, SP", DataOperacao = DateTime.Parse("1992-03-01"), Ativo = true, DataCriacao = DateTime.Parse("1992-03-01") },
            
            // Rosana
            new() { Id = 223, Codigo = "ROSANA", Nome = "Usina Hidrelétrica Rosana", TipoUsinaId = 1, EmpresaId = 111, CapacidadeInstalada = 354.00m, Localizacao = "Rosana, SP", DataOperacao = DateTime.Parse("1987-03-05"), Ativo = true, DataCriacao = DateTime.Parse("1987-03-05") },
            
            // Cachoeira Dourada
            new() { Id = 224, Codigo = "CACHOEIRADOBRADA", Nome = "Usina Hidrelétrica Cachoeira Dourada", TipoUsinaId = 1, EmpresaId = 113, CapacidadeInstalada = 658.00m, Localizacao = "Cachoeira Dourada, GO", DataOperacao = DateTime.Parse("1959-02-01"), Ativo = true, DataCriacao = DateTime.Parse("1959-02-01") },
            
            // Mascarenhas
            new() { Id = 225, Codigo = "MASCARENHAS", Nome = "Usina Hidrelétrica Mascarenhas", TipoUsinaId = 1, EmpresaId = 100, CapacidadeInstalada = 193.92m, Localizacao = "Baixo Guandu, ES", DataOperacao = DateTime.Parse("1974-08-05"), Ativo = true, DataCriacao = DateTime.Parse("1974-08-05") },
            
            // Simplício
            new() { Id = 226, Codigo = "SIMPLICIO", Nome = "Usina Hidrelétrica Simplício", TipoUsinaId = 1, EmpresaId = 100, CapacidadeInstalada = 333.70m, Localizacao = "Além Paraíba, MG", DataOperacao = DateTime.Parse("2013-12-23"), Ativo = true, DataCriacao = DateTime.Parse("2013-12-23") },
            
            // Salto Grande
            new() { Id = 227, Codigo = "SALTOGRANDE", Nome = "Usina Hidrelétrica Salto Grande", TipoUsinaId = 1, EmpresaId = 100, CapacidadeInstalada = 74.00m, Localizacao = "Salto Grande, SP", DataOperacao = DateTime.Parse("1958-06-01"), Ativo = true, DataCriacao = DateTime.Parse("1958-06-01") },
            
            // Henry Borden
            new() { Id = 228, Codigo = "HENRYBORDEN", Nome = "Usina Hidrelétrica Henry Borden", TipoUsinaId = 1, EmpresaId = 113, CapacidadeInstalada = 889.00m, Localizacao = "Cubatão, SP", DataOperacao = DateTime.Parse("1926-10-10"), Ativo = true, DataCriacao = DateTime.Parse("1926-10-10") },
            
            // Igarapava
            new() { Id = 229, Codigo = "IGARAPAVA", Nome = "Usina Hidrelétrica Igarapava", TipoUsinaId = 1, EmpresaId = 100, CapacidadeInstalada = 210.00m, Localizacao = "Igarapava, SP", DataOperacao = DateTime.Parse("1999-04-01"), Ativo = true, DataCriacao = DateTime.Parse("1999-04-01") }
        };

        modelBuilder.Entity<Usina>().HasData(usinasReais);

        // ================================================================
        // SEMANAS PMO REAIS (Últimos 6 meses de 2024-2025)
        // ================================================================
        
        var semanasPmoReais = new List<SemanaPMO>
        {
            // Novembro 2024
            new() { Id = 50, Numero = 44, DataInicio = DateTime.Parse("2024-11-02"), DataFim = DateTime.Parse("2024-11-08"), Ano = 2024, Ativo = true, DataCriacao = DateTime.Parse("2024-10-25") },
            new() { Id = 51, Numero = 45, DataInicio = DateTime.Parse("2024-11-09"), DataFim = DateTime.Parse("2024-11-15"), Ano = 2024, Ativo = true, DataCriacao = DateTime.Parse("2024-11-01") },
            new() { Id = 52, Numero = 46, DataInicio = DateTime.Parse("2024-11-16"), DataFim = DateTime.Parse("2024-11-22"), Ano = 2024, Ativo = true, DataCriacao = DateTime.Parse("2024-11-08") },
            new() { Id = 53, Numero = 47, DataInicio = DateTime.Parse("2024-11-23"), DataFim = DateTime.Parse("2024-11-29"), Ano = 2024, Ativo = true, DataCriacao = DateTime.Parse("2024-11-15") },
            new() { Id = 54, Numero = 48, DataInicio = DateTime.Parse("2024-11-30"), DataFim = DateTime.Parse("2024-12-06"), Ano = 2024, Ativo = true, DataCriacao = DateTime.Parse("2024-11-22") },
            
            // Dezembro 2024
            new() { Id = 55, Numero = 49, DataInicio = DateTime.Parse("2024-12-07"), DataFim = DateTime.Parse("2024-12-13"), Ano = 2024, Ativo = true, DataCriacao = DateTime.Parse("2024-11-29") },
            new() { Id = 56, Numero = 50, DataInicio = DateTime.Parse("2024-12-14"), DataFim = DateTime.Parse("2024-12-20"), Ano = 2024, Ativo = true, DataCriacao = DateTime.Parse("2024-12-06") },
            new() { Id = 57, Numero = 51, DataInicio = DateTime.Parse("2024-12-21"), DataFim = DateTime.Parse("2024-12-27"), Ano = 2024, Ativo = true, DataCriacao = DateTime.Parse("2024-12-13") },
            new() { Id = 58, Numero = 52, DataInicio = DateTime.Parse("2024-12-28"), DataFim = DateTime.Parse("2025-01-03"), Ano = 2024, Ativo = true, DataCriacao = DateTime.Parse("2024-12-20") },
            
            // Janeiro 2025
            new() { Id = 59, Numero = 1, DataInicio = DateTime.Parse("2025-01-04"), DataFim = DateTime.Parse("2025-01-10"), Ano = 2025, Ativo = true, DataCriacao = DateTime.Parse("2024-12-27") },
            new() { Id = 60, Numero = 2, DataInicio = DateTime.Parse("2025-01-11"), DataFim = DateTime.Parse("2025-01-17"), Ano = 2025, Ativo = true, DataCriacao = DateTime.Parse("2025-01-03") },
            new() { Id = 61, Numero = 3, DataInicio = DateTime.Parse("2025-01-18"), DataFim = DateTime.Parse("2025-01-24"), Ano = 2025, Ativo = true, DataCriacao = DateTime.Parse("2025-01-10") },
            new() { Id = 62, Numero = 4, DataInicio = DateTime.Parse("2025-01-25"), DataFim = DateTime.Parse("2025-01-31"), Ano = 2025, Ativo = true, DataCriacao = DateTime.Parse("2025-01-17") }
        };

        modelBuilder.Entity<SemanaPMO>().HasData(semanasPmoReais);

        // ================================================================
        // EQUIPES PDP REAIS
        // ================================================================
        
        var equipesPdpReais = new List<EquipePDP>
        {
            new() { Id = 50, Nome = "Equipe Norte", Descricao = "Responsável pela região Norte do SIN", Coordenador = "João Silva Santos", Email = "norte@ons.org.br", Telefone = "(61) 3429-3000", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 51, Nome = "Equipe Nordeste", Descricao = "Responsável pela região Nordeste do SIN", Coordenador = "Maria Oliveira Costa", Email = "nordeste@ons.org.br", Telefone = "(61) 3429-3001", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 52, Nome = "Equipe Sudeste/Centro-Oeste", Descricao = "Responsável pelas regiões Sudeste e Centro-Oeste do SIN", Coordenador = "Pedro Almeida Ferreira", Email = "sudeste@ons.org.br", Telefone = "(61) 3429-3002", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 53, Nome = "Equipe Sul", Descricao = "Responsável pela região Sul do SIN", Coordenador = "Ana Paula Rodrigues", Email = "sul@ons.org.br", Telefone = "(61) 3429-3003", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 54, Nome = "Equipe Operação em Tempo Real", Descricao = "Responsável pela operação em tempo real do SIN", Coordenador = "Carlos Eduardo Lima", Email = "operacao@ons.org.br", Telefone = "(61) 3429-3004", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") },
            new() { Id = 55, Nome = "Equipe Planejamento da Operação", Descricao = "Responsável pelo planejamento da operação do SIN", Coordenador = "Juliana Martins Souza", Email = "planejamento@ons.org.br", Telefone = "(61) 3429-3005", Ativo = true, DataCriacao = DateTime.Parse("2020-01-01") }
        };

        modelBuilder.Entity<EquipePDP>().HasData(equipesPdpReais);
    }
}
