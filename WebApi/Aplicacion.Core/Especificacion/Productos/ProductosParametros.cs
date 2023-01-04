using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Reflection;

namespace Aplicacion.Core.Especificacion.Productos;

public class ProductosParametros
{
    /* Columnas de especificacion*/
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public string? Categoria { get; set; }
    public bool? Estado { get; set; }
    /*---------------------------*/

    public string? Ordenar { get; set; }

    private int _indicePagina { get; set; } = 1;
    public int IndicePagina
    {
        get => _indicePagina;
        set => _indicePagina = (value == 0) ? 1 : value;
    }

    private const int MaxTamanoPagina = 50;

    private int _tamanoPagina = 3;
    public int TamanoPagina
    {
        get => _tamanoPagina;
        set => _tamanoPagina = (value > MaxTamanoPagina) ? MaxTamanoPagina : value;
    }
    public string? Busqueda { get; set; }

    public static ValueTask<ProductosParametros?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        string nombre = context.Request.Query["Nombre"];
        string descripcion = context.Request.Query["Descripcion"];
        string categoria = context.Request.Query["Categoria"];
        string ordenar = context.Request.Query["Ordenar"];
        int.TryParse(context.Request.Query["IndicePagina"], out var indicePagina);
        int.TryParse(context.Request.Query["TamanoPagina"], out var tamanoPagina);
        string busqueda = context.Request.Query["Busqueda"];

        var result = new ProductosParametros
        {
            Nombre = nombre,
            Categoria = categoria,
            Descripcion = descripcion,
            TamanoPagina = tamanoPagina == 0 ? 3 : tamanoPagina,
            IndicePagina = indicePagina == 0 ? 1 : indicePagina,
            Busqueda = busqueda,
            Ordenar = ordenar
        };
        return ValueTask.FromResult<ProductosParametros?>(result);
    }
}
