using ECommerseTemplate.Models;

namespace ECommerseTemplate.DataAccess.Repository.IRepository
{
	public interface IProductTagRepository : IRepository<ProductTag>
	{
		void Update(ProductTag productTag);
	}
}
