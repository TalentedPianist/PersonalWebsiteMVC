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
using MudBlazor.Services;
using GoogleCaptchaComponent;
using GoogleCaptchaComponent.Configuration;
using Blazorise;
using Blazorise.Captcha.ReCaptcha;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;



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
    builder.Services.AddSwaggerGen();

    builder.Services.AddRazorPages();


    builder.Services.AddCKEditor(builder.Configuration, options =>
    {
    });

    builder.Services.AddHttpContextAccessor();


    // builder.Services.AddHttpClient<IReCaptchaFormClient, ReCaptchaFormHttpClient>(client =>
    //     client.BaseAddress = new Uri("http://localhost:5051"));

    builder.Services
        .AddBlazorise(options =>
        {
            options.Immediate = true;
        })
        .AddBootstrap5Providers()
        .AddFontAwesomeIcons()
        .AddBlazoriseGoogleReCaptcha(reCaptchaOptions =>
        {
            reCaptchaOptions.SiteKey = "6LeCBlUrAAAAAGJFT1Rt-4hojR6NfEvqzsvZwnOz";
        });

    builder.Services.AddMudServices();

  

    var emailConfig = builder.Configuration
        .GetSection("MailSettings")
        .Get<MailSettings>();

    var app = builder.Build();

    // Code to seed data as seen in https://learn.microsoft.com/en-us/aspnet/core/blazor/tutorials/movie-database-app/part-4?view=aspnetcore-9.0&pivots=vs#seed-the-database
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        SeedData.Initialize(services);
    }


    app.UseStaticFiles();


    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();

    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
    }

    var galleryPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Gallery");
    if (!Directory.Exists(galleryPath))
    {
        Directory.CreateDirectory(galleryPath);
    }
    var fileProvider = new PhysicalFileProvider(galleryPath);
    var options = new FileServerOptions
    {
        FileProvider = fileProvider,
        RequestPath = "/Gallery",
        EnableDirectoryBrowsing = true,

    };
    app.UseFileServer(options);


    app.UseSession();




    app.UseRouting();

    app.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

    app.MapAreaControllerRoute(
        name: "Blog",
        areaName: "Posts",
        pattern: "Posts/{controller=Home}/{action=Index}/{id?}");

    app.MapAreaControllerRoute(
        name: "Photos",
        areaName: "Photos",
        pattern: "Photos/{controller=Home}/{action=Index}/{id?}");

    app.MapControllers();
    app.MapRazorPages();
    app.MapBlazorHub();

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseAntiforgery();


    app.MapGet("/hello", () => "Hello World");

    app.MapGet("/Captcha", (string userResponse) =>
    {
        return userResponse;

    });



    // app.MapRazorComponents<App>()
    //     .AddInteractiveServerRenderMode();
    //app.MapFallbackToPage("/");



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
