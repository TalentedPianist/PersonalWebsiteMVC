using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PersonalWebsiteMVC.Areas.pCloud.Models;
using System.Text;

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

               StringBuilder sb = new StringBuilder();
               foreach (var item in await GetPhotos(name))
               {
                    sb.Append(item.Name);
               }
               TempData["Message"] = sb.ToString();
               
               return View();
          }

          public async Task<List<FolderModel>> GetPhotos(string name)
          {
               string token = _http.HttpContext!.Request.Cookies["AccessToken"]!;
               string username = "douglas@douglasmcgregor.co.uk";
               string password = "Inkyfrog1";
               string url = $"https://eapi.pcloud.com/listfolder?getauth=1&username={username}&password={password}&path=/My Pictures/{name}";

               var httpClient = _httpClientFactory.CreateClient();
               HttpRequestMessage request = new(method: HttpMethod.Get, requestUri: url);
               HttpResponseMessage response = await httpClient.SendAsync(request);
               response.EnsureSuccessStatusCode();
               var result = await response.Content.ReadAsStringAsync();
               var apiResponse = JsonSerializer.Deserialize<ApiResponse>(result, new JsonSerializerOptions
               {
                    PropertyNameCaseInsensitive = true
               });
               var photos = apiResponse!.Metadata.Contents;
               return photos;
          }
     }
}
