namespace Domain.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T?> GetById(int id);
    Task<T?> GetById(Guid id);
    Task<IEnumerable<T>> GetAll();
    Task Insert(T entity);
    Task Update(T entity);
}
