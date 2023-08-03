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
        public Guestbook Guestbook { get; set; } = default!;
     }

     public class Guestbook 
     {
          [Key]
          public int GuestbookID { get; set; }
        [Required]
        [Display(Name = "Comment:")]
        public string GuestbookComment { get; set; } = default!;
        [Required]
        [Display(Name = "Name:")]

        public string GuestbookUser { get; set; } = default!;
        [Required]
        [Display(Name = "Email:")]

        public string GuestbookUserEmail { get; set; } = default!;

        [Display(Name = "Website:")]
        public string GuestbookUserWebsite { get; set; } = default!;
        public string GuestbookApproved { get; set; } = default!;
        public string GuestbookIP { get; set; } = default!;
          public DateTime GuestbookDate { get; set; }
     }
}
