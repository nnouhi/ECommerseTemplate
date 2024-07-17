using X.PagedList;

namespace ECommerseTemplate.Models.ViewModels
{
    public class ShopVM
    {
        public string OrderBy { get; set; }
        public string SearchByName { get; set; }
        public string ProductTag { get; set; }
        public IPagedList<Product> ProductsPagedList { get; set; }
        public string OrderByDisplayText => GetDropdownValue(OrderBy);
        // MinSliderPrice & MaxSliderPrice are used to set the range of the slider (based on db min - max product prices)
        public int MinSliderPrice { get; set; }
        public int MaxSliderPrice { get; set; }
        // PostMinPrice & PostMaxPrice are to filter the products based on the slider values the user selected
        public int PostMinPrice { get; set; }
        public int PostMaxPrice { get; set; }
        public List<Product> RecentlyViewedProducts { get; set; }
        public List<ProductTag> ProductTags { get; set; }
        public bool HasActiveFilters => !string.IsNullOrEmpty(SearchByName) || PostMinPrice != MinSliderPrice || PostMaxPrice != MaxSliderPrice || !string.IsNullOrEmpty(ProductTag);

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
