using System.ComponentModel.DataAnnotations;

namespace ECommerseTemplate.Models
{
    public class ProductTag
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Product Tag Name")]
        public string Name { get; set; }
        // public bool Disabled { get; set; } = false;
    }
}
