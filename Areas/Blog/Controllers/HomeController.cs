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

        [Route("Areas/Blog/Views/Index")]
        [Route("Blog")]
        public IActionResult Index([FromQuery(Name="pageNumber")]int? page)
        {
            // Begin code for device detector
            DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);

            var userAgent = Request.Headers["User-Agent"];
            var headers = Request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
            var clientHints = ClientHints.Factory(headers);

            var dd = new DeviceDetector(userAgent, clientHints);
            dd.Parse();

            if (dd.IsBot()) { 
                return Content("Go away bot!");
            } else { 
                var clientInfo = dd.GetClient();
                var osInfo = dd.GetOs();
                var device = dd.GetDeviceName();
                var brand = dd.GetBrandName();

                BlogCommentViewModel model = new BlogCommentViewModel();
                var pageNumber = page ?? 1;
                model.PagedPosts = _db.Posts.ToPagedList(pageNumber, 3);
                return View("~/Areas/Blog/Views/Index.cshtml", model);
            }
            // End code for device detector

            // Spending ages editing the wrong function and wondering why nothing's happening is something I will need to watch.
            


        }

        [Route("Blog/SinglePost/{id}")]
        public IActionResult SinglePost(int? id, [FromQuery(Name="pageNumber")]int? page)
        {
            BlogCommentViewModel model = new BlogCommentViewModel();
            model.Post = _db.Posts.Where(p => p.PostID == id).FirstOrDefault();
            var pageNumber = page ?? 1;
            var strId = Convert.ToInt32(id);
            model.PagedComments = (PagedList<Comments>?)_db.Comments.Where(c => c.CommentID == strId).ToPagedList<Comments>(pageNumber, 3);
            return View("~/Areas/Blog/Views/Shared/SinglePost.cshtml", model);
        }

     

    }
}
