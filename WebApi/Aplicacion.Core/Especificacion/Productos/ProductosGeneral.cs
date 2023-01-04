using Dominio.Principal.Entidades;

namespace Aplicacion.Core.Especificacion.Productos;

public class ProductosGeneral : BaseEspecificacion<Producto>
{
    public ProductosGeneral(ProductosParametros parametros)
        : base(
            x =>
                //Busqueda principal por conincidencia de parametro nombre
                (string.IsNullOrEmpty(parametros.Busqueda) || x.Nombre.Contains(parametros.Busqueda)) &&
                (string.IsNullOrEmpty(parametros.Nombre) || x.Nombre == parametros.Nombre) &&
                (string.IsNullOrEmpty(parametros.Descripcion) || x.Descripcion == parametros.Descripcion) &&
                (string.IsNullOrEmpty(parametros.Categoria) || x.Categoria.Nombre== parametros.Categoria)
                && (!parametros.Estado.HasValue || x.Estado == parametros.Estado)
        )
    {
        AgregarInclusion(x => x.Categoria);
        AplicarPaginacion(parametros.TamanoPagina * (parametros.IndicePagina - 1), parametros.TamanoPagina);

        if (!string.IsNullOrEmpty(parametros.Ordenar))
        {
            switch (parametros.Ordenar)
            {
                case "nombreAsc":
                    AgregarOrdenAsc(c => c.Nombre!);
                    break;
                case "nombreDesc":
                    AgregarOrdenDesc(c => c.Nombre!);
                    break;
                case "categoriaAsc":
                    AgregarOrdenAsc(c => c.Categoria!.Nombre!);
                    break;
                case "categoriaDesc":
                    AgregarOrdenDesc(c => c.Categoria!.Nombre!);
                    break;
                default:
                    AgregarOrdenAsc(c => c.Nombre!);
                    break;
            }
        }
    }
    public ProductosGeneral(int id) : base(x => x.IdProductoPk == id)
    {
        AgregarInclusion(x => x.Categoria);
    }
}
