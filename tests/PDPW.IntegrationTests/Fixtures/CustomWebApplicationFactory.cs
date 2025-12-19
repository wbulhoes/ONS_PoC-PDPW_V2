using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PDPW.Infrastructure.Data;

namespace PDPW.IntegrationTests.Fixtures;

/// <summary>
/// Factory personalizada para testes de integração
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove o DbContext existente
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<PdpwDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Adiciona DbContext com InMemory para testes
            services.AddDbContext<PdpwDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryTestDb");
            });

            // Build service provider
            var sp = services.BuildServiceProvider();

            // Cria escopo e obtém DbContext
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<PdpwDbContext>();

            // Garante que o banco está criado
            db.Database.EnsureCreated();

            // Seed dados de teste se necessário
            SeedTestData(db);
        });
    }

    private void SeedTestData(PdpwDbContext context)
    {
        // Limpa dados existentes
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Adiciona dados de teste básicos
        // Você pode adicionar mais conforme necessário
    }
}
