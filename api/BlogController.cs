using Microsoft.AspNetCore.Mvc;
using PersonalWebsiteMVC.Data;
using Newtonsoft.Json;
using PersonalWebsiteMVC.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PersonalWebsiteMVC.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private ApplicationDbContext _db { get; set; }
        public BlogController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/<BlogController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<Posts> Posts = new List<Posts>();
            foreach (var item in _db.Posts)
            {
                Posts.Add(item);
            }
            string json = JsonConvert.SerializeObject(Posts, Formatting.Indented);
            yield return json;
        }

        // GET api/<BlogController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<BlogController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<BlogController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BlogController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
