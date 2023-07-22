using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
     [Area("Admin")]
     [Authorize(Policy = "Admin")]
    public class RolesController : Controller
     {
          private RoleManager<IdentityRole> roleManager;
          private UserManager<ApplicationUser> userManager;

          public RolesController(RoleManager<IdentityRole> roleMgr, UserManager<ApplicationUser> userMgr)
          {
               roleManager = roleMgr;
               userManager = userMgr;
          }

          public ViewResult Index() => View(roleManager.Roles);

          [HttpGet]
          public IActionResult Create() => View();

          [HttpPost]
          public async Task<IActionResult> Create([Required]string name)
          {
               if (ModelState.IsValid)
               {
                    IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                    if (result.Succeeded)
                         return RedirectToAction("Index");
                    else
                         foreach (var item in result.Errors)
                              ModelState.AddModelError("", item.Description);
               }
               return View(name);
          }

          public async Task<IActionResult> Delete(string id)
          {
               IdentityRole role = await roleManager.FindByIdAsync(id);
               if (role != null)
               {
                    IdentityResult result = await roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                         return RedirectToAction("Index");
                    else
                         foreach (var item in result.Errors)
                              ModelState.AddModelError("", item.Description);

               }
               else
                    ModelState.AddModelError("", "No role found");
               return View("Index", roleManager.Roles);
          }

          [HttpPost]
          public async Task<IActionResult> CreateRole([Required]string name)
          {
               if (ModelState.IsValid)
               {
                    IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                    if (result.Succeeded)
                         return RedirectToAction("Index");
                    else
                         foreach (var item in result.Errors)
                              ModelState.AddModelError("", item.Description);
               }
               return View(name);
          }

          public async Task<IActionResult> Update(string id)
          {
               IdentityRole role = await roleManager.FindByIdAsync(id);
               List<ApplicationUser> members = new List<ApplicationUser>();
               List<ApplicationUser> nonMembers = new List<ApplicationUser>();
               foreach (ApplicationUser user in userManager.Users)
               {
                    var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                    list.Add(user);
               }
               return View(new RoleEdit
               {
                    Role = role,
                    Members = members,
                    NonMembers = nonMembers
               });
          }

          [HttpPost]
          public async Task<IActionResult> Update(RoleModification model)
          {
               IdentityResult result;
               if (ModelState.IsValid)
               {
                    foreach (string userId in model.AddIds ?? new string[] { })
                    {
                         ApplicationUser user = await userManager.FindByIdAsync(userId);
                         if (user != null)
                         {
                              result = await userManager.AddToRoleAsync(user, model.RoleName);
                              if (!result.Succeeded)
                                   foreach (var item in result.Errors)
                                        ModelState.AddModelError("", item.Description);
                         }
                    }
                    foreach (string userId in model.DeleteIds ?? new string[] { })
                    {
                         ApplicationUser user  = await userManager.FindByIdAsync(userId);
                         if (User != null)
                         {
                              result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                              if (!result.Succeeded)
                                   foreach (var item in result.Errors)
                                        ModelState.AddModelError("", item.Description);
                         }
                    }  
               }
               if (ModelState.IsValid)
                    return RedirectToAction(nameof(Index));
               else
                    return await Update(model.RoleId);
          }
    }
}