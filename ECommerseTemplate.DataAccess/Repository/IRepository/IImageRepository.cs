using ECommerseTemplate.Models;

namespace ECommerseTemplate.DataAccess.Repository.IRepository
{
	public interface IImageRepository : IRepository<Image>
	{
		void Update(Image image);
	}
}
