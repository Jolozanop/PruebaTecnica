using Aplicacion.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Data.Repositorios;

public class Especificacion<T> where T : class
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, IEspecificacion<T> espec)
    {
        if (espec.Criterio != null)
        {
            inputQuery = inputQuery.Where(espec.Criterio);
        }

        if (espec.OrdenarAsc != null)
        {
            inputQuery = inputQuery.OrderBy(espec.OrdenarAsc);
        }

        if (espec.OrdenarDesc != null)
        {
            inputQuery = inputQuery.OrderByDescending(espec.OrdenarDesc);
        }

        if (espec.HabilitadoPaginar)
        {
            inputQuery = inputQuery.Skip(espec.Skip).Take(espec.Take);
        }

        inputQuery = espec.Inclusion.Aggregate(inputQuery, (current, include) => current.Include(include));

        return inputQuery;
    }

    public static IQueryable<T> GetQueryCount(IQueryable<T> inputQuery, IEspecificacion<T> espec)
    {
        if (espec.Criterio != null)
        {
            inputQuery = inputQuery.Where(espec.Criterio);
        }

        inputQuery = espec.Inclusion.Aggregate(inputQuery, (current, include) => current.Include(include));

        return inputQuery;
    }
}
