using Microsoft.EntityFrameworkCore;
using PDPW.Application.Interfaces;
using PDPW.Application.Services;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;
using PDPW.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Adiciona serviços ao contêiner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "PDPW API", 
        Version = "v1",
        Description = "API modernizada do sistema PDPW - Programação Diária da Produção"
    });
});

// Configuração de banco de dados
var useInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PdpwDbContext>(options =>
{
    if (useInMemoryDatabase)
    {
        options.UseInMemoryDatabase("PDPW_InMemory");
        builder.Logging.AddConsole();
        Console.WriteLine("🗄️ Usando banco de dados InMemory (dados temporários)");
    }
    else
    {
        options.UseSqlServer(connectionString);
    }
    
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// Injeção de dependências
builder.Services.AddScoped<IDadoEnergeticoRepository, DadoEnergeticoRepository>();
builder.Services.AddScoped<IDadoEnergeticoService, DadoEnergeticoService>();

// Configuração de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Adicionar Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<PdpwDbContext>("database");

var app = builder.Build();

// Testar conexão com banco de dados na inicialização
try
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<PdpwDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    if (useInMemoryDatabase)
    {
        logger.LogInformation("🗄️ Banco de dados InMemory inicializado (dados temporários)");
        // Garante que o banco InMemory está criado
        await dbContext.Database.EnsureCreatedAsync();
    }
    else
    {
        logger.LogInformation("Testando conexão com o banco de dados SQL Server...");
        
        if (await dbContext.Database.CanConnectAsync())
        {
            logger.LogInformation("✓ Conexão com banco de dados estabelecida com sucesso!");
            
            // Verifica se há migrações pendentes
            var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                logger.LogWarning("⚠ Há {Count} migrações pendentes no banco de dados", pendingMigrations.Count());
                logger.LogInformation("Para aplicar as migrações, execute: dotnet ef database update");
            }
        }
        else
        {
            logger.LogWarning("⚠ Não foi possível conectar ao banco de dados SQL Server");
            logger.LogWarning("A aplicação continuará funcionando, mas operações de banco falharão");
            logger.LogInformation("💡 Dica: Configure UseInMemoryDatabase=true no appsettings para usar banco em memória");
        }
    }
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "❌ Erro ao testar conexão com banco de dados: {Message}", ex.Message);
    logger.LogWarning("A aplicação continuará funcionando, mas operações de banco falharão");
    if (!useInMemoryDatabase)
    {
        logger.LogInformation("💡 Dica: Configure UseInMemoryDatabase=true no appsettings para usar banco em memória");
    }
}

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthorization();

// Endpoint de health check
app.MapHealthChecks("/health");

// Endpoint raiz para teste
app.MapGet("/", () => Results.Ok(new 
{ 
    status = "running",
    application = "PDPW API",
    version = "v1",
    databaseType = useInMemoryDatabase ? "InMemory (temporário)" : "SQL Server",
    timestamp = DateTime.UtcNow
}));

app.MapControllers();

try
{
    app.Logger.LogInformation("Iniciando aplicação PDPW API...");
    app.Run();
}
catch (Exception ex)
{
    app.Logger.LogCritical(ex, "❌ Aplicação encerrada inesperadamente: {Message}", ex.Message);
    throw;
}
