using ECommerseTemplate.Models;

namespace ECommerseTemplate.DataAccess.Repository.IRepository
{
    public interface IProductProductTagRepository : IRepository<ProductProductTag>
    {
        void Update(ProductProductTag productProductTag);
    }
}
