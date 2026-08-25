using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Api
{
     [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
     [ApiController]
     public class UsersController : ControllerBase
     {
          private readonly ApplicationDbContext _context;
          private UserManager<ApplicationUser> _userManager;
          private RoleManager<IdentityRole> _roleManager;
          private IConfiguration _configuration;
          public List<string> Errors = new List<string>();
          public IPasswordHasher<ApplicationUser> passwordHasher;
          private SignInManager<ApplicationUser> _signInManager;


          public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration, IPasswordHasher<ApplicationUser> passwordHash, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
          {
               _context = context;
               _userManager = userManager;
               _roleManager = roleManager;
               _configuration = configuration;
               passwordHasher = passwordHash;
               _signInManager = signInManager;
          }

          // GET: api/Users
          [HttpGet]
          public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
          {
               return await _context.Users.ToListAsync();
          }

          [HttpPost("/api/Users/Create")]
          public async Task<IActionResult> Create(User? user)
          {
               var userExists = _userManager.Users.Where(u => u.Email == user!.Email).Any();
               if (userExists)
               {
                    return Ok("User already exists in database.");
               }
               else
               {
                    ApplicationUser? appUser = new ApplicationUser
                    {
                         FirstName = user!.FirstName,
                         LastName = user.LastName,
                         Email = user.Email,
                         UserName = user.UserName,
                    };

                    IdentityResult result = await _userManager.CreateAsync(appUser, user.Password!);

                    if (result.Succeeded)
                         return Ok(appUser);
                    else
                    {
                         foreach (IdentityError error in result.Errors)
                         {
                              ModelState.AddModelError("", error.Description);
                              Errors.Add(error.Description);

                         }
                         return Ok(Errors);
                    }
               }
          }

          [HttpGet("/api/User/{id}")]
          public IActionResult GetUser(string id)
          {
               var user = _userManager.Users.Where(u => u.Id == id).FirstOrDefault();
               return Ok(user);
          }

          [HttpPut("/api/User/Update/{id}")]
          public async Task<IActionResult> UpdateUser([FromQuery(Name = "firstname")] string? firstname, [FromQuery(Name = "lastname")] string? lastname, [FromQuery(Name = "email")] string? email, [FromQuery(Name = "password")] string? password, string? id)
          {

               ApplicationUser user = _userManager.Users.Where(u => u.Id == id).FirstOrDefault()!;
               user.FirstName = firstname;
               user.LastName = lastname;
               user.Email = email;
               user.PasswordHash = passwordHasher.HashPassword(user, password!);
               IdentityResult result = await _userManager.UpdateAsync(user);
               return Ok(result);
          }

          [HttpDelete("/api/User/Delete/{id}")]
          public async Task<IActionResult> DeleteUser([FromQuery(Name = "id")] string id)
          {
               var user = _userManager.Users.Where(u => u.Id == id).FirstOrDefault();
               var role = await _userManager.RemoveFromRoleAsync(user!, "Member");
               await _userManager.DeleteAsync(user!);
               return Ok("User successfully deleted");
          }


          private async Task<string?> GenerateToken(string email, ApplicationUser user)
          {
               var secret = _configuration["JwtConfig:Secret"];
               var issuer = _configuration["JwtConfig:ValidIssuer"];
               var audience = _configuration["JwtConfig:ValidAudiences"];
               if (secret is null || issuer is null || audience is null)
               {
                    throw new ApplicationException("Jwt is not set in the configuration");
               }
               var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
               var tokenHandler = new JwtSecurityTokenHandler();

               //var userRoles = await _userManager.GetRolesAsync(user);
               var claims = new List<Claim>
               {
                    new(ClaimTypes.Name, email)
               };


               var tokenDescriptor = new SecurityTokenDescriptor
               {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(1),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
               };
               var securityToken = tokenHandler.CreateToken(tokenDescriptor);
               var token = tokenHandler.WriteToken(securityToken);
               await Task.CompletedTask;
               return token;


          }




          [HttpPost("/api/identity/login")]
          public async Task<IActionResult> Login(Login model)
          {


               if (ModelState.IsValid)
               {
                    var appUser = await _userManager.FindByEmailAsync(model.Email);
                    if (appUser != null)
                    {
                         await _signInManager.SignOutAsync();
                         Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(appUser, model.Password, false, false);
                         if (result.Succeeded)
                         {
                              var Id = appUser.Id;
                              var FirstName = appUser.FirstName;
                              var Email = appUser.Email;
                              var member = await _userManager.IsInRoleAsync(appUser, "Member");
                              var roles = member ? "Member" : null;
                              var token = await GenerateToken(appUser.Email!, appUser);
                              return Ok(new { Id, FirstName, Email, roles, token });
                         }
                         else
                         {
                              ModelState.AddModelError("", "Invalid email or password");
                         }

                    }

               }
               return BadRequest(ModelState);
          }

          [HttpPost("/api/identity/register")]
          public async Task<IActionResult> Register(User model)
          {


               if (ModelState.IsValid)
               {
                    ApplicationUser user = new ApplicationUser
                    {
                         UserName = model.UserName,
                         Email = model.Email,
                         FirstName = model.FirstName,
                         LastName = model.LastName,
                    };

                    IdentityResult result = await _userManager.CreateAsync(user, model.Password!);
                    var addToRole = await _userManager.AddToRoleAsync(user, "Member");
                    

                    if (result.Succeeded)
                    {
                         if (addToRole.Succeeded)
                         {
                              // Successfully added the user to the member role, do other stuff here

                              var token = GenerateToken(model.Email!, user);
                              var firstName = user.FirstName;
                              var email = user.Email;

                              var member = await _userManager.IsInRoleAsync(user, "Member");
                              var role = member ? "Member" : null;

                              return Ok(new { token, firstName, email, role });

                         }
                    }
                    else
                    {
                         foreach (var error in result.Errors)
                         {
                              ModelState.AddModelError("", error.Description);
                              Errors.Add(error.Description);
                         }
                    }

               }
               return BadRequest(ModelState);
          }

          public async Task<string> GetRole(ApplicationUser user)
          {
               var role = await _userManager.IsInRoleAsync(user, "Member");
               return "Member";
          }

          public async Task AddUserToRole(ApplicationUser user)
          {
               await _userManager.AddToRoleAsync(user, "Member");
          }

     }
}
