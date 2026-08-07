using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text;
using PersonalWebsiteMVC.Models;


namespace PersonalWebsiteMVC.Api
{
     [Route("api/[controller]")]
     [ApiController]
     public class RolesController : ControllerBase
     {
          private RoleManager<IdentityRole> roleManager;
          private UserManager<ApplicationUser> userManager;
          private List<string> Errors = new List<string>();

          public RolesController(RoleManager<IdentityRole> roleMgr, UserManager<ApplicationUser> usrMgr)
          {
               roleManager = roleMgr;
               userManager = usrMgr;
          }

          [HttpGet]
          public IActionResult ListRoles()
          {
               return Ok(roleManager.Roles);
          }

          [HttpPost]
          [IgnoreAntiforgeryToken]
          public async Task<IActionResult> CreateRole(string name)
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

          [HttpGet("/api/Role/Update/{id}")]
          public async Task<IActionResult> GetRole(string id)
          {
               var role = await roleManager.FindByIdAsync(id);
               List<ApplicationUser> members = new List<ApplicationUser>();
               List<ApplicationUser> nonMembers = new List<ApplicationUser>();
               foreach (ApplicationUser user in userManager.Users)
               {
                    var list = await userManager.IsInRoleAsync(user, role!.Name!) ? members : nonMembers;
                    list.Add(user);
               }
               return Ok(new RoleEdit
               {
                    Role = role!, 
                    Members = members, 
                    NonMembers = nonMembers
               });
          }


          [HttpPut("/api/Roles/Update/{id}")]
          public async Task<IActionResult> UpdateRole([FromForm(Name="addIds")]string[]? addIds, [FromForm(Name="delIds")]string[]? delIds, [FromForm(Name="roleName")]string? RoleName)
          {
               
               IdentityResult result;
               if (addIds is not null)
               {
                    foreach (string userId in addIds)
                    {
                         var user = await userManager.FindByIdAsync(userId);
                         if (user != null)
                         {

                              result = await userManager.AddToRoleAsync(user, RoleName!);
                              if (result.Succeeded)
                              {
                                   Console.WriteLine(result);
                              }
                              else
                              {
                                   foreach (IdentityError error in result.Errors)
                                   {
                                        Errors.Add(error.Description);
                                   }
                                   return Ok(Errors);
                              }

                         }
                    }
               }

               if (delIds is not null)
               {
                    foreach (string userId in delIds)
                    {
                         var user = await userManager.FindByIdAsync(userId);
                         if (user != null)
                         {
                              result = await userManager.RemoveFromRoleAsync(user, RoleName!);
                              if (result.Succeeded)
                              {
                                   Console.WriteLine(result);
                              }
                              else
                              {
                                   foreach (IdentityError error in result.Errors)
                                   {
                                        Errors.Add(error.Description);
                                   }
                                 
                              }
                         }
                    }
               }
               return Ok();
               
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

          [HttpGet("/api/RoleUsers")]
          public async Task<IActionResult> RoleUsers([FromQuery(Name="roleID")]string? roleID)
          {
               List<string> names = new List<string>();
               var role = await roleManager.FindByIdAsync(roleID!);
               if (role != null)
               {
                    foreach (var user in userManager.Users)
                    {
                         if (user != null && await userManager.IsInRoleAsync(user, role.Name!))
                              names.Add(user.UserName!);

                    }
               }
               Console.WriteLine(names);
               return Ok(names.Count == 0 ? "No Users" : string.Join(", ", names));
          }

          [HttpGet("/api/Roles/View/{id}")]
          public IActionResult ViewRole(string id)
          {
               var role = roleManager.Roles.Where(r => r.Id == id).FirstOrDefault();
               return Ok(role);
          }

     }
}
