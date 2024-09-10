using Microsoft.AspNetCore.Mvc;

namespace PersonalWebsiteMVC.Areas.Photos.Controllers
{
    [Area("Photos")]
    public class AlbumController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
