using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

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
               TempData["Name"] = name;

               string token = _http.HttpContext!.Request.Cookies["AccessToken"]!;
               string username = "douglas@douglasmcgregor.co.uk";
               string password = "Inkyfrog1";
               string url = $"https://eapi.pcloud.com/listfolder?auth={token}";

               var httpClient = _httpClientFactory.CreateClient();
               HttpRequestMessage request = new(method: HttpMethod.Get, requestUri: url);
               HttpResponseMessage response = await httpClient.SendAsync(request);
               response.EnsureSuccessStatusCode();
               var result = await response.Content.ReadAsStringAsync();
               //var apiResponse = JsonSerializer.Deserialize<ApiResponse>(result, new JsonSerializerOptions
               //{
               //     PropertyNameCaseInsensitive = true
               //});
               //var photos = apiResponse!.Metadata.Contents;

               TempData["Message"] = result;
               return View();
          }
     }
}
