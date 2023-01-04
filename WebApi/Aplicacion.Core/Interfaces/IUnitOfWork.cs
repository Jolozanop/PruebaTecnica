namespace Aplicacion.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IRepositorioGenerico<TEntity> Repositorio<TEntity>() where TEntity : class;
    Task<int> Complete();
}
