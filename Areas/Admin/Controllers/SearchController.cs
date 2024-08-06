using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchController : Controller
    {
      
        public IWebHostEnvironment _env { get; set; } 

        public SearchController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [Microsoft.AspNetCore.Mvc.Route("Admin/Search")]
        public IActionResult Index()
        {
            try
            {
                var host = HttpContext.Request.GetTypedHeaders().Referer!.Host;
                string[] urls = new string[5];
                string path = System.IO.Path.Combine(_env.ContentRootPath, "Components");
                DirectoryInfo dirs = new DirectoryInfo(path);
                var files = dirs.GetFiles();
                foreach (FileInfo file in files)
                {
                    if (file.Name.Contains("Index"))
                    {
                        urls[0] = file.Name.ToString().Replace(".razor", "");
                    }
                    if (file.Name.Contains("About"))
                    {
                        urls[1] = file.Name.ToString().Replace(".razor", "");
                    }
                    if (file.Name.Contains("Blog"))
                    {
                        urls[2] = file.Name.ToString().Replace(".razor", "");
                    }
                    if (file.Name.Contains("Contact"))
                    {
                        urls[3] = file.Name.ToString().Replace(".razor", "");
                    }
                }
                ViewBag.Files = urls;
                if (host == "localhost")
                {
                    ViewBag.Url = "http://localhost:5051";
                }
            }
            catch (NullReferenceException ex)
            {
                TempData["Message"] = ex.Message;
            }
            return View();
        }


        [Microsoft.AspNetCore.Mvc.Route("Search/Create")]
        public IActionResult Create()
        {
            try
            {
                var host = HttpContext.Request.GetTypedHeaders().Referer!.Host;
                string[] urls = new string[5];
                string path = System.IO.Path.Combine(_env.ContentRootPath, "Components");
                DirectoryInfo dirs = new DirectoryInfo(path);
                var files = dirs.GetFiles();
                foreach (FileInfo file in files)
                {
                    if (file.Name.Contains("Index"))
                    {
                        urls[0] = file.Name.ToString().Replace(".razor", "");
                    }
                    if (file.Name.Contains("About"))
                    {
                        urls[1] = file.Name.ToString().Replace(".razor", "");
                    }
                    if (file.Name.Contains("Blog"))
                    {
                        urls[2] = file.Name.ToString().Replace(".razor", "");
                    }
                    if (file.Name.Contains("Contact"))
                    {
                        urls[3] = file.Name.ToString().Replace(".razor", "");
                    }
                }
                ViewBag.Files = urls;
                if (host == "localhost")
                {
                    ViewBag.Url = "http://localhost:5051";
                }
            }
            catch (NullReferenceException ex)
            {
                TempData["Message"] = ex.Message;
            }
            return View();
        }
    }
}
