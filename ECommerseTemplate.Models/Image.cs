using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerseTemplate.Models
{
	public class Image
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Path { get; set; }
		[Display(Name = "Product")]
		public int ProductId { get; set; }
		[ForeignKey("ProductId")]
		[ValidateNever]
		public Product Product { get; set; }
	}
}
