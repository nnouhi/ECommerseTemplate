using ECommerseTemplate.DataAccess.Repository.IRepository;
using ECommerseTemplate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerseTemplate.Areas.Admin.Controllers
{
    [Area(SD.Roles.Admin)]
    [Authorize(Roles = SD.Roles.Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> categories = _unitOfWork.Category.GetAll().ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // id of Edit is passed from the Index.cshtml asp-route-id="@obj.Id" 
            Category fetchedCategory = _unitOfWork.Category.Get(u => u.Id == id);
            if (fetchedCategory == null)
            {
                return NotFound();
            }

            return View(fetchedCategory);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category fetchedCategory = _unitOfWork.Category.Get(u => u.Id == id);
            if (fetchedCategory == null)
            {
                return NotFound();
            }

            return View(fetchedCategory);
        }

        [HttpPost]
        public IActionResult Delete(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Remove(obj);
                _unitOfWork.Save(); ;
                TempData["success"] = "Category deleted successfully";
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
