using System.ComponentModel.DataAnnotations;

namespace PersonalWebsiteMVC.Models
{
    public class Contact
    {
       
        public string? Name { get; set; }
      
        public string? Email { get; set; } 

        public string? Website { get; set; } 
       
        public string? Message { get; set; } 
    }
}
