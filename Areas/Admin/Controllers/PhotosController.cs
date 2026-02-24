using Microsoft.AspNetCore.Mvc;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
     [Area("Admin")]
     public class PhotosController : Controller
     {
          [Route("/Admin/Photos")]
          public IActionResult Index()
          {
               return View();
          }
     }
}
