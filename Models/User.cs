using System.ComponentModel.DataAnnotations;

namespace PersonalWebsiteMVC.Models
{
    public class User
    {
          public string? Id { get; set; } = default!;
          public string? FirstName { get; set; } = default!;
          public string? LastName { get; set; } = default!;
          [Required]
          public string? Email { get; set; } = default!;
          [Required]
          public string? UserName { get; set; } = default!;
          [Required]

          public string? Password { get; set; } = default!;
    }
}