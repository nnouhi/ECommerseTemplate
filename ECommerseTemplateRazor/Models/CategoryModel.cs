using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ECommerseTemplateRazor.Models
{
    public class CategoryModel
    {
        // This is called Data Annotation, basically we are saying this property will be the PK
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100)]
        public int DisplayOrder { get; set; }
    }
}
