using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Areas.Photos.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using RestSharp;
using X.PagedList.Extensions;

namespace PersonalWebsiteMVC.Areas.Photos.Controllers
{
    [Area("Photos")]
    public class HomeController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("Photos")]
        public IActionResult Index([FromQuery(Name ="pageNumber")] int? page)
        {
            AlbumsViewModel model = new AlbumsViewModel();
            var pageNumber = page ?? 1;
            model.PagedAlbums = _db.Albums.ToPagedList(pageNumber, 8);
           
            return View("~/Areas/Photos/Views/Home/Index.cshtml", model);
        }

          [Route("/Home/GetThumb{fileid}")]
          public IActionResult GetThumb(string fileid)
          {
               var bytes = GetPubLink(fileid, "600x400");
               return File(bytes, "image/jpg");
          }

          byte[] GetPubLink(string fileid, string size)
          {
               var client = new RestClient("https://eapi.pcloud.com/");
               var request = new RestRequest("getthumb", Method.Get);
               //request.AddParameter("access_token", Environment.GetEnvironmentVariable("PCloudToken"));
               request.AddParameter("username", "douglas@douglasmcgregor.co.uk");
               request.AddParameter("password", "Inkyfrog1");
               request.AddParameter("fileid", fileid);
               request.AddParameter("size", size);
               //request.AddParameter("type", "jpeg");
               var response = client.ExecuteAsync(request).Result;
               if (!response.IsSuccessful)
               {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ErrorMessage);
                    Console.WriteLine(response.ErrorException);
                    Console.WriteLine(response.Content);
               }
            
               return response.RawBytes!;
          }
    }
}
