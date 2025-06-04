using System.ComponentModel.DataAnnotations;

namespace PersonalWebsiteMVC.Models
{
    public class ViewModel
    {
        [Required]
        public string Text { get; set; } = string.Empty;
        [Required]
        public string CaptchaResponse { get; set; } = string.Empty;
    }
}
