using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ECommerseTemplate.Areas.Admin.Controllers
{
	[Area(SD.Roles.Admin)]
	[Authorize(Roles = SD.Roles.Admin)]
	public class ProductTagController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductTagController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			List<ProductTag> productTags = _unitOfWork.ProductTag.GetAll().ToList();
			return View(productTags);
		}

		// Choose between Update/Create a product
		public IActionResult Upsert(int? id)
		{
			ProductTag productTag = id.HasValue && id.Value != 0
				? _unitOfWork.ProductTag.Get(u => u.Id == id)
				: new ProductTag();

			return View(productTag);
		}

		[HttpPost]
		public IActionResult Upsert(ProductTag productTag)
		{
			if (ModelState.IsValid)
			{


				if (productTag.Id == 0)
				{
					_unitOfWork.ProductTag.Add(productTag);
				}
				else
				{
					_unitOfWork.ProductTag.Update(productTag);
				}

				TempData["success"] = $"Product tag {(productTag.Id == 0 ? "created" : "modified")} successfully";
				_unitOfWork.Save();
				return RedirectToAction(nameof(Index));
			}
			else
			{

				TempData["success"] = $"Product tag was not {(productTag.Id == 0 ? "created" : "modified")}";
				return View(productTag);
			}
		}

		#region API CALLS
		[HttpGet]
		public IActionResult GetAll()
		{
			List<ProductTag> productTags = _unitOfWork.ProductTag.GetAll().ToList();
			return Json(new { data = productTags });
		}

		[HttpDelete]
		public IActionResult Delete(int id)
		{
			var productTagsToBeDeleted = _unitOfWork.ProductTag.Get(u => u.Id == id);
			if (productTagsToBeDeleted == null)
			{
				return Json(new { success = false, message = "Error while deleting" });
			}

			_unitOfWork.ProductTag.Remove(productTagsToBeDeleted);
			_unitOfWork.Save();

			return Json(new { success = true, message = "Successfully deleted" });
		}
		#endregion
	}
}
