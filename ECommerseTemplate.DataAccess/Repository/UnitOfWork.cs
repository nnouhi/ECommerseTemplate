using ECommerseTemplate.DataAccess.Data;
using ECommerseTemplate.DataAccess.Repository.IRepository;

namespace ECommerseTemplate.DataAccess.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		public ICategoryRepository Category { get; private set; }
		public IProductRepository Product { get; private set; }
		public ICompanyRepository Company { get; private set; }
		public IShoppingCartRepository ShoppingCart { get; private set; }
		public IApplicationUserRepository ApplicationUser { get; private set; }
		public IOrderHeaderRepository OrderHeader { get; private set; }
		public IOrderDetailRepository OrderDetails { get; private set; }
		public IProductTagRepository ProductTag { get; private set; }
		public IProductProductTagRepository ProductProductTag { get; private set; }
		public IImageRepository Image { get; private set; }

		private readonly ApplicationDbContext _db;

		public UnitOfWork(ApplicationDbContext db)
		{
			_db = db;
			Category = new CategoryRepository(_db);
			Product = new ProductRepository(_db);
			Company = new CompanyRepository(_db);
			ShoppingCart = new ShoppingCartRepository(_db);
			ApplicationUser = new ApplicationUserRepository(_db);
			OrderHeader = new OrderHeaderRepository(_db);
			OrderDetails = new OrderDetailRepository(_db);
			ProductTag = new ProductTagRepository(_db);
			ProductProductTag = new ProductProductTagRepository(_db);
			Image = new ImageRepository(_db);
		}

		public void Save()
		{
			_db.SaveChanges();
		}
	}
}
