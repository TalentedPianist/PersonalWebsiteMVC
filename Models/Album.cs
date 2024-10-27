using System.ComponentModel.DataAnnotations;

namespace PersonalWebsiteMVC.Models
{
    public class Album
    {
        public List<Album> AlbumData { get; set; } = new List<Album>();
        [Key]
        public int AlbumID { get; set; }
        public string? Name { get; set; } 
        public string? Description { get; set; } 
        public string? Location { get; set; } 
        public string? CoverPhoto { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
