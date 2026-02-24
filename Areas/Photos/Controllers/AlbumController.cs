using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PersonalWebsiteMVC.Areas.Photos.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using RestSharp;
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


        [Route("Photos/{Name}/{id:int}")]
        public IActionResult Index([FromQuery(Name = "pageNumber")] int? page, string Name, int id)
        {
               try
               {
                    PhotosViewModel model = new PhotosViewModel();
                    var pageNumber = page ?? 1;
                    model.PagedPhotos = _db.Photos.Where(p => p.AlbumID == id).ToPagedList(pageNumber, 12);

                    model.Photos = _db.Photos.Where(p => p.AlbumID == id).ToList();

                    model.SingleAlbum = _db.Albums.Where(p => p.AlbumID == id).FirstOrDefault();
                    ViewBag.AlbumName = model.SingleAlbum!.Name;
                    ViewBag.AlbumID = id;

                    return View("~/Areas/Photos/Views/Album/Index.cshtml", model);
               }
               catch (NullReferenceException)
               {
                    Console.WriteLine(id);
                    return View("~/Areas/Photos/Views/Album/Index.cshtml");

               }
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

          [Route("/GetThumb/")]
          public IActionResult GetThumb(string fileid)
          {
               var bytes = GetPubLink(fileid, "600x400");
               return File(bytes, "image/jpeg");
          }

          [Route("/GetSingleThumb/")]
          public IActionResult GetSingleThumb(string fileid)
          {
               var bytes = GetPubLink(fileid, "600x400");
               return File(bytes, "image/jpeg");
          }

          byte[] GetPubLink(string fileid, string size)
          {
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("getthumb", Method.Get);
               request.AddParameter("username", "douglas@douglasmcgregor.co.uk");
               request.AddParameter("password", "Inkyfrog1");
               request.AddParameter("fileid", fileid);
               request.AddParameter("size", size);
               var response = client.ExecuteAsync(request).Result;
               Console.WriteLine(response.Content);
               return response.RawBytes!;
          }
    }
}
