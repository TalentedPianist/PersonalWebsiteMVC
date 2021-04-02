using Microsoft.AspNetCore.Identity;
using PersonalWebsiteMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsiteMVC.Data
{
     public static class UserAndRoleDataInitializer
     {
          public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
          {
               SeedRoles(roleManager);
               SeedUsers(userManager);
          }

          private static void SeedUsers(UserManager<ApplicationUser> userManager)
          {
               if (userManager.FindByEmailAsync("douglas@douglasmcgregor.co.uk").Result == null)
               {
                    ApplicationUser user = new ApplicationUser();
                    user.UserName = "douglas@douglasmcgregor.co.uk";
                    user.Email = "douglas@douglasmcgregor.co.uk";
                   
                    IdentityResult result = userManager.CreateAsync(user, "Inkyfrog1").Result;
                    if (result.Succeeded)
                    {
                         userManager.AddToRoleAsync(user, "Admin").Wait();
                    }
               }
          }

          private static void SeedRoles(RoleManager<IdentityRole> roleManager)
          {
               if (!roleManager.RoleExistsAsync("Admin").Result)
               {
                    IdentityRole role = new IdentityRole();
                    role.Name = "Admin";
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
               }

               if (!roleManager.RoleExistsAsync("Member").Result)
               {
                    IdentityRole role = new IdentityRole();
                    role.Name = "Member";
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
               }
          }
     }
}
