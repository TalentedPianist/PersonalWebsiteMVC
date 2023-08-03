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
        public string Name { get; set; } = default!;
        [Display(Name = "Email:")]
        public string Email { get; set; } = default!;
        [Display(Name = "Message:")]
        public string Message { get; set; } = default!;
     }
}
