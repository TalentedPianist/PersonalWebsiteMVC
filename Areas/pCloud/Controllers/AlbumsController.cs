using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Areas.pCloud.Models;
using System.Text.Json;

namespace PersonalWebsiteMVC.Areas.pCloud.Controllers;

[Area("pCloud")]
public class AlbumsController : Controller
{
    private IHttpClientFactory _httpClientFactory { get; set; }

    public AlbumsController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        string username = "douglas@douglasmcgregor.co.uk";
        string password = "Inkyfrog1";
        string albumsPath = "/My Pictures";
        string url = $"https://eapi.pcloud.com/listfolder?getauth=1&username={username}&password={password}&path={albumsPath}";

          // var httpClient = _httpClientFactory.CreateClient();
          // HttpRequestMessage request = new(method: HttpMethod.Get, requestUri: url);
          // HttpResponseMessage response = await httpClient.SendAsync(request);
          // response.EnsureSuccessStatusCode();
          // var result = await response.Content.ReadAsStringAsync();
          // var model = System.Text.Json.JsonSerializer.Deserialize<List<FolderModel>>(result);
          await Task.CompletedTask;
        return View();
    }


}