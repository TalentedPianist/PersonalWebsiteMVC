using System.ComponentModel.DataAnnotations;

namespace PersonalWebsiteMVC.Models
{
    public class Login
    {
          [Required]
          [EmailAddress(ErrorMessage="Please enter a valid email address.")]
          public string Email { get; set; } = default!;
          [Required]
          public string Password { get; set; } = default!;
        public string? ReturnUrl { get; set; }
    }
}
