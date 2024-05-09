using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GalleryController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        public GalleryController(ApplicationDbContext db) { _db = db; }

        [Route("Admin/Gallery")]
        public IActionResult Index()
        {
            return View(_db.Albums);
        }

        [Route("Admin/Gallery/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAlbum(Album model)
        {
            if (ModelState.IsValid)
            {
                Album album = new Album();
                album.Id = model.Id;
                album.DateCreated = DateTime.Now;
                album.Description = model.Description;
                album.CoverPhoto = model.CoverPhoto;
                album.Name = model.Name;
                album.Location = model.Location;
                _db.Add(album);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Create", model);
        }

        public IActionResult Update(int id)
        {
            var model = _db.Albums.Where(a => a.Id == id).FirstOrDefault();
            return View(model);
        }

   
        public IActionResult UpdateAlbum(Album model, [FromForm(Name="AlbumID")]int AlbumID)
        {
            var album = _db.Albums.Where(a => a.Id == AlbumID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                album!.Name = model.Name;
                album.Location = model.Location;
                album.Description = model.Description;
                album.CoverPhoto = model.CoverPhoto;
                _db.Update(album);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var model = _db.Albums.Where(a => a.Id == id).FirstOrDefault();
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var album = _db.Albums.Where(a => a.Id == id).FirstOrDefault();
            _db.Remove(album);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CreateFolder([FromForm(Name="GalleryID")]int GalleryID)
        {
            var album = _db.Albums.Where(a => a.Id == GalleryID).FirstOrDefault();
            System.IO.Directory.CreateDirectory("~/Gallery/" + album!.Name);
            return View("Update", album);
        }
    }
}
