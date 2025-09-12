using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Models;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalWebsiteMVC.Areas.Admin.Controllers
{
	[Authorize(Roles="Admin")]
	[Area("Admin")]
	[Route("Admin")]
	public class AdminController : Controller
	{
		private UserManager<ApplicationUser> userManager;
		private SignInManager<ApplicationUser> signInManager;
		public AdminController(UserManager<ApplicationUser> userMgr, SignInManager<ApplicationUser> signinMgr)
		{
			userManager = userMgr;
			signInManager = signinMgr;
		}


		// GET: /<controller>/
		[Route("Admin")]
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		
	}
}
