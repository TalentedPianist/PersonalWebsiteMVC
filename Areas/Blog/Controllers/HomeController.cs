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
        private IConfiguration _configuration;
        public HomeController(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _configuration = config;
        }

        [Route("Areas/Blog/Views/Index")]
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
        { // https://www.codeproject.com/Articles/1108855/10-Ways-to-Bind-Multiple-Models-on-a-View-in-MVC
            BlogCommentViewModel model = new BlogCommentViewModel();
            model.Post = _db.Posts.Where(p => p.PostID == id).FirstOrDefault();
            model.Comments = _db.Comments.Where(c => c.PostID == id).ToList();
            // Just spent hours editing the wrong function.  No wonder it's always null.  What an eejit.
      
            //var pageNumber = page ?? 1;
            ViewBag.OnePageOfPosts = _db.Posts.ToPagedList(1, 1);
            // The problem was in the model.  It needs to be IPagedList instead of PagedList.  This is the sort of thing you could waste so much time on.
            model.PagedPosts = _db.Posts.ToPagedList<Posts>(1, 1);
            return View(model);
        }

      
        public IActionResult Comments()
        {
            ViewBag.strId = PostID;
            var comments = _db.Comments.Where(c => c.PostID == PostID).ToList();
            ViewBag.CommentCount = comments.Count();
            BlogCommentViewModel model = new BlogCommentViewModel();
            model.Comments = comments;
            ViewData["ReCaptchaKey"] = _configuration.GetSection("GoogleReCaptcha:key").Value;

            return PartialView("~/Areas/Blog/Views/Home/Comments.cshtml");
        }
    }
}
