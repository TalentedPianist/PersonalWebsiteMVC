using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalWebsiteMVC.Models
{
    public class Photos
    {
        [Key]
        public int PhotoID { get; set; }
        [ForeignKey("AlbumID")]// Build was throwing an error because this column didn't exist!!!
        public int AlbumID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? Author { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? DateCreated { get; set; }
    }

}
