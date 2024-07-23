using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalWebsiteMVC.Models
{
    public class Photos
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("AlbumID")]
        public int AlbumID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? Author { get; set; }
        public string? ImageUrl { get; set; }
        
    }

}
