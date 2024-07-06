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
    public class ProductProductTagController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductProductTagController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<ProductProductTag> productsProductTags = _unitOfWork.ProductProductTag.GetAll(includeProperties: "Product,ProductTag").ToList();
            return View(productsProductTags);
        }

        // Choose between Update/Create a product
        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> productListItem = _unitOfWork.Product.GetAll().Select(p => new SelectListItem { Text = p.Title, Value = p.Id.ToString() });
            IEnumerable<SelectListItem> productTagListItem = _unitOfWork.ProductTag.GetAll().Select(pt => new SelectListItem { Text = pt.Name, Value = pt.Id.ToString() });
            ProductProductTagVM productProductTagVM = new()
            {
                ProductProductTag = id.HasValue && id.Value != 0
                            ? _unitOfWork.ProductProductTag.Get(u => u.Id == id.Value)
                            : new ProductProductTag(),
                ProductList = productListItem,
                ProductTagList = productTagListItem,
            };
            return View(productProductTagVM);
        }

        [HttpPost]
        public IActionResult Upsert(ProductProductTag productProductTag)
        {
            if (ModelState.IsValid)
            {
                if (productProductTag.Id == 0)
                {
                    _unitOfWork.ProductProductTag.Add(productProductTag);
                }
                else
                {
                    _unitOfWork.ProductProductTag.Update(productProductTag);
                }

                TempData["success"] = $"Product {(productProductTag.Id == 0 ? "created" : "modified")} successfully";
                _unitOfWork.Save();
            }
            else
            {
                TempData["error"] = $"Product was not {(productProductTag.Id == 0 ? "created" : "modified")}";
            }

            return RedirectToAction(nameof(Index));
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ProductProductTag> productsProductTags = _unitOfWork.ProductProductTag.GetAll(includeProperties: "Product,ProductTag").ToList();
            return Json(new { data = productsProductTags });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productProductTagsToBeDeleted = _unitOfWork.ProductProductTag.Get(u => u.Id == id);
            if (productProductTagsToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.ProductProductTag.Remove(productProductTagsToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Successfully deleted" });
        }
        #endregion
    }
}
