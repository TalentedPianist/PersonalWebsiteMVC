using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Areas.Blog.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using X.PagedList.Extensions;
using DeviceDetectorNET;
using DeviceDetectorNET.Parser;
using X.PagedList;

namespace PersonalWebsiteMVC.Areas.Blog.Controllers
{
     [Area("Blog")]
     public class HomeController : Controller
     {
          public ApplicationDbContext _db { get; set; }
          public int PostID { get; set; } = 0;
          private readonly IConfiguration _configuration;
          private readonly HttpClient _httpClient;
          public HomeController(ApplicationDbContext db, IConfiguration config, HttpClient httpClient)
          {
               _db = db;
               _configuration = config;
               _httpClient = httpClient;
          }

         
          [Route("Blog")]
          public IActionResult Index([FromQuery(Name = "pageNumber")] int? page)
          {

               BlogCommentViewModel model = new BlogCommentViewModel();
               var pageNumber = page ?? 1;
               model.PagedPosts = _db.Posts.ToPagedList(pageNumber, 3);
               model.AllPosts = _db.Posts.ToList();
               return View("~/Areas/Blog/Views/Index.cshtml", model);


          }

          [Route("/Blog/{title}")]
          public IActionResult Post(string title)
          {
               var model = _db.Posts.Where(p => p.PostTitle == title).FirstOrDefault();
               return View("~/Areas/Blog/Views/Shared/SinglePost", model);
          }



     }
}
