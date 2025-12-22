using Microsoft.EntityFrameworkCore;
using PDPW.Domain.Entities;
using PDPW.Infrastructure.Data;

namespace PDPW.Infrastructure.Data.Seeders;

/// <summary>
/// Seeder de dados realistas do setor elétrico brasileiro
/// Baseado em informações públicas da ONS e ANEEL
/// </summary>
public static class RealisticDataSeeder
{
    public static async Task SeedAsync(PdpwDbContext context)
    {
        // Se já existem dados, não faz nada
        if (await context.Empresas.AnyAsync())
        {
            Console.WriteLine("? Banco já contém dados. Pulando seed.");
            return;
        }

        Console.WriteLine("========================================");
        Console.WriteLine("  POPULANDO BANCO COM DADOS REALISTAS");
        Console.WriteLine("========================================");
        Console.WriteLine("");

        // 1. TIPOS DE USINA (8 tipos)
        await SeedTiposUsina(context);

        // 2. EMPRESAS (30 empresas reais do setor elétrico)
        await SeedEmpresas(context);

        // 3. USINAS (50 usinas reais)
        await SeedUsinas(context);

        // 4. UNIDADES GERADORAS (100 unidades)
        await SeedUnidadesGeradoras(context);

        // 5. MOTIVOS DE RESTRIÇÃO (10 motivos)
        await SeedMotivosRestricao(context);

        // 6. PARADAS UG (50 paradas)
        await SeedParadasUG(context);

        // 7. BALANÇOS (30 balanços)
        await SeedBalancos(context);

        // 8. INTERCÂMBIOS (20 intercâmbios)
        await SeedIntercambios(context);

        // 9. SEMANAS PMO (25 semanas)
        await SeedSemanasPMO(context);

        // 10. EQUIPES PDP (11 equipes)
        await SeedEquipesPDP(context);

        Console.WriteLine("");
        Console.WriteLine("========================================");
        Console.WriteLine("  ? SEED CONCLUÍDO COM SUCESSO!");
        Console.WriteLine("========================================");
    }

    private static async Task SeedTiposUsina(PdpwDbContext context)
    {
        Console.WriteLine("?? Criando Tipos de Usina...");

        var tipos = new List<TipoUsina>
        {
            new() { Nome = "UHE", Descricao = "Usina Hidrelétrica", FonteEnergia = "Hidráulica", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "UTE", Descricao = "Usina Termelétrica", FonteEnergia = "Térmica", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "EOL", Descricao = "Parque Eólico", FonteEnergia = "Eólica", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "UFV", Descricao = "Usina Fotovoltaica", FonteEnergia = "Solar", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "PCH", Descricao = "Pequena Central Hidrelétrica", FonteEnergia = "Hidráulica", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "CGH", Descricao = "Central Geradora Hidrelétrica", FonteEnergia = "Hidráulica", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "UTN", Descricao = "Usina Termonuclear", FonteEnergia = "Nuclear", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "BIO", Descricao = "Usina de Biomassa", FonteEnergia = "Biomassa", Ativo = true, DataCriacao = DateTime.UtcNow }
        };

        await context.TiposUsina.AddRangeAsync(tipos);
        await context.SaveChangesAsync();

        Console.WriteLine($"   ? {tipos.Count} tipos criados");
    }

