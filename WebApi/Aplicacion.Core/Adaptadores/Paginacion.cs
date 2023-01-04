namespace Aplicacion.Core.Adaptadores;

public class Paginacion<T> where T : class
{
    public int Count { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public IReadOnlyList<T> Data { get; set; }
    public int PageCount { get; set; }
}

public class PaginacionObject<T> where T : class
{
    public int Count { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public T Data { get; set; }
    public int PageCount { get; set; }
}
