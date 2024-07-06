using ECommerseTemplate.DataAccess.Data;
using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;

namespace ECommerseTemplate.DataAccess.Repository
{
    public class ProductProductTagRepository : Repository<ProductProductTag>, IProductProductTagRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductProductTagRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ProductProductTag productProductTag)
        {
            _db.Update(productProductTag);
        }
    }
}
