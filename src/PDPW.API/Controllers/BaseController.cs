using Microsoft.AspNetCore.Mvc;

namespace PDPW.API.Controllers;

/// <summary>
/// Controller base com funcionalidades comuns
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Retorna resultado OK ou NotFound baseado no valor
    /// </summary>
    protected IActionResult HandleResult<T>(T? result)
    {
        if (result == null)
            return NotFound(new { message = "Recurso não encontrado" });

        return Ok(result);
    }

    /// <summary>
    /// Retorna resultado OK ou NotFound para coleções
    /// </summary>
    protected IActionResult HandleCollectionResult<T>(IEnumerable<T>? result)
    {
        if (result == null || !result.Any())
            return Ok(new List<T>()); // Retorna lista vazia ao invés de 404

        return Ok(result);
    }

    /// <summary>
    /// Trata exceções e retorna erro padronizado
    /// </summary>
    protected IActionResult HandleError(Exception ex)
    {
        // Log da exceção (implementar logger depois)
        Console.WriteLine($"Erro: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");

        return StatusCode(500, new
        {
            error = "Erro interno do servidor",
            message = ex.Message,
            // Em produção, não expor detalhes da exceção
#if DEBUG
            details = ex.ToString()
#endif
        });
    }

    /// <summary>
    /// Retorna BadRequest com mensagens de validação
    /// </summary>
    protected IActionResult HandleValidationError(string message)
    {
        return BadRequest(new { error = "Erro de validação", message });
    }

    /// <summary>
    /// Retorna Created (201) com location header
    /// </summary>
    protected IActionResult HandleCreated<T>(string routeName, object routeValues, T result)
    {
        return CreatedAtRoute(routeName, routeValues, result);
    }

    /// <summary>
    /// Retorna NoContent (204) para updates/deletes
    /// </summary>
    protected IActionResult HandleNoContent()
    {
        return NoContent();
    }
}
