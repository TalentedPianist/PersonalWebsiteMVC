using Elastic.Clients.Elasticsearch.Snapshot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Controllers
{
    public class UserController : Controller
    {
        public readonly IConfiguration _configuration;
        public readonly HttpClient _httpClient;
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;

        public UserController(IConfiguration configuration, HttpClient httpClient, UserManager<ApplicationUser> userMgr, SignInManager<ApplicationUser> signinMgr)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            userManager = userMgr;
            signInManager = signinMgr;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginUser(Login login)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? appUser = await userManager.FindByEmailAsync(login.Email!);
                if (appUser != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUser, login.Password!, false, false);
                    if (result.Succeeded)
                        return Redirect(login.ReturnUrl ?? "/");
                }
                ModelState.AddModelError(nameof(login.Email), "Login failed: Invalid Email or Password");

            }
            return View("~/Views/User/Login.cshtml", login);
        }

    }
}
