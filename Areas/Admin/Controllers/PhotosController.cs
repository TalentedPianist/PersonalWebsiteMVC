using HGO.ASPNetCore.FileManager.CommandsProcessor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using System.Text;
using System.Text.Json;
using System.Web;
using X.PagedList;
using X.PagedList.Extensions;


namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhotosController : Controller
    {
        private ApplicationDbContext _db { get; set; }
        private IWebHostEnvironment Host { get; set; }
        public IHttpContextAccessor Context { get; set; }
        private readonly IFileManagerCommandsProcessor _processor;

        public PhotosController(ApplicationDbContext db, IWebHostEnvironment host, IHttpContextAccessor context, IFileManagerCommandsProcessor processor)
        {
            _db = db;
            Host = host;
            Context = context;
            _processor = processor;
        }

        [Route("Photos/Index")]
        [Route("Photos/Index/{id}")]
        [Route("Areas/Admin/Photos/Index")]

        public IActionResult Index([FromQuery(Name = "pageNumber")] int? page)
        { // X.PagedList is now working after adding the FromQuery attribute forcing it to use the pageNumber querystring.
            try
            {
                int id = Convert.ToInt32(Context.HttpContext!.Request.Query["id"]);


                var album = _db.Albums.Where(a => a.Id == id).FirstOrDefault();
                var photos = _db.Photos.Where(p => p.AlbumID == id);
                ViewBag.AlbumName = album!.Name;
                ViewBag.AlbumID = album.Id;

                // Here we need to combine three paths in the param[] array since it is the photos view and not the album view.  
                DirectoryInfo di = new DirectoryInfo(System.IO.Path.Combine(Host.ContentRootPath, "Gallery", album.Name!));

                ViewBag.Files = di.GetFiles();

                var pageNumber = page ?? 1; // If no page was specified in the querystring, default to the first page (1)
                var onePageOfFiles = di.GetFiles().Where(f => f.Name.Contains("JPEG")).ToPagedList(pageNumber, 12);
                ViewBag.OnePageOfFiles = onePageOfFiles;

                if (di.GetFiles().Count() == 0)
                {
                    TempData["Message"] = "No files found at this time.";
                }

                return View();

            }
            catch (NullReferenceException)
            {
                // Here is where any file operations should go when the album is not in the database.
                ViewBag.AlbumName = HttpContext.Request.Query["album"];
                DirectoryInfo di = new DirectoryInfo(System.IO.Path.Combine(Host.ContentRootPath, "Gallery", HttpContext.Request.Query["album"]!));
                ViewBag.AllFiles = di.GetFiles().Where(f => f.Name.Contains("JPEG"));

                var pageNumber = page ?? 1;
                var onePageOfFiles = di.GetFiles().Where(f => f.Name.Contains("JPEG")).ToPagedList(pageNumber, 12);
                ViewBag.OnePageOfFiles = onePageOfFiles;
                TempData["Message"] = DateTime.Now;
                return View();
            }


        }





        [HttpPost]
        public async Task<IActionResult> AddFiles([FromForm(Name = "AddFiles")] List<IFormFile> files, [FromForm(Name = "AlbumID")] int AlbumID)
        {
            TempData["Message"] = AlbumID;
            var album = _db.Albums.Where(a => a.Id == AlbumID).FirstOrDefault();
            long size = files.Sum(f => f.Length);
            StringBuilder sb = new StringBuilder();
            var filePath = Path.Combine(Host.ContentRootPath, "Gallery");
            try
            {
                foreach (IFormFile file in files)
                {
                    using (var stream = new FileStream(Path.Combine(filePath, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
            }

            ViewBag.Message = sb.ToString();
            return RedirectToAction("Index", "Photos", AlbumID);
        }

        [Route("/IsInDb")]
        [HttpPost]
        public bool IsInDb(string name)
        {
            List<Photos> photos = _db.Photos.Where(p => p.Name == name).ToList();
            bool hasPic = photos.Any();
            if (hasPic)
                return true;
            return false;
        }

        [HttpPost]
        [Route("/Photos/AjaxDbCheck")]
        public bool AjaxDbCheck([FromForm(Name = "name")] string name)
        {
            if (_db.Photos.Where(p => p.Name == name).Any())
                return true;
            else
                return false;
        }

        [HttpPost]
        [Route("/Photos/GetPicId")]
        public int GetPicId(string name)
        {

            var pic = _db.Photos.Where(p => p.Name == name).FirstOrDefault();
            if (pic == null)
                return 0;
            else
                return pic.Id;
        }


        [Route("/Photos/AddToDb")]
        [HttpPost]
        public IActionResult AddToDb(string album)
        {
            var pic = Request.Form["name"];
            var albumRecord = _db.Albums.Where(a => a.Name == album).FirstOrDefault();
            Photos photos = new Photos();
            photos.AlbumID = albumRecord!.Id;
            photos.Name = pic;
            photos.ImageUrl = $"/Gallery/{album}/{pic}";
            _db.Photos.Add(photos);
            _db.SaveChanges();
            return Ok();
        }

        [Route("/Photos/DelFromDb")]
        [HttpPost]
        public IActionResult RemoveFromDb(string album)
        {
            var pic = Request.Form["name"].ToString();
            var albumRecord = _db.Albums.Where(a => a.Name == album).FirstOrDefault();
            var photo = _db.Photos.Where(p => p.Name == pic).FirstOrDefault();
            if (photo is not null)
            {
                _db.Remove(photo);
                _db.SaveChanges();
            }
            else
            {
                return Content("Photo not found");
            }
            return Ok();
        }



        [Route("/Photos/AddMultipleToDb")]
        [HttpPost]
        public IActionResult AddMultipleToDb([FromBody] List<Photos> data)
        {
            // Here we are just deleting the photos from the database and ajax success function handles response on client side to refresh page.

            _db.Photos.AddRange(data);
            _db.SaveChanges();
            return Ok();

        }

        [Route("/Photos/RemoveMultipleFromDb")]
        [HttpPost("/Photos/RemoveMultipleFromDb")]
        public IActionResult RemoveMultipleFromDb([FromBody] List<Photos> data)
        {
            // Here we are just deleting the photos from the database and ajax success function handles response on client side to refresh page.
            _db.Photos.RemoveRange(data);
            _db.SaveChanges();
            return Ok();

        }

        [HttpPost]
        public int GetId([FromForm(Name = "name")] string name)
        {

            var id = _db.Photos.Where(p => p.Name == name).FirstOrDefault()!.Id;
            return id;


        }

        // Resuming normal functions for update, details and delete.
        public IActionResult Update(int id)
        {
            var model = _db.Photos.Where(p => p.Id == id).FirstOrDefault();
            return View(model);
        }

        [HttpPost("Photos/UpdatePhoto")]
        public IActionResult UpdatePhoto(Photos model)
        {
            var photo = _db.Photos.Where(p => p.Equals(model)).FirstOrDefault();
            if (ModelState.IsValid)
            {
                photo!.Name = model.Name;
                photo.Description = model.Description;
                photo.Author = model.Author;
                photo.ImageUrl = model.ImageUrl;
                photo.AlbumID = model.AlbumID;
                _db.Photos.Update(photo);
                _db.SaveChanges();
                return RedirectToAction("Update", model);
            }

            return View("Update", model);
        }

        public IActionResult Details(int id)
        {
            var model = _db.Photos.Where(p => p.Id == id).FirstOrDefault();
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var model = _db.Photos.Where(p => p.Id == id).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public IActionResult DeletePhoto(Photos model)
        {
            TempData["Message"] = model.Id;
            return View("~/Areas/Admin/Views/Photos/Delete.cshtml", model);
        }

        //HgoAspNetCore filemanager 
        [HttpGet, HttpPost]
        public async Task<IActionResult> HgoApi(string id, string command, string parameters, IFormFile file)
        {
            return await _processor.ProcessCommandAsync(id, command, parameters, file);
        }

        [HttpPost]
        public IActionResult SetCoverPic([FromForm(Name = "name")] string name, int id)
        {
            var album = _db.Albums.Where(a => a.Id == id).FirstOrDefault();
            album!.CoverPhoto = name;
            _db.Update(album);
            _db.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult RenameFile([FromBody]RenameFile data, string album)
        {
            // Investigate syntax to rename file in specified folder from album variable
            var path = System.IO.Path.Combine(Host.ContentRootPath, "Gallery", album);
            System.IO.File.Move(System.IO.Path.Combine(path, data.strOld!), System.IO.Path.Combine(path, data.strNew!));
            return Ok();
        }
    }
}
