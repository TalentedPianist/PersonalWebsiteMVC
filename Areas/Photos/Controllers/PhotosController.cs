using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;

namespace PersonalWebsiteMVC.Areas.Photos.Controllers
{
    [Area("Photos")]
    public class PhotosController : Controller
    {
        public ApplicationDbContext _db { get; set; }

        public PhotosController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("Photos")]
        public IActionResult Index()
        {

            return View();
        }
    }
}
