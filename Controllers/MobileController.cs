
using Microsoft.AspNetCore.Mvc;

namespace PersonalWebsiteMVC.Controllers;

[Controller]
public class MobileController : Controller
{
    public IActionResult ReadMore()
    {
        TempData["Message"] = DateTime.Now;
        return View();
    }

}