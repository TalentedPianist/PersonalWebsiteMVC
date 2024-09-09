using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Areas.Photos.Controllers
{
    [Area("Photos")]
    public class HomeController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            List<Models.Album> model = _db.Albums.ToList();
            return View("~/Areas/Photos/Views/Index.cshtml", model);
        }
    }
}
