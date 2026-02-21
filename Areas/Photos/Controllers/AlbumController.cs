using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalWebsiteMVC.Areas.Photos.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using SharpCompress;
using System.Text;
using X.PagedList.Extensions;

namespace PersonalWebsiteMVC.Areas.Photos.Controllers
{
    [Area("Photos")]
    public class AlbumController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        public IHttpContextAccessor _http { get; set; }

        public AlbumController(ApplicationDbContext db, IHttpContextAccessor http)
        {
            _db = db;
            _http = http;
        }


        [Microsoft.AspNetCore.Mvc.Route("Photos/{Name}")]
        public IActionResult Index([FromQuery(Name = "pageNumber")] int? page, string Name, [FromQuery(Name="id")]int id)
        {
            
            PhotosViewModel model = new PhotosViewModel();
            var pageNumber = page ?? 1;
            model.PagedPhotos = _db.Photos.Where(p => p.AlbumID == id).ToPagedList(pageNumber, 12);
            model.Photos = _db.Photos.Where(p => p.AlbumID == id).ToList();
               model.SingleAlbum = _db.Albums.Where(p => p.AlbumID == id).FirstOrDefault();
               ViewBag.AlbumName = model.SingleAlbum!.Name;
               

            return View("~/Areas/Photos/Views/Album/Index.cshtml", model);
        }

        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("Photos/Album/GetPhotos")]
        public IActionResult GetPhotos(int id)
        {
            var photo = _db.Photos.Where(p => p.AlbumID == id).FirstOrDefault();
            var comments = _db.Comments.Where(c => Convert.ToInt32(c.PhotoID) == id).ToList();
            ViewBag.Comments = comments;

            return Ok(photo);
        }

        [HttpPost]
        [Route("/Album/GetPhoto")]
        public IActionResult GetPhoto(string name, int albumID)
        {
            var photo = _db.Photos.Where(p => p.Name == name).FirstOrDefault();
            var model = new PhotosViewModel();
            model.SinglePhoto = _db.Photos.Where(p => p.Name == name).FirstOrDefault();
            model.SingleAlbum = _db.Albums.Where(a => a.AlbumID == albumID).FirstOrDefault();
            model.Comments = new Comments();

               model.AllComments = _db.Comments.Where(c => Convert.ToInt32(c.PhotoID) == model.SinglePhoto!.PhotoID).ToList();

               model.PagedComments = _db.Comments.Where(c => Convert.ToInt32(c.PhotoID) == model.SinglePhoto!.PhotoID).OrderByDescending(c => c.CommentDate).ToPagedList<Comments>(1, 3);

            return PartialView("~/Areas/Photos/Views/Album/Photo.cshtml", model);

        }


       

        public async Task<bool> VerifyCaptcha(string captchaResponse)
        {
            var secretKey = "6LeCBlUrAAAAACVipFQ2hXQkaRn1i_pFJEZIegge";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={captchaResponse}");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var captchaResult = JsonConvert.DeserializeObject<CaptchaResponse>(jsonResponse);
                return captchaResult!.Success;
            }
            return false;
        }

    }
}
