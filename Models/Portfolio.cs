using System.ComponentModel.DataAnnotations;

namespace PersonalWebsiteMVC.Models
{
     public class Portfolio
     {
          [Key]
          public int PortfolioID { get; set; }
          public string? Name { get; set; }
          public string? Url { get; set; }
          public string? Description { get; set; }
          public string? ImageUrl { get; set; }
          public DateTime DateCreated { get; set; }
          public string? IP { get; set; }
          public string? User { get; set; }

     }
}
