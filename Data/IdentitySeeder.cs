using Microsoft.AspNetCore.Identity;
using PersonalWebsiteMVC.Models;
using ServiceStack;

namespace PersonalWebsiteMVC.Data
{
    public static class IdentitySeeder
    {
          public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
          {
               var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
               var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

               // Define roles to seed
               var roles = new[] { "Admin", "Moderator", "Member" };

               // Seed roles
               foreach (var role in roles)
               {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                         await roleManager.CreateAsync(new IdentityRole(role));
                    }
               }

               // Define the admin user details
               var adminEmail = "douglas@douglasmcgregor.co.uk";
               var adminPassword = "Inkyfrog1";

               // Check if the admin user already exists
               var userExist = await userManager.FindByEmailAsync(adminEmail);
               if (userExist == null)
               {
                    var adminUser = new ApplicationUser { UserName = "admin", Email = adminEmail, FirstName = "Douglas", LastName = "McGregor", PhoneNumber = "07722957292", EmailConfirmed = true };


                    // Create the admin user
                    var result = await userManager.CreateAsync(adminUser, adminPassword);
                    if (result.Succeeded)
                    {
                         // Assign the Admin role to the user
                         await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                    else
                    {
                         throw new Exception("Failed to create the admin user:" + string.Join(", ", result.Errors));
                    }

               };
          }
    }
}
