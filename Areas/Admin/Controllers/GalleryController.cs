using HGO.ASPNetCore.FileManager.CommandsProcessor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using System.Text;
using System.Web;
using X.PagedList;
using X.PagedList.Extensions;


namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    [Area("Admin")]
    public class GalleryController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        private IWebHostEnvironment Host { get; set; }
        private IHttpContextAccessor _http { get; set; }

        public GalleryController(ApplicationDbContext db, IWebHostEnvironment env, IHttpContextAccessor http)
        {
            _db = db;
            Host = env;
            _http = http;
        }


        [Route("Admin/Gallery")]
        public IActionResult Index([FromQuery(Name = "pageNumber")] int? page)
        {
            DirectoryInfo di = new DirectoryInfo(System.IO.Path.Combine(Host.ContentRootPath, "Gallery"));
            if (di.GetDirectories().Count() == 0)
            {
                TempData["Message"] = "No albums have been found just now.";
            }
            else
            {
                ViewBag.Albums = di.GetDirectories();
                var pageNumber = page ?? 1;
                var onePageOfFiles = di.GetDirectories().ToPagedList(pageNumber, 10);
                ViewBag.OnePageOfFiles = onePageOfFiles;
            }



            return View();
        }

        [Route("Admin/Gallery/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Admin/Gallery/Create")]
        public IActionResult CreateAlbum(Album model, [FromForm(Name = "coverPhoto")] IFormFile file)
        {
            if (ModelState.IsValid)
            {
                Album album = new Album();
                album.AlbumID = model.AlbumID;
                album.DateCreated = DateTime.Now;
                album.Description = model.Description;
                album.CoverPhoto = file.FileName;
                album.Name = model.Name;
                album.Location = model.Location;
                _db.Add(album);
                _db.SaveChanges();

                if (Directory.Exists(Host.WebRootPath + "\\Gallery\\" + model.Name))
                {
                    // Directory exists in folder, do nothing
                }
                else
                {
                    Directory.CreateDirectory(Host.WebRootPath + "\\Gallery\\" + model.Name); // Create directory in Gallery folder
                }

                return RedirectToAction("Index");
            }
            return View("Create", model);
        }

        [HttpGet]
        public IActionResult Update(int id, [FromQuery(Name = "page")] int page)
        {
            var model = _db.Albums.Where(a => a.AlbumID == id).FirstOrDefault();
            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(Host.WebRootPath + "\\Gallery\\" + model!.Name);

            return View(model);
        }


        [HttpPost]
        [Route("Admin/Gallery/Update")]
        public IActionResult UpdateAlbum(Album model, [FromForm(Name = "AlbumID")] int AlbumID)
        {
            var album = _db.Albums.Where(a => a.AlbumID == AlbumID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                album!.Name = model.Name;
                album.Location = model.Location;
                album.Description = model.Description;
                _db.Update(album);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            TempData["Message"] = model.Description;
            return View("~/Areas/Admin/Views/Gallery/Update.cshtml", model);
        }

        public IActionResult Details(string name)
        {
            var model = _db.Albums.Where(a => a.Name == name).FirstOrDefault();
            return View(model);
        }

        [Route("Admin/Gallery/Delete/{id?}")]
        public IActionResult Delete(int id)
        {
            var album = _db.Albums.Where(a => a.AlbumID == id).FirstOrDefault();
            if (album == null)
            {
                return Ok();
            }
            else
            {
                _db.Remove(album);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult CreateFolder([FromForm(Name = "GalleryID")] int GalleryID)
        {
            var album = _db.Albums.Where(a => a.AlbumID == GalleryID).FirstOrDefault();
            Directory.CreateDirectory(Host.WebRootPath + "\\Gallery\\" + album!.Name);
            return View("~/Areas/Admin/Views/Gallery/Update.cshtml", album);
        }


        public bool DirectoryIsEmpty(string path)
        {
            int fileCount = Directory.GetFiles(path).Length;
            if (fileCount > 0)
            {
                return false;
            }

            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                if (!DirectoryIsEmpty(dir))
                {
                    return false;
                }
            }

            return true;
        }

        [HttpPost]
        [Route("/Gallery/CheckDb")]
        public bool CheckDb([FromForm(Name = "name")] string name)
        {
            var photo = _db.Albums.Where(a => a.Name == name).Any();
            if (photo)
                return true;
            else
                return false;


        }

        [HttpPost]
        [Route("Gallery/AddMultipleToDb")]
        public IActionResult AddMultipleToDb(string name)

        {
            Album album = new Album();
            album.Name = name;
            album.DateCreated = DateTime.Now;
            _db.Albums.Add(album);
            _db.SaveChanges();
            return Ok(album);
        }

        [HttpPost]
        [Route("/Gallery/DelMultipleFromDb")]
        public IActionResult RemoveMultipleFromDb(string name)
        {
            var album = _db.Albums.Where(a => a.Name == name).FirstOrDefault();
            _db.Albums.Remove(album!);
            _db.SaveChanges();
            return Ok(album);
        }

        [HttpPost]
        public int GetId([FromForm(Name = "name")] string name)
        {
            var album = _db.Albums.Where(a => a.Name == name).FirstOrDefault();
            if (album is not null)
            {
                return album!.AlbumID;
            }
            else
            {
                return 0;
            }
           
        }

        [HttpPost]
        [Route("Photos/GetAlbum")]
        public IActionResult GetAlbum(int id)
        {
            var album = _db.Albums.Where(a => a.AlbumID == id).FirstOrDefault();
            return Ok(album!.Name);
        }

        [HttpPost]
        public IActionResult AddToDb(Album album, List<Models.Photos> photos)
        {
            
            return Ok();
        }

        [HttpPost]
        [Route("/Gallery/AjaxDbCheck")]
        public bool AjaxDbCheck([FromForm(Name = "name")] string name)
        {
            if (_db.Albums.Where(a => a.Name == name).Any())
                return true;
            else
                return false;
        }
    }
}
