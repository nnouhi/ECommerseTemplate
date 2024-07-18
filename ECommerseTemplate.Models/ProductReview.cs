using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerseTemplate.Models
{
    public class ProductReview
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Name")]
        public string Name { get; set; } // Name of the user that left the review

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email (Not publicly displayed)")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        [Display(Name = "Rating")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Review text is required.")]
        public string Review { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // The database generates a value when a row is inserted.
        [ValidateNever]
        public DateTime DateAdded { get; set; }

        [ValidateNever]
        public bool IsAdminApproved { get; set; } // An admin needs to approve of the review to be published

        [ValidateNever]
        public string Reply { get; set; } = ""; // Admin reply to the review

        [Display(Name = "Images (Use CTRL to select multiple images to upload)")]
        [NotMapped]
        [ValidateNever]

        public List<string> Images { get; set; }

        [NotMapped]
        [ValidateNever]
        public string Country { get; set; } // The country of the user that left the review

        [NotMapped]
        [ValidateNever]
        public bool IsUserVerified { get; set; } // If the user is verified (If their email exists in our db)
    }
}
