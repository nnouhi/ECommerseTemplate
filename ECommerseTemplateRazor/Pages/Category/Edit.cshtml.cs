using ECommerseTemplateRazor.Data;
using ECommerseTemplateRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerseTemplateRazor.Pages.Category
{
    public class EditModel : PageModel
    {
		private readonly ApplicationDbContext _db;
		// This is a must in razor pages. We don't pass the new category in OnPost as param. It gets binded automnatically (can also use [BindPropterties on top of class])
		[BindProperty] 
		public CategoryModel? category { get; set; }

		public EditModel(ApplicationDbContext db)
		{
			_db = db;
		}

		public void OnGet(int? id)
		{
			if (id != null && id != 0)
			{
				category = _db.Categories.Find(id);
			}
		}

		public IActionResult OnPost()
		{
			if (ModelState.IsValid && category != null)
			{
				_db.Categories.Update(category);
				_db.SaveChanges();
				TempData["Success"] = "Category was updated successfully";
				return RedirectToPage("Index");
			}

			return NotFound();
		}
	}
}
