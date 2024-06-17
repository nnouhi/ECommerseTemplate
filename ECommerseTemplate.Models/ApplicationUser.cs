using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerseTemplate.Models
{
	// ApplicationUser extends IdentityUser (Basically adds more columns to the db table)
	public class ApplicationUser : IdentityUser
	{
		[Required]
		public string? Name { get; set; }
		public string? StreetAddress { get; set; }
		public string? City { get; set; }
		public string? Country { get; set; }
		public string? PostalCode { get; set; }
		public int? CompanyId { get; set; }
		[ValidateNever]
		[ForeignKey("CompanyId")]
		public Company? Company { get; set; }
		[NotMapped]
		public string? Role { get; set; }
	}
}
