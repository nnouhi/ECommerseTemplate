using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using ECommerseTemplate.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerseTemplate.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        public IActionResult Index()
        {
            ClaimsIdentity userClaimsIdentity = (ClaimsIdentity)User.Identity;
            string userIdValue = userClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            IEnumerable<ShoppingCart> userShoppingCarts = _unitOfWork.ShoppingCart.GetAll(
                sc => sc.ApplicationUserId == userIdValue,
                includeProperties: "Product"
            );

            ShoppingCartVM shoppingCartViewModel = new()
            {
                ShoppingCartList = userShoppingCarts
            };

            // Calculate total price
            foreach (ShoppingCart cart in shoppingCartViewModel.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                shoppingCartViewModel.OrderTotal += (cart.Price * cart.Count);
            }
            return View(shoppingCartViewModel);
        }

        public IActionResult Summary()
        {
            return View();
        }

        #region cart operations
        public IActionResult Plus(int cartId)
        {
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartFromDb.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            { 
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        private float GetPriceBasedOnQuantity(ShoppingCart cart)
        {
            // Wanted to try this (replacement for if,else if,else)
            return cart.Count switch
            {
                <= 50 => cart.Product.Price,
                <= 100 => cart.Product.Price50,
                _ => cart.Product.Price100,
            };
        }
    }
}
