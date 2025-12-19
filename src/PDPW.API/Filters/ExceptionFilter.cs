using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PDPW.API.Filters;

/// <summary>
/// Filtro para tratamento global de exceções
/// </summary>
public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionFilter(ILogger<ExceptionFilter> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Erro não tratado: {Message}", context.Exception.Message);

        var response = new
        {
            error = "Erro interno do servidor",
            message = "Ocorreu um erro ao processar sua solicitação",
            // Detalhes apenas em desenvolvimento
            details = _env.IsDevelopment() ? context.Exception.ToString() : null
        };

        context.Result = new ObjectResult(response)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }
}
