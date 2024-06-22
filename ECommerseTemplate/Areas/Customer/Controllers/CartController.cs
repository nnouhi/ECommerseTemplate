using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using ECommerseTemplate.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace ECommerseTemplate.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;

        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
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
                shoppingCartViewModel.OrderHeader.OrderStatus = SD.OrderStatuses.Pending;
                shoppingCartViewModel.OrderHeader.PaymentStatus = SD.PaymentStatuses.Pending;
            }
            else
            {
                shoppingCartViewModel.OrderHeader.OrderStatus = SD.OrderStatuses.Approved;
                shoppingCartViewModel.OrderHeader.PaymentStatus = SD.PaymentStatuses.DelayedPayment;
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


            // Create stripe session
            if (user.CompanyId.GetValueOrDefault() == 0)
            {
                const string domain = SD.ApplicationSettings.ApplicationMode == "Development" ? SD.URLs.DevelopmentDomain : SD.URLs.ProductionDomain;
                SessionCreateOptions options = new SessionCreateOptions
                {
                    SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={shoppingCartViewModel.OrderHeader.Id}",
                    CancelUrl = domain + $"customer/cart/index",
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };

                // Populate the SessionLineItems
                foreach (ShoppingCart cart in shoppingCartViewModel.ShoppingCartList)
                {
                    SessionLineItemOptions sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(cart.Price * 100), // 20.50 to 2050
                            Currency = "eur",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = cart.Product.Title
                            }
                        },
                        Quantity = cart.Count
                    };

                    // Add the line item to the options' list
                    options.LineItems.Add(sessionLineItem);
                }

                SessionService service = new SessionService();
                Session session = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStripePaymentId(shoppingCartViewModel.OrderHeader.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();

                // Redirect to the Stripe payment panel
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }

            return RedirectToAction(nameof(OrderConfirmation), new { id = shoppingCartViewModel.OrderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(oh => oh.Id == id, includeProperties: "ApplicationUser");
            if (orderHeader.PaymentStatus != SD.PaymentStatuses.DelayedPayment)
            {
                SessionService service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                if (session.PaymentStatus == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentId(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(id, SD.OrderStatuses.Approved, SD.PaymentStatuses.Approved);

                    // Payment was approved. Remove user's shopping carts
                    IEnumerable<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == orderHeader.ApplicationUserId);
                    _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
                    _unitOfWork.Save();
                    HttpContext.Session.Clear();

                    // Send order confirmation email
                    _emailSender.SendEmailAsync(
                        orderHeader.ApplicationUser.Email,
                        "ECommerseTemplate - We Received Your Order!",
                        $"<p>New Order Created - {orderHeader.Id}</p>"
                        ).GetAwaiter().GetResult();
                }
            }
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
            bool removeItemFromCart = cartFromDb.Count <= 1;
            if (removeItemFromCart)
            {
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            _unitOfWork.Save();

            // Item's quantity is < 0 after the minus operation. Remove it from the users cart
            if (removeItemFromCart)
            {
                UpdateShoppingCartCount();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();

            // Item is completely removed from the user's cart
            UpdateShoppingCartCount();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        private void UpdateShoppingCartCount()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            int numOfShoppingCarts = _unitOfWork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == userId).Count();
            HttpContext.Session.SetInt32(SD.SessionKeys.NumOfShoppingCarts, numOfShoppingCarts);
        }

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
