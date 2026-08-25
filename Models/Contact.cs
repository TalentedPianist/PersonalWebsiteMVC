using System.ComponentModel.DataAnnotations;

namespace PersonalWebsiteMVC.Models
{
    public class Contact
    {
       [Required]
        public string? Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage="Please enter a valid email address")]
        public string? Email { get; set; } 

        public string? Website { get; set; } 
       [Required]
        public string? Message { get; set; } 
    }
}
