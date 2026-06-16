using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text;

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

          [HttpGet]
          public IActionResult ListRoles()
          {
               return Ok(roleManager.Roles);
          }

          [HttpPost]
          [IgnoreAntiforgeryToken]
          public async Task<IActionResult> CreateRole([Required][FromForm(Name="name")] string name)
          {
               StringBuilder sb = new StringBuilder();
               if (ModelState.IsValid)
               {
                    IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                    if (result.Succeeded)
                         return Ok("Role created successfully");

                    return Ok();
               } else {
                    return Problem("Please enter a role name.");
               }
               

          }

          [HttpGet("{id}")]
          public async Task<IActionResult> GetRole(string id)
          {
               return Ok(await roleManager.FindByIdAsync(id));
          }


          [HttpPut("{id}")]
          public async Task<IActionResult> UpdateRole(string id, [FromForm(Name="name")]string name)
          {
               if (string.IsNullOrWhiteSpace(name))
               {
                    return Problem("Please provide the role name.");
               }
               else
               {
                    var role = await roleManager.FindByIdAsync(id);
                    if (role is not null)
                    {
                         role.Name = name;
                         await roleManager.UpdateAsync(role);
                         return Ok($"Role {name} updated successfully");
                    }
                    return NotFound(new { Message = "There was a problem" });
               }
          }

          [HttpDelete("{id}")]
          public async Task<IActionResult> DeleteRole(string id)
          {
              
                var role = await roleManager.FindByIdAsync(id);
               if (role is not null)
               {
                    await roleManager.DeleteAsync(role);
                    return Ok($"Role {role.Name} successfully deleted");
               }
               return Ok();
          }

     }
}
