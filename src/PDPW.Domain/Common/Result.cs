namespace PDPW.Domain.Common;

/// <summary>
/// Representa o resultado de uma operação que pode ter sucesso ou falhar
/// </summary>
/// <typeparam name="T">Tipo do valor retornado em caso de sucesso</typeparam>
public class Result<T>
{
    /// <summary>
    /// Indica se a operação foi bem-sucedida
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Indica se a operação falhou
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Valor retornado em caso de sucesso
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Mensagem de erro em caso de falha
    /// </summary>
    public string Error { get; }

    /// <summary>
    /// Lista de erros de validação (opcional)
    /// </summary>
    public IReadOnlyList<string>? ValidationErrors { get; }

    private Result(bool isSuccess, T? value, string error, IReadOnlyList<string>? validationErrors = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        ValidationErrors = validationErrors;
    }

    /// <summary>
    /// Cria um resultado de sucesso
    /// </summary>
    /// <param name="value">Valor retornado</param>
    /// <returns>Resultado de sucesso</returns>
    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, string.Empty);
    }

    /// <summary>
    /// Cria um resultado de falha
    /// </summary>
    /// <param name="error">Mensagem de erro</param>
    /// <returns>Resultado de falha</returns>
    public static Result<T> Failure(string error)
    {
        return new Result<T>(false, default, error);
    }

    /// <summary>
    /// Cria um resultado de falha com múltiplos erros de validação
    /// </summary>
    /// <param name="error">Mensagem de erro principal</param>
    /// <param name="validationErrors">Lista de erros de validação</param>
    /// <returns>Resultado de falha com validações</returns>
    public static Result<T> ValidationFailure(string error, IReadOnlyList<string> validationErrors)
    {
        return new Result<T>(false, default, error, validationErrors);
    }

    /// <summary>
    /// Cria um resultado de falha indicando que o recurso não foi encontrado
    /// </summary>
    /// <param name="entityName">Nome da entidade não encontrada</param>
    /// <param name="key">Chave de busca</param>
    /// <returns>Resultado de falha</returns>
    public static Result<T> NotFound(string entityName, object key)
    {
        return Failure($"{entityName} com identificador '{key}' não foi encontrado(a)");
    }

    /// <summary>
    /// Cria um resultado de falha indicando conflito (ex: registro duplicado)
    /// </summary>
    /// <param name="error">Mensagem de erro</param>
    /// <returns>Resultado de falha</returns>
    public static Result<T> Conflict(string error)
    {
        return Failure(error);
    }
}

/// <summary>
/// Representa o resultado de uma operação sem valor de retorno
/// </summary>
public class Result
{
    /// <summary>
    /// Indica se a operação foi bem-sucedida
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Indica se a operação falhou
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Mensagem de erro em caso de falha
    /// </summary>
    public string Error { get; }

    /// <summary>
    /// Lista de erros de validação (opcional)
    /// </summary>
    public IReadOnlyList<string>? ValidationErrors { get; }

    private Result(bool isSuccess, string error, IReadOnlyList<string>? validationErrors = null)
    {
        IsSuccess = isSuccess;
        Error = error;
        ValidationErrors = validationErrors;
    }

    /// <summary>
    /// Cria um resultado de sucesso
    /// </summary>
    /// <returns>Resultado de sucesso</returns>
    public static Result Success()
    {
        return new Result(true, string.Empty);
    }

    /// <summary>
    /// Cria um resultado de falha
    /// </summary>
    /// <param name="error">Mensagem de erro</param>
    /// <returns>Resultado de falha</returns>
    public static Result Failure(string error)
    {
        return new Result(false, error);
    }

    /// <summary>
    /// Cria um resultado de falha com múltiplos erros de validação
    /// </summary>
    /// <param name="error">Mensagem de erro principal</param>
    /// <param name="validationErrors">Lista de erros de validação</param>
    /// <returns>Resultado de falha com validações</returns>
    public static Result ValidationFailure(string error, IReadOnlyList<string> validationErrors)
    {
        return new Result(false, error, validationErrors);
    }
}
