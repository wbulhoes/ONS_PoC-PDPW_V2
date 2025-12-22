using Microsoft.EntityFrameworkCore;
using PDPW.Infrastructure.Data;
using PDPW.Infrastructure.Data.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Configurar InMemory Database
builder.Services.AddDbContext<PdpwDbContext>(options =>
{
    options.UseInMemoryDatabase("PDPW_Test_Seed");
});

var app = builder.Build();

// Executar seed e mostrar resultados
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PdpwDbContext>();
    
    Console.WriteLine("========================================");
    Console.WriteLine("  TESTE DE SEED - DADOS REALISTAS");
    Console.WriteLine("========================================");
    Console.WriteLine();
    
    // Popular dados
    await RealisticDataSeeder.SeedAsync(context);
    
    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("  VERIFICAÇÃO DOS DADOS POPULADOS");
    Console.WriteLine("========================================");
    Console.WriteLine();
    
    // Verificar dados
    var empresas = await context.Empresas.CountAsync();
    var usinas = await context.Usinas.CountAsync();
    var tiposUsina = await context.TiposUsina.CountAsync();
    var unidadesGeradoras = await context.UnidadesGeradoras.CountAsync();
    var motivosRestricao = await context.MotivosRestricao.CountAsync();
    var paradasUG = await context.ParadasUG.CountAsync();
    var balancos = await context.Balancos.CountAsync();
    var intercambios = await context.Intercambios.CountAsync();
    var semanasPMO = await context.SemanasPMO.CountAsync();
    var equipesPDP = await context.EquipesPDP.CountAsync();
    
    Console.WriteLine($"? {tiposUsina} Tipos de Usina");
    Console.WriteLine($"? {empresas} Empresas");
    Console.WriteLine($"? {usinas} Usinas");
    Console.WriteLine($"? {unidadesGeradoras} Unidades Geradoras");
    Console.WriteLine($"? {motivosRestricao} Motivos de Restrição");
    Console.WriteLine($"? {paradasUG} Paradas UG");
    Console.WriteLine($"? {balancos} Balanços");
    Console.WriteLine($"? {intercambios} Intercâmbios");
    Console.WriteLine($"? {semanasPMO} Semanas PMO");
    Console.WriteLine($"? {equipesPDP} Equipes PDP");
    
    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine($"  TOTAL: {empresas + usinas + unidadesGeradoras + motivosRestricao + paradasUG + balancos + intercambios + semanasPMO + equipesPDP + tiposUsina} REGISTROS");
    Console.WriteLine("========================================");
    Console.WriteLine();
    
    // Mostrar exemplos
    Console.WriteLine("?? EXEMPLOS DE DADOS:");
    Console.WriteLine();
    
    Console.WriteLine("?? Top 5 Empresas:");
    var top5Empresas = await context.Empresas.Take(5).ToListAsync();
    foreach (var emp in top5Empresas)
    {
        Console.WriteLine($"   - {emp.Nome} (CNPJ: {emp.CNPJ})");
    }
    
    Console.WriteLine();
    Console.WriteLine("? Top 5 Usinas:");
    var top5Usinas = await context.Usinas.Include(u => u.TipoUsina).Take(5).ToListAsync();
    foreach (var usina in top5Usinas)
    {
        Console.WriteLine($"   - {usina.Nome} ({usina.TipoUsina?.Nome}) - {usina.CapacidadeInstalada} MW");
    }
    
    Console.WriteLine();
    Console.WriteLine("?? Top 5 Unidades Geradoras:");
    var top5UGs = await context.UnidadesGeradoras.Include(u => u.Usina).Take(5).ToListAsync();
    foreach (var ug in top5UGs)
    {
        Console.WriteLine($"   - {ug.Codigo}: {ug.Nome} - {ug.PotenciaNominal} MW");
    }
}

Console.WriteLine();
Console.WriteLine("? Teste de seed concluído com sucesso!");