    private static async Task SeedEmpresas(PdpwDbContext context)
    {
        Console.WriteLine("?? Criando Empresas do Setor Elétrico...");

        // Verificar se já existem empresas além das 8 iniciais das migrations
        var empresasExistentes = await context.Empresas.CountAsync();
        if (empresasExistentes >= 30)
        {
            Console.WriteLine($"   ? Empresas já existem ({empresasExistentes}). Pulando...");
            return;
        }

        var empresas = new List<Empresa>
        {
            // Empresas reais do setor elétrico brasileiro (baseadas no backup do cliente)
            new() { Nome = "CEMIG", CNPJ = "17155730000164", Telefone = "(31) 3506-5024", Email = "atendimento@cemig.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "COPEL", CNPJ = "76483817000120", Telefone = "(41) 3331-4011", Email = "atendimento@copel.com", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "ELETROBRAS", CNPJ = "00001180000126", Telefone = "(21) 2514-5151", Email = "falecom@eletrobras.com", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "FURNAS", CNPJ = "23274194000106", Telefone = "(21) 2555-5555", Email = "ouvidoria@furnas.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "CHESF", CNPJ = "33541368000192", Telefone = "(81) 3229-2000", Email = "ouvidoria@chesf.gov.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "ITAIPU BINACIONAL", CNPJ = "00341590000135", Telefone = "(45) 3520-5252", Email = "itaipu@itaipu.gov.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "CPFL ENERGIA", CNPJ = "02429144000193", Telefone = "(19) 3756-8282", Email = "atendimento@cpfl.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "LIGHT", CNPJ = "60444437000176", Telefone = "(21) 2211-1000", Email = "light@light.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "ENGIE BRASIL", CNPJ = "02474103000119", Telefone = "(21) 3235-5900", Email = "engie@engie.com", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "AES BRASIL", CNPJ = "04128563000113", Telefone = "(11) 3169-7800", Email = "aes@aes.com", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "EQUATORIAL ENERGIA", CNPJ = "03220438000170", Telefone = "(98) 3216-9000", Email = "equatorial@equatorial.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "ENERGISA", CNPJ = "00864214000106", Telefone = "(27) 3382-5300", Email = "energisa@energisa.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "ELETRONORTE", CNPJ = "00357038000176", Telefone = "(61) 3429-5151", Email = "eletronorte@eletronorte.gov.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "CELESC", CNPJ = "83878892000197", Telefone = "(48) 3231-5200", Email = "celesc@celesc.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "CELPE", CNPJ = "10835932000108", Telefone = "(81) 3217-3000", Email = "celpe@celpe.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "COELBA", CNPJ = "15139629000178", Telefone = "(71) 3370-6000", Email = "coelba@coelba.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "DUKE ENERGY", CNPJ = "02793530000143", Telefone = "(11) 4130-8400", Email = "duke@duke-energy.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "CEEE", CNPJ = "92715812000144", Telefone = "(51) 3287-2500", Email = "ceee@ceee.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "CEMAR", CNPJ = "06272793000184", Telefone = "(98) 3216-9000", Email = "cemar@cemar.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "CELG", CNPJ = "01843793000126", Telefone = "(62) 3239-3000", Email = "celg@celg.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "ENEL", CNPJ = "61695227000193", Telefone = "(21) 2211-6565", Email = "enel@enel.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "NEOENERGIA", CNPJ = "01083200000118", Telefone = "(21) 2555-9999", Email = "neoenergia@neoenergia.com", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "TRACTEBEL ENERGIA", CNPJ = "02474254000119", Telefone = "(48) 3231-5170", Email = "tractebel@tractebel.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "CTG BRASIL", CNPJ = "11444460000106", Telefone = "(21) 3231-5000", Email = "ctg@ctgbr.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "ATLANTICA ENERGIA", CNPJ = "26842281000197", Telefone = "(11) 3214-4800", Email = "atlantica@atlanticaenergia.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "OMEGA ENERGIA", CNPJ = "02016120000162", Telefone = "(85) 3216-1000", Email = "omega@omegaenergia.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "VOLTALIA", CNPJ = "04026895000104", Telefone = "(85) 3533-5000", Email = "voltalia@voltalia.com", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "CASA DOS VENTOS", CNPJ = "10934446000107", Telefone = "(85) 3477-9000", Email = "contato@casadosventos.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "RENOVA ENERGIA", CNPJ = "08534605000148", Telefone = "(11) 3075-2900", Email = "renova@renovaenergia.com.br", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "SMART ENERGY", CNPJ = "16619345000102", Telefone = "(11) 4949-9370", Email = "smart@smartenergy.com.br", Ativo = true, DataCriacao = DateTime.UtcNow }
        };

        // Adicionar apenas empresas que não existem
        foreach (var empresa in empresas)
        {
            var existe = await context.Empresas.AnyAsync(e => e.CNPJ == empresa.CNPJ);
            if (!existe)
            {
                await context.Empresas.AddAsync(empresa);
            }
        }
        
        await context.SaveChangesAsync();

        var totalEmpresas = await context.Empresas.CountAsync();
        Console.WriteLine($"   ? {totalEmpresas} empresas no banco (adicionadas novas se necessário)");
    }

