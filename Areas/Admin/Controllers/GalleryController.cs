using HGO.ASPNetCore.FileManager.CommandsProcessor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using System.Text;
using System.Web;
using X.PagedList;
using X.PagedList.Extensions;


namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GalleryController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        private IWebHostEnvironment Host { get; set; }
        private readonly IFileManagerCommandsProcessor _processor;
       
        public GalleryController(ApplicationDbContext db, IWebHostEnvironment env, IFileManagerCommandsProcessor processor) 
        {
            _db = db;
            Host = env;
            _processor = processor;
        }


        [Microsoft.AspNetCore.Mvc.Route("Admin/Gallery")]
        public IActionResult Index([FromQuery(Name = "pageNumber")]int? page)
        {
            var filePath = Path.Combine(Host.ContentRootPath, "Gallery");
            DirectoryInfo di = new DirectoryInfo(filePath);

            if (System.IO.Directory.GetDirectories(filePath).Length > 0)
            {
                ViewBag.NoAlbums = false;
            }
            else
            {
                ViewBag.NoAlbums = true;
            }

            if (ViewBag.NoAlbums == false)
            {
                ViewBag.AllDirectories = di.GetDirectories();
            }

            var gallery = _db.Albums;
            var pageNumber = page ?? 1;
            var model = gallery.ToPagedList(pageNumber, 10);
            return View(model);
        }

        [Microsoft.AspNetCore.Mvc.Route("Admin/Gallery/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("Admin/Gallery/Create")]
        public IActionResult CreateAlbum(Album model, [FromForm(Name="coverPhoto")]IFormFile file)
        {
            if (ModelState.IsValid)
            {
                Album album = new Album();
                album.Id = model.Id;
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
        public IActionResult Update(int id, [FromQuery(Name="page")]int page)
        {
            var model = _db.Albums.Where(a => a.Id == id).FirstOrDefault();
            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(Host.WebRootPath + "\\Gallery\\" + model!.Name);
           
            return View(model);
        }

   
        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("Admin/Gallery/Update")]
        public IActionResult UpdateAlbum(Album model, [FromForm(Name="AlbumID")]int AlbumID, [FromForm(Name="coverPhoto")]IFormFile file)
        {
            var album = _db.Albums.Where(a => a.Id == AlbumID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                album!.Name = model.Name;
                album.Location = model.Location;
                album.Description = model.Description;
                album.CoverPhoto = file.FileName;
                _db.Update(album);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("~/Areas/Admin/Views/Gallery/Update.cshtml", model);
        }

        public IActionResult Details(int id)
        {
            var model = _db.Albums.Where(a => a.Id == id).FirstOrDefault();
            return View(model);
        }

        [Microsoft.AspNetCore.Mvc.Route("Admin/Gallery/Delete/{id?}")]
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

        [HttpPost, HttpGet]
        public async Task<IActionResult> HgoApi(string id, string command, string parameters, IFormFile file)
        {
            return await _processor.ProcessCommandAsync(id, command, parameters, file);
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
        public bool CheckDb([FromForm(Name="name")]string name)
        {
            var photo = _db.Albums.Where(a => a.Name == name).Any();
            if (photo)
                return true;
            else
                return false;

            
        }

        [HttpPost]
        public IActionResult AddMultipleToDb([FromBody]List<Album> data)
        {
            _db.Albums.AddRange(data);
            _db.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult RemoveMultipleFromDb([FromBody] List<Album> data)
        {
            _db.Albums.RemoveRange(data);
            _db.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public int GetId(string name)
        {
            var album = _db.Albums.Where(a => a.Name == name).FirstOrDefault();
            return album!.Id;
        }
    }
}
