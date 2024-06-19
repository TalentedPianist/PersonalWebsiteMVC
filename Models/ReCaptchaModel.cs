using System.ComponentModel.DataAnnotations;

namespace PersonalWebsiteMVC.Models
{
    public class ReCaptchaModel
    {
        [Required]
        public string Text { get; set; } = string.Empty;
        public string CaptchaResponse { get; set; } = string.Empty;
    }
}
