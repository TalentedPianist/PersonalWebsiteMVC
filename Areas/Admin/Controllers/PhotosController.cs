using HGO.ASPNetCore.FileManager.CommandsProcessor;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class PhotosController : Controller
    {
        private ApplicationDbContext _db { get; set; }
        private IWebHostEnvironment Host { get; set; }
        public IHttpContextAccessor _http { get; set; }


        public PhotosController(ApplicationDbContext db, IWebHostEnvironment host, IHttpContextAccessor context)
        {
            _db = db;
            Host = host;
            _http = context;
        }

        [NonAction]
        [Route("Photos/Index")]
        [Route("Photos/Index/{id}")] // To get a route working properly without the querystring, you need to have the id parameter in the route as above.
        public IActionResult Index([FromQuery(Name = "pageNumber")] int? page)
        { // X.PagedList is now working after adding the FromQuery attribute forcing it to use the pageNumber querystring.

            int id = Convert.ToInt32(RouteData.Values["id"]);

            string path = System.IO.Path.Combine(Host.ContentRootPath, "Gallery", HttpContext.Request.Query["name"]!);

            DirectoryInfo di = new DirectoryInfo(path);

            StringBuilder sb = new StringBuilder();
            ViewBag.Photos = di.GetFiles("*.jpeg");
            ViewBag.AlbumName = HttpContext.Request.Query["name"];


            var pageNumber = page ?? 1;
            var onePageOfFiles = di.GetFiles("*.jpeg").ToPagedList(pageNumber, 10);
            ViewBag.OnePageOfFiles = onePageOfFiles;

            var albumName = _http.HttpContext!.Request.Query["name"];
            var albumId = _db.Albums.Where(a => a.Name!.Contains(albumName!)).FirstOrDefault();
            if (albumId is not null)
            {
                ViewBag.AlbumID = albumId.AlbumID;
            }
            else
            {
                ViewBag.AlbumID = 0;
            }
            return View();


        }





        [HttpPost]
        public async Task<IActionResult> AddFiles([FromForm(Name = "AddFiles")] List<IFormFile> files, [FromForm(Name = "AlbumID")] int AlbumID)
        {
            TempData["Message"] = AlbumID;
            var album = _db.Albums.Where(a => a.AlbumID == AlbumID).FirstOrDefault();
            long size = files.Sum(f => f.Length);
            StringBuilder sb = new StringBuilder();
            var filePath = System.IO.Path.Combine(Host.ContentRootPath, "Gallery");
            try
            {
                foreach (IFormFile file in files)
                {
                    using (var stream = new FileStream(System.IO.Path.Combine(filePath, file.FileName), FileMode.Create))
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



        [HttpPost]
        [Route("/Photos/AjaxDbCheck")]
        public bool AjaxDbCheck([FromForm(Name = "name")] string name)
        {
            if (_db.Photos.Where(p => p.Name == name).Any())
                return true;
            else
                return false;
        }


        [Route("/Photos/AddToDb")]
        [HttpPost]
        public IActionResult AddToDb(string album)
        {
            var pic = Request.Form["name"];
            var albumRecord = _db.Albums.Where(a => a.Name == album).FirstOrDefault();
            PersonalWebsiteMVC.Models.Photos photos = new PersonalWebsiteMVC.Models.Photos();
            photos.AlbumID = albumRecord!.AlbumID;
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
        public IActionResult AddMultipleToDb([FromBody] List<Models.Photos> data)
        {
            _db.Photos.AddRange(data);
            _db.SaveChanges();
            return Ok(data);
        }

        [Route("/Photos/RemoveMultipleFromDb")]
        [HttpPost]
        public IActionResult RemoveMultipleFromDb([FromBody] List<Models.Photos> data)
        {
            // Here we are just deleting the photos from the database and ajax success function handles response on client side to refresh page.
            _db.Photos.RemoveRange(data);
            _db.SaveChanges();
            return Ok(data);

        }


        [HttpPost]
        [Route("/Photos/GetId")]
        public IActionResult GetId(List<string> name)
        {
            try
            {

                foreach (var item in name)
                {
                    var exists = _db.Photos.Where(p => p.Name == item).FirstOrDefault();
                    return Ok(exists!.PhotoID);
                }
                return Ok(0);
            }
            catch (NullReferenceException)
            {
                return Ok(0);
            }
        }

        // Resuming normal functions for update, details and delete.
        public IActionResult Update(int id)
        {
            var model = _db.Photos.Where(p => p.AlbumID == id).FirstOrDefault();
            return View(model);
        }

        [HttpPost("Photos/UpdatePhoto")]
        public IActionResult UpdatePhoto(PersonalWebsiteMVC.Models.Photos model)
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
            var model = _db.Photos.Where(p => p.AlbumID == id).FirstOrDefault();
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var model = _db.Photos.Where(p => p.AlbumID == id).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public IActionResult DeletePhoto(PersonalWebsiteMVC.Models.Photos model)
        {
            TempData["Message"] = model.AlbumID;
            return View("~/Areas/Admin/Views/Photos/Delete.cshtml", model);
        }



        [HttpPost]
        [Route("Photos/SetCoverPic")]
        public IActionResult SetCoverPic([FromBody] Album data, string strAlbum)
        {
            var album = _db.Albums.Where(a => a.Name == strAlbum).FirstOrDefault();
            album!.CoverPhoto = data.CoverPhoto;
            _db.Albums.Update(album);
            _db.SaveChanges();
            // RedirectToAction won't work because it needs to be redirected on the client side since this is an Ajax call.
            return Ok(data);
        }

        [HttpPost]
        public IActionResult RenameFile([FromBody] RenameFile data, string album)
        {
            // Investigate syntax to rename file in specified folder from album variable
            var path = System.IO.Path.Combine(Host.ContentRootPath, "Gallery", album);
            System.IO.File.Move(System.IO.Path.Combine(path, data.strOld!), System.IO.Path.Combine(path, data.strNew!), false);

            return Ok();
        }
    }
}
