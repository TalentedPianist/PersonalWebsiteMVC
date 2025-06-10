using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsiteMVC.Models
{
   public class ContactFormModel
   {
      [Display(Name = "Name:")]
      [Required]
      public string Name { get; set; } = default!;
      [Display(Name = "Email:")]
      [Required]
      public string Email { get; set; } = default!;
      public string Website { get; set; } = default!;
      [Display(Name = "Message:")]
      [Required]
      public string Message { get; set; } = default!;
   }
}
