using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Areas.OneDrive.Helpers;
using Azure.Identity;
using System.Text;
using Microsoft.Graph.Models;
using System.Diagnostics;
using PersonalWebsiteMVC.Models;
using Newtonsoft.Json;
using Microsoft.Graph;



namespace PersonalWebsiteMVC.Areas.OneDrive.Controllers
{
     [Area("OneDrive")]
     public class HomeController(IConfiguration configuration, IHttpContextAccessor http, IHttpClientFactory clientFactory, GraphServiceClient graph) : Controller
     {

        
          private readonly IConfiguration configuration = configuration;
          private readonly IHttpContextAccessor http = http;
          private readonly IHttpClientFactory _clientFactory = clientFactory;


          public async Task<IActionResult> Index()
          {
               await Task.CompletedTask;
               return View();
          }

          

          [HttpPost]
          public IActionResult Login()
          {
               string clientId = configuration["AzureAD:ClientId"]!;
               string tenantId = configuration["AzureAD:TenantId"]!;

               
               string url = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/authorize?client_id={clientId}&response_type=code&redirect_uri=http://localhost:5051/OneDrive/&response_mode=query&scope=user.read&state=12345&";
               return Redirect(url);
          }
        
         

     }

    
}
