using ECommerseTemplate.Models;

namespace ECommerseTemplate.DataAccess.Repository.IRepository
{
    public interface IProductReviewImageRepository : IRepository<ProductReviewImage>
    {
        void Update(ProductReviewImage productReviewImage);
    }
}
