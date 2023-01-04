using Dominio.Principal.Entidades;

namespace Aplicacion.Core.Especificacion.Categorias;

public class CategoriasGeneral : BaseEspecificacion<Categoria>
{
    public CategoriasGeneral(CategoriasParametros parametros)
        : base(
            x =>
                //Busqueda principal por conincidencia de parametro nombre
                (string.IsNullOrEmpty(parametros.Busqueda) || x.Nombre.Contains(parametros.Busqueda)) &&
                (string.IsNullOrEmpty(parametros.Nombre) || x.Nombre == parametros.Nombre) &&
                (string.IsNullOrEmpty(parametros.Descripcion) || x.Descripcion == parametros.Descripcion) 
                && (!parametros.Estado.HasValue || x.Estado == parametros.Estado)
        )
    {
        AplicarPaginacion(parametros.TamañoPagina * (parametros.IndicePagina - 1), parametros.TamañoPagina);

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
                default:
                    AgregarOrdenAsc(c => c.Nombre!);
                    break;
            }
        }
    }
    public CategoriasGeneral(int id) : base(x => x.IdCategoriaPk == id)
    {

    }
}
