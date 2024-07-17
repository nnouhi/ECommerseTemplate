using ECommerseTemplate.Models;
using System.Linq.Expressions;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool tracked = false);
    // Make a paginated list from db (not finished, may need extra work to add more functionality)
    Task<PaginatedList<T>> GetPaginated(IQueryable<T> set, int pageNumber, int pageSize);
    T Get(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool tracked = false);
    void Add(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entity);
}
