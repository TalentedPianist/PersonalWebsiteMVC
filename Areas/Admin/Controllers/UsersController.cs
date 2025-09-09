using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteBlazor.Models;
using PersonalWebsiteMVC.Models;
using X.PagedList;

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
     [Authorize(Roles="Admin")]
     [Area("Admin")]
    public class UsersController : Controller
    {
          private UserManager<ApplicationUser> userManager;
          private IPasswordHasher<ApplicationUser> passwordHasher;

          public UsersController(UserManager<ApplicationUser> usrMgr, IPasswordHasher<ApplicationUser> passwordHash)
          {
               userManager = usrMgr;
               passwordHasher = passwordHash;
          }

       [Route("Admin/Users")]
        public IActionResult Index([FromQuery(Name = "pageNumber")]int? page)
        {
            IEnumerable<ApplicationUser> users = userManager.Users;
            int pageNumber = 1;
            int pageSize = 1;
            IPagedList<ApplicationUser> pagedList = new PagedList<ApplicationUser>(users, pageNumber, pageSize);
            return View(pagedList);
        }

          public ViewResult Create() => View();

          [HttpPost]
          public async Task<IActionResult> Create(User user)
          {
               if (ModelState.IsValid)
               {
                ApplicationUser appUser = new ApplicationUser
                {
                   

                    UserName = user.UserName,
                    Email = user.Email,
                };
                IdentityResult result = await userManager.CreateAsync(appUser, user.Password!);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
               }
               return View(user);
          }

          public async Task<IActionResult> Update(string id)
          {
               ApplicationUser? user = await userManager.FindByIdAsync(id);
               if (user != null)
                    return View(user);
               else
                    return RedirectToAction("Index");
          }

          [HttpPost]
          public async Task<IActionResult> Update(string id, ApplicationUser model, string password)
          {

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
               ApplicationUser user = await userManager.FindByIdAsync(model.Id);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

               if (user != null)
               {
                    if (!string.IsNullOrEmpty(model.FirstName))
                         user.FirstName = model.FirstName;
                    else
                         ModelState.AddModelError("", "First name cannot be empty.");

                    if (!string.IsNullOrEmpty(model.LastName))
                         user.LastName = model.LastName;
                    else
                         ModelState.AddModelError("", "Last name cannot be empty.");

                    if (!string.IsNullOrEmpty(model.Email))
                         user.Email = model.Email;
                   

                    if (!string.IsNullOrEmpty(password))
                         user.PasswordHash = passwordHasher.HashPassword(user, password);
                    

                    if (!string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(password))
                    {
                         IdentityResult result = await userManager.UpdateAsync(user);
                         if (result.Succeeded)
                         {
                              return RedirectToAction("Index");
                         }
                         else
                         {
                              foreach (IdentityError error in result.Errors)
                              {
                                   ModelState.AddModelError("", error.Description);
                              }
                          
                         }
                    }
               }
               return View(user);
          }

          public async Task<IActionResult> Details(string id)
          {
               ApplicationUser? user = await userManager.FindByIdAsync(id);
               if (user is not null)
               {
                    return View(user);
               }
               return View();
          }

          public async Task<IActionResult> Delete(string id)
          {
               ApplicationUser? user = await userManager.FindByIdAsync(id);
               if (user != null)
               {
                    IdentityResult result = await userManager.DeleteAsync(user);
                    if (result.Succeeded)
                         return RedirectToAction("Index");
                    else
                         foreach (var item in result.Errors)
                              ModelState.AddModelError("", item.Description);
               }
               else
                    ModelState.AddModelError("", "User Not Found");
               return View("Index", userManager.Users);
          }
    }
}