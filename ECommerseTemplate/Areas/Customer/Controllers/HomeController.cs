using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ECommerseTemplate.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(products);
        }

        public IActionResult Details(int id)
        {
            Product product = _unitOfWork.Product.Get(u => u.Id == id,includeProperties: "Category");
            ShoppingCart shoppingCart = new ShoppingCart() { Product = product, Count = 1, ProductId = id };
            return View(shoppingCart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
            string userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart shoppingCartFromDb = _unitOfWork.ShoppingCart.Get(sc => sc.ApplicationUserId == shoppingCart.ApplicationUserId && sc.ProductId == shoppingCart.ProductId);

            if (shoppingCartFromDb != null)
            {
                // Shopping cart already exists, update it
                shoppingCartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(shoppingCartFromDb);
            }
            else
            {
                // Shopping cart doesn't exist, create it
                shoppingCart.Id = 0;
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }
           
            _unitOfWork.Save();
            TempData["Success"] = "Added to shopping cart succussfuly";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}