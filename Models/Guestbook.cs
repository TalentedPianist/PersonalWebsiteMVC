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
          public string GuestbookComment { get; set; }
          [Required]
          [Display(Name = "Name:")]

          public string GuestbookUser { get; set; }
          [Required]
          [Display(Name = "Email:")]

          public string GuestbookUserEmail { get; set; }

          [Display(Name = "Website:")]
          public string GuestbookUserWebsite { get; set; }
          public string GuestbookApproved { get; set; }
          public string GuestbookIP { get; set; }
          public DateTime GuestbookDate { get; set; }
     }
}
