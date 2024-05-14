using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using System.Text;
using X.PagedList;

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
            return View("~/Admin/Views/Gallery/Update.cshtml", album);
        }


        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("Gallery/AddPhotos")]
        public async Task<IActionResult> AddPhotos([FromForm(Name ="AlbumPhotos")]List<IFormFile> files, [FromForm(Name="AlbumID")]int AlbumID)
        {
            var album = _db.Albums.Where(a => a.Id == AlbumID).FirstOrDefault();
            long size = files.Sum(f => f.Length);
            var filePath = Host.WebRootPath + "\\Gallery\\" + album!.Name;
           
            TempData["Message"] = filePath;
            StringBuilder sb = new StringBuilder();
            foreach (var formFile in files)
            {
                using (var stream = System.IO.File.Create(filePath + "\\" + formFile.FileName))
                {
                    await formFile.CopyToAsync(stream);
                }

                    Photos photo = new Photos();
                photo.AlbumID = AlbumID;
                photo.Name = formFile.Name;
                _db.Photos.Add(photo);
                _db.SaveChanges();
            }
    
            await Task.CompletedTask;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("Admin/Gallery/DeleteFiles")]
        public async Task<IActionResult> DeleteFiles([FromForm(Name ="FileName")]List<string> files, [FromForm(Name="AlbumID")]int AlbumID, [FromQuery(Name ="page")]int page)
        {
            var album = _db.Albums.Where(a => a.Id == AlbumID).FirstOrDefault();
            StringBuilder sb = new StringBuilder();
            foreach (string file in files)
            {
                sb.Append(file);
            }
            TempData["Message"] = sb.ToString();
            await Task.CompletedTask;
            return View("~/Areas/Admin/Views/Gallery/Update.cshtml", album);
        }
    }
}
