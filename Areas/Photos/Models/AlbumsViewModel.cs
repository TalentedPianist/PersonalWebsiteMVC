using PersonalWebsiteMVC.Models;
using X.PagedList;

namespace PersonalWebsiteMVC.Areas.Photos.Models
{
    public class AlbumsViewModel
    {
        public IPagedList<Album>? PagedAlbums { get; set; } 
    }
}
