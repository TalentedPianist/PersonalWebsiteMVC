using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalWebsiteMVC.Data;
using PersonalWebsiteMVC.Models;

namespace PersonalWebsiteMVC.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<PaginatedList<Posts>>> GetPosts(int pageIndex = 1, int pageSize = 1)
        {
               var posts = _context.Posts.AsQueryable().AsNoTracking();
               var count = await posts.CountAsync();
               var items = await posts.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
               var route = HttpContext.Request.Path.Value;
               route = route!.Replace("/api/", "");
               var result = new PaginatedList<Posts>(items, count, pageIndex, pageSize, route);
               return Ok(result);
            
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Posts>> GetPosts(int id)
        {
            var posts = await _context.Posts.FindAsync(id);

            if (posts == null)
            {
                return NotFound();
            }

            return posts;
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("api/Posts/{id}")]
        public async Task<IActionResult> PutPosts(int id, [FromBody]Posts posts)
        {
               _context.Posts.Update(posts);
               await _context.SaveChangesAsync();
               return Ok(posts);
        }

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Posts>> PostPosts([FromBody]Posts posts)
        {
               posts.PostDate = DateTime.Now;
               posts.PostIP = HttpContext.Connection.RemoteIpAddress!.ToString();
            _context.Posts.Add(posts);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPosts", new { id = posts.id }, posts);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosts(int id)
        {
            var posts = await _context.Posts.FindAsync(id);
            if (posts == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(posts);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostsExists(int id)
        {
            return _context.Posts.Any(e => e.id == id);
        }

       
    }
}
