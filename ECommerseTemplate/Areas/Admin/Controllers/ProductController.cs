using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using ECommerseTemplate.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerseTemplate.Areas.Admin.Controllers
{
    [Area(SD.Roles.Admin)]
    [Authorize(Roles = SD.Roles.Admin)]
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
            IEnumerable<SelectListItem> categoryListItem = _unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem { Text = u.Name, Value = u.Id.ToString() });
            IEnumerable<SelectListItem> productTagListItem = _unitOfWork.ProductTag.GetAll()
                .Select(pt => new SelectListItem { Text = pt.Name, Value = pt.Id.ToString() });


            bool hasId = id.HasValue && id.Value != 0;
            ProductVM productVM = new()
            {
                Product = hasId
                            ? _unitOfWork.Product.Get(u => u.Id == id.Value)
                            : new Product(),
                CategoryList = categoryListItem,
                ProductTagList = productTagListItem,
                ProductTagIds = hasId
                    ? _unitOfWork.ProductProductTag.GetAll(pt => pt.ProductId == id.Value).Select(pt => pt.ProductTagId).ToList()
                    : new List<int>()
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
                    _unitOfWork.Save();

                    // After saving the product, we can get the new product id which will be populated
                    int newProductId = productVM.Product.Id;
                    foreach (int tagId in productVM.ProductTagIds)
                    {
                        _unitOfWork.ProductProductTag.Add(new ProductProductTag { ProductId = newProductId, ProductTagId = tagId });
                    }
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                    _unitOfWork.Save();

                    // Remove the existing tags
                    List<ProductProductTag> existingTags = _unitOfWork.ProductProductTag.GetAll(pt => pt.ProductId == productVM.Product.Id).ToList();
                    foreach (ProductProductTag existingTag in existingTags)
                    {
                        _unitOfWork.ProductProductTag.Remove(existingTag);
                    }
                    _unitOfWork.Save();

                    // Add the new updated tags
                    foreach (int tagId in productVM.ProductTagIds)
                    {
                        _unitOfWork.ProductProductTag.Add(new ProductProductTag { ProductId = productVM.Product.Id, ProductTagId = tagId });
                    }
                }

                TempData["success"] = $"Product {(productVM.Product.Id == 0 ? "created" : "modified")} successfully";
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = $"Product was not {(productVM.Product.Id == 0 ? "created" : "modified")}";
                return RedirectToAction(nameof(Index));
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, productToBeDeleted.ImageUrl);
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            var productTagsToBeDeleted = _unitOfWork.ProductProductTag.GetAll(pt => pt.ProductId == id).ToList();
            foreach (var productTag in productTagsToBeDeleted)
            {
                _unitOfWork.ProductProductTag.Remove(productTag);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Successfully deleted" });
        }
        #endregion
    }
}
