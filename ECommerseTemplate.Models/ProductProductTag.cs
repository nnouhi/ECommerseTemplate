using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerseTemplate.Models
{
    public class ProductProductTag
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Product")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

        [Display(Name = "Product Tag")]
        public int ProductTagId { get; set; }
        [ForeignKey("ProductTagId")]
        [ValidateNever]
        public ProductTag ProductTag { get; set; }
    }
}
