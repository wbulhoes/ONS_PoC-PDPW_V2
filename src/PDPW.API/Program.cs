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

// Usuarios - NOVO
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// Adicionar novos serviços conforme APIs forem criadas
builder.Services.AddApplicationServices();

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<PdpwDbContext>("database");

var app = builder.Build();

// Middleware de erro customizado (primeira coisa no pipeline)
app.UseErrorHandling();

// Testar conexão com banco de dados e aplicar migrations automaticamente
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
    }
    else
    {
        logger.LogInformation("🔧 Testando conexão com o banco de dados SQL Server...");
        
        // Tenta conectar ao banco
        var maxRetries = 10;
        var retryCount = 0;
        var connected = false;
        
        while (retryCount < maxRetries && !connected)
        {
            try
            {
                connected = await dbContext.Database.CanConnectAsync();
                
                if (connected)
                {
                    logger.LogInformation("✅ Conexão com banco de dados estabelecida com sucesso!");
                    
                    // APLICAR MIGRATIONS AUTOMATICAMENTE
                    logger.LogInformation("🚀 Verificando migrations pendentes...");
                    var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
                    
                    if (pendingMigrations.Any())
                    {
                        logger.LogWarning("📦 Encontradas {Count} migrations pendentes. Aplicando...", pendingMigrations.Count());
                        
                        foreach (var migration in pendingMigrations)
                        {
                            logger.LogInformation("   - {Migration}", migration);
                        }
                        
                        await dbContext.Database.MigrateAsync();
                        logger.LogInformation("✅ Migrations aplicadas com sucesso!");
                    }
                    else
                    {
                        logger.LogInformation("✅ Banco de dados já está atualizado (sem migrations pendentes)");
                    }
                    
                    // Verificar se há dados (contar registros principais)
                    var empresasCount = await dbContext.Empresas.CountAsync();
                    var usinasCount = await dbContext.Usinas.CountAsync();
                    var semanasPMOCount = await dbContext.SemanasPMO.CountAsync();
                    
                    logger.LogInformation("📊 Registros no banco:");
                    logger.LogInformation("   - Empresas: {Count}", empresasCount);
                    logger.LogInformation("   - Usinas: {Count}", usinasCount);
                    logger.LogInformation("   - Semanas PMO: {Count}", semanasPMOCount);
                    
                    if (empresasCount == 0 || usinasCount == 0 || semanasPMOCount == 0)
                    {
                        logger.LogWarning("⚠️  Banco parece estar vazio! Os dados de seed devem ter sido inseridos pela migration.");
                        logger.LogInformation("💡 Se não houver dados, verifique se a migration 'SeedUnicoConsolidado' foi aplicada corretamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                retryCount++;
                if (retryCount < maxRetries)
                {
                    logger.LogWarning("⚠️  Tentativa {Retry}/{MaxRetries} falhou. Aguardando 3 segundos antes de tentar novamente...", retryCount, maxRetries);
                    logger.LogWarning("   Erro: {Message}", ex.Message);
                    await Task.Delay(3000);
                }
                else
                {
                    logger.LogError(ex, "❌ Não foi possível conectar ao banco de dados após {MaxRetries} tentativas", maxRetries);
                    logger.LogInformation("💡 Dica: Configure UseInMemoryDatabase=true no appsettings para usar banco em memória");
                }
            }
        }
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "❌ Erro crítico ao inicializar banco de dados: {Message}", ex.Message);
    // Não lançar exceção para permitir que a API inicie mesmo sem banco
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
