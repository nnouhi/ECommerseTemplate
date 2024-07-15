using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerseTemplate.Models
{
    public class ProductReviewImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Path { get; set; }

        public int ProductReviewId { get; set; }
        [ValidateNever]
        [ForeignKey("ProductReviewId")]
        public ProductReview ProductReview { get; set; }
    }
}
