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
            return View("~/Views/Mobile/SinglePost.cshtml", model);
        }

        public IActionResult Comments(int id)
        {
            TempData["Message"] = DateTime.Now;
            return View("~/Views/Blog/Comments.cshtml");
        }

      
    }
}
