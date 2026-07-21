using System;
using System.Collections.Generic;
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
using ServiceStack;

namespace PersonalWebsiteMVC.Api
{
     [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
     [ApiController]
     public class UsersController : ControllerBase
     {
          private readonly ApplicationDbContext _context;
          private UserManager<ApplicationUser> _userManager;
          private IConfiguration _configuration;
          public List<string> Errors = new List<string>();
          public IPasswordHasher<ApplicationUser> passwordHasher; 

          public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration, IPasswordHasher<ApplicationUser> passwordHash)
          {
               _context = context;
               _userManager = userManager;
               _configuration = configuration;
               passwordHasher = passwordHash;
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
          public async Task<IActionResult> UpdateUser([FromQuery(Name="firstname")] string? firstname, [FromQuery(Name="lastname")] string? lastname, [FromQuery(Name="email")]string? email, [FromQuery(Name="password")]string? password, string? id)
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
          public async Task<IActionResult> DeleteUser([FromQuery(Name="id")]string id)
          {
               var user = _userManager.Users.Where(u => u.Id == id).FirstOrDefault();
               await _userManager.DeleteAsync(user!);
               return Ok("User successfully deleted");
          }


          private string? GenerateToken(string email)
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
               var tokenDescriptor = new SecurityTokenDescriptor
               {
                    Subject = new ClaimsIdentity(new[]
                    {
                         new Claim(ClaimTypes.Email, email)
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
               };
               var securityToken = tokenHandler.CreateToken(tokenDescriptor);
               var token = tokenHandler.WriteToken(securityToken);

               return token;


          }

          [HttpPost("/api/identity/login")]
          public async Task<IActionResult> Login([FromBody] Login model)
          {
               // Get the secret in the configuration

               // Check if the model is valid
               if (ModelState.IsValid)
               {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                         if (await _userManager.CheckPasswordAsync(user, model.Password))
                         {
                              var token = GenerateToken(model.Email);

                              return Ok(new { token, user.Id, user.FirstName, user.Email });
                         }

                    }
                    ModelState.AddModelError("", "Invalid email or password");
               }
               return BadRequest(ModelState);

          }

          [HttpPost("/api/identity/register")]
          public async Task<IActionResult> Register([FromBody] User model)
          {
               // Check if the model is valid
               if (ModelState.IsValid)
               {
                    var existedUser = await _userManager.FindByEmailAsync(model.Email);
                    if (existedUser != null)
                    {
                         ModelState.AddModelError("", "Email address already taken");
                         return BadRequest(ModelState);
                    }
                    // Create a new user object
                    var user = new ApplicationUser()
                    {
                         FirstName = model.FirstName,
                         LastName = model.LastName,
                         Email = model.Email,
                         UserName = model.UserName,
                         SecurityStamp = Guid.NewGuid().ToString()
                    };
                    // Try to save the user
                    var result = await _userManager.CreateAsync(user, model.Password);
                    // If the user is successfully created, return OK
                    if (result.Succeeded)
                    {
                         var token = GenerateToken(model.Email);
                         return Ok(new { token });
                    }
                    // If there are any errors, add them to the ModelState object and return the error to the client
                    foreach (var error in result.Errors)
                    {
                         ModelState.AddModelError("", error.Description);
                    }

               }
               // If we get this far something failed, redisplay form
               return BadRequest(ModelState);

          }

     }
}
