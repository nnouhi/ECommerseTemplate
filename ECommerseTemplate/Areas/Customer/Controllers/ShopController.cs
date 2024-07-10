using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using ECommerseTemplate.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq.Expressions;
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

        public async Task<IActionResult> Index(int page = 1, string orderBy = "", string searchByName = "", string productTag = "", int minPrice = 0, int maxPrice = int.MaxValue) // Change default values in the future
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

            var paginatedList = await GetPaginatedList(page, pageSize, orderBy, productTag, minPrice, maxPrice, searchByName, out int minSliderPrice, out int maxSliderPrice);
            IPagedList<Product> productsPagedList = new StaticPagedList<Product>(paginatedList.Items, page, pageSize, paginatedList.TotalItemCount);
            List<Product> recentlyViewedProducts = GetRecentlyViewedProducts();
            List<float> prices = _unitOfWork.Product.GetAll().Select(p => p.Price).ToList();
            List<ProductTag> productTags = _unitOfWork.ProductTag.GetAll().ToList();
            // If no values provided by the user, default to the slider values which is the min and max ranges
            int postMinPrice = minPrice != 0 ? minPrice : minSliderPrice;
            int postMaxPrice = maxPrice != int.MaxValue ? maxPrice : maxSliderPrice;

            ShopVM shopVM = new ShopVM()
            {
                OrderBy = orderBy,
                ProductTag = productTag,
                ProductsPagedList = productsPagedList,
                MinSliderPrice = minSliderPrice,
                MaxSliderPrice = maxSliderPrice,
                PostMinPrice = postMinPrice,
                PostMaxPrice = postMaxPrice,
                RecentlyViewedProducts = recentlyViewedProducts,
                SearchByName = searchByName,
                ProductTags = productTags
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
            List<string> productImages = _unitOfWork.ProductImage.GetAll(pi => pi.ProductId == id).Select(pi => pi.Path).ToList();
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = product,
                Count = 1,
                ProductId = id,
                ProductImages = productImages
            };

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

        private Task<PaginatedList<Product>> GetPaginatedList(int page, int pageSize, string orderBy, string productTag, int minPrice, int maxPrice, string searchbyname, out int minSliderPrice, out int maxSliderPrice)
        {
            // Start with getting all products including category
            IQueryable<Product> productSet = _unitOfWork.Product.GetAll(includeProperties: "Category");

            // Apply filter by product tag if provided
            if (!string.IsNullOrEmpty(productTag))
            {
                int productTagId = _unitOfWork.ProductTag.Get(pt => pt.Name == productTag).Id;
                HashSet<int> productIdsWithTag = _unitOfWork.ProductProductTag
                                                    .GetAll(ppt => ppt.ProductTagId == productTagId)
                                                    .Select(ppt => ppt.ProductId)
                                                    .ToHashSet();
                productSet = productSet.Where(p => productIdsWithTag.Contains(p.Id));
            }

            // Apply filter by name if searchbyname is provided
            if (!string.IsNullOrEmpty(searchbyname))
            {
                // Case insensitive search by product name
                productSet = productSet.Where(p => p.Title.ToLower().Contains(searchbyname.ToLower()));
            }

            // Based on the new sets calculated above, get the min and max prices for the slider
            List<float> prices = productSet.Select(p => p.Price).ToList();
            minSliderPrice = (int)prices.Min();
            maxSliderPrice = (int)prices.Max();

            // Apply filtering by price range
            if (minPrice > 0 || maxPrice < int.MaxValue)
            {
                productSet = productSet.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            }

            // Determine the sorting criteria
            Expression<Func<Product, object>> orderByExpression = null;
            bool descending = false;

            if (orderBy == "price")
            {
                orderByExpression = p => p.Price;
                descending = false;
            }
            else if (orderBy == "price-desc")
            {
                orderByExpression = p => p.Price;
                descending = true;
            }
            else if (orderBy == "date")
            {
                orderByExpression = p => p.DateAdded;
                descending = true;
            }
            else
            {
                // Default sorting by date added descending
                orderByExpression = p => p.DateAdded;
                descending = true;
            }

            // Get paginated result based on the determined criteria
            Task<PaginatedList<Product>> paginatedList = _unitOfWork.Product.GetPaginated(productSet, orderByExpression, page, pageSize, includeProperties: "Category", descending: descending);

            // Populate the ProductTags field for each product
            foreach (var product in paginatedList.Result.Items)
            {
                product.ProductTags = _unitOfWork.ProductProductTag
                    .GetAll(ppt => ppt.ProductId == product.Id)
                    .Select(ppt => ppt.ProductTag)
                    .ToList();
            }

            return paginatedList;
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
