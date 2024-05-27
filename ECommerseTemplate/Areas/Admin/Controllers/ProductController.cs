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

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Product.GetAll().ToList();
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
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
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
	}
}
