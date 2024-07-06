namespace ECommerseTemplate.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        ICompanyRepository Company { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailRepository OrderDetails { get; }
        IProductTagRepository ProductTag { get; }
        IProductProductTagRepository ProductProductTag { get; }
        void Save();
    }
}
