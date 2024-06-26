using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private IHttpContextAccessor _http;

        public AccountController(UserManager<ApplicationUser> userMgr, SignInManager<ApplicationUser> signinMgr, IHttpContextAccessor http)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            _http = http;
        }

    
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login();
            login.ReturnUrl = returnUrl;
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
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
                    else
                    {
                        Console.WriteLine(result.ToString());
                    }
                }
                //ModelState.AddModelError(nameof(login.Email), "Login Failed: Invalid Email or password");
            }
            return View(login);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AppLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Console.WriteLine("User is logged out.");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
