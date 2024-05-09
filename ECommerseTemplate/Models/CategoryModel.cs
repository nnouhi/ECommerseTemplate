using System.ComponentModel.DataAnnotations;

namespace ECommerseTemplate.Models
{
    public class CategoryModel
    {
        // This is called Data Annotation, basically we are saying this property will be the PK
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}
