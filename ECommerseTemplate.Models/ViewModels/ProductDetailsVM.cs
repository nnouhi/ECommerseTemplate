namespace ECommerseTemplate.Models.ViewModels
{
    public class ProductDetailsVM
    {
        public ShoppingCart ShoppingCart { get; set; }
        public List<ProductReview> Reviews { get; set; }
        public ProductReview Review { get; set; }
    }
}
