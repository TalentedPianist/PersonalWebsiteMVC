using PersonalWebsiteMVC.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsiteMVC.Models
{
     public class GuestbookMixModel
     {
          public List<Guestbook> GuestbookAll { get; set; } = new List<Guestbook>();
          public Guestbook Guestbook { get; set; }
     }

     public class Guestbook 
     {
          [Key]
          public int GuestbookID { get; set; }
          [Required]
          [Display(Name = "Comment:")]
          [ProfanityValidator]
          public string GuestbookComment { get; set; }
          [Required]
          [Display(Name = "Name:")]
          [ProfanityValidator]
          public string GuestbookUser { get; set; }
          [Required]
          [Display(Name = "Email:")]
          [ProfanityValidator]
          public string GuestbookUserEmail { get; set; }
          [ProfanityValidator]
          [Display(Name = "Website:")]
          public string GuestbookUserWebsite { get; set; }
          public string GuestbookApproved { get; set; }
          public string GuestbookIP { get; set; }
          public DateTime GuestbookDate { get; set; }
     }
}
