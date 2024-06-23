using ECommerseTemplate.DataAccess.Data;
using ECommerseTemplate.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public class Repository<T> : IRepository<T> where T : class
{
	private readonly ApplicationDbContext _db;
	internal DbSet<T> dbSet;

	public Repository(ApplicationDbContext db)
	{
		_db = db;
		this.dbSet = _db.Set<T>();
	}

	public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool tracked = false)
	{
		IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();

		if (filter != null)
		{
			query = query.Where(filter);
		}

		if (includeProperties != null)
		{
			foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(includeProperty);
			}
		}

		return query.ToList();
	}

	public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
	{
		IQueryable<T> query = tracked ? dbSet : dbSet.AsNoTracking();

		query = query.Where(filter);

		if (includeProperties != null)
		{
			foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(includeProperty);
			}
		}

		return query.FirstOrDefault();
	}

	public async Task<PaginatedList<T>> GetPaginated<TKey>(Expression<Func<T, TKey>> keySelector, int pageNumber, int pageSize, string? includeProperties = null, bool descending = false)
	{
		IQueryable<T> query = dbSet;
		List<T> items;
		if (!string.IsNullOrWhiteSpace(includeProperties))
		{
			foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(includeProperty);
			}
		}

		int count = await query.CountAsync();

		if (!descending)
		{
			items = await query
				.OrderBy(keySelector)
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
		}
		else
		{
			items = await query
				   .OrderByDescending(keySelector)
				   .Skip((pageNumber - 1) * pageSize)
				   .Take(pageSize)
				   .ToListAsync();
		}

		return new PaginatedList<T>(items, pageNumber, pageSize, count);
	}

	public void Add(T entity)
	{
		dbSet.Add(entity);
	}

	public void Remove(T entity)
	{
		dbSet.Remove(entity);
	}

	public void RemoveRange(IEnumerable<T> entity)
	{
		dbSet.RemoveRange(entity);
	}
}
