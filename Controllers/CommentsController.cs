using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Controllers
{
    public class CommentsController : Controller
    {
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
    }
}
