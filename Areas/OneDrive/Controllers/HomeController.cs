using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using PersonalWebsiteMVC.Areas.OneDrive.Helpers;
using Azure.Identity;
using System.Text;
using Microsoft.Graph.Models;


namespace PersonalWebsiteMVC.Areas.OneDrive.Controllers
{
     [Area("OneDrive")]
     public class HomeController : Controller
     {

          private readonly GraphServiceClient _graphClient;

          public HomeController(GraphServiceClient graphClient)
          {
               _graphClient = graphClient;
          }


          public async Task<IActionResult> Index()
          {
               var item = await _graphClient.Users["douglas@douglasmcgregor.co.uk"].Drive.GetAsync();
               TempData["Message"] = item!.Root;
               return View();
          }


        
         

     }

    
}
