using Aplicacion.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Data.Repositorios;

public class RepositorioGenerico<T> : IDisposable, IRepositorioGenerico<T> where T : class
{
    private readonly ContextoBD _dbContext;

    public RepositorioGenerico(ContextoBD dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public void Dispose() => _dbContext.Dispose();



    public async Task<T> GetById_ConEspecificacion(IEspecificacion<T> espec)
    {
        return await AplicarEspecificacion(espec).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> GetAll_ConEspecificacion(IEspecificacion<T> espec)
    {
        return await AplicarEspecificacion(espec).ToListAsync();
    }

    //Resolver condiciones logicas (filtros) y relaciones entre entidades de la BD
    private IQueryable<T> AplicarEspecificacion(IEspecificacion<T> espec)
    {
        return Especificacion<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), espec);
    }

    public async Task<int> ConteoAsync(IEspecificacion<T> espec)
    {
        return await Especificacion<T>.GetQueryCount(_dbContext.Set<T>().AsQueryable(), espec).CountAsync();
        //return await AplicarEspecificacion(espec).CountAsync();
    }

    public async Task<int> Agregar(T entity)
    {
        _dbContext.Set<T>().Add(entity);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> Actualizar(T entity)
    {
        _dbContext.Set<T>().Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> Eliminar(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        _dbContext.Entry(entity).State = EntityState.Deleted;
        return await _dbContext.SaveChangesAsync();
    }



    public void AddEntity(T Entity)
    {
        _dbContext.Set<T>().Add(Entity);
        //_dbContext.SaveChangesAsync();
    }

    public void UpdateEntity(T Entity)
    {
        _dbContext.Set<T>().Attach(Entity);
        _dbContext.Entry(Entity).State = EntityState.Modified;
        //_dbContext.SaveChangesAsync();
    }

    public void DeleteEntity(T Entity)
    {
        _dbContext.Set<T>().Remove(Entity);
        //_dbContext.SaveChangesAsync();
    }
}
