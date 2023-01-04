namespace Aplicacion.Core.Especificacion.Categorias;

public class CategoriasParametros
{
    /* Columnas de especificacion*/
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public bool? Estado { get; set; }
    /*---------------------------*/

    public string? Ordenar { get; set; }
    public int IndicePagina { get; set; } = 1;

    private const int MaxTamanoPagina = 50;

    private int _tamañoPagina = 3;
    public int TamañoPagina
    {
        get => _tamañoPagina;
        set => _tamañoPagina = (value > MaxTamanoPagina) ? MaxTamanoPagina : value;
    }

    public string? Busqueda { get; set; }
}
