using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace PersonalWebsiteMVC.Api
{
     [Route("api/[controller]")]
     [ApiController]
     public class RolesController : ControllerBase
     {
          private RoleManager<IdentityRole> roleManager;

          public RolesController(RoleManager<IdentityRole> roleMgr)
          {
               roleManager = roleMgr;
          }

          [HttpGet("/roles")]
          public IActionResult ListRoles()
          {
               return Ok(roleManager.Roles);
          }

     }
}
