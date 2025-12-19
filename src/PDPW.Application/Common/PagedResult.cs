namespace PDPW.Application.Common;

/// <summary>
/// Resultado paginado
/// </summary>
public class PagedResult<T>
{
    /// <summary>
    /// Número da página atual
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Tamanho da página
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total de registros
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Total de páginas
    /// </summary>
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    /// <summary>
    /// Tem página anterior
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Tem próxima página
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>
    /// Dados da página atual
    /// </summary>
    public IEnumerable<T> Data { get; set; } = new List<T>();

    /// <summary>
    /// Cria um resultado paginado
    /// </summary>
    public static PagedResult<T> Create(IEnumerable<T> source, int pageNumber, int pageSize, int totalCount)
    {
        return new PagedResult<T>
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            Data = source
        };
    }
}
