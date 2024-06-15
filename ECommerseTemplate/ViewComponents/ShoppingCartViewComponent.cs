using ECommerseTemplate.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerseTemplate.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim is not null)
            {
                if (HttpContext.Session.GetInt32(SD.SessionKeys.NumOfShoppingCarts) is null)
                {
                    int numOfShoppingCarts = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).Count();
                    HttpContext.Session.SetInt32(SD.SessionKeys.NumOfShoppingCarts, numOfShoppingCarts);
                }

                return View(HttpContext.Session.GetInt32(SD.SessionKeys.NumOfShoppingCarts));
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
