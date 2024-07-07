using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ECommerseTemplate.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ProductTagList { get; set; }
        [Display(Name = "Product Tags (Hold ctrl or shift to select multiple tags)")]
        public List<int> ProductTagIds { get; set; } // New property for selected tags
    }
}
