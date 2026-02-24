using Microsoft.AspNetCore.Mvc;

namespace PersonalWebsiteMVC.Components
{
     public class BlogLayout : ViewComponent
     {
          public IViewComponentResult Invoke()
          {
               return View("~/Components/BlogLayout/Default.cshtml");
          }


     }
}
