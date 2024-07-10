using ECommerseTemplate.Models;

namespace ECommerseTemplate.DataAccess.Repository.IRepository
{
	public interface IProductImageRepository : IRepository<ProductImage>
	{
		void Update(ProductImage image);
	}
}
