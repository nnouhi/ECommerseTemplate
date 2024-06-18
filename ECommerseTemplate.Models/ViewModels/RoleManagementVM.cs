using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerseTemplate.Models.ViewModels
{
    public class RoleManagementVM
    {
        public ApplicationUser? User { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? RoleList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? CompanyList { get; set; }
    }
}
