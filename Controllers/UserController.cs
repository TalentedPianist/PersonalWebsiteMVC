using Microsoft.AspNetCore.Mvc;

namespace PersonalWebsiteMVC.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
