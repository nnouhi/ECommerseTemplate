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
        private readonly IWebHostEnvironment _hostingEnvironment;
        private int _pageSize = 5;

        public ShopController(ILogger<ShopController> logger, IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
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

            // This will be saved in the database in the future

            PaginatedList<Product> paginatedList = GetPaginatedItems(page, _pageSize, orderBy, productTag, minPrice, maxPrice, searchByName, out int minSliderPrice, out int maxSliderPrice);
            IPagedList<Product> productsPagedList = new StaticPagedList<Product>(paginatedList.Items, page, _pageSize, paginatedList.TotalItemCount);
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

        public IActionResult Details(int id, int page = 1, string reviewFilter = "")
        {
            // Fetch the product including its category
            Product product = _unitOfWork.Product.Get(u => u.Id == id, includeProperties: "Category");
            if (product == null)
            {
                return NotFound();
            }

            // Fetch product images
            List<string> productImages = _unitOfWork.ProductImage.GetAll(pi => pi.ProductId == id)
                                                                  .Select(pi => pi.Path)
                                                                  .ToList();

            // Create the shopping cart
            ShoppingCart shoppingCart = new ShoppingCart
            {
                Product = product,
                Count = 1,
                ProductImages = productImages
            };

            // Fetch and filter approved reviews
            IQueryable<ProductReview> approvedReviewsQuery = _unitOfWork.ProductReview.GetAll(pr => pr.ProductId == id && pr.IsAdminApproved);
            var ratingOverview = CalculateRatingMetrics(approvedReviewsQuery);
            // Apply filter based on reviewFilter parameter
            switch (reviewFilter.ToLower())
            {
                case "1-star":
                case "2-star":
                case "3-star":
                case "4-star":
                case "5-star":
                    int rating = int.Parse(reviewFilter.Split('-')[0]);
                    approvedReviewsQuery = approvedReviewsQuery.Where(pr => pr.Rating == rating);
                    break;
                case "latest":
                    approvedReviewsQuery = approvedReviewsQuery.OrderByDescending(pr => pr.DateAdded);
                    break;
                case "oldest":
                    approvedReviewsQuery = approvedReviewsQuery.OrderBy(pr => pr.DateAdded);
                    break;
                case "include-pictures":
                    // Create a hash table to store reviews with at least one image
                    var reviewsWithImages = _unitOfWork.ProductReviewImage
                        .GetAll()
                        .Select(pri => pri.ProductReviewId)
                        .Distinct()
                        .ToHashSet();
                    approvedReviewsQuery = approvedReviewsQuery.Where(pr => reviewsWithImages.Contains(pr.Id));
                    break;
                case "highest-rating":
                    approvedReviewsQuery = approvedReviewsQuery.OrderByDescending(pr => pr.Rating);
                    break;
                case "lowest-rating":
                    approvedReviewsQuery = approvedReviewsQuery.OrderBy(pr => pr.Rating);
                    break;
                default:
                    approvedReviewsQuery = approvedReviewsQuery.OrderByDescending(pr => pr.DateAdded);
                    break;
            }

            // Paginate the filtered reviews
            PaginatedList<ProductReview> paginatedReviews = _unitOfWork.ProductReview
                                                                         .GetPaginated(approvedReviewsQuery, page, _pageSize)
                                                                         .GetAwaiter()
                                                                         .GetResult();

            IPagedList<ProductReview> reviewsPagedList = new StaticPagedList<ProductReview>(paginatedReviews.Items, page, _pageSize, paginatedReviews.TotalItemCount);

            // Process each review
            foreach (ProductReview review in paginatedReviews.Items)
            {
                ApplicationUser user = _unitOfWork.ApplicationUser.Get(u => u.Email == review.Email);
                review.Images = _unitOfWork.ProductReviewImage.GetAll(pri => pri.ProductReviewId == review.Id)
                                                               .Select(pri => pri.Path)
                                                               .ToList();
                review.IsUserVerified = user != null;
                review.Country = user != null ? user.Country : "Unknown";
            }

            // Create the view model
            ProductDetailsVM productDetailsVM = new ProductDetailsVM
            {
                ShoppingCart = shoppingCart,
                Reviews = reviewsPagedList,
                ProductId = id,
                RatingOverview = ratingOverview,
                ReviewFilter = reviewFilter,
            };

            // Add to recently viewed products
            AddRecentViewedProductId(id.ToString());

            return View(productDetailsVM);
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

        [HttpPost]
        public IActionResult Review(ProductDetailsVM productDetailsVM, List<IFormFile> files)
        {
            ProductReview productReview = productDetailsVM.Review;
            _unitOfWork.ProductReview.Add(productReview);
            _unitOfWork.Save();

            if (files.Count > 0)
            {
                string wwwRootPath = _hostingEnvironment.WebRootPath;
                foreach (IFormFile file in files)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, "images", "review", productReview.Id.ToString());

                    // Ensure the directory exists
                    if (!Directory.Exists(productPath))
                    {
                        Directory.CreateDirectory(productPath);
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    ProductReviewImage productReviewImage = new ProductReviewImage()
                    {
                        Path = Path.Combine("images", "review", productReview.Id.ToString(), fileName),
                        ProductReviewId = productReview.Id
                    };

                    _unitOfWork.ProductReviewImage.Add(productReviewImage);
                    _unitOfWork.Save();
                }
            }

            TempData["Success"] = "Review added successfully, your review will be visible to others once it has been reviewed and approved by an admin.";
            return RedirectToAction(nameof(Index));
        }

        private PaginatedList<Product> GetPaginatedItems(
            int page,
            int pageSize,
            string orderBy,
            string productTag,
            int minPrice,
            int maxPrice,
            string searchByName,
            out int minSliderPrice,
            out int maxSliderPrice)
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

            // Apply filter by name if searchByName is provided
            if (!string.IsNullOrEmpty(searchByName))
            {
                productSet = productSet.Where(p => p.Title.Contains(searchByName, StringComparison.OrdinalIgnoreCase));
            }

            // Calculate min and max slider prices
            minSliderPrice = (int)productSet.Min(p => p.Price);
            maxSliderPrice = (int)productSet.Max(p => p.Price);

            // Apply filtering by price range
            if (minPrice > 0 || maxPrice < int.MaxValue)
            {
                productSet = productSet.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            }

            // Determine sorting criteria
            switch (orderBy.ToLower())
            {
                case "price":
                    productSet = productSet.OrderBy(p => p.Price);
                    break;
                case "price-desc":
                    productSet = productSet.OrderByDescending(p => p.Price);
                    break;
                case "date":
                    productSet = productSet.OrderBy(p => p.DateAdded);
                    break;
                case "date-desc":
                    productSet = productSet.OrderByDescending(p => p.DateAdded);
                    break;
                default:
                    productSet = productSet.OrderByDescending(p => p.DateAdded);
                    break;
            }

            // Get paginated result
            PaginatedList<Product> paginatedList = _unitOfWork.Product.GetPaginated(productSet, page, pageSize).GetAwaiter().GetResult();

            // Populate the ProductTags field for each product
            foreach (var product in paginatedList.Items)
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

        private ProductDetailsVM.RatingMetrics CalculateRatingMetrics(IEnumerable<ProductReview> reviews)
        {
            int reviewCount = reviews.Count();
            Dictionary<int, int> countOfVotesPerStar = new Dictionary<int, int>();
            Dictionary<int, int> percentageOfVotesPerStar = new Dictionary<int, int>();
            // Init dicts
            for (int i = 1; i <= 5; i++)
            {
                countOfVotesPerStar.Add(i, 0);
                percentageOfVotesPerStar.Add(i, 0);
            }


            foreach (ProductReview review in reviews)
            {
                var rating = review.Rating;
                if (countOfVotesPerStar.ContainsKey(rating))
                {
                    countOfVotesPerStar[rating]++;
                }
            }


            int weightedSum = countOfVotesPerStar.Sum(entry => entry.Key * entry.Value);
            int averageRating = reviewCount > 0 ? (int)Math.Round((double)weightedSum / reviewCount) : 0;

            // Calculate percentage of votes per star
            foreach (var key in countOfVotesPerStar.Keys.ToList())
            {
                percentageOfVotesPerStar[key] = (int)Math.Round((countOfVotesPerStar[key] / (double)reviewCount) * 100);
            }

            ProductDetailsVM.RatingMetrics ratingOverview = new ProductDetailsVM.RatingMetrics()
            {
                ReviewCount = reviewCount,
                AverageRating = averageRating,
                CountOfVotesPerStar = countOfVotesPerStar,
                PercentageOfVotesPerStar = percentageOfVotesPerStar
            };
            return ratingOverview;
        }
    }
}
