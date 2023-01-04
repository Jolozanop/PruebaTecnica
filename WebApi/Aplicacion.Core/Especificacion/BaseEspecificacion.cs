using Aplicacion.Core.Interfaces;
using System.Linq.Expressions;

namespace Aplicacion.Core.Especificacion;

public class BaseEspecificacion<T> : IEspecificacion<T>
{
    public BaseEspecificacion() { }
    public BaseEspecificacion(Expression<Func<T, bool>> _criterio)
    {
        Criterio = _criterio;
    }
    public Expression<Func<T, bool>> Criterio { get; }

    public List<Expression<Func<T, object>>> Inclusion { get; } = new List<Expression<Func<T, object>>>();



    protected void AgregarInclusion(Expression<Func<T, object>> inclusionExpression)
    {
        Inclusion.Add(inclusionExpression);
    }
    public Expression<Func<T, object>> OrdenarAsc { get; private set; }

    public Expression<Func<T, object>> OrdenarDesc { get; private set; }

    protected void AgregarOrdenAsc(Expression<Func<T, object>> orderByAscExpression)
    {
        OrdenarAsc = orderByAscExpression;
    }
    protected void AgregarOrdenDesc(Expression<Func<T, object>> orderByDescExpression)
    {
        OrdenarDesc = orderByDescExpression;
    }

    public int Take { get; private set; }

    public int Skip { get; private set; }

    public bool HabilitadoPaginar { get; private set; }

    protected void AplicarPaginacion(int skip, int take)
    {
        Skip = skip;
        Take = take;
        HabilitadoPaginar = true;
    }
}