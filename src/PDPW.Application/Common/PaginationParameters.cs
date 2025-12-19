namespace PDPW.Application.Common;

/// <summary>
/// Parâmetros de paginação
/// </summary>
public class PaginationParameters
{
    private const int MaxPageSize = 100;
    private int _pageSize = 10;

    /// <summary>
    /// Número da página (começa em 1)
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Tamanho da página (máximo 100)
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    /// <summary>
    /// Campo para ordenação
    /// </summary>
    public string? OrderBy { get; set; }

    /// <summary>
    /// Direção da ordenação (asc/desc)
    /// </summary>
    public string OrderDirection { get; set; } = "asc";
}
