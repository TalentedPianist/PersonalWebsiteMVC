using System.ComponentModel.DataAnnotations;

namespace PersonalWebsiteMVC.Models
{
    public class Guestbook
    {
        [Key]
        public int Id { get; set; }
        
        public string? GuestbookAuthor { get; set; } 
        
        public string? GuestbookAuthorEmail { get; set; } 
        public string? GuestbookAuthorUrl { get; set; } 
        
        public string? GuestbookContent { get; set; } 
        public string? GuestbookAuthorIP { get; set; } 
        public string? GuestbookApproved { get; set; } 
        public DateTime GuestbookDate { get; set; }

    }
}