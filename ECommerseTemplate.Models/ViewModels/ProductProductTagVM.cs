using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerseTemplate.Models.ViewModels
{
	public class ProductProductTagVM
	{
		public ProductProductTag ProductProductTag { get; set; }
		public IEnumerable<SelectListItem> ProductList { get; set; }
		public IEnumerable<SelectListItem> ProductTagList { get; set; }
	}
}
