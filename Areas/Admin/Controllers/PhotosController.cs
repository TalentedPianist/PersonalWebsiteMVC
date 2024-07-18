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

        public IActionResult Index([FromQuery(Name = "pageNumber")]int? page)
        { // X.PagedList is now working after adding the FromQuery attribute forcing it to use the pageNumber querystring.
            try
            {
                int id = Convert.ToInt32(Context.HttpContext!.Request.Query["id"]);


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

                var pageNumber = page ?? 1; // If no page was specified in the querystring, default to the first page (1)
                var onePageOfFiles = di.GetFiles().ToPagedList(pageNumber, 10);
                ViewBag.OnePageOfFiles = onePageOfFiles;
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
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
        public bool AjaxDbCheck()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in Request.Form["name"])
            {
                if (_db.Photos.Any(p => p.Name == item))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            Console.WriteLine(sb.ToString());
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
            StringBuilder sb = new StringBuilder();
            Photos photo = new Photos();
            foreach (var item in data)
            {
                sb.Append(item.Name + "<br>");
            }
            Console.WriteLine(sb.ToString());
            TempData["Message"] = sb.ToString();
            _db.AddRange(data);
            _db.SaveChanges();
            return Json(new[] { data });
        }

        [Route("/Photos/RemoveMultipleFromDb")]
        [HttpPost]
        public IActionResult RemoveMultipleFromDb([FromBody] List<Photos> data)
        {
            _db.Photos.RemoveRange(data);
            _db.SaveChanges();
            return Json(new[] { data });
        }

        public int Comments(int id)
        {
            var comments = _db.Comments.Where(c => c.CommentID == id);
            return comments.Count();
        }
    }
}
