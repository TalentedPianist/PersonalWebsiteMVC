using PersonalWebsiteMVC.Models;
using X.PagedList;

namespace PersonalWebsiteMVC.Areas.Photos.Models
{
    public class PhotosViewModel
    {
        public List<PersonalWebsiteMVC.Models.Photos>? Photos { get; set; } 
        public IPagedList<PersonalWebsiteMVC.Models.Photos>? PagedPhotos { get; set; }
        public IPagedList<PersonalWebsiteMVC.Models.Album>? PagedAlbums { get; set; }
        public List<Comments>? Comments { get; set; }
    }
}
