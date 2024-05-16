using ECommerseTemplateRazor.Data;
using ECommerseTemplateRazor.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerseTemplateRazor.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public List<CategoryModel> categories { get; set; }

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            categories = _db.Categories.ToList();
        }
    }
}
