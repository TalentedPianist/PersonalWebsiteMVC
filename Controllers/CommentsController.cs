using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Controllers
{
    public class CommentsController : Controller
    {
 
        public ApplicationDbContext _db { get; set; }
        public CommentsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddComment()
        {
            TempData["Message"] = DateTime.Now;
            return View("~/Views/Shared/Components/Mobile/Comments.cshtml");
        }

        [HttpGet]
        [Route("/Comments/GetComments")]
        public IActionResult GetComments()
        {
            return Ok(DateTime.Now);
        }

        [HttpGet]
        [Route("/Comments/LoadMore")]
        public IActionResult LoadMore(int skip)
        {
            Console.WriteLine(TempData["PostID"]);
            var comments = _db.Comments
                .Where(c => c.PostID == Convert.ToInt32(TempData["PostID"]))       
                .Skip(skip)
                .Take(1)
                .OrderByDescending(c => c.CommentDate)
                .ToList();
            ViewBag.MyComments = comments;
            return PartialView("~/Views/Mobile/CommentCard.cshtml");
        }

        [Route("/Comments/LoadLess")]
        public IActionResult LoadLess(int skip)
        {
            var comments = _db.Comments
                .OrderByDescending(c => c.CommentDate)
                .Skip(skip)
                .Take(1)
                .ToList();
            return PartialView("~/Views/Mobile/CommentCard.cshtml");
        }
    }
}
