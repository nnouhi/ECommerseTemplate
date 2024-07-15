using ECommerseTemplate.DataAccess.Data;
using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;

namespace ECommerseTemplate.DataAccess.Repository
{
    public class ProductReviewRepository : Repository<ProductReview>, IProductReviewRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductReviewRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ProductReview productReview)
        {
            _db.Update(productReview);
        }
    }
}
