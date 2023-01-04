namespace Aplicacion.Core.Interfaces;

public interface IRepositorioGenerico<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IReadOnlyList<T>> GetAllAsync();

    Task<T> GetById_ConEspecificacion(IEspecificacion<T> espec);
    Task<IReadOnlyList<T>> GetAll_ConEspecificacion(IEspecificacion<T> espec);

    Task<int> ConteoAsync(IEspecificacion<T> espec);
    Task<int> Agregar(T entity);
    Task<int> Actualizar(T entity);
    Task<int> Eliminar(T entity);

    void AddEntity(T Entity);
    void UpdateEntity(T Entity);
    void DeleteEntity(T Entity);
}