using ECommerseTemplate.DataAccess.Data;
using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;

namespace ECommerseTemplate.DataAccess.Repository
{
    public class ProductReviewImageRepository : Repository<ProductReviewImage>, IProductReviewImageRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductReviewImageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ProductReviewImage productReviewImage)
        {
            _db.Update(productReviewImage);
        }
    }
}
