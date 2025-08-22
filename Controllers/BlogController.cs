using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PersonalWebsiteMVC.Areas.Blog.Models;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Controllers
{

    public class BlogController : Controller
    {
        public ApplicationDbContext _db { get; set; }
        public BlogController(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            // For the mobile view, the HTML needs to be hard coded in the partial view rather than the traditional MVC approach.  After playing around with ViewComponents and discovering that POST is disabled it makes sense.

            if (HttpContext.Request.Query["id"].ToString() != null)
            {
                int pageSize = 3;
                int currentPage = 1;
                List<Posts> pagedPosts = _db.Posts
                    .Skip((currentPage - pageSize) * pageSize)
                    .Take(pageSize)
                    .ToList();
                ViewBag.MyPosts = pagedPosts;

                return View("~/Views/Shared/Blog/SinglePost.cshtml");
            }
            else
            {
                return View();
            }
        }


        // https://dotnettutorials.net/lesson/partial-view-result-in-asp-net-core-mvc/?utm_content=cmp-true
        // Also in dotnet 8, asp-action won't seem to work without the Route attribute. 
        [HttpGet]
        [Route("/Blog/{title}")]
        public IActionResult SinglePost(string title)
        {
            

            MixModel model = new MixModel();
            model.Posts = _db.Posts.Where(p => p.PostTitle == title).FirstOrDefault();
            model.Comments = new Comments();
            model.AllComments = _db.Comments.Where(p => p.PostID == model.Posts!.PostID).ToList();

            // Paged comments
            int pageSize = 4;
            int currentPage = 1;
            List<Comments> pagedComments = _db.Comments
                .Where(c => c.PostID == model.Posts!.PostID)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            ViewBag.MyComments = pagedComments;
            TempData["PostID"] = model.Posts!.PostID;
            TempData["PostTitle"] = title;
            ViewData["PostID"] = model.Posts!.PostID;
            
            return View("~/Views/Mobile/SinglePost.cshtml", model);
        }

        public IActionResult Comments(int id)
        {
            TempData["Message"] = DateTime.Now;
            return View("~/Views/Blog/Comments.cshtml");
        }


        [Route("/Blog/LoadMore")]
        public IActionResult LoadMore(int skip)
        {
            Console.WriteLine(TempData["PostTitle"]);

            var posts = _db.Posts.Skip(skip).Take(3).OrderByDescending(p => p.PostTitle);
            ViewBag.MyPosts = posts;

            return PartialView("~/Views/Mobile/PostCard.cshtml");
            
        }

        [Route("Blog/LoadLess")]
        public IActionResult LoadLess(int skip)
        {
            var post = _db.Posts
                .OrderByDescending(p => p.PostID)
                .Skip(skip)
                .Take(1)
                .ToList();

            
            return PartialView("~/Views/Mobile/PostCard.cshtml");
        }
    }
}
