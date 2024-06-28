using System.ComponentModel.DataAnnotations;

namespace PersonalWebsiteMVC.Models
{
    public class User
    {
       
       public string? Name { get; set; } 
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; } 
    }
}