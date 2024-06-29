using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using ECommerseTemplate.Models.ViewModels;
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

		public async Task<IActionResult> Index(int page = 1, string orderby = "", int minPrice = 0, int maxPrice = int.MaxValue) // Change default values in the future
		{
			ClaimsIdentity claimsIdentity = (ClaimsIdentity)User.Identity;
			Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			// If the user is logged in populate the shopping cart count
			if (claim != null)
			{
				string userId = claim.Value;
				IEnumerable<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == userId, includeProperties: "Product");
				HttpContext.Session.SetInt32(SD.SessionKeys.NumOfShoppingCarts, shoppingCarts.Count());
			}

			int pageSize = 5; // This will be saved in the database in the future
			var paginatedList = await GetPaginatedList(page, pageSize, orderby, minPrice, maxPrice);
			IPagedList<Product> productsPagedList = new StaticPagedList<Product>(paginatedList.Items, page, pageSize, paginatedList.TotalItemCount);
			List<float> prices = _unitOfWork.Product.GetAll().Select(p => p.Price).ToList();
			int minSliderMinPrice = (int)prices.Min();
			int maxSliderMaxPrice = (int)prices.Max();
			// If no values provided by the user, default to the slider values which is the min and max ranges
			int postMinPrice = minPrice == 0 || maxPrice == int.MaxValue ? minSliderMinPrice : minPrice;
			int postMaxPrice = minPrice == 0 || maxPrice == int.MaxValue ? maxSliderMaxPrice : maxPrice;

			ShopVM shopVM = new ShopVM()
			{
				OrderBy = orderby,
				ProductsPagedList = productsPagedList,
				MinSliderPrice = minSliderMinPrice,
				MaxSliderPrice = maxSliderMaxPrice,
				PostMinPrice = postMinPrice,
				PostMaxPrice = postMaxPrice
			};

			return View(shopVM);
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

		private Task<PaginatedList<Product>> GetPaginatedList(int page, int pageSize, string orderBy, int minPrice, int maxPrice)
		{
			// No filtering by price, no need to generate a custom set based on price filter
			if (minPrice == 0 && maxPrice == int.MaxValue)
			{
				// For now we only have support for ordering by price and id (date added I guess)
				if (orderBy == "price" || orderBy == "price-desc")
				{
					bool isDescending = orderBy == "price-desc";
					return _unitOfWork.Product.GetPaginated(p => p.Price, page, pageSize, includeProperties: "Category", descending: isDescending);
				}
				else if (orderBy == "date")
				{
					return _unitOfWork.Product.GetPaginated(p => p.DateAdded, page, pageSize, includeProperties: "Category", descending: true);
				}
				else
				{
					// Default is date for now
					return _unitOfWork.Product.GetPaginated(p => p.DateAdded, page, pageSize, includeProperties: "Category", descending: true);
				}
			}
			else
			{
				IQueryable<Product> productSet = _unitOfWork.Product.GetAll(includeProperties: "Category").Where(p => p.Price >= minPrice && p.Price <= maxPrice);
				// For now we only have support for ordering by price and id (date added I guess)
				if (orderBy == "price" || orderBy == "price-desc")
				{
					bool isDescending = orderBy == "price-desc";
					return _unitOfWork.Product.GetPaginated(productSet, p => p.Price, page, pageSize, includeProperties: "Category", descending: isDescending);
				}
				else if (orderBy == "date")
				{
					return _unitOfWork.Product.GetPaginated(productSet, p => p.DateAdded, page, pageSize, includeProperties: "Category", descending: true);
				}
				else
				{
					// Default is date for now
					return _unitOfWork.Product.GetPaginated(productSet, p => p.DateAdded, page, pageSize, includeProperties: "Category", descending: true);
				}
			}


		}
	}
}