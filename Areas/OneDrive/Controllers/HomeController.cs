using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using PersonalWebsiteMVC.Areas.OneDrive.Helpers;
using Azure.Identity;


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
               try
               {
                    var me = await _graphClient.Users["douglas@douglasmcgregor.co.uk"].GetAsync();
                    TempData["Message"] = me!.GivenName;
                    await Task.CompletedTask;
                    return View();
               }
               catch (Microsoft.Graph.Models.ODataErrors.ODataError e)
               {
                    TempData["Message"] = e.Message;
                    return View();
               }
          }


        
         

     }

    
}