    private static async Task SeedUsinas(PdpwDbContext context)
    {
        Console.WriteLine("? Criando Usinas...");

        var tipoUHE = await context.TiposUsina.FirstAsync(t => t.Nome == "UHE");
        var tipoUTE = await context.TiposUsina.FirstAsync(t => t.Nome == "UTE");
        var tipoEOL = await context.TiposUsina.FirstAsync(t => t.Nome == "EOL");
        var tipoUFV = await context.TiposUsina.FirstAsync(t => t.Nome == "UFV");
        var tipoPCH = await context.TiposUsina.FirstAsync(t => t.Nome == "PCH");

        var empresas = await context.Empresas.ToListAsync();

        var usinas = new List<Usina>
        {
            // Hidrelétricas (20 maiores do Brasil)
            new() { Codigo = "ITAIPU", Nome = "UHE Itaipu", TipoUsinaId = tipoUHE.Id, EmpresaId = empresas[5].Id, CapacidadeInstalada = 14000, Localizacao = "PR/MS", DataOperacao = new DateTime(1984, 5, 5), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "BELO_MONTE", Nome = "UHE Belo Monte", TipoUsinaId = tipoUHE.Id, EmpresaId = empresas[2].Id, CapacidadeInstalada = 11233, Localizacao = "PA", DataOperacao = new DateTime(2016, 4, 29), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "TUCURUI", Nome = "UHE Tucuruí", TipoUsinaId = tipoUHE.Id, EmpresaId = empresas[12].Id, CapacidadeInstalada = 8370, Localizacao = "PA", DataOperacao = new DateTime(1984, 11, 22), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "S_ANTONIO", Nome = "UHE Santo Antônio", TipoUsinaId = tipoUHE.Id, EmpresaId = empresas[3].Id, CapacidadeInstalada = 3568, Localizacao = "RO", DataOperacao = new DateTime(2012, 3, 30), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "JIRAU", Nome = "UHE Jirau", TipoUsinaId = tipoUHE.Id, EmpresaId = empresas[8].Id, CapacidadeInstalada = 3750, Localizacao = "RO", DataOperacao = new DateTime(2013, 9, 10), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "ILHA_SOLT", Nome = "UHE Ilha Solteira", TipoUsinaId = tipoUHE.Id, EmpresaId = empresas[3].Id, CapacidadeInstalada = 3444, Localizacao = "SP/MS", DataOperacao = new DateTime(1973, 6, 13), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "XINGO", Nome = "UHE Xingó", TipoUsinaId = tipoUHE.Id, EmpresaId = empresas[4].Id, CapacidadeInstalada = 3162, Localizacao = "AL/SE", DataOperacao = new DateTime(1994, 12, 20), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "P_AFONSO", Nome = "UHE Paulo Afonso IV", TipoUsinaId = tipoUHE.Id, EmpresaId = empresas[4].Id, CapacidadeInstalada = 2462, Localizacao = "BA", DataOperacao = new DateTime(1979, 9, 20), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "ITUMBIARA", Nome = "UHE Itumbiara", TipoUsinaId = tipoUHE.Id, EmpresaId = empresas[3].Id, CapacidadeInstalada = 2082, Localizacao = "GO/MG", DataOperacao = new DateTime(1980, 5, 10), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "S_SIMAO", Nome = "UHE São Simão", TipoUsinaId = tipoUHE.Id, EmpresaId = empresas[3].Id, CapacidadeInstalada = 1710, Localizacao = "GO/MG", DataOperacao = new DateTime(1978, 2, 15), Ativo = true, DataCriacao = DateTime.UtcNow },

            // Termelétricas (10 principais)
            new() { Codigo = "ANGRA1", Nome = "UTE Angra 1", TipoUsinaId = tipoUTE.Id, EmpresaId = empresas[2].Id, CapacidadeInstalada = 640, Localizacao = "RJ", DataOperacao = new DateTime(1985, 1, 1), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "ANGRA2", Nome = "UTE Angra 2", TipoUsinaId = tipoUTE.Id, EmpresaId = empresas[2].Id, CapacidadeInstalada = 1350, Localizacao = "RJ", DataOperacao = new DateTime(2001, 2, 1), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "TERMONORTE", Nome = "UTE Termonorte I", TipoUsinaId = tipoUTE.Id, EmpresaId = empresas[6].Id, CapacidadeInstalada = 340, Localizacao = "MG", DataOperacao = new DateTime(2003, 6, 15), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "TERMOPE", Nome = "UTE Termopernambuco", TipoUsinaId = tipoUTE.Id, EmpresaId = empresas[14].Id, CapacidadeInstalada = 532, Localizacao = "PE", DataOperacao = new DateTime(2004, 3, 20), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "SUAPE2", Nome = "UTE Suape II", TipoUsinaId = tipoUTE.Id, EmpresaId = empresas[14].Id, CapacidadeInstalada = 380, Localizacao = "PE", DataOperacao = new DateTime(2013, 8, 10), Ativo = true, DataCriacao = DateTime.UtcNow },

            // Eólicas (10 parques)
            new() { Codigo = "LAGOA1", Nome = "EOL Lagoa dos Ventos I", TipoUsinaId = tipoEOL.Id, EmpresaId = empresas[27].Id, CapacidadeInstalada = 126, Localizacao = "PI", DataOperacao = new DateTime(2019, 7, 15), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "LAGOA2", Nome = "EOL Lagoa dos Ventos II", TipoUsinaId = tipoEOL.Id, EmpresaId = empresas[27].Id, CapacidadeInstalada = 120, Localizacao = "PI", DataOperacao = new DateTime(2020, 5, 20), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "DELTA1", Nome = "EOL Delta 1", TipoUsinaId = tipoEOL.Id, EmpresaId = empresas[25].Id, CapacidadeInstalada = 103, Localizacao = "PI", DataOperacao = new DateTime(2018, 11, 10), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "SERRA_MESA", Nome = "EOL Serra da Mesa", TipoUsinaId = tipoEOL.Id, EmpresaId = empresas[26].Id, CapacidadeInstalada = 96, Localizacao = "BA", DataOperacao = new DateTime(2017, 9, 5), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "ALTO_SERTAO", Nome = "EOL Alto Sertão II", TipoUsinaId = tipoEOL.Id, EmpresaId = empresas[28].Id, CapacidadeInstalada = 386, Localizacao = "BA", DataOperacao = new DateTime(2015, 12, 1), Ativo = true, DataCriacao = DateTime.UtcNow },

            // Solares (10 usinas)
            new() { Codigo = "S_GONCALO", Nome = "UFV São Gonçalo", TipoUsinaId = tipoUFV.Id, EmpresaId = empresas[20].Id, CapacidadeInstalada = 475, Localizacao = "PI", DataOperacao = new DateTime(2020, 6, 15), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "PIRAPORA", Nome = "UFV Pirapora", TipoUsinaId = tipoUFV.Id, EmpresaId = empresas[20].Id, CapacidadeInstalada = 321, Localizacao = "MG", DataOperacao = new DateTime(2017, 11, 20), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "SOL_NORDESTE", Nome = "UFV Sol do Nordeste", TipoUsinaId = tipoUFV.Id, EmpresaId = empresas[24].Id, CapacidadeInstalada = 292, Localizacao = "BA", DataOperacao = new DateTime(2019, 8, 10), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "GUAIMBÉ", Nome = "UFV Guaimbé", TipoUsinaId = tipoUFV.Id, EmpresaId = empresas[24].Id, CapacidadeInstalada = 150, Localizacao = "SP", DataOperacao = new DateTime(2018, 4, 5), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "JANAUBA", Nome = "UFV Janaúba", TipoUsinaId = tipoUFV.Id, EmpresaId = empresas[20].Id, CapacidadeInstalada = 186, Localizacao = "MG", DataOperacao = new DateTime(2017, 7, 12), Ativo = true, DataCriacao = DateTime.UtcNow },

            // PCHs (5 pequenas centrais)
            new() { Codigo = "PCH_SALTO", Nome = "PCH Salto", TipoUsinaId = tipoPCH.Id, EmpresaId = empresas[0].Id, CapacidadeInstalada = 28, Localizacao = "MG", DataOperacao = new DateTime(2010, 3, 15), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "PCH_CORUMBÁ", Nome = "PCH Corumbá", TipoUsinaId = tipoPCH.Id, EmpresaId = empresas[19].Id, CapacidadeInstalada = 95, Localizacao = "GO", DataOperacao = new DateTime(2005, 8, 20), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "PCH_CAPIM1", Nome = "PCH Capim Branco I", TipoUsinaId = tipoPCH.Id, EmpresaId = empresas[0].Id, CapacidadeInstalada = 240, Localizacao = "MG", DataOperacao = new DateTime(2006, 11, 10), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "PCH_CAPIM2", Nome = "PCH Capim Branco II", TipoUsinaId = tipoPCH.Id, EmpresaId = empresas[0].Id, CapacidadeInstalada = 210, Localizacao = "MG", DataOperacao = new DateTime(2007, 5, 25), Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Codigo = "PCH_PIRAPETINGA", Nome = "PCH Pirapetinga", TipoUsinaId = tipoPCH.Id, EmpresaId = empresas[0].Id, CapacidadeInstalada = 22, Localizacao = "MG", DataOperacao = new DateTime(2012, 9, 5), Ativo = true, DataCriacao = DateTime.UtcNow }
        };

        await context.Usinas.AddRangeAsync(usinas);
        await context.SaveChangesAsync();

        Console.WriteLine($"   ? {usinas.Count} usinas criadas");
    }

