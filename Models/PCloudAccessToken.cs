using System.ComponentModel.DataAnnotations;

namespace PersonalWebsiteMVC.Models
{
     public class PCloudAccessToken
     {
          [Key]
          public string Id { get; set; } = default!;
          public string Token { get; set; } = default!;
     }
}
