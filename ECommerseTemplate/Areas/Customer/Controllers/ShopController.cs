using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using X.PagedList;


namespace ECommerseTemplate.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class ShopController : Controller
	{
		private readonly ILogger<ShopController> _logger;
		private readonly IUnitOfWork _unitOfWork;

		public ShopController(ILogger<ShopController> logger, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
			Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			// If the user is logged in populate the shopping cart count
			if (claim is not null)
			{
				string userId = claim.Value;
				IEnumerable<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == userId, includeProperties: "Product");
				HttpContext.Session.SetInt32(SD.SessionKeys.NumOfShoppingCarts, shoppingCarts.Count());
			}

			return RedirectToAction(nameof(Products));
		}

		public async Task<IActionResult> Products(int pageNumber = 1, int pageSize = 5)
		{
			var paginatedList = await _unitOfWork.Product.GetPaginated(p => p.Id, pageNumber, pageSize, includeProperties: "Category");
			IPagedList<Product> products = new StaticPagedList<Product>(paginatedList.Items, pageNumber, pageSize, paginatedList.TotalItemCount);
			return View(products);
		}

		public IActionResult Details(int id)
		{
			Product product = _unitOfWork.Product.Get(u => u.Id == id, includeProperties: "Category");
			if (product is null)
			{
				return NotFound();
			}
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
	}
}