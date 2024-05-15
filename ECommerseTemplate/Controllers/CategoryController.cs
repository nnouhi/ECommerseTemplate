using ECommerseTemplate.Data;
using ECommerseTemplate.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerseTemplate.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<CategoryModel> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
			return View();
		}

        [HttpPost]
		public IActionResult Create(CategoryModel obj)
		{
            if (ModelState.IsValid)
            {
				_db.Categories.Add(obj);
				_db.SaveChanges();
				TempData["success"] = "Category created successfully";
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

			// id of Edit is passed from the Index.cshtml asp-route-id="@obj.Id" 
			CategoryModel fetchedCategory = _db.Categories.Find(id);
			if (fetchedCategory == null)
			{
				return NotFound();
			}

			return View(fetchedCategory);
		}

		[HttpPost]
		public IActionResult Edit(CategoryModel obj)
		{
			if (ModelState.IsValid)
			{
				_db.Categories.Update(obj);
				_db.SaveChanges();
				TempData["success"] = "Category updated successfully";
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

			CategoryModel fetchedCategory = _db.Categories.Find(id);
			if (fetchedCategory == null)
			{
				return NotFound();
			}

			return View(fetchedCategory);
		}

		[HttpPost]
		public IActionResult Delete(CategoryModel obj)
		{
			if (ModelState.IsValid)
			{
				_db.Categories.Remove(obj);
				_db.SaveChanges();
				TempData["success"] = "Category deleted successfully";
				return RedirectToAction("Index");
			}

			return View();
		}
	}
}
