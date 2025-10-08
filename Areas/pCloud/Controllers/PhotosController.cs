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

          [Route("/pCloud/Photos/{id}/{name}")]
          public IActionResult Index(string id, string name)
          {
               ViewBag.FolderId = id;
               ViewBag.Name = name;
               return View();
          }

     }
}
