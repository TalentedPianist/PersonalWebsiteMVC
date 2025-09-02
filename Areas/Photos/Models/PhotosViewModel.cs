using PersonalWebsiteMVC.Models;
using X.PagedList;

namespace PersonalWebsiteMVC.Areas.Photos.Models
{
    public class PhotosViewModel
    {
        public List<PersonalWebsiteMVC.Models.Photos>? Photos { get; set; }
        public IPagedList<PersonalWebsiteMVC.Models.Photos>? PagedPhotos { get; set; }
        public IPagedList<PersonalWebsiteMVC.Models.Album>? PagedAlbums { get; set; }
        public PersonalWebsiteMVC.Models.Comments? Comments { get; set; }
        public PersonalWebsiteMVC.Models.Album? SingleAlbum { get; set; }
        public PersonalWebsiteMVC.Models.Photos? SinglePhoto { get; set; }
        
        public IEnumerable<PersonalWebsiteMVC.Models.Comments>? AllComments { get; set; }
    }
}
