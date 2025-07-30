
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;

namespace PersonalWebsiteMVC.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi=true)]
[Route("api/[controller]")]
public class ApiController : ControllerBase
{
    public ApplicationDbContext _db { get; set; }
    public ApiController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    [NonAction]
    public IActionResult Get()
    {
        return Ok(DateTime.Now);
    }

}