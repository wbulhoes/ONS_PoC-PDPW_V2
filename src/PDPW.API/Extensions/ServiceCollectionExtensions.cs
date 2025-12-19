using Microsoft.EntityFrameworkCore;
using PDPW.Application.Interfaces;
using PDPW.Application.Services;
using PDPW.Domain.Interfaces;
using PDPW.Infrastructure.Data;
using PDPW.Infrastructure.Repositories;

namespace PDPW.API.Extensions;

/// <summary>
/// Extensões para configuração de serviços
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adiciona configuração do banco de dados
    /// </summary>
    public static IServiceCollection AddDatabaseConfiguration(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<PdpwDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null
                );
            });

            // Habilitar sensitive data logging apenas em desenvolvimento
            if (configuration.GetValue<bool>("EnableSensitiveDataLogging"))
            {
                options.EnableSensitiveDataLogging();
            }
        });

        return services;
    }

    /// <summary>
    /// Adiciona AutoMapper
    /// </summary>
    public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }

    /// <summary>
    /// Adiciona configuração de CORS
    /// </summary>
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

            options.AddPolicy("Development", builder =>
            {
                builder.WithOrigins("http://localhost:5173", "http://localhost:3000")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            });
        });

        return services;
    }

    /// <summary>
    /// Adiciona configuração do Swagger
    /// </summary>
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new()
            {
                Title = "PDPW API",
                Version = "v1",
                Description = "API para Programação Diária da Produção de Energia",
                Contact = new()
                {
                    Name = "ONS - Operador Nacional do Sistema",
                    Email = "contato@ons.org.br"
                }
            });

            // Incluir comentários XML
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            // Ordenar por controller
            c.OrderActionsBy(apiDesc => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");
        });

        return services;
    }

    /// <summary>
    /// Adiciona repositórios e serviços da aplicação
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // === REPOSITÓRIOS ===
        services.AddScoped<IUsinaRepository, UsinaRepository>();
        services.AddScoped<ITipoUsinaRepository, TipoUsinaRepository>();
        services.AddScoped<IEmpresaRepository, EmpresaRepository>();
        services.AddScoped<ISemanaPmoRepository, SemanaPmoRepository>();

        // === SERVICES ===
        services.AddScoped<IUsinaService, UsinaService>();
        services.AddScoped<ITipoUsinaService, TipoUsinaService>();
        services.AddScoped<IEmpresaService, EmpresaService>();
        services.AddScoped<ISemanaPmoService, SemanaPmoService>();

        // Outros repositórios e services serão adicionados aqui conforme forem criados
        
        return services;
    }
}
