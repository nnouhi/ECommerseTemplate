using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using ECommerseTemplate.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
			Claim claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

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
			List<Product> recentlyViewedProducts = GetRecentlyViewedProducts();
			List<float> prices = _unitOfWork.Product.GetAll().Select(p => p.Price).ToList();
			int minSliderPrice = (int)prices.Min();
			int maxSliderPrice = (int)prices.Max();
			// If no values provided by the user, default to the slider values which is the min and max ranges
			int postMinPrice = minPrice != 0 ? minPrice : minSliderPrice;
			int postMaxPrice = maxPrice != int.MaxValue ? maxPrice : maxSliderPrice;

			ShopVM shopVM = new ShopVM()
			{
				OrderBy = orderby,
				ProductsPagedList = productsPagedList,
				MinSliderPrice = minSliderPrice,
				MaxSliderPrice = maxSliderPrice,
				PostMinPrice = postMinPrice,
				PostMaxPrice = postMaxPrice,
				RecentlyViewedProducts = recentlyViewedProducts
			};

			return View(shopVM);
		}

		public IActionResult Details(int id)
		{
			Product product = _unitOfWork.Product.Get(u => u.Id == id, includeProperties: "Category");
			if (product == null)
			{
				return NotFound();
			}
			ShoppingCart shoppingCart = new ShoppingCart() { Product = product, Count = 1, ProductId = id };

			// Add the product to the recently viewed products
			AddRecentViewedProductId(id.ToString());
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
			TempData["Success"] = "Added to shopping cart successfully";
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

		private void AddRecentViewedProductId(string productId)
		{
			Queue<string> recentlyViewedProductIds = GetRecentlyViewedProductIds();
			HashSet<string> seenProductIds = new HashSet<string>(recentlyViewedProductIds);

			// Remove productId from queue and set if it exists to avoid duplicates
			if (seenProductIds.Contains(productId))
			{
				// Create a queue and don't include the existing productId
				recentlyViewedProductIds = new Queue<string>(recentlyViewedProductIds.Where(id => id != productId));
			}

			// Add the new productId to the queue and set
			recentlyViewedProductIds.Enqueue(productId);
			seenProductIds.Add(productId);

			// Limit the queue size to 3 (or any desired size)
			while (recentlyViewedProductIds.Count > 3)
			{
				string removedId = recentlyViewedProductIds.Dequeue();
				seenProductIds.Remove(removedId);
			}

			// Serialize queue to store in cookie
			string recentlyViewedProductIdsString = JsonConvert.SerializeObject(recentlyViewedProductIds);

			// Store in cookie
			CookieOptions option = new CookieOptions();
			option.Expires = DateTime.Now.AddMinutes(100); // Adjust as needed
			Response.Cookies.Append(SD.CookieKeys.RecentlyViewedProductIds, recentlyViewedProductIdsString, option);

			_logger.LogInformation("Viewed Product Ids after adding new item: {RecentlyViewedProductIds}", string.Join(", ", recentlyViewedProductIds));
		}

		private Queue<string> GetRecentlyViewedProductIds()
		{
			string recentlyViewedProductIdsString = Request.Cookies[SD.CookieKeys.RecentlyViewedProductIds];
			Queue<string> recentlyViewedProductIds = new Queue<string>();

			if (!string.IsNullOrEmpty(recentlyViewedProductIdsString))
			{
				recentlyViewedProductIds = JsonConvert.DeserializeObject<Queue<string>>(recentlyViewedProductIdsString);
			}

			_logger.LogInformation("Viewed Product Ids before adding new item: {RecentlyViewedProductIds}", string.Join(", ", recentlyViewedProductIds));
			return recentlyViewedProductIds;
		}

		private List<Product> GetRecentlyViewedProducts()
		{
			Queue<string> recentlyViewedProductIds = GetRecentlyViewedProductIds();
			List<Product> recentlyViewedProducts = new List<Product>();

			foreach (string productId in recentlyViewedProductIds.Reverse()) // Reverse to show the latest viewed product first
			{
				Product product = _unitOfWork.Product.Get(p => p.Id == int.Parse(productId), includeProperties: "Category");
				if (product != null)
				{
					recentlyViewedProducts.Add(product);
				}
			}

			return recentlyViewedProducts;
		}
	}
}
