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


        [Microsoft.AspNetCore.Mvc.Route("Photos/Album")]
        public IActionResult Index([FromQuery(Name = "pageNumber")] int? page)
        {
            int id = Convert.ToInt32(_http.HttpContext!.Request.Query["id"]);
            PhotosViewModel model = new PhotosViewModel();
            var pageNumber = page ?? 1;
            model.PagedPhotos = _db.Photos.Where(p => p.AlbumID == id).ToPagedList(pageNumber, 8);
            model.Photos = _db.Photos.Where(p => p.AlbumID == id).ToList();


            var album = _db.Albums.Where(a => a.AlbumID == id).FirstOrDefault();
            ViewBag.AlbumName = album!.Name;
            return View("~/Areas/Photos/Views/Album/Index.cshtml", model);
        }

        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("Photos/Album/GetPhotos")]
        public IActionResult GetPhotos(int id)
        {
            var photo = _db.Photos.Where(p => p.AlbumID == id).FirstOrDefault();
            var comments = _db.Comments.Where(c => Convert.ToInt32(c.PhotoID) == id).ToList();
            ViewBag.Comments = comments;

            return Ok(photo);
        }

        [HttpPost]
        [Route("/Album/GetPhoto")]
        public IActionResult GetPhoto(string name)
        {
            var photo = _db.Photos.Where(p => p.Name == name).FirstOrDefault();
            return Ok(photo);
        }

        
        [Route("/Album/GetComments")]
        public IActionResult GetComments(string name)
        {
            var photo = _db.Photos.Where(p => p.Name == name).FirstOrDefault();
            
            return Ok(photo!.Name);
        }
    }
}
