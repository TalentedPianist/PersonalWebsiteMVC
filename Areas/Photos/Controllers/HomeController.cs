using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Areas.Photos.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using X.PagedList.Extensions;

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

        [Route("Photos")]
        public IActionResult Index([FromQuery(Name ="pageNumber")] int? page)
        {
            AlbumsViewModel model = new AlbumsViewModel();
            var pageNumber = page ?? 1;
            model.PagedAlbums = _db.Albums.ToPagedList(pageNumber, 8);
           
            return View("~/Areas/Photos/Views/Home/Index.cshtml", model);
        }

    
    }
}
