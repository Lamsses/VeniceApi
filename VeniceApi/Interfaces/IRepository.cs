using System.Linq.Expressions;

namespace VeniceApi.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAll();
    IQueryable<T> GetAllQuery();
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
    Task<T>  GetById(int id);
    Task<T> Add(T entity);
    Task<IEnumerable<T>> AddRange(IEnumerable<T> entity);
    Task Update(T entity);
    Task Delete(int id);
}
