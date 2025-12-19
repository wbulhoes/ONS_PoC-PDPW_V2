using Microsoft.AspNetCore.Mvc;
using PDPW.Domain.Common;

namespace PDPW.API.Extensions;

/// <summary>
/// Extensões para trabalhar com Result Pattern nos Controllers
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Converte um Result em ActionResult apropriado
    /// </summary>
    /// <typeparam name="T">Tipo do valor</typeparam>
    /// <param name="result">Resultado da operação</param>
    /// <param name="controller">Controller base</param>
    /// <returns>ActionResult apropriado</returns>
    public static ActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
    {
        if (result.IsSuccess)
        {
            return controller.Ok(result.Value);
        }

        // NotFound
        if (result.Error.Contains("não foi encontrado", StringComparison.OrdinalIgnoreCase))
        {
            return controller.NotFound(new { message = result.Error });
        }

        // Conflict (duplicação, etc)
        if (result.Error.Contains("já existe", StringComparison.OrdinalIgnoreCase) ||
            result.Error.Contains("duplicado", StringComparison.OrdinalIgnoreCase))
        {
            return controller.Conflict(new { message = result.Error });
        }

        // Validation errors
        if (result.ValidationErrors != null && result.ValidationErrors.Any())
        {
            return controller.BadRequest(new
            {
                message = result.Error,
                errors = result.ValidationErrors
            });
        }

        // BadRequest genérico
        return controller.BadRequest(new { message = result.Error });
    }

    /// <summary>
    /// Converte um Result sem valor em ActionResult apropriado
    /// </summary>
    /// <param name="result">Resultado da operação</param>
    /// <param name="controller">Controller base</param>
    /// <returns>ActionResult apropriado</returns>
    public static ActionResult ToActionResult(this Result result, ControllerBase controller)
    {
        if (result.IsSuccess)
        {
            return controller.NoContent();
        }

        // NotFound
        if (result.Error.Contains("não foi encontrado", StringComparison.OrdinalIgnoreCase))
        {
            return controller.NotFound(new { message = result.Error });
        }

        // Validation errors
        if (result.ValidationErrors != null && result.ValidationErrors.Any())
        {
            return controller.BadRequest(new
            {
                message = result.Error,
                errors = result.ValidationErrors
            });
        }

        // BadRequest genérico
        return controller.BadRequest(new { message = result.Error });
    }

    /// <summary>
    /// Converte um Result em CreatedAtActionResult
    /// </summary>
    /// <typeparam name="T">Tipo do valor</typeparam>
    /// <param name="result">Resultado da operação</param>
    /// <param name="controller">Controller base</param>
    /// <param name="actionName">Nome da action</param>
    /// <param name="routeValues">Valores da rota</param>
    /// <returns>CreatedAtActionResult ou erro apropriado</returns>
    public static ActionResult ToCreatedAtActionResult<T>(
        this Result<T> result,
        ControllerBase controller,
        string actionName,
        object routeValues)
    {
        if (result.IsSuccess)
        {
            return controller.CreatedAtAction(actionName, routeValues, result.Value);
        }

        // Se falhou, usar mesma lógica de ToActionResult
        return result.ToActionResult(controller);
    }
}
