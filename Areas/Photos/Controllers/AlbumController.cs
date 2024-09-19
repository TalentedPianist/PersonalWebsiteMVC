using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Areas.Photos.Models;
using PersonalWebsiteMVC.Data;
using X.PagedList.Extensions;

namespace PersonalWebsiteMVC.Areas.Photos.Controllers
{
    [Area("Photos")]
    public class AlbumController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        public IHttpContextAccessor _http { get; set; }

        public AlbumController(ApplicationDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
        }

       
        public IActionResult Index(int? page)
        {
            ViewBag.Message = DateTime.Now;
            PhotosViewModel model = new PhotosViewModel();
            var albums = _db.Albums;
            var pageNumber = page ?? 1;
            model.PagedAlbums = albums.ToPagedList(pageNumber, 1);
            return View(model);
        }
    }
}