    private static async Task SeedUnidadesGeradoras(PdpwDbContext context)
    {
        Console.WriteLine("?? Criando Unidades Geradoras...");

        var usinas = await context.Usinas.ToListAsync();
        var unidades = new List<UnidadeGeradora>();

        foreach (var usina in usinas.Take(25)) // 25 usinas x 4 unidades = 100 unidades
        {
            var numUnidades = usina.CapacidadeInstalada > 1000 ? 4 : 2;
            var potenciaPorUnidade = usina.CapacidadeInstalada / numUnidades;

            for (int i = 1; i <= numUnidades; i++)
            {
                unidades.Add(new UnidadeGeradora
                {
                    Codigo = $"{usina.Codigo}-UG{i:D2}",
                    Nome = $"Unidade Geradora {i} - {usina.Nome}",
                    UsinaId = usina.Id,
                    PotenciaNominal = potenciaPorUnidade,
                    PotenciaMinima = potenciaPorUnidade * 0.3m,
                    DataComissionamento = usina.DataOperacao.AddMonths(i * 2),
                    Status = "OPERANDO",
                    Ativo = true,
                    DataCriacao = DateTime.UtcNow
                });
            }
        }

        await context.UnidadesGeradoras.AddRangeAsync(unidades);
        await context.SaveChangesAsync();

        Console.WriteLine($"   ? {unidades.Count} unidades geradoras criadas");
    }

