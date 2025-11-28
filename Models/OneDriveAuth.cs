namespace PersonalWebsiteMVC.Models
{
     public class OneDriveAuth
     {
          public string token_type { get; set; } = default!;
          public string expires_in { get; set; } = default!;
          public string access_token { get; set; } = default!;
     }
}
