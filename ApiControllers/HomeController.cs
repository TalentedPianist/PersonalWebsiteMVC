using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PersonalWebsiteMVC.ApiControllers
{
     [Route("api/[controller]")]
     [Route("/swagger/v1")]
     [ApiController]
     public class HomeController : ControllerBase
     {
          public IActionResult Index()
          {
               return Ok("Hello World!");
          }
     }
}
