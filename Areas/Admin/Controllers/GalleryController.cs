using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using System.Text;
using Sakura.AspNetCore;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GalleryController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        private IWebHostEnvironment Host { get; set; }
        public GalleryController(ApplicationDbContext db, IWebHostEnvironment env) 
        {
            _db = db;
            Host = env;
        }


        [Microsoft.AspNetCore.Mvc.Route("Admin/Gallery")]
        public IActionResult Index()
        {
            return View(_db.Albums);
        }

        [Microsoft.AspNetCore.Mvc.Route("Admin/Gallery/Create")]
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

        [HttpGet]
        public IActionResult Update(int id, [FromQuery(Name="page")]int page)
        {
            var model = _db.Albums.Where(a => a.Id == id).FirstOrDefault();
            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(Host.WebRootPath + "\\Gallery\\" + model!.Name);
            try
            {
                ViewBag.Files = info.GetFiles().ToPagedList(page, 5);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ViewBag.Files = info.GetFiles().ToPagedList(1, 5);
            }
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
            Directory.CreateDirectory(Host.WebRootPath + "\\Gallery\\" + album!.Name);
            return View("~/Areas/Admin/Views/Gallery/Update.cshtml", album);
        }


    
    }
}
