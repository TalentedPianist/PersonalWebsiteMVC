using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using Newtonsoft.Json.Serialization;
using Sitko.Blazor.CKEditor;
using Microsoft.Extensions.FileProviders;
using PersonalWebsiteMVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using ServiceStack.Text;
using GoogleCaptchaComponent.Configuration;
using PersonalWebsiteMVC.Components;
using GoogleCaptchaComponent;
using MudBlazor.Services;
using CurrentDevice;
using Blazored.Modal;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Wangkanai.Detection.Services;
using HotChocolate.AspNetCore;
using Scalar.AspNetCore;
using SolrNet;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Aspire.Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using PersonalWebsiteMVC.Areas.OneDrive.Helpers;
using Microsoft.Graph;
using Azure.Identity;
using SolrNet.Impl;
using PersonalWebsiteMVC.Areas.pCloud.Helpers;
using reCAPTCHA.AspNetCore;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{

     var builder = WebApplication.CreateBuilder(args);

     builder.Host.UseSerilog((context, loggerConfiguration) =>
     {
          loggerConfiguration.WriteTo.Console();
          loggerConfiguration.ReadFrom.Configuration(context.Configuration);
     });

     // Add services to the container.
     var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
     builder.Services.AddDbContext<ApplicationDbContext>(options =>
         options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));



     builder.Services.AddDatabaseDeveloperPageExceptionFilter();

     //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
     //.AddEntityFrameworkStores<ApplicationDbContext>();
     builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
         .AddEntityFrameworkStores<ApplicationDbContext>()
         .AddDefaultTokenProviders()
         .AddDefaultUI();

     builder.Services.AddControllersWithViews().AddNewtonsoftJson().AddSessionStateTempDataProvider().AddRazorRuntimeCompilation();
     builder.Services.AddRazorPages().AddNewtonsoftJson();


     builder.Services.AddMvc(options =>
     {
          options.MaxModelBindingCollectionSize = int.MaxValue;
          options.EnableEndpointRouting = false;


     }).AddRazorRuntimeCompilation();





     builder.Services.AddControllers()
         .AddNewtonsoftJson(options =>
         {
              options.SerializerSettings.ContractResolver = new DefaultContractResolver();
         })
         .AddJsonOptions(options =>
         {
              options.JsonSerializerOptions.PropertyNamingPolicy = null;
         });

     // Specifically for Microsoft Graph authentication



     builder.Services.AddAuthentication(options =>
     {
          options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
      
     })
          .AddCookie(options =>
          {
               options.ExpireTimeSpan = TimeSpan.FromHours(5);
          });



     builder.Services.Configure<IdentityOptions>(options =>
     {
          // Password settings.
          options.Password.RequireDigit = true;
          options.Password.RequireLowercase = false;
          options.Password.RequireNonAlphanumeric = false;
          options.Password.RequireUppercase = true;
          options.Password.RequiredLength = 6;
          options.Password.RequiredUniqueChars = 0;

          // Lockout settings.
          //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
          //options.Lockout.MaxFailedAccessAttempts = 5;
          options.Lockout.AllowedForNewUsers = true;

          // User settings
          options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
          options.User.RequireUniqueEmail = false;

          // Sign in settings
          options.SignIn.RequireConfirmedEmail = false;
          options.SignIn.RequireConfirmedAccount = false; // Needed to get passed the "NotAllowed" error
          options.SignIn.RequireConfirmedPhoneNumber = false;

     });

     builder.Services.ConfigureApplicationCookie(options =>
     {
          // Cookie settings
          options.Cookie.Name = ".AspNetCore.Identity.Application";
          options.Cookie.HttpOnly = true;
          options.ExpireTimeSpan = TimeSpan.FromDays(1);


          options.LoginPath = "/Identity/Account/Login";
          options.LogoutPath = "/Identity/Account/Logout";
          options.AccessDeniedPath = "/Identity/Account/AccessDenied";
          options.SlidingExpiration = true;
     });

     builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });

     builder.Services.AddRazorComponents().AddInteractiveServerComponents();

     builder.Services.AddCKEditor(builder.Configuration, options =>
     {
          options.EditorClassName = "ClassicEditor";
          options.ScriptPath = "https://cdn.ckeditor.com/ckeditor5/28.0.0/classic/ckeditor.js";
          //options.StylePath = "https://cdn.ckeditor.com/ckeditor5/42.0.0/ckeditor5.css";


     });

     builder.Services.AddSession(options =>
     {
          options.IdleTimeout = TimeSpan.FromHours(2);
          options.Cookie.HttpOnly = true;
          options.Cookie.IsEssential = true;
     });

     builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

     builder.Services.AddCascadingAuthenticationState();

     
     builder.Services.AddAuthorization(options =>
     {
          options.AddPolicy("Admin",
             authBuilder =>
             {
                  authBuilder.RequireRole("Admin");
             });
     });

     builder.Services.AddHttpContextAccessor();

     builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
     builder.Services.AddSingleton<EmailService>();

     builder.Services.Configure<RouteOptions>(options =>
     {
          options.AppendTrailingSlash = true;
     });

     builder.Services.AddResponsive();


     builder.Services.AddCors(options =>
     {
          options.AddDefaultPolicy(
             policy =>
             {
                  policy.AllowAnyOrigin()
                 .AllowAnyHeader()
                 .AllowAnyMethod();
             });
     });

     builder.Services.AddHttpClient();

     builder.Services.AddEndpointsApiExplorer();

     builder.Services.AddRazorPages();


     builder.Services.AddCKEditor(builder.Configuration, options =>
     {
     });

     builder.Services.AddHttpContextAccessor();


     // builder.Services.AddHttpClient<IReCaptchaFormClient, ReCaptchaFormHttpClient>(client =>
     //     client.BaseAddress = new Uri("http://localhost:5051"));

     

     builder.Services.AddBlazorBootstrap();
     builder.Services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

     builder.Services.AddDetection();

     builder.Services.AddSwaggerGen();

     

     builder.Services.AddSingleton<OneDriveAuthHelper>();

     var emailConfig = builder.Configuration
         .GetSection("MailSettings")
         .Get<MailSettings>();



     builder.Services.AddSolrNet<SearchModel>("http://localhost:8983/solr/blog");
     // https://devapo.io/blog/technology/leverage-the-power-of-indexing-with-apache-solr-and-net/
     builder.Services.AddScoped<ISolrStatusResponseParser, SolrStatusResponseParser>();
     builder.Services.AddScoped<ISolrCoreAdmin, SolrCoreAdmin>();

     builder.Services.AddScoped<IPCloudAuth, PCloudAuth>();

     builder.Services.AddValidation();

     // https://github.com/TimothyMeadows/reCAPTCHA.AspNetCore
     builder.Services.AddRecaptcha(builder.Configuration.GetSection("RecaptchaSettings"));
     builder.Services.AddRecaptcha(options =>
     {
          options.SecretKey = "6Ld1l2osAAAAAK-VlYhJAohCH60eD_EqSGLsujP_";
          options.SiteKey = "6LcGnnUsAAAAAKS1RqaVpuDdYQQb7-pyvInVTjtP";
     });

     
     var app = builder.Build();

     // Code to seed data as seen in https://learn.microsoft.com/en-us/aspnet/core/blazor/tutorials/movie-database-app/part-4?view=aspnetcore-9.0&pivots=vs#seed-the-database
     using (var scope = app.Services.CreateScope())
     {
          var services = scope.ServiceProvider;
          //SeedData.Initialize(services);
     }


     app.MapStaticAssets();


     // Configure the HTTP request pipeline.
     if (app.Environment.IsDevelopment())
     {
          app.UseMigrationsEndPoint();

     }
     else
     {
          app.UseExceptionHandler("/Home/Error");
     }


     app.UseDetection();

     if (app.Environment.IsProduction())
     {
          app.UseHttpsRedirection();
     }
     app.UseStaticFiles();
    
   

     app.UseRouting();
     app.UseAuthentication();
     app.UseAuthorization();
     app.UseSession();
     app.UseAntiforgery();

     app.MapRazorPages();

     app.MapAreaControllerRoute(
         name: "Admin",
         areaName: "Admin",
         pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

     
     //app.MapControllerRoute(
     //     name: "areas",
     //     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

     app.MapControllerRoute(
          name: "blog-root",
          pattern: "Blog",
          defaults: new { area = "Blog", action = "Index" });

     app.MapControllerRoute(
          name: "Post",
          pattern: "Title",
          defaults: new { area = "Blog", action="Post" });

     app.MapAreaControllerRoute(
          name: "OneDrive",
          areaName: "OneDrive",
          pattern: "OneDrive/{controller=Home}/{action=Index}/{id?}");

    app.MapAreaControllerRoute(
        name: "pCloud",
        areaName: "pCloud",
        pattern: "pCloud/{controller=Home}/{action=Index}/{id?}");

     app.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}");



     app.MapGet("/Captcha", (string userResponse) =>
    {
        return userResponse;

    });

    app.MapGet("antiforgery/token", (IAntiforgery forgeryService, HttpContext context) =>
    {
        var tokens = forgeryService.GetAndStoreTokens(context);
        context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!,
            new CookieOptions { HttpOnly = false });

        return tokens;
    });


    // app.MapRazorComponents<App>()
    //     .AddInteractiveServerRenderMode();
    //app.MapFallbackToPage("/");

    app.MapOpenApi();
    app.MapScalarApiReference();

    app.MapPostsEndpoints();

     app.UseDeveloperExceptionPage();

     using (var scope = app.Services.CreateScope())
     {
          var services = scope.ServiceProvider;
          var dbContext = services.GetRequiredService<ApplicationDbContext>();
          // Apply pending migrations
          await dbContext.Database.MigrateAsync();
          // Seed roles and admin user
          await IdentitySeeder.SeedRolesAndAdminAsync(services);



     }


     app.Run();

        

} // https://stackoverflow.com/questions/70247187/microsoft-extensions-hosting-hostfactoryresolverhostinglistenerstopthehostexce
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "server terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
