using X.PagedList;

namespace ECommerseTemplate.Models.ViewModels
{
    public class ShopVM
    {
        public string OrderBy { get; set; }
        public IPagedList<Product> productsPagedList { get; set; }
        public string OrderByDisplayText => GetDropdownValue(OrderBy);

        private string GetDropdownValue(string orderBy)
        {
            switch (orderBy)
            {
                case "price":
                    return "Sort by price: low to high";
                case "price-desc":
                    return "Sort by price: high to low";
                case "date":
                    return "Sort by latest";
                default:
                    return "Sort by latest";
            }
        }
    }
}
