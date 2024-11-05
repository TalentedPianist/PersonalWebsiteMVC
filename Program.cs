using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalWebsiteMVC;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using PersonalWebsiteMVC.Components;
using PersonalWebsiteMVC.Components.Layout;
using SolrNet;
using Sitko.Blazor.CKEditor;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.UI.Services;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Microsoft.AspNetCore.Mvc.Razor;
using Wangkanai.Responsive;
using Serilog;



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

    /*var connectionString = builder.Configuration.GetConnectionString("SQLite") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");*/
    /*builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));*/
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

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);


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
        //options.Cookie.Name = ".AspNetCore.Identity.Application";
        options.Cookie.HttpOnly = true;
        //options.ExpireTimeSpan = TimeSpan.FromMinutes(20);


        options.LoginPath = "/Identity/Account/Login";
        options.LogoutPath = "/Identity/Account/Logout";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        options.SlidingExpiration = true;
    });

    builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
    builder.Services.AddMudServices();
    builder.Services.AddRazorComponents().AddInteractiveServerComponents();

    builder.Services.AddCKEditor(builder.Configuration, options =>
    {
        options.EditorClassName = "ClassicEditor";
        options.ScriptPath = "https://cdn.ckeditor.com/ckeditor5/28.0.0/classic/ckeditor.js";
        options.StylePath = "https://cdn.ckeditor.com/ckeditor5/42.0.0/ckeditor5.css";


    });




    builder.Services.AddSession();



    builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

    builder.Services.AddCascadingAuthenticationState();

    builder.Services.AddAuthentication(); // This is needed for logout to work otherwise nothing happens.
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
    builder.Services.AddTransient<IMailService, MailService>();
    builder.Services.AddTransient<IEmailSender, EmailSender>();

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
    builder.Services.AddSwaggerGen();


    var emailConfig = builder.Configuration
        .GetSection("MailSettings")
        .Get<MailSettings>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
    }
    app.UseStaticFiles();

    var fileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Gallery"));
    var options = new FileServerOptions
    {
        FileProvider = fileProvider,
        RequestPath = "/Gallery",
        EnableDirectoryBrowsing = true,

    };
    app.UseFileServer(options);


    app.UseSession();


    app.UseMvc(routes =>
    {
        routes.MapAreaRoute("Admin", "Admin", "Admin/{controller}/{action}/{id?}");
        routes.MapAreaRoute("Blog", "Blog", "Blog/{controller}/{action}/{id?}");
        routes.MapAreaRoute("Photos", "Photos", "Photos/{action}/{id?}");
    });

    app.MapRazorPages();

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseAntiforgery();


    //app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

    /* app.MapAreaControllerRoute(
            name: "Admin",
            areaName: "Admin",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapAreaControllerRoute(
            name: "Photos",
            areaName: "Photos",
            pattern: "Photos/{controller=Photos}/{action=Index}/{id?}");


        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{id?}");*/


    app.MapGet("/hello", () => "Hello World");

    app.MapGet("/Captcha", (string userResponse) =>
    {
        return userResponse;
      
    });

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.RoutePrefix = string.Empty;
    });

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
