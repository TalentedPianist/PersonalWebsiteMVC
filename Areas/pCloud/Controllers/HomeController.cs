using Microsoft.AspNetCore.Mvc;
using Wangkanai.Extensions;

namespace PersonalWebsiteMVC.Areas.pCloud.Controllers
{
    [Area("pCloud")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            string url = "https://my.pcloud.com/oauth2/authorize";
            string clientId = "GJR8uDME26u";
            string redirectUri = "http://localhost:5051/pCloud/";
            return Redirect($"{url}?response_type=code&client_id={clientId}&redirect_uri={redirectUri}&state=12345");
        }

        public IActionResult GetAccessToken()
        {
            if (Request.Query["code"].IsNullOrEmpty())
            {
                return View("Index");
            }
            else
            {

                TempData["Message"] = HttpContext.Request.Query["code"];
            }
            return View("Index");
        }
    }
}
