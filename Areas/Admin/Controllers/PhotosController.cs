using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;
using System.Text;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhotosController : Controller
    {
        private ApplicationDbContext _db { get; set; }
        private IWebHostEnvironment Host { get; set; }
        public PhotosController(ApplicationDbContext db, IWebHostEnvironment host)
        {
            _db = db;
            Host = host;
        }

        [Route("Photos/Index")]
        [Route("Photos/Index/{id}")]
        public IActionResult Index(int id)
        {
            var album = _db.Albums.Where(a => a.Id == id).FirstOrDefault();
            var photos = _db.Photos.Where(p => p.AlbumID == id);
            var filePath = Host.WebRootPath + "\\Gallery\\" + album!.Name;
            ViewBag.AlbumName = album.Name;

            var files = Directory.GetFiles(filePath);
            ViewBag.Message = files.Count();
            StringBuilder sb = new StringBuilder();
            foreach (var file in files)
            {
                sb.Append(file);
            }
            ViewBag.Files = sb.ToString();
            return View();
        }
    }
}
