using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using ECommerseTemplate.Models.ViewModels;
using ECommerseTemplate.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
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
                ShoppingCartList = userShoppingCarts,
                OrderHeader = new()
            };

            // Calculate total price
            foreach (ShoppingCart cart in shoppingCartViewModel.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                shoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(shoppingCartViewModel);
        }

        public IActionResult Summary()
        {
            ClaimsIdentity userClaimsIdentity = (ClaimsIdentity)User.Identity;
            string userIdValue = userClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            IEnumerable<ShoppingCart> userShoppingCarts = _unitOfWork.ShoppingCart.GetAll(
                sc => sc.ApplicationUserId == userIdValue,
                includeProperties: "Product"
            );

            ShoppingCartVM shoppingCartViewModel = new()
            {
                ShoppingCartList = userShoppingCarts,
                OrderHeader = new()
            };

            // Populate the Order Header based on Application User info
            ApplicationUser user = _unitOfWork.ApplicationUser.Get(u => u.Id == userIdValue); 
            shoppingCartViewModel.OrderHeader.ApplicationUser = user;

            shoppingCartViewModel.OrderHeader.Name = user.Name;
            shoppingCartViewModel.OrderHeader.PhoneNumber = user.PhoneNumber;
            shoppingCartViewModel.OrderHeader.StreetAddress = user.StreetAddress;
            shoppingCartViewModel.OrderHeader.City = user.City;
            shoppingCartViewModel.OrderHeader.Country = user.Country;
            shoppingCartViewModel.OrderHeader.PostalCode = user.PostalCode;

			// Calculate total price
			foreach (ShoppingCart cart in shoppingCartViewModel.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				shoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}
			return View(shoppingCartViewModel);
        }

        [HttpPost]
		public IActionResult Summary(ShoppingCartVM shoppingCartViewModel)
		{
			ClaimsIdentity userClaimsIdentity = (ClaimsIdentity)User.Identity;
			string userIdValue = userClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            // Populate Order Header and add to db
			shoppingCartViewModel.OrderHeader.ApplicationUserId = userIdValue;
			shoppingCartViewModel.OrderHeader.OrderDate = DateTime.Now;
			shoppingCartViewModel.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(
                sc => sc.ApplicationUserId == userIdValue, includeProperties: "Product"
            );

            foreach (ShoppingCart cart in shoppingCartViewModel.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
				shoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}

			// Handle if user is company user or just a regular user
			ApplicationUser user = _unitOfWork.ApplicationUser.Get(u => u.Id == userIdValue);
			if (user.CompanyId.GetValueOrDefault() == 0)
            {
				shoppingCartViewModel.OrderHeader.OrderStatus = SD.Order_Status_Pending;
				shoppingCartViewModel.OrderHeader.PaymentStatus = SD.Payment_Status_Pending;
			}
			else 
            {
				shoppingCartViewModel.OrderHeader.OrderStatus = SD.Order_Status_Approved;
				shoppingCartViewModel.OrderHeader.PaymentStatus = SD.Payment_Status_DelayedPayment;
			}

            _unitOfWork.OrderHeader.Add(shoppingCartViewModel.OrderHeader);
            _unitOfWork.Save();

			// Populate Order Details and add to db
			foreach (ShoppingCart cart in shoppingCartViewModel.ShoppingCartList)
			{
                OrderDetail orderDetail = new()
                {
                    OrderHeaderId = shoppingCartViewModel.OrderHeader.Id,
                    ProductId = cart.ProductId,
                    Price = GetPriceBasedOnQuantity(cart),
                    Count = cart.Count
                };
				
                _unitOfWork.OrderDetails.Add(orderDetail);
                _unitOfWork.Save();
			}


			if (user.CompanyId.GetValueOrDefault() == 0)
			{
				//it is a regular customer account and we need to capture payment
				//stripe logic
			}

			return RedirectToAction(nameof(OrderConfirmation), new { id = shoppingCartViewModel.OrderHeader.Id });
		}
		public IActionResult OrderConfirmation(int id)
		{
			return View(id);
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
