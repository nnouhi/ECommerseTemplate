using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Create()
        { 
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            { 
                _unitOfWork.Product.Add(product);
                _unitOfWork.Save();
				TempData["success"] = "Product created successfully";
				return RedirectToAction("Index");
			}

			return View();
        }

        public IActionResult Edit(int? id) 
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
        public IActionResult Edit(Product product) 
        {
            if (ModelState.IsValid) 
            {
                _unitOfWork.Product.Update(product);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }

            return View();
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
