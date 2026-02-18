using Microsoft.AspNetCore.Mvc;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
     public class PhotosController : Controller
     {
          public IActionResult Index()
          {
               return View();
          }
     }
}
