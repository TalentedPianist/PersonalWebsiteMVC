using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PersonalWebsiteMVC.Areas.pCloud.Models;
using System.Text;
using SharpCompress;

namespace PersonalWebsiteMVC.Areas.pCloud.Controllers
{
     
     [Area("pCloud")]
     public class PhotosController : Controller
     {
          public IHttpClientFactory _httpClientFactory { get; set; }
          public IHttpContextAccessor _http { get; set; }

          public PhotosController(IHttpClientFactory httpClientFactory,  IHttpContextAccessor http)
          {
               _httpClientFactory = httpClientFactory;
               _http = http;
          }


          public async Task<IActionResult> Index(string name)
          {
               ViewBag.Name = name;

               await Task.CompletedTask;
               
               return View();
          }

     }
}
