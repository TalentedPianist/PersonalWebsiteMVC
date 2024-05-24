using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
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

        public List<Comments> GetComments(int id)
        {
            
        }

      
    }

    
}
