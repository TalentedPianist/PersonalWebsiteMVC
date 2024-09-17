using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Areas.Photos.Models;
using PersonalWebsiteMVC.Data;
using X.PagedList.Extensions;

namespace PersonalWebsiteMVC.Areas.Photos.Controllers
{
    [Area("Photos")]
    public class PhotosController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        public IHttpContextAccessor _http { get; set; }

        public PhotosController(ApplicationDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
        }

        [Route("Photos")]
        public IActionResult Index([FromQuery(Name="pageNumber")]int? pageNumber)
        {
            PhotosViewModel model = new PhotosViewModel();
            var photos = _db.Photos.Where(p => p.Name == _http.HttpContext!.Request.Query["name"].ToString());
            var page = pageNumber ?? 1;
            model.PagedPhotos = photos.ToPagedList(page, 1);
                
            return View(model);
        }
    }
}
