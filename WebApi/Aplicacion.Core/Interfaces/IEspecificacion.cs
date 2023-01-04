using System.Linq.Expressions;

namespace Aplicacion.Core.Interfaces;

public interface IEspecificacion<T>
{
    //Para aplicar filtros hacia las entidades (Filtros de busqueda hacia la BD)
    Expression<Func<T, bool>> Criterio { get; }

    //Para resolver las relaciones de las entidades (Relaciones Foreaneas de la BD)
    List<Expression<Func<T, object>>> Inclusion { get; }

    Expression<Func<T, object>> OrdenarAsc { get; }
    Expression<Func<T, object>> OrdenarDesc { get; }

    int Take { get; }
    int Skip { get; }
    bool HabilitadoPaginar { get; }
}
