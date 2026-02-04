using PersonalWebsiteMVC.Models;
using X.PagedList;

namespace PersonalWebsiteMVC.Areas.Photos.Models
{
    public class PhotosViewModel
    {
        public List<PersonalWebsiteMVC.Models.Photos>? Photos { get; set; }
        public IPagedList<PersonalWebsiteMVC.Models.Photos>? PagedPhotos { get; set; }
        public IPagedList<Album>? PagedAlbums { get; set; }
          public Comments Comments { get; set; } = new Comments();
          public IPagedList<Comments>? PagedComments { get; set; }
        public Album? SingleAlbum { get; set; }
        public PersonalWebsiteMVC.Models.Photos? SinglePhoto { get; set; }
        
        public List<Comments>? AllComments { get; set; }
    }
}
