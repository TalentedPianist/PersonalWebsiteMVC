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
using KITT.Web.ReCaptcha.Http.v2;
using KITT.Web.ReCaptcha.Http.Internals;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Services;
using PersonalWebsiteMVC.Areas.Identity.Data;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews().AddNewtonsoftJson().AddSessionStateTempDataProvider();
builder.Services.AddRazorPages().AddNewtonsoftJson();
builder.Services.AddMvc(options =>
{
    options.MaxModelBindingCollectionSize = int.MaxValue;

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
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Accounts/Login";
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
    options.StylePath = "/lib/ckeditor/samples/css/samples.css";
});

builder.Services.AddReCaptchaV2HttpClient(options =>
{
    options.SecretKey = "6LdB-1kpAAAAAPEBR2GuCm8rTEucGiU89cQPMaC0";
});


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IReCaptchaFormClient, ReCaptchaFormClient>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

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

app.UseSession();

app.MapRazorPages();
app.MapControllers();



app.UseRouting();
app.UseAuthorization();


app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.UseAuthentication();

app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapPost("api/send", async (ReCaptchaService reCaptcha, [FromBody] ReCaptchaModel model) =>
{
    var result = await reCaptcha.VerifyAsync(model.CaptchaResponse);
    if (!result.Success)
    {
        return Results.BadRequest(result.ErrorCodes);
    }
    return Results.Ok();
});


app.Run();
