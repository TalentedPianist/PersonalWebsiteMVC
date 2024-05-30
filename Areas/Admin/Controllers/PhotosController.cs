using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using System.Text;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhotosController : Controller
    {
        private ApplicationDbContext _db { get; set; }
        private IWebHostEnvironment Host { get; set; }
        public IHttpContextAccessor Context { get; set; }
        public PhotosController(ApplicationDbContext db, IWebHostEnvironment host, IHttpContextAccessor context)
        {
            _db = db;
            Host = host;
            Context = context;
        }

        [Route("Photos/Index")]
        [Route("Photos/Index/{id}")]
        [Route("Areas/Admin/Photos/Index")]
        public IActionResult Index(int id)
        {
            var album = _db.Albums.Where(a => a.Id == id).FirstOrDefault();
            var photos = _db.Photos.Where(p => p.AlbumID == id);
            var filePath = Host.WebRootPath + "\\Gallery\\" + album!.Name;
            ViewBag.AlbumName = album.Name;
            ViewBag.AlbumID = album.Id;
            ViewBag.Path = filePath;

            StringBuilder sb = new StringBuilder();
            DirectoryInfo di = new DirectoryInfo(filePath);

            foreach (FileInfo file in di.GetFiles())
            {
                sb.Append(file.Name);

            }

            ViewBag.Message = sb.ToString();
            ViewBag.Files = di.GetFiles();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFiles([FromForm(Name = "AddFiles")] List<IFormFile> files, [FromForm(Name = "AlbumID")] int AlbumID)
        {
            TempData["Message"] = AlbumID;
            var album = _db.Albums.Where(a => a.Id == AlbumID).FirstOrDefault();
            long size = files.Sum(f => f.Length);
            StringBuilder sb = new StringBuilder();
            var filePath = Host.WebRootPath + "\\Gallery\\" + album!.Name;
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
            catch (DirectoryNotFoundException ex)
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
        public bool AjaxDbCheck()
        {
            string name = HttpContext.Request.Form["name"]!;
            //Console.WriteLine(name);
            var photo = _db.Photos.Where(p => p.Name == name);
            if (photo.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [Route("/Photos/AddToDb")]
        [HttpPost]
        public void AddToDb()
        {
            try
            {
                var strName = HttpContext.Request.Form["name"]!.ToString();
                Console.WriteLine(strName);
                var strAlbum = HttpContext.Request.Form["album"]!.ToString();
                var album = _db.Albums.Where(a => a.Name == strAlbum).FirstOrDefault();
                Photos photo = new Photos();
                photo.Name = strName;
                photo.AlbumID = album!.Id;
                photo.ImageUrl = $"/Gallery/{album.Name}/{strName}";
                _db.Photos.Add(photo);
                _db.SaveChanges();
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [Route("/Photos/AddMultipleToDb")]
        [HttpPost]
        public void AddMultipleToDb([FromBody]JObject json)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in json)
            {
                sb.Append(item.Key);
            }
            Console.WriteLine(sb.ToString());
           
        }
    }


}
