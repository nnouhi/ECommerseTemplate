using ECommerseTemplate.DataAccess.Data;
using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;

namespace ECommerseTemplate.DataAccess.Repository
{
	public class ProductTagRepository : Repository<ProductTag>, IProductTagRepository
	{
		private readonly ApplicationDbContext _db;
		public ProductTagRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(ProductTag productTag)
		{
			_db.Update(productTag);
		}
	}
}
