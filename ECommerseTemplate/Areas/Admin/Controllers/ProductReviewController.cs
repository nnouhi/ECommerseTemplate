using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ECommerseTemplate.Areas.Admin.Controllers
{
	[Area(SD.Roles.Admin)]
	[Authorize(Roles = SD.Roles.Admin)]
	public class ProductReviewController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public ProductReviewController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			List<ProductReview> productReviews = _unitOfWork.ProductReview.GetAll(includeProperties: "Product").ToList();
			return View(productReviews);
		}

		#region API CALLS
		[HttpGet]
		public IActionResult GetAll()
		{
			List<ProductReview> productReviews = _unitOfWork.ProductReview.GetAll(includeProperties: "Product").ToList();
			// Process each review
			foreach (ProductReview review in productReviews)
			{
				review.Images = _unitOfWork.ProductReviewImage.GetAll(pri => pri.ProductReviewId == review.Id)
															   .Select(pri => pri.Path)
															   .ToList();
			}
			return Json(new { data = productReviews });
		}

		[HttpGet]
		public IActionResult VerifyReview(int id, bool verify)
		{
			ProductReview productReview = _unitOfWork.ProductReview.Get(r => r.Id == id);
			productReview.IsAdminApproved = verify;
			_unitOfWork.ProductReview.Update(productReview);
			_unitOfWork.Save();
			return RedirectToAction(nameof(Index));
		}
		#endregion
	}
}
