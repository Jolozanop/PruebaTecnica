using Aplicacion.Core.Interfaces;
using System.Collections;

namespace Infraestructura.Data.Repositorios;

public class UnitOfWork : IUnitOfWork
{
    private Hashtable _repositorios;

    private readonly ContextoBD _context;

    public UnitOfWork(ContextoBD context)
    {
        _context = context;
    }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public IRepositorioGenerico<TEntity> Repositorio<TEntity>() where TEntity : class
    {
        if (_repositorios == null)
        {
            _repositorios = new Hashtable();
        }
        var type = typeof(TEntity).Name;
        if (!_repositorios.ContainsKey(type))
        {
            var repoType = typeof(RepositorioGenerico<>);
            var repoInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(TEntity)), _context);
            _repositorios.Add(type, repoInstance);
        }
        return (IRepositorioGenerico<TEntity>)_repositorios[type];
    }
}
