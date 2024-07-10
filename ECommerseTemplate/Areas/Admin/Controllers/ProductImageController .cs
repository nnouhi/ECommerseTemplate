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
    public class ProductImageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public ProductImageController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            List<ProductImage> productImages = _unitOfWork.ProductImage.GetAll(includeProperties: "Product").ToList();
            return View(productImages);
        }

        // Choose between Update/Create a product
        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> productsListItems = _unitOfWork.Product.GetAll().Select(p => new SelectListItem
            {
                Text = p.Title,
                Value = p.Id.ToString()
            });
            ProductImage productImage = _unitOfWork.ProductImage.Get(pi => pi.Id == id, includeProperties: "Product");
            ProductImageVM productImageVM = new ProductImageVM
            {
                ProductImage = id.HasValue && id.Value != 0 ? productImage : new ProductImage(),
                ProductList = productsListItems
            };

            return View(productImageVM);
        }

        [HttpPost]
        public IActionResult Upsert(ProductImageVM productImageVM, IFormFile? file)
        {
            bool isNewEntry = productImageVM.ProductImage.Id == 0;
            if (ModelState.IsValid)
            {
                // Handle file upload
                string wwwRootPath = _hostingEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, "images", "product");

                    // Delete old image, replace with new one
                    if (!string.IsNullOrEmpty(productImageVM.ProductImage.Path))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productImageVM.ProductImage.Path);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productImageVM.ProductImage.Path = Path.Combine("images", "product", fileName);
                }

                if (isNewEntry)
                {
                    _unitOfWork.ProductImage.Add(productImageVM.ProductImage);
                }
                else
                {
                    _unitOfWork.ProductImage.Update(productImageVM.ProductImage);
                }

                _unitOfWork.Save();
                TempData["success"] = $"Product {(isNewEntry ? "created" : "modified")} successfully";
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = $"Product was not {(isNewEntry ? "created" : "modified")}";
                return RedirectToAction(nameof(Index));
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ProductImage> productImages = _unitOfWork.ProductImage.GetAll(includeProperties: "Product").ToList();
            return Json(new { data = productImages });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productImageToDelete = _unitOfWork.ProductImage.Get(pi => pi.Id == id);
            if (productImageToDelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.ProductImage.Remove(productImageToDelete);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Successfully deleted" });
        }
        #endregion
    }
}
