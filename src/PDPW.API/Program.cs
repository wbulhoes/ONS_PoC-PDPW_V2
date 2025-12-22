using Microsoft.EntityFrameworkCore;
using PDPW.API.Extensions;
using PDPW.API.Filters;
using PDPW.API.Middlewares;
using PDPW.Application.Interfaces;
using PDPW.Application.Services;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;
using PDPW.Infrastructure.Data.Seeders;
using PDPW.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner
builder.Services.AddControllers(options =>
{
    // Adiciona filtros globais
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<ExceptionFilter>();
});

// Configurações usando Extension Methods
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddAutoMapperConfiguration();
builder.Services.AddCorsConfiguration();
builder.Services.AddSwaggerConfiguration();

// Injeção de dependências - Repositórios e Services existentes
builder.Services.AddScoped<IDadoEnergeticoRepository, DadoEnergeticoRepository>();
builder.Services.AddScoped<IDadoEnergeticoService, DadoEnergeticoService>();

// Adicionar novos serviços conforme APIs forem criadas
builder.Services.AddApplicationServices();

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<PdpwDbContext>("database");

var app = builder.Build();

// Middleware de erro customizado (primeira coisa no pipeline)
app.UseErrorHandling();

// Testar conexão com banco de dados e popular dados realistas
try
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<PdpwDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    var useInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");
    
    if (useInMemoryDatabase)
    {
        logger.LogInformation("🗄️ Banco de dados InMemory inicializado (dados temporários)");
        await dbContext.Database.EnsureCreatedAsync();
        
        // Popular com dados realistas
        logger.LogInformation("📊 Populando banco com dados realistas do setor elétrico brasileiro...");
        await RealisticDataSeeder.SeedAsync(dbContext);
    }
    else
    {
        logger.LogInformation("Testando conexão com o banco de dados SQL Server...");
        
        if (await dbContext.Database.CanConnectAsync())
        {
            logger.LogInformation("✓ Conexão com banco de dados estabelecida com sucesso!");
            
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                logger.LogWarning("⚠ Há {Count} migrações pendentes no banco de dados", pendingMigrations.Count());
                logger.LogInformation("Para aplicar as migrações, execute: dotnet ef database update");
            }
            else
            {
                // Popular com dados realistas se o banco estiver vazio
                // FORÇA SEED PARA POPULAR DADOS COMPLETOS
                logger.LogInformation("📊 Forçando seed de dados realistas do setor elétrico brasileiro...");
                await RealisticDataSeeder.SeedAsync(dbContext);
            }
        }
        else
        {
            logger.LogWarning("⚠ Não foi possível conectar ao banco de dados SQL Server");
            logger.LogInformation("💡 Dica: Configure UseInMemoryDatabase=true no appsettings para usar banco em memória");
        }
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "❌ Erro ao testar conexão com banco de dados: {Message}", ex.Message);
}

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PDPW API v1");
        c.RoutePrefix = "swagger";
    });
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// CORS - usar política do ambiente
var corsPolicy = app.Environment.IsDevelopment() ? "Development" : "AllowAll";
app.UseCors(corsPolicy);

app.UseAuthorization();

// Endpoints
app.MapHealthChecks("/health");

app.MapGet("/", () => Results.Ok(new 
{ 
    status = "running",
    application = "PDPW API",
    version = "v1",
    environment = app.Environment.EnvironmentName,
    timestamp = DateTime.UtcNow
}));

app.MapControllers();

try
{
    app.Logger.LogInformation("🚀 Iniciando aplicação PDPW API...");
    app.Logger.LogInformation("📊 Ambiente: {Environment}", app.Environment.EnvironmentName);
    app.Logger.LogInformation("📖 Swagger: {SwaggerUrl}", app.Environment.IsDevelopment() ? "http://localhost:5001/swagger" : "Desabilitado");
    app.Run();
}
catch (Exception ex)
{
    app.Logger.LogCritical(ex, "❌ Aplicação encerrada inesperadamente: {Message}", ex.Message);
    throw;
}

/// <summary>
/// Classe Program pública para permitir testes de integração
/// </summary>
public partial class Program { }
