using ECommerseTemplate.DataAccess.Data;
using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;

namespace ECommerseTemplate.DataAccess.Repository
{
	public class ImageRepository : Repository<Image>, IImageRepository
	{
		private readonly ApplicationDbContext _db;
		public ImageRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Image image)
		{
			_db.Update(image);
		}
	}
}
