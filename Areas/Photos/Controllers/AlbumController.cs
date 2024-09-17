using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;

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

        public IActionResult Index()
        {
            ViewBag.Message = DateTime.Now;
            return View();
        }
    }
}