    private static async Task SeedMotivosRestricao(PdpwDbContext context)
    {
        Console.WriteLine("?? Criando Motivos de Restrição...");

        var motivos = new List<MotivoRestricao>
        {
            new() { Nome = "Manutenção Programada", Descricao = "Parada programada para manutenção preventiva", Categoria = "PROGRAMADA", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Manutenção Corretiva", Descricao = "Parada não programada para correção de falhas", Categoria = "EMERGENCIAL", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Restrição Hidráulica", Descricao = "Nível de reservatório abaixo do mínimo operacional", Categoria = "OPERACIONAL", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Restrição Térmica", Descricao = "Temperatura acima dos limites operacionais", Categoria = "OPERACIONAL", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Restrição Elétrica", Descricao = "Limitação por restrições da rede elétrica", Categoria = "OPERACIONAL", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Falha de Equipamento", Descricao = "Equipamento com defeito ou avaria", Categoria = "EMERGENCIAL", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Teste Operacional", Descricao = "Testes de performance e operação", Categoria = "PROGRAMADA", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Inspeção Regulatória", Descricao = "Inspeção determinada por órgão regulador", Categoria = "PROGRAMADA", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Falta de Combustível", Descricao = "Falta de combustível para operação", Categoria = "OPERACIONAL", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Condições Climáticas", Descricao = "Restrição por condições meteorológicas adversas", Categoria = "OPERACIONAL", Ativo = true, DataCriacao = DateTime.UtcNow }
        };

        await context.MotivosRestricao.AddRangeAsync(motivos);
        await context.SaveChangesAsync();

        Console.WriteLine($"   ? {motivos.Count} motivos de restrição criados");
    }

    private static async Task SeedParadasUG(PdpwDbContext context)
    {
        Console.WriteLine("??  Criando Paradas de Unidades Geradoras...");

        var unidades = await context.UnidadesGeradoras.Take(25).ToListAsync();
        var motivos = await context.MotivosRestricao.ToListAsync();
        var paradas = new List<ParadaUG>();
        var random = new Random(42);

        for (int i = 0; i < 50; i++)
        {
            var unidade = unidades[random.Next(unidades.Count)];
            var motivo = motivos[random.Next(motivos.Count)];
            var dataInicio = DateTime.UtcNow.AddDays(-random.Next(1, 365));
            var duracao = random.Next(4, 72); // 4 a 72 horas

            paradas.Add(new ParadaUG
            {
                UnidadeGeradoraId = unidade.Id,
                DataInicio = dataInicio,
                DataFim = dataInicio.AddHours(duracao),
                MotivoParada = motivo.Nome,
                Observacoes = $"Parada para {motivo.Descricao.ToLower()}",
                Programada = motivo.Categoria == "PROGRAMADA",
                Ativo = true,
                DataCriacao = DateTime.UtcNow
            });
        }

        await context.ParadasUG.AddRangeAsync(paradas);
        await context.SaveChangesAsync();

        Console.WriteLine($"   ? {paradas.Count} paradas criadas");
    }

    private static async Task SeedBalancos(PdpwDbContext context)
    {
        Console.WriteLine("??  Criando Balanços Energéticos...");

        var subsistemas = new[] { "SE", "S", "NE", "N" };
        var balancos = new List<Balanco>();
        var random = new Random(42);

        for (int dia = 0; dia < 30; dia++)
        {
            var data = DateTime.UtcNow.AddDays(-dia);
            
            foreach (var subsistema in subsistemas)
            {
                var geracao = random.Next(30000, 80000);
                var carga = random.Next(28000, 75000);
                var intercambio = random.Next(-5000, 5000);
                var perdas = geracao * 0.03m; // 3% de perdas

                balancos.Add(new Balanco
                {
                    DataReferencia = data.Date,
                    SubsistemaId = subsistema,
                    Geracao = geracao,
                    Carga = carga,
                    Intercambio = intercambio,
                    Perdas = perdas,
                    Deficit = 0,
                    Observacoes = $"Balanço operacional {subsistema}",
                    Ativo = true,
                    DataCriacao = DateTime.UtcNow
                });
            }
        }

        await context.Balancos.AddRangeAsync(balancos);
        await context.SaveChangesAsync();

        Console.WriteLine($"   ? {balancos.Count} balanços criados");
    }

    private static async Task SeedIntercambios(PdpwDbContext context)
    {
        Console.WriteLine("?? Criando Intercâmbios...");

        var fluxos = new[]
        {
            ("SE", "S"), ("S", "SE"),
            ("SE", "NE"), ("NE", "SE"),
            ("S", "N"), ("N", "S"),
            ("NE", "N"), ("N", "NE")
        };

        var intercambios = new List<Intercambio>();
        var random = new Random(42);

        for (int dia = 0; dia < 30; dia++)
        {
            var data = DateTime.UtcNow.AddDays(-dia);

            foreach (var (origem, destino) in fluxos)
            {
                intercambios.Add(new Intercambio
                {
                    DataReferencia = data.Date,
                    SubsistemaOrigem = origem,
                    SubsistemaDestino = destino,
                    EnergiaIntercambiada = random.Next(500, 5000),
                    Observacoes = $"Intercâmbio {origem} ? {destino}",
                    Ativo = true,
                    DataCriacao = DateTime.UtcNow
                });
            }
        }

        await context.Intercambios.AddRangeAsync(intercambios);
        await context.SaveChangesAsync();

        Console.WriteLine($"   ? {intercambios.Count} intercâmbios criados");
    }

    private static async Task SeedSemanasPMO(PdpwDbContext context)
    {
        Console.WriteLine("?? Criando Semanas PMO...");

        var semanas = new List<SemanaPMO>();
        var dataBase = new DateTime(2024, 1, 1);

        for (int i = 0; i < 25; i++)
        {
            var dataInicio = dataBase.AddDays(i * 7);
            var dataFim = dataInicio.AddDays(6);

            semanas.Add(new SemanaPMO
            {
                Numero = i + 1,
                Ano = 2024,
                DataInicio = dataInicio,
                DataFim = dataFim,
                Observacoes = $"Semana Operativa {i + 1}/2024",
                Ativo = true,
                DataCriacao = DateTime.UtcNow
            });
        }

        await context.SemanasPMO.AddRangeAsync(semanas);
        await context.SaveChangesAsync();

        Console.WriteLine($"   ? {semanas.Count} semanas PMO criadas");
    }

    private static async Task SeedEquipesPDP(PdpwDbContext context)
    {
        Console.WriteLine("?? Criando Equipes PDP...");

        var equipes = new List<EquipePDP>
        {
            new() { Nome = "Equipe Sudeste/Centro-Oeste", Descricao = "Responsável pela região SE/CO", Coordenador = "João Silva", Email = "joao.silva@ons.org.br", Telefone = "(11) 3214-4800", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Equipe Sul", Descricao = "Responsável pela região Sul", Coordenador = "Maria Santos", Email = "maria.santos@ons.org.br", Telefone = "(48) 3231-5200", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Equipe Nordeste", Descricao = "Responsável pela região Nordeste", Coordenador = "Pedro Costa", Email = "pedro.costa@ons.org.br", Telefone = "(81) 3217-3000", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Equipe Norte", Descricao = "Responsável pela região Norte", Coordenador = "Ana Oliveira", Email = "ana.oliveira@ons.org.br", Telefone = "(61) 3429-5151", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Equipe Hidráulica", Descricao = "Especialistas em geração hidráulica", Coordenador = "Carlos Almeida", Email = "carlos.almeida@ons.org.br", Telefone = "(21) 2514-5151", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Equipe Térmica", Descricao = "Especialistas em geração térmica", Coordenador = "Fernanda Lima", Email = "fernanda.lima@ons.org.br", Telefone = "(21) 2514-5152", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Equipe Eólica", Descricao = "Especialistas em geração eólica", Coordenador = "Ricardo Souza", Email = "ricardo.souza@ons.org.br", Telefone = "(85) 3216-1000", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Equipe Solar", Descricao = "Especialistas em geração solar", Coordenador = "Juliana Pereira", Email = "juliana.pereira@ons.org.br", Telefone = "(85) 3216-1001", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Equipe Transmissão", Descricao = "Responsável por intercâmbios e transmissão", Coordenador = "Marcos Rodrigues", Email = "marcos.rodrigues@ons.org.br", Telefone = "(21) 2514-5153", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Equipe Planejamento", Descricao = "Planejamento energético", Coordenador = "Paula Martins", Email = "paula.martins@ons.org.br", Telefone = "(21) 2514-5154", Ativo = true, DataCriacao = DateTime.UtcNow },
            new() { Nome = "Equipe Análise", Descricao = "Análise de dados e relatórios", Coordenador = "Roberto Ferreira", Email = "roberto.ferreira@ons.org.br", Telefone = "(21) 2514-5155", Ativo = true, DataCriacao = DateTime.UtcNow }
        };

        await context.EquipesPDP.AddRangeAsync(equipes);
        await context.SaveChangesAsync();

        Console.WriteLine($"   ? {equipes.Count} equipes PDP criadas");
    }
}
