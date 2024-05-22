using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [DisplayName("List Price")]
        [Range(1, 1000)]
        public float ListPrice { get; set; }
        [Required]
        [DisplayName("Price for 1-50")]
        [Range(1, 1000)]
        public float Price { get; set; }
        [Required]
        [DisplayName("Price for 50+")]
        [Range(1, 1000)]
        public float Price50 { get; set; }
        [Required]
        [DisplayName("Price for 100+")]
        [Range(1, 1000)]
        public float Price100 { get; set; }
      
    }
}
