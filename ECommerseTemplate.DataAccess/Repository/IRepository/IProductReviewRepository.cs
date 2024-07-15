using ECommerseTemplate.Models;

namespace ECommerseTemplate.DataAccess.Repository.IRepository
{
    public interface IProductReviewRepository : IRepository<ProductReview>
    {
        void Update(ProductReview productReview);
    }
}
