using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerseTemplate.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		public string ISBN { get; set; }

		[Required]
		public string Author { get; set; }

		[Required]
		[Display(Name = "List Price")]
		[Range(1, 1000)]
		public float ListPrice { get; set; }

		[Required]
		[Display(Name = "Price for 1-50")]
		[Range(1, 1000)]
		public float Price { get; set; }

		[Required]
		[Display(Name = "Price for 50+")]
		[Range(1, 1000)]
		public float Price50 { get; set; }

		[Required]
		[Display(Name = "Price for 100+")]
		[Range(1, 1000)]
		public float Price100 { get; set; }

		[DisplayName("Category")]
		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		[ValidateNever]
		public Category Category { get; set; }

		[ValidateNever]
		public string ImageUrl { get; set; }

		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)] // The database generates a value when a row is inserted.
		public DateTime DateAdded { get; set; }
		[NotMapped]
		[ValidateNever]
		public List<ProductTag> ProductTags { get; set; }
	}
}
