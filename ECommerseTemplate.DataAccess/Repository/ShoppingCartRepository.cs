using ECommerseTemplate.DataAccess.Data;
using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerseTemplate.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ShoppingCart shoppingCart)
        {
            _db.Update(shoppingCart);
        }
    }
}
