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

        public IActionResult Reply(int id)
        {
            ProductReview productReview = _unitOfWork.ProductReview.Get(pr => pr.Id == id, includeProperties: "Product");
            productReview.Images = _unitOfWork.ProductReviewImage.GetAll(pri => pri.ProductReviewId == id)
            .Select(pri => pri.Path)
            .ToList();
            return View(productReview);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reply(ProductReview model)
        {
            if (ModelState.IsValid)
            {
                var reviewFromDb = _unitOfWork.ProductReview.Get(pr => pr.Id == model.Id);

                if (reviewFromDb != null)
                {
                    reviewFromDb.Reply = model.Reply;
                    _unitOfWork.ProductReview.Update(reviewFromDb);
                    _unitOfWork.Save();

                    TempData["success"] = "Reply has been saved successfully.";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Review not found.");
            }

            // If we get here, something failed, redisplay form
            model.Images = _unitOfWork.ProductReviewImage.GetAll(pri => pri.ProductReviewId == model.Id)
                .Select(pri => pri.Path)
                .ToList();

            return View(model);
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
