using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using PersonalWebsiteMVC.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonalWebsiteMVC.Models;
using PersonalWebsiteMVC.Helpers;
using Microsoft.Extensions.Logging;
using PersonalWebsiteMVC.Entities;
using PersonalWebsiteMVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using SolrNet;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

namespace PersonalWebsiteMVC
{
     public class Startup
     {
          public Startup(IConfiguration configuration)
          {
               Configuration = configuration;
          }

          public IConfiguration Configuration { get; }

          // This method gets called by the runtime. Use this method to add services to the container.
          public void ConfigureServices(IServiceCollection services)
          {
               services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseSqlServer(
                       Configuration.GetConnectionString("DefaultConnection")));

               services.AddIdentity<ApplicationUser, IdentityRole>()
                     .AddDefaultTokenProviders()
                     .AddDefaultUI()
                     .AddEntityFrameworkStores<ApplicationDbContext>()
                     .AddRoles<IdentityRole>();

              services.AddMicrosoftIdentityWebApiAuthentication(Configuration, "AzureADConfiguration");


               services.Configure<IdentityOptions>(options =>
               {
                    // Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings.
                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = false;
               });

               services.ConfigureApplicationCookie(options =>
               {
                    // Cookie settings
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                    options.LoginPath = "/Identity/Account/Login";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                    options.SlidingExpiration = true;
               });

               services.AddAuthorization(options =>
               {
                    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
               });

               services.AddMemoryCache();
               services.AddResponseCaching();
               services.AddSession(options => {
                    options.IdleTimeout = TimeSpan.FromMinutes(60);
               });

               services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                   .AddCookie(options =>
                   {
                        options.Cookie.Expiration = TimeSpan.FromHours(5);
                   });

               services.AddControllersWithViews().AddNewtonsoftJson();
              
               
               services.AddHttpContextAccessor();
               services.AddRazorPages().AddMvcOptions( options => { options.AllowEmptyInputInBodyModelBinding = true; });
               services.AddSingleton<IAuthHelper, AuthHelper>();
               services.AddTransient<IGraphSDKHelper, GraphSDKHelper>();
               services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
               services.AddSingleton<IEmailSender, EmailSender>();
               services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
               services.AddLogging(config =>
               {
                    config.AddDebug();
                    config.AddConsole();
               });
               services.AddHttpClient();

            services.Configure<RazorViewEngineOptions>(options =>
            {
               
            });
               services.AddSolrNet("http://localhost:8983/solr/PersonalWebsiteMVC");
             
          }

          // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
          public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
          {
               /*
               if (env.IsDevelopment())
               {
                    
                    app.UseDeveloperExceptionPage();
                    app.UseDatabaseErrorPage();
               }
               else
               {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
               }
               */
               app.UseDeveloperExceptionPage();
             
               //app.UseHttpsRedirection();
               app.UseStaticFiles();
               app.UseRouting();
               app.UseResponseCaching();
               app.UseAuthentication();
               app.UseAuthorization();
               app.UseCookiePolicy();


               app.UseEndpoints(endpoints =>
               {
                    endpoints.MapAreaControllerRoute(
                         name: "Admin",
                         pattern: "Admin/{controller=Admin}/{action=Index}/{id?}",
                         areaName: "Admin");


                    endpoints.MapControllerRoute(
                     name: "default",
                     pattern: "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapRazorPages();


               });
          }

		
	}
}
