using ECommerseTemplateRazor.Data;
using ECommerseTemplateRazor.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerseTemplateRazor.Pages.Category
{
    public class CreateModel : PageModel
    {
		private readonly ApplicationDbContext _db;
		// This is a must in razor pages. We don't pass the new category in OnPost as param. It gets binded automnatically (can also use [BindPropterties on top of class])
		[BindProperty] 
		public CategoryModel category { get; set; }

		public CreateModel(ApplicationDbContext db)
		{
			_db = db;
		}

		public void OnGet()
		{
		}

		public IActionResult OnPost()
		{
			if (ModelState.IsValid)
			{
				_db.Categories.Add(category);
				_db.SaveChanges();
				TempData["Success"] = "Category was created successfully";
				return RedirectToPage("Index");
			}

			return NotFound();
		}
	}
}
