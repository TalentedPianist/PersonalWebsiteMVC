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

       [Route("Photos/Album")]
        public IActionResult Index([FromQuery(Name="pageNumber")]int? page)
        {
            int id = Convert.ToInt32(_http.HttpContext!.Request.Query["id"]);
            PhotosViewModel model = new PhotosViewModel();
            var pageNumber = page ?? 1;
            model.PagedPhotos = _db.Photos.Where(p => p.AlbumID == id).ToPagedList(pageNumber, 8);
            model.Photos = _db.Photos.Where(p => p.AlbumID == id).ToList();
            

            var album = _db.Albums.Where(a => a.Id == id).FirstOrDefault();
            ViewBag.AlbumName = album!.Name;
            return View("~/Areas/Photos/Views/Album/Index.cshtml", model);
        }

        [HttpPost]
        public IActionResult GetPhoto(int id)
        {
            var photo = _db.Photos.Where(p => p.Id == id).FirstOrDefault();
            return Ok(photo);
        }
    }
}
