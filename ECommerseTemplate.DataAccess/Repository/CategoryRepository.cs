using ECommerseTemplate.DataAccess.Data;
using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerseTemplate.DataAccess.Repository
{
	public class CategoryRepository : Repository<CategoryModel>, ICategoryRepository
	{
		private readonly ApplicationDbContext _db;
		public CategoryRepository(ApplicationDbContext db) : base(db) 
		{
			_db = db;
		}

		public void Update(CategoryModel category)
		{
			_db.Update(category);
		}
	}
}
