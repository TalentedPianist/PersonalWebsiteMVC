using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using Serilog;
using Serilog.Debugging;

namespace PersonalWebsiteMVC
{
     public class Program
     {
          public static void Main(string[] args)
          {

			//CreateHostBuilder(args).Build().Run();
			var host = CreateHostBuilder(args).Build();
               using (var scope = host.Services.CreateScope())
               {
                    var serviceProvider = scope.ServiceProvider;
                    try
                    {
                         var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                         var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                         UserAndRoleDataInitializer.SeedData(userManager, roleManager);
                    }
                    catch (Exception ex)
                    {
                         Debug.WriteLine(ex.Message);
                    }
               }
               
               host.Run();

          }

          public static IHostBuilder CreateHostBuilder(string[] args) =>
              Host.CreateDefaultBuilder(args)
                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                       webBuilder.UseStartup<Startup>();
                         webBuilder.UseKestrel(options =>
                         {
                              options.Limits.MaxRequestBodySize = 102428800;
                         });
                  });
     }
}
