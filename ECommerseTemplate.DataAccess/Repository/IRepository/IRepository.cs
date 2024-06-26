using ECommerseTemplate.Models;
using System.Linq.Expressions;

public interface IRepository<T> where T : class
{
	IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool tracked = false);
	Task<PaginatedList<T>> GetPaginated<TKey>(Expression<Func<T, TKey>> keySelector, int pageNumber, int pageSize, string? includeProperties = null, bool descending = false);
	T Get(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool tracked = false);
	void Add(T entity);
	void Remove(T entity);
	void RemoveRange(IEnumerable<T> entity);
}
