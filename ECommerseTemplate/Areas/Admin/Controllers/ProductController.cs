using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using ECommerseTemplate.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerseTemplate.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return View(products);
        }

        // Choose between Update/Create a product
        public IActionResult Upsert(int? id)
		{
			var categoryList = _unitOfWork.Category.GetAll()
				.Select(u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() });

			ProductVM productVM = new()
			{
				Product = id.HasValue && id.Value != 0
							? _unitOfWork.Product.Get(u => u.Id == id.Value)
							: new Product(),
				CategoryList = categoryList,
			};

			return View(productVM);
		}


		[HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
				// Handle file upload
				string wwwRootPath = _hostingEnvironment.WebRootPath;
				if (file != null)
				{
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, "images", "product");

					// Delete old image, replace with new one
					if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
					{
						var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl);
						if (System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
					{
						file.CopyTo(fileStream);
					}

                    productVM.Product.ImageUrl = Path.Combine("images", "product", fileName);
                }

				if (productVM.Product.Id == 0)
				{
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
				{
					_unitOfWork.Product.Update(productVM.Product);
				}

                TempData["success"] = $"Product {(productVM.Product.Id == 0 ? "created" : "modified")} successfully";
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
				productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() });
				return View(productVM);
			}
        }

		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Product fetchedProduct = _unitOfWork.Product.Get(u => u.Id == id);
			if (fetchedProduct == null)
			{
				return NotFound();
			}

			return View(fetchedProduct);
		}

		[HttpPost]
		public IActionResult Delete(Product product)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Product.Remove(product);
				_unitOfWork.Save(); ;
				TempData["success"] = "Product deleted successfully";
				return RedirectToAction("Index");
			}

			return View();
		}

		#region API CALLS
		[HttpGet]
		public IActionResult GetAll()
        {
            List<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
			return Json(new { data = products });
        }
        #endregion
    }
}
