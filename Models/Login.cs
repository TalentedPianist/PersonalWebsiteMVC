using System.ComponentModel.DataAnnotations;

namespace PersonalWebsiteMVC.Models
{
    public class Login
    {
          [Required]
          public string Email { get; set; } = default!;
          [Required]
          public string Password { get; set; } = default!;
        public string? ReturnUrl { get; set; }
    }
}
