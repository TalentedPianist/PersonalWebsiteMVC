using Microsoft.AspNetCore.Mvc;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
        [Route("Admin/Search")]
        public IActionResult Index()
        {
            return View();
        }


        [Route("Search/Create")]
        public IActionResult Create()
        {
            return View();
        }
    }
}
