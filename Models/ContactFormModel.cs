using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsiteMVC.Models
{
   public class ContactFormModel
   {
      
      public string? Name { get; set; } 
      [Required]
      [EmailAddress(ErrorMessage="Email address is invalid.")]
      public string? Email { get; set; } 
      public string? Website { get; set; } 
      
      public string? Message { get; set; } 
   }
}
