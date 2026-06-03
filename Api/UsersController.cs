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

namespace PersonalWebsiteMVC.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
          private UserManager<ApplicationUser> _userManager;
          private IConfiguration _configuration;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
               _userManager = userManager;
               _configuration = configuration;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUser(string id)
        {
            var applicationUser = await _context.Users.FindAsync(id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            return applicationUser;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(applicationUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
     
        public async Task<ActionResult<ApplicationUser>> PostApplicationUser(ApplicationUser applicationUser)
        {
            _context.Users.Add(applicationUser);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(applicationUser.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = applicationUser.Id }, applicationUser);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var applicationUser = await _context.Users.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            _context.Users.Remove(applicationUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
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

          [HttpPost("/identity/login")]
          public async Task<IActionResult> Login([FromBody]Login model)
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
                                   return Ok(new { token });
                              }

                         }
                         ModelState.AddModelError("", "Invalid email or password");
                    }
                    return BadRequest(ModelState);
              
          }
          
    }
}
