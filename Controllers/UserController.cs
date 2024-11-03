using Elastic.Clients.Elasticsearch.Snapshot;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Controllers
{
    public class UserController : Controller
    {
        public readonly IConfiguration _configuration;
        public readonly HttpClient _httpClient;

        public UserController(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}
